using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SM.Server
{
   


public class UnmanagedOccShape : IDisposable
{
    #region PInvokes
    //[DllImport("gmshInterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    [DllImport("InterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    static public extern IntPtr CreateOccShape();

     [DllImport("InterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    static public extern bool CreateOccShapeFromSTP(IntPtr pTestClassObject, string name);

    [DllImport("InterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    static public extern void DisposeOccShape(IntPtr pTestClassObject);
    
    [DllImport("InterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
            //[return: MarshalAs(UnmanagedType.U4)]
    //[return: MarshalAs(UnmanagedType.I1)]
    //[return: MarshalAs(UnmanagedType.Bool)]
    static public extern int CallIsCLosed(IntPtr pTestClassObject);
    
    [DllImport("InterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    static public extern int CallNumOfFaces(IntPtr pTestClassObject);
    
    [DllImport("InterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    static public extern int CallNumTriaInFace(IntPtr pTestClassObject, int nface);

    [DllImport("InterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    static public extern int CallNumNodesInFace(IntPtr pTestClassObject, int nface);

    [DllImport("InterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void CallSetTriaListForFace
    (
        IntPtr pTestClassObject,
        [In][Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[,] pArrayOfInt,
        //[In] int[,] pArrayOfInt,
        int nface    
    );

    [DllImport("InterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void CallSetNodeListForFace
    (
        IntPtr pTestClassObject,
        [In][Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[,] pArrayOfInt,
        //[In] double[,] pArrayOfInt,
        int nface
    );

    #endregion PInvokes



    #region Members
    private IntPtr m_pNativeObject; 
    // Variable to hold the C++ class's this pointer
    #endregion Members

    public UnmanagedOccShape()
    {
        // We have to Create an instance of this class through an exported 
        // function
        this.m_pNativeObject = CreateOccShape();
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool bDisposing)
    {
        if(this.m_pNativeObject != IntPtr.Zero)
        {
            // Call the DLL Export to dispose this class
            DisposeOccShape(this.m_pNativeObject);
            this.m_pNativeObject = IntPtr.Zero;
        }

        if(bDisposing)
        {
            // No need to call the finalizer since we've now cleaned
            // up the unmanaged memory
            GC.SuppressFinalize(this);
        }
    }

    // This finalizer is called when Garbage collection occurs, but only if
    // the IDisposable.Dispose method wasn't already called.
    ~UnmanagedOccShape()
    {
        Dispose(false);
    }



    #region Wrapper methods
   
    public int IsClosed()
    {
        return CallIsCLosed(this.m_pNativeObject);
    }
    #endregion Wrapper methods


    public bool FillSTEPOccShape(string filpath)
    {
        // We have to Create an instance of this class through an exported 
        // function
        //this.m_pNativeObject = CreateOccShape();
        return CreateOccShapeFromSTP(this.m_pNativeObject,filpath);
    }

    //
    public int NumOfFaces()
    {
        // We have to Create an instance of this class through an exported 
        // function
        //this.m_pNativeObject = CreateOccShape();
        return CallNumOfFaces(this.m_pNativeObject);
    }

    //
    public int NumTriaInFace(int nf)
    {
        return CallNumTriaInFace(this.m_pNativeObject,nf);
    }

    //
    public int NumNodesInFace(int nf)
    {
        return CallNumNodesInFace(this.m_pNativeObject, nf);
    }

    //
    public bool SetTriaListForFace(int nf,int[,] TriangleList)
    {
        CallSetTriaListForFace(this.m_pNativeObject, TriangleList, nf);
        return true;
    }

    public bool SetNodeListForFace(int nf, double[,] NodeList)
    {
        CallSetNodeListForFace(this.m_pNativeObject, NodeList, nf);
        return true;
    }


}




    }
