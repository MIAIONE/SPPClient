// Decompiled with JetBrains decompiler
// Type: HwidGetCurrentEx.ComHelper
// Assembly: HwidGetCurrentEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 200C1AD7-2186-49E5-9EB2-5AB7013ECA80 Assembly location: D:\downloads\Programs\HwidGetCurrentEx.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace HWIDEx
{
    public static class ComHelper
    {
        public struct MyGuid
        {
            public int Data1;
            public short Data2;
            public short Data3;
            public byte[] Data4;

            public MyGuid(Guid g)
            {
                byte[] byteArray = g.ToByteArray();
                this.Data1 = BitConverter.ToInt32(byteArray, 0);
                this.Data2 = BitConverter.ToInt16(byteArray, 4);
                this.Data3 = BitConverter.ToInt16(byteArray, 6);
                this.Data4 = new byte[8];
                Buffer.BlockCopy((Array)byteArray, 8, (Array)this.Data4, 0, 8);
            }

            public Guid ToGuid() => new Guid(this.Data1, this.Data2, this.Data3, this.Data4);
        }

        [ComVisible(false)]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("eb89a21b-1f9c-4093-9a4d-05d4002543f6")]
        [ComImport]
        public interface IUnknown
        {
            int QueryInterface(IntPtr pUnk, ref Guid riid, out IntPtr pVoid);

            [MethodImpl(MethodImplOptions.PreserveSig)]
            int AddRef(IntPtr pUnk);

            [MethodImpl(MethodImplOptions.PreserveSig)]
            int Release(IntPtr pUnk);
        }

        [ClassInterface(ClassInterfaceType.None)]
        [Guid("eb89a21b-1f9c-4093-9a4d-05d4002543f6")]
        public class MyUnknown : ComHelper.IUnknown
        {
            public int QueryInterface(IntPtr pUnk, ref Guid riid, out IntPtr pVoid) => Marshal.QueryInterface(pUnk, ref riid, out pVoid);

            public int AddRef(IntPtr pUnk) => Marshal.AddRef(pUnk);

            public int Release(IntPtr pUnk) => Marshal.Release(pUnk);
        }

        [ComVisible(false)]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("dca42645-c410-4859-ab3c-9e9c563c57bb")]
        [ComImport]
        public interface ISppNamedParamsReadWrite
        {
            int QueryInterface(IntPtr pUnk, ref Guid riid, out IntPtr pVoid);
        }

        [ClassInterface(ClassInterfaceType.None)]
        [Guid("dca42645-c410-4859-ab3c-9e9c563c57bb")]
        public class MySppNamedParamsReadWrite : ComHelper.ISppNamedParamsReadWrite
        {
            public int QueryInterface(IntPtr pUnk, ref Guid riid, out IntPtr pVoid) => Marshal.QueryInterface(pUnk, ref riid, out pVoid);
        }

        [ComVisible(false)]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("22F58556-C467-43CD-98FF-7DBCADB2F661")]
        [ComImport]
        public interface ISppNamedParamsReadOnly
        {
            int QueryInterface(IntPtr pUnk, ref Guid riid, out IntPtr pVoid);
        }

        [ClassInterface(ClassInterfaceType.None)]
        [Guid("22F58556-C467-43CD-98FF-7DBCADB2F661")]
        public class MySppNamedParamsReadOnly : ComHelper.ISppNamedParamsReadOnly
        {
            public int QueryInterface(IntPtr pUnk, ref Guid riid, out IntPtr pVoid) => Marshal.QueryInterface(pUnk, ref riid, out pVoid);
        }

        [ComVisible(false)]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("96B97320-ED0E-4D9F-B390-6C17EAF67277")]
        [ComImport]
        public interface ISppParamsReadWrite
        {
            int QueryInterface(IntPtr pUnk, ref Guid riid, out IntPtr pVoid);
        }

        [ClassInterface(ClassInterfaceType.None)]
        [Guid("96B97320-ED0E-4D9F-B390-6C17EAF67277")]
        public class MySppParamsReadWrite : ComHelper.ISppParamsReadWrite
        {
            public int QueryInterface(IntPtr pUnk, ref Guid riid, out IntPtr pVoid) => Marshal.QueryInterface(pUnk, ref riid, out pVoid);
        }

        [ComVisible(false)]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("BE73DD34-4DAD-4AC5-BBE0-7930F45CED73")]
        [ComImport]
        public interface ISppParamsReadOnly
        {
            int QueryInterface(IntPtr pUnk, ref Guid riid, out IntPtr pVoid);
        }

        [ClassInterface(ClassInterfaceType.None)]
        [Guid("BE73DD34-4DAD-4AC5-BBE0-7930F45CED73")]
        public class MySppParamsReadOnly : ComHelper.ISppParamsReadOnly
        {
            public int QueryInterface(IntPtr pUnk, ref Guid riid, out IntPtr pVoid) => Marshal.QueryInterface(pUnk, ref riid, out pVoid);
        }
    }
}