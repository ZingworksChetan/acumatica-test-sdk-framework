using Core.Login;
using Core.TestExecution;
using System;
using Tests_Acumatica_demo.Tests;

namespace Tests_Acumatica_demo
{
    class TestRunner
    {
        public class TestCase : Check
        {
            private SalesOrder salesOrder = new SalesOrder();
            private GridAutomation_Test gridAutomationTest = new GridAutomation_Test();

            private void RunTest(string testName, Action testMethod)
            {
                try
                {
                    Console.WriteLine($"\nRunning: {testName}");
                    testMethod.Invoke();
                    Console.WriteLine($"Result: PASS");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Result: FAIL - {ex.Message}");
                }
            }
            public override void Execute()
            {
                PxLogin.LoginToDestinationSite();

                RunTest("Test 1 - Create new Sales Order", salesOrder.Execute);
                RunTest("Test 2 - Grid Automation Test", gridAutomationTest.Execute);
            }
        }
    }
}
