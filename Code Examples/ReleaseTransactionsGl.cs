using GeneratedWrappers.Acumatica;

namespace TestAcumatica.Extension
{   
    //Class represents Release Transactions page, extends auto-generated page wrapper.
    public class ReleaseTransactionsGl : GL501000_BatchRelease
    {
        //Makes transactions grid container accessible.
        public c_batchlist_grid Details   
        {
            get 
            {
                return base.BatchList_grid; 
            }
        }
    }
}