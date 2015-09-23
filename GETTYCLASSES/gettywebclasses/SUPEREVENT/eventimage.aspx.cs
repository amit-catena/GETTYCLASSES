// ***********************************************************************
// Assembly         : gettywebclasses
// Author           : anil
// Created          : 09-15-2015
//
// Last Modified By : anil
// Last Modified On : 09-16-2015
// ***********************************************************************
// <copyright file="eventimage.aspx.cs" company="">
//     Copyright ©  2013
// </copyright>
// <summary></summary>
// ***********************************************************************
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

/// <summary>
/// The superevent namespace.
/// </summary>
namespace gettywebclasses.superevent
{
    /// <summary>
    /// Class eventimage.
    /// </summary>
    public partial class eventimage : System.Web.UI.Page
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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                baseurl = ConfigurationSettings.AppSettings["baseurl"];

                if (!Page.IsPostBack)
                {
                    if (Request.QueryString["supereventid"] != null)
                    {
                        jsondata = GetSuperEventImages(Request.QueryString["supereventid"]);
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
                using (supereventimagesmgmt objsuper = new supereventimagesmgmt())
                {

                    if (hdnimage1.Value.Trim().Length > 0)
                    {
                        string imageName = DateTime.Now.ToString("ddMMyyyyHHmmsss") + "1.jpg";
                        string dir = MapPath("../superevent/");
                        string imgpath = dir + imageName;
                        #region :: Base64 ::
                        string strImg = hdnimage1.Value;
                        strImg = strImg.Replace("data:image/png;base64,", "");
                        if (strImg.Trim().Length > 0)
                            Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        #endregion
                        objsuper.SaveSuperEventImages(Request.QueryString["supereventid"], imageName);
                        i++;

                    }

                    if (hdnimage2.Value.Trim().Length > 0)
                    {

                        string imageName = DateTime.Now.ToString("ddMMyyyyHHmmsss") + "2.jpg";
                        string dir = MapPath("../superevent/");
                        string imgpath = dir + imageName;
                        #region :: Base64 ::
                        string strImg = hdnimage2.Value;
                        strImg = strImg.Replace("data:image/png;base64,", "");
                        if (strImg.Trim().Length > 0)
                            Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        #endregion
                        objsuper.SaveSuperEventImages(Request.QueryString["supereventid"], imageName);
                        i++;

                    }
                    if (hdnimage3.Value.Trim().Length > 0)
                    {

                        string imageName = DateTime.Now.ToString("ddMMyyyyHHmmsss") + "3.jpg";
                        string dir = MapPath("../superevent/");
                        string imgpath = dir + imageName;
                        #region :: Base64 ::
                        string strImg = hdnimage3.Value;
                        strImg = strImg.Replace("data:image/png;base64,", "");
                        if (strImg.Trim().Length > 0)
                            Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        #endregion
                        objsuper.SaveSuperEventImages(Request.QueryString["supereventid"], imageName);
                        i++;
                    }

                    if (hdnimage4.Value.Trim().Length > 0)
                    {

                        string imageName = DateTime.Now.ToString("ddMMyyyyHHmmsss") + "4.jpg";
                        string dir = MapPath("../superevent/");
                        string imgpath = dir + imageName;
                        #region :: Base64 ::
                        string strImg = hdnimage4.Value;
                        strImg = strImg.Replace("data:image/png;base64,", "");
                        if (strImg.Trim().Length > 0)
                            Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        #endregion
                        objsuper.SaveSuperEventImages(Request.QueryString["supereventid"], imageName);
                        i++;
                    }
                    if (hdnimage5.Value.Trim().Length > 0)
                    {

                        string imageName = DateTime.Now.ToString("ddMMyyyyHHmmsss") + "5.jpg";
                        string dir = MapPath("../superevent/");                       
                        string imgpath = dir + imageName;
                        #region :: Base64 ::
                        string strImg = hdnimage5.Value;
                        strImg = strImg.Replace("data:image/png;base64,", "");
                        if (strImg.Trim().Length > 0)
                            Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        #endregion
                        objsuper.SaveSuperEventImages(Request.QueryString["supereventid"], imageName);
                        i++;
                    }
                }

                if (i > 0)
                {
                    Page.RegisterStartupScript("onsave", "<script>parent.closePOP();</script>");
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

        #region :: GetAllImages ::
        /// <summary>
        /// Gets the super event images.
        /// </summary>
        /// <param name="supereventid">The supereventid.</param>
        /// <returns>System.String.</returns>
        public string GetSuperEventImages(string supereventid)
        {
            string jsondata = "";
          
            DataTable dt = new DataTable();
            try
            {
                using (supereventimagesmgmt objsuper = new supereventimagesmgmt())
                {
                    dt = objsuper.GetSuperEventImages(supereventid);
                    if (dt.Rows.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.Append("{\"items\":[");
                        foreach (DataRow dr in dt.Rows)
                        {
                            sb.Append("{\"id\":" + dr["id"].ToString() + ","+"\"title\":\"" + dr["imagename"].ToString() + "\"},");

                        }
                        sb.Append("]}");
                        jsondata = sb.ToString();
                        jsondata = jsondata.Remove(jsondata.Length - 3, 1);	
                    }
                }
            }
            catch(Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "eventimage.aspx.cs GetSuperEventImages ", ex); 
            }
            return jsondata;
        }


        #endregion
    }
}