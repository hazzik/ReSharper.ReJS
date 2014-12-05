#if !RESHARPER9
using JetBrains.ReSharper.Intentions.JavaScript.Tests.ContextActions;
#else
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
#endif
using NUnit.Framework;

namespace ReSharper.ReJS.Tests
{
    [TestFixture]
    public class ReplaceReferenceWithIndexActionTest : JavaScriptContextActionExecuteTestBase<ReplaceReferenceWithIndexAction>
    {
        [TestCase("execute01")]
        public void Test(string file)
        {
            DoOneTest(file);
        }

        protected override string ExtraPath
        {
            get { return "ReplaceReferenceWithIndexActionTest"; }
        }

        protected override string RelativeTestDataPath
        {
            get { return ExtraPath; }
        }
    }
}