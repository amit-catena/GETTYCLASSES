using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gettyclasses
{
    public class imagedata
    {
        public IList<Imagedetails> imagesdetails { get; set; } 
    }
    public class Imagedetails
    {
        public string ImageId { get; set; }
        public string Title { get; set; }
        public string ImageFamily { get; set; }
        public string CollectionName { get; set; }
        public DateTime DateCreated { get; set; }
        public string LicensingModel { get; set; }
        public string UrlThumb { get; set; }
        public string UrlPreview { get; set; }
        public string Artist { get; set; }
        public string Caption { get; set; }
        public string ShortCaption { get; set; }
        public int _gotevent { get; set; }
        public int _goteventcnt { get; set; }
    }



}
