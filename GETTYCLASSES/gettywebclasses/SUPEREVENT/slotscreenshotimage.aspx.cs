using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Gettyclasses;
using System.Drawing;
using System.IO;
using System.Data;
using System.Text;
using CommonLib;

namespace gettywebclasses.superevent
{
    public partial class slotscreenshotimage : System.Web.UI.Page
    {
        /// <summary>
        /// The baseurl
        /// </summary>
        public string baseurl = "";
        /// <summary>
        /// The jsondata
        /// </summary>
        public string jsondata = "";
        public string jsonuserdata = "";
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                baseurl = ConfigurationSettings.AppSettings["baseurl"];

                if (!Page.IsPostBack)
                {
                  //  jsonuserdata = GetDummyUsers();
                    if (Request.QueryString["slotid"] != null)
                    {
                        jsondata = GetScreenShotImages(Request.QueryString["slotid"]);
                        ViewState["json"] = jsondata;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "eventimage.aspx.cs Pageload ", ex);
            }
            jsondata = ViewState["json"].ToString();
            if (string.IsNullOrEmpty(jsondata))
            {
                jsondata = "null";
            }

        }


        /// <summary>
        /// Handles the Click event of the btnsave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                using (slotmgmt obj = new slotmgmt())
                {

                    if (hdnimage1.Value.Trim().Length > 0)
                    {
                        string imageName ="Screenshot_"+ DateTime.Now.ToString("ddMMyyyyHHmmsss") + "1.jpg";
                        string dir = MapPath("../commonreview/");
                        string imgpath = dir + imageName;
                        #region :: Base64 ::
                        string strImg = hdnimage1.Value;
                        strImg = strImg.Replace("data:image/png;base64,", "");
                        strImg = strImg.Replace("data:image/jpeg;base64,", "");
                        if (strImg.Trim().Length > 0)
                            Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        #endregion
                        obj.SaveSlotScreenShotImages(Request.QueryString["slotid"], imageName);
                        i++;
                        SaveThumbnail(imageName);

                    }

                    if (hdnimage2.Value.Trim().Length > 0)
                    {

                        string imageName ="Screenshot_"+ DateTime.Now.ToString("ddMMyyyyHHmmsss") + "2.jpg";
                        string dir = MapPath("../commonreview/");
                        string imgpath = dir + imageName;
                        #region :: Base64 ::
                        string strImg = hdnimage2.Value;
                        strImg = strImg.Replace("data:image/png;base64,", "");
                        strImg = strImg.Replace("data:image/jpeg;base64,", "");
                        if (strImg.Trim().Length > 0)
                            Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        #endregion
                        obj.SaveSlotScreenShotImages(Request.QueryString["slotid"], imageName);
                        i++;
                        SaveThumbnail(imageName);

                    }
                    if (hdnimage3.Value.Trim().Length > 0)
                    {

                        string imageName ="Screenshot_"+ DateTime.Now.ToString("ddMMyyyyHHmmsss") + "3.jpg";
                        string dir = MapPath("../commonreview/");
                        string imgpath = dir + imageName;
                        #region :: Base64 ::
                        string strImg = hdnimage3.Value;
                        strImg = strImg.Replace("data:image/png;base64,", "");
                        strImg = strImg.Replace("data:image/jpeg;base64,", "");
                        if (strImg.Trim().Length > 0)
                            Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        #endregion
                        obj.SaveSlotScreenShotImages(Request.QueryString["slotid"], imageName);
                        i++;
                        SaveThumbnail(imageName);
                    }

                    if (hdnimage4.Value.Trim().Length > 0)
                    {

                        string imageName ="Screenshot_"+ DateTime.Now.ToString("ddMMyyyyHHmmsss") + "4.jpg";
                        string dir = MapPath("../commonreview/");
                        string imgpath = dir + imageName;
                        #region :: Base64 ::
                        string strImg = hdnimage4.Value;
                        strImg = strImg.Replace("data:image/png;base64,", "");
                        strImg = strImg.Replace("data:image/jpeg;base64,", "");
                        if (strImg.Trim().Length > 0)
                            Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        #endregion
                        obj.SaveSlotScreenShotImages(Request.QueryString["slotid"], imageName);
                        i++;
                        SaveThumbnail(imageName);
                    }
                    if (hdnimage5.Value.Trim().Length > 0)
                    {

                        string imageName ="Screenshot_"+ DateTime.Now.ToString("ddMMyyyyHHmmsss") + "5.jpg";
                        string dir = MapPath("../commonreview/");
                        string imgpath = dir + imageName;
                        #region :: Base64 ::
                        string strImg = hdnimage5.Value;
                        strImg = strImg.Replace("data:image/png;base64,", "");
                        strImg = strImg.Replace("data:image/jpeg;base64,", "");
                        if (strImg.Trim().Length > 0)
                            Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        #endregion
                        obj.SaveSlotScreenShotImages(Request.QueryString["slotid"], imageName);
                        i++;
                        SaveThumbnail(imageName);
                    }
                }

                if (i > 0)
                {
                    Page.RegisterStartupScript("onsave", "<script>closePOP();</script>");
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "slotscrrenshotimages.aspx.cs btnsave_Click ", ex);
            }


        }

        #region :: Base 64 functions ::
        /// <summary>
        /// Base64s to image.
        /// </summary>
        /// <param name="base64String">The base64 string.</param>
        /// <returns>System.Drawing.Image.</returns>
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

        /// <summary>
        /// Images to base64.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="format">The format.</param>
        /// <returns>System.String.</returns>
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

        #region :: GetAllImages ::
        /// <summary>
        /// Gets the super event images.
        /// </summary>
        /// <param name="supereventid">The supereventid.</param>
        /// <returns>System.String.</returns>
        public string GetScreenShotImages(string slotid)
        {
            string jsondata = "";

            DataTable dt = new DataTable();
            try
            {
                using (slotmgmt obj=new slotmgmt())
                {
                    dt = obj.GetSlotScreenShotImages(slotid);
                    if (dt.Rows.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.Append("{\"items\":[");
                        foreach (DataRow dr in dt.Rows)
                        {
                            sb.Append("{\"id\":" + dr["id"].ToString() + "," + "\"title\":\"" + dr["imagename"].ToString() + "\"},");

                        }
                        sb.Append("]}");
                        jsondata = sb.ToString();
                        jsondata = jsondata.Remove(jsondata.Length - 3, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "slotscrrenshotimages.aspx.cs GetScreenShotImages ", ex);
            }
            return jsondata;
        }


        public string GetDummyUsers()
        {
            string jsondata = "";

            DataTable dt = new DataTable();
            try
            {
                using (slotmgmt obj = new slotmgmt())
                {
                    dt = obj.GetDummyUsers();
                    if (dt.Rows.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.Append("{\"items\":[");
                        foreach (DataRow dr in dt.Rows)
                        {
                            sb.Append("{\"id\":" + dr["UserID"].ToString() + "," + "\"title\":\"" + dr["name"].ToString() + "\"},");

                        }
                        sb.Append("]}");
                        jsondata = sb.ToString();
                        jsondata = jsondata.Remove(jsondata.Length - 3, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "slotscrrenshotimages.aspx.cs GetDummyUsers ", ex);
            }
            return jsondata;
        }

        private string SaveThumbnail(string img)
        {

            string imgpath = "";
            string albumPath = "";
            imgpath = MapPath("../commonreview/" + img);
            albumPath = MapPath("../commonreview/");
            
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



        #endregion
    }
}