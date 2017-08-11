using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gettyclasses;
using System.Data;
using System.Data.SqlClient;
using ConnectExample.Samples;
using System.Net;
using System.Configuration;
using CommonLib;
using System.Web.UI.HtmlControls;
using System.Web.Services;

namespace gettywebclasses
{
    public partial class geteventimages : System.Web.UI.Page
    {
        int eventID;
        string _imagepathdownload = ConfigurationSettings.AppSettings["ImagePath"];
        string _baseURL = ConfigurationSettings.AppSettings["baseurl"];
        public int _networkid = 1;
        public int _intnewsID = 0;
        public string _strcookiename = string.Empty;
        public string _strSiteID = string.Empty;
        public string _strnews = string.Empty;
        public static string _redirecturl = string.Empty;
        public static int _intstartval;
        public string _strjson = string.Empty;
        public static List<String> _lilist = new List<String>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (null != Request.QueryString["NwtID"])
            {
                _networkid = Function.GetnetworkID(Request.QueryString["NwtID"].ToString());
            }

            if (null != Request.QueryString["eventid"])
            {
                eventID = Convert.ToInt32(Request.QueryString["eventid"].ToString());
            }
            if (null != Request.QueryString["SiteId"])
            {
                _strSiteID = Request.QueryString["SiteId"].ToString();
            }
            if (null != Request.QueryString["randomID"])
            {
                _strcookiename = Request.QueryString["randomID"].ToString();
            }
            if (null != Request.QueryString["NewsID"])
            {
                _intnewsID = Convert.ToInt32(Request.QueryString["NewsID"].ToString());
            }
            if (null != Request.QueryString["url"])
            {
                _redirecturl = HttpUtility.UrlDecode(Request.QueryString["url"].ToString());
                _redirecturl = _redirecturl.Replace("&amp;", "&");
            }
            if (!Page.IsPostBack)
            {
                try
                {
                    _lilist = new List<String>();
                    string _strItemhtml = @"'ImageTitle':'{0}' ,'Imageid' :'{1}','ImageUrl':'{2}','ImgUrlThumbformat':'{3}','Imagecaption':'{4}','ImgArtist':'{5}','ImgPreviewUrl':'{6}'";
                    string _strOuterhtml = @"jsonCallback({'menu': {
                                         'header': '{0}',items': [{{1}}]}})";
                    string _title = string.Empty;
                    string _gotitems = string.Empty;
                    string _gotouter = string.Empty;
                    int _cntget = 1;
                    Getimagedata _objdata = new Getimagedata();
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                    _objdata._intstartcnt = 1;
                    _objdata._strorientation = "horizontal";
                    var _data = _objdata.GetalleventimageDatalist(eventID);
                    if (_data.Count > 0)
                    {
                        foreach (var _dr in _data)
                        {
                            _title = _dr.Title;
                            if (_cntget == 1)
                            {
                                _gotitems += "{ " + string.Format(_strItemhtml,getData(_dr.Title), _dr.ImageId, _dr.UrlPreview, _dr.UrlThumb,getData(_dr.Caption),getData( _dr.Artist), _dr.UrlPreview) + " }";
                                _cntget++;
                            }
                            else
                            {
                                _gotitems += ", { " + string.Format(_strItemhtml, getData(_dr.Title), _dr.ImageId, _dr.UrlPreview, _dr.UrlThumb,getData(_dr.Caption),getData( _dr.Artist), _dr.UrlPreview) + " }";

                            }
                        }
                        //_strjson = "jsonCallback({'menu':{'imgname':'" + returnimagename + "','imgpath':'" + returnimagepath + "'}})";
                        //_gotouter = string.Format(_strOuterhtml, _title, _gotitems);
                        _gotouter = @"jsonCallback({'menu': {'header': '" + _title + "','items': [" + _gotitems + "]}})";
                        ltresult.Text = _gotouter;
                    }
                }
                catch (Exception ex)
                {
                    CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
                    ErrorLog.SaveErrorLog(_strSiteID, "Indexpage", "btnalbum_Click", "btnalbum_Click", ex.Message, _networkid.ToString());
                }
                
            }

        }

        public string getData(string data)
        {
            data= data.Replace("'", "");
            data = data.Replace("\n", "");

            return data;
        }
    }
}