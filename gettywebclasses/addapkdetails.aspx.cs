using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLib;
using Gettyclasses;
using System.IO;
using System.Data;
using System.Configuration;

namespace gettywebclasses
{
    public partial class addapkdetails : System.Web.UI.Page
    {
         public int tmpapklinkid= 0;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["apkid"].ToString() != "0")
                {
                    GetDetails(Request.QueryString["apkid"].ToString());
                }
               
            }
        }
        protected void btnsubmit_click(object sender, EventArgs e)
        {
            try
            {
                using (apkadddetailsmgmt obj = new apkadddetailsmgmt())
                {  
                    string imagedate="";
                    string monthyearfolder = "";
                    string dayfolder = "";
                    string filename = "";
                    obj.ApkFileName = txtApktitle.Text;
                    if (Apkfileupload.PostedFile.ContentLength > 0)
                    {

                        ValidateDateFolder();
                        string dir = ConfigurationSettings.AppSettings["newImagePath"];
                        imagedate = GetImageFolderNameToUpload();
                        string[] strarry = imagedate.Split('/');
                         monthyearfolder = strarry[0];
                         dayfolder = strarry[1];
                         filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".apk";
                         Apkfileupload.PostedFile.SaveAs(dir + "gamingappstore/APK/" + monthyearfolder + "/" + dayfolder + "/" + filename);
                         obj.filename = filename;
                         obj.imagedate = imagedate;
                    }
                    else
                    {
                        if (Request.QueryString["apkid"].ToString() != "0")
                        {
                            if (ViewState["filename"] != null)
                            {
                                obj.filename = ViewState["filename"].ToString();

                            }
                            if (ViewState["imagedate"] != null)
                            {
                                obj.imagedate = ViewState["imagedate"].ToString();
                            }
                        }

                     }

                         obj.LinkReference = "http://www.pix123.com/gamingappstore/APK/" + monthyearfolder + "/" + dayfolder + "/" + obj.filename;
                         obj.Version = txtversion.Text;
                         obj.LinkName = txtApktitle.Text + "-" + obj.Version;
                         obj.LinkId = tmpapklinkid;
                         if (!string.IsNullOrEmpty(obj.filename))
                         {
                             obj.ApklinkId = obj.SaveApkLink();
                         }
                         else
                         {
                             obj.ApklinkId = 0;
                         }
                          
                        //Apk details
                       
                        obj.Size = txtSize.Text;
                        obj.Description = txtdesc.Text;
                        obj.AppmasterId = Convert.ToInt32(Request.QueryString["appid"]);
                       
                        obj.Addedby = Convert.ToInt32(Request.QueryString["userid"]);
                        if (Request.QueryString["apkid"] != "0")
                        {
                            obj.ApkFileId = Convert.ToInt32(Request.QueryString["apkid"]);
                        }
                        else
                        {
                            obj.ApkFileId = 0;
                        }

                        obj.SaveApkDetails();
                    }               
                        
                    
                     Page.RegisterStartupScript("onsave", "<script>closePOP();</script>");
                    

                
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, " ", ex);   
            }
        }

        public void GetDetails(string apkid)
        {
            DataTable dt = new DataTable();
            try
            {
                using (apkadddetailsmgmt obj = new apkadddetailsmgmt())
                {
                    dt = obj.GetApkDetails(apkid);
                    if (dt.Rows.Count > 0)
                    {
                        txtApktitle.Text = dt.Rows[0]["apkfilename"].ToString();
                        txtSize.Text = dt.Rows[0]["size"].ToString();
                        txtversion.Text = dt.Rows[0]["version"].ToString();
                        txtdesc.Text = dt.Rows[0]["description"].ToString();
                        ltfilename.Text = dt.Rows[0]["filename"].ToString();
                        ViewState["filename"] = dt.Rows[0]["filename"].ToString();
                        ViewState["imagedate"] = dt.Rows[0]["imagedate"].ToString();
                        tmpapklinkid = Convert.ToInt32(dt.Rows[0]["apklinkid"]);

                    }

                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, " ", ex);
            }

        }

        public static string GetImageFolderNameToUpload()
        {
            return string.Format("{0}/{1}/", DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("MMMdd"));
        }

        public void ValidateDateFolder()
        {
            try
            {
                string dir = ConfigurationSettings.AppSettings["newImagePath"];
                string imagedate = GetImageFolderNameToUpload();
                string[] strarry = imagedate.Split('/');
                string monthyearfolder = strarry[0];
                string dayfolder = strarry[1];
                if (!Directory.Exists(dir + "gamingappstore/APK/" + monthyearfolder + "/" +dayfolder))
                {

                    if (!Directory.Exists(dir + "gamingappstore"))
                    {
                        Directory.CreateDirectory(dir + "gamingappstore");
                    }

                    if (!Directory.Exists(dir + "gamingappstore/APK"))
                    {
                        Directory.CreateDirectory(dir + "gamingappstore/APK");
                    }
                    if (!Directory.Exists(dir + "gamingappstore/APK/" + monthyearfolder))
                    {
                        Directory.CreateDirectory(dir + "gamingappstore/APK/" + monthyearfolder);
                    }
                    if (!Directory.Exists(dir + "gamingappstore/APK/" + monthyearfolder + "/" + dayfolder))
                    {
                        Directory.CreateDirectory(dir + "gamingappstore/APK/" + monthyearfolder + "/" + dayfolder);
                    }
                }
                

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, " ", ex);
            }


        }
    }
}