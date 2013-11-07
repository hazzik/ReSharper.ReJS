using JetBrains.ReSharper.Psi;
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
                   binaryexpression.IsAssignment;
        }
    }
}
