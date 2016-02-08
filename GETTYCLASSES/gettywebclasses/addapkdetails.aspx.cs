using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLib;
using Gettyclasses;
using System.IO;

namespace gettywebclasses
{
    public partial class addapkdetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ValidateDateFolder();
        }
        protected void btnsubmit_click(object sender, EventArgs e)
        {
            try
            {
                using (apkadddetailsmgmt obj = new apkadddetailsmgmt())
                {                   
                    obj.ApkFileName = txtApktitle.Text;
                    if (Apkfileupload.PostedFile.ContentLength > 0)
                    {

                        ValidateDateFolder();
                        string dir = Server.MapPath("../");
                        string imagedate = GetImageFolderNameToUpload();
                        string[] strarry = imagedate.Split('/');
                        string monthyearfolder = strarry[0];
                        string dayfolder = strarry[1];
                        string filename=DateTime.Now.ToString("yyyyMMddHHmmss") + ".apk";
                        Apkfileupload.PostedFile.SaveAs(dir + "gamingappstore/APK/" + monthyearfolder + "/" + dayfolder+"/"+filename );
                        obj.LinkReference = "http://www/pix123.com/gamingappstore/APK/" + monthyearfolder + "/" + dayfolder + "/" + filename;
                        obj.Version = txtversion.Text;
                        obj.LinkName=txtApktitle.Text+"-"+obj.Version;
                        obj.ApklinkId=obj.SaveApkLink();
                        //Apk details
                        
                        obj.Size = txtSize.Text;
                        obj.Description = txtdesc.Text;
                        obj.AppmasterId = Convert.ToInt32(Request.QueryString["appid"]);
                        obj.imagedate = imagedate;
                        obj.Addedby = Convert.ToInt32(Request.QueryString["userid"]);
                        obj.SaveApkDetails();
                        
                    }
                    Page.RegisterStartupScript("onsave", "<script>closePOP();</script>");
                    

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
                string dir = Server.MapPath("../");
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

                    else if (!Directory.Exists(dir + "gamingappstore/APK"))
                    {
                        Directory.CreateDirectory(dir + "gamingappstore/APK");
                    }
                    else if (!Directory.Exists(dir + "gamingappstore/APK/" + monthyearfolder))
                    {
                        Directory.CreateDirectory(dir + "gamingappstore/APK/" + monthyearfolder);
                    }
                    else if (!Directory.Exists(dir + "gamingappstore/APK/" + monthyearfolder + "/" + dayfolder))
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