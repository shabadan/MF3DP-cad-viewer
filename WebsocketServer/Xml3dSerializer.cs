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
    /// Serializes a xml3D mesh to JSON format.
    /// </summary>
    public static class Xml3DSerializer
    {

        [DataContract]
        class JSONBrepObject
        {
            [DataMember(Name = "ObjectMeshes")]
            public List<JSONsinglemesh> MList { get; set; }

        }

        [DataContract]
        class JSONsinglemesh
        {
            [DataMember(Name = "Vertices")]
            public double[] NodeArray { get; set; }

            [DataMember(Name = "Triangles")]
            public int[] TriaArray { get; set; }
        }

       

      

        public static string Serialize(this Dictionary<int, FaceObject3D> listafacce)
        {
            JSONBrepObject dati = new JSONBrepObject { MList = new List<JSONsinglemesh>() };

            foreach (var mesh in listafacce)
            {
                int nnodi = mesh.Value.NodeList.GetLength(0);
                int ntria = mesh.Value.TriangleList.GetLength(0);

                JSONsinglemesh jsonmesh = new JSONsinglemesh
                {
                    NodeArray = new double[3 * nnodi],
                    TriaArray = new int[3 * ntria]
                };

                
                for (int nn = 0; nn < nnodi; nn++)
                {
                    jsonmesh.NodeArray[3 * nn] = mesh.Value.NodeList[nn, 0];
                    jsonmesh.NodeArray[3 * nn + 1] = mesh.Value.NodeList[nn, 1];
                    jsonmesh.NodeArray[3 * nn + 2] = mesh.Value.NodeList[nn, 2];

                }

                
                for (int nf = 0; nf < ntria; nf++)
                {
                    jsonmesh.TriaArray[3 * nf] = mesh.Value.TriangleList[nf, 0];
                    jsonmesh.TriaArray[3 * nf + 1] = mesh.Value.TriangleList[nf, 1];
                    jsonmesh.TriaArray[3 * nf + 2] = mesh.Value.TriangleList[nf, 2];
                }

                dati.MList.Add(jsonmesh);
            }

            return Serialize(dati);
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
