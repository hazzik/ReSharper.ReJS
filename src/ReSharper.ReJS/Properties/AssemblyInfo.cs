using System.Reflection;
using JetBrains.ActionManagement;
using JetBrains.Application.PluginSupport;
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
using JetBrains.ReSharper.Daemon;
using ReSharper.ReJS;

[assembly: AssemblyTitle("ReSharper.ReJS")]
[assembly: AssemblyDescription("Useful refactorings for JavaScript")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Alexander Zaytsev")]
[assembly: AssemblyProduct("ReSharper.ReJS")]
[assembly: AssemblyCopyright("Copyright © Alexander Zaytsev, 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("0.3.2.0")]
[assembly: AssemblyFileVersion("0.3.2.0")]

[assembly: ActionsXml("ReSharper.ReJS.Actions.xml")]

// The following information is displayed by ReSharper in the Plugins dialog
[assembly: PluginTitle("ReSharper.reJS")]
[assembly: PluginDescription("Useful refactorings for JavaScript")]
[assembly: PluginVendor("Alexander Zaytsev")]

[assembly: RegisterConfigurableSeverity(AccessToModifiedClosureWarning.HIGHLIGHTING_ID, null, HighlightingGroupIds.CodeSmell, "Access to modified closure", "\n          Access to closure variable from anonymous function when the variable is modified externally\n        ", Severity.WARNING, false)]
[assembly: RegisterConfigurableSeverity(CallWithSameContextWarning.HIGHLIGHTING_ID, null, HighlightingGroupIds.CodeRedundancy, "Call of a function with the same context", "Call of a function with the same context", Severity.WARNING, false)]

