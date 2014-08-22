using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;


namespace WpfApplication1
{
   
   public class Savexml
    {
        private  string xml_context;
        private  string xml_style;
        private static XmlNode root_context;
        private static XmlNode root_style;
        private string  type; //节点类型，路劲
        private string _id;
        private string context_html;
        public  XmlDocument doc_style;
        private string xml_tem;
        public  XmlDocument doc_context;
        private XmlDocument doc_tem;
        private XmlNode root_tem;
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
            xml_tem = MainWindow.idd_href + "\\webTemplate.xml";
            doc_tem = new XmlDocument();
            doc_tem.Load(xml_tem);
            root_tem = doc_tem.DocumentElement;
           
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
        public void savexml()  //保存更改后的信息
        {
            XmlNodeList ccwww = root_context.SelectNodes(type);
            foreach (XmlNode ccd in ccwww)
            {
                if (((XmlElement)ccd).GetAttribute("id") == _id)
                {
                    ccd.InnerXml = context_html.Replace("&", "&amp;");
                    doc_context.Save(@xml_context);
                    ThreadPool.QueueUserWorkItem(status=>savetem(ccd));
                    break;
                }     
            }   
        }
        public void savetem(XmlNode cc)
        {
            XmlNodeList ccwww = root_tem.SelectNodes(type);
            foreach (XmlNode ccd in ccwww)
            {
                if (((XmlElement)ccd).GetAttribute("id") == _id)
                {
                    foreach (XmlNode xm in ccd.ChildNodes)
                    {
                        xm.InnerText = cc.SelectSingleNode(xm.Name).InnerText;
                    }
                    break;
                }
            }
            doc_tem.Save(xml_tem);
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
