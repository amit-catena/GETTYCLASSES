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
using System.Text.RegularExpressions;
using System.Globalization; 

namespace gettywebclasses
{
    public partial class eventdetails : System.Web.UI.Page
    {
        string _imagepathdownload = ConfigurationSettings.AppSettings["ImagePath"];
        string _baseURL = ConfigurationSettings.AppSettings["baseurl"];
        public int _networkid = 1;
        public int _intnewsID = 0;
        public string _strcookiename = string.Empty;
        public string _strSiteID = string.Empty;
        public string _strtitle = string.Empty;
        public string _strnews = string.Empty;
        public static string _redirecturl = string.Empty;
        public static int _intstartval;
        public static List<String> _lilist = new List<String>();
        CultureInfo pr = CultureInfo.CurrentCulture;  
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (null != Request.QueryString["NwtID"])
                {
                    _networkid = Function.GetnetworkID(Request.QueryString["NwtID"].ToString());
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
                    _lilist = new List<String>();
                    if (_intstartval > 75)
                    {
                        prev.Visible = true;
                    }
                    loadmore.Visible = true;
                    prev.Visible = false;
                    Getimagedata _objdata = new Getimagedata();
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                    _objdata._intstartcnt = 1;
                    _objdata._strorientation = "horizontal";
                    var _data = _objdata.GeteventimageDatalist();
                    if (_data.Count > 0)
                    {
                        gettydata.DataSource = _data;
                        gettydata.DataBind();
                    }
                    else
                    {
                        gettydata.DataSource = null;
                    }
                }
                else
                {
                    if (_intstartval > 75)
                    {
                        prev.Visible = true;
                    }
                }
               
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
            }

           
        }


        /// <summary>
        /// item data bound on datalist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gettydata_ItemDataBound(object sender, DataListItemEventArgs e)
        {
           
            Literal _html = (Literal)e.Item.FindControl("lthtml");
            _html.Text = @"<a href='" + _baseURL + "alleventImages.aspx?eventid=" + (int)DataBinder.Eval(e.Item.DataItem, "_gotevent") + @"'  id='htmlA' >
                                <img src='" + HttpUtility.HtmlDecode((string)DataBinder.Eval(e.Item.DataItem, "UrlPreview")).Replace("&amp;", "&") + @"'   runat='server'    />
                            </a>";
            Literal _htmlcnt = (Literal)e.Item.FindControl("ltcount");
            _htmlcnt.Text = @"</br><a href='" + _baseURL + "alleventImages.aspx?eventid=" + (int)DataBinder.Eval(e.Item.DataItem, "_gotevent") + @"'  > "+  (int)DataBinder.Eval(e.Item.DataItem, "_goteventcnt") +" images</a>";
            Literal _ltdate = (Literal)e.Item.FindControl("ltdate");
           // _ltdate.Text = @"<div class='dttxt'><b>Date:</b>" + DateTime.Parse((DateTime)DataBinder.Eval(e.Item.DataItem, "DateCreated")) + "</div>";
            //_ltdate.Text = DateTime.ParseExact(Convert.ToString((DateTime)DataBinder.Eval(e.Item.DataItem, "DateCreated")), "DD MMM YY", pr).ToString();
            //DateTime dt = DateTime.ParseExact(Convert.ToString((DateTime)DataBinder.Eval(e.Item.DataItem, "DateCreated")), "DD MMM yyyy", CultureInfo.InvariantCulture);
            //string _strdate = dt.ToString("DD MMM yyyy");
            _ltdate.Text = DateTime.Parse(Convert.ToString((DateTime)DataBinder.Eval(e.Item.DataItem, "DateCreated"))).ToString("dd MMM yyyy");
            Literal lttitle = (Literal)e.Item.FindControl("lttitle");
            _strtitle = (string)DataBinder.Eval(e.Item.DataItem, "Title");
            if (_strtitle.Length > 300)
            {
                lttitle.Text = _strtitle + "...";
            }
            else
            {
                lttitle.Text = _strtitle;
            }

        }


        protected void btnmore_Click(object sender, EventArgs e)
        {
            try
            {
                Getimagedata _objdata = new Getimagedata();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                _intstartval = _intstartval + 75;
                prev.Visible = true;
                _objdata._intstartcnt = _intstartval;
                _objdata._strorientation = "horizontal";
                var _data = _objdata.GeteventimageDatalist();
                if (_data.Count > 0)
                {
                    gettydata.DataSource = _data;
                    gettydata.DataBind();
                    commonfn _objfn = new commonfn();
                    commonfn.setSession("token", _objdata._strtoken);
                    commonfn.setSession("securetoken", _objdata._strsecuretoken);
                    ltscript.Text = "";
                }
                else
                {
                    gettydata.DataSource = null;
                    ltscript.Text = "Sorry no records found.";
                }
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
                ErrorLog.SaveErrorLog(_strSiteID, "Indexpage", "btnmore_Click", "btnmore_Click", ex.Message, _networkid.ToString());
            }
        }
        protected void btnprev_Click(object sender, EventArgs e)
        {
            try
            {
                Getimagedata _objdata = new Getimagedata();
                _intstartval = _intstartval - 75;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                //Response.Write("_intstartval " + _intstartval+"</br>");  
                _objdata._intstartcnt = _intstartval;
                _objdata._strorientation = "horizontal";
                if (_intstartval == 1)
                    prev.Visible = false;

                var _data = _objdata.GeteventimageDatalist();
                if (_data.Count > 0)
                {
                    gettydata.DataSource = _data;
                    gettydata.DataBind();
                    commonfn _objfn = new commonfn();
                    commonfn.setSession("token", _objdata._strtoken);
                    commonfn.setSession("securetoken", _objdata._strsecuretoken);
                    ltscript.Text = "";
                }
                else
                {
                    gettydata.DataSource = null;
                    ltscript.Text = "Sorry no records found.";
                }
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
                ErrorLog.SaveErrorLog(_strSiteID, "Indexpage", "btnprev_Click", "btnprev_Click", ex.Message, _networkid.ToString());
            }
        }

     
    }
}