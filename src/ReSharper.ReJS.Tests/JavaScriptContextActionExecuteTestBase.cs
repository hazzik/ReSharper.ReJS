using System;
using JetBrains.ReSharper.Feature.Services.JavaScript.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.JavaScript.Tests.ContextActions;

namespace ReSharper.ReJS.Tests
{
    public abstract class JavaScriptContextActionExecuteTestBase<T> : JavaScriptContextActionExecuteTestBase where T:IContextAction
    {
        protected override IContextAction CreateContextAction(IJavaScriptContextActionDataProvider provider)
        {
            return (IContextAction) Activator.CreateInstance(typeof (T), provider);
        }

        protected override string RelativeTestDataPath
        {
            get { return ExtraPath; }
        }
    }
}