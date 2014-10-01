using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Phytel.Services.Serializer
{
    public class SerializerXml : ISerializer
    {
        public object Deserialize(string value, Type targetType)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(targetType);
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(value));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return xs.Deserialize(memoryStream);
        }

        public T Deserialize<T>(string value)
        {
            return (T)Deserialize(value, typeof(T));
        }

        public string Serialize(object value, Type type)
        {
            String xmlString = null;
            MemoryStream memoryStream = new MemoryStream();
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(value.GetType());

            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            xs.Serialize(xmlTextWriter, value);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            xmlString = UTF8ByteArrayToString(memoryStream.ToArray());

            return xmlString;
        }

        public string Serialize<T>(T value)
        {
            return Serialize(value, typeof(T));
        }

        private byte[] StringToUTF8ByteArray(string xmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(xmlString);
            return byteArray;
        }

        private string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }
    }
}