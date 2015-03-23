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
    public partial class Showeventlist : System.Web.UI.Page
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
        
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable _dt = new DataTable();
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
                    addimages _objdata = new addimages();
                    _objdata.NetworkId = commonfn._defaultNetwork;  
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                    _dt=_objdata.GetAddedgettyevents();
                    if (_dt.Rows.Count > 0)
                    {
                        gettydata.DataSource = _dt;
                        gettydata.DataBind();
                    }
                    else
                    {
                        gettydata.DataSource = null;  
                    }
                    UpdatePanel1.Update();   
                    
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
            }


        }

        protected void gettydata_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            Literal _html = (Literal)e.Item.FindControl("lthtml");
            _html.Text = @"<a href='" + _baseURL + "alleventImages.aspx?eventid=" + (int)DataBinder.Eval(e.Item.DataItem, "EventId") + @"'  id='htmlA' >
                                <img src='" + HttpUtility.HtmlDecode((string)DataBinder.Eval(e.Item.DataItem, "ImageURL")).Replace("&amp;", "&") + @"'   runat='server'    />
                            </a>";
            Literal _htmlcnt = (Literal)e.Item.FindControl("ltcount");
            //_htmlcnt.Text = @"</br><a href='" + _baseURL + "alleventImages.aspx?eventid=" + (int)DataBinder.Eval(e.Item.DataItem, "EventId") + @"'  > " + (int)DataBinder.Eval(e.Item.DataItem, "ImageCount") + " images</a>";
            _htmlcnt.Text = @"</br><a href='#'  > " + (int)DataBinder.Eval(e.Item.DataItem, "ImageCount") + " images</a>";
            Literal _ltdate = (Literal)e.Item.FindControl("ltdate");
            //string _strdate = DataBinder.Eval(e.Item.DataItem, "EventDate");
            //IFormatProvider yyyymmddFormat = new System.Globalization.CultureInfo(String.Empty, false);
            //var culture = System.Globalization.CultureInfo.CurrentCulture;
            IFormatProvider culture = new CultureInfo("en-US", true); 
            DateTime _dt = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "EventDate"));
            //string converted = DateTime.ParseExact(DataBinder.Eval(e.Item.DataItem, "EventDate").ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            _ltdate.Text = _dt.ToString("d MMM yyyy", culture);
            //_ltdate.Text = _dt.ToString("MMMM d yyyy", commonfn.cultures) + "/date/" + DataBinder.Eval(e.Item.DataItem, "EventDate").ToString();
            Literal lttitle = (Literal)e.Item.FindControl("lttitle");
            _strtitle = (string)DataBinder.Eval(e.Item.DataItem, "EventTitle");
            if (_strtitle.Length > 300)
            {
                lttitle.Text = _strtitle + "...";
            }
            else
            {
                lttitle.Text = _strtitle;
            }

        }

    }
}