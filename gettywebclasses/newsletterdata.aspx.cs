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
    public partial class newsletterdata : System.Web.UI.Page
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
        public string _sessionselected = string.Empty;
        private static List<String> _lilist = new List<String>();


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
                commonfn.setSession("_strcookiename", _strcookiename);
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
                HttpContext.Current.Session["_dt"] = null;
                _lilist = new List<String>();
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
                _data = null;
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

            if (_lilist.Count > 0)
            {
                foreach (DataListItem _chk in gettydata.Items)
                {
                    HtmlInputCheckBox _selchk = _chk.FindControl("chkimg") as HtmlInputCheckBox;
                    if (_lilist.Contains(_selchk.Value))
                    {
                        _selchk.Checked = true;
                    }
                }
            }


        }

        protected void btnalbum_Click(object sender, EventArgs e)
        {

            DataTable _dt = new DataTable();
            List<string> _sellist = new List<string>();
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
                /* List<string> _imagesselect = new List<string>();
                 foreach (DataListItem _chk in gettydata.Items)
                 {
                     HtmlInputCheckBox _selchk = _chk.FindControl("chkimg") as HtmlInputCheckBox;
                     if (_selchk.Checked)
                     {
                         _imagesselect.Add(_selchk.Value);
                     }
                 }*/
                //get download seleted images
                Getimagedata _objdata = new Getimagedata();
                _objdata._strsiteID = !string.IsNullOrEmpty(_strSiteID) ? _strSiteID : "1";
                _objdata._strnetworkID = _networkid;
                _objdata._strrandomecookie = _strcookiename;
                _objdata._strnewsID = _intnewsID;
                _objdata._strnews = _strnews;

                if (null != HttpContext.Current.Session["_dt"])
                {
                    _dt = (DataTable)HttpContext.Current.Session["_dt"];
                    foreach (DataRow _dtval in _dt.Rows)
                    {
                        _sellist.Add(_dtval["ImageID"].ToString());
                    }
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert('" + _sellist.Count + "!');", true);
                }
                if (_sellist.Count > 0)
                {
                    if (_objdata.GetdownloadimagesFromEditors(_sellist))
                    {
                        _sellist.Clear();
                        if (null != HttpContext.Current.Session["_dt"])
                        {
                            HttpContext.Current.Session["_dt"] = null;
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "javascript:self.close()", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert('Please select image!');", true);
                    UpdatePanel1.Update();
                }

                //_imagesselect.Clear();

            }
            catch (Exception ex)
            {
                if (_sellist.Count > 0)
                {
                    _sellist.Clear(); 
                }
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
                ErrorLog.SaveErrorLog(_strSiteID, "postmultipleimages", "GoWebshot", "GoWebshot", ex.Message, _networkid.ToString());
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
                ErrorLog.SaveErrorLog(_strSiteID, "postmultipleimages", "GoWebshot", "GoWebshot", ex.Message, _networkid.ToString());
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
                ErrorLog.SaveErrorLog(_strSiteID, "postmultipleimages", "GoWebshot", "GoWebshot", ex.Message, _networkid.ToString());
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
                ErrorLog.SaveErrorLog(_strSiteID, "postmultipleimages", "GoWebshot", "GoWebshot", ex.Message, _networkid.ToString());
            }

        }

        protected void btnclear_Click(object sender, EventArgs e)
        {
            try
            {
                _lilist = new List<string>();
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

        [WebMethod]
        public static string ProcessIT(string _val, bool _chk)
        {
            string result = "false";
            string _tbkey = string.Empty;
            string _tbval = string.Empty;
            //create data table
            DataTable _dt = new DataTable();
            //find out in session
            if (null != HttpContext.Current.Session["_dt"])
            {
                _dt = (DataTable)HttpContext.Current.Session["_dt"] as DataTable;
                if (_chk)
                {
                    //maintain staic values
                    _lilist.Add(_val);
                    //result = "true";
                    //get randomnumber twitterID 
                    _tbkey = "Img" + RandomNumber(422, 40000);
                    //data value for twitter embeded code
                    _tbval = _val;
                    result = "true";
                }
                else
                {
                    //remove images from  staic values
                    if (_lilist.Contains(_val))
                        _lilist.Remove(_val);
                    DataRow[] drr = _dt.Select("ImageID=' " + _val + " ' ");
                    for (int i = 0; i < drr.Length; i++)
                        drr[i].Delete();
                    _dt.AcceptChanges();
                    result = "false";
                }

                //genrate row in datatable
                _dt = DynamicRows(_dt, _tbkey, _tbval);
                HttpContext.Current.Session["_dt"] = _dt;//stored datatable in session
            }
            else
            {
                //maintain staic values
                _lilist.Add(_val);
                _dt = DynamicColumns();//genrate first time colomn in datatable
                //get randomnumber twitterID 
                _tbkey = "Img" + RandomNumber(422, 40000);
                //data value for twitter embeded code
                _tbval = _val;
                //genrate row in datatable
                _dt = DynamicRows(_dt, _tbkey, _tbval);
                HttpContext.Current.Session["_dt"] = _dt;//stored datatable in session
                result = "true";
            }

            /* if (_sessiondata != "0" && !string.IsNullOrEmpty(_sessiondata))
             {
                 if (_sessiondata == getsessiondata)
                 {
                     if (_lilist.Count > 0)
                     {
                         if (_chk)
                         {
                             _lilist.Add(_val);
                             result = "true";
                         }
                         else
                         {
                             if (_lilist.Contains(_val))
                                 _lilist.Remove(_val);
                             result = "false";
                         }

                     }
                     else
                     {
                         if (_chk)
                         {
                             _lilist.Add(_val);
                             result = "true";
                         }

                     }

                 }

             }*/



            return result;
        }

        public static DataTable DynamicRows(object dynamicDataTable, string RandonTwitId, string TwitterText)
        {
            // Use Existing DataTable
            DataTable dt = (DataTable)dynamicDataTable;
            //Add Rows
            DataRow row = dt.NewRow();
            row["RandonimageId"] = RandonTwitId;
            row["ImageID"] = TwitterText;
            dt.Rows.Add(row);
            return dt;
        }


        public static DataTable DynamicColumns()
        {
            // Define the new datatable
            DataTable dt = new DataTable();
            // Define 2 columns
            DataColumn dc;
            dc = new DataColumn("RandonimageId");
            dt.Columns.Add(dc);
            dc = new DataColumn("ImageID");
            dt.Columns.Add(dc);
            return dt;
        }


        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}