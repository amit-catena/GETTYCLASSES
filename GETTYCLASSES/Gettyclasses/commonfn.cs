#region Namespaces
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Globalization;
#endregion

namespace Gettyclasses
{
    public class commonfn
    {
        #region Constant members
        public static readonly  string FTPusername = ConfigurationSettings.AppSettings["FTPusername"];
        public static readonly string FTppassword = ConfigurationSettings.AppSettings["FTppassword"];
        public static readonly string FTpRootdir = ConfigurationSettings.AppSettings["FTpRootdir"];
        public static readonly string FTpRoot = ConfigurationSettings.AppSettings["FTpRoot"];
        public static readonly string _servertype = ConfigurationSettings.AppSettings["_servertype"];
        public static readonly string _imagepath = ConfigurationSettings.AppSettings["ImagePath"];
        public static readonly string _baseURL = ConfigurationSettings.AppSettings["baseurl"];
        public static readonly string _admin = ConfigurationSettings.AppSettings["Adminurl"];
        public static readonly string _uploadurl = ConfigurationSettings.AppSettings["uploadurl"];
        public static readonly string _imgserver = ConfigurationSettings.AppSettings["imgserver"];
        public static readonly int _defaultNetwork = Convert.ToInt32(ConfigurationSettings.AppSettings["_defaultNetwork"].ToString());
        //public static readonly CultureInfo cultures = new CultureInfo(ConfigurationSettings.AppSettings["dateCulture"].ToString());
        #endregion

        #region :Public Methods:
        public static void setSession(string sessionName, string sessionValue)
        {
            try
            {
                System.Web.HttpContext.Current.Session[sessionName] = sessionValue;
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "LSS commonfunctions.cs setSession", ex);
            }
        }

        public static string getSession(string sessionName)
        {
            string svar = "0";

            try
            {
                if (System.Web.HttpContext.Current.Session[sessionName] != null)
                    svar = System.Web.HttpContext.Current.Session[sessionName].ToString();
            }

            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "LSS commonfunctions.cs getSession", ex);
            }

            return svar;
        }

        public static void clearSession(string sessionName)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[sessionName] != null)
                    System.Web.HttpContext.Current.Session.Remove(sessionName);
            }

            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "LSS commonfunctions.cs clearSession", ex);
            }
            
        }

        public static bool ValidateIsNormalSave(bool imgserver,string _newsval)
        {
            bool flag = true;
            if (imgserver)
            {
                if (!string.IsNullOrEmpty(_newsval))
                {
                    flag = false;
                }
            }

            return flag;
        }

        public static bool Transfer_File(string strfilename, string ip, string uname, string pwd, string root)
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

                return false;
            }
            return true;
        }

        public static string GetConnectionstring(string Network_ID)
        {
            string _nwtstring = string.Empty;
            try
            {
                switch (Network_ID)
                {
                    case "1":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network1"].ToString();
                        break;
                    case "2":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network2"].ToString();
                        break;
                    case "3":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network3"].ToString();
                        break;
                    case "4":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network4"].ToString();
                        break;
                    case "5":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network5"].ToString();
                        break;
                    case "6":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network6"].ToString();
                        break;
                    case "7":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network7"].ToString();
                        break;
                    case "8":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network8"].ToString();
                        break;
                    case "9":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network9"].ToString();
                        break;
                    case "10":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network10"].ToString();
                        break;
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "Gettyimages-addimages-GetConnectionstring-" + Network_ID, ex);

            }
            return _nwtstring;
        }
        #endregion

    }
}
