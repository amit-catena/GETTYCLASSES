#region Namespaces
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
#endregion

namespace Gettyclasses
{
    public class addimages : IDisposable
    {

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

        #region Members
        CommonLib.DAL ObjDal;
        #endregion

        public addimages()
        {
            ObjDal = new DAL();
        }
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


        public bool Addnewsimages()
        {
            DataTable objDT = new DataTable();
            SqlConnection _newconn = new SqlConnection(GetnetworkID(NetworkId.ToString()));
            int cnt = 0;
            try
            {
                SqlCommand _spcommand = new SqlCommand("AK_SP_AddnewsimagesForslideshow", _newconn);
                _spcommand.CommandType = CommandType.StoredProcedure;
                _spcommand.Parameters.Add("@networkid", SqlDbType.Int).Value = NetworkId;
                _spcommand.Parameters.Add("@newsId", SqlDbType.Int).Value = NewsId;
                _spcommand.Parameters.Add("@imagename", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imagename) ? imagename : " ";
                _spcommand.Parameters.Add("@imagetxt", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imagetxt) ? imagetxt : " ";
                _spcommand.Parameters.Add("@imageURL", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imageurl) ? imageurl : " ";
                _spcommand.Parameters.Add("@ImageBelowTxt", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imagetxtbelow) ? imagetxtbelow : " ";
                _spcommand.Parameters.Add("@imagecredit", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imagecredit) ? imagecredit : " ";
                _spcommand.Parameters.Add("@IsNewImage", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(IsNewImage) ? IsNewImage : "N";
                _spcommand.Parameters.Add("@ImageDate", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(ImageAddedDate) ? ImageAddedDate : "N";
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


        public bool Addnewssingleimages()
        {
            DataTable objDT = new DataTable();
            SqlConnection _newconn = new SqlConnection(GetnetworkID(NetworkId.ToString()));
            int cnt = 0;
            try
            {
                SqlCommand _spcommand = new SqlCommand("AK_SP_AddSinglegettyimage", _newconn);
                _spcommand.CommandType = CommandType.StoredProcedure;
                _spcommand.Parameters.Add("@RandomId", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(RandomId) ? RandomId : " ";
                _spcommand.Parameters.Add("@SelectnewsImage", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imagename) ? imagename : " ";
                _spcommand.Parameters.Add("@sitId", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(SiteID) ? SiteID : " ";
                _spcommand.Parameters.Add("@Gettyurl", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imageurl) ? imageurl : " ";
                _spcommand.Parameters.Add("@ImageDesc", SqlDbType.NVarChar).Value = !string.IsNullOrEmpty(imagetxtbelow) ? imagetxtbelow : " ";
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
        public bool AddCategoryimages()
        {
            DataTable objDT = new DataTable();
            SqlConnection _newconn = new SqlConnection(GetnetworkID(NetworkId.ToString()));
            int cnt = 0;
            try
            {
                SqlCommand _spcommand = new SqlCommand("AK_SP_AddCategoryimagesForslideshow", _newconn);
                _spcommand.CommandType = CommandType.StoredProcedure;
                _spcommand.Parameters.Add("@networkid", SqlDbType.Int).Value = NetworkId;
                _spcommand.Parameters.Add("@catsId", SqlDbType.Int).Value = CatID;
                _spcommand.Parameters.Add("@SiteID", SqlDbType.Int).Value = this.SiteID;
                _spcommand.Parameters.Add("@imagename", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imagename) ? imagename : " ";
                _spcommand.Parameters.Add("@imagetxt", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imagetxt) ? imagetxt : " ";
                _spcommand.Parameters.Add("@imageURL", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imageurl) ? imageurl : " ";
                _spcommand.Parameters.Add("@ImageBelowTxt", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imagetxtbelow) ? imagetxtbelow : " ";
                _spcommand.Parameters.Add("@imagecredit", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imagecredit) ? imagecredit : " ";
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

        public string Getsitename()
        {

            string _strsitename = string.Empty;
            DataTable objDT = new DataTable();
            SqlConnection _newconn = new SqlConnection(GetnetworkID(NetworkId.ToString()));
            int cnt = 0;
            try
            {
                SqlCommand _spcommand = new SqlCommand("AK_SP_GetSitename_Forgetty", _newconn);
                _spcommand.CommandType = CommandType.StoredProcedure;
                _spcommand.Parameters.Add("@SiteID", SqlDbType.Int).Value = this.SiteID;
                _newconn.Open();
                _strsitename = (string)_spcommand.ExecuteScalar();
                string[] strarr = _strsitename.Split('.');
                if (strarr.Length > 3)
                {
                    _strsitename = string.Format("{0}-{1}", strarr[1].Trim(), strarr[3].Trim());
                }
                else
                {
                    _strsitename = strarr[1];
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

            return _strsitename;



        }



        public void GetsitenameandserverID()
        {

            string _strsitename = string.Empty;
            DataTable objDT = new DataTable();
            DataSet _ds = new DataSet();
            SqlConnection _newconn = new SqlConnection(GetnetworkID(NetworkId.ToString()));
            int cnt = 0;
            try
            {
                SqlCommand _spcommand = new SqlCommand("AK_SP_GetSitename_Servertype_Forgetty", _newconn);
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


        public string GetCategoryename()
        {

            string _strcatname = string.Empty;
            DataTable objDT = new DataTable();
            SqlConnection _newconn = new SqlConnection(GetnetworkID(NetworkId.ToString()));
            int cnt = 0;
            try
            {
                SqlCommand _spcommand = new SqlCommand("AK_SP_GetCategoryename_Forgetty", _newconn);
                _spcommand.CommandType = CommandType.StoredProcedure;
                _spcommand.Parameters.Add("@SiteID", SqlDbType.Int).Value = this.SiteID;
                _spcommand.Parameters.Add("@catID", SqlDbType.Int).Value = this.CatID;
                _newconn.Open();
                _strcatname = (string)_spcommand.ExecuteScalar();
                _newconn.Close();

            }
            catch (Exception ex)
            {
                if (_newconn.State == ConnectionState.Open)
                {
                    _newconn.Close();

                }
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "Gettyimages-addimages-GetCategoryename-", ex);

            }

            return _strcatname;



        }



        public bool AddexternalimagesURL()
        {
            DataTable objDT = new DataTable();
            SqlConnection _newconn = new SqlConnection(GetnetworkID(NetworkId.ToString()));
            int cnt = 0;
            try
            {

                SqlCommand _spcommand = new SqlCommand("AK_SP_Addexternalimagepostfromgetty", _newconn);
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


        public bool AddGettyEvents()
        {
            DataTable objDT = new DataTable();
            SqlConnection _newconn = new SqlConnection(GetnetworkID(NetworkId.ToString()));
            int cnt = 0;
            try
            {
                SqlCommand _spcommand = new SqlCommand("AK_SP_AddGettyEvent", _newconn);
                _spcommand.CommandType = CommandType.StoredProcedure;
                _spcommand.Parameters.Add("@eventId", SqlDbType.VarChar).Value = EventId;
                _spcommand.Parameters.Add("@imageurl", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(eventImageURL) ? eventImageURL : "";
                _spcommand.Parameters.Add("@title", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(EventTitle) ? EventTitle : "";
                _spcommand.Parameters.Add("@eventdate", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(EventDate) ? EventDate : "";
                _spcommand.Parameters.Add("@imagecount", SqlDbType.VarChar).Value = ImageCount;
                _spcommand.Parameters.Add("@imagecaption", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(imagetxtbelow) ? imagetxtbelow : "";
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


        public DataTable GetAddedgettyevents()
        {
            DataTable objDT = new DataTable();
            SqlConnection _newconn = new SqlConnection(GetnetworkID(NetworkId.ToString()));
            SqlDataAdapter _sdlda;
            DataSet _ds = new DataSet();  
            int cnt = 0;
            try
            {
                SqlCommand _spcommand = new SqlCommand("AK_SP_GetGettyEvents", _newconn);
                _spcommand.CommandType = CommandType.StoredProcedure;
                _newconn.Open();
                _sdlda = new SqlDataAdapter(_spcommand);
                _sdlda.Fill(objDT);   
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

            return objDT;

        }
        public bool UpdateGettyEvents(string _srtIDs)
        {
            DataTable objDT = new DataTable();
            SqlConnection _newconn = new SqlConnection(GetnetworkID(NetworkId.ToString()));
            int cnt = 0;
            try
            {
                SqlCommand _spcommand = new SqlCommand("AK_SP_UpdategettyEvents", _newconn);
                _spcommand.CommandType = CommandType.StoredProcedure;
                _spcommand.Parameters.Add("@eventId", SqlDbType.VarChar).Value = _srtIDs;
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


        public DataTable GetGettyevents_toupdatecount()
        {
            DataTable objDT = new DataTable();
            SqlConnection _newconn = new SqlConnection(GetnetworkID(NetworkId.ToString()));
            SqlDataAdapter _sdlda;
            DataSet _ds = new DataSet();
            int cnt = 0;
            try
            {
                SqlCommand _spcommand = new SqlCommand("AK_SP_GettyEvents_to_updateimagecount", _newconn);
                _spcommand.CommandType = CommandType.StoredProcedure;
                _newconn.Open();
                _sdlda = new SqlDataAdapter(_spcommand);
                _sdlda.Fill(objDT);
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

            return objDT;

        }


        public bool Update_GettyEvents_imagecount(int imagecount,int ID)
        {
            DataTable objDT = new DataTable();
            SqlConnection _newconn = new SqlConnection(GetnetworkID(NetworkId.ToString()));
            try
            {
                SqlCommand _spcommand = new SqlCommand("AK_SP_UpdategettyEventsimagecount", _newconn);
                _spcommand.CommandType = CommandType.StoredProcedure;
                _spcommand.Parameters.Add("@eventId", SqlDbType.Int).Value = ID;
                _spcommand.Parameters.Add("@imagecount", SqlDbType.Int).Value = imagecount;
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
        public bool checkGettyEvents(int _valevent)
        {
            DataTable objDT = new DataTable();
            SqlConnection _newconn = new SqlConnection(GetnetworkID(NetworkId.ToString()));
            int cnt = 0;
            string _checkval = string.Empty;
            try
            {
                /*SqlParameter[] msgpara = { new SqlParameter("@eventId", SqlDbType.Int),
                                           new SqlParameter("@retval", SqlDbType.VarChar)
                                         };
                msgpara[0].Value = _valevent;
                msgpara[1].Value = "";
                msgpara[1].Direction = ParameterDirection.Output;*/
                SqlCommand _spcommand = new SqlCommand("AK_SP_CheckGettyEvent", _newconn);
                _spcommand.CommandType = CommandType.StoredProcedure;
                _spcommand.Parameters.Add("@eventId", SqlDbType.Int).Value = _valevent;
                SqlParameter parm3 = new SqlParameter("@retval", SqlDbType.VarChar);
                parm3.Value = "false";  
                parm3.Direction = ParameterDirection.Output;
                _spcommand.Parameters.Add(parm3);
                _newconn.Open();
                //_spcommand.ExecuteNonQuery();
                _checkval = (string)_spcommand.ExecuteScalar();
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
            finally
            {
                if (_newconn.State == ConnectionState.Open)
                {
                    _newconn.Close();
                }

            }
            if (_checkval == "true")
            {
                return true;

            }
            else
            {
                return false;
            }
        }





        #region :Dispose:
        public void Dispose()
        {
            //_DL = null;
            ObjDal.Dispose();
        }

        #endregion

    }
}
