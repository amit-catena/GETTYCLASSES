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

namespace gettywebclasses
{
    public partial class searchgettyimage : System.Web.UI.Page
    {
        string _imagepathdownload = ConfigurationSettings.AppSettings["ImagePath"];
        string _baseURL = ConfigurationSettings.AppSettings["baseurl"];
        public int _networkid = 1;
        public int _intnewsID = 0;
        public string _strcookiename = string.Empty;
        public string _strSiteID = string.Empty;
        public string _strnews = string.Empty;
        public static string _redirecturl = string.Empty;
        public static int _intstartval;
        protected void Page_Load(object sender, EventArgs e)
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
                if (_intstartval > 75)
                {
                    prev.Visible = true;
                }
                loadmore.Visible = false;
                prev.Visible = false;
            }
            else
            {

                if (_intstartval > 75)
                {
                    prev.Visible = true;
                }

            }

        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                Getimagedata _objdata = new Getimagedata();
                if (txtsearch.Text != "")
                    _objdata._strserachterm = txtsearch.Text.Trim();
                _intstartval = 1;
                prev.Visible = false;
                loadmore.Visible = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                //Response.Write("_intstartval " + _intstartval+"</br>");  
                _objdata._intstartcnt = _intstartval;
                _objdata._strorientation = RDchkbox.SelectedValue;
                //new changes
                //var _data = _objdata.GetimageDatalist();
                var _data = _objdata.GetimageDatalist_withdaterange();

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
                ErrorLog.SaveErrorLog(_strSiteID, "singleimage", "btnsearch_Click", "btnsearch_Click", ex.Message, _networkid.ToString());
            }
        }

        protected void btnmore_Click(object sender, EventArgs e)
        {
            try
            {
                Getimagedata _objdata = new Getimagedata();
                if (txtsearch.Text != "")
                    _objdata._strserachterm = txtsearch.Text.Trim();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                _intstartval = _intstartval + 75;
                prev.Visible = true;
                _objdata._intstartcnt = _intstartval;
                _objdata._strorientation = RDchkbox.SelectedValue;
                // var _data = _objdata.GetimageDatalist();
                var _data = _objdata.GetimageDatalist_withdaterange();
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
                ErrorLog.SaveErrorLog(_strSiteID, "singleimage", "btnsearch_Click", "btnmore_Click", ex.Message, _networkid.ToString());

            }
        }
        protected void btnprev_Click(object sender, EventArgs e)
        {
            try
            {
                Getimagedata _objdata = new Getimagedata();
                if (txtsearch.Text != "")
                    _objdata._strserachterm = txtsearch.Text.Trim();
                _intstartval = _intstartval - 75;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                //Response.Write("_intstartval " + _intstartval+"</br>");  
                _objdata._intstartcnt = _intstartval;
                _objdata._strorientation = RDchkbox.SelectedValue;
                if (_intstartval == 1)
                    prev.Visible = false;

                //var _data = _objdata.GetimageDatalist();
                var _data = _objdata.GetimageDatalist_withdaterange();
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
                ErrorLog.SaveErrorLog(_strSiteID, "singleimage", "btnsearch_Click", "btnprev_Click", ex.Message, _networkid.ToString());
            }
        }

        protected void RDchkbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Getimagedata _objdata = new Getimagedata();
                if (txtsearch.Text != "")
                    _objdata._strserachterm = txtsearch.Text.Trim();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                //Response.Write("_intstartval " + _intstartval+"</br>");  
                _intstartval = 1;
                _objdata._intstartcnt = _intstartval;
                _objdata._strorientation = RDchkbox.SelectedValue;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + RDchkbox.SelectedValue + ");", true);
                //var _data = _objdata.GetimageDatalist();
                var _data = _objdata.GetimageDatalist_withdaterange();
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
                ErrorLog.SaveErrorLog(_strSiteID, "singleimage", "RDchkbox_SelectedIndexChanged", "RDchkbox_SelectedIndexChanged", ex.Message, _networkid.ToString());
            }

        }

        protected void btnclear_Click(object sender, EventArgs e)
        {
            try
            {
                txtsearch.Text = "";
                _intstartval = 0;
                prev.Visible = false;
                loadmore.Visible = false;
                gettydata.DataSource = null;
                gettydata.DataBind();
                ltscript.Text = "Search it again.............!";
                UpdatePanel1.Update();
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
            _html.Text = @"<a href='" + (string)DataBinder.Eval(e.Item.DataItem, "UrlPreview") + @"' id='htmlA' >
                                <img src='" + _baseURL + @"images/spacer.gif'    data-original='" + HttpUtility.HtmlDecode((string)DataBinder.Eval(e.Item.DataItem, "UrlThumb")).Replace("&amp;", "&") + @"'   class='lazy' id='_imgtemp' runat='server'    />
                            </a>";
        }

        protected void rd_selectedbtn(object sender, EventArgs e)
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
                if (null != Request.QueryString["news"])
                {
                    _strnews = Request.QueryString["news"].ToString();
                }
                List<string> _imagesselect = new List<string>();
                foreach (DataListItem _chk in gettydata.Items)
                {
                    RadioButton _selchk = _chk.FindControl("Rdselect") as RadioButton;
                    if (_selchk.Checked)
                    {
                        HtmlInputHidden _selectedID = _chk.FindControl("btnselectId") as HtmlInputHidden;
                        _imagesselect.Add(_selectedID.Value);
                        apkadddetailsmgmt _objdata = new apkadddetailsmgmt();
                        _objdata._strsiteID = !string.IsNullOrEmpty(_strSiteID) ? _strSiteID : "1";
                        _objdata._strnetworkID = _networkid;
                        _objdata._strrandomecookie = _strcookiename;
                        _objdata._strnewsID = _intnewsID;
                        _objdata._strnews = _strnews;
                        //getdownload images.

                        if (_objdata.Getdownloadsingleimages(_imagesselect))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "javascript:self.close()", true);

                        }
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
                ErrorLog.SaveErrorLog(_strSiteID, "singleimage", "rd_selectedbtn", "rd_selectedbtn", ex.Message, _networkid.ToString());
            }
        }
    }
}