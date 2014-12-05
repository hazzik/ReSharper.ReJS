using System;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.JavaScript.Stages;
using JetBrains.ReSharper.Feature.Services.Bulbs;
#if !RESHARPER9
using JetBrains.ReSharper.Intentions.Extensibility;
#else
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Resources.Shell;
#endif
using JetBrains.ReSharper.Intentions.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.TextControl;
using JetBrains.Util;

namespace ReSharper.ReJS
{
    [QuickFix]
    public class RemoveUnreachableCodeFix : QuickFixBase
    {
        [CanBeNull]
        private readonly ITreeRange _range;

        private bool _executing;

        public RemoveUnreachableCodeFix(UnreachableCodeWarning error)
        {
            _range = error.TreeRange;
        }

        public RemoveUnreachableCodeFix(HeuristicallyUnreachableCodeWarning error)
        {
            _range = error.TreeRange;
        }

        public override string Text
        {
            get { return "Remove unreachable code"; }
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            if (_range == null || !ValidUtils.Valid(_range.First) || !ValidUtils.Valid(_range.Last))
                return false;
            var treeRange = AdjustTreeRange(_range);
            if (treeRange != null)
                return treeRange.First.Parent == treeRange.Last.Parent;
            return false;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            _executing = true;
            var treeRange = AdjustTreeRange(_range);
            if (treeRange == null)
                return null;
            var first = treeRange.First;
            var last = treeRange.Last;
            for (var prevSibling = first.PrevSibling; prevSibling is IWhitespaceNode && !((IWhitespaceNode) prevSibling).IsNewLine; prevSibling = prevSibling.PrevSibling)
                first = prevSibling;
            for (var nextSibling = last.NextSibling; nextSibling is IWhitespaceNode; nextSibling = nextSibling.NextSibling)
            {
                last = nextSibling;
                if (((IWhitespaceNode) last).IsNewLine)
                    break;
            }
            var containingNode = first.GetContainingNode<IJavaScriptStatement>(true);
            using (WriteLockCookie.Create())
                ModificationUtil.DeleteChildRange(new TreeRange(first, last));
            _executing = false;
            if (containingNode != null && containingNode.IsValid())
            {
                var containingFile = containingNode.GetContainingFile();
                if (containingFile != null)
                    containingFile.ReParse(containingNode.GetTreeTextRange(), containingNode.GetText());
            }
            return null;
        }

        private ITreeRange AdjustTreeRange(ITreeRange range)
        {
            if (range.First == range.Last)
            {
                var statement = range.First as IJavaScriptStatement;
                if (statement != null)
                    return AdjustStatement(range, statement);
                var expression = range.First as IJavaScriptExpression;
                if (expression != null)
                    return AdjustExpression(range, expression);
                var compoundExpression = range.First as ICompoundExpression;
                if (compoundExpression != null)
                    return AdjustExpression(range, compoundExpression);
            }
            return range;
        }

        private ITreeRange AdjustExpression(ITreeRange range, IJavaScriptExpression expression)
        {
            var binaryExpressionByLeftOperand = BinaryExpressionNavigator.GetByLeftOperand(expression);
            if (binaryExpressionByLeftOperand != null && binaryExpressionByLeftOperand.Sign != null)
                return new TreeRange(expression, binaryExpressionByLeftOperand.Sign);

            var binaryExpressionByRightOperand = BinaryExpressionNavigator.GetByRightOperand(expression);
            if (binaryExpressionByRightOperand != null && binaryExpressionByRightOperand.Sign != null)
                return new TreeRange(binaryExpressionByRightOperand.Sign, expression);

            var conditionalTernaryExpressionByConditionOperand = ConditionalTernaryExpressionNavigator.GetByConditionOperand(expression);
            if (conditionalTernaryExpressionByConditionOperand != null)
                return AdjustExpression(new TreeRange(conditionalTernaryExpressionByConditionOperand, conditionalTernaryExpressionByConditionOperand), conditionalTernaryExpressionByConditionOperand);
            
            var conditionalTernaryExpressionByThenResult = ConditionalTernaryExpressionNavigator.GetByThenResult(expression);
            if (conditionalTernaryExpressionByThenResult != null)
            {
                if (conditionalTernaryExpressionByThenResult.ElseResult == null)
                    return AdjustExpression(new TreeRange(conditionalTernaryExpressionByThenResult, conditionalTernaryExpressionByThenResult), conditionalTernaryExpressionByThenResult);
                if (!_executing)
                    return range;
                conditionalTernaryExpressionByThenResult.ReplaceBy(conditionalTernaryExpressionByThenResult.ElseResult);
                return null;
            }

            var conditionalTernaryExpressionByElseResult = ConditionalTernaryExpressionNavigator.GetByElseResult(expression);
            if (conditionalTernaryExpressionByElseResult != null)
            {
                if (!_executing)
                    return range;
                conditionalTernaryExpressionByElseResult.ReplaceBy(conditionalTernaryExpressionByElseResult.ThenResult);
                return null;
            }
            
            var prefixExpression = PrefixExpressionNavigator.GetByOperand(expression);
            if (prefixExpression != null)
                return AdjustExpression(new TreeRange(prefixExpression, prefixExpression), prefixExpression);
            
            var postfixExpression = PostfixExpressionNavigator.GetByOperand(expression);
            if (postfixExpression != null)
                return AdjustExpression(new TreeRange(postfixExpression, postfixExpression), postfixExpression);
            return range;
        }

        private ITreeRange AdjustExpression(ITreeRange range, ICompoundExpression expression)
        {
            var expressionStatement = ExpressionStatementNavigator.GetByExpression(expression);
            if (expressionStatement != null)
                return AdjustStatement(new TreeRange(expressionStatement, expressionStatement), expressionStatement);
            
            var ifStatement = IfStatementNavigator.GetByCondition(expression);
            if (ifStatement != null)
            {
                if (ifStatement.Else == null)
                {
                    return new TreeRange(ifStatement, ifStatement);
                }
                if (_executing)
                {
                    var block = ifStatement.Else as IBlock;
                    if (block == null)
                    {
                        ifStatement.ReplaceBy(ifStatement.Else);
                    }
                    else
                    {
                        //StatementUtil.ReplaceStatementWithBlock(block, ifStatementByCondition);
                    }
                }
                return null;
            }

            var switchStatement = SwitchStatementNavigator.GetByCondition(expression);
            if (switchStatement != null)
                return new TreeRange(switchStatement, switchStatement);
            
            var whileStatement = WhileStatementNavigator.GetByCondition(expression);
            if (whileStatement != null)
                return new TreeRange(whileStatement, whileStatement);

            var doStatement = DoStatementNavigator.GetByCondition(expression);
            if (doStatement != null)
            {
                if (!_executing)
                    return range;
                var block = doStatement.Statement as IBlock;
                if (block != null && block.Statements.Count == 1)
                {
                    doStatement.ReplaceBy(block.Statements[0]);
                }
                else
                {
                    doStatement.ReplaceBy(doStatement.Statement);
                }
                return null;
            }

            var forStatement = ForStatementNavigator.GetByForCondition(expression);
            if (forStatement != null)
                return new TreeRange(forStatement, forStatement);

            return range;
        }

        private ITreeRange AdjustStatement(ITreeRange range, IJavaScriptStatement statement)
        {
            var ifStatementByThen = IfStatementNavigator.GetByThen(statement);
            if (ifStatementByThen != null)
            {
                if (ifStatementByThen.Else == null)
                    return new TreeRange(ifStatementByThen, ifStatementByThen);
                if (!_executing)
                    return range;
                ifStatementByThen.ReplaceBy(ifStatementByThen.Else);
                return null;
            }
            
            var ifStatementByElse = IfStatementNavigator.GetByElse(statement);
            if (ifStatementByElse != null)
            {
                if (ifStatementByElse.Then == null)
                    return new TreeRange(ifStatementByElse, ifStatementByElse);
                if (!_executing)
                    return range;
                ifStatementByElse.SetElse(null);
                return null;
            }
            
            var doStatement = DoStatementNavigator.GetByStatement(statement);
            if (doStatement != null)
                return new TreeRange(doStatement, doStatement);
            
            var whileStatement = WhileStatementNavigator.GetByStatement(statement);
            if (whileStatement != null)
                return new TreeRange(whileStatement, whileStatement);
            
            var forStatement = ForStatementNavigator.GetByStatement(statement);
            if (forStatement != null)
                return new TreeRange(forStatement, forStatement);
            
            var foreachStatement = ForeachStatementNavigator.GetByStatement(statement);
            if (foreachStatement != null)
                return new TreeRange(foreachStatement, foreachStatement);
            
            var withStatement = WithStatementNavigator.GetByStatement(statement);
            if (withStatement != null)
                return new TreeRange(withStatement, withStatement);
            
            return range;
        }
    }
}
