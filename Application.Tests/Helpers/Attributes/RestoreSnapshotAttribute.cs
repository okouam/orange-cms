using System;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes
{
    internal class RestoreSnapshotAttribute : Attribute, ITestAction
    {
        public ActionTargets Targets => ActionTargets.Default;

        public void BeforeTest(TestDetails testDetails)
        {
            ApplicationDatabase.RollbackToSnapshot();
        }

        public void AfterTest(TestDetails testDetails)
        {
            // do nothing
        }
    }
}
