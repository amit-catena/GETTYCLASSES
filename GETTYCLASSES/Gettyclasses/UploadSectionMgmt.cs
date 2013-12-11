using System;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
namespace Gettyclasses
{
    /// <summary>
    /// Summary description for UploadSectionMgmt.
    /// </summary>
    public class UploadSectionMgmt
    {

        public string IpAddress = "", UserName = "", Password = "";
        public string Rootdir = "";
        string strtype = System.Configuration.ConfigurationSettings.AppSettings["_servertype"].ToString();

        public UploadSectionMgmt()
        {


            // TODO: Add constructor logic here


            if (strtype != "local")
            {


                IpAddress = System.Configuration.ConfigurationSettings.AppSettings["FTpRoot"].ToString();
                UserName = System.Configuration.ConfigurationSettings.AppSettings["FTPusername"].ToString();
                Password = System.Configuration.ConfigurationSettings.AppSettings["FTppassword"].ToString();
                Rootdir = System.Configuration.ConfigurationSettings.AppSettings["FTpRootdir"].ToString();

            }
            else
            {

                IpAddress = System.Configuration.ConfigurationSettings.AppSettings["FTpRoot"].ToString();
                UserName = System.Configuration.ConfigurationSettings.AppSettings["FTPusername"].ToString();
                Password = System.Configuration.ConfigurationSettings.AppSettings["FTppassword"].ToString();
                Rootdir = System.Configuration.ConfigurationSettings.AppSettings["FTpRootdir"].ToString();

            }
        }



        #region "Common Function"

        public bool ValidateImageFolder(string datetime, string sitename)
        {
            bool flagfolder = false;

            if (ValidateSitefolder(sitename, datetime))
            {
                flagfolder = true;
            }
            else
                flagfolder = false;


            return flagfolder;
        }


        public bool ValidateSitefolder(string sitename, string datetime)
        {
            bool flagsite = false;
            bool flagfolder = false;
            bool flagdayfolder = false;

            string folder = Convert.ToDateTime(datetime).ToString("yyyyMM");
            string dayfolder = Convert.ToDateTime(datetime).ToString("MMMdd");

            try
            {
                FtpClient ftp = new FtpClient(IpAddress, UserName, Password);
                ftp.RemotePath = "/" + Rootdir;
                ftp.Login();
                string[] dirlist;
                dirlist = ftp.GetFileList("/" + Rootdir);


                for (int i = 0; i < dirlist.Length; i++)
                {
                    if (dirlist[i].ToString() == sitename)
                    {
                        flagsite = true;
                        break;
                    }
                    else
                        flagsite = false;
                }

                if (!flagsite)
                {
                    ftp.MakeDir(sitename);
                    ftp.MakeDir(string.Format("/{0}{1}/{2}", Rootdir, sitename, folder));
                    ftp.MakeDir(string.Format("/{0}{1}/{2}/{3}", Rootdir, sitename, folder, dayfolder));
                    flagsite = true;
                }
                else
                {
                    //Check monthyearfolder
                    dirlist = ftp.GetFileList("/" + Rootdir + sitename + "/");
                    for (int i = 0; i < dirlist.Length; i++)
                    {
                        if (dirlist[i].ToString() == folder)
                        {
                            flagfolder = true;
                            flagsite = true;
                            break;
                        }
                        else
                            flagfolder = false;
                    }

                    if (!flagfolder)
                    {
                        ftp.MakeDir(string.Format("/{0}{1}/{2}", Rootdir, sitename, folder));
                        ftp.MakeDir(string.Format("/{0}{1}/{2}/{3}", Rootdir, sitename, folder, dayfolder));
                        flagfolder = true; flagsite = true; flagdayfolder = true;
                    }
                    else
                    {
                        //validate day folder
                        dirlist = ftp.GetFileList(string.Format("/{0}{1}/{2}/", Rootdir, sitename, folder));

                        for (int i = 0; i < dirlist.Length; i++)
                        {
                            if (dirlist[i].ToString() == dayfolder)
                            {
                                flagdayfolder = true; flagfolder = true; flagsite = true;
                                break;
                            }
                            else
                                flagdayfolder = false;
                        }

                        if (!flagdayfolder)
                        {
                            ftp.MakeDir(string.Format("/{0}{1}/{2}/{3}", Rootdir, sitename, folder, dayfolder));
                            flagfolder = true; flagfolder = true; flagsite = true;
                        }
                    }

                }
                ftp.Close();
                ftp = null;
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "UploadSectionMgmt :ValidateSitefolder", ex);

            }

            return flagsite;

        }




        #endregion

        #region "Image Transper Function"

        public bool ImageTransper(string datetime, string sitename, string imagename, string imagepath)
        {
            bool flagimagetransper = false;
            string monthyearfoldr = Convert.ToDateTime(datetime).ToString("yyyyMM");
            string dayfolder = Convert.ToDateTime(datetime).ToString("MMMdd");
            string strfilename = "";
            string t = "";
            bool result = false;


            try
            {


                if (ValidateImageFolder(datetime, sitename))
                {

                    strfilename = imagepath + imagename;


                    t = string.Format("/{0}/{1}/{2}/{3}/", Rootdir, sitename, monthyearfoldr, dayfolder);

                    result = Transfer_File(strfilename, IpAddress, UserName, Password, t);

                    strfilename = imagepath + "TN_" + imagename;
                    result = Transfer_File(strfilename, IpAddress, UserName, Password, t);

                    strfilename = imagepath + imagename.Substring(2, imagename.Length - 2);

                    result = Transfer_File(strfilename, IpAddress, UserName, Password, t);

                    flagimagetransper = true;
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "UploadSectionMgmt :ImageTransper", ex);
            }

            return flagimagetransper;
        }



        #endregion

        #region "Main Transper code function"

        public bool Transfer_File(string strfilename, string ip, string uname, string pwd, string root)
        {
            FtpClient objftp = new FtpClient(ip, uname, pwd);
            try
            {

                objftp.Timeout = 120;
                objftp.Port = 21;
                objftp.RemotePath = @"/" + root;
                objftp.Login();
                objftp.Upload(strfilename);
                objftp.Close();
            }
            catch (Exception ex)
            {

                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "UploadSectionMgmt :Transfer_File", ex);
                return false;
            }
            return true;
        }

        #endregion






    }
}
