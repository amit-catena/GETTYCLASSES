using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GettyImages.Api;
using Newtonsoft.Json;
using System.Configuration;
using System.Web;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.Web.Http;
using NUnit.Framework;
using TechTalk.SpecFlow;
using GettyImages.Api.Search;
using NewGettyAPIclasses.StepBindings;
using System.Collections;

namespace NewGettyAPIclasses
{
    /// <summary>
    /// Getimagedata class
    /// </summary>
    public class Getimagedata : IDisposable
    {
        /// <summary>
        /// public members
        /// </summary>
        #region Public Memberrs
        public string strAccesstoken = string.Empty;
        public static StringBuilder _strhtmldat = new StringBuilder();
        public static int pagesize = 30;
        public string strnetworkid{get;set;}
        public int SiteID {get;set;}
        public int TNWidth { get; set; }
        public int TNHeight { get; set; }
        public int TN_TNWidth { get; set; }
        public int TN_TNHeight { get; set; }
        #endregion
        public Getimagedata()
        {
        }
        ~Getimagedata()
        {
            this.Dispose();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #region intial

        public static string Getsearchimages(string searchterm)
        {
            string data = string.Empty;
            try
            {
                List<string> l = new List<string>();
                var apikey = ConstantAPI.API_KEY; 
                var apiscretkey = ConstantAPI.API_SecretKEY; 
                var client = ApiClient.GetApiClientWithResourceOwnerCredentials(apikey, apiscretkey, ConstantAPI.Username, ConstantAPI.Password);
                data = DoSearchwebfortest(client, searchterm);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return data;
        }
        private static string DoSearch(ApiClient client, string term)
        {
            var phraseToSearchFor = string.Empty;
            var results = client.Search().Images().Editorial().WithResponseField("comp").WithPhrase(term).WithPageSize(5).ExecuteAsync().Result;
            phraseToSearchFor = JsonConvert.SerializeObject(results, Formatting.Indented);
            return phraseToSearchFor;
        }

        private static string DoSearchweb(ApiClient client, string term)
        {
            var phraseToSearchFor = string.Empty;
            try
            {
                var results = client.Search().Images().Editorial().ExecuteAsync().Result;
                phraseToSearchFor = JsonConvert.SerializeObject(results, Formatting.Indented);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return phraseToSearchFor.ToString();
        }

        public static string DoSearchwebfortest(ApiClient client, string term)
        {
            var phraseToSearchFor = string.Empty;
            try
            {
                var results = client.Search().Images().Editorial().ExecuteAsync();
                phraseToSearchFor = JsonConvert.SerializeObject(results, Formatting.Indented);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return phraseToSearchFor.ToString();
        }

        #endregion

        #region Asyn call methods
        /// <summary>
        /// get asyn call for gettyimages API
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async static Task<StringBuilder> AccessTheWebAsync(string search,int page,string type)
        {
            StringBuilder HtmlData = new StringBuilder();
            if (string.IsNullOrEmpty(search))
                search = "sports";

            // You need to add a reference to System.Net.Http to declare client.
            // GetStringAsync returns a Task<string>. That means that when you await the 
            var clienttask = ApiClient.GetApiClientWithResourceOwnerCredentials(ConstantAPI.API_KEY, ConstantAPI.API_SecretKEY, ConstantAPI.Username, ConstantAPI.Password);
            var phraseToSearchFor = string.Empty;
            var results = await clienttask.Search().Images().WithResponseField("preview,caption,event_ids,title").WithPhrase(search)
                .WithSortOrder("newest").WithPageSize(pagesize).WithPage(page).WithOrientation(GettyImages.Api.Search.Entity.Orientation.Horizontal).ExecuteAsync();
            phraseToSearchFor = JsonConvert.SerializeObject(results, Formatting.Indented);
            if (!String.IsNullOrEmpty(phraseToSearchFor))
            { 
            // You can do work here that doesn't rely on the string from GetStringAsync.
            dynamic array = JsonConvert.DeserializeObject(phraseToSearchFor);
            List<imagedetails> lisimg = new List<imagedetails>();
            foreach (var item in array)
            {
                if (item.Name == "images")
                {
                    dynamic array2 = item.Value;
                    foreach (var l in array2)
                    {
                        var innerl = l;
                        imagedetails img = new imagedetails();
                        foreach (var r in innerl)
                        {
                            if (r.Name == "id")
                            {
                                img.ID = Convert.ToInt32(r.Value); ;
                            }
                            if (r.Name == "caption")
                            {
                                img.caption = Convert.ToString(r.Value);
                            }
                            if (r.Name == "title")
                            {
                                img.Title = Convert.ToString(r.Value);
                                lisimg.Add(img);
                            }
                            if (r.Name == "display_sizes")
                            {
                                var display = r.Value;
                                foreach (var rr in display)
                                {
                                    img.previewURL = Convert.ToString(rr["uri"].Value);
                                }
                            }
                        }
                    }

                }
            }
            //--------------append images to Html-----------//
            if (lisimg.Count > 0)
            {

                foreach (var dr in lisimg)
                {
                    if (type == "Y")
                        HtmlData.Append(string.Format(ALLHtmlstring.searchimagesmultipleli, dr.previewURL, dr.ID, dr.caption, dr.Title));
                    else
                        HtmlData.Append(string.Format(ALLHtmlstring.searchimagesli, dr.previewURL, dr.ID, dr.caption, dr.Title));

                }
            }
            else
            {
                HtmlData.Append("<li>Sorry, your search returned zero results for \"" + search + "\"</li>");
            }
            }
            else
            {
                HtmlData.Append("<li>Sorry, your search returned zero results for \"" + search + "\"</li>");
            }

            return HtmlData;
        }
        /// <summary>
        /// download image with help of image id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async static Task<StringBuilder> AccessTheWebAsync(string search, int page, string type, string sortorder, string Orientation,string dateorder)
        {
            StringBuilder HtmlData = new StringBuilder();
            string order = string.Empty;
            string[] orient = null;
            string[] date = null;
            DateTime dt = System.DateTime.Now;
            string enddate = DateTime.Parse(dt.ToString()).ToString("yyyy-MM-dd");
            string startdate = DateTime.Parse(dt.ToString()).ToString("yyyy-MM-dd");
            var phraseToSearchFor = string.Empty;
            try
            {
              
                if (string.IsNullOrEmpty(search))
                    search = "sports";
                orient = Orientation.Split(',');
              
                var clienttask = ApiClient.GetApiClientWithResourceOwnerCredentials(ConstantAPI.API_KEY, ConstantAPI.API_SecretKEY, ConstantAPI.Username, ConstantAPI.Password);
                var results = clienttask.Search().Images().Editorial();
                // GetStringAsync returns a Task<string>. That means that when you await the 
                if (orient.Length > 0)
                {

                    foreach (var o in orient)
                    {
                        switch (o.ToString())
                        {
                            case "Horizontal":
                                results = results.WithOrientation(GettyImages.Api.Search.Entity.Orientation.Horizontal);
                                break;
                            case "PanoramicHorizontal":
                                results = results.WithOrientation(GettyImages.Api.Search.Entity.Orientation.Panoramic_Horizontal);
                                break;
                            case "PanoramicVertical":
                                results = results.WithOrientation(GettyImages.Api.Search.Entity.Orientation.Panoramic_Vertical);
                                break;
                            case "Square":
                                results = results.WithOrientation(GettyImages.Api.Search.Entity.Orientation.Square);
                                break;
                            case "Vertical":
                                results = results.WithOrientation(GettyImages.Api.Search.Entity.Orientation.Vertical);
                                break;
                            default:
                                results = results.WithOrientation(GettyImages.Api.Search.Entity.Orientation.Horizontal);
                                break;
                        }
                    }
                }
                if (dateorder == "0")
                {

                }
                else
                {
                    date = dateorder.Split('-');
                    if (date.Length > 1)
                    {
                        enddate = DateTime.Parse(dt.ToString()).ToString("yyyy-MM-dd");
                        if (Convert.ToInt32(date[0]) == 12)
                        {
                            startdate = DateTime.Parse(dt.AddMonths(-Convert.ToInt32(date[0])).ToString()).ToString("yyyy-MM-dd");
                            sortorder = "oldest";
                        }
                        else
                        {
                            startdate = DateTime.Parse(dt.AddDays(-Convert.ToInt32(date[0])).ToString()).ToString("yyyy-MM-dd");
                            //sortorder = "none";
                            sortorder = "oldest";
                        }

                        results = results.WithDateFrom(startdate).WithDateTo(enddate);
                       
                    }
                    else
                    {
                        enddate = DateTime.Parse(dt.ToString()).ToString("yyyy-MM-dd");
                        startdate = DateTime.Parse(dt.AddHours(-Convert.ToInt32(dateorder)).ToString()).ToString("yyyy-MM-dd");
                        results = results.WithDateFrom(startdate).WithDateTo(enddate);
                        //sortorder = "none";
                        sortorder = "oldest";
                    }
                }

                if (sortorder=="none")
                {
                    var exeresults = await results.WithResponseField("preview,caption,event_ids,title").WithPhrase(search)
                       .WithPageSize(pagesize).WithPage(page).ExecuteAsync();
                    phraseToSearchFor = JsonConvert.SerializeObject(exeresults, Formatting.Indented);

                }
                else
                {
                    var exeresults = await results.WithResponseField("preview,caption,event_ids,title").WithPhrase(search)
                  .WithSortOrder(sortorder).WithPageSize(pagesize).WithPage(page).ExecuteAsync();
                    phraseToSearchFor = JsonConvert.SerializeObject(exeresults, Formatting.Indented);
                }
                if (!String.IsNullOrEmpty(phraseToSearchFor))
                {
                    // You can do work here that doesn't rely on the string from GetStringAsync.
                    dynamic array = JsonConvert.DeserializeObject(phraseToSearchFor);
                    List<imagedetails> lisimg = new List<imagedetails>();
                    foreach (var item in array)
                    {
                        if (item.Name == "images")
                        {
                            dynamic array2 = item.Value;
                            foreach (var l in array2)
                            {
                                var innerl = l;
                                imagedetails img = new imagedetails();
                                foreach (var r in innerl)
                                {
                                    if (r.Name == "id")
                                    {
                                        img.ID = Convert.ToInt32(r.Value); ;
                                    }
                                    if (r.Name == "caption")
                                    {
                                        img.caption = Convert.ToString(r.Value);
                                    }
                                    if (r.Name == "title")
                                    {
                                        img.Title = Convert.ToString(r.Value);
                                        lisimg.Add(img);
                                    }
                                    if (r.Name == "display_sizes")
                                    {
                                        var display = r.Value;
                                        foreach (var rr in display)
                                        {
                                            img.previewURL = Convert.ToString(rr["uri"].Value);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    //--------------append images to Html-----------//
                    if (lisimg.Count > 0)
                    {

                        foreach (var dr in lisimg)
                        {
                            if (type == "Y")
                                HtmlData.Append(string.Format(ALLHtmlstring.searchimagesmultipleli, dr.previewURL, dr.ID, dr.caption, dr.Title));
                            else
                                HtmlData.Append(string.Format(ALLHtmlstring.searchimagesli, dr.previewURL, dr.ID, dr.caption, dr.Title));

                        }
                    }
                    else
                    {
                        HtmlData.Append("<li>Sorry, your search returned zero results for \"" + search + "\"</li>");
                    }
                }
                else
                {
                    HtmlData.Append("<li>Sorry, your search returned zero results for \"" + search + "\"</li>");
                }
            }catch(Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "AccessTheWebAsync :", ex);

            }
            return HtmlData;
        }
        /// <summary>
        /// Download getty image by Asysnc methode 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async static Task<string> AccessTheWebAsync_downloadimage(string ID)
        {
            StringBuilder HtmlData = new StringBuilder();
            string struri = string.Empty;
            // You need to add a reference to System.Net.Http to declare client.
            // GetStringAsync returns a Task<string>. That means that when you await the 
            var clienttask = ApiClient.GetApiClientWithResourceOwnerCredentials(ConstantAPI.API_KEY, ConstantAPI.API_SecretKEY, ConstantAPI.Username, ConstantAPI.Password);
            var phraseToSearchFor = string.Empty;
            var tokenstr = await clienttask.GetAccessToken();
            var token = tokenstr.AccessToken;
            var results = await clienttask.Download().Image().WithFileType(GettyImages.Api.Search.Entity.FileType.Jpg).WithId(ID).ExecuteAsync();   
            phraseToSearchFor = JsonConvert.SerializeObject(results, Formatting.Indented);
            // You can do work here that doesn't rely on the string from GetStringAsync.
            dynamic array = JsonConvert.DeserializeObject(phraseToSearchFor);
            //--------------append images to Html-----------//
            foreach (var   item in  array)
            {
                if (item.Name == "uri")
                {
                    var dd = item;
                    foreach(var i in dd)
                    {
                         struri = Convert.ToString(i);
                    }
                }
            }
            return struri;
        }

        #region Multiple Images
        StringBuilder jsonimageData = new StringBuilder();
        /// <summary>
        /// Download multiple selected images by Asysnc methode
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public async Task<string> AccessTheWebAsync_download_multiple_image(List<string> strID)
        {
            StringBuilder HtmlData = new StringBuilder();
            string struri = string.Empty;
            int totalimg = 1;
            jsonimageData.Append("{\"imagedetails\":{");
            foreach (var ID in strID)
            {
                var clienttask = ApiClient.GetApiClientWithResourceOwnerCredentials(ConstantAPI.API_KEY, ConstantAPI.API_SecretKEY, ConstantAPI.Username, ConstantAPI.Password);
                var phraseToSearchFor = string.Empty;
                var imagecaption = string.Empty;
                var tokenstr = await clienttask.GetAccessToken();
                var token = tokenstr.AccessToken;
                var results = await clienttask.Download().Image().WithFileType(GettyImages.Api.Search.Entity.FileType.Jpg).WithId(ID).ExecuteAsync();
                var results2 = await clienttask.Images().WithId(ID).WithResponseField("caption").ExecuteAsync();
                phraseToSearchFor = JsonConvert.SerializeObject(results, Formatting.Indented);
                imagecaption = JsonConvert.SerializeObject(results2, Formatting.Indented);
                // You can do work here that doesn't rely on the string from GetStringAsync.
                dynamic array = JsonConvert.DeserializeObject(phraseToSearchFor);
                dynamic arraycaption = JsonConvert.DeserializeObject(imagecaption);
                //--------------append images to Html-----------//
                foreach (var item in array)
                {
                    if (item.Name == "uri")
                    {
                        var dd = item;
                        foreach (var i in dd)
                        {
                            struri = Convert.ToString(i);
                            foreach (var inner in arraycaption)
                            {
                                if (inner.Name == "images")
                                {
                                    dynamic array2 = inner.Value;
                                     foreach (var l in array2)
                                     {
                                          var innerl = l;
                                          foreach (var r in innerl)
                                          {
                                              if (r.Name == "caption")
                                              {
                                                  jsonimageData.Append(save_multiple_imagesInsitefolder(struri, ID, totalimg, Convert.ToString(r.Value)));
                                              }
                                          }
                                     }
                                }
                            }
                        }
                    }
                }
                totalimg++;
                
            }
            jsonimageData.Append("}}");
            return jsonimageData.ToString();
        }
        /// <summary>
        /// Asysnc function for multiple image with caption
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>

        public async Task<string> AccessTheWebAsync_download_multiple_imagewithcaption(Dictionary<int,string> strID)
        {
            StringBuilder HtmlData = new StringBuilder();
            string struri = string.Empty;
            int totalimg = 1;
            jsonimageData.Append("{\"imagedetails\":{");
            foreach (KeyValuePair<int,string> ID in strID)
            {
                var clienttask = ApiClient.GetApiClientWithResourceOwnerCredentials(ConstantAPI.API_KEY, ConstantAPI.API_SecretKEY, ConstantAPI.Username, ConstantAPI.Password);
                var phraseToSearchFor = string.Empty;
                var imagecaption = string.Empty;
                var tokenstr = await clienttask.GetAccessToken();
                var token = tokenstr.AccessToken;
                string strimageID = Convert.ToString(ID.Key);
                var results = await clienttask.Download().Image().WithFileType(GettyImages.Api.Search.Entity.FileType.Jpg).WithId(strimageID).ExecuteAsync();
                phraseToSearchFor = JsonConvert.SerializeObject(results, Formatting.Indented);
                // You can do work here that doesn't rely on the string from GetStringAsync.
                dynamic array = JsonConvert.DeserializeObject(phraseToSearchFor);
                dynamic arraycaption = JsonConvert.DeserializeObject(imagecaption);
                //--------------append images to Html-----------//
                foreach (var item in array)
                {
                    if (item.Name == "uri")
                    {
                        var dd = item;
                        foreach (var i in dd)
                        {
                            struri = Convert.ToString(i);
                            jsonimageData.Append(save_multiple_imagesInsitefolder(struri, strimageID, totalimg, ID.Value));
                        }
                    }
                }
                strimageID = string.Empty;
                totalimg++;
            }
            jsonimageData.Append("}}");
            return jsonimageData.ToString();
        }
        /// <summary>
        /// Save selected multiple image  in pix folder
        /// </summary>
        /// <param name="strImageurl"></param>
        /// <param name="imgID"></param>
        /// <param name="imgno"></param>
        /// <param name="captiondata"></param>
        /// <returns></returns>
        private string save_multiple_imagesInsitefolder(string strImageurl, string imgID,int imgno,string captiondata)
        {
            string _imagepathdownload = "";
            StringBuilder JSonformat = new StringBuilder();
            string filename = "";
            string orgfilename = "";
            string sitefolder = string.Empty;
            string imgSrcURL = strImageurl;
            string pathclient = null;
            string Tnname = string.Empty;
            string orgname = string.Empty;
            bool result = false;
            string monthyearfolder = DateTime.Now.ToString("yyyyMM");
            string dayfolder = DateTime.Now.ToString("MMMdd");
            using (SiteInfo objsite = new SiteInfo())
            {
                objsite.NetWorkID = strnetworkid;
                objsite.GetAllsitdetails(SiteID, strnetworkid);
                sitefolder = Function.GetSiteFolderName(objsite.SiteUrl);
                if (objsite.ImageServer)
                {
                    Function.CreateImagesFolderInBackend(sitefolder, monthyearfolder, dayfolder);
                    pathclient = ConstantAPI.downloadsitepath + string.Format("{0}/{1}/{2}/", sitefolder, monthyearfolder, dayfolder);
                    _imagepathdownload = pathclient;
                }
                else
                {
                    pathclient = HttpContext.Current.Server.MapPath("images/newsimages/");
                    _imagepathdownload = pathclient;
                }
                try
                {
                    orgfilename = "getty_" + DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                    filename = DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                    Downloadimage(imgSrcURL, _imagepathdownload + orgfilename);
                    Tnname = Saveimage(Imagesetting.ImageType.NewsImage, _imagepathdownload, filename, orgfilename);
                    if (imgno==1)
                    {
                        JSonformat.Append("\"image" + imgno + "\":{");
                        JSonformat.Append("\"ID\":\"" + imgID + "\",");
                        JSonformat.Append("\"URL\":\"" + ConstantAPI.pixURL + string.Format("{0}/{1}/{2}/", sitefolder, monthyearfolder, dayfolder) + filename + "\",");
                        JSonformat.Append("\"DESC\":\"" + captiondata + "\",");
                        JSonformat.Append("\"TNName\":\"" + Tnname + "\"}");
                    }
                    else
                    {
                        JSonformat.Append(",\"image" + imgno + "\":{");
                        JSonformat.Append("\"ID\":\"" + imgID + "\",");
                        JSonformat.Append("\"URL\":\"" + ConstantAPI.pixURL + string.Format("{0}/{1}/{2}/", sitefolder, monthyearfolder, dayfolder) + filename + "\",");
                        JSonformat.Append("\"DESC\":\"" + captiondata + "\",");
                        JSonformat.Append("\"TNName\":\"" + Tnname + "\"}");
                    }
                   
                }
                catch (Exception exp)
                {
                    CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GoWebshot :", exp);
                }
                imgID = "";
            }

            return JSonformat.ToString();
        }
        #endregion
        /// <summary>
        /// Download selected images
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="caption"></param>
        /// <param name="live"></param>
        /// <returns></returns>
        public async Task<string> AccessTheWebAsync_downloadimage_fromID(string ID,string caption,string live)
        {
            StringBuilder HtmlData = new StringBuilder();
            string struri = string.Empty;
            string strfile = string.Empty;
            // You need to add a reference to System.Net.Http to declare client.
            // GetStringAsync returns a Task<string>. That means that when you await the 
            var clienttask = ApiClient.GetApiClientWithResourceOwnerCredentials(ConstantAPI.API_KEY, ConstantAPI.API_SecretKEY, ConstantAPI.Username, ConstantAPI.Password);
            var phraseToSearchFor = string.Empty;
            var tokenstr = await clienttask.GetAccessToken();
            var token = tokenstr.AccessToken;
            var results = await clienttask.Download().Image().WithFileType(GettyImages.Api.Search.Entity.FileType.Jpg).WithId(ID).ExecuteAsync();
            phraseToSearchFor = JsonConvert.SerializeObject(results, Formatting.Indented);
            // You can do work here that doesn't rely on the string from GetStringAsync.
            dynamic array = JsonConvert.DeserializeObject(phraseToSearchFor);
            //--------------append images to Html-----------//
            foreach (var item in array)
            {
                if (item.Name == "uri")
                {
                    var dd = item;
                    foreach (var i in dd)
                    {
                        struri = Convert.ToString(i);
                        strfile = saveimagesInsitefolder(struri, ID, caption, live);
                    }
                }
            }
            return strfile;
        }


        #region events
        /// <summary>
        /// Asyscn call for listout getty image events
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="eventid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async static Task<StringBuilder> AccessTheWebAsyncevents(string search, int page,int eventid,string type)
        {
            StringBuilder HtmlData = new StringBuilder();
            List<string> fields = new List<string>();
            DateTime dt = System.DateTime.Now;
            string enddate = DateTime.Parse(dt.AddDays(10).ToString()).ToString("yyyy-MM-dd");
            string startdate = DateTime.Parse(dt.ToString()).ToString("yyyy-MM-dd");
            int loopevent = 5;
            try
            {

                if (string.IsNullOrEmpty(search))
                    search = "sports";

                var clienttask = ApiClient.GetApiClientWithResourceOwnerCredentials(ConstantAPI.API_KEY, ConstantAPI.API_SecretKEY, ConstantAPI.Username, ConstantAPI.Password);
                var phraseToSearchFor = string.Empty;
                var results = await clienttask.Search().Images().WithResponseField("preview,caption,title").WithEventId(eventid).ExecuteAsync();
                phraseToSearchFor = JsonConvert.SerializeObject(results, Formatting.Indented);
                // You can do work here that doesn't rely on the string from GetStringAsync.
                dynamic array = JsonConvert.DeserializeObject(phraseToSearchFor);
                List<imagedetails> lisimg = new List<imagedetails>();
                foreach (var item in array)
                {
                    if (item.Name == "images")
                    {
                        dynamic array2 = item.Value;
                        foreach (var l in array2)
                        {
                            var innerl = l;
                            imagedetails img = new imagedetails();
                            foreach (var r in innerl)
                            {
                                if (r.Name == "id")
                                {
                                    img.ID = Convert.ToInt32(r.Value); ;
                                }
                                if (r.Name == "caption")
                                {
                                    img.caption = Convert.ToString(r.Value);
                                }
                                if (r.Name == "title")
                                {
                                    img.Title = Convert.ToString(r.Value);
                                    lisimg.Add(img);
                                }
                                if (r.Name == "display_sizes")
                                {
                                    var display = r.Value;
                                    foreach (var rr in display)
                                    {
                                        img.previewURL = Convert.ToString(rr["uri"].Value);
                                    }
                                }
                            }
                        }

                    }
                }
                //--------------append images to Html-----------//
                foreach (var dr in lisimg)
                {
                    if (type == "Y")
                        HtmlData.Append(string.Format(ALLHtmlstring.searchimagesmultipleli, dr.previewURL, dr.ID, dr.caption, dr.Title));
                    else
                        HtmlData.Append(string.Format(ALLHtmlstring.searchimagesli, dr.previewURL, dr.ID, dr.caption, dr.Title));

                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return HtmlData;
        }
        #endregion

        #endregion
        #region Set folders
        /// <summary>
        /// Save images in site date and time folder
        /// </summary>
        /// <param name="strImageurl"></param>
        /// <param name="imgID"></param>
        /// <param name="captiondata"></param>
        /// <param name="live"></param>
        /// <returns></returns>
        private string saveimagesInsitefolder(string strImageurl, string imgID, string captiondata, string live)
        {
            string _imagepathdownload = "";
            StringBuilder  JSonformat = new StringBuilder();
            string filename = "";
            string orgfilename = "";
            string sitefolder = string.Empty;
            string imgSrcURL = strImageurl;
            string pathclient = null;
            string Tnname = string.Empty;
            string orgname = string.Empty;
            bool result = false;
            string monthyearfolder = DateTime.Now.ToString("yyyyMM");
            string dayfolder = DateTime.Now.ToString("MMMdd");
            using (SiteInfo objsite = new SiteInfo())
            {
                objsite.NetWorkID = strnetworkid;
                objsite.GetAllsitdetails(SiteID, strnetworkid);
                sitefolder = Function.GetSiteFolderName(objsite.SiteUrl);
                if (objsite.ImageServer)
                {
                    Function.CreateImagesFolderInBackend(sitefolder, monthyearfolder, dayfolder);
                    if (live == "Y")
                    {
                        pathclient = ConstantAPI.downloadsitepath +"newsliveupdate/";
                    }
                    else
                    {
                        pathclient = ConstantAPI.downloadsitepath + string.Format("{0}/{1}/{2}/", sitefolder, monthyearfolder, dayfolder);
                    }
                    _imagepathdownload = pathclient;
                }
                else
                {
                    if (live == "Y")
                    {
                        pathclient = ConstantAPI.downloadsitepath + "newsliveupdate/";
                    }
                    else
                    {
                        pathclient = HttpContext.Current.Server.MapPath("images/newsimages/");
                    }
                    _imagepathdownload = pathclient;
                }


                try
                {
                    orgfilename = "getty_" + DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                    filename = DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                    Downloadimage(imgSrcURL, _imagepathdownload + orgfilename);
                    Tnname = Saveimage(Imagesetting.ImageType.NewsImage, _imagepathdownload, filename, orgfilename);
                    JSonformat.Append("{\"imagedetails\":{");
                    if (live == "Y")
                    {
                        JSonformat.Append("\"URL\":\"" + ConstantAPI.pixURL +"newsliveupdate/" + filename + "\",");
                    }
                    else
                    {
                        JSonformat.Append("\"URL\":\"" + ConstantAPI.pixURL + string.Format("{0}/{1}/{2}/", sitefolder, monthyearfolder, dayfolder) + filename + "\",");
                    }
                    JSonformat.Append("\"DESC\":\"" + captiondata + "\",");
                    JSonformat.Append("\"TNName\":\"" + Tnname + "\"");
                    JSonformat.Append("}}"); 
                }
                catch (Exception exp)
                {
                    CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GoWebshot :", exp);
                }
                imgID = "";
            }

            return JSonformat.ToString();
        }


        /// <summary>
        /// Download image in site
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="path"></param>
        public void Downloadimage(string URL,string path)
        {
            try
            {
            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.DownloadFile(URL, path);
            webClient.Dispose();
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "Getimagedata----Downloadimage SiteID:"+this.SiteID +"---network"+this.strnetworkid, ex);
            }
        }

        /// <summary>
        /// Save images 
        /// </summary>
        /// <param name="imagetype"></param>
        /// <param name="path"></param>
        /// <param name="strfilename"></param>
        /// <param name="originalfile"></param>
        /// <returns></returns>
        public string Saveimage(Imagesetting.ImageType imagetype, string path, string strfilename,string originalfile)
        {
            string filename = string.Empty;
            string TNSizes = string.Empty;
            try
            {
                switch (imagetype)
                {
                        
                    case Imagesetting.ImageType.NewsImage:
                        filename = Function.SaveThumbnailCompress(originalfile,strfilename, path, "TN", Convert.ToInt32(Imagesetting.imagesizeratio.TNWidth), Convert.ToInt32(Imagesetting.imagesizeratio.TNHeight));
                        Function.SaveThumbnailCompress(originalfile,strfilename, path, "TN_TN", Convert.ToInt32(Imagesetting.imagesizeratio.TNTNWIDTH), Convert.ToInt32(Imagesetting.imagesizeratio.TNTNHHeight));
                        break;
                    default:

                        break;
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "Getimagedata----Saveimage ", ex);
            }
            return filename; 

        }

        #endregion

        #region web request methods


      /// <summary>
      /// Webrequest call from URL
      /// </summary>
      /// <param name="requestUrl"></param>
      /// <returns></returns>
        public static string MakeWebRequestfortoken(string requestUrl)
        {
            requestUrl = "https://api.gettyimages.com/oauth2/token/";
            var webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            var jsonSerializer = new JavaScriptSerializer();
            var response = webRequest.GetResponse() as HttpWebResponse;
            string jsonResult;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                jsonResult = reader.ReadToEnd();
                reader.Close();
            }
            Debug.WriteLine(jsonResult);
            return jsonSerializer.Deserialize<string>(jsonResult);
        }
        /// <summary>
        /// Get Token for gettyimages
        /// </summary>
        /// <returns></returns>
        public static string Callwebrequestfortoken()
        {
            string responseData = string.Empty;
            try
            {
                string URLAuth = ConstantAPI.OauthURL;
                string postString = ConstantAPI.Genrate_tokenURL();
                const string contentType = "application/x-www-form-urlencoded";
                HttpWebRequest webRequest = WebRequest.Create(URLAuth) as HttpWebRequest;
                webRequest.Method = "POST";
                webRequest.ContentType = contentType;
                webRequest.ContentLength = postString.Length;
                StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
                requestWriter.Write(postString);
                requestWriter.Close();
                var jsonSerializer = new JavaScriptSerializer();
                var response = webRequest.GetResponse() as HttpWebResponse;
                string jsonResult;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    jsonResult = reader.ReadToEnd();
                    reader.Close();
                }
                dynamic array = JsonConvert.DeserializeObject(jsonResult);
                foreach (var item in array)
                {
                    if ((item.Name == "access_token"))
                    {
                        responseData = Convert.ToString(item.Value);
                        //Response.Write(item.Name + "-:" + item.Value + "</br>");
                        //Getimagesbytoken("",Convert.ToString(item.Value));
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return responseData;
        }
        /// <summary>
        /// Get Image token
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public StringBuilder Getimagesbytoken(string requestUrl, string search)
        {
            string token = Callwebrequestfortoken();
            StringBuilder HtmlData = new StringBuilder();
            Dictionary<int, string> imagedictionary = new Dictionary<int, string>();
            int ID = 0;
            string caption = string.Empty;
            string previeURL = string.Empty;
            string url = string.Empty;
            requestUrl = ConstantAPI.Genrate_imagesearchURL(search);
            var webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ContentType = "application/json charset=UTF-8 ";
            webRequest.Headers.Add("Api-Key", ConstantAPI.API_KEY);
            webRequest.Headers.Add("Authorization", "Bearer " + token);
            var jsonSerializer = new JavaScriptSerializer();
            var response = webRequest.GetResponse() as HttpWebResponse;
            string jsonResult;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                jsonResult = reader.ReadToEnd();
                reader.Close();
            }
            dynamic array = JsonConvert.DeserializeObject(jsonResult);
            List<imagedetails> lisimg = new List<imagedetails>();
            foreach (var item in array)
            {
                if (item.Name == "images")
                {
                    dynamic array2 = item.Value;
                    foreach (var l in array2)
                    {
                        var innerl = l;
                        imagedetails img = new imagedetails();
                        foreach (var r in innerl)
                        {
                            if (r.Name == "id")
                            {
                                ID = Convert.ToInt32(r.Value);
                                img.ID = ID;
                            }

                            if (r.Name == "caption")
                            {
                                img.caption = Convert.ToString(r.Value);
                               
                            }

                            if (r.Name == "display_sizes")
                            {
                                var display = r.Value;
                                foreach (var rr in display)
                                {
                                    url = Convert.ToString(rr["uri"].Value);
                                    img.previewURL = Convert.ToString(rr["uri"].Value);
                                    imagedictionary.Add(ID, url);
                                    ID = 0; url = string.Empty;
                                }
                                lisimg.Add(img);
                            }
                        }
                    }

                }
            }
            //--------------append images to Html-----------//
            foreach (var dr in lisimg)
            {
                HtmlData.Append(string.Format(ALLHtmlstring.searchimagesli, dr.previewURL, dr.ID, dr.caption));
            }
            return HtmlData;
        }

        /// <summary>
        /// Get Event images
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public StringBuilder Getimagesbytoken_events(string requestUrl, string search,int page)
        {
            List<int> eventid = new List<int>();
            int loopevent = 5;
            string token = Callwebrequestfortoken();
            StringBuilder HtmlData = new StringBuilder();
            Dictionary<int, string> imagedictionary = new Dictionary<int, string>();
            int ID = 0;
            string caption = string.Empty;
            string previeURL = string.Empty;
            string url = string.Empty;
            string eventurl = string.Empty;
            string startdate = string.Empty;
            requestUrl = ConstantAPI.Genrate_imagesearchURL_event(search, page,pagesize);
            var webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ContentType = "application/json charset=UTF-8 ";
            webRequest.Headers.Add("Api-Key", ConstantAPI.API_KEY);
            webRequest.Headers.Add("Authorization", "Bearer " + token);
            var jsonSerializer = new JavaScriptSerializer();
            var response = webRequest.GetResponse() as HttpWebResponse;
            string jsonResult;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                jsonResult = reader.ReadToEnd();
                reader.Close();
            }
            dynamic array = JsonConvert.DeserializeObject(jsonResult.Trim());
            List<eventdetails> lisimg = new List<eventdetails>();
            foreach (var item in array)
            {
                if (item.Name == "events")
                {
                    dynamic array2 = item.Value;
                    foreach (var l in array2)
                    {
                        var innerl = l;
                        eventdetails img = new eventdetails();
                        foreach (var r in innerl)
                        {
                            if (r.Name == "hero_image")
                            {
                                dynamic array3 = r.Value;
                                foreach (var id in array3)
                                {
                                    if (id.Name == "display_sizes")
                                    {
                                        dynamic array4 =id.Value;
                                        foreach (var id4 in array4[0])
                                        {
                                            if (id4.Name == "uri")
                                            {
                                                img.previewURL = Convert.ToString(id4.Value);
                                            }
                                        }
                                    }
                                }
                            }
                            if (r.Name == "id")
                            {
                               img.ID = Convert.ToInt32(r.Value);
                            }
                            if (r.Name == "image_count")
                            {
                                img.Imagecount = Convert.ToInt32(r.Value);
                            }

                            if (r.Name == "name")
                            {
                                img.Title = Convert.ToString(r.Value);
                            }
                            if (r.Name == "start_date")
                            {
                                img.startdate = Convert.ToString(r.Value);
                            }
                        }
                        lisimg.Add(img);
                    }
                }
            }
            foreach (var dr in lisimg)
            {
                if (!string.IsNullOrEmpty(dr.previewURL))
                {
                    if(!string.IsNullOrEmpty(dr.startdate))
                    {
                        startdate = DateTime.Parse(dr.startdate).ToString("dd-MMM-yyyy");
                    }
                    eventurl = string.Format(ConstantAPI.streventurl, dr.ID);
                    HtmlData.Append(string.Format(ALLHtmlstring.searchimageseventli, dr.previewURL, dr.ID, dr.Title, dr.Imagecount, startdate, eventurl));
                    startdate = string.Empty;
                    eventurl = string.Empty;
                }
            }
            return HtmlData;
        }
        /// <summary>
        /// Get images by token
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="eventid"></param>
        /// <param name="type"></param>
        /// <returns></returns>

        public StringBuilder Getimagesbytoken_events(string requestUrl, string search, int page,int eventid,string type)
        {
            StringBuilder HtmlData = new StringBuilder();
            try
            {
                int loopevent = 5;
                string token = Callwebrequestfortoken();
                Dictionary<int, string> imagedictionary = new Dictionary<int, string>();
                int ID = 0;
                string caption = string.Empty;
                string previeURL = string.Empty;
                string url = string.Empty;
                string startdate = string.Empty;
                requestUrl = ConstantAPI.Genrate_imagesearchURL_event(eventid);
                var webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
                webRequest.Method = "GET";
                webRequest.ContentType = "application/json charset=UTF-8 ";
                webRequest.Headers.Add("Api-Key", ConstantAPI.API_KEY);
                webRequest.Headers.Add("Authorization", "Bearer " + token);
                var jsonSerializer = new JavaScriptSerializer();
                var response = webRequest.GetResponse() as HttpWebResponse;
                string jsonResult;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    jsonResult = reader.ReadToEnd();
                    reader.Close();
                }
                dynamic array = JsonConvert.DeserializeObject(jsonResult);
                List<imagedetails> lisimg = new List<imagedetails>();
                foreach (var item in array)
                {
                    if (item.Name == "images")
                    {
                        dynamic array2 = item.Value;
                        foreach (var l in array2)
                        {
                            var innerl = l;
                            imagedetails img = new imagedetails();
                            foreach (var r in innerl)
                            {
                                if (r.Name == "id")
                                {
                                    img.ID = Convert.ToInt32(r.Value); ;
                                }
                                if (r.Name == "caption")
                                {
                                    img.caption = Convert.ToString(r.Value);
                                }
                                if (r.Name == "title")
                                {
                                    img.Title = Convert.ToString(r.Value);
                                    lisimg.Add(img);
                                }
                                if (r.Name == "display_sizes")
                                {
                                    var display = r.Value;
                                    foreach (var rr in display)
                                    {
                                        img.previewURL = Convert.ToString(rr["uri"].Value);
                                    }
                                }
                            }
                        }

                    }
                }
                //--------------append images to Html-----------//

                if (lisimg.Count > 0)
                {
                    foreach (var dr in lisimg)
                    {
                        if (type == "Y")
                            HtmlData.Append(string.Format(ALLHtmlstring.Eventimagesli, dr.previewURL, dr.ID, dr.caption, dr.Title));
                        else
                            HtmlData.Append(string.Format(ALLHtmlstring.Eventimagesli, dr.previewURL, dr.ID, dr.caption, dr.Title));

                    }
                }
                else
                {
                    HtmlData.Append("<li>No record in getty response</li>");

                }


            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "Getimagedata----Downloadimage SiteID:"+this.SiteID +"---network"+this.strnetworkid, ex);
            }
            return HtmlData;
        }
        /// <summary>
        /// Get images by imageID
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="token"></param>
        /// <param name="IDS"></param>
        public void GetimagesbyImageID(string requestUrl, string token, string IDS)
        {
            token = Callwebrequestfortoken();
            // requestUrl = "https://api.gettyimages.com/v3/images?ids=" + IDS;
            requestUrl = "https://api.gettyimages.com/v3/downloads/" + IDS;
            var webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Headers.Add("Api-Key", ConfigurationSettings.AppSettings["apikey"]);
            webRequest.Headers.Add("Authorization", "Bearer " + token);
            string postString = "?auto_download=false";
            StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
            requestWriter.Write(postString);
            requestWriter.Close();
            var jsonSerializer = new JavaScriptSerializer();
            var response = webRequest.GetResponse() as HttpWebResponse;
            string jsonResult;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                jsonResult = reader.ReadToEnd();
                reader.Close();
            }
            dynamic array = JsonConvert.DeserializeObject(jsonResult);
            foreach (var item in array)
            {

            }
        }
        /// <summary>
        /// Get Refreshtoken
        /// </summary>
        /// <returns></returns>
        public string GetRefreshtoken()
        {
            string refreshtoken = string.Empty;
            try
            {
                string URLAuth = ConstantAPI.OauthURL;
                string postString = ConstantAPI.Genrate_tokenURL();
                const string contentType = "application/x-www-form-urlencoded";
                HttpWebRequest webRequest = WebRequest.Create(URLAuth) as HttpWebRequest;
                webRequest.Method = "POST";
                webRequest.ContentType = contentType;
                webRequest.ContentLength = postString.Length;
                StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
                requestWriter.Write(postString);
                requestWriter.Close();
                var jsonSerializer = new JavaScriptSerializer();
                var response = webRequest.GetResponse() as HttpWebResponse;
                string jsonResult;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    jsonResult = reader.ReadToEnd();
                    reader.Close();
                }
                dynamic array = JsonConvert.DeserializeObject(jsonResult);
                foreach (var item in array)
                {
                    if ((item.Name == "access_token"))
                    {
                        refreshtoken = Convert.ToString(item.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return refreshtoken;
        }

        public void WhenIAskTheSdkForAnAuthenticationToken()
        {
            var client = ScenarioCredentialsHelper.GetCredentials();
            ScenarioContext.Current.Add("task", client.GetAccessToken());
        }

        [When(@"I configure my search for editorial images")]
        public void WhenIConfigureMySearchForEditorialImages()
        {
            ScenarioContext.Current.Set(
                ScenarioCredentialsHelper.GetCredentials().Search().Images().Editorial(), "request");
        }

    #endregion

    }
}
