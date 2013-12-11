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
namespace Gettyclasses
{
    public class Class1
    {

        public string _dnloadurl { get; set; }
        public string _dnloadurlID { get; set; }
        public List<Imagedetails>    getser()
        {
            List<string> li=new List<string> {};
            List<Image> limg = new List<Image> { };
            List<Imagedetails> _liimgsetails = new List<Imagedetails>();    
            List<DownloadItem> lidwn = new List<DownloadItem> { };   
            List<ImageSize> limgsize = new List<ImageSize> { }; 
            DataTable _dt = new DataTable();
            DataRow _dr=null;
            SearchForEditorialImagesSample s = new SearchForEditorialImagesSample();
            CreateSessionRequestBody serr = new CreateSessionRequestBody();
            CreateSessionSample cs=new CreateSessionSample();
            //create secure token
            
            //s.Search(token);
            //SearchForImages2RequestBody serbody = new SearchForImages2RequestBody();
            //SearchForImagesResult resu = new SearchForImagesResult();
            var searchQuery = new SearchForImages2RequestBody();
            searchQuery.Filter = new Filter();
            searchQuery.Query = new Query();
            searchQuery.Query.SearchPhrase = "football";
            var token = cs.GetToken();
            var securetoken = cs.GetSecureToken(); 
            //searchQuery.Query.SpecificPersons = new List<string> {_strtext};
            //searchQuery.Query.SpecificPersons = new List<string> { "Jennifer Lopez" }; ;
            searchQuery.ResultOptions = new ResultOptions();
            searchQuery.ResultOptions.ItemCount = 75;
            var searchResponse = s.Search(token,searchQuery.Query.SearchPhrase);
            var getimg = from images in searchResponse.SearchForImagesResult.Images
                                   select new
                                   {
                                       Titel = images.Title,
                                       Collectname = images.CollectionName,
                                       urlpre=images.UrlPreview,
                                       urlthumb=images.UrlThumb,
                                       ImagID=images.ImageId, 
                                       imglic=images.LicensingModel,
                                       imgfamily=images.ImageFamily,  
                                       imgdate=images.DateCreated,
                                       imgart=images.Artist  
                                        
                                   };
            _dt.Columns.Add("urlthumb", typeof(string));
            _dt.Columns.Add("urlpre", typeof(string));
            GetLargestImageDownloadAuthorizationsRequestBody b = new GetLargestImageDownloadAuthorizationsRequestBody();
            Image i = new Image();
            ImageSize _imgsize = new ImageSize();
            
            int cnt = 1;
            bool _got = false;
            foreach (var img in getimg)
            {
                if (cnt == 4)
                {
                    i.ImageId = img.ImagID;
                    i.ImageFamily = img.imgfamily;
                    i.LicensingModel = img.imglic;
                    i.UrlPreview = img.urlpre;
                    i.UrlThumb = img.urlthumb;
                    i.CollectionName = img.Collectname;
                    i.Title = img.Titel;
                    _got = true;
                }
                _dr = _dt.NewRow();
                _dr["urlthumb"] = img.urlthumb;
                _dr["urlpre"] = img.urlpre;
                _dt.Rows.Add(_dr);
                Imagedetails _objimgdetails = new Imagedetails();
                _objimgdetails.Artist=img.imgart;
                _objimgdetails.CollectionName = img.Collectname;
                _objimgdetails.DateCreated=img.imgdate;
                _objimgdetails.ImageFamily=img.imgfamily;
                _objimgdetails.ImageId=img.ImagID;
                _objimgdetails.LicensingModel=img.imglic;
                _objimgdetails.UrlPreview=img.urlpre;
                _objimgdetails.UrlThumb=img.urlthumb;
                _liimgsetails.Add(_objimgdetails);   
                cnt++;
            }
            
            GetLargestImageDownloadAuthorizationsResponse bb = new GetLargestImageDownloadAuthorizationsResponse();
            GetLargestImageDownloadAuthorizationsSample bbb = new GetLargestImageDownloadAuthorizationsSample();
            if (_got)
            {
                limg.Add(i);
                var strjn = bbb.GetLargestDownloadForImages(token, limg);
                var _objr = from res in strjn.GetLargestImageDownloadAuthorizationsResult.Images
                            from auths in res.Authorizations
                            select new
                            {
                                ImageIDs = res.ImageId,
                                DownloadIsFree = auths.DownloadIsFree,
                                DownloadToken = auths.DownloadToken,
                                ProductOfferingInstanceId = auths.ProductOfferingInstanceId,
                                ProductOfferingType = auths.ProductOfferingType,
                                SizeKey = auths.SizeKey
                            };

                foreach (var _testauth in _objr)
                {

                    //get image valu
                    GetImageDownloadAuthorizationsSample _objimg = new GetImageDownloadAuthorizationsSample();
                    Authorization _objaut = new Authorization();
                    DownloadItem _objdw = new DownloadItem();
                    CreateDownloadRequestSample _objcrtdwn = new CreateDownloadRequestSample();
                    _objaut.SizeKey = _testauth.SizeKey;
                    _objaut.DownloadIsFree = _testauth.DownloadIsFree;
                    _objaut.DownloadToken = _testauth.DownloadToken;
                    _objaut.ProductOfferingInstanceId = _testauth.ProductOfferingInstanceId;
                    _objaut.ProductOfferingType = _testauth.ProductOfferingType;
                    _imgsize.SizeKey = _testauth.SizeKey;
                    _imgsize.ImageId = _testauth.ImageIDs;
                    _dnloadurlID = _testauth.ImageIDs;
                    limgsize.Add(_imgsize);
                    _objdw.DownloadToken = _objaut.DownloadToken;
                    lidwn.Add(_objdw);
                    var _getdownlod = _objimg.AuthorizeDownload(token, limgsize);
                    var _getdwnurl = _objcrtdwn.CreateRequest(securetoken, lidwn);
                    var _popupurl = from _popdwn in _getdwnurl.CreateDownloadRequestResult.DownloadUrls
                                    select new
                                    {
                                        dwnlargeimage = _popdwn.UrlAttachment
                                    };
                    foreach (var getvalur in _popupurl)
                    {
                        _dnloadurl = getvalur.dwnlargeimage;
                    }
                }
                // JObject node = JObject.Parse(strjn);
                /*_n.DownloadIsFree=Convert.ToBoolean(node["DownloadIsFree"]);
                _n.DownloadToken = Convert.ToString(node["DownloadToken"]);
                _n.ProductOfferingInstanceId = Convert.ToString(node["ProductOfferingInstanceId"]);
                _n.ProductOfferingType = Convert.ToString(node["ProductOfferingType"]);
                _in.ImageId = Convert.ToString(node["ImageId"]);
                _in.SizeKey = Convert.ToString(node["SizeKey"]);*/
                /* var jArray = JArray.Parse(strjn.ToString());
                 var countyNames = new List<string>();
                 foreach (var element in jArray.SelectToken("[0]"))
                 {
                     var value = element.SelectToken("Authorizations.DownloadToken").Value<string>();
                     countyNames.Add(value);
                 }*/
            }

            return _liimgsetails; 
            
        }
    }

}
