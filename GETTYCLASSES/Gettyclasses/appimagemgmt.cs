using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CommonLib;
using System.Configuration;


namespace Gettyclasses
{
     public class appimagemgmt:IDisposable
    {
         #region :: constructor ::
        DAL dal;
		public appimagemgmt()
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
        ~appimagemgmt()
		{
			Dispose();
		}		
		#endregion


        #region :: SuperEvent Images ::
        public int SaveAppImages(string appid, string imagename,string siteid)
        {
            int rowsaffected = 0;
                   
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["gamingappstore"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_SP_InsertAppImages";
                cmd.Connection = con;
                cmd.Parameters.Add("@AppId", SqlDbType.Int).Value = appid;
                cmd.Parameters.Add("@Image", SqlDbType.VarChar).Value = imagename;
                cmd.Parameters.Add("@SiteID", SqlDbType.Int).Value = siteid;
                con.Open();
                rowsaffected=cmd.ExecuteNonQuery();
                con.Close();
              
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "", ex); 
            }
            return rowsaffected;
        }

        public DataTable GetAppImages(string appid,string siteid)
        {
            DataTable dt = new DataTable();
          
            try
            {
                SqlDataAdapter ada = new SqlDataAdapter();
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["gamingappstore"]);
                SqlCommand cmd = new SqlCommand();               

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_AppImages_GetAll";
                cmd.Connection = con;
                cmd.Parameters.Add("@AppId", SqlDbType.Int);
                cmd.Parameters["@AppId"].Value = appid;
                cmd.Parameters.Add("@siteid", SqlDbType.Int);
                cmd.Parameters["@siteid"].Value = siteid;
                ada.SelectCommand = cmd;
                ada.Fill(dt);
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, " ", ex); 
            }
            return dt;
        }

        public int DeleteAppImages(string ids,string siteid)
        {
            int rowsaffected = 0;            
                    
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["gamingappstore"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_AppImages_Delete";
                cmd.Connection = con;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = ids;
                cmd.Parameters.Add("@siteid", SqlDbType.VarChar).Value = siteid;
                con.Open();
                rowsaffected=cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "", ex);
            }
            return rowsaffected;
        }



        #endregion
    }
}
