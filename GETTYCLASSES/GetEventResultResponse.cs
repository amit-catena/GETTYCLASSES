
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace ConnectExample.Samples
{
    public class GetEventResultResponse
    {
        public GetEventsResult GetEventsResult { get; set; }
    }

    public class GetEventsResult
    {
        public List<Event> Events { get; set; }
    }

    public class Event
    {
        public string Description { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public int ImageCount { get; set; }
        public string RepresentativeImageId { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
    }
    public class GetEventsRequestBody
    {
        public List<int> EventIds { get; set; }
        public RequestHeader RequestHeader { get; set; }
        public string EventSortType { get; set; }
    }

    public class GetEventDetailsResults
    {
        private const string SearchForImagesRequestUrl = "http://connect.gettyimages.com/v1/search/GetEventDetails";
        private const string SearchTerm = "tree";

        // token received from CreateSession/RenewSession API call
        public Event GetEvents(string token, List<int> _EventID)
        {
            var GetEventsRequestBody = new GetEventsRequestBody
            {
                RequestHeader = new RequestHeader { Token = token },
                EventIds = _EventID
            };
            return MakeWebRequest(SearchForImagesRequestUrl, GetEventsRequestBody);
        }

        //You may wish to replace this code with your preferred library for posting and receiving JSON data.
        private static Event MakeWebRequest(string requestUrl, GetEventsRequestBody request)
        {
            var webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;

            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";

            var jsonSerializer = new JavaScriptSerializer();
            var requestStr = jsonSerializer.Serialize(request);

            Debug.WriteLine(requestStr);

            using (var writer = new StreamWriter(webRequest.GetRequestStream()))
            {
                writer.Write(requestStr);
                writer.Close();
            }

            var response = webRequest.GetResponse() as HttpWebResponse;

            string jsonResult;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                jsonResult = reader.ReadToEnd();
                reader.Close();
            }

            Debug.WriteLine(jsonResult);

            return jsonSerializer.Deserialize<Event>(jsonResult);
        }

    }

}
