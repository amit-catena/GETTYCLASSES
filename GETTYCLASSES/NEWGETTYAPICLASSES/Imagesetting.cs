using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGettyAPIclasses
{
    /// <summary>
    /// Image setting Thubnail,
    /// </summary>
    public class Imagesetting
    {
        public enum ImageType
        {
            NewsImage = 0,
            CategoryImage = 1,
            SubcategoryImage = 2
        }
       public enum ImageSize
        {
            Thumbnail = 0,
            Medium = 1,
            Large = 2
        }

        public enum imagesizeratio
        {
            TNHeight=225,
            TNWidth = 300,
            TNTNHHeight=85,
            TNTNWIDTH=125


        }
       
    }
}
