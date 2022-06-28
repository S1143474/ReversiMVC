#pragma checksum "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "52f597a421ab196f1c4893986ed946f5707991c7"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"52f597a421ab196f1c4893986ed946f5707991c7", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"23ac09be4bcfaa7f9829a01d1a134874eaae1f3b", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Application.Spelers.Queries.GetSpeler.SpelerDTO>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
  
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
#line (22,109)-(22,119) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(Model.Wins);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></li>\r\n            <li class=\"home__stats__list__item\"><span class=\"material-icons\">trending_down</span>Losses:<span>");
#nullable restore
#line (23,112)-(23,124) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(Model.Losses);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></li>\r\n            <li class=\"home__stats__list__item\"><span class=\"material-icons\">handshake</span>Draws:<span>");
#nullable restore
#line (24,107)-(24,118) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(Model.Draws);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></li>\r\n");
            WriteLiteral("        </ul>\r\n\r\n    </div>\r\n</aside>\r\n\r\n<div class=\"board\">\r\n    <div class=\"home__board\">\r\n");
#nullable restore
#line 45 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
         for (int i = 0; i < letters.GetLength(0); i++)
        {
            

#line default
#line hidden
#nullable disable
#nullable restore
#line 47 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
             for (int j = 0; j < letters.GetLength(1); j++)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div>\r\n            <a>\r\n");
#nullable restore
#line 51 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
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
#line (59,70)-(59,83) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(letters[i, j]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 60 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
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
#line (67,70)-(67,83) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(letters[i, j]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 68 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
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
#line (75,70)-(75,83) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(letters[i, j]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 76 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
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
#line (83,70)-(83,83) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(letters[i, j]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 84 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
                    }
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <span>");
#nullable restore
#line (88,28)-(88,41) 6 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
Write(letters[i, j]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 89 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </a>\r\n        </div>\r\n");
#nullable restore
#line 92 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 92 "C:\Users\bassc\source\repos\ReversiMvcApp\WebUI\Views\Home\Index.cshtml"
             
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Application.Spelers.Queries.GetSpeler.SpelerDTO> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
