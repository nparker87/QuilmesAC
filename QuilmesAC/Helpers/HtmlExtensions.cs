namespace QuilmesAC.Helpers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class HtmlExtensions
    {
        // Will display error messages with HTML in them properly
        public static HtmlString Raw(this MvcHtmlString htmlString)
        {
            return htmlString == null
                ? new HtmlString("")
                : new HtmlString(HttpUtility.HtmlDecode(htmlString.ToString()));
        }

        public static string DatePicker(this HtmlHelper helper, string name, object date)
        {
            var html = new StringBuilder();
            var id = name.Replace('.', '_'); //Fix to match built-in HelperExtensions

            //Build our base input element
            html.Append("<input type=\"text\" id=\"" + id + "\" name=\"" + name + "\"");

            // Model Binding Support
            if (date != null)
            {
                string dateValue = String.Empty;

                if (date is DateTime? && ((DateTime)date) != DateTime.MinValue)
                    dateValue = ((DateTime)date).ToShortDateString();
                else if (date is DateTime && (DateTime)date != DateTime.MinValue)
                    dateValue = ((DateTime)date).ToShortDateString();
                else if (date is string)
                    dateValue = (string)date;

                html.Append(" value=\"" + dateValue + "\"");
            }

            // We're hard-coding the width here, a better option would be to pass in html attributes and reflect through them
            // here ( default to 75px width if no style attributes )
            html.Append(" style=\"width: 75px;\" />");

            // Now we call the datepicker function, passing in our options.  Again, a future enhancement would be to
            // pass in date options as a list of attributes ( min dates, day/month/year formats, etc. )
            html.Append("<script type=\"text/javascript\">$(document).ready(function() { $('#" + id + "').datepicker({ duration: 0, changeMonth: true, changeYear: true }); });</script>");

            return html.ToString();
        }

        public static string DatePickerFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var expressionText = ExpressionHelper.GetExpressionText(expression);
            return DatePicker(htmlHelper, expressionText, htmlHelper.ViewData.Eval(expressionText));
        }

        public static MvcHtmlString RequriedLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            var tag = new TagBuilder("label");
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.Attributes.Add("class", "required");
            if (htmlAttributes != null)
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            var span = new TagBuilder("span");

            // assign label text and <span> to <label> inner html
            tag.InnerHtml = labelText + span.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ScriptInclude(this HtmlHelper helper, string url, object htmlAttributes = null)
        {
            var tag = new TagBuilder("script");

            // apply attributes
            tag.MergeAttribute("src", PathHelper.ToAbsolute(url, helper.ViewContext.RequestContext));
            tag.MergeAttribute("type", "text/javascript");
            if (htmlAttributes != null)
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            var result = tag.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(result);
        }

        public static MvcHtmlString Stylesheet(this HtmlHelper helper, string url, object htmlAttributes = null)
        {
            var tag = new TagBuilder("link");

            // apply attributes
            tag.MergeAttribute("href", PathHelper.ToAbsolute(url, helper.ViewContext.RequestContext));
            tag.MergeAttribute("rel", "stylesheet");
            tag.MergeAttribute("type", "text/css");
            if (htmlAttributes != null)
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            var result = tag.ToString(TagRenderMode.StartTag);
            return MvcHtmlString.Create(result);
        }

        /// <summary> Basically the equivalent of Url.Content() for use outside of the View/Controller </summary>
        public static class PathHelper
        {
            public static string ToAbsolute(string path, RequestContext request)
            {
                if (path.IndexOf('?') == -1)
                    path = VirtualPathUtility.ToAbsolute(path);
                else
                    path =
                        VirtualPathUtility.ToAbsolute(path.Substring(0, path.IndexOf('?')), request.HttpContext.Request.ApplicationPath) +
                        path.Substring(path.IndexOf('?'));

                return path;
            }
        }
    }
}