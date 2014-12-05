#if !RESHARPER9
using JetBrains.ReSharper.Intentions.JavaScript.Tests.QuickFixesTests;
#else
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
#endif
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