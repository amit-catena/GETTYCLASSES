#region :: namespaces ::
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Web.Services;
using System.Configuration;
using Gettyclasses;
#endregion

namespace gettywebclasses
{
    public partial class UploadImageInTemlpate : System.Web.UI.Page
    {
        #region :: variables ::
        public string _imgserver = ConfigurationSettings.AppSettings["imgserver"];
        int siteid = 0;
        int userid = 0;
        public string networkid = string.Empty;
        bool imgserver = true;
        public string str = string.Empty;
        string imageName = string.Empty;
        string retval = string.Empty;
        string img1 = string.Empty;
        string modifieddate = string.Empty;
        string sitefldname = "signup";
        public System.Drawing.Image imgPhoto;
        int imageid = 0;
        string dir = string.Empty;
        string _imagepath = string.Empty;
        string monthyearfolder = DateTime.Now.ToString("yyyyMM");
        string dayfolder = DateTime.Now.ToString("MMMdd");
        string returnimagepath = "", returnimagename = "";
        string returnimagename_big = "";
        public string baseurl = ConfigurationSettings.AppSettings["baseurl"];
        string templateid;
        //string ImageServerURL = ConfigurationSettings.AppSettings["ImageServerURL"];
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (null != Request.QueryString["siteid"])
                {
                    siteid = Convert.ToInt32(Request.QueryString["siteid"].ToString());
                    Session["signup_siteid"] = siteid;
                }

                if (null != Request.QueryString["userid"])
                    userid = Convert.ToInt32(Request.QueryString["userid"].ToString());
                if (null != Request.QueryString["networkid"])
                {
                    networkid = Request.QueryString["networkid"].ToString();
                    Session["signup_networkid"] = networkid;
                }
                if (null != Request.QueryString["templateid"])
                {
                    templateid = Request.QueryString["templateid"].ToString();
                    //idserver_templateid.Value = templateid;
                    Session.Add("signup_templateid", templateid);
                }
                if (null != Session["signup_templateid"])
                    templateid = Session["signup_templateid"].ToString();

                idserver_templateid.Value = templateid;
                
                ConfigurationSettings.AppSettings["connString"] = Function.GetnetworkConnectionstring(Request.QueryString["networkid"]);

                dir = Server.MapPath(string.Format("{0}/", "signup")); //BLL.Constants.SaveImagePathSignUp;
                CommonLib.CurrentPage.LinkCSS("http://www.developersllc.com/signup/css/style.css");
                CommonLib.CurrentPage.IncludeScript("http://www.developersllc.com/signup/js/jquery-1.4.4.min.js");
               
                if (IsPostBack && FileUpload1.PostedFile != null)
                {                    
                    string FName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    FName = FName.Replace(' ', '_');
                    FName = CommonLib.FileHandler.GetUniqueFileName(dir, FName);

                    string imgExt = Path.GetExtension(FName);


                    if (imgExt != string.Empty && (imgExt.ToLower() == ".jpg" || imgExt.ToLower() == ".jpeg" || imgExt.ToLower() == ".gif" || imgExt.ToLower() == ".png"))
                    {
                        imageName = DateTime.Now.ToString("yyyyMMddHHmmss") + imgExt;
                    }
                    else
                    {
                        imageName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                    }
                    imgserver = true;
                    img1 = imageName;

                    modifieddate = DateTime.Now.ToString("yyyy/MM/dd");
                    ValidateSiteFolder(sitefldname);
                    CreateImagesFolderInBackend(sitefldname, Convert.ToDateTime(modifieddate).ToString("yyyyMM"), Convert.ToDateTime(modifieddate).ToString("MMMdd"));
                    dir = string.Format(dir + "{2}/{0}/{1}/", Convert.ToDateTime(modifieddate).ToString("yyyyMM"), Convert.ToDateTime(modifieddate).ToString("MMMdd"), sitefldname);
                    _imagepath = Server.MapPath(string.Format("{0}/{1}/{2}/", sitefldname, monthyearfolder, dayfolder));
                    using (FileStream fs = File.Create(_imagepath + imageName))
                    {
                        SaveFile(FileUpload1.PostedFile.InputStream, fs, imageName);                        
                        returnimagename = Function.SaveThumbnailCompress(imageName, _imagepath, "TN", 300, 170);
                        returnimagename_big = Function.SaveThumbnailCompress(imageName, _imagepath, "TN_TN", 600, 340);

                        if (templateid == "1")
                        {                           
                            returnimagepath = string.Format("{0}{1}/{2}/{3}/{4}", _imgserver, sitefldname, monthyearfolder, dayfolder, returnimagename);
                        }
                        else
                        {
                            returnimagepath = string.Format("{0}{1}/{2}/{3}/{4}", _imgserver, sitefldname, monthyearfolder, dayfolder, returnimagename_big);
                        }                        
                    }                    
                    if (!string.IsNullOrEmpty(returnimagepath))
                    {
                        using (Signup objsignup = new Signup())
                        {
                            objsignup.SiteID = Convert.ToInt32(siteid);
                            objsignup.NetworkID = networkid;
                            if (!string.IsNullOrEmpty(txttitle.Text))
                                objsignup.ImageTitle = txttitle.Text;
                            else
                                objsignup.ImageTitle = imageName;
                            objsignup.ImageAlttext = txtalttext.Text;
                            objsignup.ImageName = "TN" + imageName;
                            objsignup.ImageDate =  monthyearfolder + "/" + dayfolder + "/";
                            objsignup.AddedBy = Convert.ToInt32(userid);
                            objsignup.ImageID = Convert.ToInt32(idserver_image.Value);
                            if (objsignup.ImageID == 0)
                                imageid = objsignup.AddImageDetails();
                           /* else
                                objsignup.UpdateImageDetails();*/
                            
                            ltsignupimages.Text = LoadAllImages();
                            CommonLib.JavaScriptHandler.RegisterScriptForSM("ShowLoading('N');", true);
                        }
                    }

                }
                else
                {                   
                    ltsignupimages.Text = LoadAllImages();                   
                }
            }
            catch(Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "gettyclasses signup uploadimageintemplate pageload", ex);
            }
        }
        #region :: methods ::
        protected void DeleteImage(object sender, EventArgs e)
        {
            try
            {
                using (Signup objsignup = new Signup())
                {
                    objsignup.ImageID = Convert.ToInt32(idserver_image.Value);
                    bool flag = objsignup.DeleteImage();
                    idserver_image.Value = "0";
                    ltsignupimages.Text = LoadAllImages();
                    CommonLib.JavaScriptHandler.RegisterScriptForSM("ShowLoading('N');", true);
                }
            }
            catch(Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "gettyclasses signup uploadimageintemplate deleteimage", ex);
            }
        }
        public string LoadAllImages()
        {
            string allimages = string.Empty;
            DataSet ds = new DataSet();
            StringBuilder str = new StringBuilder();
            string imageurl = string.Empty;
            string bigimageurl = string.Empty;
            try
            {
                using (Signup objsignup = new Signup())
                {                    
                    int siteid = Convert.ToInt32(Session["signup_siteid"].ToString());                 
                    objsignup.NetworkID = Session["signup_networkid"].ToString();
                    ds = objsignup.GetsignupimageList(siteid);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {                            
                            str.Append("<ul id='ul_image'>");
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                DateTime dt = new DateTime();
                                dt = Convert.ToDateTime(dr["addedon"].ToString());
                                string date = dt.ToString("MMMM d, yyyy");
                                imageurl = ConfigurationSettings.AppSettings["baseurl"] + sitefldname + "/" + dr["imagedate"].ToString() + dr["imagename"].ToString();
                                bigimageurl = ConfigurationSettings.AppSettings["baseurl"] + sitefldname + "/" + dr["imagedate"].ToString() + "TN_" + dr["imagename"].ToString();
                                str.Append("<li data-date='" + date + "' data-alt='" + dr["ImageAlttext"].ToString() + "' data-name='" + dr["imagetitle"].ToString() + "' data-url='" + imageurl + "' data-bigurl='" + bigimageurl + "' id='" + dr["imageid"].ToString() + "'><img src='" + imageurl + "' border='0' width='200' height='200' /></li>");
                            }
                            str.Append("</ul>");
                        }
                    }
                    else
                    {                        
                    }
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "gettyclasses signup uploadimageintemplate loadallimages", ex);
            }
            allimages = str.ToString();
            return allimages;
        }
        
        public static bool ValidateSiteFolder(string sitefolder)
        {
            string folderpath = ConfigurationSettings.AppSettings["NewImagePath"];// NewImagePath
            bool flag = false;
            try
            {
                if (!Directory.Exists(string.Format("{0}{1}", folderpath, sitefolder)))
                {
                    Directory.CreateDirectory(string.Format("{0}{1}", folderpath, sitefolder));
                    flag = true;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "gettyclasses signup uploadimageintemplate validatesitefolder", ex);
            }
            return flag;
        }
        public static bool CreateImagesFolderInBackend(string sitefolder, string monthyearfolder, string dayfolder)
        {
            string folderpath = ConfigurationSettings.AppSettings["NewImagePath"];
            bool flag = false;
            try
            {
                if (!Directory.Exists(string.Format("{0}{1}\\{2}", folderpath, sitefolder, monthyearfolder)))
                {
                    Directory.CreateDirectory(string.Format("{0}{1}\\{2}", folderpath, sitefolder, monthyearfolder));
                }
                if (!Directory.Exists(string.Format("{0}{1}\\{2}\\{3}", folderpath, sitefolder, monthyearfolder, dayfolder)))
                {
                    Directory.CreateDirectory(string.Format("{0}{1}\\{2}\\{3}", folderpath, sitefolder, monthyearfolder, dayfolder));
                }
                flag = true;
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "gettyclasses signup uploadimageintemplate createimagesfolderinbackend", ex);
            }
            return flag;
        }
        private void SaveFile(Stream stream, FileStream fs, string FName)
        {
            try
            {

                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                }
                fs.Dispose();
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "gettyclasses signup uploadimageintemplate savefile", ex);
            }
        }
       
        #endregion
    }
}