using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConnectExample.Samples;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using Newtonsoft.Json.Linq;
using CommonLib;
using System.IO; 

namespace Gettyclasses
{
     
    public class apkadddetailsmgmt:IDisposable
    {
        DAL dal;
        public apkadddetailsmgmt()
        {
            dal = new DAL();
        }
        public void Dispose()
        {
            dal.Dispose();
        }
        ~apkadddetailsmgmt()
        {
            Dispose();
        }

        #region :: public properties ::
        public int ApkFileId { get; set; }
        public string ApkFileName { get; set; }
        public string Version { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public int Addedby { get; set; }
        public int ApklinkId { get; set; }
        public int AppmasterId { get; set; }
        public string imagedate { get; set; }
        public string LinkName { get; set; }
        public string LinkReference { get; set; }
        public string filename { get; set; }
        public int LinkId { get; set; }


        public string _strserachterm { get; set; }
        public string _strtoken { get; set; }
        public string _strsecuretoken { get; set; }
        string _imagepathdownload = ConfigurationSettings.AppSettings["ImagePath"];

        public int _strnewsID { get; set; }
        public int _strnetworkID { get; set; }
        public string _strrandomecookie { get; set; }
        public string _strsiteID { get; set; }
        public string _strnews { get; set; }
        public int _intstartcnt { get; set; }
        public string _strorientation { get; set; }
        public string _strimageTextBelow { get; set; }
        public string _strimageName { get; set; }
        public string _strimageDate { get; set; }
        public string _strimageTitle { get; set; }
        public string _strimageUrl { get; set; }
        #endregion

        #region :: Method ::
        public int SaveApkLink()
        {          
            int apklinkid = 0;
           
            try
            {               

                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["appstorelinkmanager"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_ApkLink_Save";
                cmd.Connection = con;
                SqlParameter[] param = { new SqlParameter("@LinkName", SqlDbType.VarChar),
                                           new SqlParameter("@LinkReference",SqlDbType.VarChar),
                                           new SqlParameter("@LinkID",SqlDbType.Int)
                                      };
                param[0].Value = LinkName;
                param[1].Value = LinkReference;
                param[2].Value = LinkId;
                param[2].Direction = ParameterDirection.InputOutput;
                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
                con.Open();
                cmd.ExecuteNonQuery();
                apklinkid = Convert.ToInt32(param[2].Value);
                con.Close();
                 
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(Sections.BLL,"",ex);
            }
            return apklinkid;
        }

        public void SaveApkDetails()
        {
            int rowsaffected = 0;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["gamingappstore"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_SaveApkDetails";
                cmd.Connection = con;
               
                SqlParameter[] param = { new SqlParameter("@apkfilename", SqlDbType.VarChar),
                                       new SqlParameter("@version",SqlDbType.VarChar),
                                       new SqlParameter("@size",SqlDbType.VarChar),
                                       new SqlParameter("@description",SqlDbType.VarChar),
                                       new SqlParameter("@addedby",SqlDbType.Int),
                                       new SqlParameter("@apklinkid",SqlDbType.Int),
                                       new SqlParameter("@appmasterid",SqlDbType.Int),
                                       new SqlParameter("@imagedate",SqlDbType.VarChar),
                                       new SqlParameter("@apkfileid",SqlDbType.Int),
                                       new SqlParameter("@filename",SqlDbType.VarChar)};

                param[0].Value = ApkFileName;
                param[1].Value = Version==null?"":Version;
                param[2].Value = Size==null?"":Size;
                param[3].Value = Description==null?"":Description;
                param[4].Value = Addedby;
                param[5].Value = ApklinkId;
                param[6].Value = AppmasterId;
                param[7].Value = (imagedate==null)?"":imagedate;
                param[8].Value = ApkFileId;
                param[9].Value = filename==null?"":filename;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
                con.Open();
                rowsaffected=cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {

                CommonLib.ExceptionHandler.WriteLog(Sections.BLL, "", ex);
            }
        }


        public DataTable GetApkDetails(string apkid)
        {
            DataTable dt = new DataTable();
           
           
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["gamingappstore"]);
                SqlCommand cmd = new SqlCommand("AN_SP_GetApkDetails", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_GetApkDetails";
                cmd.Connection = con;
                cmd.Parameters.Add("@apkfileid", SqlDbType.Int).Value = apkid;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                  
            }
            catch (Exception ex)
            {

                CommonLib.ExceptionHandler.WriteLog(Sections.BLL, "", ex);
            }
            return dt;

        }

        public void SaveNewsImage(string randomid, string siteid, string imagedate, string imagename)
        {
            int rowsaffected = 0;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["gamingappstore"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_SP_SaveNewsImage";
                cmd.Connection = con;

                SqlParameter[] param = { 
                                            new SqlParameter("@RandomId", SqlDbType.VarChar),
                                            new SqlParameter("@SiteId", SqlDbType.Int),
                                            new SqlParameter("@imagedate",SqlDbType.VarChar),
                                            new SqlParameter("@imagename",SqlDbType.VarChar)
                                       };

                param[0].Value = randomid;
                param[1].Value = Convert.ToInt32(siteid);
                param[2].Value = (imagedate == null) ? "" : imagedate;
                param[3].Value = imagename == null ? "" : imagename;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
                con.Open();
                rowsaffected = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {

                CommonLib.ExceptionHandler.WriteLog(Sections.BLL, "", ex);
            }
        }
        #endregion

        #region :: gettyimages ::
        /// <summary>
        /// download single image from news.............
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public bool Getdownloadsingleimages(List<string> IDs)
        {
            List<Image> _liimg = new List<Image>();
            List<ImageSize> _liimgsize = new List<ImageSize>();
            List<DownloadItem> lidwn = new List<DownloadItem>();
            List<Imagedetails> _liimgdetails = new List<Imagedetails>();
            List<string> _getIDs = new List<string>();
            bool _flag = false;
            SiteInfo _objsite = new SiteInfo();
            string sitefolder = string.Empty;
            string monthyearfolder = DateTime.Now.ToString("yyyyMM");
            string dayfolder = DateTime.Now.ToString("MMMdd");
            string imageName = string.Empty;
            string filename = string.Empty;
            string _returnfile = string.Empty;
            try
            {
                GetLargestImageDownloadAuthorizationsSample _objdwnl = new GetLargestImageDownloadAuthorizationsSample();
                GetImageDownloadAuthorizationsSample _objsmall = new GetImageDownloadAuthorizationsSample();
                if (!string.IsNullOrEmpty(commonfn.getSession("token")))
                {
                    var token = commonfn.getSession("token");
                    var securetoken = commonfn.getSession("securetoken");
                    //set imeges ID
                    if (IDs.Count > 0)
                    {
                        //new code to download images
                        GetImageDetailsSample _objgetalldetails = new GetImageDetailsSample();
                        ImageSize _objimgsize = new ImageSize();
                        Authorization _objallauth = new Authorization();
                        DownloadItem _objalldw = new DownloadItem();
                        GetImageDownloadAuthorizationsSample _objallimg = new GetImageDownloadAuthorizationsSample();
                        CreateDownloadRequestSample _objallcrtdwn = new CreateDownloadRequestSample();
                        //get all data for selected images.
                        var _getalldetails = _objgetalldetails.GetImageDetails(token, IDs);
                        var _getmorealldetails = from images in _getalldetails.GetImageDetailsResult.Images
                                                 select new
                                                 {
                                                     Titel = images.Title,
                                                     Collectname = images.CollectionName,
                                                     UrlPreview = images.UrlPreview,
                                                     UrlThumb = images.UrlThumb,
                                                     ImageId = images.ImageId,
                                                     LicensingModel = images.LicensingModel,
                                                     ImageFamily = images.ImageFamily,
                                                     DateCreated = images.DateCreated,
                                                     Artist = images.Artist,
                                                     ShortCaption = images.Caption.Substring(0, 100) + "..",
                                                     Caption = images.Caption,
                                                     imagesize = images.SizesDownloadableImages[0].SizeKey
                                                 };

                        foreach (var _objall in _getmorealldetails)
                        {
                            //assing imageID and sizes
                            _objimgsize.SizeKey = _objall.imagesize;
                            _objimgsize.ImageId = _objall.ImageId;
                            _liimgsize.Add(_objimgsize);
                            //authorize iamge
                            var _getauthtoken = _objallimg.AuthorizeDownload(token, _liimgsize);
                            var _getdatas = from _firstauth in _getauthtoken.GetImageDownloadAuthorizationsResult.Images
                                            from _seondauth in _firstauth.Authorizations
                                            select new
                                            {
                                                //get download token
                                                _newdwntoken = _seondauth.DownloadToken
                                            };
                            foreach (var _val1 in _getdatas)
                            {
                                //assign token
                                _objalldw.DownloadToken = _val1._newdwntoken;
                                lidwn.Add(_objalldw);
                                var _getdwnurl2 = _objallcrtdwn.CreateRequest(securetoken, lidwn);
                                lidwn.Clear();
                                var _popupurl2 = from _popdwn2 in _getdwnurl2.CreateDownloadRequestResult.DownloadUrls
                                                 select new
                                                 {
                                                     dwnlargeimage = _popdwn2.UrlAttachment
                                                 };
                                using (addimages _addimg = new addimages())
                                {
                                    //download image 

                                    filename = DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + _objall.ImageId + ".jpg";
                                    foreach (var goturl in _popupurl2)
                                    {
                                        _returnfile = GoWebshot(goturl.dwnlargeimage, _objall.ImageId);
                                    }
                                    _strimageName = _returnfile;
                                    _addimg.imagename = _returnfile;

                                    _strimageTitle = _objall.Titel;
                                    _addimg.imagecredit = _objall.Titel;

                                    _strimageTextBelow = _objall.Caption;
                                    _addimg.imagetxtbelow = _objall.Caption;

                                    _addimg.NetworkId = _strnetworkID;
                                    _addimg.NewsId = _strnewsID;
                                    _addimg.imagetxt = _objall.Titel;

                                    _strimageUrl = "http://www.gettyimages.co.uk";
                                    _addimg.imageurl = "http://www.gettyimages.co.uk";

                                    _addimg.RandomId = _strrandomecookie;
                                    _addimg.SiteID = _strsiteID;
                                    //_addimg.Addnewssingleimages();

                                    Addnewssingleimages();

                                    _flag = true;
                                    _addimg.imagename = "";
                                    _addimg.imagecredit = "";
                                    _addimg.imagetxtbelow = "";
                                    _addimg.imagetxt = "";
                                    imageName = string.Empty;
                                    filename = string.Empty;
                                    _returnfile = string.Empty;
                                    _flag = true;
                                }

                            }
                        }
                        //end new code details
                    }
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GetdownloadSelectedImage :", ex);
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "Getdownloadsingleimages", "Getdownloadsingleimages", ex.Message, _strnetworkID.ToString());
            }
            return _flag;
        }

        /// <summary>
        /// Get image download here with webclient
        /// </summary>
        /// <param name="strurl"></param>
        /// <param name="imgID"></param>
        /// <returns></returns>
        private string GoWebshot(string strurl, string imgID)
        {
            string _imagepathdownload = "";
            string sitefolder = "";
            string monthyearfolder = DateTime.Now.ToString("yyyyMM");
            string dayfolder = DateTime.Now.ToString("MMMdd");
            string filename = "";
            string orgfilename = "";
            string imgSrcURL = strurl;
            string pathclient = null;
            string Tnname = string.Empty;
            string orgname = string.Empty;
            
            sitefolder = "gamingappstore/";

            if (_strnews == "y")
                sitefolder = sitefolder + "NEWS/";

            Function.CreateImagesFolderInBackend(sitefolder, monthyearfolder, dayfolder);
            pathclient = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}/{2}/", sitefolder, monthyearfolder, dayfolder));
            _imagepathdownload = pathclient;

            try
            {
                string imgExt = Path.GetExtension(imgSrcURL);
                string imageName = string.Empty;
                imageName = Function.ToFileName((imgSrcURL));
                orgfilename = "getty_" + DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                filename = DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadFile(imgSrcURL, _imagepathdownload + filename);
                webClient.DownloadFile(imgSrcURL, _imagepathdownload + orgfilename);
                webClient.Dispose();

                Tnname = Function.SaveThumbnailCompress(orgfilename, filename, _imagepathdownload, "TN_", 300, 225);
                Function.SaveThumbnailCompress(orgfilename, filename, _imagepathdownload, "TN_TN_", 128, 85);


            }
            catch (Exception exp)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GoWebshot :" + _strnetworkID, exp);
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GoWebshot", "GoWebshot", exp.Message, _strnetworkID.ToString());
            }
            return filename;
        }

        public bool Addnewssingleimages()
        {
            DataTable objDT = new DataTable();
            SqlConnection _newconn = new SqlConnection(ConfigurationSettings.AppSettings["gamingappstore"]);
            string monthyearfolder = DateTime.Now.ToString("yyyyMM");
            string dayfolder = DateTime.Now.ToString("MMMdd");
            int cnt = 0;
            try
            {
                _strimageDate = string.Format("{0}/{1}/", monthyearfolder, dayfolder);

                SqlCommand _spcommand = new SqlCommand("PR_SP_AddSingleGettyImage_Session", _newconn);
                _spcommand.CommandType = CommandType.StoredProcedure;
                _spcommand.Parameters.Add("@RandomId", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(_strrandomecookie) ? _strrandomecookie : " ";
                _spcommand.Parameters.Add("@SelectnewsImage", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(_strimageName) ? _strimageName : " ";
                _spcommand.Parameters.Add("@sitId", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(_strsiteID) ? _strsiteID : " ";
                _spcommand.Parameters.Add("@Gettyurl", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(_strimageUrl) ? _strimageUrl : " ";
                _spcommand.Parameters.Add("@ImageDesc", SqlDbType.NVarChar).Value = !string.IsNullOrEmpty(_strimageTextBelow) ? _strimageTextBelow : " ";
                _spcommand.Parameters.Add("@ImageDate", SqlDbType.NVarChar).Value = !string.IsNullOrEmpty(_strimageDate) ? _strimageDate : " ";
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
        #endregion

    }
}
