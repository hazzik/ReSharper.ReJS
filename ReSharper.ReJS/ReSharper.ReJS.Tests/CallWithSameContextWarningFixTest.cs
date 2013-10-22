using JetBrains.ReSharper.Intentions.JavaScript.Tests.QuickFixesTests;
using NUnit.Framework;

namespace ReSharper.ReJS.Tests
{
    [TestFixture]
    public class CallWithSameContextWarningFixTestTest : JavaScriptQuickFixTestBase<CallWithSameContextWarningFix>
    {
        protected override string RelativeTestDataPath
        {
            get { return ""; }
        }

        [TestCase("execute01.js")]
        public void Test(string file)
        {
            DoTestFiles(file);
        }
    }
}
