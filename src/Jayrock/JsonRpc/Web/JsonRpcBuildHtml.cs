using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jayrock.Json.RPC.JsonRpc.Web
{
    /// <summary>
    /// Generate HTML
    /// </summary>
    public class JsonRpcBuildHtml
    {
        /// <summary>
        /// Generate request parameter HTML
        /// </summary>
        public static string BuildHelpInput(string inputDescription)
        {
            StringBuilder htmlString = new StringBuilder();

            try
            {
                string[] splitArray = null;
                splitArray = Regex.Split(inputDescription, ";");

                if (splitArray.Length >= 1 && splitArray[0].Length > 0 && Regex.Split(splitArray[0], "--").Length == 6)
                {
                    htmlString.Append("<div class=\"help\">");
                    htmlString.Append("<table bordercolor=\"#dee7e6\" border=\"1\" cellspacing=\"0\">");
                    htmlString.Append("	<tbody>");
                    htmlString.Append("		<tr>");
                    htmlString.Append("			<th width=\"91\">");
                    htmlString.Append("				Request");
                    htmlString.Append("			</th>");
                    htmlString.Append("			<th width=\"33\">");
                    htmlString.Append("				Required");
                    htmlString.Append("			</th>");
                    htmlString.Append("			<th width=\"100\">");
                    htmlString.Append("				Type");
                    htmlString.Append("			</th>");
                    htmlString.Append("			<th width=\"465\">");
                    htmlString.Append("				Explain");
                    htmlString.Append("			</th>");
                    htmlString.Append("			<th width=\"66\">");
                    htmlString.Append("				Default");
                    htmlString.Append("			</th>");
                    htmlString.Append("			<th width=\"66\">");
                    htmlString.Append("				Test");
                    htmlString.Append("			</th>");
                    htmlString.Append("		</tr>");

                    int i = 0;
                    foreach (var s in splitArray)
                    {
                        string[] splitArray1 = null;
                        try
                        {
                            splitArray1 = Regex.Split(s, "--");
                            if (splitArray1.Length == 6)
                            {
                                htmlString.Append("		<tr "+(i%2==0?"":"class=\"line\"")+">");
                                htmlString.Append("			<td style=\"font-weight: bold;\">");
                                htmlString.Append("				" + splitArray1[0]);
                                htmlString.Append("			</td>");
                                htmlString.Append("			<td>");
                                htmlString.Append("				" + splitArray1[2]);
                                htmlString.Append("			</td>");
                                htmlString.Append("			<td>");
                                htmlString.Append("				" + splitArray1[1]);
                                htmlString.Append("			</td>");
                                htmlString.Append("			<td>");
                                htmlString.Append("				" + splitArray1[4]);
                                htmlString.Append("			</td>");
                                htmlString.Append("			<td>");
                                htmlString.Append("				" + splitArray1[3]);
                                htmlString.Append("			</td>");
                                htmlString.Append("			<td>");
                                htmlString.Append("				" + splitArray1[5]);
                                htmlString.Append("			</td>");
                                htmlString.Append("		</tr>");
                            }

                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }
                        i++;
                    }

                    htmlString.Append("	</tbody>");
                    htmlString.Append("</table>");
                    htmlString.Append("</div>");
                }
                else if (splitArray.Length >= 1 && splitArray[0].Length > 0 && Regex.Split(splitArray[0], "--").Length == 2)
                {
                    foreach (var s in splitArray)
                    {
                        string[] splitArray1 = null;
                        try
                        {
                            splitArray1 = Regex.Split(s, "--");
                            if (splitArray1.Length == 2)
                            {
                                htmlString.Append("<span style=\"font-weight: bold;\">[" + splitArray1[0] + "]</span>" + splitArray1[1] + "<br/>");
                            }

                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }
                    }
                }
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            return htmlString.ToString();
        }

        /// <summary>
        /// Gets the test value of the test page
        /// </summary>
        public static string GetTestValue(string inputDescription,string parameterName)
        {
            StringBuilder htmlString = new StringBuilder();

            try
            {
                string[] splitArray = null;
                splitArray = Regex.Split(inputDescription, ";");

                if (splitArray.Length >= 1 && splitArray[0].Length > 0 && Regex.Split(splitArray[0], "--").Length == 6)
                {
                    int i = 0;
                    foreach (var s in splitArray)
                    {
                        string[] splitArray1 = null;
                        try
                        {
                            splitArray1 = Regex.Split(s, "--");
                            if (splitArray1.Length == 6)
                            {
                                if (parameterName.Equals(splitArray1[0]) && splitArray1[5].Length > 0)
                                {
                                    if (splitArray1[1].Equals(JsonType.Array.ToString().ToLower()))
                                    {
                                        htmlString.Append(splitArray1[5]);
                                    }
                                    else if (splitArray1[1].Equals(JsonType.Boolean.ToString().ToLower()))
                                    {
                                        htmlString.Append(splitArray1[5]);
                                    }
                                    else if (splitArray1[1].Equals(JsonType.Null.ToString().ToLower()))
                                    {
                                        htmlString.Append("null");
                                    }
                                    else if (splitArray1[1].Equals(JsonType.Number.ToString().ToLower()))
                                    {
                                        htmlString.Append(splitArray1[5]);
                                    }
                                    else if (splitArray1[1].Equals(JsonType.Object.ToString().ToLower()))
                                    {
                                        htmlString.Append(splitArray1[5]);
                                    }
                                    else if (splitArray1[1].Equals(JsonType.String.ToString().ToLower()))
                                    {
                                        htmlString.Append(string.Format("\"{0}\"",splitArray1[5]));
                                    }
                                    else if (splitArray1[1].Equals(JsonType.Value.ToString().ToLower()))
                                    {
                                        htmlString.Append(splitArray1[5]);
                                    }
                                    else
                                    {
                                        htmlString.Append(string.Format("\"{0}\"", splitArray1[5]));
                                    }
                                }
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }
                        i++;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            if (htmlString.Length==0)
            {
                htmlString.Append("null");
            }
            return htmlString.ToString();
        }

        /// <summary>
        /// Generates a return value field HTML
        /// </summary>
        public static string BuildHelpOutput(string outputDescription)
        {
            StringBuilder htmlString = new StringBuilder();

            try
            {
                string[] splitArray = null;
                splitArray = Regex.Split(outputDescription, ";");

                if (splitArray.Length >= 1 && splitArray[0].Length > 0 && Regex.Split(splitArray[0], "--").Length == 3)
                {
                    htmlString.Append("<div class=\"help\">");
                    htmlString.Append("<table bordercolor=\"#dee7e6\" border=\"1\" cellspacing=\"0\">");
                    htmlString.Append("	<tbody>");
                    htmlString.Append("		<tr>");
                    htmlString.Append("			<th width=\"91\">");
                    htmlString.Append("				Return");
                    htmlString.Append("			</th>");
                    htmlString.Append("			<th width=\"100\">");
                    htmlString.Append("				Type");
                    htmlString.Append("			</th>");
                    htmlString.Append("			<th width=\"630\">");
                    htmlString.Append("				Explain");
                    htmlString.Append("			</th>"); 
                    htmlString.Append("		</tr>");
                    
                    int i = 0;
                    foreach (var s in splitArray)
                    {
                        string[] splitArray1 = null;
                        try
                        {
                            splitArray1 = Regex.Split(s, "--");
                            if (splitArray1.Length == 3)
                            {
                                htmlString.Append("		<tr " + (i % 2 == 0 ? "" : "class=\"line\"") + ">");
                                htmlString.Append("			<td style=\"font-weight: bold;\">");
                                htmlString.Append("				" + splitArray1[0]);
                                htmlString.Append("			</td>");
                                htmlString.Append("			<td>");
                                htmlString.Append("				" + splitArray1[1]);
                                htmlString.Append("			</td>");
                                htmlString.Append("			<td>");
                                htmlString.Append("				" + splitArray1[2]);
                                htmlString.Append("			</td>");
                                htmlString.Append("		</tr>");
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }
                        i++;
                    }

                    htmlString.Append("	</tbody>");
                    htmlString.Append("</table>");
                    htmlString.Append("</div>");
                }
                else if (splitArray.Length >= 1 && splitArray[0].Length > 0 && Regex.Split(splitArray[0], "--").Length == 2)
                {
                    foreach (var s in splitArray)
                    {
                        string[] splitArray1 = null;
                        try
                        {
                            splitArray1 = Regex.Split(s, "--");
                            if (splitArray1.Length == 2)
                            {
                                htmlString.Append("<span style=\"font-weight: bold;\">[" + splitArray1[0] + "]</span>" + splitArray1[1] + "<br/>");
                            }

                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }
                    }
                }
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            return htmlString.ToString();
        }
    }
}
