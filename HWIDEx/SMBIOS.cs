// Decompiled with JetBrains decompiler
// Type: HwidGetCurrentEx.SMBIOS
// Assembly: HwidGetCurrentEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 200C1AD7-2186-49E5-9EB2-5AB7013ECA80 Assembly location: D:\downloads\Programs\HwidGetCurrentEx.dll

using System.Runtime.InteropServices;

namespace HWIDEx
{
    public class SMBIOS
    {
        public enum SMBIOSTableType : sbyte
        {
            BIOSInformation = 0,
            SystemInformation = 1,
            BaseBoardInformation = 2,
            EnclosureInformation = 3,
            ProcessorInformation = 4,
            MemoryControllerInformation = 5,
            MemoryModuleInformation = 6,
            CacheInformation = 7,
            PortConnectorInformation = 8,
            SystemSlotsInformation = 9,
            OnBoardDevicesInformation = 10, // 0x0A
            const_11 = 11, // 0x0B
            SystemConfigurationOptions = 12, // 0x0C
            BIOSLanguageInformation = 13, // 0x0D
            GroupAssociations = 14, // 0x0E
            SystemEventLog = 15, // 0x0F
            PhysicalMemoryArray = 16, // 0x10
            MemoryDevice = 17, // 0x11
            MemoryErrorInformation = 18, // 0x12
            MemoryArrayMappedAddress = 19, // 0x13
            MemoryDeviceMappedAddress = 20, // 0x14
            EndofTable = 127, // 0x7F
        }

        public struct SMBIOSTableHeader
        {
            public SMBIOS.SMBIOSTableType type;
            public byte length;
            public ushort Handle;
        }

        public struct SMBIOSTableSystemInfo
        {
            public SMBIOS.SMBIOSTableHeader header;
            public byte manufacturer;
            public byte productName;
            public byte version;
            public byte serialNumber;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] UUID;
        }

        public struct SMBIOSTableBaseBoardInfo
        {
            public SMBIOS.SMBIOSTableHeader header;
        }

        public struct SMBIOSTableEnclosureInfo
        {
            public SMBIOS.SMBIOSTableHeader header;
        }

        public struct SMBIOSTableProcessorInfo
        {
            public SMBIOS.SMBIOSTableHeader header;
        }

        public struct SMBIOSTableCacheInfo
        {
            public SMBIOS.SMBIOSTableHeader header;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct UnkownInfo
        {
            public SMBIOS.SMBIOSTableHeader header;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SHAInfo
        {
            public SMBIOS.SMBIOSTableHeader header;
            public int size;
            public byte cache;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PhysicalMemoryArray
        {
            public SMBIOS.SMBIOSTableHeader header;
            public uint EmptyPageNumber;
            public uint TotalPageNumber;
        }

        public struct BIOSInformation
        {
            public SMBIOS.SMBIOSTableHeader header;
            public byte vendor;
            public byte version;
            public ushort startingSegment;
            public byte releaseDate;
            public byte biosRomSize;
            public ulong characteristics;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] extensionBytes;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MemCtrlInfo
        {
            public SMBIOS.SMBIOSTableHeader header;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MemModuleInfo
        {
            public SMBIOS.SMBIOSTableHeader header;
            public byte SocketDesignation;
            public byte BankConnections;
            public byte CurrentSpeed;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct OemString
        {
            public SMBIOS.SMBIOSTableHeader header;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MemoryArrayMappedAddress
        {
            public SMBIOS.SMBIOSTableHeader header;
            public uint Starting;
            public uint Ending;
            public ushort Handle;
            public byte PartitionWidth;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BuiltinPointDevice
        {
            public SMBIOS.SMBIOSTableHeader header;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PortableBattery
        {
            public SMBIOS.SMBIOSTableHeader header;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MemoryDevice
        {
            public SMBIOS.SMBIOSTableHeader header;
            public ushort PhysicalArrayHandle;
            public ushort ErrorInformationHandle;
            public ushort TotalWidth;
            public ushort DataWidth;
            public ushort Size;
        }

        public struct RawSMBIOSData
        {
            public byte Used20CallingMethod;
            public byte SMBIOSMajorVersion;
            public byte SMBIOSMinorVersion;
            public byte DmiRevision;
            public uint Length;
        }
    }
}