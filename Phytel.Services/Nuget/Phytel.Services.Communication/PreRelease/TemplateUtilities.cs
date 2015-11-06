using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Phytel.Services.Communication
{
    public class TemplateUtilities
    {
        public TemplateUtilities(){ }
        
        public string GetModeSpecificTag(string originalTag, string mode)
        {
            string modeSpecificTag = string.Empty;

            modeSpecificTag = originalTag.Replace(XMLFields.ModePlaceHolder, mode);

            return modeSpecificTag;
        }

        public string ProperCase(string input)
        {
            string pattern = @"\w+|\W+";
            string result = "";
            Boolean capitalizeNext = true;

            foreach (Match m in Regex.Matches(input, pattern))
            {
                // get the matched string
                string x = m.ToString().ToLower();

                // if the first char is lower case
                if (char.IsLower(x[0]) && capitalizeNext)
                {
                    // capitalize it
                    x = char.ToUpper(x[0]) + x.Substring(1, x.Length - 1);
                }

                // Check if the word starts with Mc
                //if (x[0] == 'M' && x[1] == 'c' && !String.IsNullOrEmpty(x[2].ToString()))
                if (x[0] == 'M' && x.Length > 1 && x[1] == 'c' && !String.IsNullOrEmpty(x[2].ToString()))
                {
                    // Capitalize the letter after Mc
                    x = "Mc" + char.ToUpper(x[2]) + x.Substring(3, x.Length - 3);
                }

                if (capitalizeNext == false)
                    capitalizeNext = true;

                // if the apostrophe is at the end i.e. Andrew's
                // then do not capitalize the next letter
                if (x[0].ToString() == "'" && m.NextMatch().ToString().Length == 1)
                {
                    capitalizeNext = false;
                }

                // collect all text
                result += x;
            }
            return result;
        }

        public Hashtable AddMissingObjects(Hashtable missingObjects, string missingObjString)
        {
            if (!missingObjects.ContainsValue(missingObjString))
            {
                missingObjects.Add(missingObjects.Count + 1, missingObjString);
            }
            return missingObjects;
        }

        public XmlNode SetXMlNodeInnerText(XmlNode originalNode, string innerText)
        {
            try
            {
                if (originalNode != null)
                {
                    //HandleXMlSpecialCharacters(ref innerText);
                    originalNode.InnerText = innerText;
                }
            }
            catch (XmlException xmlEx)
            {
                throw xmlEx;
            }
            catch (Exception e)
            {
                throw e;
            }
            return originalNode;
        }

        public XmlDocument SetCDATAXMlNodeInnerText(XmlNode node, string innerText, XmlDocument xmlDoc)
        {
            try
            {
                if (node != null)
                {
                    XmlAttribute isCDATAAttr = node.Attributes["IsCDATA"];
                    if (isCDATAAttr != null && isCDATAAttr.Value.ToString().ToLower() == "true")
                    {
                        XmlCDataSection cdata = xmlDoc.CreateCDataSection(innerText);
                        node.AppendChild(cdata);
                    }
                    else
                    {
                        HandleXMlSpecialCharacters(innerText);
                        node.InnerText = innerText;
                    }
                }
            }
            catch (XmlException xmlEx)
            {
                throw xmlEx;
            }
            catch (Exception e)
            {
                throw e;
            }
            return xmlDoc;
        }

        public string HandleXMlSpecialCharacters(string innerText)
        {
            if (!String.IsNullOrEmpty(innerText) && !String.IsNullOrEmpty(innerText.Trim()))
            {
                //1.& - &amp; 
                innerText = innerText.Replace("&", "&amp;");
                //2.< - &lt; 
                innerText = innerText.Replace("<", "&lt;");
                //3.> - &gt; 
                innerText = innerText.Replace(">", "&gt;");
                //4." - &quot; 
                innerText = innerText.Replace("\"", "&quot;");
                //5.' - &apos;
                innerText = innerText.Replace("'", "&apos;");
            }
            return innerText;
        }
    }
}
