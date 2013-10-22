using System;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.JavaScript.Services;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace ReSharper.ReJS
{
    [QuickFix]
    public sealed class CallWithSameContextWarningFix : QuickFixBase
    {
        private readonly IInvocationExpression _invocationExpression;

        public CallWithSameContextWarningFix(CallWithSameContextWarning warning)
        {
            _invocationExpression = warning.InvocationExpression;
        }

        public override string Text
        {
            get { return "Replace with direct function call"; }
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var factory = JavaScriptElementFactory.GetInstance(_invocationExpression);
            var arguments = Enumerable.Range(1, _invocationExpression.Arguments.Count - 1).Select(n => "$" + n);
            var expression = string.Format("$0({0})", string.Join(", ", arguments));
            var args = _invocationExpression.Arguments.Skip(1)
                .Prepend(((IReferenceExpression) _invocationExpression.InvokedExpression).Qualifier)
                .Cast<object>()
                .ToArray();

            _invocationExpression.ReplaceBy(factory.CreateExpression(expression, args));
            return null;
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            return _invocationExpression.IsValid();
        }
    }
}
