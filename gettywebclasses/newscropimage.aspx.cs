using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using Gettyclasses;
using System.Drawing;



namespace gettywebclasses
{
    public partial class newscropimage : System.Web.UI.Page
    {
        public string _mstrImag64 = string.Empty;
        public string baseurl = ConfigurationSettings.AppSettings["baseurl"];
        public string topImagepath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {

                }
                else
                {
                    /*try
                    {
                        string imageName = DateTime.Now.ToString("ddMMyyyyHHmmsss") + ".jpg";
                        string dir = MapPath("~/" + Request.QueryString["sitename"] + "/" + GetImageFolderNameToUpload());

                        if (Request.Form["templateText3"] != null)
                        {
                            if (Request.Form["templateText3"].Trim().Length > 10)
                            {
                                string imgpath = dir + imageName;
                                #region :: Base64 ::
                                string strImg = Request.Form["templateText3"];
                                strImg = strImg.Replace("data:image/jpeg;base64,", "");
                                strImg = strImg.Replace("data:image/png;base64,", "");

                                if (strImg.Trim().Length > 0)
                                {
                                    ValidateDateFolder();
                                    Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    string Tnname = "";
                                    Tnname = Function.SaveThumbnailCompress(imageName, dir, "TN", 300, 225);
                                    Function.SaveThumbnailCompress(imageName, dir, "TN_TN", 128, 85);
                                    Page.RegisterStartupScript("onsave", "<script>closePOP('" + Tnname + "');</script>");
                                }
                                #endregion
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnSave_Click", ex);
                    }*/
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string imageName = DateTime.Now.ToString("ddMMyyyyHHmmsss") +".jpg";             
                string dir = MapPath("~/" + Request.QueryString["sitename"] + "/" + GetImageFolderNameToUpload());
               
                string imgpath = dir + imageName;
                #region :: Base64 ::
                string strImg = Request.Form["templateText3"];
                strImg = strImg.Replace("data:image/jpeg;base64,", "");
                strImg = strImg.Replace("data:image/png;base64,", "");
               
                if (strImg.Trim().Length > 0)
                {
                    ValidateDateFolder();
                    Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);                
                    string Tnname = "";
                    Tnname = Function.SaveThumbnailCompress(imageName, dir, "TN", 300, 225);
                    Function.SaveThumbnailCompress(imageName,dir, "TN_TN", 128, 85);
                    Page.RegisterStartupScript("onsave", "<script>closePOP('" + Tnname + "');</script>");
                }
                #endregion
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnSave_Click", ex);
            }
        }

        #region :: Base 64 functions ::
        public System.Drawing.Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
                imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }

        public string ImageToBase64(System.Drawing.Image image,
            System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
       
        #endregion


        public void ValidateDateFolder()
        {
            try
            {
                string dir = ConfigurationSettings.AppSettings["newImagePath"];
                string imagedate = GetImageFolderNameToUpload();
                string[] strarry = imagedate.Split('/');
                string monthyearfolder = strarry[0];
                string dayfolder = strarry[1];
                string sitename = Request.QueryString["sitename"].ToString();
                if (!Directory.Exists(dir + sitename + "/" + monthyearfolder + "/" + dayfolder))
                {

                    if (!Directory.Exists(dir + sitename))
                    {
                        Directory.CreateDirectory(dir + sitename);
                    }

                    if (!Directory.Exists(dir + sitename + "/" + monthyearfolder))
                    {
                        Directory.CreateDirectory(dir + sitename + "/" + monthyearfolder);
                    }
                    if (!Directory.Exists(dir + sitename + "/" + monthyearfolder + "/" + dayfolder))
                    {
                        Directory.CreateDirectory(dir + sitename + "/" + monthyearfolder + "/" + dayfolder);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "ValidateDateFolder", ex);
            }

        }

        public string GetImageFolderNameToUpload()
        {
            return string.Format("{0}/{1}/", DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("MMMdd"));
        }       
    }
}