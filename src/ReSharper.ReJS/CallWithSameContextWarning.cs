using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.JavaScript.Tree;

namespace ReSharper.ReJS
{
    [StaticSeverityHighlighting(Severity.WARNING, HighlightingGroupIds.CodeRedundancy)]
    public class CallWithSameContextWarning : IHighlighting
    {
        private readonly IInvocationExpression _invocationExpression;

        public CallWithSameContextWarning(IInvocationExpression invocationExpression)
        {
            _invocationExpression = invocationExpression;
        }

        public bool IsValid()
        {
            return InvocationExpression != null && InvocationExpression.IsValid();
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
