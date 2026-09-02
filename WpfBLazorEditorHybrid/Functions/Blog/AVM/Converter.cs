using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using static System.Net.Mime.MediaTypeNames;


namespace WpfBLazorHybridClient.Functions.Blog.AVM
{
    public static class Converter
    {
        public static FlowDocument Execute(string text, int _length) 
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(text);
            int length = _length;

            FlowDocument flowDoc = new FlowDocument();

            foreach (var node in htmlDoc.DocumentNode.ChildNodes)
            {
                if (length == 0) break;
                List? li = FoundList(node);
                if (li!=null) flowDoc.Blocks.Add(li);
                Paragraph? para = FoundParagraph(node);
                if(para!=null) flowDoc.Blocks.Add(para);
            }
            return flowDoc;



            void FoundBold(HtmlNode node, InlineCollection inlines)
            {
                var nodeName = node.Name;
                if (nodeName == "b" || nodeName == "B")
                {
                    Bold bold = new Bold();
                    if (node.HasChildNodes)
                    {
                        foreach (var child in node.ChildNodes)
                        {
                            if (length == 0) break;
                            Run text = FoundText(child);
                            if (text != null)
                            {
                                bold.Inlines.Add(text);
                            }
                            else FoundMarkedText(child, bold.Inlines);
                        }
                    }
                    inlines.Add(bold);
                }
            }

            void FoundItalic(HtmlNode node, InlineCollection inlines)
            {
                var nodeName = node.Name;
                if (nodeName == "i" || nodeName == "I")
                {
                    Italic italic = new Italic();
                    if (node.HasChildNodes)
                    {
                        foreach (var child in node.ChildNodes)
                        {
                            if (length == 0) break;
                            Run text = FoundText(child);
                            if (text != null)
                            {
                                italic.Inlines.Add(text);
                            }
                            else FoundMarkedText(child, italic.Inlines);
                        }
                    }
                    inlines.Add(italic);
                }
            }

            void FoundUnderline(HtmlNode node, InlineCollection inlines)
            {
                var nodeName = node.Name;
                if (nodeName == "u" || nodeName == "U")
                {
                    Underline underline = new Underline();
                    if (node.HasChildNodes)
                    {
                        foreach (var child in node.ChildNodes)
                        {
                            if (length == 0) break;
                            Run text = FoundText(child);
                            if (text != null)
                            {
                                underline.Inlines.Add(text);
                            }
                            else FoundMarkedText(child, underline.Inlines);
                        }
                    }
                    inlines.Add(underline);
                }
            }

            void FoundStrikethrough(HtmlNode node, InlineCollection inlines)
            {
                var nodeName = node.Name;
                if (nodeName == "strike" || nodeName == "STRIKE")
                {
                    Span span = new Span();
                    if (node.HasChildNodes)
                    {
                        foreach (var child in node.ChildNodes)
                        {
                            if (length == 0) break;
                            Run text = FoundText(child);
                            if (text != null)
                            {
                                span.Inlines.Add(text);
                                span.TextDecorations = TextDecorations.Strikethrough;
                            }
                            else FoundMarkedText(child, span.Inlines);
                        }
                    }
                    inlines.Add(span);
                }
            }

            void FoundMarkedText(HtmlNode child, InlineCollection inlines)
            {
                FoundStrikethrough(child, inlines);
                FoundUnderline(child, inlines);
                FoundItalic(child, inlines);
                FoundBold(child, inlines);
            }



            Run? FoundText(HtmlNode node)
            {
                var nodeName = node.Name;
                if (nodeName == "#text")
                {
                    var childtext = node.InnerText;
                    childtext = childtext.Replace("&nbsp;", " ");
                    if (childtext.Length > length)
                    {
                        childtext = childtext.Substring(0, length) + "...";
                        length = 0;
                    }
                    else length = length - childtext.Length;

                    Run text = new Run(childtext);
                    return text;
                }
                else return null;
            }


            List? FoundList(HtmlNode node)
            {
                var nodeName = node.Name;
                if (nodeName == "li" || nodeName == "LI")
                {
                    List list = new List();
                    foreach (var child in node.ChildNodes)
                    {
                        if (length == 0) break;
                        if (child.Name == "ul" || child.Name == "UL")
                        {
                            Paragraph para = new Paragraph();
                            ListItem item = new ListItem(para);
                            if (child.HasChildNodes)
                            {
                                foreach (var subchild in child.ChildNodes)
                                {
                                    if (length == 0) break;
                                    Run run = FoundText(child);
                                    if (run != null) para.Inlines.Add(run);
                                    else FoundMarkedText(subchild, para.Inlines);
                                }
                            }
                            list.ListItems.Add(item);

                        }
                        else
                            if (child.Name == "ol" || child.Name == "OL")
                            {
                                list.MarkerStyle = TextMarkerStyle.Decimal;
                                Paragraph para = new Paragraph();
                                ListItem item = new ListItem(para);
                                if (child.HasChildNodes)
                                {
                                    foreach (var subchild in child.ChildNodes)
                                    {
                                        if (length == 0) break;
                                        Run run = FoundText(child);
                                        if (run != null) para.Inlines.Add(run);
                                        else FoundMarkedText(subchild, para.Inlines);
                                    }
                                }
                                list.ListItems.Add(item);
                            }
                    }
                    return list;
                }
                return null;
            }


            Paragraph? FoundParagraph(HtmlNode node)
            {
                var nodeName = node.Name;
                if (nodeName == "p" || nodeName == "P")
                {
                    Paragraph para = new Paragraph();
                    if (node.HasChildNodes)
                    {
                        foreach (var child in node.ChildNodes)
                        {
                            if (length == 0) break;
                            Run run = FoundText(child);
                            if (run != null) para.Inlines.Add(run);
                            else FoundMarkedText(child, para.Inlines);
                        }
                    }
                    return para;
                }
                return null;
            }


        }
    }
}
