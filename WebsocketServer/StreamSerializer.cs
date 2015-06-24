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
    /// Serializes/Deserializes a CAD file (e.g. STP, IGES) to JSON fromat.
    /// </summary>
    /// 

    [DataContract]
    public class WebSocketStream
    {
        [DataMember(Name = "extension")]
        public string extension { get; set; }

        [DataMember(Name = "contents_b64")]
        public string contents_b64 { get; set; }

        [DataMember(Name = "instruction")]
        public string instruction { get; set; }

    }

    public static class WebSocketStreamSerializer
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

            return returnValue;

        }
        //






        public static string Serialize(this string jsonString, string instructionstring)
        {
            WebSocketStream dati = new WebSocketStream();

            if (instructionstring == "xml3dplot")
                dati.extension = "xml3d";
            else if (instructionstring == "plot_mesh")
                dati.extension = "msh";
            else
                dati.extension = "";
            
            dati.contents_b64 = EncodeTo64(jsonString);

            dati.instruction = instructionstring;


            return Serialize(dati);
        }

        public static WebSocketStream Deserialize(string json)
        {
            WebSocketStream CADObject = new WebSocketStream();
            CADObject = Deserialize<WebSocketStream>(json);
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
