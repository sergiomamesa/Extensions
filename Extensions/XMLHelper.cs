using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Extensions
{
    public class XMLHelper
    {
        protected String sFileName = "";
        protected XmlDocument Document;

        public XMLHelper(string fileName, bool hasDeclaration = false)
        {
            sFileName = fileName;
            Document = new XmlDocument();
            if (hasDeclaration)
            {
                XmlDeclaration xmldecl = Document.CreateXmlDeclaration("1.0", "UTF-8", null);
                Document.InsertBefore(xmldecl, Document.DocumentElement);
            }

        }

        public XmlAttributeCollection GetAttributes()
        {
            return Document.Attributes;
        }

        /// <summary>
        /// Creates an element on the top level
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public XmlElement CreateElement(string name)
        {
            var element = Document.CreateElement(name.ToXMLName());
            Document.AppendChild(element);

            return element;
        }

        /// <summary>
        /// Creates an element on the top level with an attribute
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public XmlElement CreateElement(string name, XmlAttribute attribute)
        {
            var element = Document.CreateElement(name.ToXMLName());

            element.SetAttribute(attribute.Name, attribute.Value);

            Document.AppendChild(element);

            return element;
        }

        /// <summary>
        /// Creates an element on the top level with attributes
        /// </summary>
        /// <param name="name"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public XmlElement CreateElement(string name, XmlAttributeCollection collection)
        {
            var element = Document.CreateElement(name.ToXMLName());

            foreach (XmlAttribute item in collection)
            {
                element.SetAttribute(item.Name, item.Value);
            }

            Document.AppendChild(element);

            return element;
        }

        /// <summary>
        /// Append an element to the defined parent element
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public XmlElement CreateElement(string name, XmlElement parent)
        {
            var element = Document.CreateElement(name.ToXMLName());
            parent.AppendChild(element);

            return element;
        }

        public virtual XmlElement CreateElement(string name, string value, XmlElement parent, bool isMandatory = false)
        {
            //var realValue = value.Substring(value.IndexOf("<![CDATA[") + "<![CDATA[".Count(), value.IndexOf("]]>"));

            if (!value.HasValue())
            {
                if (isMandatory)
                    throw new Exception("El valor {0} para el fichero {1}, es obligatorio".FormatWith(name, this.sFileName));
            }

            if (value.HasValue())
            {
                var element = CreateElement(name.ToXMLName(), parent);
                if (value.LastIndexOf("[CDATA[") != -1)
                    element.InnerXml = value;
                else
                    element.InnerText = value;

                return element;
            }
            return null;
        }

        public virtual XmlElement CreateElement(string name, int? value, XmlElement parent, bool isMandatory = false)
        {
            return CreateElement(name, value.ToString(), parent, isMandatory);
        }

        public XmlElement CreateElement(string name, XmlElement parent, XmlAttributeCollection collection)
        {
            var element = CreateElement(name, parent);

            foreach (XmlAttribute item in collection)
            {
                element.SetAttribute(item.Name, item.Value);
            }

            return element;
        }

        public XmlElement CreateElement(string name, XmlElement parent, XmlAttribute attribute)
        {
            var element = CreateElement(name, parent);
            element.SetAttribute(attribute.Name, attribute.Value);

            return element;
        }

        public XmlElement CreateElement(string name, string value, XmlElement parent, XmlAttribute attribute)
        {
            var element = CreateElement(name, value, parent);
            element.SetAttribute(attribute.Name, attribute.Value);

            return element;
        }

        public XmlAttribute CreateAttribute(string name, string value)
        {
            var attribute = Document.CreateAttribute(name);
            attribute.Value = value;

            return attribute;
        }

        public virtual void Save()
        {
            if (sFileName.HasValue())
            {
                string s = Document.InnerXml;
                s = s.Replace("&amp;", "&");
                using (TextWriter sw = new StreamWriter(sFileName, false, Encoding.UTF8)) //Set encoding
                {
                    sw.Write(s);
                }
                //Document.Save(sFileName);
            }
        }
    }
}
