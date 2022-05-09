// Decompiled with JetBrains decompiler
// Type: VRSAVaultSignPKCS.DynamicDllLoader
// Assembly: VRSAVaultSignPKCS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835E1F41-447B-4B59-919B-3F453537ACCB
// Assembly location: D:\downloads\Programs\VRSAVaultSignPKCS-cleaned.dll

using System;
using System.Runtime.InteropServices;

namespace HWIDEx
{
  public class DynamicDllLoader
  {
    public IntPtr pCode = IntPtr.Zero;
    private static readonly int[][][] ProtectionFlags = new int[2][][];
    private DynamicDllLoader.MEMORYMODULE module;

    internal unsafe bool LoadLibrary(byte[] data)
    {
      DynamicDllLoader.IMAGE_DOS_HEADER dosHeader = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_DOS_HEADER>(data);
      DynamicDllLoader.IMAGE_NT_HEADERS oldHeaders = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_NT_HEADERS>(data, (uint) dosHeader.e_lfanew);
      IntPtr lpStartAddr = (IntPtr) (long) DynamicDllLoader.Win32Imports.VirtualAlloc(oldHeaders.OptionalHeader.ImageBase, oldHeaders.OptionalHeader.SizeOfImage, DynamicDllLoader.Win32Constants.MEM_RESERVE, DynamicDllLoader.Win32Constants.PAGE_READWRITE);
      if (lpStartAddr.ToInt32() == 0)
        lpStartAddr = (IntPtr) (long) DynamicDllLoader.Win32Imports.VirtualAlloc((uint) (int) lpStartAddr, oldHeaders.OptionalHeader.SizeOfImage, DynamicDllLoader.Win32Constants.MEM_RESERVE, DynamicDllLoader.Win32Constants.PAGE_READWRITE);
      this.module = new DynamicDllLoader.MEMORYMODULE()
      {
        codeBase = lpStartAddr,
        numModules = 0,
        modules = new IntPtr(0),
        initialized = 0
      };
      int num1 = (int) DynamicDllLoader.Win32Imports.VirtualAlloc((uint) (int) lpStartAddr, oldHeaders.OptionalHeader.SizeOfImage, DynamicDllLoader.Win32Constants.MEM_COMMIT, DynamicDllLoader.Win32Constants.PAGE_READWRITE);
      IntPtr num2 = (IntPtr) (long) DynamicDllLoader.Win32Imports.VirtualAlloc((uint) (int) lpStartAddr, oldHeaders.OptionalHeader.SizeOfHeaders, DynamicDllLoader.Win32Constants.MEM_COMMIT, DynamicDllLoader.Win32Constants.PAGE_READWRITE);
      Marshal.Copy(data, 0, num2, (int) ((long) dosHeader.e_lfanew + (long) oldHeaders.OptionalHeader.SizeOfHeaders));
      this.module.headers = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_NT_HEADERS>(num2, (uint) dosHeader.e_lfanew);
      this.module.headers.OptionalHeader.ImageBase = (uint) (int) lpStartAddr;
      this.CopySections(data, oldHeaders, num2, dosHeader);
      uint delta = (uint) (int) (lpStartAddr - (int) oldHeaders.OptionalHeader.ImageBase);
      if (delta > 0U)
        this.PerformBaseRelocation(delta);
      this.BuildImportTable();
      this.FinalizeSections(num2, dosHeader, oldHeaders);
      bool flag1;
      bool flag2;
      try
      {
        flag1 = ((DynamicDllLoader.fnDllEntry) Marshal.GetDelegateForFunctionPointer(new IntPtr(this.module.codeBase.ToInt32() + (int) this.module.headers.OptionalHeader.AddressOfEntryPoint), typeof (DynamicDllLoader.fnDllEntry)))(lpStartAddr.ToInt32(), 1U, (void*) null);
        this.pCode = this.module.codeBase;
      }
      catch
      {
        flag2 = false;
        goto label_8;
      }
      flag2 = flag1;
label_8:
      return flag2;
    }

    public int GetModuleCount()
    {
      int moduleCount = 0;
      IntPtr codeBase = this.module.codeBase;
      DynamicDllLoader.IMAGE_DATA_DIRECTORY imageDataDirectory = this.module.headers.OptionalHeader.DataDirectory[1];
      if (imageDataDirectory.Size > 0U)
      {
        for (DynamicDllLoader.IMAGE_IMPORT_DESCRIPTOR importDescriptor = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_IMPORT_DESCRIPTOR>(codeBase, imageDataDirectory.VirtualAddress); importDescriptor.Name > 0U && DynamicDllLoader.Win32Imports.LoadLibrary(Marshal.PtrToStringAnsi(codeBase + (int) importDescriptor.Name)) != -1; importDescriptor = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_IMPORT_DESCRIPTOR>(codeBase, (uint) ((ulong) imageDataDirectory.VirtualAddress + (ulong) (Marshal.SizeOf(typeof (DynamicDllLoader.IMAGE_IMPORT_DESCRIPTOR)) * moduleCount))))
          ++moduleCount;
      }
      return moduleCount;
    }

    public unsafe int BuildImportTable()
    {
      this.module.modules = Marshal.AllocHGlobal(this.GetModuleCount() * 4);
      int num1 = 0;
      int num2 = 1;
      IntPtr codeBase = this.module.codeBase;
      DynamicDllLoader.IMAGE_DATA_DIRECTORY imageDataDirectory = this.module.headers.OptionalHeader.DataDirectory[1];
      if (imageDataDirectory.Size > 0U)
      {
        for (DynamicDllLoader.IMAGE_IMPORT_DESCRIPTOR importDescriptor = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_IMPORT_DESCRIPTOR>(codeBase, imageDataDirectory.VirtualAddress); importDescriptor.Name > 0U; importDescriptor = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_IMPORT_DESCRIPTOR>(codeBase, imageDataDirectory.VirtualAddress + (uint) (Marshal.SizeOf(typeof (DynamicDllLoader.IMAGE_IMPORT_DESCRIPTOR)) * num1)))
        {
          int num3 = DynamicDllLoader.Win32Imports.LoadLibrary(Marshal.PtrToStringAnsi(codeBase + (int) importDescriptor.Name));
          if (num3 != -1)
          {
            uint* numPtr1;
            uint* numPtr2;
            if (importDescriptor.CharacteristicsOrOriginalFirstThunk > 0U)
            {
              numPtr1 = (uint*) (void*) (codeBase + (int) importDescriptor.CharacteristicsOrOriginalFirstThunk);
              numPtr2 = (uint*) (void*) (codeBase + (int) importDescriptor.FirstThunk);
            }
            else
            {
              numPtr1 = (uint*) (void*) (codeBase + (int) importDescriptor.FirstThunk);
              numPtr2 = (uint*) (void*) (codeBase + (int) importDescriptor.FirstThunk);
            }
            while (*numPtr1 > 0U)
            {
              if ((*numPtr1 & 2147483648U) > 0U)
              {
                *numPtr2 = (uint) (int) DynamicDllLoader.Win32Imports.GetProcAddress(new IntPtr(num3), new IntPtr((long) (*numPtr1 & (uint) ushort.MaxValue)));
              }
              else
              {
                string stringAnsi = Marshal.PtrToStringAnsi(codeBase + (int) *numPtr1 + 2);
                *numPtr2 = DynamicDllLoader.Win32Imports.GetProcAddress(new IntPtr(num3), stringAnsi);
              }
              if (*numPtr2 != 0U)
              {
                ++numPtr1;
                ++numPtr2;
              }
              else
              {
                num2 = 0;
                break;
              }
            }
            ++num1;
          }
          else
          {
            num2 = 0;
            break;
          }
        }
      }
      return num2;
    }

    public void FinalizeSections(
      IntPtr headers,
      DynamicDllLoader.IMAGE_DOS_HEADER dosHeader,
      DynamicDllLoader.IMAGE_NT_HEADERS oldHeaders)
    {
      DynamicDllLoader.ProtectionFlags[0] = new int[2][];
      DynamicDllLoader.ProtectionFlags[1] = new int[2][];
      DynamicDllLoader.ProtectionFlags[0][0] = new int[2];
      DynamicDllLoader.ProtectionFlags[0][1] = new int[2];
      DynamicDllLoader.ProtectionFlags[1][0] = new int[2];
      DynamicDllLoader.ProtectionFlags[1][1] = new int[2];
      DynamicDllLoader.ProtectionFlags[0][0][0] = 1;
      DynamicDllLoader.ProtectionFlags[0][0][1] = 8;
      DynamicDllLoader.ProtectionFlags[0][1][0] = 2;
      DynamicDllLoader.ProtectionFlags[0][1][1] = 4;
      DynamicDllLoader.ProtectionFlags[1][0][0] = 16;
      DynamicDllLoader.ProtectionFlags[1][0][1] = 128;
      DynamicDllLoader.ProtectionFlags[1][1][0] = 32;
      DynamicDllLoader.ProtectionFlags[1][1][1] = 64;
      DynamicDllLoader.IMAGE_SECTION_HEADER imageSectionHeader = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_SECTION_HEADER>(headers, (uint) (24 + dosHeader.e_lfanew) + (uint) oldHeaders.FileHeader.SizeOfOptionalHeader);
      for (int index1 = 0; index1 < (int) this.module.headers.FileHeader.NumberOfSections; ++index1)
      {
        int index2 = ((int) imageSectionHeader.Characteristics & 536870912) != 0 ? 1 : 0;
        int index3 = ((int) imageSectionHeader.Characteristics & 1073741824) != 0 ? 1 : 0;
        int index4 = ((int) imageSectionHeader.Characteristics & int.MinValue) != 0 ? 1 : 0;
        if ((imageSectionHeader.Characteristics & 33554432U) > 0U)
        {
          DynamicDllLoader.Win32Imports.VirtualFree(new IntPtr((long) imageSectionHeader.PhysicalAddress), (UIntPtr) imageSectionHeader.SizeOfRawData, 16384U);
        }
        else
        {
          uint flNewProtect = (uint) DynamicDllLoader.ProtectionFlags[index2][index3][index4];
          if ((imageSectionHeader.Characteristics & 67108864U) > 0U)
            flNewProtect |= 512U;
          int num = (int) imageSectionHeader.SizeOfRawData;
          if (num == 0)
          {
            if ((imageSectionHeader.Characteristics & 64U) > 0U)
              num = (int) this.module.headers.OptionalHeader.SizeOfInitializedData;
            else if ((imageSectionHeader.Characteristics & 128U) > 0U)
              num = (int) this.module.headers.OptionalHeader.SizeOfUninitializedData;
          }
                    if (num <= 0 || !DynamicDllLoader.Win32Imports.VirtualProtect(new IntPtr((long)imageSectionHeader.PhysicalAddress), imageSectionHeader.SizeOfRawData, flNewProtect, out uint _))
                    { }
          imageSectionHeader = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_SECTION_HEADER>(headers, (uint) (24 + dosHeader.e_lfanew + (int) oldHeaders.FileHeader.SizeOfOptionalHeader + Marshal.SizeOf(typeof (DynamicDllLoader.IMAGE_SECTION_HEADER)) * (index1 + 1)));
        }
      }
    }

    public unsafe void PerformBaseRelocation(uint delta)
    {
      IntPtr codeBase = this.module.codeBase;
      int num1 = Marshal.SizeOf(typeof (DynamicDllLoader.IMAGE_BASE_RELOCATION));
      DynamicDllLoader.IMAGE_DATA_DIRECTORY imageDataDirectory = this.module.headers.OptionalHeader.DataDirectory[5];
      int num2 = 0;
      if (imageDataDirectory.Size <= 0U)
        return;
      for (DynamicDllLoader.IMAGE_BASE_RELOCATION imageBaseRelocation = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_BASE_RELOCATION>(codeBase, imageDataDirectory.VirtualAddress); imageBaseRelocation.VirtualAddress > 0U; imageBaseRelocation = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_BASE_RELOCATION>(codeBase, (uint) ((ulong) imageDataDirectory.VirtualAddress + (ulong) num2)))
      {
        IntPtr num3 = (IntPtr) (codeBase.ToInt32() + (int) imageBaseRelocation.VirtualAddress);
        ushort* numPtr1 = (ushort*) (codeBase.ToInt32() + (int) imageDataDirectory.VirtualAddress + num1);
        uint num4 = 0;
        while ((long) num4 < ((long) imageBaseRelocation.SizeOfBlock - (long) Marshal.SizeOf(typeof (DynamicDllLoader.IMAGE_BASE_RELOCATION))) / 2L)
        {
          int num5 = (int) *numPtr1 >> 12;
          int num6 = (int) *numPtr1 & 4095;
          switch (num5)
          {
            case 3:
              uint* numPtr2 = (uint*) (void*) (num3 + num6);
              int num7 = (int) *numPtr2 + (int) delta;
              *numPtr2 = (uint) num7;
              break;
          }
          ++num4;
          ++numPtr1;
        }
        num2 += (int) imageBaseRelocation.SizeOfBlock;
      }
    }

    public unsafe uint GetProcAddress(string name)
    {
      IntPtr codeBase = this.module.codeBase;
      int num1 = -1;
      DynamicDllLoader.IMAGE_DATA_DIRECTORY imageDataDirectory = this.module.headers.OptionalHeader.DataDirectory[0];
      uint procAddress;
      if (imageDataDirectory.Size == 0U)
      {
        procAddress = 0U;
      }
      else
      {
        DynamicDllLoader.IMAGE_EXPORT_DIRECTORY imageExportDirectory = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_EXPORT_DIRECTORY>(codeBase, imageDataDirectory.VirtualAddress);
        uint* numPtr1 = (uint*) (void*) new IntPtr((long) codeBase.ToInt32() + (long) imageExportDirectory.AddressOfNames);
        ushort* numPtr2 = (ushort*) (void*) new IntPtr((long) codeBase.ToInt32() + (long) imageExportDirectory.AddressOfNameOrdinals);
        uint num2 = 0;
        while (num2 < imageExportDirectory.NumberOfNames)
        {
          if (!(Marshal.PtrToStringAnsi(codeBase + (int) *numPtr1) == name))
          {
            ++num2;
            ++numPtr1;
            ++numPtr2;
          }
          else
          {
            num1 = (int) *numPtr2;
            break;
          }
        }
        uint* numPtr3 = (uint*) ((ulong) codeBase.ToInt32() + ((ulong) imageExportDirectory.AddressOfFunctions + (ulong) (num1 * 4)));
        procAddress = (uint) ((ulong) codeBase.ToInt32() + (ulong) *numPtr3);
      }
      return procAddress;
    }

    public void CopySections(
      byte[] data,
      DynamicDllLoader.IMAGE_NT_HEADERS oldHeaders,
      IntPtr headers,
      DynamicDllLoader.IMAGE_DOS_HEADER dosHeader)
    {
      IntPtr codeBase = this.module.codeBase;
      DynamicDllLoader.IMAGE_SECTION_HEADER imageSectionHeader = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_SECTION_HEADER>(headers, (uint) (24 + dosHeader.e_lfanew) + (uint) oldHeaders.FileHeader.SizeOfOptionalHeader);
      IntPtr num;
      for (int index = 0; index < (int) this.module.headers.FileHeader.NumberOfSections; ++index)
      {
        if (imageSectionHeader.SizeOfRawData == 0U)
        {
          uint sectionAlignment = oldHeaders.OptionalHeader.SectionAlignment;
          if (sectionAlignment > 0U)
          {
            num = new IntPtr((long) DynamicDllLoader.Win32Imports.VirtualAlloc((uint) (int) (codeBase + (int) imageSectionHeader.VirtualAddress), sectionAlignment, DynamicDllLoader.Win32Constants.MEM_COMMIT, DynamicDllLoader.Win32Constants.PAGE_READWRITE));
            imageSectionHeader.PhysicalAddress = (uint) (int) num;
            Marshal.WriteInt32(new IntPtr(headers.ToInt32() + (32 + dosHeader.e_lfanew + (int) oldHeaders.FileHeader.SizeOfOptionalHeader) + Marshal.SizeOf(typeof (DynamicDllLoader.IMAGE_SECTION_HEADER)) * index), (int) num);
            Marshal.Copy(new byte[(int) sectionAlignment + 1], 0, num, (int) sectionAlignment);
          }
          imageSectionHeader = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_SECTION_HEADER>(headers, (uint) (24 + dosHeader.e_lfanew + (int) oldHeaders.FileHeader.SizeOfOptionalHeader + Marshal.SizeOf(typeof (DynamicDllLoader.IMAGE_SECTION_HEADER)) * (index + 1)));
        }
        else
        {
          num = new IntPtr((long) DynamicDllLoader.Win32Imports.VirtualAlloc((uint) (int) (codeBase + (int) imageSectionHeader.VirtualAddress), imageSectionHeader.SizeOfRawData, DynamicDllLoader.Win32Constants.MEM_COMMIT, DynamicDllLoader.Win32Constants.PAGE_READWRITE));
          Marshal.Copy(data, (int) imageSectionHeader.PointerToRawData, num, (int) imageSectionHeader.SizeOfRawData);
          imageSectionHeader.PhysicalAddress = (uint) (int) num;
          Marshal.WriteInt32(new IntPtr(headers.ToInt32() + (32 + dosHeader.e_lfanew + (int) oldHeaders.FileHeader.SizeOfOptionalHeader) + Marshal.SizeOf(typeof (DynamicDllLoader.IMAGE_SECTION_HEADER)) * index), (int) num);
          imageSectionHeader = DynamicDllLoader.PointerHelpers.ToStruct<DynamicDllLoader.IMAGE_SECTION_HEADER>(headers, (uint) (24 + dosHeader.e_lfanew + (int) oldHeaders.FileHeader.SizeOfOptionalHeader + Marshal.SizeOf(typeof (DynamicDllLoader.IMAGE_SECTION_HEADER)) * (index + 1)));
        }
      }
    }

    public struct IMAGE_EXPORT_DIRECTORY
    {
      public uint Characteristics;
      public uint TimeDateStamp;
      public ushort MajorVersion;
      public ushort MinorVersion;
      public uint Name;
      public uint Base;
      public uint NumberOfFunctions;
      public uint NumberOfNames;
      public uint AddressOfFunctions;
      public uint AddressOfNames;
      public uint AddressOfNameOrdinals;
    }

    public struct IMAGE_IMPORT_BY_NAME
    {
      public short Hint;
      public byte Name;
    }

    public struct MEMORYMODULE
    {
      public DynamicDllLoader.IMAGE_NT_HEADERS headers;
      public IntPtr codeBase;
      public IntPtr modules;
      public int numModules;
      public int initialized;
    }

    public struct IMAGE_BASE_RELOCATION
    {
      public uint VirtualAddress;
      public uint SizeOfBlock;
    }

    public struct IMAGE_IMPORT_DESCRIPTOR
    {
      public uint CharacteristicsOrOriginalFirstThunk;
      public uint TimeDateStamp;
      public uint ForwarderChain;
      public uint Name;
      public uint FirstThunk;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct IMAGE_SECTION_HEADER
    {
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
      public byte[] Name;
      public uint PhysicalAddress;
      public uint VirtualAddress;
      public uint SizeOfRawData;
      public uint PointerToRawData;
      public uint PointerToRelocations;
      public uint PointerToLinenumbers;
      public short NumberOfRelocations;
      public short NumberOfLinenumbers;
      public uint Characteristics;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct IMAGE_DOS_HEADER
    {
      public ushort e_magic;
      public ushort e_cblp;
      public ushort e_cp;
      public ushort e_crlc;
      public ushort e_cparhdr;
      public ushort e_minalloc;
      public ushort e_maxalloc;
      public ushort e_ss;
      public ushort e_sp;
      public ushort e_csum;
      public ushort e_ip;
      public ushort e_cs;
      public ushort e_lfarlc;
      public ushort e_ovno;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public ushort[] e_res1;
      public ushort e_oemid;
      public ushort e_oeminfo;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
      public ushort[] e_res2;
      public int e_lfanew;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct IMAGE_DATA_DIRECTORY
    {
      public uint VirtualAddress;
      public uint Size;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct IMAGE_OPTIONAL_HEADER32
    {
      public ushort Magic;
      public byte MajorLinkerVersion;
      public byte MinorLinkerVersion;
      public uint SizeOfCode;
      public uint SizeOfInitializedData;
      public uint SizeOfUninitializedData;
      public uint AddressOfEntryPoint;
      public uint BaseOfCode;
      public uint BaseOfData;
      public uint ImageBase;
      public uint SectionAlignment;
      public uint FileAlignment;
      public ushort MajorOperatingSystemVersion;
      public ushort MinorOperatingSystemVersion;
      public ushort MajorImageVersion;
      public ushort MinorImageVersion;
      public ushort MajorSubsystemVersion;
      public ushort MinorSubsystemVersion;
      public uint Win32VersionValue;
      public uint SizeOfImage;
      public uint SizeOfHeaders;
      public uint CheckSum;
      public ushort Subsystem;
      public ushort DllCharacteristics;
      public uint SizeOfStackReserve;
      public uint SizeOfStackCommit;
      public uint SizeOfHeapReserve;
      public uint SizeOfHeapCommit;
      public uint LoaderFlags;
      public uint NumberOfRvaAndSizes;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
      public DynamicDllLoader.IMAGE_DATA_DIRECTORY[] DataDirectory;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct IMAGE_FILE_HEADER
    {
      public ushort Machine;
      public ushort NumberOfSections;
      public uint TimeDateStamp;
      public uint PointerToSymbolTable;
      public uint NumberOfSymbols;
      public ushort SizeOfOptionalHeader;
      public ushort Characteristics;
    }

    public struct IMAGE_NT_HEADERS
    {
      public uint Signature;
      public DynamicDllLoader.IMAGE_FILE_HEADER FileHeader;
      public DynamicDllLoader.IMAGE_OPTIONAL_HEADER32 OptionalHeader;
    }

    internal class Win32Constants
    {
      public static uint MEM_COMMIT = 4096;
      public static uint PAGE_EXECUTE_READWRITE = 64;
      public static uint PAGE_READWRITE = 4;
      public static uint MEM_RELEASE = 32768;
      public static uint MEM_RESERVE = 8192;
    }

    internal static class Win32Imports
    {
      [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
      public static extern uint GetProcAddress(IntPtr hModule, string procName);

      [DllImport("kernel32")]
      public static extern int LoadLibrary(string lpFileName);

      [DllImport("kernel32")]
      public static extern uint GetLastError();

      [DllImport("kernel32.dll")]
      public static extern IntPtr GetProcAddress(IntPtr module, IntPtr ordinal);

      [DllImport("kernel32")]
      public static extern uint VirtualAlloc(
        uint lpStartAddr,
        uint size,
        uint flAllocationType,
        uint flProtect);

      [DllImport("kernel32.dll", SetLastError = true)]
      internal static extern bool VirtualFree(IntPtr lpAddress, UIntPtr dwSize, uint dwFreeType);

      [DllImport("kernel32.dll", SetLastError = true)]
      internal static extern bool VirtualProtect(
        IntPtr lpAddress,
        uint dwSize,
        uint flNewProtect,
        out uint lpflOldProtect);
    }

    internal static class PointerHelpers
    {
      public static unsafe T ToStruct<T>(byte[] data) where T : struct
      {
        fixed (byte* numPtr = &data[0])
          return (T) Marshal.PtrToStructure(new IntPtr((void*) numPtr), typeof (T));
      }

      public static unsafe T ToStruct<T>(byte[] data, uint from) where T : struct
      {
        fixed (byte* numPtr = &data[(int) from])
          return (T) Marshal.PtrToStructure(new IntPtr((void*) numPtr), typeof (T));
      }

      public static T ToStruct<T>(IntPtr ptr, uint from) where T : struct => (T) Marshal.PtrToStructure(ptr + (int) from, typeof (T));
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private unsafe delegate bool fnDllEntry(int instance, uint reason, void* reserved);
  }
}
