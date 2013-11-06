using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.JavaScript.Impl;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Feature.Services.CSharp;
using JetBrains.ReSharper.InplaceRefactorings;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ControlFlow.Impl;
using JetBrains.ReSharper.Psi.JavaScript.ControlFlow;
using JetBrains.ReSharper.Psi.JavaScript.Impl.ControlFlow.Inspections.ValueAnalysis;
using JetBrains.ReSharper.Psi.JavaScript.Impl.Resolve;
using JetBrains.ReSharper.Psi.JavaScript.Services;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.ReJS
{
    public class ReJsHighlightingStageProcess : JavaScriptDaemonStageProcessBase
    {
        private readonly JavaScriptServices _services;
        private readonly JavaScriptResolveContext _context;

        public ReJsHighlightingStageProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore, IJavaScriptFile file)
            : base(process, settingsStore, file)
        {
            _services = DaemonProcess.Solution.GetComponent<JavaScriptServices>();
            _context = JavaScriptResolveContext.CreateInitialContext(_services, File.GetSourceFile());
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

        public override void VisitAccessorBody(IAccessorBody accessorBody, IHighlightingConsumer consumer)
        {
            VisitFunctionLike(accessorBody, consumer);
        }

        public override void VisitFunctionExpression(IFunctionExpression function, IHighlightingConsumer consumer)
        {
            VisitFunctionLike(function, consumer);
        }

        private void VisitFunctionLike(ITreeNode function, IHighlightingConsumer consumer)
        {
            var accessAnalizer = new ReferenceExpressionCollector();
            function.ProcessThisAndDescendants(accessAnalizer);
            var accessToExternalModifiedClosure = accessAnalizer.References
                .GroupBy(r => r.Reference.Resolve().DeclaredElement)
                .Select(g => g.Select(r => new ReferenceInfo(r)).ToArray())
                .Where(l => l.Length > 1)
                .Where(l => l.Any(r => r.FunctionLike != function) && l.Any(r => r.FunctionLike == function && r.IsWriteUsage))
                .SelectMany(l => l)
                .Where(r => r.FunctionLike != function)
                .Select(r => r.ReferenceExpression);

            foreach (var referenceExpression in accessToExternalModifiedClosure)
            {
                consumer.AddHighlighting(new AccessToModifiedClosureWarning(referenceExpression), referenceExpression.GetHighlightingRange(), File);
            }
        }


        private static bool IsCallWithTheSameContextAsFunctionOwner(IInvocationExpression invocation)
        {
            var invokedReference = invocation.InvokedExpression as IReferenceExpression;
            if (invokedReference == null || invokedReference.Name != "call")
                return false;

            var function = invokedReference.Qualifier as IReferenceExpression;
            if (function == null)
                return false;

            return AreSame(function.Qualifier, invocation.Arguments.FirstOrDefault());
        }

        private static bool AreSame(ITreeNode x, ITreeNode y)
        {
            var referenceX = x as IReferenceExpression;
            var referenceY = y as IReferenceExpression;
            if (referenceX != null && referenceY != null)
            {
                var resolvedReferenceX = referenceX.Reference.Resolve().DeclaredElement;
                var resolvedReferenceY = referenceY.Reference.Resolve().DeclaredElement;

                return Equals(resolvedReferenceY, resolvedReferenceX);
            }

            var thisX = x as IThisExpression;
            var thisY = y as IThisExpression;
            if (thisX != null && thisY != null)
            {
                //TODO: add proper comparision
                return true;
            }

            return false;
        }
    }
}
