using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Configuration;
using System.Data;
using Gettyclasses;

namespace gettywebclasses
{
    public partial class addappscreenshots : System.Web.UI.Page
    {
        #region:variables:
        public string str = string.Empty;
        public string appId = "";
        public int siteid = 0;
        public string _data;
        public string cnt = "";
        public string filename;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["appId"] != null)
                {
                    appId = Request.QueryString["appId"];
                }
                if (Request.QueryString["siteid"] != null)
                {
                    siteid = Convert.ToInt32(Request.QueryString["siteid"]);
                }
            }
            catch (System.Exception ex) { CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "Page_Load", ex); }
            finally { }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            gettyclasses.AppImages objImg = new gettyclasses.AppImages();
            DataTable dt = null;
            string fname, imgthumbnail = "";
            int i = 1;
            string strImageDate = string.Empty;

            try
            {
                if (Request.QueryString["appId"] != null)
                {
                    appId = Request.QueryString["appId"];
                }
                if (Request.QueryString["siteid"] != null)
                {
                    siteid = Convert.ToInt32(Request.QueryString["siteid"]);
                }

                string dir = ConfigurationSettings.AppSettings["newImagePath"];
                dir = dir + "gamingappstore/AppImages/";

                if (!fup1.HasFile && !fup2.HasFile && !fup3.HasFile && !fup4.HasFile && !fup5.HasFile)
                {
                    lblMsg.Text = "Please browse the image to upload.";
                }
                else
                {
                    strImageDate = string.Format("{0}/{1}/", DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("MMMdd"));

                    #region:Image1:
                    if (fup1.HasFile)
                    {
                        if (fup1.PostedFile.ContentLength > 0)
                        {
                            string strError = "";
                            fname = Path.GetFileName(fup1.PostedFile.FileName);
                            // first increase the order of all images of Category by 1
                            if (fname != "")
                            {
                                dt = objImg.GetImageOrder(Convert.ToInt32(appId), siteid);
                                if (dt.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        int imgid = Convert.ToInt32(dt.Rows[j]["ImageID"].ToString());
                                        int imgorder = Convert.ToInt32(dt.Rows[j]["ImageOrder"].ToString());
                                        objImg.UpdateImgOrderByOne(imgid, imgorder + 1, siteid, Convert.ToInt32(appId));
                                    }
                                }
                                string ext = Path.GetExtension(fup1.PostedFile.FileName);
                                //fname = DateTime.Now.ToString("ddMMyyyyHHmmsss") + ext;
                                fname = CommonLib.FileHandler.GetUniqueFileName(dir, fname);
                                fup1.PostedFile.SaveAs(dir + fname);
                                fup1.PostedFile.SaveAs(dir + "org_" + fname);
                                string Tnname = Function.SaveThumbnailCompress("org_" + fname, fname, dir, "TN_", 100, 100);

                                objImg.InsertAppImages(appId, siteid, fname, txttitle1.Text.Trim(), txtimgurl1.Text.Trim(), txtimgcredit1.Text.Trim(), strImageDate);

                            }
                        }
                    }
                    #endregion
                    
                    #region:Image2:
                    if (fup2.HasFile)
                    {
                        if (fup2.PostedFile.ContentLength > 0)
                        {
                            string strError = "";
                            fname = Path.GetFileName(fup2.PostedFile.FileName);
                            if (fname != "")
                            {
                                dt = objImg.GetImageOrder(Convert.ToInt32(appId), siteid);
                                if (dt.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        int imgid = Convert.ToInt32(dt.Rows[j]["ImageID"].ToString());
                                        int imgorder = Convert.ToInt32(dt.Rows[j]["ImageOrder"].ToString());
                                        objImg.UpdateImgOrderByOne(imgid, imgorder + 1, siteid, Convert.ToInt32(appId));
                                    }
                                }
                                string ext = Path.GetExtension(fup2.PostedFile.FileName);
                                //fname = DateTime.Now.ToString("ddMMyyyyHHmmsss") + ext;
                                fname = CommonLib.FileHandler.GetUniqueFileName(dir, fname);
                                fup2.PostedFile.SaveAs(dir + fname);
                                fup2.PostedFile.SaveAs(dir + "org_" + fname);
                                string Tnname = Function.SaveThumbnailCompress("org_" + fname, fname, dir, "TN_", 100, 100);

                                objImg.InsertAppImages(appId, siteid, fname, txttitle2.Text.Trim(), txtimgurl2.Text.Trim(), txtimgcredit2.Text.Trim(), strImageDate);
                            }
                        }
                    }
                    #endregion

                    #region:Image3:
                    if (fup3.HasFile)
                    {
                        if (fup3.PostedFile.ContentLength > 0)
                        {
                            string strError = "";
                            fname = Path.GetFileName(fup3.PostedFile.FileName);
                            if (fname != "")
                            {
                                dt = objImg.GetImageOrder(Convert.ToInt32(appId), siteid);
                                if (dt.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        int imgid = Convert.ToInt32(dt.Rows[j]["ImageID"].ToString());
                                        int imgorder = Convert.ToInt32(dt.Rows[j]["ImageOrder"].ToString());
                                        objImg.UpdateImgOrderByOne(imgid, imgorder + 1, siteid, Convert.ToInt32(appId));
                                    }
                                }
                                string ext = Path.GetExtension(fup3.PostedFile.FileName);
                                //fname = DateTime.Now.ToString("ddMMyyyyHHmmsss") + ext;
                                fname = CommonLib.FileHandler.GetUniqueFileName(dir, fname);
                                fup3.PostedFile.SaveAs(dir + fname);
                                fup3.PostedFile.SaveAs(dir + "org_" + fname);
                                string Tnname = Function.SaveThumbnailCompress("org_" + fname, fname, dir, "TN_", 100, 100);

                                objImg.InsertAppImages(appId, siteid, fname, txttitle3.Text.Trim(), txtimgurl3.Text.Trim(), txtimgcredit3.Text.Trim(), strImageDate);

                            }
                        }
                    }
                    #endregion

                    #region:Image4:
                    if (fup4.HasFile)
                    {
                        if (fup4.PostedFile.ContentLength > 0)
                        {
                            string strError = "";
                            fname = Path.GetFileName(fup4.PostedFile.FileName);
                            if (fname != "")
                            {
                                dt = objImg.GetImageOrder(Convert.ToInt32(appId), siteid);
                                if (dt.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        int imgid = Convert.ToInt32(dt.Rows[j]["ImageID"].ToString());
                                        int imgorder = Convert.ToInt32(dt.Rows[j]["ImageOrder"].ToString());
                                        objImg.UpdateImgOrderByOne(imgid, imgorder + 1, siteid, Convert.ToInt32(appId));
                                    }
                                }
                                string ext = Path.GetExtension(fup4.PostedFile.FileName);
                                //fname = DateTime.Now.ToString("ddMMyyyyHHmmsss") + ext;
                                fname = CommonLib.FileHandler.GetUniqueFileName(dir, fname);
                                fup4.PostedFile.SaveAs(dir + fname);
                                fup4.PostedFile.SaveAs(dir + "org_" + fname);
                                string Tnname = Function.SaveThumbnailCompress("org_" + fname, fname, dir, "TN_", 100, 100);

                                objImg.InsertAppImages(appId, siteid, fname, txttitle4.Text.Trim(), txtimgurl4.Text.Trim(), txtimgcredit4.Text.Trim(), strImageDate);
                            }
                        }
                    }
                    #endregion

                    #region:Image5:
                    if (fup5.HasFile)
                    {
                        if (fup5.PostedFile.ContentLength > 0)
                        {
                            string strError = "";
                            fname = Path.GetFileName(fup2.PostedFile.FileName);
                            if (fname != "")
                            {
                                dt = objImg.GetImageOrder(Convert.ToInt32(appId), siteid);
                                if (dt.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        int imgid = Convert.ToInt32(dt.Rows[j]["ImageID"].ToString());
                                        int imgorder = Convert.ToInt32(dt.Rows[j]["ImageOrder"].ToString());
                                        objImg.UpdateImgOrderByOne(imgid, imgorder + 1, siteid, Convert.ToInt32(appId));
                                    }
                                }
                                string ext = Path.GetExtension(fup5.PostedFile.FileName);
                                //fname = DateTime.Now.ToString("ddMMyyyyHHmmsss") + ext;
                                fname = CommonLib.FileHandler.GetUniqueFileName(dir, fname);
                                fup5.PostedFile.SaveAs(dir + fname);
                                fup5.PostedFile.SaveAs(dir + "org_" + fname);
                                string Tnname = Function.SaveThumbnailCompress("org_" + fname, fname, dir, "TN_", 100, 100);

                                objImg.InsertAppImages(appId, siteid, fname, txttitle5.Text.Trim(), txtimgurl5.Text.Trim(), txtimgcredit5.Text.Trim(), strImageDate);
                            }
                        }
                    }
                    #endregion

                    Page.RegisterStartupScript("onsave", "<script>closePOP();</script>");
                }
            }
            catch (System.Exception ex) {
                Response.Write(ex);
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "BLL.AppImages.btnSubmit_Click", ex); 
            
            }
            finally { }
        }
    }
}