
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

namespace gettywebclasses
{
    public partial class SaveBinaryImage : System.Web.UI.Page
    {
         public string baseurl="";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                baseurl = ConfigurationSettings.AppSettings["baseurl"];
                if (!Page.IsPostBack)
                {
                    if (!string.IsNullOrEmpty(txtbinarydata.Value))
                    {
                        string imageName = "cp-"+DateTime.Now.ToString("ddMMyyyyHHmmsss") + ".jpg";
                        string dir = MapPath("../images/");
                        string imgpath = dir + imageName;
                        #region :: Base64 ::
                        string strImg = txtbinarydata.Value;
                        strImg = strImg.Replace("data:image/png;base64,", "");
                        if (strImg.Trim().Length > 0)
                            Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);

                        Page.RegisterStartupScript("onsave", "<script>GetImagePath('" + imageName + "');</script>");

                    }

                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "SaveBinaryImage.aspx.cs IsnotPostback", ex);
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

    }
    #endregion

}

 #endregion