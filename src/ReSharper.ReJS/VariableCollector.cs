using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.ReJS
{
    internal class VariableCollector : IRecursiveElementProcessor
    {
        private readonly ICollection<VariableInfo> _variables = new JetHashSet<VariableInfo>();

        public IEnumerable<VariableInfo> Variables
        {
            get { return _variables; }
        }

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
            if (referenceExpression != null)
            {
                _variables.Add(new VariableInfo(referenceExpression));
            }
            var declaration = element as IVariableDeclaration;
            if (declaration != null)
            {
                _variables.Add(new VariableInfo(declaration));
            }
        }

        public bool ProcessingIsFinished { get; private set; }
    }
}
