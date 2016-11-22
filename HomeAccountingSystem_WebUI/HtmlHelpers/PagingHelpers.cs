using System;
using System.Text;
using System.Web.Mvc;
using HomeAccountingSystem_WebUI.Models;

namespace HomeAccountingSystem_WebUI.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tBuilder = new TagBuilder("a");
                tBuilder.MergeAttribute("href",pageUrl(i));
                tBuilder.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                {
                    tBuilder.AddCssClass("selected");
                    tBuilder.AddCssClass("btn-primary");
                }
                tBuilder.AddCssClass("btn btn-default");
                result.Append(tBuilder.ToString());}

            return MvcHtmlString.Create(result.ToString());
        }
    }
}