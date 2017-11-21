using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using yxw.SQLServerBase;

namespace onlinepayBase.PinganBank.Messages
{
    public class ReceiveMessage
    {
        public void FromXml(string xml)
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

                if (!string.IsNullOrEmpty(id))
                    SetValue(this, id, value);
            }
        }

        /// <summary>
        /// 设置相应属性的值

        /// </summary>

        /// <param name="entity">实体</param>

        /// <param name="fieldName">属性名</param>

        /// <param name="fieldValue">属性值</param>

        public void SetValue(object entity, string fieldName, string fieldValue)

        {

            Type entityType = entity.GetType();



            PropertyInfo propertyInfo = entityType.GetProperty(fieldName);

            if (propertyInfo == null)
                return;


            if (IsType(propertyInfo.PropertyType, "System.String"))

            {

                propertyInfo.SetValue(entity, fieldValue, null);



            }



            if (IsType(propertyInfo.PropertyType, "System.Boolean"))

            {
                try
                {
                    propertyInfo.SetValue(entity, Boolean.Parse(fieldValue), null);
                }
                catch (Exception)
                {
                    
                }



            }



            if (IsType(propertyInfo.PropertyType, "System.Int32"))

            {

                if (fieldValue != "")
                    try
                    {
                    propertyInfo.SetValue(entity, int.Parse(fieldValue), null);

                    }
                    catch (Exception)
                    {
                    }

                else

                    propertyInfo.SetValue(entity, 0, null);



            }


            if (IsType(propertyInfo.PropertyType, "System.Float"))

            {

                if (fieldValue != "")
                    try
                    {
                    propertyInfo.SetValue(entity, Decimal.Parse(fieldValue), null);

                    }
                    catch (Exception)
                    {

                    }

                else

                    propertyInfo.SetValue(entity, new Decimal(0), null);



            }

            if (IsType(propertyInfo.PropertyType, "System.Decimal"))

            {

                if (fieldValue != "")
                    try
                    {
                    propertyInfo.SetValue(entity, Decimal.Parse(fieldValue), null);

                    }
                    catch (Exception)
                    {

                    }

                else

                    propertyInfo.SetValue(entity, new Decimal(0), null);



            }



            if (IsType(propertyInfo.PropertyType, "System.Nullable`1[System.DateTime]"))

            {

                if (fieldValue != "")

                {

                    try

                    {

                        propertyInfo.SetValue(

                            entity,

                            (DateTime?)DateTime.ParseExact(fieldValue, "yyyy-MM-dd HH:mm:ss", null), null);

                    }

                    catch

                    {
                        try
                        {
                        propertyInfo.SetValue(entity, (DateTime?)DateTime.ParseExact(fieldValue, "yyyy-MM-dd", null), null);

                        }
                        catch (Exception)
                        {

                        }

                    }

                }

                else

                    propertyInfo.SetValue(entity, null, null);



            }
        }

        /// <summary>

        /// 类型匹配

        /// </summary>

        /// <param name="type"></param>

        /// <param name="typeName"></param>

        /// <returns></returns>

        public bool IsType(Type type, string typeName)

        {

            if (type.ToString() == typeName)

                return true;

            if (type.ToString() == "System.Object")

                return false;



            return IsType(type.BaseType, typeName);

        }

        public void save(DBTableObject dbtable)
        {
            //讲属性值复制到dbtable
            Type t = this.GetType();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                string fieldname = pi.Name;
                string value = pi.GetValue(this, null).ToString();

                SetValue(dbtable, fieldname, value);
            }
            if (dbtable != null)
                dbtable.Insert();
        }
    }
}