using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Gettyclasses
{
     public class slotmgmt:IDisposable
    {
        #region :: Default Constructor ::
        public slotmgmt()
        {

        }
        #endregion

        #region :: Dispose Method ::
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region :: Destructor  ::
        ~slotmgmt()
        {
            Dispose();
        }
        #endregion

        public int SaveSlotHomePageImages(string slotid, string imagename)
        {
            int rowsaffected = 0;

            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["Adsensestring"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_SlotHomePageImage_Save";
                cmd.Connection = con;
                cmd.Parameters.Add("@SlotId", SqlDbType.Int).Value = slotid;
                cmd.Parameters.Add("@ImageName", SqlDbType.VarChar).Value = imagename;
                con.Open();
                rowsaffected = cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "slotmgmt.cs SaveSlotHomePageImages ", ex);
            }
            return rowsaffected;
        }

        public int SaveSlotScreenShotImages(string slotid, string imagename)
        {
            int rowsaffected = 0;

            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["Adsensestring"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_SlotScrrenShotImage_Save";
                cmd.Connection = con;
                cmd.Parameters.Add("@SlotId", SqlDbType.Int).Value = slotid;
                cmd.Parameters.Add("@ImageName", SqlDbType.VarChar).Value = imagename;
                con.Open();
                rowsaffected = cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "slotmgmt.cs SaveSlotScreenShotImages ", ex);
            }
            return rowsaffected;
        }

        public DataTable GetSlotScreenShotImages(string slotid)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlDataAdapter ada = new SqlDataAdapter();
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["Adsensestring"]);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_GetScreenshotImages";
                cmd.Connection = con;
                cmd.Parameters.Add("@SlotId", SqlDbType.Int);
                cmd.Parameters["@SlotId"].Value = slotid;
                ada.SelectCommand = cmd;
                ada.Fill(dt);
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "slotmgmt.cs GetSlotScreenShotImages ", ex);
            }
            return dt;
        }

        public int DeleteScreenshotImages(string ids)
        {
            int rowsaffected = 0;

            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["Adsensestring"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_DeleteScreenshotImages";
                cmd.Connection = con;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = ids;
                con.Open();
                rowsaffected = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "slotmgmt.cs DeleteScreenshotImages ", ex);
            }
            return rowsaffected;
        }

        public DataTable GetDummyUsers()
        {
            DataTable dt = new DataTable();
            try
            {
                string strsql = "select Top 5 UserID,FirstName+' '+LastName as name from RegisteredUser order by AddedOn desc";
                SqlDataAdapter ada = new SqlDataAdapter();
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["Network1"]);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strsql;
                cmd.Connection = con;            
              
                ada.SelectCommand = cmd;
                ada.Fill(dt);
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "slotmgmt.cs GetDummyUsers ", ex);
            }
            return dt;
        }

        public int SavePosterImage(string slotid,string imagename)
        {
            int rowsaffected = 0;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["Adsensestring"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_SlotPosterImage_Save";
                cmd.Connection = con;
                cmd.Parameters.Add("@SlotId", SqlDbType.Int).Value = slotid;
                cmd.Parameters.Add("@ImageName", SqlDbType.VarChar).Value = imagename;
                con.Open();
                rowsaffected = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "slotmgmt.cs SavePosterImage ", ex);
            }
            return rowsaffected;
        }

        public string GetSlotPosterImage(string slotid)
        {
            string imagename = "";
            object objimage = null;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["Adsensestring"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_GetSlotPosterImage";
                cmd.Connection = con;
                cmd.Parameters.Add("@SlotId", SqlDbType.Int).Value = slotid;
                con.Open();
                objimage = cmd.ExecuteScalar();
                if (objimage != null)
                {
                    imagename = objimage.ToString();
                }
                con.Close(); 
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "slotmgmt.cs GetSlotPosterImage ", ex);
            }
            return imagename;
        }

        public DataTable GetHyperlinkForReview()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SQLHelper obj = new SQLHelper(ConfigurationSettings.AppSettings["Network1"]))
                {
                   
                    string strsql = "select LinkID,LinkName from HyperLinkManager where SiteID=1  and IsActive ='Y' order by LinkName";

                    dt = obj.ExecuteDataTable(strsql);

                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "slotmgmt.cs GetHyperlinkForReview ", ex);
            }
            return dt;
        }
         

    }
}
