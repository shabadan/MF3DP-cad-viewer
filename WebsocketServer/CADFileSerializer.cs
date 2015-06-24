using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace SM.Server
{
    /// <summary>
    /// Serializes a CAD file (e.g. STP, IGES) to JSON fromat.
    /// </summary>
    /// 

    [DataContract]
    public class CADFile
    {
        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "type")]
        public string type { get; set; }

        [DataMember(Name = "size")]
        public string size { get; set; }

        [DataMember(Name = "contents_b64")]
        public string contents_b64 { get; set; }

    }

    public static class CADFileSerializer
    {
        
        public static string EncodeTo64(string toEncode)
        {

            byte[] toEncodeAsBytes

                  = System.Text.Encoding.UTF8.GetBytes(toEncode);

            string returnValue

                  = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;

        }


        public static string DecodeFrom64(string encodedData)
        {

            byte[] encodedDataAsBytes

                = System.Convert.FromBase64String(encodedData);

            string returnValue =

               System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
               //System.Text.Encoding.Unicode.GetString(encodedDataAsBytes);

            return returnValue;

        }


        //






        public static string Serialize(this string fileName)
        {
            //fileName può essere il percorso completo o quello relativo del file
            CADFile dati = new CADFile();
            FileInfo f = new FileInfo(fileName);
            
            dati.name = f.Name;
            dati.size = f.Length.ToString();
            dati.type = f.Extension;

            System.IO.StreamReader myFile = new System.IO.StreamReader(fileName);
            string myString = myFile.ReadToEnd();
            myFile.Close();
            dati.contents_b64 = EncodeTo64(myString);
           

            return Serialize(dati);
        }

        public static CADFile Deserialize(string json)
        { 
            CADFile CADObject = new CADFile();
            CADObject = Deserialize<CADFile>(json);
            return CADObject;
        }


        // Resource: http://pietschsoft.com/post/2008/02/NET-35-JSON-Serialization-using-the-DataContractJsonSerializer.aspx.
        private static string Serialize(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            string retVal = Encoding.Default.GetString(ms.ToArray());
            ms.Dispose();

            return retVal;
        }


        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            ms.Dispose();
            return obj;
        }





       




    }
}
