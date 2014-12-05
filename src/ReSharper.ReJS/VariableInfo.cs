using JetBrains.ReSharper.Psi;
#if RESHARPER9
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.JavaScript.Parsing;
#endif
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.ReJS
{
    internal class VariableInfo
    {
        public VariableInfo(IReferenceExpression reference)
        {
            Node = reference;
            IsWriteUsage = reference.Parent is IPrefixExpression ||
                           reference.Parent is IPostfixExpression ||
                           IsAssignment(reference);
            FunctionLike = reference.GetContainingNode<IJsFunctionLike>();
            DeclaredElement = reference.Reference.Resolve().DeclaredElement;
        }

        public VariableInfo(IVariableDeclaration declaration)
        {
            Node = declaration;
            IsWriteUsage = true;
            FunctionLike = declaration.GetContainingNode<IJsFunctionLike>();
            DeclaredElement = declaration.DeclaredElement;
        }

        public bool IsWriteUsage { get; private set; }
        public IJsFunctionLike FunctionLike { get; private set; }
        public IJavaScriptTreeNode Node { get; private set; }
        public IDeclaredElement DeclaredElement { get; private set; }

        private static bool IsAssignment(ITreeNode referenceExpression)
        {
            var binaryexpression = referenceExpression.Parent as IBinaryExpression;
            return binaryexpression != null &&
                   binaryexpression.LeftOperand == referenceExpression &&
                   IsAssignmentImpl(binaryexpression);
        }

#if !RESHARPER9
        private static bool IsAssignmentImpl(IBinaryExpression binaryexpression)
        {
            return binaryexpression.IsAssignment;
        }
#else
        private static bool IsAssignmentImpl(ITreeNode binaryexpression)
        {
            var treeElement = binaryexpression.FirstChild;
            if (treeElement == null)
                return false;

            for (; treeElement != null; treeElement = treeElement.NextSibling)
            {
                var javaScriptTokenBase = treeElement as JavaScriptTokenBase;
                if (javaScriptTokenBase != null)
                {
                    NodeType nodeType = javaScriptTokenBase.NodeType;
                    if (nodeType == JavaScriptTokenType.EQ ||
                        nodeType == JavaScriptTokenType.PLUSEQ ||
                        nodeType == JavaScriptTokenType.MINUSEQ ||
                        nodeType == JavaScriptTokenType.TIMESEQ ||
                        nodeType == JavaScriptTokenType.PERCENTEQ ||
                        nodeType == JavaScriptTokenType.LSHIFTEQ ||
                        nodeType == JavaScriptTokenType.RSHIFTEQ ||
                        nodeType == JavaScriptTokenType.GT3EQ ||
                        nodeType == JavaScriptTokenType.AMPEREQ ||
                        nodeType == JavaScriptTokenType.PIPEEQ ||
                        nodeType == JavaScriptTokenType.CAROTEQ ||
                        nodeType == JavaScriptTokenType.DIVIDEEQ)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
#endif
    }
}