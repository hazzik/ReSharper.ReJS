using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.ReJS
{
    internal class ReferenceInfo
    {
        public ReferenceInfo(IReferenceExpression referenceExpression)
        {
            ReferenceExpression = referenceExpression;
            IsWriteUsage = referenceExpression.Parent is IPrefixExpression ||
                           referenceExpression.Parent is IPostfixExpression ||
                           IsAssignment(referenceExpression);
            FunctionLike = referenceExpression.GetContainingNode<IJsFunctionLike>();
        }
        private static bool IsAssignment(ITreeNode referenceExpression)
        {
            var binaryexpression = referenceExpression.Parent as IBinaryExpression;
            return binaryexpression != null &&
                   binaryexpression.LeftOperand == referenceExpression &&
                   binaryexpression.IsAssignment;
        }

        public bool IsWriteUsage { get; set; }
        public IJsFunctionLike FunctionLike { get; set; }
        public IReferenceExpression ReferenceExpression { get; set; }
    }
}