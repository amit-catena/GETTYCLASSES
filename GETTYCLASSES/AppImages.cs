using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace gettyclasses
{
    public class AppImages:IDisposable
    {
        DAL dal;

        #region:Initializers (Constructors):
        public AppImages()
        {
            dal = new DAL();
        }
        public AppImages(string conn)
        {
            dal = new DAL();
        }
        #endregion

        #region:Destructor:
        ~AppImages()
       {
           Dispose();
       }
        #endregion
        

        #region:Dispose:
        public void Dispose()
       {
           dal.Dispose();
       }
       #endregion

        #region :: Properties ::
        public int Currentpage { get; set; }
        public int Pagesize { get; set; }
        public int Totalpages { get; set; }
        public int Siteid { get; set; }
        #endregion

        #region :: Public Methods ::
        /// <summary>
        ///  Method to get all details from CategoryImages with
        ///  given category Id & site Id
        /// </summary>
        /// <param name="catid"></param>
        /// <param name="siteid"></param>
        /// <returns></returns>
        public DataTable GetImageOrder(int catid, int siteid)
        {
            DataTable dt = new DataTable();
            try
            {
                if (catid > 0 && siteid > 0)
                {
                    SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["gamingappstore"]);
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PR_SP_GetAppImageOrder";
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@AppId", catid);
                    cmd.Parameters.AddWithValue("@siteId", siteid);
                    SqlDataAdapter objAdpt = new SqlDataAdapter(cmd);
                    objAdpt.Fill(dt);

                }
            }
            catch (System.Exception ex)
            { CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "BLL.AppImages.GetImageOrder", ex); }
            finally { }

            return dt;

        }

        //////update ImageOrder - increase by 1
        /// <summary>
        ///  Method to update ImageOrder in CategoryImages
        /// </summary>
        /// <param name="imgid"></param>
        /// <param name="imgorder"></param>
        /// <param name="siteid"></param>
        /// <param name="catid"></param>
        /// <returns></returns>
        public bool UpdateImgOrderByOne(int imgid, int imgorder, int siteid, int catid)
        {
            bool flag = true;
            try
            {
                if (siteid > 0 && catid > 0 && imgorder > 0 && imgid > 0)
                {
                    SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["gamingappstore"]);
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PR_SP_UpdateAppImageOrder";
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@ImgId", imgid);
                    cmd.Parameters.AddWithValue("@AppId", catid);
                    cmd.Parameters.AddWithValue("@siteId", siteid);
                    cmd.Parameters.AddWithValue("@imgOrder", imgorder);

                    con.Open();
                    int cnt = cmd.ExecuteNonQuery();
                    con.Close();

                    flag = true;

                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "BLL.AppImages.UpdateImgOrderByOne", ex);
            }
            finally { }

            return flag;

        }

        public bool InsertAppImages(string catid, int siteid, string fname, string imgnm, string imgurl, string imgcredit, string imageDate)
        {
            int cnt = 0;
            bool flag = false;
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["gamingappstore"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_SP_InsertAppImages";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@AppId", catid);
                cmd.Parameters.AddWithValue("@SiteId", siteid);
                cmd.Parameters.AddWithValue("@ImageTitle", imgnm);
                cmd.Parameters.AddWithValue("@ImageURL", imgurl);
                cmd.Parameters.AddWithValue("@ImageOrder", 1);
                cmd.Parameters.AddWithValue("@ImageDate", imageDate);
                cmd.Parameters.AddWithValue("@Image", fname);

                con.Open();
                cnt = cmd.ExecuteNonQuery();
                con.Close();

                if (cnt > 0)
                {
                    flag = true;
                }

            }
            catch (System.Exception ex) { CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "BLL.AppImages.InsertAppImages", ex); }
            finally { }
            return flag;
        }

        #endregion
    }
}
