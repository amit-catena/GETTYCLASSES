using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib;
using System.Data;
using System.Data.SqlClient;


namespace Gettyclasses
{
     public class categoryimagemgmt:IDisposable
     {
        #region :: constructor ::
         DAL dal;
		public categoryimagemgmt()
		{
			//
			// TODO: Add constructor logic here
			//
			dal=new DAL();
		}       
		#endregion

		#region :: Dispose ::
		public void Dispose()
		{
			dal.Dispose();
		}
		
		#endregion

		#region :: Destructor ::
        ~categoryimagemgmt()
		{
			Dispose();
		}		
		#endregion

        public DataTable GetCategoryImage(string Network, string catid, string siteid)
        {
            DataTable dt = new DataTable();
            SqlCommand mycommand;
            try
            {
                string strconn = System.Configuration.ConfigurationSettings.AppSettings[Network].ToString();
                SqlConnection dbconn = new SqlConnection(strconn);
                mycommand = new SqlCommand();
                mycommand.CommandType = CommandType.StoredProcedure;
                mycommand.CommandText = "AN_SP_GetCategoryImage";
                mycommand.Connection = dbconn;
                mycommand.Parameters.Add("@catid", SqlDbType.Int);
                mycommand.Parameters.Add("@siteid", SqlDbType.Int);
                mycommand.Parameters["@catid"].Value = catid;
                mycommand.Parameters["@siteid"].Value = siteid;
                SqlDataAdapter da=new SqlDataAdapter();
                da.SelectCommand = mycommand;
                da.Fill(dt);
                
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public bool AddImage(string Network,string CID, string SID, string Iname, string title, string URL, string credit, string ImageDate)
        {
            string error = "";

            string strsql = "";
            SqlCommand mycommand;
            int cnt = 0;
            try
            {
                string strconn = System.Configuration.ConfigurationSettings.AppSettings[Network].ToString();
                SqlConnection dbconn = new SqlConnection(strconn);
                mycommand = new SqlCommand();
                mycommand.CommandType = CommandType.StoredProcedure;
                mycommand.CommandText = "DD_SP_SaveCategoryImages";
                mycommand.Parameters.Add("@ImageTitle", SqlDbType.VarChar);
                mycommand.Parameters.Add("@CategoryID", SqlDbType.Int);
                mycommand.Parameters.Add("@SiteID", SqlDbType.Int);
                mycommand.Parameters.Add("@Image", SqlDbType.VarChar);
                mycommand.Parameters.Add("@ImageURL", SqlDbType.VarChar);
                mycommand.Parameters.Add("@Credit", SqlDbType.VarChar);
                mycommand.Parameters.Add("@ImageDate", SqlDbType.VarChar);

                mycommand.Parameters["@ImageTitle"].Value = title;
                mycommand.Parameters["@CategoryID"].Value = CID;
                mycommand.Parameters["@SiteID"].Value = SID;
                mycommand.Parameters["@Image"].Value = Iname;
                mycommand.Parameters["@ImageURL"].Value = URL;
                mycommand.Parameters["@Credit"].Value = credit;
                mycommand.Parameters["@ImageDate"].Value = ImageDate;
                mycommand.Connection = dbconn;
                dbconn.Open();
                cnt = mycommand.ExecuteNonQuery();
                dbconn.Close();

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            if (cnt > 0)
                return true;
            else
                return false;
        }
    }
}
