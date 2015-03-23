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
namespace gettywebclasses
{
    public partial class _Default : System.Web.UI.Page
    {
        string _imagepathdownload = ConfigurationSettings.AppSettings["ImagePath"];
        string _strsearch = string.Empty; 
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                Getimagedata _objdata = new Getimagedata();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                _objdata._intstartcnt = 1;
                _objdata._strorientation = "horizontal";
                if (null != Request.QueryString["search"])
                {
                    _objdata._strserachterm = Request.QueryString["search"].ToString().Trim() ;
                }
                else
                {
                    _objdata._strserachterm = "nadal";
                }
                var _data = _objdata.GetDefaultimageDatalist();
                if (_data.Count > 0)
                {
                    cdcatalog.DataSource = _data;
                    cdcatalog.DataBind();
                }
                else
                {
                    cdcatalog.DataSource = null;
                  
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
            }
         /*  Gettyclasses.Class1 n = new Class1();
           //List<string>li=n.getser();  
            
           DataTable _dt = new DataTable();
           var _objval = n.getser();
           cdcatalog.DataSource = _objval;
           cdcatalog.DataBind();
           //cdcatalog.DataSource = _dt;
           //cdcatalog.DataBind();  
            //get all classes
           string imageName = string.Empty;
           string filename = string.Empty;
           filename = DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + n._dnloadurlID + ".jpg";
           try
           {
               WebClient webClient = new WebClient();
               webClient.DownloadFile(n._dnloadurl, _imagepathdownload + filename);
               webClient.Dispose();
           }
           catch (Exception ex)
           {
               Response.Write(ex.Message.ToString());    
           }*/
        }
    }
}
