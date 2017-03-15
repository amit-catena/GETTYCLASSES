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
    public partial class liveupdateigettymage : System.Web.UI.Page
    {
        /// <summary>
        /// The baseurl
        /// </summary>
        public string baseurl = "";
        /// <summary>
        /// The jsondata
        /// </summary>
        public string jsondata = "";
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>

        public System.Drawing.Image imgPhoto;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                baseurl = ConfigurationSettings.AppSettings["baseurl"];

                if (!Page.IsPostBack)
                {
                    /*
                    using (categoryimagemgmt objcat = new categoryimagemgmt())
                    {
                        if (Request.QueryString["catid"] != null)
                        {
                            //  jsondata = GetCategoryImage(Request.QueryString["catid"]);
                            ViewState["json"] = jsondata;
                        }
                        else
                            ViewState["json"] = jsondata;
                    }
                     */
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "eventimage.aspx.cs Pageload ", ex);
            }
             /*
            jsondata = ViewState["json"].ToString();
            if (string.IsNullOrEmpty(jsondata))
            {
                jsondata = "null";
            }
              */ 

        }


        /// <summary>
        /// Handles the Click event of the btnsave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnsave_Click(object sender, EventArgs e)
        {
            string imageName = "";
            try
            {
                int i = 0;
                using (categoryimagemgmt objcat = new categoryimagemgmt())
                {                   

                    if (hdnimage1.Value.Trim().Length > 0)
                    {
                         imageName = DateTime.Now.ToString("ddMMyyyyHHmmsss") + "_getty.jpg";
                        // string dir = MapPath("../superevent/");
                        string dir = MapPath("~/newsliveupdate/");
                        string imgpath = dir + imageName;
                        #region :: Base64 ::
                        string strImg = hdnimage1.Value;
                        strImg = strImg.Replace("data:image/png;base64,", "");
                        strImg = strImg.Replace("data:image/jpeg;base64,", "");
                        if (strImg.Trim().Length > 0)
                        {
                            Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            
                           
                        }
                        #endregion                   
                      
                        i++;
                    }
                }

                if (i > 0)
                {
                    Page.RegisterStartupScript("onsave", "<script>GetImage('" + imageName + "');window.close();</script>");
                    
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "eventimage.aspx.cs btnsave_Click ", ex);
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

       
    }
}