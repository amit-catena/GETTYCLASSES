using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using CommonLib;

namespace NewGettyAPIclasses
{
    /// <summary>
    /// Get sire related information
    /// </summary>
    public class SiteInfo:IDisposable 
    {
        /// <summary>
        /// Public variables
        /// </summary>
         
        public SiteInfo()
        {
            ImageServer = false;
            SiteUrl = "";
        }
        public string SiteUrl { get; set; }
        public bool ImageServer { get; set; }
        public string NetWorkID { get; set; }

        /// <summary>
        /// Get Site Details
        /// </summary>
        /// <param name="siteid"></param>
        /// <param name="NetWorkID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get All Site details
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="NetWorkID"></param>
        /// <returns></returns>
        public bool GetAllsitdetails(int siteId,string NetWorkID)
        {
            bool flag = false;
            DataTable _dt = new DataTable();
            DAL ObjDal = new CommonLib.DAL(Function.GetnetworkIDwithconnectionstring(NetWorkID));
             SqlParameter[] msgpara = { new SqlParameter("@SiteID", SqlDbType.Int)
                                         
                                         };
                msgpara[0].Value = siteId;
                _dt = ObjDal.GetDataTable(CommandType.StoredProcedure,"GettyAPI_AK_SP_Getsitedetails", msgpara);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        foreach(DataRow dr in _dt.Rows)
                        {
                            SiteUrl = dr["siteurl"].ToString();
                            if (dr["IsImageServer"].ToString().Trim().ToUpper() == "Y")
                                ImageServer = true;
                            else
                                ImageServer = false;
                            flag = true;
                        }
                    }
                }
          return flag;
        }

        ~SiteInfo()
        {
            Dispose();
        }

        #region :: Dispose Method ::
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
