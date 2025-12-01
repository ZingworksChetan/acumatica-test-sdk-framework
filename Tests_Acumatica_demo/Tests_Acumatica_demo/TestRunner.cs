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
            //private SO301000_SOOrderEntry SOOrderScreen = new SO301000_SOOrderEntry();
            private SalesOrder salesOrder = new SalesOrder();

            private void RunTest(string testName, Action testMethod)
            {
                try
                {
                    Console.WriteLine($"\nRunning: {testName}");
                    testMethod.Invoke();
                    //Console.WriteLine($"Result: PASS");
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
            }
        }
    }
}
