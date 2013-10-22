using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.JavaScript.Impl;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.JavaScript.Tree;

namespace ReSharper.ReJS
{
    public class ReJsHighlightingStageProcess : JavaScriptDaemonStageProcessBase
    {
        public ReJsHighlightingStageProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore, IJavaScriptFile file)
            : base(process, settingsStore, file)
        {
        }

        public override void Execute(Action<DaemonStageResult> commiter)
        {
            HighlightInFile((file, consumer) => file.ProcessDescendants(this, consumer), commiter);
        }

        public override void VisitInvocationExpression(IInvocationExpression invocation, IHighlightingConsumer consumer)
        {
            if (IsCallWithTheSameContextAsFunctionOwner(invocation))
                consumer.AddHighlighting(new CallWithSameContextWarning(invocation), invocation.GetHighlightingRange(), File);

            base.VisitInvocationExpression(invocation, consumer);
        }

        private static bool IsCallWithTheSameContextAsFunctionOwner(IInvocationExpression invocation)
        {
            var invokedReference = invocation.InvokedExpression as IReferenceExpression;
            if (invokedReference == null || invokedReference.Name != "call")
                return false;

            var function = invokedReference.Qualifier as IReferenceExpression;
            if (function == null)
                return false;

            var context = function.Qualifier as IReferenceExpression;
            if (context == null)
                return false;

            var contextElement = context.Reference.Resolve().DeclaredElement;

            var suppliedContext = invocation.Arguments.FirstOrDefault() as IReferenceExpression;
            if (suppliedContext == null)
                return false;

            var suppliedContextElement = suppliedContext.Reference.Resolve().DeclaredElement;

            return Equals(suppliedContextElement, contextElement);
        }
    }
}
