using System.Reflection;
using System.Runtime.InteropServices;

using Microsoft.Owin;

[assembly: AssemblyTitle("Application")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Application")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("f6b140b7-d32a-4f4a-b2bd-624aeb1adbb8")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: OwinStartup(typeof(CodeKinden.OrangeCMS.Application.Startup))]
