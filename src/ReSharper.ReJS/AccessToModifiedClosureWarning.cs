using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.JavaScript.LanguageImpl;
using JetBrains.ReSharper.Psi.JavaScript.Tree;

namespace ReSharper.ReJS
{
    [ConfigurableSeverityHighlighting(HIGHLIGHTING_ID, JavaScriptLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING)]
    public class AccessToModifiedClosureWarning : IHighlighting
    {
        //TODO: Can I reuse CSharp's HIGHLIGHTING_ID?
        public const string HIGHLIGHTING_ID = "JsAccessToModifiedClosure";

        private readonly IReferenceExpression _referenceExpression;

        public AccessToModifiedClosureWarning(IReferenceExpression referenceExpression)
        {
            _referenceExpression = referenceExpression;
        }

        public IReferenceExpression ReferenceExpression
        {
            get { return _referenceExpression; }
        }

        public bool IsValid()
        {
            return ReferenceExpression != null && ReferenceExpression.IsValid();
        }

        public string ToolTip
        {
            get { return "Access to externally modified closure"; }
        }

        public string ErrorStripeToolTip
        {
            get { return ToolTip; }
        }

        public int NavigationOffsetPatch
        {
            get { return 0; }
        }
    }
}