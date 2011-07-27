using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace CA.SharePoint.Common
{
   public static class SerializeUtil
   {

       public static string Seralize(object obj)
       {
           System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(obj.GetType());

           MemoryStream s = new MemoryStream();
           XmlTextWriter w = new XmlTextWriter(s, Encoding.UTF8);

           //xs.Serialize(w, o, ns);

           xs.Serialize(w, obj);

           string xmlData = Encoding.UTF8.GetString(s.ToArray()).Trim();

           return xmlData;
       }

       public static object Deserialize(Type t, string xml)
       {
           System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer( t );

           System.IO.StringReader sw = new System.IO.StringReader(xml);

           return xs.Deserialize(sw) ;
       }
    }
}
