using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace Gettyclasses
{
    public class Signup : IDisposable
    {
        #region :: Private Members ::
        private bool _Disposed = false;
        #endregion
        #region :: Constructors ::
        public Signup()
        {
            try
            {
                
            }
            catch (System.Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "gettyclasses signup.cs Constructor", ex);
            }
        }
        #region :: Destructors ::
        ~Signup()
        {
            try
            {
                this.Dispose();
            }
            catch (System.Exception ex) { CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "gettyclasses signup.cs Destructors", ex); }
        }

        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (System.Exception ex) { CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "gettyclasses signup.cs Dispose", ex); }
        }

        private void Dispose(bool disposing)
        {
            try
            {
                if (!this._Disposed)
                {
                    if (disposing)
                    {
                    }
                }
                this._Disposed = true;
            }
            catch (System.Exception ex) { CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "gettyclasses Writer.cs Dispose()", ex); }
        }
        #endregion
        #endregion
        #region public properties::

        public int SiteID { get; set; }

        public int TemplateID { get; set; }

        public string Title { get; set; }

        public string SignupFormString { get; set; }
        public string SignupFormThankyouString { get; set; }
        public int AddedBy { get; set; }

        public int SignupFormID { get; set; }

        public string ShowNameField { get; set; }

        public string HeaderText1 { get; set; }
        public string HeaderTextColor1 { get; set; }
        public string HeaderTextFont1 { get; set; }
        public string HeaderText2 { get; set; }
        public string HeaderTextColor2 { get; set; }
        public string HeaderTextFont2 { get; set; }

        public string NamePlaceHolder { get; set; }

        public string NameColor { get; set; }

        public string NameFont { get; set; }

        public string EmailPlaceHolder { get; set; }

        public string EmailColor { get; set; }

        public string EmailFont { get; set; }
        public string EmptyEmailMessage { get; set; }
        public string EmptyEmailColor { get; set; }
        public string EmptyEmailFont { get; set; }
        public string EmailExistMessage { get; set; }
        public string EmailExistColor { get; set; }
        public string EmailExistFont { get; set; }

        public string SubmitButtonValue { get; set; }

        public string SubmitButtonColor { get; set; }

        public string SubmitButtonBackGround { get; set; }

        public string SubmitButtonFont { get; set; }

        public string ShowPrivacyStatement { get; set; }

        public string OverlayBackground { get; set; }

        public string CustomCSS { get; set; }

        public int LoadingDelay { get; set; }

        public int ScrollTriggerPercentage { get; set; }

        public int CookieDuration { get; set; }

        public int SuccessCookieDuration { get; set; }

        public string SuccessMessage1 { get; set; }
        public string SuccessMessageColor1 { get; set; }
        public string SuccessMessageFont1 { get; set; }
        public string SuccessMessage2 { get; set; }
        public string SuccessMessageColor2 { get; set; }
        public string SuccessMessageFont2 { get; set; }

        public string RedirectOnSuccess { get; set; }

        public string PassDataToRedirectURL { get; set; }

        public string EnableExitIntent { get; set; }

        public int ExitIntentSensitivity { get; set; }

        public string EnableReferrerDetection { get; set; }

        public string ReferrerDomain { get; set; }

        public string ExcludeThisReferrer { get; set; }

        public string PageSlug { get; set; }

        public string ExactMatchOnPageSlug { get; set; }

        public string ExcludeThisPage { get; set; }

        public int CurrentPage { get; set; }
        public int Totalpages { get; set; }
        public int TotalCounts { get; set; }
        public int PageSize { get; set; }

        public int ImageID { get; set; }
        public string ImageTitle { get; set; }
        public string ImageAlttext { get; set; }
        public string ImageName { get; set; }
        public string ImageDate { get; set; }
        public string NetworkID { get; set; }

        public string ImageIDS { get; set; }

        #endregion
        #region public methods::
        #region Sign up Details::
        
        public int AddImageDetails()
        {
            int res = 0;
            try
            {
             /*  CommonLib.SqlParameterArray param = new CommonLib.SqlParameterArray();
                param.Add("@SiteID", this.SiteID, SqlDbType.Int);
                param.Add("@ImageTitle", this.ImageTitle, SqlDbType.NVarChar, 200);
                param.Add("@ImageAlttext", this.ImageAlttext, SqlDbType.NVarChar, 200);
                param.Add("@ImageName", this.ImageName, SqlDbType.VarChar, 200);
                param.Add("@ImageDate", this.ImageDate, SqlDbType.VarChar, 20);
                param.Add("@AddedBy", this.AddedBy, SqlDbType.Int);*/

                SqlParameter[] msgPara ={ new SqlParameter("@SiteID",SqlDbType.Int),
                                         new SqlParameter("@ImageTitle",SqlDbType.NVarChar,200),
                                         new SqlParameter("@ImageAlttext",SqlDbType.NVarChar,200),                                        
                                         new SqlParameter("@ImageName",SqlDbType.VarChar,200),
                                         new SqlParameter("@ImageDate",SqlDbType.VarChar,20),
                                         new SqlParameter("@AddedBy",SqlDbType.Int)
                                        };
                msgPara[0].Value = this.SiteID;
                msgPara[1].Value = this.ImageTitle;
                msgPara[2].Value = this.ImageAlttext;
                msgPara[3].Value = this.ImageName;
                msgPara[4].Value = this.ImageDate;
                msgPara[5].Value = this.AddedBy;
                System.Configuration.ConfigurationSettings.AppSettings["connString"] = Function.GetnetworkConnectionstring(this.NetworkID);
                using (CommonLib.DAL dal = new CommonLib.DAL())
                {                      
                    res = dal.ExecuteNonQuery("SP_PW_O_SignupFormMaster_AddImageDetails", CommandType.StoredProcedure, msgPara);
                }
            }
            catch (System.Exception ex)
            {
                
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "Signuptemplate :: BLL signup.cs AddImageDetails", ex);
            }
            return res;
        }
        public int UpdateImageDetails()
        {
            int res = 0;
            try
            {
               /* CommonLib.SqlParameterArray param = new CommonLib.SqlParameterArray();
                param.Add("@ImageTitle", this.ImageTitle, SqlDbType.NVarChar, 200);
                param.Add("@ImageAlttext", this.ImageAlttext, SqlDbType.NVarChar, 200);
                param.Add("@ImageID", this.ImageID, SqlDbType.Int);*/


                SqlParameter[] msgPara ={ new SqlParameter("@ImageTitle",SqlDbType.NVarChar,200),
                                         new SqlParameter("@ImageAlttext",SqlDbType.NVarChar,200),                                      
                                         new SqlParameter("@ImageID",SqlDbType.Int)
                                        };
                msgPara[0].Value = this.ImageTitle;
                msgPara[1].Value = this.ImageAlttext;
                msgPara[2].Value = this.ImageID;
                System.Configuration.ConfigurationSettings.AppSettings["connString"] = Function.GetnetworkConnectionstring(this.NetworkID);
                
                using (CommonLib.DAL dal = new CommonLib.DAL())
                    res = dal.ExecuteNonQuery("SP_PW_O_SignupFormMaster_UpdateImageDetails", CommandType.StoredProcedure, msgPara);
            }
            catch (System.Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "Signuptemplate :: BLL signup.cs AddImageDetails", ex);
            }
            return res;
        }
        public DataSet GetsignupimageList(int siteid)
        {
            DataSet ds = new DataSet();
            try
            {
              /*  CommonLib.SqlParameterArray param = new CommonLib.SqlParameterArray();
                param.Add("@SiteID", siteid, SqlDbType.Int);*/
                //System.Web.HttpContext.Current.Response.Write("inside list" + this.NetworkID + "..<br>");
                SqlParameter[] msgPara ={                          
                                         new SqlParameter("@SiteID",SqlDbType.Int)
                                        };
                msgPara[0].Value = this.SiteID;
                System.Configuration.ConfigurationSettings.AppSettings["connString"] = Function.GetnetworkConnectionstring(this.NetworkID);
                using (CommonLib.DAL dal = new CommonLib.DAL())
                {
                    //System.Web.HttpContext.Current.Response.Write(dal.ConnString);
                    ds = dal.GetDataSet("SP_PW_O_SignupFormMaster_GetImageList", CommandType.StoredProcedure, msgPara);
                }
            }
            catch (Exception ex)
            {
                //System.Web.HttpContext.Current.Response.Write(ex.ToString());
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "Signuptemplate :: BLL signup.cs GetsignupimageList", ex);
            }
            return ds;
        }
        #endregion


        public bool DeleteImage()
        {
            bool boolReturn = false;
            int intid = 0;
            try
            {
                if (this.ImageIDS.Length > 0)
                {
                    System.Configuration.ConfigurationSettings.AppSettings["connString"] = Function.GetnetworkConnectionstring(this.NetworkID);
                    /*CommonLib.SqlParameterArray ParamArray = new CommonLib.SqlParameterArray();
                    ParamArray.Add("@imageid", this.ImageID, SqlDbType.Int);*/
                    string[] arr = this.ImageIDS.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        SqlParameter[] msgPara ={                          
                                         new SqlParameter("@imageid",SqlDbType.Int)
                                        };
                        msgPara[0].Value = arr[i].ToString();
                        
                        using (CommonLib.DAL dal = new CommonLib.DAL())
                            intid = Convert.ToInt32(dal.ExecuteNonQuery("SP_O_PW_SignupFormMaster_DeleteImage", CommandType.StoredProcedure, msgPara));
                        if (intid > 0)
                        {
                            boolReturn = true;
                        }
                        else
                        {
                            boolReturn = false;
                        }
                    }
                }
            }
            catch (System.Exception ex) { CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "signuptemplate signup.cs DeleteImage", ex); }
            finally { }
            return boolReturn;
        }

        #endregion
    }
}
