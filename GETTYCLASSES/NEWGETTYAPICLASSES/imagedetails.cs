using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGettyAPIclasses
{
    /// <summary>
    /// Public imagedetails collection entity class
    /// </summary>
  public   class imagedetails
    {
       public int ID { get; set; }
       public string caption { get; set; }
       public string Title { get; set; }
       public  string previewURL { get; set; }
    }


    /// <summary>
  /// Public eventdetails collection entity class
    /// </summary>
  public class eventdetails
  {
      public int ID { get; set; }
      public string caption { get; set; }
      public string Title { get; set; }
      public string previewURL { get; set; }
      public int Imagecount { get; set; }
      public string startdate { get; set; }
  }
}
