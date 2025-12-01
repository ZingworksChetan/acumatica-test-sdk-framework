using Core.Config;
using Core.Login;
using Core.TestExecution;
using TestAcumatica.Extension;
using System;

namespace TestAcumatica
{
    public class Test : Check
    {
        public override void Execute()
        {
            //Test case 1
            PxLogin.LogIn(Config.SITE_DST_LOGIN, Config.SITE_DST_PASSWORD);

            //Test case 2
            var journalEntry = new JournalEntry();
            journalEntry.OpenScreen();
            journalEntry.SetBusinessDate(new DateTime(2017, 12, 31));
            journalEntry.Insert();
            journalEntry.Summary.BranchID.Select("MAIN");
            journalEntry.Summary.LedgerID.Select("ACTUAL");
            journalEntry.Summary.Description.Type("Test journal entry 1");
            journalEntry.Details.New();
            journalEntry.Details.Row.AccountID.Select("101020");
            journalEntry.Details.Row.AccountID.GetError().VerifyContains("The currency assigned to the denominated GL account (101020) is different from the transaction currency.");
            journalEntry.Details.Row.AccountID.Type("100000");
            journalEntry.Details.Row.ProjectID.Select("X");
            journalEntry.Details.Row.AccountID_Account_description.GetValue().VerifyEquals("Petty Cash USD");
            journalEntry.Details.Row.CuryDebitAmt.Type(100);
            journalEntry.Details.New();
            journalEntry.Summary.CuryDebitTotal.GetError().VerifyContains("Batch is out of balance, please review.");
            journalEntry.Details.Row.AccountID.Select("101000");
            journalEntry.Details.Row.ProjectID.GetValue().VerifyEquals("X");
            journalEntry.Details.Row.CuryCreditAmt.Type(0);
            journalEntry.VerifyAlert(journalEntry.Save, "Inserting  'GL Batch' record raised at least one error. Please review the errors.");
            journalEntry.Details.SelectRow(2);
            journalEntry.Details.Row.CuryCreditAmt.Type(100);
            journalEntry.Save();

            //Test case 3
            journalEntry.Release();
            journalEntry.Summary.AutoReverseCopy.GetValue().VerifyEquals(false);

            //Test case 4
            journalEntry.ReverseBatch();
            journalEntry.Summary.AutoReverseCopy.GetValue().VerifyEquals(true);
            journalEntry.Summary.Description.Type("Test journal entry 2");
            journalEntry.Save();

            //Test case 5
            var releaseTransactionsGl = new ReleaseTransactionsGl();
            releaseTransactionsGl.OpenScreen();
            releaseTransactionsGl.Details.RowsCount().VerifyEquals(1);
            releaseTransactionsGl.Details.Row.Selected.SetTrue();
            releaseTransactionsGl.Process();
        }
    }
}