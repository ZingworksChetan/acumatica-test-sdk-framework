using Core.TestExecution;
using GeneratedWrappers.Acumatica;
using System;

namespace Tests_Acumatica_demo.Tests
{
    public class GridAutomation_Test : Check
    {
        private readonly SO301000_SOOrderEntry salesOrderEntry = new SO301000_SOOrderEntry();
        public SalesOrder SalesOrder = new SalesOrder();

        public override void Execute()
        {
            TestExecution.CreateGroup("Grid Automation Test Scenario");
            grid_automation_test();
        }

        public void grid_automation_test()
        {
            using (TestExecution.CreateTestCaseGroup("Test Grid Automation - add, edit and delete row"))
            {
                salesOrderEntry.OpenScreen();
                salesOrderEntry.Insert();
                salesOrderEntry.Document_form.CustomerID.Type("ABAKERY");
                salesOrderEntry.Document_form.OrderDesc.Type("Test Grid Automation");

                //Adding Grid lines
                using (TestExecution.CreateTestStepGroup("Add Lines in Grid"))
                {
                    try
                    {
                        SalesOrder.AddLineItemsToSalesOrder("APPLES", "WHOLESALE", 5);
                        SalesOrder.AddLineItemsToSalesOrder("APJAM08", "WHOLESALE", 5);
                        SalesOrder.AddLineItemsToSalesOrder("CHERJAM08", "WHOLESALE", 2);
                        salesOrderEntry.Save();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding lines in grid: {ex.Message}");
                    }
                }
                //Edit Row
                using (TestExecution.CreateTestStepGroup("Edit Line in Grid"))
                {
                    try
                    {
                        salesOrderEntry.Transactions_grid.SelectRow(2);
                        salesOrderEntry.Transactions_grid.Row.OrderQty.Type("10");
                        salesOrderEntry.Save();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error editing line in grid: {ex.Message}");
                    }
                }
                //Delete Row
                using (TestExecution.CreateTestStepGroup("Delete Line in Grid"))
                {
                     try{
                          salesOrderEntry.Transactions_grid.SelectRow(3);
                          salesOrderEntry.Transactions_grid.Delete();
                          salesOrderEntry.Save();
                     }
                     catch (Exception ex)
                     {
                         Console.WriteLine($"Error deleting line in grid: {ex.Message}");
                     }  
                }
            }
        }
    }
}