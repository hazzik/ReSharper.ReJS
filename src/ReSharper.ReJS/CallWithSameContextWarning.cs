using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.JavaScript.LanguageImpl;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.ReJS
{
    [ConfigurableSeverityHighlighting(HIGHLIGHTING_ID, JavaScriptLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING)]
    public class CallWithSameContextWarning : IHighlighting
    {
        private readonly IInvocationExpression _invocationExpression;
        public const string HIGHLIGHTING_ID = "CallWithTheSameContext";

        public CallWithSameContextWarning(IInvocationExpression invocationExpression)
        {
            _invocationExpression = invocationExpression;
        }

        public bool IsValid()
        {
            return InvocationExpression != null && InvocationExpression.IsValid();
        }

        public DocumentRange CalculateRange()
        {
            return _invocationExpression.GetDocumentRange();
        }

        public string ToolTip
        {
            get { return "Call of a function with the same context"; }
        }

        public string ErrorStripeToolTip
        {
            get { return ToolTip; }
        }

        public int NavigationOffsetPatch
        {
            get { return 0; }
        }

        public IInvocationExpression InvocationExpression
        {
            get { return _invocationExpression; }
        }
    }
}
