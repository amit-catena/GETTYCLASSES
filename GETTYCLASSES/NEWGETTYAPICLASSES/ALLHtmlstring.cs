using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGettyAPIclasses
{
    /// <summary>
    /// Html string class for binding html code
    /// </summary>
    public class ALLHtmlstring
    {
        /// <summary>
        /// Design  li for image search 
        /// </summary>
        public static string imagesli = @"<li><span class='inrimg'><img src='{0}'></span><div class='downdetail'><span class='dow-date'><em>Download date </em>                 			    		<span>2 March 2017 04:41</span><span>Editorial</span>
			    	</span>
			    	<ul>
			    		<li><span>Item #</span><span><a href='#'>{1}</a></span></li>				    		
			    		<li><span>title</span><span>FBL-ESP-LIGA-REALMADRID-VILLARRIAL</span></li>
			    		<li><span>Download notes</span><span><a href='#'> Add notes </a></span></li>
			    	</ul>
                </div>
			</li>";
        /// <summary>
        /// Design  li for image search 
        /// </summary>

        public static string searchimagesli = @"<li><div class='mainlidiv'><span class='inrimg'><img src='" + ConstantAPI.strspacer + @"'  data-original='{0}'><div class='box'><span class='innerinrimg'><img src='" + ConstantAPI.strspacer + @"'  data-original='{0}'></span><div class='innerpopup'><span>{3}</span><span>{2}</span></div></div></span><div class='downdetail'>
			    	<ul>
			    		<li><span><input type='radio' id='{1}' value='{1}' onclick='javascript:getdownloadfornetwork(this);'/>{3}</span></li>
                        <li><span id='cap{1}'>{2}</span></li>
			    		<!--<li><span>Download</span><span><a href='javascript:void(0);' onclick='javascript:getdownloadfornetwork({1})'>Download image</a></span><input type='hidden' value='{1}' id='imgid{1}' /></li>-->
			    	</ul>
                </div>
			</div></li>";

        /// <summary>
        /// Design  li for multiple image search 
        /// </summary>
        public static string searchimagesmultipleli = @"<li><div class='mainlidiv'><span class='inrimg'><img src='" + ConstantAPI.strspacer + @"'  data-original='{0}'><div class='box'><span class='innerinrimg'><img src='" + ConstantAPI.strspacer + @"'  data-original='{0}'></span><div class='innerpopup'><span>{3}</span><span>{2}</span></div></div></span><div class='downdetail'>
			    	<ul>
			    					    		
			    		<li><span><input type='checkbox' id='{1}' value='{1}' onclick='javascript:getImagecheck(this);' />{3}</span></li>
                        <li><span id='cap{1}'>{2}</span></li>
			    		
			    	</ul>
                </div>
			</div></li>";
        /// <summary>
        /// Design  li for search event image  
        /// </summary>
        public static string searchimageseventli = @"<li><div class='eventdiv'><span class='spdate'>{4}</span><span class='spinfo'>{2}</span><span class='spimg'><a href='{5}'><img src='" + ConstantAPI.strspacer + @"'  data-original='{0}'></a></span><span><a href='{5}'>{3} Images</a></span>
			</div></li>";

        /// <summary>
        /// Design  li and for search event image  
        /// </summary>
        public static string Eventimagesli = @"<li><div class='mainlidiv'><span class='inrimg'><img src='" + ConstantAPI.strspacer + @"'  data-original='{0}'><div class='box'><span class='innerinrimg'><img src='" + ConstantAPI.strspacer + @"'  data-original='{0}'></span><div class='innerpopup'><span>{3}</span><span>{2}</span></div></div></span><div class='downdetail'><span>Editorial</span>
			    	</span>
			    	<ul>
			    		<li><span>Item #</span><span><a href='#'>{1}</a></span></li>				    		
			    		<li><span>title</span><span>{3}</span></li>
                        <li><span>{2}</span></li>
			    		
			    	</ul>
                </div>
			</div></li>";

      
    }
}
