using NUnit.Framework;
#if !RESHARPER9
using JetBrains.ReSharper.Intentions.JavaScript.Tests.ContextActions;
#else
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
#endif

namespace ReSharper.ReJS.Tests
{
    [TestFixture]
    public class ReplaceIndexWithReferenceActionTest : JavaScriptContextActionExecuteTestBase<ReplaceIndexWithReferenceAction>
    {
        protected override string ExtraPath
        {
            get { return "ReplaceIndexWithReferenceActionTest"; }
        }

        protected override string RelativeTestDataPath
        {
            get { return ExtraPath; }
        }

        [TestCase("execute01")]
        public void Test(string file)
        {
            DoOneTest(file);
        }
    }
}