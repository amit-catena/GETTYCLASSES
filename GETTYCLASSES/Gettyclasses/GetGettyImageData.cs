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
using System.Configuration;
using System.Web;
using System.IO; 

namespace Gettyclasses
{
    public class GetGettyImageData
    {
        //get image details to list
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

        /// <summary>
        /// search images with date range
        /// </summary>
        /// <returns></returns>
        public List<Imagedetails> GetimageDatalist_withdaterange()
        {
            List<Imagedetails> _listimg = new List<Imagedetails>();
            try
            {
                //declare objects.
                SearchForEditorialImagesSample _objs = new SearchForEditorialImagesSample();
                CreateSessionSample _objcs = new CreateSessionSample();
                SearchForImagesResponse _objresponse = new SearchForImagesResponse();
                var searchQuery = new SearchForImages2RequestBody();
                searchQuery.Filter = new Filter();
                searchQuery.Filter.Orientations = new List<string> { "horizontal" };
                searchQuery.Query = new Query();
                searchQuery.Query.SearchPhrase = !string.IsNullOrEmpty(_strserachterm) ? _strserachterm : "sports";
                searchQuery.Filter.ImageFamilies = new List<string> { "Editorial" };
                var token = _objcs.GetToken();
                var strsecuretoken = _objcs.GetSecureToken();
                _strtoken = token;
                _strsecuretoken = strsecuretoken;
                searchQuery.ResultOptions = new ResultOptions();
                searchQuery.ResultOptions.ItemCount = 75;
                _objs._Orientations = _strorientation;
                _objs._startcnt = _intstartcnt;
                _objs._imagefamilies = "Editorial";
                var searchResponse = _objs.defaultSearch(token, searchQuery.Query.SearchPhrase);
                //var searchResponse = _objs.Search(token, searchQuery.Query.SearchPhrase);
                var valtotal = searchResponse.SearchForImagesResult.ItemTotalCount;
                /*if (valtotal < 20)
                {
                    searchResponse = _objs.Search(token, searchQuery.Query.SearchPhrase);
                    valtotal = searchResponse.SearchForImagesResult.ItemTotalCount;
                }*/
                var _getimg = from images in searchResponse.SearchForImagesResult.Images
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
                                  Caption = images.Caption
                              };

                //bind all data to image class
                foreach (var _getdata in _getimg)
                {
                    Imagedetails _objimgdetails = new Imagedetails();
                    _objimgdetails.Artist = _getdata.Artist;
                    _objimgdetails.CollectionName = _getdata.Collectname;
                    _objimgdetails.DateCreated = _getdata.DateCreated;
                    _objimgdetails.ImageFamily = _getdata.ImageFamily;
                    _objimgdetails.ImageId = _getdata.ImageId;
                    _objimgdetails.LicensingModel = _getdata.LicensingModel;
                    _objimgdetails.UrlPreview = _getdata.UrlPreview;
                    _objimgdetails.UrlThumb = _getdata.UrlThumb;
                    _objimgdetails.Title = _getdata.Titel;
                    _objimgdetails.Caption = _getdata.Caption;
                    _objimgdetails.ShortCaption = _getdata.ShortCaption;
                    _listimg.Add(_objimgdetails);
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GetimageDatalist :", ex);
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GetimageDatalist", "GetimageDatalist", ex.Message, _strnetworkID.ToString());

            }
            return _listimg;
        }

        public bool GetdownloadimagesFromEditors(List<string> IDs)
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
                                using (AddGettyImages _addimg = new AddGettyImages())
                                {
                                    //download image 
                                    _addimg.SiteID = _strsiteID;
                                    _addimg.NetworkId = _strnetworkID;
                                    _addimg.GetsitenameandserverID();
                                    filename = DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + _objall.ImageId + ".jpg";
                                    foreach (var goturl in _popupurl2)
                                    {
                                        _returnfile = GoWebshotwithpath(goturl.dwnlargeimage, _objall.ImageId, _addimg.Sitename, _addimg.serverId, _addimg.IsImageServer);
                                    }
                                    //download image and save it in database
                                    //_posturl = GoWebshot(_dr.Imgurl, _dr.ImgId, _addimg.Sitename, _addimg.serverId, _addimg.IsImageServer);
                                    _addimg.RandomId = _strrandomecookie;
                                    _addimg.exturl = _returnfile;
                                    _addimg.imagetxt = _objall.Titel;
                                    _addimg.imagetxtbelow = _objall.Caption;
                                    _addimg.imagecredit = _objall.Titel;
                                    _addimg.AddexternalimagesURL();
                                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "javascript:Singlegettyimage('" + _posturl +"');", true);
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
                ErrorLog.SaveErrorLog("0", "getimagedata", "GoWebshotwithpath", "GoWebshotwithpath", ex.Message, _strnetworkID.ToString());
            }
            return _flag;
        }


        /// <summary>
        /// save image from editors
        /// </summary>
        /// <param name="strurl"></param>
        /// <param name="imgID"></param>
        /// <param name="_sitename"></param>
        /// <param name="_serverId"></param>
        /// <param name="IsImageServer"></param>
        /// <returns></returns>
        private string GoWebshotwithpath(string strurl, string imgID, string _sitename, string _serverId, string IsImageServer)
        {

            string monthyearfolder = DateTime.Now.ToString("yyyyMM");
            string dayfolder = DateTime.Now.ToString("MMMdd");
            string filename = "";
            string imgSrcURL = strurl;
            string path = null;
            string pathclient = null;
            string Tnname = string.Empty;
            bool result = false;
            string _connftroo = string.Empty;
            string _connftpusername = string.Empty;
            string _connftppass = string.Empty;
            string _connftpdir = string.Empty;
            //transfer file on uploadimages
            string _strsepath = string.Empty;
            string _postimagepath = string.Empty;
            try
            {
                filename = DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                System.Net.WebClient webClient = new System.Net.WebClient();
                Function.ValidateSiteFolder(_sitename);
                Function.CreateImagesFolderInBackend(_sitename, monthyearfolder, dayfolder);
                // Image Server Code
                pathclient = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}/{2}/", _sitename, monthyearfolder, dayfolder));
                webClient.DownloadFile(imgSrcURL, pathclient + filename);
                webClient.Dispose();
                _postimagepath = string.Format("{0}{1}/{2}/{3}/{4}", commonfn._imgserver, _sitename, monthyearfolder, dayfolder, filename);

            }
            catch (Exception exp)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GoWebshotwithpath :" + _strnetworkID, exp);
                ErrorLog.SaveErrorLog("0", "getimagedata", "GoWebshotwithpath", "GoWebshotwithpath", exp.Message, _strnetworkID.ToString());

            }



            return _postimagepath;
        }

    }
}
