#pragma checksum "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9c622a28d17fd5d1107a9c9974f53d518c566775"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9c622a28d17fd5d1107a9c9974f53d518c566775", @"/Views/Home/Index.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";
    string[,] letters = new string[7, 7] {
        { "", "L", "P", "", "", "", "R" },
        { "W", "E", "L", "C", "O", "M", "E" },
        { "", "T", "A", "", "", "", "V" },
        { "", "`", "Y", "", "", "", "E" },
        { "", "S", "", "", "", "", "R" },
        { "", "", "", "", "", "", "S" },
        { "", "", "", "", "", "", "I" },
    };


#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<aside class=""home__stats"">
    <div class=""home__stats__card"">
        <span data-title=""Stats""></span>

        <ul class=""home__stats__list"">
            <li class=""home__stats__list__item""><span class=""material-icons"">emoji_events</span>Wins:<span>");
#nullable restore
#line (20,109)-(20,121) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(ViewBag.Wins);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></li>\r\n            <li class=\"home__stats__list__item\"><span class=\"material-icons\">trending_down</span>Losses:<span>");
#nullable restore
#line (21,112)-(21,126) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(ViewBag.Losses);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></li>\r\n            <li class=\"home__stats__list__item\"><span class=\"material-icons\">handshake</span>Draws:<span>");
#nullable restore
#line (22,107)-(22,120) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(ViewBag.Draws);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></li>\r\n");
            WriteLiteral("        </ul>\r\n\r\n    </div>\r\n</aside>\r\n\r\n<div class=\"board\">\r\n    <div class=\"home__board\">\r\n");
#nullable restore
#line 43 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
         for (int i = 0; i < letters.GetLength(0); i++)
        {
            

#line default
#line hidden
#nullable disable
#nullable restore
#line 45 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
             for (int j = 0; j < letters.GetLength(1); j++)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div>\r\n            <a>\r\n");
#nullable restore
#line 49 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
                 if (letters[i, j] != "")
                {
                    if (i == 1)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                        <span class=""home__board-animation delay-1""></span>
                        <span class=""home__board-animation delay-1""></span>
                        <span class=""home__board-animation delay-1""></span>
                        <span class=""home__board-animation delay-1""></span>
                        <span class=""home__board-animation delay-1"">");
#nullable restore
#line (57,70)-(57,83) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(letters[i, j]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 58 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
                    }
                    else if (i < 5 && j == 1)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                        <span class=""home__board-animation delay-2""></span>
                        <span class=""home__board-animation delay-2""></span>
                        <span class=""home__board-animation delay-2""></span>
                        <span class=""home__board-animation delay-2""></span>
                        <span class=""home__board-animation delay-2"">");
#nullable restore
#line (65,70)-(65,83) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(letters[i, j]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 66 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
                    }
                    else if (i < 4 && j == 2)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                        <span class=""home__board-animation delay-3""></span>
                        <span class=""home__board-animation delay-3""></span>
                        <span class=""home__board-animation delay-3""></span>
                        <span class=""home__board-animation delay-3""></span>
                        <span class=""home__board-animation delay-3"">");
#nullable restore
#line (73,70)-(73,83) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(letters[i, j]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 74 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
                    }
                    else if (j == 6)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                        <span class=""home__board-animation delay-4""></span>
                        <span class=""home__board-animation delay-4""></span>
                        <span class=""home__board-animation delay-4""></span>
                        <span class=""home__board-animation delay-4""></span>
                        <span class=""home__board-animation delay-4"">");
#nullable restore
#line (81,70)-(81,83) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(letters[i, j]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 82 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
                    }
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <span>");
#nullable restore
#line (86,28)-(86,41) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(letters[i, j]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 87 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </a>\r\n        </div>\r\n");
#nullable restore
#line 90 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 90 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
             
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
