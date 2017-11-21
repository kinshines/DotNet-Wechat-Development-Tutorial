using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace onlinepayBase.PinganBank
{
    public class MessageParser
    {
        public void parse(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xml);
            //查找根节点
            XmlNode root = xmlDoc.SelectSingleNode("kColl");
            //获取到所有<users>的子节点
            XmlNodeList nodeList = root.ChildNodes;
            //遍历所有子节点
            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;
                if (xn.Name.ToLower() != "field") // 字段都是field
                    continue;
                string id = "", value = "";

                foreach (XmlAttribute attr in xn.Attributes)
                {
                    if (attr.Name.ToLower() == "id")
                        id = attr.Value;
                    if (attr.Name.ToLower() == "value")
                        value = attr.Value;
                }


            }

        }
    }
}
