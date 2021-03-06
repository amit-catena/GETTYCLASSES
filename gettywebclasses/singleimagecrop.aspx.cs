﻿using System;
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
    public partial class singleimagecrop : System.Web.UI.Page
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
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string imageName = DateTime.Now.ToString("ddMMyyyyHHmmsss") + ".jpg";
                string dir = MapPath("~/" + Request.QueryString["dir"] + "/");

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
                  //  SaveThumbnail(imageName, dir);
                    Page.RegisterStartupScript("onsave", "<script>closePOP('" + imageName + "');</script>");
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

        private string SaveThumbnail(string img,string dir)
        {

            string imgpath = "";
            string albumPath = "";
            imgpath = dir+img;
            albumPath = dir;

            int jheight = System.Drawing.Image.FromFile(imgpath).Height;
            int jwidthAct = System.Drawing.Image.FromFile(imgpath).Width;
            double ratio = (double)jheight / (double)jwidthAct;
            int jwidth = 200;
            double calHeight = jwidth * ratio;

            string ImgFilePath = img;
            string uriName = Path.GetFileName(imgpath);
            Bitmap SourceBitmap = null;
            System.Drawing.Image thumbnail = null;
            string fnameHRW = "TN_" + uriName;
            Bitmap SourceBitmap1 = null;
            System.Drawing.Image thumbnail1 = null;
            try
            {
                SourceBitmap = new Bitmap(albumPath + uriName);
                thumbnail = SourceBitmap.GetThumbnailImage(jwidth, Convert.ToInt32(Math.Round(calHeight)), null, IntPtr.Zero);
                FileStream fs = File.Create(albumPath + fnameHRW);
                thumbnail.Save(fs, SourceBitmap.RawFormat);
                fs.Flush();
                fs.Close();
            }
            catch
            {
            }
            finally
            {
                SourceBitmap.GetThumbnailImage(jwidth, Convert.ToInt32(Math.Round(calHeight)), null, IntPtr.Zero).Dispose();
                thumbnail.Dispose();
                SourceBitmap.Dispose();
            }

            return "TN_" + img;
        }


        public void ValidateDateFolder()
        {
            try
            {
                string dir = ConfigurationSettings.AppSettings["newImagePath"];
                string foldername = Request.QueryString["dir"].ToString();               
                if (!Directory.Exists(dir + foldername))
                {
                    Directory.CreateDirectory(dir + foldername);
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