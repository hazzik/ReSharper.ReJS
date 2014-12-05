using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;

public class ReSharperTestEnvironmentAssembly : TestEnvironmentAssembly<TestEnvironmentZone>
{
}
[ZoneDefinition]
public class TestEnvironmentZone : ITestsZone
{
}