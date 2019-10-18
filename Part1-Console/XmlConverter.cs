using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Part1_Console
{
    public class XmlConverter
    {
        public static string Serialize<T>(T data)
        {
            if (data == null)
                return string.Empty;

            var xmlserializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, data);
                return stringWriter.ToString();
            }
        }
    }
}
