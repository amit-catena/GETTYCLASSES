using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gettyclasses;   
namespace Gettyevents
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Getimagedata _obj = new Getimagedata();
                Console.WriteLine("Scheduler started");  
                _obj._strnetworkID = commonfn._defaultNetwork;
                _obj.AddGettyeventdetails();
                Console.WriteLine("Scheduler end");  
            }
            catch (Exception ex)
            {
                Console.WriteLine("Scheduler Error" + ex.Message);  
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
                ErrorLog.SaveErrorLog("0", "Main(string[] args)", "Program", "Program", ex.Message, "2");
            }
        }
    }
}
