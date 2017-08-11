using System;
using System.Data;
using System.Data.SqlClient;

namespace Gettyclasses
{
    /// <summary>
    /// Summary description for ErrorLog.
    /// </summary>
    public class ErrorLog
    {
        public ErrorLog()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public static string constr = "";

        public static void SaveErrorLog(string siteid, string module, string classname, string methodname, string exceptionMesaage, string networkid)
        {

            if (exceptionMesaage.Trim() != "Thread was being aborted.")
            {

                try
                {

                    SqlConnection dbConn = new SqlConnection(Function.GetnetworkConnectionstring(networkid));

                    if (dbConn.State == ConnectionState.Closed)
                        dbConn.Open();
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        cmd.Connection = dbConn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "DD_SP_SaveAdminErrorLog";

                        cmd.Parameters.Add("@module", SqlDbType.VarChar);
                        cmd.Parameters.Add("@classname", SqlDbType.VarChar);
                        cmd.Parameters.Add("@methodname", SqlDbType.VarChar);
                        cmd.Parameters.Add("@errormsg", SqlDbType.VarChar);
                        cmd.Parameters.Add("@siteid", SqlDbType.Int);

                        cmd.Parameters["@module"].Value = module;
                        cmd.Parameters["@classname"].Value = classname;
                        cmd.Parameters["@methodname"].Value = methodname;
                        cmd.Parameters["@errormsg"].Value = exceptionMesaage;
                        cmd.Parameters["@siteid"].Value = siteid;

                        cmd.ExecuteNonQuery();

                        dbConn.Close();
                        dbConn = null;
                    }
                    catch
                    {

                    }
                    finally
                    {

                        cmd = null;


                    }
                }
                catch
                {

                }
            }

        }






    }
}
