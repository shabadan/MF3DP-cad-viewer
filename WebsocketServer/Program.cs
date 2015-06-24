using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fleck;
using System.IO;

using System.Web.Script.Serialization;
using System.Diagnostics;




namespace SM.Server
{
    class Program
    {
       
        
        static List<IWebSocketConnection> _sockets;
        static bool _initialized = false;
        
        

        static void Main(string[] args)
        {
            InitializeSockets();
            _initialized = true;
        }


       
        

        private static void InitializeSockets()
        {
            UnmanagedOccShape testClass = new UnmanagedOccShape();
            
            _sockets = new List<IWebSocketConnection>();

            var server = new WebSocketServer("ws://localhost:8181");
            

            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Connected to " + socket.ConnectionInfo.ClientIpAddress);
                    _sockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Disconnected from " + socket.ConnectionInfo.ClientIpAddress);
                    _sockets.Remove(socket);
                };

                
                socket.OnBinary = message =>
                {
                };
                


                socket.OnMessage = message =>
                {
                    int l = Math.Min(message.Length, 50);
                    string first = message.Substring(0, l);
                    char[] delimiters = new char[] { '\"', ':' , ',', '{'};
	                string[] parts = first.Split(delimiters,StringSplitOptions.RemoveEmptyEntries);
                    //Console.WriteLine("Message {0} = {1}", parts[0],parts[1]);
                    string instruction = parts[1];
                    if (instruction == "KeepAlive")
                    { }
                    else if (instruction == "cad2xml3d")
                    {
                        // Create new stopwatch
                        Stopwatch stopwatch = new Stopwatch();
                        
                        // Begin timing
                        stopwatch.Start();                     
                        CADFile CADObject = new CADFile();
                        CADObject = CADFileSerializer.Deserialize(message);
                        stopwatch.Stop();
                        Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);


                        Console.WriteLine("type: {0}, name: {1}", CADObject.type, CADObject.name);
                        string contenuto = CADObject.contents_b64;
                        string modelfile = CADFileSerializer.DecodeFrom64(contenuto);
                        Console.WriteLine("File received! Saving...");
                        string mypath = ".\\temp" + CADObject.name;
                        File.WriteAllText(@mypath, modelfile);


                        Console.WriteLine("File written on disk");
                        try
                        {
                            //UnmanagedOccShape testClass = new UnmanagedOccShape();
                            bool atto = testClass.FillSTEPOccShape(mypath);
                            if (atto)
                                Console.WriteLine("\nSTEP loaded successfully\n");
                            else
                                Console.WriteLine("\nSTEP not loaded\n");

                            int check = testClass.IsClosed();
                            if (check == 1)
                                Console.WriteLine("\nStep shell is closed!");
                            else
                                Console.WriteLine("\nStep shell is not closed!");

                            int NumOfFac = testClass.NumOfFaces();
                            Console.WriteLine("\nNumber of faces {0}\n", NumOfFac);

                            SolidObject3D Solid1 = new SolidObject3D(testClass);
                            bool isadded;
                            for (int nfc = 1; nfc <= NumOfFac; nfc++)
                            {
                                isadded = Solid1.AddFace(nfc);
                                Console.WriteLine("\nFace {0} of {1} added\n", nfc, NumOfFac);
                            }

                            //string json0 = Solid3DSerializer.Serialize(Solid1.FaceList);
                            string json0 = Xml3DSerializer.Serialize(Solid1.FaceList);
                            string json = WebSocketStreamSerializer.Serialize(json0, "xml3dplot");

                            foreach (var sockettemp in _sockets)
                            {
                                sockettemp.Send(json);
                            }
                            Console.WriteLine("Mesh sent to the client");




                            //testClass.Dispose();aaa

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Cannot open txt file for writing");
                            Console.WriteLine("Cannot {0}", e.Message);
                            return;
                        }



                    }


                };
            });

            

            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
        }

       

 

      
    }
}

