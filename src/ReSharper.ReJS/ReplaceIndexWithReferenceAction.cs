using System;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.JavaScript.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.JavaScript.Services;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace ReSharper.ReJS
{
    [ContextAction(Name = "ReplaceIndexWithReference", Description = "Replaces index expression with reference expression", Group = "JavaScript")]
    public class ReplaceIndexWithReferenceAction : ContextActionBase
    {
        private readonly IJavaScriptContextActionDataProvider _provider;
        private IIndexExpression _indexExpression;
        private string _replacement;

        public ReplaceIndexWithReferenceAction(IJavaScriptContextActionDataProvider provider)
        {
            _provider = provider;
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            var index = _provider.GetSelectedElement<IIndexExpression>(true, true);
            if (index != null && index.IsValid() && index.AccessedPropertyName != null)
            {
                _indexExpression = index;
                _replacement = string.Format("{0}.{1}", index.IndexedExpression.GetText(), index.AccessedPropertyName);
                return true;
            }
            return false;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var factory = JavaScriptElementFactory.GetInstance(_indexExpression);
            using (WriteLockCookie.Create())
                ModificationUtil.ReplaceChild(_indexExpression, factory.CreateExpression(_replacement));
            return null;
        }

        public override string Text
        {
            get { return "Replace with " + _replacement; }
        }
    }
}