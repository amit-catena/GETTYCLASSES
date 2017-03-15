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
    public class newsingleimagemgmt
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
        /// Set first search data to datalist return image class
        /// </summary>
        /// <returns></returns>
        public List<Imagedetails> GetimageDatalist()
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
                var searchResponse = _objs.Search(token, searchQuery.Query.SearchPhrase);
                var valtotal = searchResponse.SearchForImagesResult.ItemTotalCount;
                var _getimg = from images in searchResponse.SearchForImagesResult.Images
                              from ievent in images.EventIds
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
                                  eventid = ievent.ToString()
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
                    _objimgdetails._gotevent = Convert.ToInt32(_getdata.eventid);
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
        public bool GetdownloadSelectedImage(List<string> IDs)
        {
            List<Image> _liimg = new List<Image>();
            List<ImageSize> _liimgsize = new List<ImageSize>();
            List<DownloadItem> lidwn = new List<DownloadItem>();
            List<Imagedetails> _liimgdetails = new List<Imagedetails>();
            bool _flag = false;
            try
            {
                GetLargestImageDownloadAuthorizationsSample _objdwnl = new GetLargestImageDownloadAuthorizationsSample();
                GetImageDownloadAuthorizationsSample _objsmall = new GetImageDownloadAuthorizationsSample();

                var securetoken = commonfn.getSession("securetoken");
                //set imeges ID
                if (!string.IsNullOrEmpty(commonfn.getSession("token")))
                {
                    var token = commonfn.getSession("token");
                    if (IDs.Count > 0)
                    {
                        foreach (var _ids in IDs)
                        {
                            //ad image details to class 
                            Image _objimg = new Image();
                            _objimg.ImageId = _ids;
                            _liimg.Add(_objimg);
                        }
                        //genrate tokens for download image
                        var _gotimg = _objdwnl.GetLargestDownloadForImages(token, _liimg);
                        var _objr = from res in _gotimg.GetLargestImageDownloadAuthorizationsResult.Images
                                    from auths in res.Authorizations
                                    select new
                                    {
                                        //image authorization
                                        ImageIDs = res.ImageId,
                                        DownloadIsFree = auths.DownloadIsFree,
                                        DownloadToken = auths.DownloadToken,
                                        ProductOfferingInstanceId = auths.ProductOfferingInstanceId,
                                        ProductOfferingType = auths.ProductOfferingType,
                                        SizeKey = auths.SizeKey
                                    };
                        foreach (var _imgdwn in _objr)
                        {
                            //create instance
                            Authorization _objauth = new Authorization();
                            DownloadItem _objdw = new DownloadItem();
                            GetImageDownloadAuthorizationsSample _objimg = new GetImageDownloadAuthorizationsSample();
                            CreateDownloadRequestSample _objcrtdwn = new CreateDownloadRequestSample();
                            ImageSize _imgsize = new ImageSize();

                            //get all image details from data sectect
                            GetImageDetailsSample _objgetdetails = new GetImageDetailsSample();
                            var getdetialsimg = _objgetdetails.GetImageDetails(token, IDs);
                            var _getmoredetails = from images in getdetialsimg.GetImageDetailsResult.Images
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

                            _objauth.SizeKey = _imgdwn.SizeKey;
                            _objauth.DownloadIsFree = _imgdwn.DownloadIsFree;
                            _objauth.DownloadToken = _imgdwn.DownloadToken;
                            _objauth.ProductOfferingInstanceId = _imgdwn.ProductOfferingInstanceId;
                            _objauth.ProductOfferingType = _imgdwn.ProductOfferingType;
                            _imgsize.SizeKey = _imgdwn.SizeKey;
                            _imgsize.ImageId = _imgdwn.ImageIDs;
                            _liimgsize.Add(_imgsize);
                            _objdw.DownloadToken = _objauth.DownloadToken;
                            lidwn.Add(_objdw);
                            var _getdwnurl = _objcrtdwn.CreateRequest(securetoken, lidwn);
                            var _popupurl = from _popdwn in _getdwnurl.CreateDownloadRequestResult.DownloadUrls
                                            select new
                                            {
                                                dwnlargeimage = _popdwn.UrlAttachment
                                            };
                            foreach (var getvalur in _popupurl)
                            {
                                //download image 
                                string imageName = string.Empty;
                                string filename = string.Empty;
                                string _returnfile = string.Empty;
                                filename = DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + _imgdwn.ImageIDs + ".jpg";
                                try
                                {
                                    //download image on secure path.
                                    /*System.Net.WebClient webClient = new System.Net.WebClient();
                                    webClient.DownloadFile(getvalur.dwnlargeimage, _imagepathdownload + filename);
                                    webClient.Dispose();*/
                                    _returnfile = GoWebshot(getvalur.dwnlargeimage, _imgdwn.ImageIDs);
                                    //add image details 
                                    using (addimages _addimg = new addimages())
                                    {
                                        foreach (var _dr in _getmoredetails)
                                        {
                                            _addimg.imagename = _returnfile;
                                            _addimg.imagecredit = _dr.Titel;
                                            _addimg.imagetxtbelow = _dr.Caption;
                                            _addimg.NetworkId = _strnetworkID;
                                            _addimg.NewsId = _strnewsID;
                                            _addimg.imagetxt = _dr.Titel;
                                            _addimg.imageurl = "http://www.gettyimages.co.uk";
                                            _addimg.RandomId = _strrandomecookie;
                                            _addimg.SiteID = _strsiteID;
                                            _addimg.Addnewssingleimages();
                                            _flag = true;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "webClient.Dispose() :", ex);
                                    ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GetdownloadSelectedImage", "GetdownloadSelectedImage", ex.Message, _strnetworkID.ToString());
                                }

                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GetdownloadSelectedImage :", ex);
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GetdownloadSelectedImage", "GetdownloadSelectedImage", ex.Message, _strnetworkID.ToString());
            }
            return _flag;
        }
        public bool GetdownloadSelectedMultipleImage(List<string> IDs)
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
            try
            {
                _objsite.NetWorkID = _strnetworkID.ToString();
                _objsite.GetSiteDetails(_strsiteID, _strnetworkID.ToString());
                if (_objsite.ImageServer)
                {
                    sitefolder = Function.GetSiteFolderName(_objsite.SiteUrl);
                    Function.CreateImagesFolderInBackend(sitefolder, monthyearfolder, dayfolder);
                }
            }
            catch
            {

            }
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
                        foreach (var _ids in IDs)
                        {
                            //ad image details to class 
                            Image _objimg = new Image();
                            _objimg.ImageId = _ids;
                            _liimg.Add(_objimg);
                        }
                        //genrate tokens for download image
                        var _gotimg = _objdwnl.GetLargestDownloadForImages(token, _liimg);
                        var _objr = from res in _gotimg.GetLargestImageDownloadAuthorizationsResult.Images
                                    from auths in res.Authorizations
                                    select new
                                    {
                                        //image authorization
                                        ImageIDs = res.ImageId,
                                        DownloadIsFree = auths.DownloadIsFree,
                                        DownloadToken = auths.DownloadToken,
                                        ProductOfferingInstanceId = auths.ProductOfferingInstanceId,
                                        ProductOfferingType = auths.ProductOfferingType,
                                        SizeKey = auths.SizeKey
                                    };
                        //foreach loop for selected images 
                        foreach (var _imgdwn in _objr)
                        {
                            //create instance
                            Authorization _objauth = new Authorization();
                            DownloadItem _objdw = new DownloadItem();
                            GetImageDownloadAuthorizationsSample _objimg = new GetImageDownloadAuthorizationsSample();
                            CreateDownloadRequestSample _objcrtdwn = new CreateDownloadRequestSample();
                            ImageSize _imgsize = new ImageSize();
                            //get all image details data 
                            GetImageDetailsSample _objgetdetails = new GetImageDetailsSample();
                            _getIDs.Add(_imgdwn.ImageIDs);
                            var getdetialsimg = _objgetdetails.GetImageDetails(token, _getIDs);
                            _getIDs.Clear();
                            var _getmoredetails = from images in getdetialsimg.GetImageDetailsResult.Images
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
                            _objauth.SizeKey = _imgdwn.SizeKey;
                            _objauth.DownloadIsFree = _imgdwn.DownloadIsFree;
                            _objauth.DownloadToken = _imgdwn.DownloadToken;
                            _objauth.ProductOfferingInstanceId = _imgdwn.ProductOfferingInstanceId;
                            _objauth.ProductOfferingType = _imgdwn.ProductOfferingType;
                            _imgsize.SizeKey = _imgdwn.SizeKey;
                            _imgsize.ImageId = _imgdwn.ImageIDs;
                            var datadf = from iii in _getmoredetails.AsEnumerable()
                                         select new
                                         {
                                             valadds = iii.imagesize
                                         };
                            foreach (var getsize in datadf)
                            {
                                _imgsize.SizeKey = getsize.valadds;
                                _objauth.SizeKey = getsize.valadds;
                            }
                            _liimgsize.Add(_imgsize);
                            _objdw.DownloadToken = _objauth.DownloadToken;
                            /*lidwn.Add(_objdw);//large image download
                            var _getdwnurl = _objcrtdwn.CreateRequest(securetoken, lidwn);
                            lidwn.Clear();
                             var _popupurl = from _popdwn in _getdwnurl.CreateDownloadRequestResult.DownloadUrls
                                            select new
                                            {
                                                dwnlargeimage = _popdwn.UrlAttachment
                                            }; 
                             */
                            //new code
                            var _getdownlod = _objimg.AuthorizeDownload(token, _liimgsize);
                            var _getdatas = from _firstauth in _getdownlod.GetImageDownloadAuthorizationsResult.Images
                                            from _seondauth in _firstauth.Authorizations
                                            select new
                                            {
                                                _newdwntoken = _seondauth.DownloadToken
                                            };
                            foreach (var _val1 in _getdatas)
                            {
                                _objdw.DownloadToken = _val1._newdwntoken;
                                lidwn.Add(_objdw);
                                var _getdwnurl2 = _objcrtdwn.CreateRequest(securetoken, lidwn);
                                lidwn.Clear();
                                var _popupurl2 = from _popdwn2 in _getdwnurl2.CreateDownloadRequestResult.DownloadUrls
                                                 select new
                                                 {
                                                     dwnlargeimage = _popdwn2.UrlAttachment
                                                 };
                                foreach (var getvalur in _popupurl2)
                                {
                                    //download image 
                                    string imageName = string.Empty;
                                    string filename = string.Empty;
                                    string _returnfile = string.Empty;
                                    filename = DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + _imgdwn.ImageIDs + ".jpg";
                                    try
                                    {
                                        //download image on secure path.
                                        /*System.Net.WebClient webClient = new System.Net.WebClient();
                                        webClient.DownloadFile(getvalur.dwnlargeimage, _imagepathdownload + filename);
                                        webClient.Dispose();*/
                                        _returnfile = GoWebshotmultipleimages(getvalur.dwnlargeimage, _imgdwn.ImageIDs, _objsite.ImageServer, sitefolder, monthyearfolder, dayfolder);
                                        //add image details 
                                        using (addimages _addimg = new addimages())
                                        {
                                            foreach (var _dr in _getmoredetails)
                                            {
                                                _addimg.imagename = _returnfile;
                                                _addimg.imagecredit = _dr.Titel;
                                                _addimg.imagetxtbelow = _dr.Caption;
                                                _addimg.NetworkId = _strnetworkID;
                                                _addimg.NewsId = _strnewsID;
                                                _addimg.imagetxt = _dr.Titel;
                                                _addimg.imageurl = "http://www.gettyimages.co.uk";
                                                if (_objsite.ImageServer)
                                                {
                                                    _addimg.ImageAddedDate = Function.GetDateForImageToSave();
                                                    _addimg.IsNewImage = "Y";
                                                }
                                                else
                                                {
                                                    _addimg.ImageAddedDate = "N";
                                                    _addimg.IsNewImage = "N";
                                                }
                                                _addimg.RandomId = _strrandomecookie;
                                                _addimg.SiteID = _strsiteID;
                                                _addimg.Addnewsimages();
                                                _addimg.imagename = "";
                                                _addimg.imagecredit = "";
                                                _addimg.imagetxtbelow = "";
                                                _addimg.imagetxt = "";
                                                _flag = true;

                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "webClient.Dispose() :", ex);
                                        ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GetdownloadSelectedImage", "GetdownloadSelectedImage", ex.Message, _strnetworkID.ToString());

                                    }

                                }

                            }
                            //end code

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GetdownloadSelectedImage :", ex);
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GetdownloadSelectedImage", "GetdownloadSelectedImage", ex.Message, _strnetworkID.ToString());
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
            string path = null;
            string pathclient = null;
            string Tnname = string.Empty;
            string orgname = string.Empty;
            bool result = false;
            _imagepathdownload = HttpContext.Current.Server.MapPath("~/newsliveupdate/"); 
            
            try
            {
                string imgExt = Path.GetExtension(imgSrcURL);
                string imageName = string.Empty;
                imageName = Function.ToFileName((imgSrcURL));
                orgfilename = "getty_" + DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                filename = DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadFile(imgSrcURL, _imagepathdownload + filename);             
                webClient.Dispose();
                
               

            }
            catch (Exception exp)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GoWebshot :" + _strnetworkID, exp);
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GoWebshot", "GoWebshot", exp.Message, _strnetworkID.ToString());
            }
            return filename;
        }

        /// <summary>
        /// store multiple images from getty.
        /// </summary>
        /// <param name="strurl"></param>
        /// <param name="imgID"></param>
        /// <param name="ImageServer"></param>
        /// <param name="sitefolder"></param>
        /// <param name="monthyearfolder"></param>
        /// <param name="dayfolder"></param>
        /// <returns></returns>
        private string GoWebshotmultipleimages(string strurl, string imgID, bool ImageServer, string sitefolder, string monthyearfolder, string dayfolder)
        {
            string _imagepathdownload = "";
            string filename = "";
            string orgfilename = "";
            string imgSrcURL = strurl;
            string pathclient = null;
            string Tnname = string.Empty;
            string orgname = string.Empty;
            bool result = false;
            if (ImageServer)
            {
                Function.CreateImagesFolderInBackend(sitefolder, monthyearfolder, dayfolder);
                pathclient = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}/{2}/", sitefolder, monthyearfolder, dayfolder));
                _imagepathdownload = pathclient;
            }
            else
            {
                pathclient = HttpContext.Current.Server.MapPath("images/newsimages/");
                _imagepathdownload = pathclient;
            }
            try
            {
                string imgExt = Path.GetExtension(imgSrcURL);
                string imageName = string.Empty;
                imageName = Function.ToFileName((imgSrcURL));
                orgfilename = "getty_" + DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                filename = DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadFile(imgSrcURL, _imagepathdownload + filename);
                webClient.Dispose();
                if (!ImageServer)
                {
                    //ltscript.Text = " webClient.DownloadFile" + imgID;
                    //connect FTp  with writerllc
                    FtpClient ftp = new FtpClient(commonfn.FTpRoot, commonfn.FTPusername, commonfn.FTppassword);
                    ftp.RemotePath = commonfn.FTpRootdir;
                    ftp.Login();
                    //transfer file on uploadimages
                    string _strsepath = string.Empty;
                    if (commonfn._servertype != "local")
                    {
                        _strsepath = commonfn.FTpRootdir;
                    }
                    else
                    {
                        _strsepath = commonfn.FTpRootdir + "newsimages/";
                    }
                    //orgname = Function.SaveoriginalImage(orgfilename, commonfn._imagepath, "", 700, 700);
                    result = commonfn.Transfer_File(commonfn._uploadurl + filename, commonfn.FTpRoot, commonfn.FTPusername, commonfn.FTppassword, _strsepath);
                    //create thumbnail for uploadimages
                    Tnname = Function.SaveThumbnailCompress(filename, commonfn._imagepath, "TN", 300, 225);
                    //Common.Function.SaveThumbnailCompress(filename, _imagepath, "TN_TN", 300, 225);
                    Function.SaveThumbnailCompress(filename, commonfn._imagepath, "TN_TN", 128, 85);
                    //transfer file on writerllc
                    result = commonfn.Transfer_File(commonfn._uploadurl + "TN" + filename, commonfn.FTpRoot, commonfn.FTPusername, commonfn.FTppassword, _strsepath);
                    result = commonfn.Transfer_File(commonfn._uploadurl + "TN_TN" + filename, commonfn.FTpRoot, commonfn.FTPusername, commonfn.FTppassword, _strsepath);
                    ftp.Close();
                }
                else
                {
                    //orgname = Function.SaveoriginalImage(orgfilename, _imagepathdownload, "", 700, 700);
                    //create thumbnail for uploadimages
                    Tnname = Function.SaveThumbnailCompress(filename, _imagepathdownload, "TN", 300, 225);
                    //Common.Function.SaveThumbnailCompress(filename, _imagepath, "TN_TN", 300, 225);
                    Function.SaveThumbnailCompress(filename, _imagepathdownload, "TN_TN", 128, 85);
                }

            }
            catch (Exception exp)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GoWebshot :" + _strnetworkID, exp);
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GoWebshotmultipleimages", "GoWebshotmultipleimages", exp.Message, _strnetworkID.ToString());
            }
            imgID = "";
            return Tnname;
        }
        /// <summary>
        /// new download code images...................
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public bool Getdownloadmultipleimages(List<string> IDs)
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
                _objsite.NetWorkID = _strnetworkID.ToString();
                _objsite.GetSiteDetails(_strsiteID, _strnetworkID.ToString());
                if (_objsite.ImageServer)
                {
                    sitefolder = Function.GetSiteFolderName(_objsite.SiteUrl);
                    Function.CreateImagesFolderInBackend(sitefolder, monthyearfolder, dayfolder);
                }
            }
            catch
            {

            }
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
                                        _returnfile = GoWebshotmultipleimages(goturl.dwnlargeimage, _objall.ImageId, _objsite.ImageServer, sitefolder, monthyearfolder, dayfolder);
                                    }
                                    _addimg.imagename = _returnfile;
                                    _addimg.imagecredit = _objall.Titel;
                                    _addimg.imagetxtbelow = _objall.Caption;
                                    _addimg.NetworkId = _strnetworkID;
                                    _addimg.NewsId = _strnewsID;
                                    _addimg.imagetxt = _objall.Titel;
                                    _addimg.imageurl = "http://www.gettyimages.co.uk";
                                    if (_objsite.ImageServer)
                                    {
                                        _addimg.ImageAddedDate = Function.GetDateForImageToSave();
                                        _addimg.IsNewImage = "Y";
                                    }
                                    else
                                    {
                                        _addimg.ImageAddedDate = "N";
                                        _addimg.IsNewImage = "N";
                                    }
                                    _addimg.RandomId = _strrandomecookie;
                                    _addimg.SiteID = _strsiteID;
                                    _addimg.Addnewsimages();
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
            }
            return _flag;
        }
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
                                    _addimg.imagename = _returnfile;
                                    _addimg.imagecredit = _objall.Titel;
                                    _addimg.imagetxtbelow = _objall.Caption;
                                    _addimg.NetworkId = _strnetworkID;
                                    _addimg.NewsId = _strnewsID;
                                    _addimg.imagetxt = _objall.Titel;
                                    _addimg.imageurl = "http://www.gettyimages.co.uk";
                                    _addimg.RandomId = _strrandomecookie;
                                    _addimg.SiteID = _strsiteID;
                                    _addimg.Addnewssingleimages();
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
                                using (addimages _addimg = new addimages())
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

        /// <summary>
        /// get event details 
        /// </summary>
        /// <returns></returns>
        public List<Imagedetails> GeteventimageDatalist()
        {
            List<Imagedetails> _listimg = new List<Imagedetails>();
            List<int> _eventID = new List<int>();
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
            _intstartcnt = 1;
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    //declare objects.
                    _objs._Orientations = _strorientation;
                    _objs._startcnt = _intstartcnt;
                    _objs._imagefamilies = "Editorial";
                    var searchResponse = _objs.Search(token, searchQuery.Query.SearchPhrase);
                    var valtotal = searchResponse.SearchForImagesResult.ItemTotalCount;
                    var _getimg = from images in searchResponse.SearchForImagesResult.Images
                                  from ievent in images.EventIds
                                  select new
                                  {
                                      eventid = ievent.ToString()
                                  };
                    //bind all data to image class
                    foreach (var _getdata in _getimg)
                    {
                        if (_eventID.Count > 0)
                        {
                            if (!_eventID.Contains(Convert.ToInt32(_getdata.eventid)))
                                _eventID.Add(Convert.ToInt32(_getdata.eventid));
                        }
                        else
                        {
                            _eventID.Add(Convert.ToInt32(_getdata.eventid));
                        }

                    }
                    /* foreach (int intevent in _eventID)
                     {
                         if (intevent != 0)
                         {
                             _objs._eventID = intevent;
                             _objs._Orientations = _strorientation;
                             _objs._startcnt = 1;
                             _objs._imagefamilies = "Editorial";
                             var eventrespone = _objs.EventSearch(token, searchQuery.Query.SearchPhrase);
                             var _getvalcount = eventrespone.SearchForImagesResult.ItemTotalCount;
                             var _getevrnrespons = from images in eventrespone.SearchForImagesResult.Images
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
                                                   };

                             foreach (var _getallevent in _getevrnrespons)
                             {

                                 Imagedetails _objimgdetails = new Imagedetails();
                                 _objimgdetails.Artist = _getallevent.Artist;
                                 _objimgdetails.CollectionName = _getallevent.Collectname;
                                 _objimgdetails.DateCreated = _getallevent.DateCreated;
                                 _objimgdetails.ImageFamily = _getallevent.ImageFamily;
                                 _objimgdetails.ImageId = _getallevent.ImageId;
                                 _objimgdetails.LicensingModel = _getallevent.LicensingModel;
                                 _objimgdetails.UrlPreview = _getallevent.UrlPreview;
                                 _objimgdetails.UrlThumb = _getallevent.UrlThumb;
                                 _objimgdetails.Title = _getallevent.Titel;
                                 _objimgdetails.Caption = _getallevent.Caption;
                                 _objimgdetails.ShortCaption = _getallevent.ShortCaption;
                                 _objimgdetails._gotevent = intevent;
                                 _objimgdetails._goteventcnt = _getvalcount;
                                 _listimg.Add(_objimgdetails);
                                 break;
                             }



                         }



                     }*/
                }
                catch (Exception ex)
                {
                    CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GetimageDatalist :", ex);
                    ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GetimageDatalist", "GetimageDatalist", ex.Message, _strnetworkID.ToString());

                }
                _intstartcnt += 75;
            }
            _listimg = GetEventRelateID(_eventID);
            return _listimg;
        }

        public List<Imagedetails> GetEventRelateID(List<int> _eventID)
        {
            List<Imagedetails> _listimg = new List<Imagedetails>();
            try
            {
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
                _intstartcnt = 1;
                foreach (int intevent in _eventID)
                {
                    if (intevent != 0)
                    {
                        _objs._eventID = intevent;
                        _objs._Orientations = _strorientation;
                        _objs._startcnt = 1;
                        _objs._imagefamilies = "Editorial";
                        var eventrespone = _objs.EventSearch(token, searchQuery.Query.SearchPhrase);
                        var _getvalcount = eventrespone.SearchForImagesResult.ItemTotalCount;
                        var _getevrnrespons = from images in eventrespone.SearchForImagesResult.Images
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
                                              };


                        foreach (var _getallevent in _getevrnrespons)
                        {

                            Imagedetails _objimgdetails = new Imagedetails();
                            _objimgdetails.Artist = _getallevent.Artist;
                            _objimgdetails.CollectionName = _getallevent.Collectname;
                            _objimgdetails.DateCreated = _getallevent.DateCreated;
                            _objimgdetails.ImageFamily = _getallevent.ImageFamily;
                            _objimgdetails.ImageId = _getallevent.ImageId;
                            _objimgdetails.LicensingModel = _getallevent.LicensingModel;
                            _objimgdetails.UrlPreview = _getallevent.UrlPreview;
                            _objimgdetails.UrlThumb = _getallevent.UrlThumb;
                            _objimgdetails.Title = _getallevent.Titel;
                            _objimgdetails.Caption = _getallevent.Caption;
                            _objimgdetails.ShortCaption = _getallevent.ShortCaption;
                            _objimgdetails._gotevent = intevent;
                            _objimgdetails._goteventcnt = _getvalcount;
                            _listimg.Add(_objimgdetails);
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GetimageDatalist :", ex);
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GetimageDatalist", "GetimageDatalist", ex.Message, _strnetworkID.ToString());

            }

            return _listimg;

        }



        public List<Imagedetails> GetalleventimageDatalist(int _id)
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
                _objs._eventID = _id;
                var searchResponse = _objs.EventSearch(token, searchQuery.Query.SearchPhrase);
                var valtotal = searchResponse.SearchForImagesResult.ItemTotalCount;
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


        #region "NEWS LETTERDATA"

        public bool GetdownloadNewsletterimage(List<string> IDs)
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
                                        _returnfile = GoWebshotForNewsletterimage(goturl.dwnlargeimage, _objall.ImageId);
                                    }
                                    _addimg.imagename = _returnfile;
                                    _addimg.imagecredit = _objall.Titel;
                                    _addimg.imagetxtbelow = _objall.Caption;
                                    _addimg.NetworkId = _strnetworkID;
                                    _addimg.NewsId = _strnewsID;
                                    _addimg.imagetxt = _objall.Titel;
                                    _addimg.imageurl = "http://www.gettyimages.co.uk";
                                    _addimg.RandomId = _strrandomecookie;
                                    _addimg.SiteID = _strsiteID;
                                    _addimg.Addnewssingleimages();
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

        private string GoWebshotForNewsletterimage(string strurl, string imgID)
        {
            string _imagepathdownload = "";
            string sitefolder = "";
            string monthyearfolder = DateTime.Now.ToString("yyyyMM");
            string dayfolder = DateTime.Now.ToString("MMMdd");
            string filename = "";
            string orgfilename = "";
            string imgSrcURL = strurl;
            string path = null;
            string pathclient = null;
            string Tnname = string.Empty;
            string orgname = string.Empty;
            bool result = false;
            SiteInfo objsite = new SiteInfo();
            objsite.NetWorkID = _strnetworkID.ToString();

            _imagepathdownload = ConfigurationSettings.AppSettings["newImagePath"] + "banner/";
            try
            {
                string imgExt = Path.GetExtension(imgSrcURL);
                string imageName = string.Empty;
                imageName = Function.ToFileName((imgSrcURL));
                orgfilename = "getty_" + DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                filename = DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadFile(imgSrcURL, _imagepathdownload + filename);
                webClient.Dispose();

                //orgname = Function.SaveoriginalImage(orgfilename, _imagepathdownload, "", 700, 700);
                //create thumbnail for uploadimages
                Tnname = Function.SaveThumbnailCompress(filename, _imagepathdownload, "TN", 400, 400);
            }
            catch (Exception exp)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GoWebshot :" + _strnetworkID, exp);
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GoWebshotForNewsletterimage", "GoWebshotForNewsletterimage", exp.Message, _strnetworkID.ToString());
            }
            return Tnname;
        }
        #endregion


        #region Scheduler For Gettyevents

        /// <summary>
        /// get event details 
        /// </summary>
        /// <returns></returns>
        public void AddGettyeventdetails()
        {
            List<Imagedetails> _listimg = new List<Imagedetails>();
            List<int> _eventID = new List<int>();
            string _vareventID = string.Empty;
            int _cnt = 1;
            SearchForEditorialImagesSample _objs = new SearchForEditorialImagesSample();
            CreateSessionSample _objcs = new CreateSessionSample();
            SearchForImagesResponse _objresponse = new SearchForImagesResponse();
            var searchQuery = new SearchForImages2RequestBody();
            searchQuery.Filter = new Filter();
            searchQuery.Filter.Orientations = new List<string> { "horizontal" };
            searchQuery.Query = new Query();
            searchQuery.Query.SearchPhrase = !string.IsNullOrEmpty(_strserachterm) ? _strserachterm : "";
            searchQuery.Filter.ImageFamilies = new List<string> { "Editorial" };
            var token = _objcs.GetToken();
            _strtoken = token;
            searchQuery.ResultOptions = new ResultOptions();
            searchQuery.ResultOptions.ItemCount = 10;
            _intstartcnt = 1;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    //declare objects.
                    _objs._Orientations = _strorientation;
                    _objs._startcnt = _intstartcnt;
                    _objs._imagefamilies = "Editorial";
                    var searchResponse = _objs.defaultSearch(token, searchQuery.Query.SearchPhrase);
                    var valtotal = searchResponse.SearchForImagesResult.ItemTotalCount;
                    var _getimg = from images in searchResponse.SearchForImagesResult.Images
                                  from ievent in images.EventIds
                                  select new
                                  {
                                      eventid = ievent.ToString()
                                  };
                    //bind all data to image class
                    foreach (var _getdata in _getimg)
                    {
                        using (addimages _objimg = new addimages())
                        {
                            _objimg.NetworkId = _strnetworkID;
                            if (_objimg.checkGettyEvents(Convert.ToInt32(_getdata.eventid)))
                            {
                                if (_eventID.Count > 0)
                                {
                                    if (!_eventID.Contains(Convert.ToInt32(_getdata.eventid)))
                                    {
                                        _eventID.Add(Convert.ToInt32(_getdata.eventid));
                                        System.Console.WriteLine("Added EventID-: " + _getdata.eventid);
                                    }
                                    else
                                    {
                                        if (_cnt == 1)
                                        {
                                            if (!_vareventID.Contains(Convert.ToString(_getdata.eventid)))
                                                _vareventID = Convert.ToString(_getdata.eventid);
                                            _cnt++;
                                        }
                                        else
                                        {
                                            if (!_vareventID.Contains(Convert.ToString(_getdata.eventid)))
                                                _vareventID += "," + Convert.ToString(_getdata.eventid);
                                        }

                                    }
                                }
                                else
                                {
                                    _eventID.Add(Convert.ToInt32(_getdata.eventid));
                                    System.Console.WriteLine("Added EventID-: " + _getdata.eventid);

                                }
                            }
                            else
                            {
                                System.Console.WriteLine("Already added EventID-: " + _getdata.eventid);
                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                    CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GetimageDatalist :", ex);
                    ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GetimageDatalist", "GetimageDatalist", ex.Message, _strnetworkID.ToString());

                }
                _intstartcnt += 10;
            }

            //update Repeat Events Dates
            if (!string.IsNullOrEmpty(_vareventID))
            {
                using (addimages _objimg = new addimages())
                {
                    System.Console.WriteLine("Updated EventID-: " + _vareventID);
                    _objimg.NetworkId = _strnetworkID;
                    _objimg.UpdateGettyEvents(_vareventID);
                }
            }

            InsertGettyEvnets(_eventID);
        }

        public void NewFN_AddGettyeventdetails()
        {
            List<Imagedetails> _listimg = new List<Imagedetails>();
            List<int> _eventID = new List<int>();
            string _vareventID = string.Empty;
            int _cnt = 1;
            int _totalimg = 1;
            int _addedevent = 1;
            SearchForEditorialImagesSample _objs = new SearchForEditorialImagesSample();
            CreateSessionSample _objcs = new CreateSessionSample();
            SearchForImagesResponse _objresponse = new SearchForImagesResponse();
            var searchQuery = new SearchForImages2RequestBody();
            searchQuery.Filter = new Filter();
            searchQuery.Filter.Orientations = new List<string> { "horizontal" };
            searchQuery.Query = new Query();
            searchQuery.Query.SearchPhrase = !string.IsNullOrEmpty(_strserachterm) ? _strserachterm : "";
            searchQuery.Filter.ImageFamilies = new List<string> { "Editorial" };
            var token = _objcs.GetToken();
            _strtoken = token;
            searchQuery.ResultOptions = new ResultOptions();
            searchQuery.ResultOptions.ItemCount = 75;
            _intstartcnt = 1;
            _totalimg = 75;
            for (int i = 0; i < 60; i++)
            {
                try
                {
                    if (_totalimg >= _intstartcnt)
                    {
                        //declare objects.
                        _objs._Orientations = _strorientation;
                        _objs._startcnt = _intstartcnt;
                        _objs._imagefamilies = "Editorial";
                        var searchResponse = _objs.defaultSearch(token, searchQuery.Query.SearchPhrase);
                        var valtotal = searchResponse.SearchForImagesResult.ItemTotalCount;
                        if (_cnt == 1)
                        {
                            _totalimg = valtotal;
                            if (valtotal == 0)
                            {
                                System.Console.WriteLine("No Event added ,No Images or Event Created Daterange in StartDate =" + System.DateTime.Now.AddHours(-10).ToString("yyyy-MM-dd HH:mm") + " EndDate =" + System.DateTime.Now.AddHours(3).ToString("yyyy-MM-dd HH:mm"));
                                break;
                            }
                            _cnt++;
                        }
                        var _getimg = from images in searchResponse.SearchForImagesResult.Images
                                      from ievent in images.EventIds
                                      select new
                                      {
                                          eventid = ievent.ToString()
                                      };
                        //bind all data to image class
                        foreach (var _getdata in _getimg)
                        {
                            using (addimages _objimg = new addimages())
                            {
                                _objimg.NetworkId = _strnetworkID;
                                if (_objimg.checkGettyEvents(Convert.ToInt32(_getdata.eventid)))
                                {
                                    if (_addedevent == 1)
                                    {
                                        System.Console.WriteLine("Event added Created Daterange in StartDate =" + System.DateTime.Now.AddHours(-10).ToString("yyyy-MM-dd HH:mm") + " EndDate =" + System.DateTime.Now.AddHours(3).ToString("yyyy-MM-dd HH:mm"));
                                        _addedevent++;
                                    }
                                    if (_eventID.Count > 0)
                                    {
                                        if (!_eventID.Contains(Convert.ToInt32(_getdata.eventid)))
                                        {
                                            _eventID.Add(Convert.ToInt32(_getdata.eventid));
                                            //System.Console.WriteLine("Added EventID-: " + _getdata.eventid);
                                            System.Console.WriteLine("Event added Created Daterange in StartDate =" + System.DateTime.Now.AddHours(-10).ToString("yyyy-MM-dd HH:mm") + " EndDate =" + System.DateTime.Now.AddHours(3).ToString("yyyy-MM-dd HH:mm"));
                                        }
                                    }
                                    else
                                    {
                                        _eventID.Add(Convert.ToInt32(_getdata.eventid));
                                        //System.Console.WriteLine("Added EventID-: " + _getdata.eventid);
                                        System.Console.WriteLine("Added EventID-: " + _getdata.eventid + " Created Daterange in StartDate =" + System.DateTime.Now.AddHours(-10).ToString("yyyy-MM-dd HH:mm") + " EndDate =" + System.DateTime.Now.AddHours(3).ToString("yyyy-MM-dd HH:mm"));
                                    }
                                }
                                else
                                {
                                    System.Console.WriteLine("Already added EventID-: " + _getdata.eventid);
                                }
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("NewFN_AddGettyeventdetails Error-: " + ex.Message.ToString());
                    CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GetimageDatalist :", ex);
                    ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GetimageDatalist", "GetimageDatalist", ex.Message, _strnetworkID.ToString());

                }
                _intstartcnt += 75;
            }
            InsertGettyEvnets(_eventID);
        }

        public void InsertGettyEvnets(List<int> _eventID)
        {
            string _strtime = System.DateTime.Now.ToString("HH:mm:ss");
            List<Imagedetails> _listimg = new List<Imagedetails>();
            try
            {
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
                _strtoken = token;
                searchQuery.ResultOptions = new ResultOptions();
                searchQuery.ResultOptions.ItemCount = 75;
                _intstartcnt = 1;
                foreach (int intevent in _eventID)
                {
                    if (intevent != 0)
                    {
                        _objs._eventID = intevent;
                        _objs._Orientations = _strorientation;
                        _objs._startcnt = 1;
                        _objs._imagefamilies = "Editorial";
                        var eventrespone = _objs.EventSearch(token, searchQuery.Query.SearchPhrase);
                        var _getvalcount = eventrespone.SearchForImagesResult.ItemTotalCount;
                        var _getevrnrespons = from images in eventrespone.SearchForImagesResult.Images
                                              select new
                                              {
                                                  Titel = images.Title,
                                                  Collectname = images.CollectionName,
                                                  UrlPreview = images.UrlPreview,
                                                  UrlThumb = images.UrlThumb,
                                                  ImageId = images.ImageId,
                                                  LicensingModel = images.LicensingModel,
                                                  ImageFamily = images.ImageFamily,
                                                  DateCreated = Convert.ToDateTime(images.DateCreated).ToString("yyyy-MM-dd"),
                                                  Artist = images.Artist,
                                                  ShortCaption = images.Caption.Substring(0, 100) + "..",
                                                  Caption = images.Caption,
                                              };

                        using (addimages _objimg = new addimages())
                        {
                            foreach (var _getallevent in _getevrnrespons)
                            {
                                _objimg.EventId = intevent;
                                _objimg.NetworkId = _strnetworkID;
                                _objimg.eventImageURL = _getallevent.UrlPreview;
                                _objimg.EventTitle = _getallevent.Titel;
                                _objimg.EventDate = _getallevent.DateCreated + " " + _strtime;
                                _objimg.ImageCount = _getvalcount;
                                _objimg.imagetxtbelow = _getallevent.Caption;
                                _objimg.AddGettyEvents();
                                System.Console.WriteLine("EventID-: " + _objimg.EventId + "\t Event Title: " + _objimg.EventTitle + " \t image count: " + _objimg.ImageCount + " \t EventDate: " + _objimg.EventDate + " \t addedDate: " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                _objimg.EventId = 0;
                                _objimg.eventImageURL = "";
                                _objimg.EventTitle = "";
                                _objimg.imagetxt = "";
                                _objimg.ImageCount = 0;
                                break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("InsertGettyEvnets Error-: " + ex.Message.ToString());
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GetimageDatalist :", ex);
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GetimageDatalist", "GetimageDatalist", ex.Message, _strnetworkID.ToString());

            }

        }

        public List<Imagedetails> GetDefaultimageDatalist()
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
                searchQuery.Filter.EditorialSegments = new List<string> { "Sports" };
                searchQuery.Query = new Query();
                string _dtstart = System.DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd");
                string _dtend = System.DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                searchQuery.Query.DateCreatedRange.StartDate = _dtstart;
                searchQuery.Query.DateCreatedRange.EndDate = _dtend;
                searchQuery.Query.SearchPhrase = !string.IsNullOrEmpty(_strserachterm) ? _strserachterm : "Sports";
                //searchQuery.Filter.ImageFamilies = new List<string> { "Editorial" };
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
                var _getimg = from images in searchResponse.SearchForImagesResult.Images
                              //orderby images.DateCreated descending
                              //from ievent in images.EventIds
                              select new
                              {
                                  Titel = images.Title,
                                  Collectname = images.CollectionName,
                                  UrlPreview = images.UrlPreview,
                                  UrlThumb = images.UrlThumb,
                                  ImageId = images.ImageId,
                                  LicensingModel = images.LicensingModel,
                                  ImageFamily = images.ImageFamily,
                                  DateCreated = Convert.ToDateTime(images.DateCreated).ToString("yyyy-MM-dd HH:mm"),
                                  Artist = images.Artist,
                                  ShortCaption = images.Caption.Substring(0, 100) + "..",
                                  Caption = images.Caption
                                  //eventid = ievent.ToString()
                              };

                //bind all data to image class
                foreach (var _getdata in _getimg)
                {
                    Imagedetails _objimgdetails = new Imagedetails();
                    _objimgdetails.Artist = _getdata.Artist;
                    _objimgdetails.CollectionName = _getdata.Collectname;
                    _objimgdetails.DateCreated = Convert.ToDateTime(_getdata.DateCreated);
                    _objimgdetails.ImageFamily = _getdata.ImageFamily;
                    _objimgdetails.ImageId = _getdata.ImageId;
                    _objimgdetails.LicensingModel = _getdata.LicensingModel;
                    _objimgdetails.UrlPreview = _getdata.UrlPreview;
                    _objimgdetails.UrlThumb = _getdata.UrlThumb;
                    _objimgdetails.Title = _getdata.Titel;
                    _objimgdetails.Caption = _getdata.Caption;
                    _objimgdetails.ShortCaption = _getdata.ShortCaption;
                    //_objimgdetails._gotevent = Convert.ToInt32(_getdata.eventid);
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

        public void Get_gettyevents_to_updatecount()
        {
            DataTable _dt;
            int _intoldcnt = 0;
            try
            {
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
                _strtoken = token;
                searchQuery.ResultOptions = new ResultOptions();
                searchQuery.ResultOptions.ItemCount = 75;
                _intstartcnt = 1;
                using (addimages _objimg = new addimages())
                {
                    _objimg.NetworkId = _strnetworkID;
                    _dt = _objimg.GetGettyevents_toupdatecount();
                    if (_dt.Rows.Count > 0)
                    {
                        foreach (DataRow _dr in _dt.Rows)
                        {
                            if (!string.IsNullOrEmpty(_dr["eventID"].ToString()))
                            {
                                _objs._eventID = Convert.ToInt32(_dr["eventID"].ToString());
                                _objs._Orientations = _strorientation;
                                _objs._startcnt = 1;
                                _objs._imagefamilies = "Editorial";
                                var eventrespone = _objs.EventSearch(token, searchQuery.Query.SearchPhrase);
                                var _getvalcount = eventrespone.SearchForImagesResult.ItemTotalCount;
                                _intoldcnt = (int)(_dr["ImageCount"]);
                                if (_getvalcount > _intoldcnt)
                                {
                                    _objimg.Update_GettyEvents_imagecount(_getvalcount, _objs._eventID);
                                    System.Console.WriteLine("Updated Record EventID-: " + _objs._eventID + "\t Event Title: " + _dr["EventTitle"].ToString() + " \t Old image count: " + _dr["ImageCount"].ToString() + "\tNew Image count: " + _getvalcount);
                                }
                                else
                                {
                                    System.Console.WriteLine("Not Updated Record EventID-: " + _objs._eventID + "\t Event Title: " + _dr["EventTitle"].ToString() + " \t Old image count: " + _dr["ImageCount"].ToString() + "\tNew Image count: " + _getvalcount);
                                }
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GetimageDatalist :", ex);
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "Get_gettyents_to_updatecount", "Get_gettyents_to_updatecount", ex.Message, _strnetworkID.ToString());

            }

        }


        public int Get_gettyevents_Responseimagecount(string _intreseventID)
        {
            DataTable _dt;
            int _intoldcnt = 1;
            int _inttotalcnt = 0;
            try
            {
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
                _strtoken = token;
                searchQuery.ResultOptions = new ResultOptions();
                searchQuery.ResultOptions.ItemCount = 75;
                _intstartcnt = 1;
                if (!string.IsNullOrEmpty(_intreseventID))
                {
                    _objs._eventID = Convert.ToInt32(_intreseventID);
                    _objs._Orientations = _strorientation;
                    _objs._startcnt = 1;
                    _objs._imagefamilies = "Editorial";
                    var eventrespone = _objs.EventSearch(token, searchQuery.Query.SearchPhrase);
                    var _getvalcount = eventrespone.SearchForImagesResult.ItemTotalCount;
                    _inttotalcnt = _getvalcount;
                }

            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write("imagecount" + ex);
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "Get_gettyevents_Responseimagecount :", ex);
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "Get_gettyents_to_updatecount", "Get_gettyents_to_updatecount", ex.Message, _strnetworkID.ToString());

            }
            return _inttotalcnt;

        }







        public string getlargeimage_download_DandD(List<string> IDs)
        {
            string _strURL = string.Empty;
            List<Image> _liimg = new List<Image>();
            List<ImageSize> _liimgsize = new List<ImageSize>();
            List<DownloadItem> lidwn = new List<DownloadItem>();
            List<Imagedetails> _liimgdetails = new List<Imagedetails>();
            bool _flag = false;
            try
            {
                //declare objects.
                CreateSessionSample _objcs = new CreateSessionSample();
                var token = _objcs.GetToken();
                var strsecuretoken = _objcs.GetSecureToken();
                _strtoken = token;
                _strsecuretoken = strsecuretoken;
                GetLargestImageDownloadAuthorizationsSample _objdwnl = new GetLargestImageDownloadAuthorizationsSample();
                GetImageDownloadAuthorizationsSample _objsmall = new GetImageDownloadAuthorizationsSample();
                //set imeges ID
                if (!string.IsNullOrEmpty(token))
                {
                    if (IDs.Count > 0)
                    {
                        foreach (var _ids in IDs)
                        {
                            //ad image details to class 
                            Image _objimg = new Image();
                            _objimg.ImageId = _ids;
                            _liimg.Add(_objimg);
                        }
                        //genrate tokens for download image
                        var _gotimg = _objdwnl.GetLargestDownloadForImages(token, _liimg);
                        var _objr = from res in _gotimg.GetLargestImageDownloadAuthorizationsResult.Images
                                    from auths in res.Authorizations
                                    select new
                                    {
                                        //image authorization
                                        ImageIDs = res.ImageId,
                                        DownloadIsFree = auths.DownloadIsFree,
                                        DownloadToken = auths.DownloadToken,
                                        ProductOfferingInstanceId = auths.ProductOfferingInstanceId,
                                        ProductOfferingType = auths.ProductOfferingType,
                                        SizeKey = auths.SizeKey
                                    };
                        foreach (var _imgdwn in _objr)
                        {
                            //create instance
                            Authorization _objauth = new Authorization();
                            DownloadItem _objdw = new DownloadItem();
                            GetImageDownloadAuthorizationsSample _objimg = new GetImageDownloadAuthorizationsSample();
                            CreateDownloadRequestSample _objcrtdwn = new CreateDownloadRequestSample();
                            ImageSize _imgsize = new ImageSize();
                            //get all image details from data sectect
                            GetImageDetailsSample _objgetdetails = new GetImageDetailsSample();
                            var getdetialsimg = _objgetdetails.GetImageDetails(token, IDs);
                            var _getmoredetails = from images in getdetialsimg.GetImageDetailsResult.Images
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

                            _objauth.SizeKey = _imgdwn.SizeKey;
                            _objauth.DownloadIsFree = _imgdwn.DownloadIsFree;
                            _objauth.DownloadToken = _imgdwn.DownloadToken;
                            _objauth.ProductOfferingInstanceId = _imgdwn.ProductOfferingInstanceId;
                            _objauth.ProductOfferingType = _imgdwn.ProductOfferingType;
                            _imgsize.SizeKey = _imgdwn.SizeKey;
                            _imgsize.ImageId = _imgdwn.ImageIDs;
                            _liimgsize.Add(_imgsize);
                            _objdw.DownloadToken = _objauth.DownloadToken;
                            lidwn.Add(_objdw);
                            var _getdwnurl = _objcrtdwn.CreateRequest(strsecuretoken, lidwn);
                            var _popupurl = from _popdwn in _getdwnurl.CreateDownloadRequestResult.DownloadUrls
                                            select new
                                            {
                                                dwnlargeimage = _popdwn.UrlAttachment
                                            };
                            foreach (var getvalur in _popupurl)
                            {
                                //download image 
                                string imageName = string.Empty;
                                string filename = string.Empty;
                                string _returnfile = string.Empty;
                                filename = "large_" + DateTime.Now.ToString("yyyyMMMddhhmmss") + ".jpg";
                                try
                                {
                                    _strURL = getvalur.dwnlargeimage;
                                }
                                catch (Exception ex)
                                {
                                    CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "webClient.Dispose() :", ex);
                                    ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "getlargeimage_download_DandD", "getlargeimage_download_DandD", ex.Message, _strnetworkID.ToString());
                                }

                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "getlargeimage_download_DandD :", ex);
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "getlargeimage_download_DandD", "getlargeimage_download_DandD", ex.Message, _strnetworkID.ToString());
            }
            return _strURL;

        }

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
        #endregion


        public string GetdownloadsingleimagesLiveUpdate(string IDs)
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
            List<string> imgdinfo = new List<string>();
            try
            {
                GetLargestImageDownloadAuthorizationsSample _objdwnl = new GetLargestImageDownloadAuthorizationsSample();
                GetImageDownloadAuthorizationsSample _objsmall = new GetImageDownloadAuthorizationsSample();
                if (!string.IsNullOrEmpty(commonfn.getSession("token")))
                {
                    var token = commonfn.getSession("token");
                    var securetoken = commonfn.getSession("securetoken");
                    //set imeges ID
                    if (!string.IsNullOrEmpty(IDs))
                    {

                        imgdinfo.Add(IDs);
                        //new code to download images
                        GetImageDetailsSample _objgetalldetails = new GetImageDetailsSample();
                        ImageSize _objimgsize = new ImageSize();
                        Authorization _objallauth = new Authorization();
                        DownloadItem _objalldw = new DownloadItem();
                        GetImageDownloadAuthorizationsSample _objallimg = new GetImageDownloadAuthorizationsSample();
                        CreateDownloadRequestSample _objallcrtdwn = new CreateDownloadRequestSample();
                        //get all data for selected images.
                        var _getalldetails = _objgetalldetails.GetImageDetails(token, imgdinfo);
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
                                        _returnfile = GoWebshotLiveUpdate(goturl.dwnlargeimage, _objall.ImageId);
                                    }

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
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "Getdownloadsingleimages", "GetdownloadsingleimagesLiveUpdate", ex.Message, _strnetworkID.ToString());
            }
            return _returnfile;
        }


        private string GoWebshotLiveUpdate(string strurl, string imgID)
        {
            string _imagepathdownload = "";
            string sitefolder = "";
            string monthyearfolder = DateTime.Now.ToString("yyyyMM");
            string dayfolder = DateTime.Now.ToString("MMMdd");
            string filename = "";
            string orgfilename = "";
            string imgSrcURL = strurl;
            string path = null;
            string pathclient = null;
            string Tnname = string.Empty;
            string orgname = string.Empty;
            bool result = false;
            SiteInfo objsite = new SiteInfo();
            objsite.NetWorkID = _strnetworkID.ToString();
            objsite.GetSiteDetails(_strsiteID, _strnetworkID.ToString());
            if (objsite.ImageServer)
            {
                sitefolder = Function.GetSiteFolderName(objsite.SiteUrl);
                Function.CreateImagesFolderInBackend(sitefolder, monthyearfolder, dayfolder);
                pathclient = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}/{2}/", sitefolder, monthyearfolder, dayfolder));
                if (!string.IsNullOrEmpty(_strnews))
                    _imagepathdownload = pathclient;
                else
                    _imagepathdownload = ConfigurationSettings.AppSettings["ImagePath"];
            }
            else
            {
                pathclient = HttpContext.Current.Server.MapPath("images/newsimages/");
                _imagepathdownload = ConfigurationSettings.AppSettings["ImagePath"];
            }
            try
            {
                string imgExt = Path.GetExtension(imgSrcURL);
                string imageName = string.Empty;
                imageName = Function.ToFileName((imgSrcURL));
                orgfilename = "getty_" + DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                filename = DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadFile(imgSrcURL, HttpContext.Current.Server.MapPath("~/newsliveupdate/") + filename);

                webClient.Dispose();



            }
            catch (Exception exp)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GoWebshot :" + _strnetworkID, exp);
                ErrorLog.SaveErrorLog(_strsiteID, "getdata.cs class", "GoWebshot", "GoWebshot", exp.Message, _strnetworkID.ToString());
            }
            return filename;
        }

    }
}
