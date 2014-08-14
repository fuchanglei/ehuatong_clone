using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


namespace WpfApplication1
{
   
   public class Savexml
    {
        private  string xml_context;
        private  string xml_style;
        private static XmlNode root_context;
        private static XmlNode root_style;
        private string  type;
        private string _id;
        private string context_html;
        public  XmlDocument doc_style; 
        public  XmlDocument doc_context;
       // private  XmlNodeList chilnodes;
        public Savexml(string _type,string _html,string id)
        {
            type = _type;
            context_html = _html;
            _id = id;
            xml_context = MainWindow.idd_href + "/idis.xml"; 
            doc_context = new XmlDocument();
            doc_context.Load(xml_context);
            root_context = doc_context.DocumentElement;
            //chilnodes = root_context.ChildNodes;

        }
        public Savexml(string path)
        {
            xml_context = path + "/idis.xml"; 
            xml_style= path + "/webTemplateStyle.xml";
            doc_style = new XmlDocument();
            doc_style.Load(xml_style);
            root_style = doc_style.DocumentElement;
            doc_context = new XmlDocument();
            doc_context.Load(xml_context);
            root_context = doc_context.DocumentElement;
        }
        public void savexml()
        {
            XmlNodeList ccwww = root_context.SelectNodes(type);
            foreach (XmlNode ccd in ccwww)
            {
                if (((XmlElement)ccd).GetAttribute("id") == _id)
                {
                    ccd.InnerXml = context_html.Replace("&", "&amp;");
                    doc_context.Save(@xml_context);
                    break;
                }
                   
            }
            
        } 
        public void init_idis()  //创建的时候初始化idis
        {
            XmlNodeList styles = root_style.ChildNodes;
            foreach (XmlNode xm in styles)
            {
                if (xm.Name.Contains("Chapter") == true || xm.Name.Contains("Section") == true)
                {
                    if (xm.Name == "Chapter")
                    {
                        (root_context.SelectSingleNode("Papersection/Chapter")).InnerXml = xm.InnerXml;
                       // doc_context.Save(@xml_context);
                       // xm.Name
                    }
                    else if (xm.Name == "Section1")
                    {
                        (root_context.SelectSingleNode("Papersection/Section1")).InnerXml = xm.InnerXml;
                       // doc_context.Save(@xml_context);
                    }
                    else
                    {
                        (root_context.SelectSingleNode("Papersection/Section2")).InnerXml = xm.InnerXml;
                       // doc_context.Save(@xml_context);
                    }
                }
                else
                    (root_context.SelectSingleNode(xm.Name)).InnerXml = xm.InnerXml;
                    doc_context.Save(@xml_context);
            }
            
        }
    }
}
