using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.ReJS
{
    internal class ReferenceExpressionCollector : IRecursiveElementProcessor
    {
        private readonly ICollection<IReferenceExpression> references = new JetHashSet<IReferenceExpression>();
  
        public bool InteriorShouldBeProcessed(ITreeNode element)
        {
            return true;
        }

        public void ProcessBeforeInterior(ITreeNode element)
        {
        }

        public void ProcessAfterInterior(ITreeNode element)
        {
            var referenceExpression = element as IReferenceExpression;
            if (referenceExpression == null)
                return;

            References.Add(referenceExpression);
        }

        public bool ProcessingIsFinished { get; private set; }

        public ICollection<IReferenceExpression> References
        {
            get { return references; }
        }
    }
}