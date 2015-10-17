using Microsoft.AspNet.Mvc.Rendering;
using PagedList;
//using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XPagedList.Mvc
{
    ///<summary>
    ///	Extension methods for generating paging controls that can operate on instances of IPagedList.
    ///</summary>
    public static class HtmlHelper
    {
        private static TagBuilder WrapInListItem(string text)
        {
            var li = new TagBuilder("li");
            li.SetInnerText(text);
            return li;
        }

        private static TagBuilder WrapInListItem(TagBuilder inner, PagedListRenderOptions options, params string[] classes)
        {
            var li = new TagBuilder("li");
            foreach (var @class in classes)
                li.AddCssClass(@class);
            if (options.FunctionToTransformEachPageLink != null)
                return options.FunctionToTransformEachPageLink(li, inner);
            li.InnerHtml = inner.ToString();
            return li;
        }

        private static TagBuilder First(IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            const int targetPageNumber = 1;
            var first = new TagBuilder("a")
            {
                InnerHtml = string.Format(options.LinkToFirstPageFormat, targetPageNumber)
            };

            if (list.IsFirstPage)
                return WrapInListItem(first, options, "PagedList-skipToFirst", "disabled");

            first.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(first, options, "PagedList-skipToFirst");
        }

        private static TagBuilder Previous(IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            var targetPageNumber = list.PageNumber - 1;
            var previous = new TagBuilder("a")
            {
                InnerHtml = string.Format(options.LinkToPreviousPageFormat, targetPageNumber)
            };
            previous.Attributes["rel"] = "prev";

            if (!list.HasPreviousPage)
                return WrapInListItem(previous, options, "PagedList-skipToPrevious", "disabled");

            previous.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(previous, options, "PagedList-skipToPrevious");
        }

        private static TagBuilder Page(int i, IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            var format = options.FunctionToDisplayEachPageNumber
                ?? (pageNumber => string.Format(options.LinkToIndividualPageFormat, pageNumber));
            var targetPageNumber = i;
            var page = new TagBuilder("a");
            page.SetInnerText(format(targetPageNumber));

            if (i == list.PageNumber)
                return WrapInListItem(page, options, "active");

            page.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(page, options);
        }

        private static TagBuilder Next(IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            var targetPageNumber = list.PageNumber + 1;
            var next = new TagBuilder("a")
            {
                InnerHtml = string.Format(options.LinkToNextPageFormat, targetPageNumber)
            };
            next.Attributes["rel"] = "next";

            if (!list.HasNextPage)
                return WrapInListItem(next, options, "PagedList-skipToNext", "disabled");

            next.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(next, options, "PagedList-skipToNext");
        }

        private static TagBuilder Last(IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            var targetPageNumber = list.PageCount;
            var last = new TagBuilder("a")
            {
                InnerHtml = string.Format(options.LinkToLastPageFormat, targetPageNumber)
            };

            if (list.IsLastPage)
                return WrapInListItem(last, options, "PagedList-skipToLast", "disabled");

            last.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(last, options, "PagedList-skipToLast");
        }

        private static TagBuilder PageCountAndLocationText(IPagedList list, PagedListRenderOptions options)
        {
            var text = new TagBuilder("a");
            text.SetInnerText(string.Format(options.PageCountAndCurrentLocationFormat, list.PageNumber, list.PageCount));

            return WrapInListItem(text, options, "PagedList-pageCountAndLocation", "disabled");
        }

        private static TagBuilder ItemSliceAndTotalText(IPagedList list, PagedListRenderOptions options)
        {
            var text = new TagBuilder("a");
            text.SetInnerText(string.Format(options.ItemSliceAndTotalFormat, list.FirstItemOnPage, list.LastItemOnPage, list.TotalItemCount));

            return WrapInListItem(text, options, "PagedList-pageCountAndLocation", "disabled");
        }

        private static TagBuilder Ellipses(PagedListRenderOptions options)
        {
            var a = new TagBuilder("a")
            {
                InnerHtml = options.EllipsesFormat
            };

            return WrapInListItem(a, options, "PagedList-ellipses", "disabled");
        }

        ///<summary>
        ///	Displays a configurable paging control for instances of PagedList.
        ///</summary>
        ///<param name = "html">This method is meant to hook off HtmlHelper as an extension method.</param>
        ///<param name = "list">The PagedList to use as the data source.</param>
        ///<param name = "generatePageUrl">A function that takes the page number of the desired page and returns a URL-string that will load that page.</param>
        ///<returns>Outputs the paging control HTML.</returns>
        public static HtmlString PagedListPager(this IHtmlHelper html,
                                                   IPagedList list,
                                                   Func<int, string> generatePageUrl)
        {
            return PagedListPager(html, list, generatePageUrl, new PagedListRenderOptions());
        }

        ///<summary>
        ///	Displays a configurable paging control for instances of PagedList.
        ///</summary>
        ///<param name = "html">This method is meant to hook off HtmlHelper as an extension method.</param>
        ///<param name = "list">The PagedList to use as the data source.</param>
        ///<param name = "generatePageUrl">A function that takes the page number  of the desired page and returns a URL-string that will load that page.</param>
        ///<param name = "options">Formatting options.</param>
        ///<returns>Outputs the paging control HTML.</returns>
        public static HtmlString PagedListPager(this IHtmlHelper html,
                                                   IPagedList list,
                                                   Func<int, string> generatePageUrl,
                                                   PagedListRenderOptions options)
        {
            if (options.Display == PagedListDisplayMode.Never || (options.Display == PagedListDisplayMode.IfNeeded && list.PageCount <= 1))
                return null;

            var listItemLinks = new List<TagBuilder>();

            //calculate start and end of range of page numbers
            var firstPageToDisplay = 1;
            var lastPageToDisplay = list.PageCount;
            var pageNumbersToDisplay = lastPageToDisplay;
            if (options.MaximumPageNumbersToDisplay.HasValue && list.PageCount > options.MaximumPageNumbersToDisplay)
            {
                // cannot fit all pages into pager
                var maxPageNumbersToDisplay = options.MaximumPageNumbersToDisplay.Value;
                firstPageToDisplay = list.PageNumber - maxPageNumbersToDisplay / 2;
                if (firstPageToDisplay < 1)
                    firstPageToDisplay = 1;
                pageNumbersToDisplay = maxPageNumbersToDisplay;
                lastPageToDisplay = firstPageToDisplay + pageNumbersToDisplay - 1;
                if (lastPageToDisplay > list.PageCount)
                    firstPageToDisplay = list.PageCount - maxPageNumbersToDisplay + 1;
            }

            //first
            if (options.DisplayLinkToFirstPage == PagedListDisplayMode.Always || (options.DisplayLinkToFirstPage == PagedListDisplayMode.IfNeeded && firstPageToDisplay > 1))
                listItemLinks.Add(First(list, generatePageUrl, options));

            //previous
            if (options.DisplayLinkToPreviousPage == PagedListDisplayMode.Always || (options.DisplayLinkToPreviousPage == PagedListDisplayMode.IfNeeded && !list.IsFirstPage))
                listItemLinks.Add(Previous(list, generatePageUrl, options));

            //text
            if (options.DisplayPageCountAndCurrentLocation)
                listItemLinks.Add(PageCountAndLocationText(list, options));

            //text
            if (options.DisplayItemSliceAndTotal)
                listItemLinks.Add(ItemSliceAndTotalText(list, options));

            //page
            if (options.DisplayLinkToIndividualPages)
            {
                //if there are previous page numbers not displayed, show an ellipsis
                if (options.DisplayEllipsesWhenNotShowingAllPageNumbers && firstPageToDisplay > 1)
                    listItemLinks.Add(Ellipses(options));

                foreach (var i in Enumerable.Range(firstPageToDisplay, pageNumbersToDisplay))
                {
                    //show delimiter between page numbers
                    if (i > firstPageToDisplay && !string.IsNullOrWhiteSpace(options.DelimiterBetweenPageNumbers))
                        listItemLinks.Add(WrapInListItem(options.DelimiterBetweenPageNumbers));

                    //show page number link
                    listItemLinks.Add(Page(i, list, generatePageUrl, options));
                }

                //if there are subsequent page numbers not displayed, show an ellipsis
                if (options.DisplayEllipsesWhenNotShowingAllPageNumbers && (firstPageToDisplay + pageNumbersToDisplay - 1) < list.PageCount)
                    listItemLinks.Add(Ellipses(options));
            }

            //next
            if (options.DisplayLinkToNextPage == PagedListDisplayMode.Always || (options.DisplayLinkToNextPage == PagedListDisplayMode.IfNeeded && !list.IsLastPage))
                listItemLinks.Add(Next(list, generatePageUrl, options));

            //last
            if (options.DisplayLinkToLastPage == PagedListDisplayMode.Always || (options.DisplayLinkToLastPage == PagedListDisplayMode.IfNeeded && lastPageToDisplay < list.PageCount))
                listItemLinks.Add(Last(list, generatePageUrl, options));

            if (listItemLinks.Any())
            {
                //append class to first item in list?
                if (!string.IsNullOrWhiteSpace(options.ClassToApplyToFirstListItemInPager))
                    listItemLinks.First().AddCssClass(options.ClassToApplyToFirstListItemInPager);

                //append class to last item in list?
                if (!string.IsNullOrWhiteSpace(options.ClassToApplyToLastListItemInPager))
                    listItemLinks.Last().AddCssClass(options.ClassToApplyToLastListItemInPager);

                //append classes to all list item links
                foreach (var li in listItemLinks)
                    foreach (var c in options.LiElementClasses ?? Enumerable.Empty<string>())
                        li.AddCssClass(c);
            }

            //collapse all of the list items into one big string
            var listItemLinksString = listItemLinks.Aggregate(
                new StringBuilder(),
                (sb, listItem) => sb.Append(listItem.ToString()),
                sb => sb.ToString()
                );

            var ul = new TagBuilder("ul")
            {
                InnerHtml = listItemLinksString
            };
            foreach (var c in options.UlElementClasses ?? Enumerable.Empty<string>())
                ul.AddCssClass(c);

            var outerDiv = new TagBuilder("div");
            foreach (var c in options.ContainerDivClasses ?? Enumerable.Empty<string>())
                outerDiv.AddCssClass(c);
            outerDiv.InnerHtml = ul.ToString();

            return new HtmlString(outerDiv.ToString());
        }

        ///<summary>
        /// Displays a configurable "Go To Page:" form for instances of PagedList.
        ///</summary>
        ///<param name="html">This method is meant to hook off HtmlHelper as an extension method.</param>
        ///<param name="list">The PagedList to use as the data source.</param>
        ///<param name="formAction">The URL this form should submit the GET request to.</param>
        ///<returns>Outputs the "Go To Page:" form HTML.</returns>
        public static HtmlString PagedListGoToPageForm(this IHtmlHelper html,
                                                          IPagedList list,
                                                          string formAction)
        {
            return PagedListGoToPageForm(html, list, formAction, "page");
        }

        ///<summary>
        /// Displays a configurable "Go To Page:" form for instances of PagedList.
        ///</summary>
        ///<param name="html">This method is meant to hook off HtmlHelper as an extension method.</param>
        ///<param name="list">The PagedList to use as the data source.</param>
        ///<param name="formAction">The URL this form should submit the GET request to.</param>
        ///<param name="inputFieldName">The querystring key this form should submit the new page number as.</param>
        ///<returns>Outputs the "Go To Page:" form HTML.</returns>
        public static HtmlString PagedListGoToPageForm(this IHtmlHelper html,
                                                          IPagedList list,
                                                          string formAction,
                                                          string inputFieldName)
        {
            return PagedListGoToPageForm(html, list, formAction, new GoToFormRenderOptions(inputFieldName));
        }

        ///<summary>
        /// Displays a configurable "Go To Page:" form for instances of PagedList.
        ///</summary>
        ///<param name="html">This method is meant to hook off HtmlHelper as an extension method.</param>
        ///<param name="list">The PagedList to use as the data source.</param>
        ///<param name="formAction">The URL this form should submit the GET request to.</param>
        ///<param name="options">Formatting options.</param>
        ///<returns>Outputs the "Go To Page:" form HTML.</returns>
        public static HtmlString PagedListGoToPageForm(this IHtmlHelper html,
                                                 IPagedList list,
                                                 string formAction,
                                                 GoToFormRenderOptions options)
        {
            var form = new TagBuilder("form");
            form.AddCssClass("PagedList-goToPage");
            form.Attributes.Add("action", formAction);
            form.Attributes.Add("method", "get");

            var fieldset = new TagBuilder("fieldset");

            var label = new TagBuilder("label");
            label.Attributes.Add("for", options.InputFieldName);
            label.SetInnerText(options.LabelFormat);

            var input = new TagBuilder("input");
            input.Attributes.Add("type", options.InputFieldType);
            input.Attributes.Add("name", options.InputFieldName);
            input.Attributes.Add("value", list.PageNumber.ToString());

            var submit = new TagBuilder("input");
            submit.Attributes.Add("type", "submit");
            submit.Attributes.Add("value", options.SubmitButtonFormat);

            fieldset.InnerHtml = label.ToString();
            fieldset.InnerHtml += input.ToString(TagRenderMode.SelfClosing);
            fieldset.InnerHtml += submit.ToString(TagRenderMode.SelfClosing);
            form.InnerHtml = fieldset.ToString();
            return new HtmlString(form.ToString());
        }
    }

    ///<summary>
	/// Options for configuring the output of <see cref = "HtmlHelper" />.
	///</summary>
	public class PagedListRenderOptions
    {
        ///<summary>
        /// The default settings render all navigation links and no descriptive text.
        ///</summary>
        public PagedListRenderOptions()
        {
            DisplayLinkToFirstPage = PagedListDisplayMode.IfNeeded;
            DisplayLinkToLastPage = PagedListDisplayMode.IfNeeded;
            DisplayLinkToPreviousPage = PagedListDisplayMode.IfNeeded;
            DisplayLinkToNextPage = PagedListDisplayMode.IfNeeded;
            DisplayLinkToIndividualPages = true;
            DisplayPageCountAndCurrentLocation = false;
            MaximumPageNumbersToDisplay = 10;
            DisplayEllipsesWhenNotShowingAllPageNumbers = true;
            EllipsesFormat = "&#8230;";
            LinkToFirstPageFormat = "««";
            LinkToPreviousPageFormat = "«";
            LinkToIndividualPageFormat = "{0}";
            LinkToNextPageFormat = "»";
            LinkToLastPageFormat = "»»";
            PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
            ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
            FunctionToDisplayEachPageNumber = null;
            ClassToApplyToFirstListItemInPager = null;
            ClassToApplyToLastListItemInPager = null;
            ContainerDivClasses = new[] { "pagination-container" };
            UlElementClasses = new[] { "pagination" };
            LiElementClasses = Enumerable.Empty<string>();
        }

        ///<summary>
        /// CSS Classes to append to the &lt;div&gt; element that wraps the paging control.
        ///</summary>
        public IEnumerable<string> ContainerDivClasses { get; set; }

        ///<summary>
        /// CSSClasses to append to the &lt;ul&gt; element in the paging control.
        ///</summary>
        public IEnumerable<string> UlElementClasses { get; set; }

        ///<summary>
        /// CSS Classes to append to every &lt;li&gt; element in the paging control.
        ///</summary>
        public IEnumerable<string> LiElementClasses { get; set; }

        ///<summary>
        /// Specifies a CSS class to append to the first list item in the pager. If null or whitespace is defined, no additional class is added to first list item in list.
        ///</summary>
        public string ClassToApplyToFirstListItemInPager { get; set; }

        ///<summary>
        /// Specifies a CSS class to append to the last list item in the pager. If null or whitespace is defined, no additional class is added to last list item in list.
        ///</summary>
        public string ClassToApplyToLastListItemInPager { get; set; }

        /// <summary>
        /// If set to Always, always renders the paging control. If set to IfNeeded, render the paging control when there is more than one page.
        /// </summary>
        public PagedListDisplayMode Display { get; set; }

        ///<summary>
        /// If set to Always, render a hyperlink to the first page in the list. If set to IfNeeded, render the hyperlink only when the first page isn't visible in the paging control.
        ///</summary>
        public PagedListDisplayMode DisplayLinkToFirstPage { get; set; }

        ///<summary>
        /// If set to Always, render a hyperlink to the last page in the list. If set to IfNeeded, render the hyperlink only when the last page isn't visible in the paging control.
        ///</summary>
        public PagedListDisplayMode DisplayLinkToLastPage { get; set; }

        ///<summary>
        /// If set to Always, render a hyperlink to the previous page of the list. If set to IfNeeded, render the hyperlink only when there is a previous page in the list.
        ///</summary>
        public PagedListDisplayMode DisplayLinkToPreviousPage { get; set; }

        ///<summary>
        /// If set to Always, render a hyperlink to the next page of the list. If set to IfNeeded, render the hyperlink only when there is a next page in the list.
        ///</summary>
        public PagedListDisplayMode DisplayLinkToNextPage { get; set; }

        ///<summary>
        /// When true, includes hyperlinks for each page in the list.
        ///</summary>
        public bool DisplayLinkToIndividualPages { get; set; }

        ///<summary>
        /// When true, shows the current page number and the total number of pages in the list.
        ///</summary>
        ///<example>
        /// "Page 3 of 8."
        ///</example>
        public bool DisplayPageCountAndCurrentLocation { get; set; }

        ///<summary>
        /// When true, shows the one-based index of the first and last items on the page, and the total number of items in the list.
        ///</summary>
        ///<example>
        /// "Showing items 75 through 100 of 183."
        ///</example>
        public bool DisplayItemSliceAndTotal { get; set; }

        ///<summary>
        /// The maximum number of page numbers to display. Null displays all page numbers.
        ///</summary>
        public int? MaximumPageNumbersToDisplay { get; set; }

        ///<summary>
        /// If true, adds an ellipsis where not all page numbers are being displayed.
        ///</summary>
        ///<example>
        /// "1 2 3 4 5 ...",
        /// "... 6 7 8 9 10 ...",
        /// "... 11 12 13 14 15"
        ///</example>
        public bool DisplayEllipsesWhenNotShowingAllPageNumbers { get; set; }

        ///<summary>
        /// The pre-formatted text to display when not all page numbers are displayed at once.
        ///</summary>
        ///<example>
        /// "..."
        ///</example>
        public string EllipsesFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to the first page. The one-based index of the page (always 1 in this case) is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "&lt;&lt; First"
        ///</example>
        public string LinkToFirstPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to the previous page. The one-based index of the page is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "&lt; Previous"
        ///</example>
        public string LinkToPreviousPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to each individual page. The one-based index of the page is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "{0}"
        ///</example>
        public string LinkToIndividualPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to the next page. The one-based index of the page is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "Next &gt;"
        ///</example>
        public string LinkToNextPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to the last page. The one-based index of the page is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "Last &gt;&gt;"
        ///</example>
        public string LinkToLastPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display when DisplayPageCountAndCurrentLocation is true. Use {0} to reference the current page and {1} to reference the total number of pages.
        ///</summary>
        ///<example>
        /// "Page {0} of {1}."
        ///</example>
        public string PageCountAndCurrentLocationFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display when DisplayItemSliceAndTotal is true. Use {0} to reference the first item on the page, {1} for the last item on the page, and {2} for the total number of items across all pages.
        ///</summary>
        ///<example>
        /// "Showing items {0} through {1} of {2}."
        ///</example>
        public string ItemSliceAndTotalFormat { get; set; }

        /// <summary>
        /// A function that will render each page number when specified (and DisplayLinkToIndividualPages is true). If no function is specified, the LinkToIndividualPageFormat value will be used instead.
        /// </summary>
        public Func<int, string> FunctionToDisplayEachPageNumber { get; set; }

        /// <summary>
        /// Text that will appear between each page number. If null or whitespace is specified, no delimiter will be shown.
        /// </summary>
        public string DelimiterBetweenPageNumbers { get; set; }

        /// <summary>
        /// An extension point which allows you to fully customize the anchor tags used for clickable pages, as well as navigation features such as Next, Last, etc.
        /// </summary>
        public Func<TagBuilder, TagBuilder, TagBuilder> FunctionToTransformEachPageLink { get; set; }

        /// <summary>
        /// Enables ASP.NET MVC's unobtrusive AJAX feature. An XHR request will retrieve HTML from the clicked page and replace the innerHtml of the provided element ID.
        /// </summary>
        /// <param name="options">The preferred Html.PagedList(...) style options.</param>
        /// <param name="ajaxOptions">The ajax options that will put into the link</param>
        /// <returns>The PagedListRenderOptions value passed in, with unobtrusive AJAX attributes added to the page links.</returns>
        //public static PagedListRenderOptions EnableUnobtrusiveAjaxReplacing(PagedListRenderOptions options, AjaxOptions ajaxOptions)
        //{
        //    options.FunctionToTransformEachPageLink = (liTagBuilder, aTagBuilder) =>
        //    {
        //        var liClass = liTagBuilder.Attributes.ContainsKey("class") ? liTagBuilder.Attributes["class"] ?? "" : "";
        //        if (ajaxOptions != null && !liClass.Contains("disabled") && !liClass.Contains("active"))
        //        {
        //            foreach (var ajaxOption in ajaxOptions.ToUnobtrusiveHtmlAttributes())
        //                aTagBuilder.Attributes.Add(ajaxOption.Key, ajaxOption.Value.ToString());
        //        }

        //        liTagBuilder.InnerHtml = aTagBuilder.ToString();
        //        return liTagBuilder;
        //    };
        //    return options;
        //}

        /// <summary>
        /// Enables ASP.NET MVC's unobtrusive AJAX feature. An XHR request will retrieve HTML from the clicked page and replace the innerHtml of the provided element ID.
        /// </summary>
        /// <param name="id">The element ID ("my_id") of the element whose innerHtml should be replaced, if # is included at the start this will be removed.</param>
        /// <returns>A default instance of PagedListRenderOptions value passed in, with unobtrusive AJAX attributes added to the page links.</returns>
        //public static PagedListRenderOptions EnableUnobtrusiveAjaxReplacing(string id)
        //{

        //    if (id.StartsWith("#"))
        //        id = id.Substring(1);

        //    var ajaxOptions = new AjaxOptions()
        //    {
        //        HttpMethod = "GET",
        //        InsertionMode = InsertionMode.Replace,
        //        UpdateTargetId = id
        //    };

        //    return EnableUnobtrusiveAjaxReplacing(new PagedListRenderOptions(), ajaxOptions);
        //}

        ///// <summary>
        ///// Enables ASP.NET MVC's unobtrusive AJAX feature. An XHR request will retrieve HTML from the clicked page and replace the innerHtml of the provided element ID.
        ///// </summary>
        ///// <param name="ajaxOptions">Ajax options that will be used to generate the unobstrusive tags on the link</param>
        ///// <returns>A default instance of PagedListRenderOptions value passed in, with unobtrusive AJAX attributes added to the page links.</returns>
        //public static PagedListRenderOptions EnableUnobtrusiveAjaxReplacing(AjaxOptions ajaxOptions)
        //{
        //    return EnableUnobtrusiveAjaxReplacing(new PagedListRenderOptions(), ajaxOptions);
        //}

        ///<summary>
        /// Also includes links to First and Last pages.
        ///</summary>
        public static PagedListRenderOptions Classic
        {
            get
            {
                return new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Never,
                    DisplayLinkToLastPage = PagedListDisplayMode.Never,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always
                };
            }
        }

        ///<summary>
        /// Also includes links to First and Last pages.
        ///</summary>
        public static PagedListRenderOptions ClassicPlusFirstAndLast
        {
            get
            {
                return new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                    DisplayLinkToLastPage = PagedListDisplayMode.Always,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always
                };
            }
        }

        ///<summary>
        /// Shows only the Previous and Next links.
        ///</summary>
        public static PagedListRenderOptions Minimal
        {
            get
            {
                return new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Never,
                    DisplayLinkToLastPage = PagedListDisplayMode.Never,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    DisplayLinkToIndividualPages = false
                };
            }
        }

        ///<summary>
        /// Shows Previous and Next links along with current page number and page count.
        ///</summary>
        public static PagedListRenderOptions MinimalWithPageCountText
        {
            get
            {
                return new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Never,
                    DisplayLinkToLastPage = PagedListDisplayMode.Never,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    DisplayLinkToIndividualPages = false,
                    DisplayPageCountAndCurrentLocation = true
                };
            }
        }

        ///<summary>
        ///	Shows Previous and Next links along with index of first and last items on page and total number of items across all pages.
        ///</summary>
        public static PagedListRenderOptions MinimalWithItemCountText
        {
            get
            {
                return new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Never,
                    DisplayLinkToLastPage = PagedListDisplayMode.Never,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    DisplayLinkToIndividualPages = false,
                    DisplayItemSliceAndTotal = true
                };
            }
        }

        ///<summary>
        ///	Shows only links to each individual page.
        ///</summary>
        public static PagedListRenderOptions PageNumbersOnly
        {
            get
            {
                return new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Never,
                    DisplayLinkToLastPage = PagedListDisplayMode.Never,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Never,
                    DisplayLinkToNextPage = PagedListDisplayMode.Never,
                    DisplayEllipsesWhenNotShowingAllPageNumbers = false
                };
            }
        }

        ///<summary>
        ///	Shows Next and Previous while limiting to a max of 5 page numbers at a time.
        ///</summary>
        public static PagedListRenderOptions OnlyShowFivePagesAtATime
        {
            get
            {
                return new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Never,
                    DisplayLinkToLastPage = PagedListDisplayMode.Never,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    MaximumPageNumbersToDisplay = 5
                };
            }
        }

        ///<summary>
        /// Twitter Bootstrap 2's basic pager format (just Previous and Next links).
        ///</summary>
        public static PagedListRenderOptions TwitterBootstrapPager
        {
            get
            {
                return new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Never,
                    DisplayLinkToLastPage = PagedListDisplayMode.Never,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    DisplayLinkToIndividualPages = false,
                    ContainerDivClasses = null,
                    UlElementClasses = new[] { "pager" },
                    ClassToApplyToFirstListItemInPager = null,
                    ClassToApplyToLastListItemInPager = null,
                    LinkToPreviousPageFormat = "Previous",
                    LinkToNextPageFormat = "Next"
                };
            }
        }

        ///<summary>
        /// Twitter Bootstrap 2's basic pager format (just Previous and Next links), with aligned links.
        ///</summary>
        public static PagedListRenderOptions TwitterBootstrapPagerAligned
        {
            get
            {
                return new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Never,
                    DisplayLinkToLastPage = PagedListDisplayMode.Never,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    DisplayLinkToIndividualPages = false,
                    ContainerDivClasses = null,
                    UlElementClasses = new[] { "pager" },
                    ClassToApplyToFirstListItemInPager = "previous",
                    ClassToApplyToLastListItemInPager = "next",
                    LinkToPreviousPageFormat = "&larr; Older",
                    LinkToNextPageFormat = "Newer &rarr;"
                };
            }
        }
    }

    /// <summary>
    /// A tri-state enum that controls the visibility of portions of the PagedList paging control.
    /// </summary>
    public enum PagedListDisplayMode
    {
        /// <summary>
        /// Always render.
        /// </summary>
        Always,

        /// <summary>
        /// Never render.
        /// </summary>
        Never,

        /// <summary>
        /// Only render when there is data that makes sense to show (context sensitive).
        /// </summary>
        IfNeeded
    }

    ///<summary>
	/// Options for configuring the output of <see cref = "HtmlHelper" />.
	///</summary>
	public class GoToFormRenderOptions
	{
		///<summary>
		/// The default settings, with configurable querystring key (input field name).
		///</summary>
		public GoToFormRenderOptions(string inputFieldName)
		{
			LabelFormat = "Go to page:";
			SubmitButtonFormat = "Go";
			InputFieldName = inputFieldName;
			InputFieldType = "number";
		}
	
		///<summary>
		/// The default settings.
		///</summary>
		public GoToFormRenderOptions() : this("page")
		{
		}

		///<summary>
		/// The text to show in the form's input label.
		///</summary>
		///<example>
		/// "Go to page:"
		///</example>
		public string LabelFormat { get; set; }

		///<summary>
		/// The text to show in the form's submit button.
		///</summary>
		///<example>
		/// "Go"
		///</example>
		public string SubmitButtonFormat { get; set; }

		///<summary>
		/// The querystring key this form should submit the new page number as.
		///</summary>
		///<example>
		/// "page"
		///</example>
		public string InputFieldName { get; set; }

		///<summary>
		/// The HTML input type for this field. Defaults to the HTML5 "number" type, but can be changed to "text" if targetting previous versions of HTML.
		///</summary>
		///<example>
		/// "number"
		///</example>
		public string InputFieldType { get; set; }
	}
}
