using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Gettyclasses
{
    public class supereventimagesmgmt:IDisposable
    {
         #region :: constructor ::
         DAL dal;
		public supereventimagesmgmt()
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
        ~supereventimagesmgmt()
		{
			Dispose();
		}		
		#endregion


        #region :: SuperEvent Images ::
        public int SaveSuperEventImages(string supereventid, string imagename)
        {
            int rowsaffected = 0;
                   
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["StreamingConnectionString"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_SuperEventImages_Save";
                cmd.Connection = con;   
                cmd.Parameters.Add("@supereventid", SqlDbType.Int).Value = supereventid;
                cmd.Parameters.Add("@imagename", SqlDbType.VarChar).Value = imagename;
                con.Open();
                rowsaffected=cmd.ExecuteNonQuery();
                con.Close();
              
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "supereventimagesmgmt.cs SaveSuperEventImages ", ex); 
            }
            return rowsaffected;
        }

        public DataTable GetSuperEventImages(string supereventid)
        {
            DataTable dt = new DataTable();
          
            try
            {
                SqlDataAdapter ada = new SqlDataAdapter();
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["StreamingConnectionString"]);
                SqlCommand cmd = new SqlCommand();               

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_SuperEventImages_GetAll";
                cmd.Connection = con;
                cmd.Parameters.Add("@supereventid", SqlDbType.Int);
                cmd.Parameters["@supereventid"].Value =supereventid ;
                ada.SelectCommand = cmd;
                ada.Fill(dt);
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "supereventimagesmgmt.cs GetSuperEventImages ", ex); 
            }
            return dt;
        }

        public int DeleteSuperEventImages(string ids)
        {
            int rowsaffected = 0;            
                    
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["StreamingConnectionString"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_SuperEventImages_Delete";
                cmd.Connection = con;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = ids;
                con.Open();
                rowsaffected=cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "supereventimagesmgmt.cs DeleteSuperEventImages ", ex);
            }
            return rowsaffected;
        }



        #endregion
    }
}
