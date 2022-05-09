// Decompiled with JetBrains decompiler
// Type: HwidGetCurrentEx.Native
// Assembly: HwidGetCurrentEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 200C1AD7-2186-49E5-9EB2-5AB7013ECA80 Assembly location: D:\downloads\Programs\HwidGetCurrentEx.dll

using System;
using System.Runtime.InteropServices;

namespace HWIDEx
{
    public static class Native
    {
        public const int ERROR_INVALID_HANDLE_VALUE = -1;
        public const uint GENERIC_READ = 2147483648;
        public const uint GENERIC_WRITE = 1073741824;
        public const uint FILE_SHARE_READ = 1;
        public const uint FILE_SHARE_WRITE = 2;
        public const uint OPEN_EXISTING = 3;
        public const uint IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS = 5636096;
        public const uint IOCTL_STORAGE_QUERY_PROPERTY = 2954240;
        public const uint IOCTL_BTH_GET_LOCAL_INFO = 4259840;
        public const uint IOCTL_NDIS_QUERY_GLOBAL_STATS = 1507330;
        public const uint PERMANENT_ADDRESS = 16843009;
        public const uint RSMB = 1381190978;

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetupDiGetDriverInfoDetail(
          IntPtr DeviceInfoSet,
          ref Native.SP_DEVINFO_DATA DeviceInfoData,
          ref Native.SP_DRVINFO_DATA DriverInfoData,
          ref Native.SP_DRVINFO_DETAIL_DATA DriverInfoDetailData,
          int DriverInfoDetailDataSize,
          ref int RequiredSize);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetupDiEnumDriverInfo(
          IntPtr DeviceInfoSet,
          ref Native.SP_DEVINFO_DATA DeviceInfoData,
          int DriverType,
          int MemberIndex,
          ref Native.SP_DRVINFO_DATA DriverInfoData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetupDiGetClassDevs(
          ref Guid ClassGuid,
          [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
          IntPtr hwndParent,
          uint Flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetupDiGetClassDevs(
          ref Guid ClassGuid,
          IntPtr Enumerator,
          IntPtr hwndParent,
          int Flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetupDiGetClassDevs(
          IntPtr ClassGuid,
          [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
          IntPtr hwndParent,
          int Flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevsW(
          [In] ref Guid ClassGuid,
          [MarshalAs(UnmanagedType.LPWStr)] string Enumerator,
          IntPtr parent,
          int flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInterfaces(
          IntPtr hDevInfo,
          IntPtr devInfo,
          ref Guid interfaceClassGuid,
          uint memberIndex,
          ref Native.SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetupDiGetDeviceInterfaceDetailW(
          IntPtr hDevInfo,
          ref Native.SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
          ref Native.SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
          uint deviceInterfaceDetailDataSize,
          out uint requiredSize,
          ref Native.SP_DEVINFO_DATA deviceInfoData);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetupDiGetDeviceInterfaceDetailW(
          IntPtr hDevInfo,
          ref Native.SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
          IntPtr deviceInterfaceDetailData,
          uint deviceInterfaceDetailDataSize,
          out uint requiredSize,
          IntPtr deviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInfo(
          IntPtr DeviceInfoSet,
          uint MemberIndex,
          ref Native.SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll")]
        public static extern int CM_Get_Parent(ref IntPtr pdnDevInst, IntPtr dnDevInst, int ulFlags);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("cfgmgr32.dll", CharSet = CharSet.Auto)]
        public static extern int CM_Get_DevNode_Registry_PropertyW(
          IntPtr dnDevInst,
          int ulProperty,
          ref int pulRegDataType,
          IntPtr Buffer,
          ref int pulLength,
          int ulFlags);

        [DllImport("cfgmgr32.dll", EntryPoint = "CM_Get_DevNode_Registry_PropertyA", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CM_Get_DevNode_Registry_Property(
          IntPtr dnDevInst,
          int ulProperty,
          ref int pulRegDataType,
          ref IntPtr Buffer,
          ref int pulLength,
          int ulFlags);

        [DllImport("cfgmgr32.dll", SetLastError = true)]
        public static extern int CM_Get_DevNode_Status(
          ref int status,
          ref int probNum,
          IntPtr devInst,
          int flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern int CM_Get_Device_ID(
          IntPtr pdnDevInst,
          ref IntPtr buffer,
          int bufferLen,
          int flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern int CM_Get_Device_IDW(
          IntPtr pdnDevInst,
          IntPtr Buffer,
          uint bufferLen,
          uint flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetupDiGetDeviceRegistryProperty(
          IntPtr deviceInfoSet,
          ref Native.SP_DEVINFO_DATA deviceInfoData,
          uint property,
          int propertyRegDataType,
          IntPtr propertyBuffer,
          uint propertyBufferSize,
          ref int requiredSize);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetupDiGetDeviceRegistryPropertyW(
          IntPtr deviceInfoSet,
          ref Native.SP_DEVINFO_DATA deviceInfoData,
          int property,
          int propertyRegDataType,
          byte[] propertyBuffer,
          int propertyBufferSize,
          out int requiredSize);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, UIntPtr count);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(
          string lpFileName,
          uint dwDesiredAccess,
          uint dwShareMode,
          uint lpSecurityAttributes,
          uint dwCreationDisposition,
          uint dwFlagsAndAttributes,
          uint hTemplateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CreateFileW(
          string filename,
          uint dwDesiredAccess,
          uint dwShareMode,
          uint lpSecurityAttributes,
          uint dwCreationDisposition,
          uint dwFlagsAndAttributes,
          uint hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("hid.dll")]
        public static extern bool HidD_GetAttributes(
          IntPtr HidDeviceObject,
          ref Native.HidD_Attributes Attributes);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
          IntPtr hDevice,
          uint dwIoControlCode,
          IntPtr lpInBuffer,
          int nInBufferSize,
          IntPtr lpOutBuffer,
          int nOutBufferSize,
          out uint lpBytesReturned,
          IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
          IntPtr hDevice,
          uint dwIoControlCode,
          ref uint lpInBuffer,
          int nInBufferSize,
          IntPtr lpOutBuffer,
          int nOutBufferSize,
          out uint lpBytesReturned,
          IntPtr lpOverlapped);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetCurrentHwProfile(IntPtr fProfile);

        [DllImport("kernel32")]
        public static extern bool GlobalMemoryStatusEx(ref Native.MEMORYSTATUSEX stat);

        [DllImport("kernel32.dll")]
        public static extern uint GetSystemFirmwareTable(
          uint FirmwareTableProviderSignature,
          uint FirmwareTableID,
          IntPtr pFirmwareTableBuffer,
          uint BufferSize);

        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("wwapi.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int WwanOpenHandle(
          int dwClientVersion,
          IntPtr pReserved,
          out int pdwNegotiatedVersion,
          out IntPtr phClientHandle);

        [DllImport("wwapi.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int WwanCloseHandle(IntPtr hClientHandle, IntPtr pReserved);

        [DllImport("wwapi.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int WwanEnumerateInterfaces(
          IntPtr hClientHandle,
          int pdwReserved,
          out Native.WWAN_INTERFACE_INFO_LIST ppInterfaceList);

        [DllImport("wwapi.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int WwanFreeMemory(IntPtr pMem);

        [DllImport("wwapi.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int WwanSetInterface(
          IntPtr hClientHandle,
          Guid pInterfaceGuid,
          int OpCode,
          int dwDataSize,
          IntPtr pData,
          IntPtr pReserved1,
          IntPtr pReserved2,
          IntPtr pReserved3);

        [DllImport("wwapi.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int WlanQueryInterface(
          IntPtr hClientHandle,
          Guid pInterfaceGuid,
          int OpCode,
          IntPtr pReserved,
          out int pdwDataSize,
          out IntPtr ppData,
          out int pWlanOpcodeValueType);

        [DllImport("rpcrt4.dll", SetLastError = true)]
        public static extern int UuidCreateSequential(out Guid guid);

        public enum DI_FUNCTION
        {
            DIF_SELECTDEVICE = 1,
            DIF_INSTALLDEVICE = 2,
            DIF_ASSIGNRESOURCES = 3,
            DIF_PROPERTIES = 4,
            DIF_REMOVE = 5,
            DIF_FIRSTTIMESETUP = 6,
            DIF_FOUNDDEVICE = 7,
            DIF_SELECTCLASSDRIVERS = 8,
            DIF_VALIDATECLASSDRIVERS = 9,
            DIF_INSTALLCLASSDRIVERS = 10, // 0x0000000A
            DIF_CALCDISKSPACE = 11, // 0x0000000B
            DIF_DESTROYPRIVATEDATA = 12, // 0x0000000C
            DIF_VALIDATEDRIVER = 13, // 0x0000000D
            DIF_MOVEDEVICE = 14, // 0x0000000E
            DIF_DETECT = 15, // 0x0000000F
            DIF_INSTALLWIZARD = 16, // 0x00000010
            DIF_DESTROYWIZARDDATA = 17, // 0x00000011
            DIF_PROPERTYCHANGE = 18, // 0x00000012
            DIF_ENABLECLASS = 19, // 0x00000013
            DIF_DETECTVERIFY = 20, // 0x00000014
            DIF_INSTALLDEVICEFILES = 21, // 0x00000015
            DIF_UNREMOVE = 22, // 0x00000016
            DIF_SELECTBESTCOMPATDRV = 23, // 0x00000017
            DIF_ALLOW_INSTALL = 24, // 0x00000018
            DIF_REGISTERDEVICE = 25, // 0x00000019
            DIF_NEWDEVICEWIZARD_PRESELECT = 26, // 0x0000001A
            DIF_NEWDEVICEWIZARD_SELECT = 27, // 0x0000001B
            DIF_NEWDEVICEWIZARD_PREANALYZE = 28, // 0x0000001C
            DIF_NEWDEVICEWIZARD_POSTANALYZE = 29, // 0x0000001D
            DIF_NEWDEVICEWIZARD_FINISHINSTALL = 30, // 0x0000001E
            DIF_UNUSED1 = 31, // 0x0000001F
            DIF_INSTALLINTERFACES = 32, // 0x00000020
            DIF_DETECTCANCEL = 33, // 0x00000021
            DIF_REGISTER_COINSTALLERS = 34, // 0x00000022
            DIF_ADDPROPERTYPAGE_ADVANCED = 35, // 0x00000023
            DIF_ADDPROPERTYPAGE_BASIC = 36, // 0x00000024
            DIF_RESERVED1 = 37, // 0x00000025
            DIF_TROUBLESHOOTER = 38, // 0x00000026
            DIF_POWERMESSAGEWAKE = 39, // 0x00000027
            DIF_ADDREMOTEPROPERTYPAGE_ADVANCED = 40, // 0x00000028
            DIF_UPDATEDRIVER_UI = 41, // 0x00000029
            DIF_RESERVED2 = 48, // 0x00000030
        }

        public struct MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class HWProfile
        {
            public int dwDockInfo;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 39)]
            public string szHwProfileGuid;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szHwProfileName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class DISK_EXTENT
        {
            public uint DiskNumber;
            public long StartingOffset;
            public long ExtentLength;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class VOLUME_DISK_EXTENTS
        {
            public uint NumberOfDiskExtents;
            public Native.DISK_EXTENT Extents;
        }

        public struct DEVICE_SEEK_PENALTY_DESCRIPTOR
        {
            public readonly uint Version;
            public readonly uint Size;

            [MarshalAs(UnmanagedType.U1)]
            public readonly bool IncursSeekPenalty;
        }

        public struct STORAGE_DESCRIPTOR_HEADER
        {
            public uint Version;
            public uint Size;
        }

        public struct STORAGE_DEVICE_DESCRIPTOR
        {
            public uint Version;
            public uint Size;
            public byte DeviceType;
            public byte DeviceTypeModifier;
            public byte RemovableMedia;
            public byte CommandQueueing;
            public uint VendorIdOffset;
            public uint ProductIdOffset;
            public uint ProductRevisionOffset;
            public uint SerialNumberOffset;
            public byte BusType;
            public uint RawPropertiesLength;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] RawDeviceProperties;
        }

        public enum WWAN_INTERFACE_STATE
        {
            WwanInterfaceStateNotReady,
            WwanInterfaceStateDeviceLocked,
            WwanInterfaceStateUserAccountNotActivated,
            WwanInterfaceStateRegistered,
            WwanInterfaceStateRegistering,
            WwanInterfaceStateDeregistered,
            WwanInterfaceStateAttached,
            WwanInterfaceStateAttaching,
            WwanInterfaceStateDetaching,
            WwanInterfaceStateActivated,
            WwanInterfaceStateActivating,
            WwanInterfaceStateDeactivating,
        }

        public struct WWAN_INTERFACE_STATUS
        {
            private bool fInitialized;
            private Native.WWAN_INTERFACE_STATE InterfaceState;
        }

        public struct WWAN_INTERFACE_INFO
        {
            public Guid InterfaceGuid;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] strInterfaceDescription;

            public Native.WWAN_INTERFACE_STATUS InterfaceStatus;
            public int dwReserved1;
            public Guid guidReserved;
            public Guid ParentInterfaceGuid;
            public int dwReserved2;
            public int dwIndex;
            public int dwReserved3;
            public int dwReserved4;
        }

        public struct WWAN_INTERFACE_INFO_LIST
        {
            public int dwNumberOfItems;
            public Native.WWAN_INTERFACE_INFO[] InterfaceInfo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode)]
        public struct SP_DRVINFO_DATA
        {
            public int cbSize;
            public uint DriverType;
            public UIntPtr Reserved;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Description;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string MfgName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string ProviderName;

            public System.Runtime.InteropServices.ComTypes.FILETIME DriverDate;
            public ulong DriverVersion;
        }

        public struct STORAGE_PROPERTY_QUERY
        {
            public int PropertyId;
            public int QueryType;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] AdditionalParameters;
        }

        public struct HidD_Attributes
        {
            public int Size;
            public ushort VendorID;
            public ushort ProductID;
            public ushort VersionNumber;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode)]
        public struct SP_DRVINFO_DETAIL_DATA
        {
            public int cbSize;
            public System.Runtime.InteropServices.ComTypes.FILETIME InfDate;
            public int CompatIDsOffset;
            public int CompatIDsLength;
            public IntPtr Reserved;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string SectionName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string InfFileName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DrvDescription;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string HardwareID;
        }

        [Flags]
        public enum DiGetClassFlags : uint
        {
            DIGCF_DEFAULT = 1,
            DIGCF_PRESENT = 2,
            DIGCF_ALLCLASSES = 4,
            SPDRP_UNUSED2 = DIGCF_ALLCLASSES | DIGCF_PRESENT, // 0x00000006
            DIGCF_PROFILE = 8,
            DIGCF_DEVICEINTERFACE = 16, // 0x00000010
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public uint cbSize;
            public Guid InterfaceClassGuid;
            public uint Flags;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
        public struct NativeDeviceInterfaceDetailData
        {
            public int size;
            public char devicePath;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string DevicePath;
        }
    }
}