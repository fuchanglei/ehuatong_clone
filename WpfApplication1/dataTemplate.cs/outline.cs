using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Xml;
//using Code.WebbrowserInteropr;

namespace WpfApplication1
{
    public enum outlinetype
    {   
        common=0,
        Chapter = 1,
        Section1=2,
        Section2=3,
        empty=4,
    }

 public class outline : INotifyPropertyChanged  //目录数据类型
    {
       public string secid { get; set; }
       private string _Name;
       public string Name1 {
           get
           {
               return _Name;
           }
           set
           {
               if (_Name != value)
               {
                   _Name = value;
                   OnPropertyChanged("Name");
               }
           }
       }
       public string nodename{get;set;}
       private outline _parent;
       public outline parent
       {
           get
           {
               return this._parent;
           }
           set
           {
               if (_parent!= value)
               {
                   _parent = value;
                   OnPropertyChanged("parent");
               }
           }
       }
       public string toollip { get; set; }
       //public string nodetype { get; set; }
       private outlinetype _type;
       public outlinetype type
       {
           get
           {
               return this._type;
           }
           set
           {
               if (_type != value)
               {
                   _type = value;
                   OnPropertyChanged("type");
               }
           }
       }
       private string _href;
       public string href
       {
           get
           {
               return this._href;
           }
           set
           {
               if (_href != value)
               {
                   _href = value;
               }
           }
       }
       public ObservableCollection<outline> children{ get; set; }
       public outline()
       {
           children = new ObservableCollection<outline>();
       }
     
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
        #endregion
   public class outline_Data {
       private XmlDocument doc_outline;
       private XmlDocument doc_contex;
       private XmlNode root_outline;
       private XmlNode root_contex;
       private string outline_xml;
       private string idis_xml;
       private static outline_Data mInstance = new outline_Data();
       public static outline_Data Instance
       {
           get { return mInstance; }
       }
       private outline getnodes(XmlNode parent, outline parentnode) //parent 是xml节点的父节点parentnode是父节点在tree中的父节点
       {
           //
           outline aa = new outline()
           {
               secid = ((XmlElement)parent).GetAttribute("id"),
               Name1 = ((XmlElement)parent).GetAttribute("id")+" "+((XmlElement)parent).GetAttribute("sec-title"),
               nodename=parent.Name,
               toollip=((XmlElement)parent).GetAttribute("sec-title"),
               href =((XmlElement)parent).GetAttribute("href"),
               parent=parentnode,
               type = (outlinetype)((int)parentnode.type+1)
              // nodetype=parent.Name
               
               
           };
           
           if (parent.HasChildNodes == false)
           {
              
               return aa;
           }
           else
           {
              // Console.WriteLine(aa.secid + " " + aa.Name);
               XmlNodeList ac = parent.ChildNodes;
               foreach (XmlNode acs in ac)
               {
                   aa.children.Add(getnodes(acs,aa));
               }
               return aa;
           }
          
       }
       public outline_Data()
       {
           outline_xml = MainWindow.idd_href + "\\iDisseration_outline.xml";
           idis_xml = MainWindow.idd_href + "\\idis.xml";
           doc_outline = new XmlDocument();
           doc_outline.Load(outline_xml);
           root_outline = doc_outline.DocumentElement;
           doc_contex = new XmlDocument();
           doc_contex.Load(idis_xml);
           root_contex = doc_contex.DocumentElement.SelectSingleNode("Papersection");

           
       }
       public static string getparent(outline sel)
       {
           if (sel.parent == null)
               return sel.nodename;
           else
              return getparent(sel.parent)+"/"+sel.nodename;
             
       }  //获取选中节点的父类
       private ObservableCollection<outline> GenerateTreeViewItems1()
       {
           ObservableCollection<outline> cc = new ObservableCollection<outline>();
           XmlDocument doc = new XmlDocument();
           doc.Load(MainWindow.idd_href+"\\iDisseration_outline.xml");
           //Console.WriteLine("Begin reading xml");
          // XmlElement rootElem = doc.DocumentElement;
           //XmlNodeList section = rootElem.GetElementsByTagName("section");
           //List<outline> cc = new List<outline>();
           XmlNode root = doc.DocumentElement;
           XmlNodeList xmllist = root.ChildNodes;
           foreach (XmlNode xm in xmllist)
           {
               if (xm.Name.Contains("Chapter")==false)
               {
                   outline newnoe = new outline()
                   {
                       secid = ((XmlElement)xm).GetAttribute("id"),
                       Name1 = xm.InnerText,
                       nodename=xm.Name,
                       toollip = xm.InnerText,
                       type =outlinetype.common,
                       href= ((XmlElement)xm).GetAttribute("href"),
                       parent=null

                   };
                   
                   cc.Add(newnoe);
                   // Console.WriteLine(xm.Name);
                  // Console.WriteLine(xm.InnerText);
               }
               else
               {
                   outline chapter = new outline()
                   {
                       secid = ((XmlElement)xm).GetAttribute("id"),
                       Name1 = ((XmlElement)xm).GetAttribute("id")+" "+((XmlElement)xm).GetAttribute("sec-title"),
                       nodename=xm.Name,
                       href = ((XmlElement)xm).GetAttribute("href"),
                       toollip=((XmlElement)xm).GetAttribute("sec-title"),
                       type=outlinetype.Chapter,
                       parent=null
                   };
                   cc.Add(chapter);
                   //Console.WriteLine(xm.Name);
                   XmlNodeList xml = xm.ChildNodes;

                   foreach (XmlNode xms in xml)
                   {
                       string title = ((XmlElement)xms).GetAttribute("sec-title");
                       outline aa = new outline()
                       {
                           secid = ((XmlElement)xms).GetAttribute("id"),
                           Name1 = ((XmlElement)xms).GetAttribute("sec-title"),
                           nodename=xms.Name,
                           href =((XmlElement)xms).GetAttribute("href"),
                           toollip=title,
                           type=outlinetype.Section1,
                           parent=chapter

                       };
                       
                       chapter.children.Add(getnodes(xms,chapter));
                       // Console.WriteLine(xms.Name);
                       //Console.WriteLine(((XmlElement)xms).GetAttribute("sec-id"));
                       //Console.WriteLine(((XmlElement)xms).GetAttribute("sec-title"));
                       //Console.WriteLine(((XmlElement)xms).GetAttribute("href"));
                   }

               }

           }
           return cc;
       }
       public ObservableCollection<outline> TreeViewItems1
       {
           get
           {
               if (mTreeViewItems1 == null)
                   mTreeViewItems1 = GenerateTreeViewItems1();
               return mTreeViewItems1;
           }
          
       }
       private XmlNode getselectnode(outline ss)
       {
           XmlNode select=null;
           XmlNodeList xmllist = root_outline.SelectNodes(getparent(ss));
           foreach (XmlNode ses in xmllist)
           {
               if (((XmlElement)ses).GetAttribute("id") == ss.secid)
               {
                   select = ses;
                   break;
               }
           }
           return select;
       }
       public void outline_node_add(outline select,string newname)  //增加小节
       {
           
         //  string cc = getparent(select);
           //root_outline = doc.DocumentElement;
           XmlNode a = getselectnode(select);
           XmlNodeList xmllist = a.ChildNodes;
           string id=select.secid+"."+(xmllist.Count+1).ToString();
           string newname_add = id + " " + newname;
           string href = select.href + "/" + id;
           string type = ((outlinetype)((int)select.type + 1)).ToString();
           string nodename = type;
           outline newone = new outline()
           {
               Name1 = newname_add,
               toollip = newname_add,
               parent = select,
               secid=id,
               nodename=nodename,
               href=href,
               type = (outlinetype)((int)select.type + 1)
           };
           select.children.Add(newone);
           XmlElement xe1 = doc_outline.CreateElement(nodename);//创建一个节点
           xe1.SetAttribute("id", id);//设置该节点id属性
           xe1.SetAttribute("sec-title", newname);//设置该节点name属性
           xe1.SetAttribute("nodetype", type);//设置该节点type属性
           xe1.SetAttribute("href", href);//设置节点的href
           a.AppendChild(xe1);
           doc_outline.Save(outline_xml);
           XmlDocument doc = new XmlDocument();
           doc.Load( MainWindow.idd_href + "/webTemplateStyle.xml");
           XmlNode style= doc.DocumentElement.SelectSingleNode(type);
           XmlElement xe2 = doc_contex.CreateElement(nodename);
           xe2.SetAttribute("id",id);
           xe2.InnerXml = style.InnerXml;
           root_contex.AppendChild(xe2);
           doc_contex.Save(idis_xml);
       }
       private void delete_contex(string name,string id)
       {
           XmlNodeList context = root_contex.SelectNodes(name);
           foreach (XmlNode xm in context)
           {
               if (((XmlElement)xm).GetAttribute("id") == id)
               {
                   root_contex.RemoveChild(xm);
                   doc_contex.Save(idis_xml);
                   break;
               }
           }
       }
       private void updateid_context(string name,string id,string newid)
       { 
           XmlNodeList context = root_contex.SelectNodes(name);
           foreach (XmlNode xm in context)
               {
                   if (((XmlElement)xm).GetAttribute("id") == id)
                   {    

                       ((XmlElement)xm).SetAttribute("id",newid);
                       doc_contex.Save(idis_xml);
                       break;
                   }
               }
       }
       private void updateid(XmlNode ss,string oldid,string newid)
       {
           
           Regex r = new Regex(oldid);
           XmlElement xe = (XmlElement)ss;
           string id=xe.GetAttribute("id");
           string _newid = r.Replace(id,newid,1);//只替换一次
           if (ss.HasChildNodes == false)
           {
              
               updateid_context(ss.Name,id,_newid);
               xe.SetAttribute("id",_newid);
               doc_outline.Save(outline_xml);
               
           }
           else
           {
               XmlNodeList xmlist = ss.ChildNodes;
               foreach (XmlNode xn in xmlist)
               {
                   updateid(xn,id,_newid);

               }
           }

       }
       private void upadatexml(XmlNode select,string oldid)
       {
           //XmlNode ss = select.NextSibling;
           string id = ((XmlElement)select).GetAttribute("id");
           ((XmlElement)select).SetAttribute("id", oldid);
           updateid_context(select.Name, id, oldid);
           foreach (XmlNode ccs in select.ChildNodes)
           {
               updateid(ccs, id, oldid);
           }
           if (select.NextSibling == null || ((XmlElement)select.NextSibling).GetAttribute("nodetype") == "common")
               return;
           else
           {
               //string cc = ((XmlElement)select).GetAttribute("id");



               //  doc_outline.Save(outline_xml);
               upadatexml(select.NextSibling, id);


           }
       }
       private void delet_outline(XmlNode deletenode) //删除outjlin节点同时删除idis相对应的节点
       {
           if (deletenode.HasChildNodes == false)
           {
               deletenode.ParentNode.RemoveChild(deletenode);
               delete_contex(deletenode.Name,((XmlElement)deletenode).GetAttribute("id"));
           }
           else
           {
               foreach (XmlNode xm in deletenode.ChildNodes)
               {
                   //deletenode.RemoveChild(xm);
                   delet_outline(xm);

               }
               deletenode.ParentNode.RemoveChild(deletenode);
               delete_contex(deletenode.Name, ((XmlElement)deletenode).GetAttribute("id"));
           }
       }
       public void outline_node_delet(outline select) //删除节点
       {
           
           XmlNode b = getselectnode(select);
           XmlNode b_next = b.NextSibling;
           string oldid = ((XmlElement)b).GetAttribute("id");
           delet_outline(b);
           //root_outline.RemoveChild(b);
           delete_contex(b.Name, oldid);
           if(b_next!=null)
           {
          
               upadatexml(b_next, oldid);
           }
           
           
      
           doc_outline.Save(outline_xml);
           
          

       }
       public void outline_node_modify(outline select, string newname)  //修改outline的接点名称
       {
          // doc_contex = new XmlDocument();
           //doc_contex.Load(idis_xml);
          // root_contex = doc_contex.DocumentElement.SelectSingleNode("Papersection");
           //string cc = getparent(select);
           XmlNode a = getselectnode(select);
           //root_outline = doc.DocumentElement;
           //XmlNodeList xmllist = root_outline.SelectSingleNode(cc).ChildNodes;
           string id = select.secid;
           //string newname_add = id + " " + newname;
          // string href = select.href + "/" + id;
           string nodename = "Section" + id;
          // string type = ((outlinetype)((int)select.type + 1)).ToString();
           //XmlElement xe1 =(XmlElement)root_outline.SelectSingleNode(cc);
           ((XmlElement)a).SetAttribute("sec-title",newname);
           doc_outline.Save(outline_xml);
           
       }
       public outline outline_chapter_add(outline select, string newname)  //增加章节
       {
           doc_outline = new XmlDocument();
           doc_outline.Load(outline_xml);
           root_outline = doc_outline.DocumentElement;
           doc_contex = new XmlDocument();
           doc_contex.Load(idis_xml);
           root_contex = doc_contex.DocumentElement.SelectSingleNode("Papersection");
           string id = (int.Parse(select.secid) + 1).ToString();
           string href = id;
           string newname_add = id + " " + newname;
           string nodename = "Chapter";
           string type = "Chapter";
           outline newone = new outline()
           {
               Name1 = newname_add,
               toollip = newname_add,
               parent = null,
               secid = id,
               nodename = nodename,
               href = href,
               type = outlinetype.Chapter
           };
           
           XmlElement xe1 = doc_outline.CreateElement(nodename);//创建一个节点
           xe1.SetAttribute("id", id);//设置该节点id属性
           xe1.SetAttribute("sec-title", newname);//设置该节点name属性
           xe1.SetAttribute("nodetype", type);//设置该节点type属性
           xe1.SetAttribute("href", href);//设置节点的href
           XmlNode bb = getselectnode(select);
           root_outline.InsertAfter(xe1,bb);
           doc_outline.Save(outline_xml);
           XmlDocument doc = new XmlDocument();
           doc.Load(MainWindow.idd_href + "/webTemplateStyle.xml");
           XmlNode style = doc.DocumentElement.SelectSingleNode(type);
           XmlElement xe2 = doc_contex.CreateElement(nodename);
           xe2.SetAttribute("id",id);
           xe2.InnerXml = style.InnerXml;
           root_contex.AppendChild(xe2);
           doc_contex.Save(idis_xml);
           return newone;

       }
       private ObservableCollection<outline> mTreeViewItems1 = null;
       
   }
}

