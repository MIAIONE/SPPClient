// Decompiled with JetBrains decompiler
// Type: HwidGetCurrentEx.CPUID
// Assembly: HwidGetCurrentEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 200C1AD7-2186-49E5-9EB2-5AB7013ECA80 Assembly location: D:\downloads\Programs\HwidGetCurrentEx.dll

using System;
using System.Runtime.InteropServices;

namespace HWIDEx
{
    public static class CPUID
    {
        private static readonly byte[] x86CodeBytes = new byte[30]
        {
      (byte) 85,
      (byte) 139,
      (byte) 236,
      (byte) 83,
      (byte) 87,
      (byte) 139,
      (byte) 69,
      (byte) 8,
      (byte) 15,
      (byte) 162,
      (byte) 139,
      (byte) 125,
      (byte) 12,
      (byte) 137,
      (byte) 7,
      (byte) 137,
      (byte) 95,
      (byte) 4,
      (byte) 137,
      (byte) 79,
      (byte) 8,
      (byte) 137,
      (byte) 87,
      (byte) 12,
      (byte) 95,
      (byte) 91,
      (byte) 139,
      (byte) 229,
      (byte) 93,
      (byte) 195
        };

        private static readonly byte[] x64CodeBytes = new byte[26]
        {
      (byte) 83,
      (byte) 73,
      (byte) 137,
      (byte) 208,
      (byte) 137,
      (byte) 200,
      (byte) 15,
      (byte) 162,
      (byte) 65,
      (byte) 137,
      (byte) 64,
      (byte) 0,
      (byte) 65,
      (byte) 137,
      (byte) 88,
      (byte) 4,
      (byte) 65,
      (byte) 137,
      (byte) 72,
      (byte) 8,
      (byte) 65,
      (byte) 137,
      (byte) 80,
      (byte) 12,
      (byte) 91,
      (byte) 195
        };

        public static byte[] Invoke(int level)
        {
            IntPtr num = IntPtr.Zero;
            try
            {
                byte[] source = IntPtr.Size != 4 ? CPUID.x64CodeBytes : CPUID.x86CodeBytes;
                num = CPUID.VirtualAlloc(IntPtr.Zero, new UIntPtr((uint)source.Length), CPUID.AllocationType.COMMIT | CPUID.AllocationType.RESERVE, CPUID.MemoryProtection.EXECUTE_READWRITE);
                Marshal.Copy(source, 0, num, source.Length);
                CPUID.CpuIDDelegate forFunctionPointer = (CPUID.CpuIDDelegate)Marshal.GetDelegateForFunctionPointer(num, typeof(CPUID.CpuIDDelegate));
                GCHandle gcHandle = new GCHandle();
                byte[] buffer = new byte[16];
                try
                {
                    gcHandle = GCHandle.Alloc((object)buffer, GCHandleType.Pinned);
                    forFunctionPointer(level, buffer);
                }
                finally
                {
                    if (gcHandle != new GCHandle())
                        gcHandle.Free();
                }
                return buffer;
            }
            finally
            {
                if (num != IntPtr.Zero)
                {
                    CPUID.VirtualFree(num, 0U, 32768U);
                    IntPtr zero = IntPtr.Zero;
                }
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAlloc(
          IntPtr lpAddress,
          UIntPtr dwSize,
          CPUID.AllocationType flAllocationType,
          CPUID.MemoryProtection flProtect);

        [DllImport("kernel32")]
        private static extern bool VirtualFree(IntPtr lpAddress, uint dwSize, uint dwFreeType);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CpuIDDelegate(int level, byte[] buffer);

        [Flags]
        private enum AllocationType : uint
        {
            COMMIT = 4096, // 0x00001000
            RESERVE = 8192, // 0x00002000
            RESET = 524288, // 0x00080000
            LARGE_PAGES = 536870912, // 0x20000000
            PHYSICAL = 4194304, // 0x00400000
            TOP_DOWN = 1048576, // 0x00100000
            WRITE_WATCH = 2097152, // 0x00200000
        }

        [Flags]
        private enum MemoryProtection : uint
        {
            EXECUTE = 16, // 0x00000010
            EXECUTE_READ = 32, // 0x00000020
            EXECUTE_READWRITE = 64, // 0x00000040
            EXECUTE_WRITECOPY = 128, // 0x00000080
            NOACCESS = 1,
            READONLY = 2,
            READWRITE = 4,
            WRITECOPY = 8,
            GUARD_Modifierflag = 256, // 0x00000100
            NOCACHE_Modifierflag = 512, // 0x00000200
            WRITECOMBINE_Modifierflag = 1024, // 0x00000400
        }
    }
}