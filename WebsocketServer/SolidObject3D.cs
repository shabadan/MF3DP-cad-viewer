using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Server
{
    public class SolidObject3D
    {
        public Dictionary<int, FaceObject3D> FaceList { get; set; }
        public UnmanagedOccShape ShapeClass { get; set; }

        public SolidObject3D(UnmanagedOccShape sh)
        {
            FaceList = new Dictionary<int, FaceObject3D>();
            ShapeClass = sh;
        }

        public bool AddFace(int nf)
        {
            FaceList.Add(nf, new FaceObject3D(nf,ShapeClass));
            return true;
        }
    }
}
