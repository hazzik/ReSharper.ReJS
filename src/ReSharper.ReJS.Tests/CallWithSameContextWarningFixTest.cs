using JetBrains.ReSharper.Intentions.JavaScript.Tests.QuickFixesTests;
using NUnit.Framework;

namespace ReSharper.ReJS.Tests
{
    [TestFixture]
    public class CallWithSameContextWarningFixTest : JavaScriptQuickFixTestBase<CallWithSameContextWarningFix>
    {
        [TestCase("execute01.js")]
        [TestCase("execute02.js")]
        public void Test(string file)
        {
            DoTestFiles(file);
        }
    }
}
