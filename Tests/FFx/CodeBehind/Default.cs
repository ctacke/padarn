using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web.UI;

namespace OpenNETCF.Padarn
{
    public class Default : Page
    {
        protected override void Render(HtmlTextWriter writer)
        {
            writer
                .Html()

                .Text(DoctypeTags.XHTML1)

                // open HTML tag
                .Tag(HtmlTextWriterTag.Html, t => t
                    ["xmlns", "http://www.w3.org/1999/xhtml"]
                    )

                    // open HEAD
                    .Tag(HtmlTextWriterTag.Head)
                        // style sheets
                        .StylesheetLink("/css/padarn.css")

                        // scripts
                        .JavascriptImport("/script/jquery-1.11.1.min.js")

                        .Meta(t=>t
                            ["http-equiv", "cache-control"]
                            ["content", "max-age=0"])
                        .EndTag()
                        .Meta(t=>t
                            ["http-equiv", "cache-control"]
                            ["content", "no-cache"])
                        .EndTag()
                        .Meta(t=>t
                            ["http-equiv", "pragma"]
                            ["content", "no-cache"])
                        .EndTag()
                        .Meta(t=>t
                            ["http-equiv", "expires"]
                            ["content", "0"])
                        .EndTag()

                        .Title("Padarn Sample Project")
                    .EndTag() // head

                    .Body()
                        .Div(t => t[HtmlTextWriterAttribute.Id, "page-header-container-left"])
                            .Div(t => t
                                [HtmlTextWriterAttribute.Class, "logo"]
                                [HtmlTextWriterAttribute.Id, "logo"]
                                [HtmlTextWriterAttribute.Alt, "Logo"])
                                .Img(t => t[HtmlTextWriterAttribute.Src, "/img/logo.png"])
                                .EndTag() // img
                            .EndTag() // div
                            .Div(t => t
                                [HtmlTextWriterAttribute.Class, "page-title"]
                                [HtmlTextWriterAttribute.Id, "page-title"])
                                .Text("Padarn Test Page")
                            .EndTag() // div
                        .EndTag() // div

                        .Div()
                            .H1()
                                .Text("Hello World!")
                            .EndTag()
                        .EndTag() // div

                        .Div()
                            .Img(t => t[HtmlTextWriterAttribute.Src, "/img/a.png"])
                            .EndTag() // img
                        .EndTag() // div
                        .Div()
                            .Img(t => t[HtmlTextWriterAttribute.Src, "/img/b.png"])
                            .EndTag() // img
                        .EndTag() // div
                        .Div()
                            .Img(t => t[HtmlTextWriterAttribute.Src, "/img/c.png"])
                            .EndTag() // img
                        .EndTag() // div
                        .Div()
                            .Img(t => t[HtmlTextWriterAttribute.Src, "/img/d.png"])
                            .EndTag() // img
                        .EndTag() // div
                    .EndTag() // body

                .EndTag(); // html
        }
    }
}
