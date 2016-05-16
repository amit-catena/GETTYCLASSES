using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib;
using System.Data;
using System.Data.SqlClient;

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
                    SqlParameter[] objSqlParameter = {new SqlParameter("@AppId",SqlDbType.Int),
                                                      new SqlParameter("@siteId",SqlDbType.Int)};

                    objSqlParameter[0].Value = catid;
                    objSqlParameter[1].Value = siteid;

                    //dt = dal.GetDataTable(CommandType.StoredProcedure, "PR_SP_GetAppImageOrder", objSqlParameter);

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
                    SqlParameter[] objSqlParameter = {new SqlParameter("@ImgId",SqlDbType.Int),
                                                      new SqlParameter("@AppId",SqlDbType.Int),
                                                      new SqlParameter("@siteId",SqlDbType.Int),
                                                      new SqlParameter("@imgOrder",SqlDbType.Int)};

                    objSqlParameter[0].Value = imgid;
                    objSqlParameter[1].Value = catid;
                    objSqlParameter[2].Value = siteid;
                    objSqlParameter[3].Value = imgorder;

                    //dal.ExecuteNonQuery(CommandType.StoredProcedure, "PR_SP_UpdateAppImageOrder", objSqlParameter);

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
                //CommonLib.SqlParameterArray arr = new CommonLib.SqlParameterArray();
                //arr.Add("@AppId", catid, SqlDbType.Int);
                //arr.Add("@SiteID", siteid, SqlDbType.Int);
                //arr.Add("@ImageTitle", imgnm, SqlDbType.VarChar, 150);
                //arr.Add("@ImageURL", imgurl, SqlDbType.VarChar, 250);
                //arr.Add("@ImageOrder", 1, SqlDbType.Int);
                //arr.Add("@ImageDate", imageDate, SqlDbType.VarChar);
                //arr.Add("@Image", fname, SqlDbType.VarChar);

                //cnt = dal.ExecuteNonQuery(CommandType.StoredProcedure, "PR_SP_InsertAppImages", arr);
                if (cnt > 0)
                {
                    flag = true;
                }

            }
            catch (System.Exception ex) { CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "BLL.AppImages.InsertAppImages", ex); }
            finally { }
            return flag;
        }

        public DataSet AppImagesList(string appId, int siteid, string strsearch)
        {
            DataSet ds = null;
            try
            {
                SqlParameter[] msgPara ={ new SqlParameter("@PageSize",SqlDbType.Int),
                                         new SqlParameter("@CurrentPage",SqlDbType.Int),
                                         new SqlParameter("@txtsearch",SqlDbType.VarChar),
                                         new SqlParameter("@AppId",SqlDbType.Int),
                                         new SqlParameter("@SiteID",SqlDbType.Int),
                                         new SqlParameter("@ItemCount",SqlDbType.Int)
                                        };
                msgPara[0].Value = this.Pagesize;
                msgPara[1].Value = this.Currentpage;
                msgPara[2].Value = strsearch;
                msgPara[3].Value = Convert.ToInt32(appId);
                msgPara[4].Value = siteid;
                msgPara[5].Value = 0;
                msgPara[5].Direction = ParameterDirection.Output;

                //ds = dal.GetDataSet(CommandType.StoredProcedure, "PR_SP_GetPagingAppMultiImages", msgPara);
                this.Totalpages = Convert.ToInt32(msgPara[5].Value);
            }
            catch (System.Exception ex) { CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "BLL.AppImages.AppImagesList", ex); }
            finally { }
            return ds;
        }

        /// <summary>
        ///  Method to check the count of CategoryImages
        ///  for perticular category & siteId
        /// </summary>
        /// <param name="catid"></param>
        /// <param name="siteid"></param>
        /// <returns></returns>
        public int GetImageCount(int appId, int siteid)
        {
            int intCnt = 0;
            try
            {
                if (siteid > 0 && appId > 0)
                {
                    SqlParameter[] objSqlParameter = {new SqlParameter("@AppId",SqlDbType.Int),
                                                  new SqlParameter("@siteId",SqlDbType.Int)};
                    objSqlParameter[0].Value = appId;
                    objSqlParameter[1].Value = siteid;


                    //intCnt = Convert.ToInt32(dal.ExecuteScalar(CommandType.StoredProcedure, "PR_SP_CountAppImages", objSqlParameter));

                }
            }
            catch (System.Exception ex)
            { CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "BLL.AppImages.GetImageCount()", ex); }
            finally { }

            return intCnt;
        }

        /// <summary>
        ///  Method to update ImageOrder
        /// </summary>
        /// <param name="imgid"></param>
        /// <param name="imgorder"></param>
        /// <param name="alterlid"></param>
        /// <param name="alterorder"></param>
        /// <param name="siteid"></param>
        /// <param name="catid"></param>
        /// <returns></returns>
        public bool UpdateImageOrderAppImage(int imgid, int imgorder, int alterlid, int alterorder, int siteid, int appId)
        {
            bool flag = true;
            try
            {
                if (imgid > 0 && imgorder > 0 && alterlid > 0 && alterorder > 0 && appId > 0)
                {

                    SqlParameter[] objSqlParameter ={
                                                     new SqlParameter("@siteId",SqlDbType.Int),
                                                     new SqlParameter("@AppId",SqlDbType.Int),
                                                     new SqlParameter("@alterorder",SqlDbType.Int),
                                                     new SqlParameter("@imgid",SqlDbType.Int),
                                                     new SqlParameter("@imgorder",SqlDbType.Int),
                                                     new SqlParameter("@alterlid",SqlDbType.Int)
                                                    };

                    objSqlParameter[0].Value = siteid;
                    objSqlParameter[1].Value = appId;
                    objSqlParameter[2].Value = alterorder;
                    objSqlParameter[3].Value = imgid;
                    objSqlParameter[4].Value = imgorder;
                    objSqlParameter[5].Value = alterlid;


                    //dal.ExecuteNonQuery(CommandType.StoredProcedure, "PR_SP_UpdateImageOrderAppImage", objSqlParameter);
                    flag = true;

                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "BLL.AppImages.UpdateImageOrderAppImage", ex);
            }
            finally { }

            return flag;

        }

        //change status of image in CategoryImages table 
        public bool UpdateStatusAppImages(int intimgID, string status)
        {
            bool boolReturn = false;
            try
            {
                if (intimgID > 0)
                {
                    //CommonLib.SqlParameterArray ParamArray = new CommonLib.SqlParameterArray();
                    //ParamArray.Add("@ImageID", intimgID, SqlDbType.Int);
                    //ParamArray.Add("@Status", status, SqlDbType.Char, 1);
                    //intimgID = Convert.ToInt32(dal.ExecuteNonQuery(CommandType.StoredProcedure, "PR_SP_UpdateStatusAppImages", ParamArray));
                    if (intimgID > 0)
                    {
                        boolReturn = true;
                    }
                    else
                    {
                        boolReturn = false;
                    }
                }
            }
            catch (System.Exception ex) { CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "", ex); }
            finally { }
            return boolReturn;
        }

        /// <summary>
        ///  Get ImageID fromCategoryImages with smaller
        ///  ImageOrder than given to set the app-image order(up)
        /// </summary>
        /// <param name="intimgorder"></param>
        /// <param name="siteid"></param>
        /// <param name="catid"></param>
        /// <returns></returns>
        public int GetMinCatImageAlterID(int intimgorder, int siteid, int appId, ref int alterorder)
        {
            int intCnt = 0;
            DataTable dt = null;
            try
            {
                if (intimgorder > 0 && siteid > 0 && appId > 0)
                {
                    SqlParameter[] objSqlParameter = { 
                                                       new SqlParameter("@AppId",SqlDbType.Int),
                                                       new SqlParameter("@siteId",SqlDbType.Int),
                                                       new SqlParameter("@imgOrder",SqlDbType.Int),
                                                       new SqlParameter("@selector",SqlDbType.VarChar)
                                                     };
                    objSqlParameter[0].Value = appId;
                    objSqlParameter[1].Value = siteid;
                    objSqlParameter[2].Value = intimgorder;

                    //Selector below is used to reuse the same storedprocedure
                    string selector = "min";
                    objSqlParameter[3].Value = selector;

                    //dt = dal.GetDataTable(CommandType.StoredProcedure, "PR_SP_GetMinMaxAppImageAlterID", objSqlParameter);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        intCnt = Convert.ToInt32(dt.Rows[0]["ImageId"]);
                        alterorder = dt.Rows[0]["ImageOrder"] == System.DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["ImageOrder"]);
                    }
                }
            }
            catch (System.Exception ex)
            { CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "BLL.AppImages.GetMinCatImageAlterID", ex); }
            finally { }

            return intCnt;
        }


        /// <summary>
        ///  Get ImageID from AppScreenShots with greater
        ///  ImageOrder than given to set the category-image order(down)
        /// </summary>
        /// <param name="intimgorder"></param>
        /// <param name="siteid"></param>
        /// <param name="catid"></param>
        /// <returns></returns>
        public int GetMaxCatImageAlterID(int intimgorder, int siteid, int appId, ref int alterorder)
        {
            int intCnt = 0;
            DataTable dt = null;
            try
            {
                if (intimgorder > 0 && siteid > 0 && appId > 0)
                {
                     SqlParameter[] objSqlParameter = { 
                                                       new SqlParameter("@AppId",SqlDbType.Int),
                                                       new SqlParameter("@siteId",SqlDbType.Int),
                                                       new SqlParameter("@imgOrder",SqlDbType.Int),
                                                       new SqlParameter("@selector",SqlDbType.VarChar)
                                                     };
                     objSqlParameter[0].Value = appId;
                    objSqlParameter[1].Value = siteid;
                    objSqlParameter[2].Value = intimgorder;

                    //Selector below is used to reuse the same storedprocedure
                    string selector = "max";
                    objSqlParameter[3].Value = selector;

                    //dt = dal.GetDataTable(CommandType.StoredProcedure, "PR_SP_GetMinMaxAppImageAlterID", objSqlParameter);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        intCnt = Convert.ToInt32(dt.Rows[0]["ImageId"]);
                        alterorder = dt.Rows[0]["ImageOrder"] == System.DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["ImageOrder"]);
                    }

                }
            }
            catch (System.Exception ex)
            { CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "BLL.AppImages.GetMaxCatImageAlterID", ex); }
            finally { }

            return intCnt;
        }

        /// <summary>
        ///  Method to get Image Name from AppImages
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetAppImages(string id)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] objSqlParameter = { 
                                                    new SqlParameter("@AppId", SqlDbType.Int),
                                                    new SqlParameter("@SiteID", SqlDbType.Int) 
                                                };
                objSqlParameter[0].Value = id;
                objSqlParameter[1].Value = this.Siteid;

                //dt = dal.GetDataTable(CommandType.StoredProcedure, "PR_SP_GetAppImages", objSqlParameter);
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "BLL.Category.GetAppImages", ex);
            }
            return dt;
        }

        /// <summary>
        ///  Method to get site name for publishing image
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetSiteNameforUpload(string id)
        {
            string strSiteName = "";
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] objSqlParameter = { new SqlParameter("@siteID", SqlDbType.Int) };
                objSqlParameter[0].Value = id;

                //dt = dal.GetDataTable(CommandType.StoredProcedure, "SB_Admin_SP_GetSiteURL", objSqlParameter);

                if (dt.Rows.Count > 0)
                {

                    strSiteName = dt.Rows[0]["siteURL"].ToString();
                    string[] strarr = strSiteName.Split('.');
                    return strarr[1];
                }
                else
                {
                    return strSiteName;
                }

            }
            catch (Exception ex)
            {
                dt = null;
                return ex.ToString();
            }
        }

        /// <summary>
        ///  Method to set uploaded flag to Y in AppMaster
        /// </summary>
        /// <param name="catid"></param>
        public void SetUploadedFlag(string appId)
        {
            try
            {
                SqlParameter[] objSqlParameter = { new SqlParameter("@AppId", SqlDbType.Int) };
                objSqlParameter[0].Value = appId;

                //dal.ExecuteNonQuery(CommandType.StoredProcedure, "PR_SP_UpdateUploadedImageFlag", objSqlParameter);

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "", ex);
            }
        }
        #endregion
    }
}
