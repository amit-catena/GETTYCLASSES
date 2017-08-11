using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Gettyclasses
{
    public class SiteInfo
    {


        public SiteInfo()
        {
            ImageServer = false;
            SiteUrl = "";
        }
        public string SiteUrl { get; set; }
        public bool ImageServer { get; set; }
        public string NetWorkID { get; set; }


        public bool GetSiteDetails(string siteid, string NetWorkID)
        {
            bool flag = false;
            SqlConnection _newconn = new SqlConnection();
            try
            {
                _newconn = new SqlConnection(Function.GetnetworkConnectionstring(NetWorkID));
                SqlCommand mycmd = new SqlCommand();
                SqlDataReader dr;
                mycmd.CommandType = CommandType.Text;
                mycmd.CommandText = string.Format("SELECT siteurl,IsImageServer FROM SiteMaster WHERE SiteID={0}", siteid);
                mycmd.Connection = _newconn;
                _newconn.Open();
                dr = mycmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    SiteUrl = dr["siteurl"].ToString();
                    if (dr["IsImageServer"].ToString().Trim().ToUpper() == "Y")
                        ImageServer = true;
                    else
                        ImageServer = false;
                    flag = true;
                }
                dr.Close();
                _newconn.Close();
                flag = true;
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "webClient.Dispose() :", ex);
            }
            finally
            {
                if (_newconn.State == ConnectionState.Open)
                    _newconn.Close();
            }

            return flag;

        }
    }
}
