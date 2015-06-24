using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SM.Server
{
    public class FaceObject3D
    {
        public int FaceIndex { get; set; }
        public int[,] TriangleList { get; set; } //int[]=[node1 node2 node3]
        public double[,] NodeList { get; set; } //double[]=[x y z]

        private int NumOfTriaInFace { get; set; }
        private int NumOfNodesInFace { get; set; }

        public FaceObject3D(int nf, UnmanagedOccShape ShapeContainer)
        {
            FaceIndex=nf;
            NumOfTriaInFace = ShapeContainer.NumTriaInFace(nf);
            Console.WriteLine("\nNum of triangle = {0}\n", NumOfTriaInFace);
            NumOfNodesInFace = ShapeContainer.NumNodesInFace(nf);
            Console.WriteLine("\nNum of nodes = {0}\n", NumOfNodesInFace);

            //Fill triangles list
            TriangleList = new int[NumOfTriaInFace, 3];
            //debug
            /*
            int k = 0;
            for (int i = 0; i < NumOfTriaInFace; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    TriangleList[i, j] = k++;
                }
            }
            Console.WriteLine("\nRow 1 is = {0}, {1}, {2}\n", TriangleList[0, 0], TriangleList[0, 1], TriangleList[0, 2]);
            //
             */


            // Note that the way a managed array arranges its data is very similar to the way a C/C++ program arranges
            // array data : sequentially.
            //
            // To demonstrate this, pin the array data in memory...
            GCHandle gch = GCHandle.Alloc(TriangleList, GCHandleType.Pinned);
            // Get a pointer to the array data...
            IntPtr pArrayData = gch.AddrOfPinnedObject();
            // Via the debugger's memory viewer, confirm
            // that the data is laid in memory in sequence.
            ShapeContainer.SetTriaListForFace(FaceIndex,TriangleList);
            // free resource
            pArrayData = IntPtr.Zero; // va fatto?
            gch.Free();



            //Fill nodes list
            NodeList = new double[NumOfNodesInFace, 3];
           
            // Note that the way a managed array arranges its data is very similar to the way a C/C++ program arranges
            // array data : sequentially.
            //
            // To demonstrate this, pin the array data in memory...
            gch = GCHandle.Alloc(NodeList, GCHandleType.Pinned);
            // Get a pointer to the array data...
            pArrayData = gch.AddrOfPinnedObject();
            // Via the debugger's memory viewer, confirm
            // that the data is laid in memory in sequence.
            ShapeContainer.SetNodeListForFace(FaceIndex, NodeList);
            // free resource
            pArrayData = IntPtr.Zero; // va fatto?
            gch.Free();
            //Console.WriteLine("\nRow 1 is = {0}, {1}, {2}\n", NodeList[0, 0], NodeList[0, 1], NodeList[0, 2]);
        }
    }
}
