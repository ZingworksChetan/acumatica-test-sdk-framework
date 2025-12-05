using Core.Log;
using Core.TestExecution;
using GeneratedWrappers.Acumatica;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.IO;

namespace Tests_Acumatica_demo.Tests
{
    public class SalesOrder : Check
    {
        private readonly SO301000_SOOrderEntry soorderScreen = new SO301000_SOOrderEntry();
        private SalesOrderData testData;

        public override void Execute()
        {
            TestExecution.CreateGroup("Sales Order Test Scenarios");
                LoadTestData();
                CreateSOOrder_WithValidData();
        }

        public JObject LoadTestData()
        {
            string jsonPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "SalesOrderData.json");
            string json = File.ReadAllText(jsonPath);

            testData = JsonConvert.DeserializeObject<SalesOrderData>(json);
            Assert.That(testData, Is.Not.Null, "Test data could not be loaded or is null.");
            return JObject.Parse(json);
        }

        #region
        public void CreateSOOrder_WithValidData()
        {
            using (TestExecution.CreateTestCaseGroup("Verify Sales Order with valid data"))
                try
                {
                    using (TestExecution.CreateTestStepGroup("Open Sales Order Screen"))
                        try
                        {
                            soorderScreen.OpenScreen();
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"Failed to open Sales Order screen: {ex.Message}");
                            throw;
                        }
                    using (TestExecution.CreateTestStepGroup("Verify Order Type is Visible"))
                        try
                        {
                            soorderScreen.Document_form.OrderType.IsPresent();
                            Log.Screenshot();
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"Order Type field is not visible: {ex.Message}");
                            throw;
                        }
                    using (TestExecution.CreateTestStepGroup("Verify that by default Order Type is 'SO'"))
                        try
                        {
                            String expectedOrderType = testData.Expected.ExpectedOrderType;
                            String actualOrderType = soorderScreen.Document_form.OrderType.GetValue();
                            Assert.That(actualOrderType, Is.EqualTo(expectedOrderType), $"Verification Failed: Expected Order Type: {expectedOrderType}, Actual Order Type: {actualOrderType}");
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"Order Type verification failed: {ex.Message}");
                            throw;
                        }
                    using (TestExecution.CreateTestStepGroup("Enter Valid Test Data"))
                        try
                        {
                            //Filling Header details
                            soorderScreen.Document_form.CustomerID.Type(testData.SalesOrder.Customer);
                            soorderScreen.Document_form.OrderDesc.Type(testData.SalesOrder.Description);

                            //Adding first item
                            AddLineItemsToSalesOrder(testData.Details[0].Item1.InventoryID,
                                                     testData.Details[0].Item1.Warehouse,
                                                     testData.Details[0].Item1.OrderQty
                                                     );
                            //Adding second item details
                            AddLineItemsToSalesOrder(testData.Details[1].Item2.InventoryID,
                                                     testData.Details[1].Item2.Warehouse,
                                                     testData.Details[1].Item2.OrderQty
                                                     );
                            soorderScreen.Save();
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"Failed to enter test data: {ex.Message}");
                            throw;
                        }
                    using (TestExecution.CreateTestStepGroup("Validate Saved Order Number"))
                        try
                        {
                            string orderNumber = soorderScreen.Document_form.OrderNbr.GetValue();
                            Assert.That(!string.IsNullOrEmpty(orderNumber), "Order Number should not be null or empty after saving.");
                            Log.Information($"Order created successfully with Order Number: {orderNumber}");
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"Order Number validation failed: {ex.Message}");
                            throw;
                        }
                }
                catch (Exception ex)
                {
                    Log.Error($"Test case failed with exception: {ex.Message}");
                    Log.Screenshot("Failure Screenshot");
                    throw;
                }
        }
        #endregion

        public void AddLineItemsToSalesOrder(String inventoryId, String warehouse, int qty)
        {
            soorderScreen.Transactions_grid.ClickAndAddNewRow();
            soorderScreen.Transactions_grid.Row.InventoryID.Type(inventoryId);
            soorderScreen.Transactions_grid.Row.SiteID.Type(warehouse);
            soorderScreen.Transactions_grid.Row.OrderQty.Type(qty);
        }

    }

    public class SalesOrderData
    {
        public SalesOrderHeader SalesOrder { get; set; }
        public SalesOrderDetail[] Details { get; set; }
        public ExpectedValues Expected { get; set; }
    }

    public class SalesOrderHeader
    {
        public string Customer { get; set; }
        public string Description { get; set; }
    }

    public class SalesOrderDetail
    {
        public ItemDetail Item1 { get; set; }
        public ItemDetail Item2 { get; set; }
    }   

    public class ItemDetail
    {
        public string Branch { get; set; }
        public string InventoryID { get; set; }
        public string Warehouse { get; set; }
        public int OrderQty { get; set; }
    }
    public class ExpectedValues
    {
        public string ExpectedOrderType { get; set; }
        public int OrderedQty { get; set; }
    }
}