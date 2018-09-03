using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Tira.Logic.Helpers
{
    /// <summary>
    /// Serialization helper
    /// </summary>
    internal class SerializationHelper
    {
        #region Public methods

        /// <summary>
        /// Deserializes from XML
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="xml">Xml</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T DeserializeFromXml<T>(string xml)
        {
            T local;
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
                    local = (T)serializer.ReadObject(reader);
            }
            catch (Exception exception)
            {
                string message = "Failed to deserialize xml string to data contract object";
                LogHelper.Logger.Error(exception, message);
                throw new Exception(message, exception);
            }
            return local;
        }

        /// <summary>
        /// Serializes to XML.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="obj">Object</param>
        /// <returns></returns>
        public static string SerializeToXml<T>(T obj)
        {
            string str;
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                StringBuilder output = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(output))
                {
                    serializer.WriteObject(writer, obj);
                    writer.Flush();
                    writer.Close();
                }
                str = output.ToString();
            }
            catch (Exception exception)
            {
                string message = "Failed to serialize data contract object to xml string";
                LogHelper.Logger.Error(exception, message);
                throw new Exception(message, exception);
            }
            return str;
        }

        #endregion
    }
}
