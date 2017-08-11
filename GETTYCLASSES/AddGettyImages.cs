using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.IO;
using CommonLib;

namespace Gettyclasses
{
    public class AddGettyImages : IDisposable
    {
        #region Members
        CommonLib.DAL ObjDal;
        #endregion

        public AddGettyImages()
        {
            ObjDal = new DAL();
        }

        #region :Dispose:
        public void Dispose()
        {
            //_DL = null;
            ObjDal.Dispose();
        }

        #endregion
        #region Properties
        public string imagename { get; set; }
        public string imagetxt { get; set; }
        public string imageurl { get; set; }
        public int NewsId { get; set; }
        public int NetworkId { get; set; }
        public string imagetxtbelow { get; set; }
        public string imagecredit { get; set; }
        public string RandomId { get; set; }
        public string SiteID { get; set; }
        public int CatID { get; set; }
        public string serverId { get; set; }
        public string Sitealias { get; set; }
        public string Sitename { get; set; }
        public string exturl { get; set; }
        public string ImageAddedDate { get; set; }
        public string IsNewImage { get; set; }
        public string IsImageServer { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public string EventDate { get; set; }
        public string eventImageURL { get; set; }
        public int ImageCount { get; set; }
        public int getImageCount { get; set; }
        #endregion

        public string GetnetworkID(string _ID)
        {
            string _nwtstring = string.Empty;
            try
            {
                switch (_ID)
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
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "Gettyimages-addimages-GetnetworkID-" + _ID, ex);

            }
            return _nwtstring;
        }

        public void GetsitenameandserverID()
        {

            string _strsitename = string.Empty;
            DataTable objDT = new DataTable();
            DataSet _ds = new DataSet();
            SqlConnection _newconn = new SqlConnection(ConfigurationSettings.AppSettings["gamingappstore"]);
            int cnt = 0;
            try
            {
                SqlCommand _spcommand = new SqlCommand("PR_SP_GetSitename_Servertype_Forgetty", _newconn);
                _spcommand.CommandType = CommandType.StoredProcedure;
                _spcommand.Parameters.Add("@SiteID", SqlDbType.Int).Value = this.SiteID;
                _newconn.Open();
                SqlDataAdapter da = new SqlDataAdapter(_spcommand);
                da.Fill(_ds);

                if (_ds != null)
                {
                    if (_ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow _dr in _ds.Tables[0].Rows)
                        {
                            _strsitename = _dr["SiteURL"].ToString();
                            serverId = _dr["serverid"].ToString();
                            Sitealias = _dr["SiteAlias"].ToString();
                            IsImageServer = _dr["IsImageServer"].ToString();
                        }
                    }

                }
                string[] strarr = _strsitename.Split('.');
                if (strarr.Length > 3)
                {
                    Sitename = string.Format("{0}-{1}", strarr[1].Trim(), strarr[3].Trim());
                }
                else
                {
                    Sitename = strarr[1];
                }
                _newconn.Close();

            }
            catch (Exception ex)
            {
                if (_newconn.State == ConnectionState.Open)
                {
                    _newconn.Close();

                }
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "Gettyimages-addimages-Getsitename-", ex);

            }




        }

        public bool AddexternalimagesURL()
        {
            DataTable objDT = new DataTable();
            SqlConnection _newconn = new SqlConnection(ConfigurationSettings.AppSettings["gamingappstore"]);
            int cnt = 0;
            try
            {

                SqlCommand _spcommand = new SqlCommand("PR_SP_Addexternalimagepostfromgetty", _newconn);
                _spcommand.CommandType = CommandType.StoredProcedure;
                _spcommand.Parameters.Add("@SiteID", SqlDbType.Int).Value = this.SiteID;
                _spcommand.Parameters.Add("@randomId", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(RandomId) ? RandomId : "00";
                _spcommand.Parameters.Add("@exturl", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(exturl) ? exturl : " ";
                _spcommand.Parameters.Add("@title", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imagetxt) ? imagetxt : " ";
                _spcommand.Parameters.Add("@desc", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imagetxtbelow) ? imagetxtbelow : " ";
                _spcommand.Parameters.Add("@credit", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imagecredit) ? imagecredit : " ";
                _newconn.Open();
                _spcommand.ExecuteNonQuery();
                _newconn.Close();

            }
            catch (Exception ex)
            {
                if (_newconn.State == ConnectionState.Open)
                {
                    _newconn.Close();

                }
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "Gettyimages-addimages-Addnewsimages-", ex);


            }

            return true;



        }
    }
}
