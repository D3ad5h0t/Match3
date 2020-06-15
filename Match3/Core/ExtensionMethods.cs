using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Match3.Core
{
    public static class ExtensionMethods
    {
        public static T DeepCopy<T>(this T self)
        {
            using (var stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, self);
                stream.Seek(0, SeekOrigin.Begin);

                return (T) formatter.Deserialize(stream);
            }
        }

        public static T DeepCopyXml<T>(this T self)
        {
            using (var ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(ms, self);
                ms.Position = 0;

                return (T) serializer.Deserialize(ms);
            }
        }
    }
}
