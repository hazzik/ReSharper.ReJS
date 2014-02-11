using JetBrains.ReSharper.Intentions.JavaScript.Tests.ContextActions;
using NUnit.Framework;

namespace ReSharper.ReJS.Tests
{
    [TestFixture]
    public class ReplaceIndexWithReferenceActionTest : JavaScriptContextActionExecuteTestBase<ReplaceIndexWithReferenceAction>
    {
        [TestCase("execute01")]
        public void Test(string file)
        {
            DoOneTest(file);
        }

        protected override string ExtraPath
        {
            get { return "ReplaceIndexWithReferenceActionTest"; }
        }

        protected override string RelativeTestDataPath
        {
            get { return ExtraPath; }
        }
    }
}