// Decompiled with JetBrains decompiler
// Type: HwidGetCurrentEx.HWID
// Assembly: HwidGetCurrentEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 200C1AD7-2186-49E5-9EB2-5AB7013ECA80 Assembly location: D:\downloads\Programs\HwidGetCurrentEx.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace HWIDEx
{
    public static class HWID
    {
        private static byte[] signByte = new byte[14]
        {
      (byte) 0,
      (byte) 14,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 4,
      (byte) 15,
      (byte) 5,
      (byte) 6,
      (byte) 7,
      (byte) 8,
      (byte) 9,
      (byte) 10,
      (byte) 12
        };

        private static uint[] SHA256Magic = new uint[64]
        {
      1116352408U,
      1899447441U,
      3049323471U,
      3921009573U,
      961987163U,
      1508970993U,
      2453635748U,
      2870763221U,
      3624381080U,
      310598401U,
      607225278U,
      1426881987U,
      1925078388U,
      2162078206U,
      2614888103U,
      3248222580U,
      3835390401U,
      4022224774U,
      264347078U,
      604807628U,
      770255983U,
      1249150122U,
      1555081692U,
      1996064986U,
      2554220882U,
      2821834349U,
      2952996808U,
      3210313671U,
      3336571891U,
      3584528711U,
      113926993U,
      338241895U,
      666307205U,
      773529912U,
      1294757372U,
      1396182291U,
      1695183700U,
      1986661051U,
      2177026350U,
      2456956037U,
      2730485921U,
      2820302411U,
      3259730800U,
      3345764771U,
      3516065817U,
      3600352804U,
      4094571909U,
      275423344U,
      430227734U,
      506948616U,
      659060556U,
      883997877U,
      958139571U,
      1322822218U,
      1537002063U,
      1747873779U,
      1955562222U,
      2024104815U,
      2227730452U,
      2361852424U,
      2428436474U,
      2756734187U,
      3204031479U,
      3329325298U
        };

        private static uint[] MemoryMagic = new uint[15]
        {
      0U,
      268435456U,
      0U,
      536870912U,
      0U,
      1073741824U,
      0U,
      1610612736U,
      0U,
      2147483648U,
      0U,
      3221225472U,
      0U,
      1073741824U,
      0U
        };

        public static string CreateBlock(byte[] arrayHWID, int cbsize)
        {
            byte[] src = new byte[36]
            {
        (byte) 0,
        (byte) 2,
        (byte) 0,
        (byte) 1,
        (byte) 1,
        (byte) 0,
        (byte) 2,
        (byte) 5,
        (byte) 0,
        (byte) 3,
        (byte) 1,
        (byte) 0,
        (byte) 4,
        (byte) 2,
        (byte) 0,
        (byte) 6,
        (byte) 1,
        (byte) 0,
        (byte) 8,
        (byte) 7,
        (byte) 0,
        (byte) 9,
        (byte) 3,
        (byte) 0,
        (byte) 10,
        (byte) 1,
        (byte) 0,
        (byte) 12,
        (byte) 7,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
            };
            int length = cbsize + 6 + 37;
            byte[] numArray = new byte[length];
            numArray[0] = (byte)length;
            numArray[4] = (byte)19;
            Buffer.BlockCopy((Array)arrayHWID, 0, (Array)numArray, 6, cbsize);
            numArray[cbsize + 6] = (byte)12;
            Buffer.BlockCopy((Array)src, 0, (Array)numArray, cbsize + 6 + 1, src.Length);
            return Convert.ToBase64String(numArray);
        }

        public static byte[] GetCurrentEx()
        {
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            byte[] source = new byte[280];
            List<ushort> ushortList = new List<ushort>();
            for (int index1 = 0; index1 < HWID.signByte.Length; ++index1)
            {
                switch (HWID.signByte[index1])
                {
                    case 0:
                        ushortList = HWID.CollectInternal(ref GUID_DEVINTERFACE.GUID_DEVINTERFACE_CDROM, false);
                        num2 = 0;
                        goto default;
                    case 1:
                        ushortList = HWID.CollectInternal(ref GUID_DEVINTERFACE.GUID_DEVCLASS_HDC, false);
                        num2 = 1;
                        goto default;
                    case 2:
                        ushortList = HWID.EnumInterfaces(ref GUID_DEVINTERFACE.GUID_DEVINTERFACE_DISK, 2954240U);
                        num2 = 2;
                        goto default;
                    case 3:
                        ushortList = HWID.CollectInternal(ref GUID_DEVINTERFACE.GUID_DEVCLASS_DISPLAY, false);
                        num2 = 3;
                        goto default;
                    case 4:
                        ushortList = HWID.CollectInternal(ref GUID_DEVINTERFACE.GUID_DEVCLASS_SCSIADAPTER, false);
                        num2 = 4;
                        goto default;
                    case 5:
                        ushortList = HWID.CollectInternal(ref GUID_DEVINTERFACE.GUID_DEVCLASS_PCMCIA, false);
                        num2 = -1;
                        goto default;
                    case 6:
                        ushortList = HWID.CollectInternal(ref GUID_DEVINTERFACE.GUID_6994ad04_93ef_11d0_a3cc_00a0c9223196, true);
                        num2 = 5;
                        goto default;
                    case 7:
                        ushortList = HWID.CollectHWProfile();
                        source[22] = (byte)1;
                        break;

                    case 8:
                        ushortList = HWID.EnumInterfaces(ref GUID_DEVINTERFACE.GUID_NDIS_LAN_CLASS, 1507330U);
                        num2 = 7;
                        goto default;
                    case 9:
                        ushortList = HWID.CollectCPU();
                        num2 = 8;
                        goto default;
                    case 10:
                        ushortList = HWID.CollectMemory();
                        num2 = -2;
                        num3 -= ushortList.Count;
                        num1 = -2;
                        goto default;
                    case 12:
                        ushortList = HWID.CollectBIOS();
                        num2 = -1;
                        num3 -= ushortList.Count;
                        num1 = -1;
                        goto default;
                    case 14:
                        ushortList = HWID.CollectWwan();
                        num2 = 0;
                        goto default;
                    case 15:
                        ushortList = HWID.EnumInterfaces(ref GUID_DEVINTERFACE.GUID_BTHPORT_DEVICE_INTERFACE, 4259840U);
                        num2 = 4;
                        goto default;
                    default:
                        if (ushortList.Count > 0)
                        {
                            if (num2 >= 0)
                            {
                                int index2 = num2 * 2 + 4;
                                source[index2] += (byte)(ushortList.Count & (int)byte.MaxValue);
                            }
                            ushortList.Sort();
                            for (int index3 = 0; index3 < ushortList.Count; ++index3)
                            {
                                int index4 = (num1 + 14) * 2;
                                byte[] bytes = BitConverter.GetBytes(ushortList[index3]);
                                source[index4 + 1] = bytes[1];
                                source[index4] = bytes[0];
                                ++num3;
                                ++num1;
                            }
                        }
                        else
                        {
                            int num4 = num2 * 2 + 4;
                            source[num4 + 1] += (byte)(ushortList.Count & (int)byte.MaxValue);
                        }
                        break;
                }
            }
            source[0] = (byte)(num3 * 2 + 28);
            //Debug.Print(source[0].ToString() + Environment.NewLine + BitConverter.ToString(((IEnumerable<byte>)source).Take<byte>((int)source[0]).ToArray<byte>()).Replace("-", " "));
            return ((IEnumerable<byte>)source).Take<byte>((int)source[0]).ToArray<byte>();
        }

        private static List<ushort> CollectWwan()
        {
            List<ushort> ushortList = new List<ushort>();
            IntPtr phClientHandle = IntPtr.Zero;
            int dwClientVersion = 1;
            int pdwNegotiatedVersion = 0;
            int num1 = 5;
            int num2 = 0;
            Native.WWAN_INTERFACE_INFO_LIST ppInterfaceList = new Native.WWAN_INTERFACE_INFO_LIST();
            try
            {
                if (Native.WwanOpenHandle(dwClientVersion, IntPtr.Zero, out pdwNegotiatedVersion, out phClientHandle) != 0)
                {
                    bool flag;
                    do
                    {
                        if (Native.WwanEnumerateInterfaces(phClientHandle, 0, out ppInterfaceList) == 0)
                        {
                            ++num2;
                            if (num2 <= num1)
                            {
                                flag = false;
                                for (int index = 0; index < ppInterfaceList.dwNumberOfItems; ++index)
                                {
                                    Native.WWAN_INTERFACE_INFO wwanInterfaceInfo = ppInterfaceList.InterfaceInfo[index];
                                    IntPtr ppData;
                                    if (Native.WlanQueryInterface(phClientHandle, wwanInterfaceInfo.InterfaceGuid, 7, IntPtr.Zero, out int _, out ppData, out int _) != 0)
                                    {
                                        byte[] numArray = new byte[36];
                                        Marshal.Copy(new IntPtr(IntPtr.Size == 4 ? (long)(ppData.ToInt32() + 652) : ppData.ToInt64() + 652L), numArray, 0, numArray.Length);
                                        ushort num3 = HWID.AddInstanceHash(numArray, 36U, true);
                                        if (!ushortList.Contains(num3))
                                            ushortList.Add(num3);
                                    }
                                }
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                    while (!flag);
                }
                if (phClientHandle != IntPtr.Zero)
                    Native.WwanCloseHandle(phClientHandle, IntPtr.Zero);
            }
            catch
            {
            }
            return ushortList;
        }

        private static List<ushort> CollectHWProfile()
        {
            List<ushort> ushortList = new List<ushort>();
            IntPtr num1 = Marshal.AllocHGlobal(123);
            Native.HWProfile structure = new Native.HWProfile();
            Marshal.StructureToPtr<Native.HWProfile>(structure, num1, false);
            if (Native.GetCurrentHwProfile(num1))
                Marshal.PtrToStructure<Native.HWProfile>(num1, structure);
            else
                structure.dwDockInfo = 1;
            if ((structure.dwDockInfo & 4) == 0 && (structure.dwDockInfo & 3) != 3)
            {
                ushort num2 = HWID.AddInstanceHash(BitConverter.GetBytes(structure.dwDockInfo), 4U, false);
                if (!ushortList.Contains(num2))
                    ushortList.Add(num2);
            }
            Marshal.FreeHGlobal(num1);
            return ushortList;
        }

        private static List<ushort> CollectCPU()
        {
            List<ushort> ushortList = new List<ushort>();
            byte[] first = new byte[0];
            byte[] source = CPUID.Invoke(0);
            byte[] numArray = CPUID.Invoke(1);
            int num1 = (int)BitConverter.ToUInt32(numArray, 0) & -16;
            int num2 = (int)BitConverter.ToUInt32(numArray, 4) & 16777215;
            ushort num3 = HWID.AddInstanceHash(((IEnumerable<byte>)first).Concat<byte>((IEnumerable<byte>)((IEnumerable<byte>)source).Skip<byte>(4).Take<byte>(4).ToArray<byte>()).Concat<byte>((IEnumerable<byte>)((IEnumerable<byte>)source).Skip<byte>(12).Take<byte>(4).ToArray<byte>()).Concat<byte>((IEnumerable<byte>)((IEnumerable<byte>)source).Skip<byte>(8).Take<byte>(4).ToArray<byte>()).Concat<byte>((IEnumerable<byte>)BitConverter.GetBytes(num1)).Concat<byte>((IEnumerable<byte>)BitConverter.GetBytes(num2)).ToArray<byte>(), 20U, true);
            if (!ushortList.Contains(num3))
                ushortList.Add(num3);
            return ushortList;
        }

        private static List<ushort> CollectMemory()
        {
            List<ushort> ushortList = new List<ushort>();
            Native.MEMORYSTATUSEX stat = new Native.MEMORYSTATUSEX();
            stat.dwLength = (uint)Marshal.SizeOf(typeof(Native.MEMORYSTATUSEX));
            Native.GlobalMemoryStatusEx(ref stat);
            byte[] bytes = BitConverter.GetBytes(stat.ullTotalPhys);
            int int32_1 = BitConverter.ToInt32(bytes, 0);
            int int32_2 = BitConverter.ToInt32(bytes, 4);
            int num1 = (int)(BitUtil.PAIR(int32_2, (long)int32_1) >> 10);
            int num2 = int32_2 >> 10;
            uint InstalledMemorySize = 0;
            if (!HWID.GetInstalledMemorySize(ref InstalledMemorySize))
                InstalledMemorySize = 0U;
            if ((long)InstalledMemorySize < (long)num1)
                InstalledMemorySize = (uint)num1;
            uint high = (uint)(BitUtil.PAIR(0U, (ulong)InstalledMemorySize) >> 22);
            uint low = InstalledMemorySize << 10;
            int num3 = 0;
            while (high >= HWID.MemoryMagic[2 * num3] && (high > HWID.MemoryMagic[2 * num3] || low > HWID.MemoryMagic[2 * num3]))
            {
                ++num3;
                if (num3 >= 8)
                    break;
            }
            ushort num4 = HWID.AddInstanceHash(BitConverter.GetBytes(num3 != 8 ? num3 : 8 * (int)(BitUtil.PAIR(high, (ulong)low) - 3221225472UL >> 30) + 7), 4U, true);
            if (!ushortList.Contains(num4))
                ushortList.Add(num4);
            return ushortList;
        }

        private static int GetProductName(
          ref List<string> productList,
          ref IntPtr buffer,
          ref int position)
        {
            do
            {
                string stringAnsi = Marshal.PtrToStringAnsi(new IntPtr(IntPtr.Size == 4 ? (long)(buffer.ToInt32() + position) : buffer.ToInt64() + (long)position));
                if (!string.IsNullOrEmpty(stringAnsi))
                    productList.Add(stringAnsi);
                position += stringAnsi.Length + 1;
            }
            while (Marshal.ReadByte(buffer, position) > (byte)0);
            ++position;
            return position;
        }

        private static List<ushort> CollectBIOS()
        {
            List<ushort> ushortList = new List<ushort>();
            List<string> stringList = new List<string>();
            uint systemFirmwareTable = Native.GetSystemFirmwareTable(1381190978U, 0U, IntPtr.Zero, 0U);
            IntPtr num1 = Marshal.AllocHGlobal((int)systemFirmwareTable);
            Native.GetSystemFirmwareTable(1381190978U, 0U, num1, systemFirmwareTable);
            int num2 = 8;
            SMBIOS.BIOSInformation structure1 = (SMBIOS.BIOSInformation)Marshal.PtrToStructure(new IntPtr(IntPtr.Size == 4 ? (long)(num1.ToInt32() + num2) : num1.ToInt64() + (long)num2), typeof(SMBIOS.BIOSInformation));
            int num3 = num2 + (int)structure1.header.length;
            string stringAnsi1 = Marshal.PtrToStringAnsi(new IntPtr(IntPtr.Size == 4 ? (long)(num1.ToInt32() + num3) : num1.ToInt64() + (long)num3));
            int num4 = num3 + (stringAnsi1.Length + 1);
            string stringAnsi2 = Marshal.PtrToStringAnsi(new IntPtr(IntPtr.Size == 4 ? (long)(num1.ToInt32() + num4) : num1.ToInt64() + (long)num4));
            int num5 = num4 + (stringAnsi2.Length + 1);
            string stringAnsi3 = Marshal.PtrToStringAnsi(new IntPtr(IntPtr.Size == 4 ? (long)(num1.ToInt32() + num5) : num1.ToInt64() + (long)num5));
            int num6 = num5 + (stringAnsi3.Length + 1) + 1;
            int num7 = num6 + 8;
            SMBIOS.SMBIOSTableSystemInfo structure2 = (SMBIOS.SMBIOSTableSystemInfo)Marshal.PtrToStructure(new IntPtr(IntPtr.Size == 4 ? (long)(num1.ToInt32() + num6) : num1.ToInt64() + (long)num6), typeof(SMBIOS.SMBIOSTableSystemInfo));
            int num8 = num6 + (int)structure2.header.length;
            string stringAnsi4 = Marshal.PtrToStringAnsi(new IntPtr(IntPtr.Size == 4 ? (long)(num1.ToInt32() + num8) : num1.ToInt64() + (long)num8));
            int num9 = num8 + (stringAnsi4.Length + 1);
            string stringAnsi5 = Marshal.PtrToStringAnsi(new IntPtr(IntPtr.Size == 4 ? (long)(num1.ToInt32() + num9) : num1.ToInt64() + (long)num9));
            int num10 = num9 + (stringAnsi5.Length + 1);
            string stringAnsi6 = Marshal.PtrToStringAnsi(new IntPtr(IntPtr.Size == 4 ? (long)(num1.ToInt32() + num10) : num1.ToInt64() + (long)num10));
            int num11 = num10 + (stringAnsi6.Length + 1);
            string stringAnsi7 = Marshal.PtrToStringAnsi(new IntPtr(IntPtr.Size == 4 ? (long)(num1.ToInt32() + num11) : num1.ToInt64() + (long)num11));
            int num12 = num11 + (stringAnsi7.Length + 1);
            byte[] numArray1 = new byte[16];
            Marshal.Copy(new IntPtr(IntPtr.Size == 4 ? (long)(num1.ToInt32() + num7) : num1.ToInt64() + (long)num7), numArray1, 0, numArray1.Length);
            byte[] numArray2 = new byte[0];
            byte[] buffer = !(stringAnsi5 == "None") ? ((IEnumerable<byte>)numArray1).Concat<byte>((IEnumerable<byte>)Encoding.UTF8.GetBytes(stringAnsi4)).Concat<byte>((IEnumerable<byte>)Encoding.UTF8.GetBytes(stringAnsi5)).Concat<byte>((IEnumerable<byte>)Encoding.UTF8.GetBytes(stringAnsi7)).Concat<byte>((IEnumerable<byte>)Encoding.UTF8.GetBytes(stringAnsi1)).ToArray<byte>() : ((IEnumerable<byte>)numArray1).Concat<byte>((IEnumerable<byte>)Encoding.UTF8.GetBytes(stringAnsi4)).Concat<byte>((IEnumerable<byte>)Encoding.UTF8.GetBytes(stringAnsi7)).Concat<byte>((IEnumerable<byte>)Encoding.UTF8.GetBytes(stringAnsi6)).Concat<byte>((IEnumerable<byte>)Encoding.UTF8.GetBytes(stringAnsi1)).ToArray<byte>();
            ushort num13 = HWID.AddInstanceHash(buffer, (uint)buffer.Length, true);
            if (!ushortList.Contains(num13))
                ushortList.Add(num13);
            Marshal.FreeHGlobal(num1);
            return ushortList;
        }

        private static bool GetInstalledMemorySize(ref uint InstalledMemorySize)
        {
            List<string> productList = new List<string>();
            uint systemFirmwareTable1 = Native.GetSystemFirmwareTable(1381190978U, 0U, IntPtr.Zero, 0U);
            IntPtr buffer = Marshal.AllocHGlobal((int)systemFirmwareTable1);
            uint systemFirmwareTable2 = Native.GetSystemFirmwareTable(1381190978U, 0U, buffer, systemFirmwareTable1);
            if (systemFirmwareTable2 > 0U)
            {
                int position = 8;
                byte num1 = Marshal.ReadByte(buffer, position);
                while ((long)(position + 4) < (long)systemFirmwareTable2 && num1 != (byte)127)
                {
                    if (num1 == (byte)17)
                    {
                        SMBIOS.MemoryDevice structure = (SMBIOS.MemoryDevice)Marshal.PtrToStructure(new IntPtr(IntPtr.Size == 4 ? (long)(buffer.ToInt32() + position) : buffer.ToInt64() + (long)position), typeof(SMBIOS.MemoryDevice));
                        if (structure.Size == (ushort)0)
                            return false;
                        InstalledMemorySize += (uint)structure.Size << 10;
                        position += (int)structure.header.length;
                        position = HWID.GetProductName(ref productList, ref buffer, ref position);
                        num1 = Marshal.ReadByte(buffer, position);
                    }
                    else
                    {
                        int num2 = (int)Marshal.ReadByte(buffer, position + 1);
                        position += num2;
                        position = HWID.GetProductName(ref productList, ref buffer, ref position);
                        num1 = Marshal.ReadByte(buffer, position);
                    }
                }
            }
            return true;
        }

        private static bool HwidGetPnPDeviceRegistryProperty(
          IntPtr hDevInfo,
          Native.SP_DEVINFO_DATA devData,
          ref int cbsize,
          ref byte[] buffer)
        {
            int requiredSize = 0;
            if (!Native.SetupDiGetDeviceRegistryPropertyW(hDevInfo, ref devData, 8, 0, (byte[])null, 0, out requiredSize))
            {
                switch (Marshal.GetLastWin32Error())
                {
                    case 13:
                        return false;

                    case 122:
                        buffer = new byte[requiredSize];
                        if (Native.SetupDiGetDeviceRegistryPropertyW(hDevInfo, ref devData, 8, 0, buffer, requiredSize, out requiredSize))
                        {
                            cbsize = requiredSize;
                            return true;
                        }
                        break;

                    default:
                        return false;
                }
            }
            return false;
        }

        private static bool HwidGetPnPRemovalPolicy(IntPtr hDevInfo, Native.SP_DEVINFO_DATA devData)
        {
            int requiredSize = 0;
            byte[] propertyBuffer = new byte[4];
            return !Native.SetupDiGetDeviceRegistryPropertyW(hDevInfo, ref devData, 31, 0, propertyBuffer, 4, out requiredSize) || BitConverter.ToInt32(propertyBuffer, 0) == 1 || BitConverter.ToInt32(propertyBuffer, 0) - 2 > 3;
        }

        public static ushort EnumInterfaceCallback(IntPtr hFile)
        {
            ushort num = 0;
            IntPtr lpOutBuffer = Marshal.AllocHGlobal(292);
            uint lpBytesReturned;
            if (!Native.DeviceIoControl(hFile, 4259840U, IntPtr.Zero, 0, lpOutBuffer, 292, out lpBytesReturned, IntPtr.Zero))
                Marshal.GetLastWin32Error();
            if (lpBytesReturned == 292U)
            {
                byte[] numArray = new byte[6];
                Marshal.Copy(new IntPtr(IntPtr.Size == 4 ? (long)(lpOutBuffer.ToInt32() + 8) : lpOutBuffer.ToInt64() + 8L), numArray, 0, numArray.Length);
                num = HWID.AddInstanceHash(((IEnumerable<byte>)Encoding.Unicode.GetBytes(BitConverter.ToString(((IEnumerable<byte>)numArray).Reverse<byte>().ToArray<byte>()).Replace("-", "").ToLower())).Concat<byte>((IEnumerable<byte>)new byte[2]).ToArray<byte>(), 26U, true);
            }
            return num;
        }

        public static bool NextInterface(
          IntPtr hDevInfo,
          ref Guid ClassGuid,
          ref Native.SP_DEVICE_INTERFACE_DETAIL_DATA devDetail,
          ref Native.SP_DEVINFO_DATA deviceInfoData,
          ref uint index)
        {
            uint requiredSize = 0;
            Native.SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new Native.SP_DEVICE_INTERFACE_DATA();
            deviceInterfaceData.cbSize = (uint)Marshal.SizeOf<Native.SP_DEVICE_INTERFACE_DATA>(deviceInterfaceData);
            if (!Native.SetupDiEnumDeviceInterfaces(hDevInfo, IntPtr.Zero, ref ClassGuid, index, ref deviceInterfaceData))
                return false;
            Native.SetupDiGetDeviceInterfaceDetailW(hDevInfo, ref deviceInterfaceData, IntPtr.Zero, 0U, out requiredSize, IntPtr.Zero);
            if (requiredSize > 6U)
            {
                Native.SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData = new Native.SP_DEVICE_INTERFACE_DETAIL_DATA();
                deviceInterfaceDetailData.cbSize = IntPtr.Size != 8 ? 4 + Marshal.SystemDefaultCharSize : 8;
                Native.SP_DEVINFO_DATA deviceInfoData1 = new Native.SP_DEVINFO_DATA();
                deviceInfoData1.cbSize = (uint)Marshal.SizeOf<Native.SP_DEVINFO_DATA>(deviceInfoData1);
                if (Native.SetupDiGetDeviceInterfaceDetailW(hDevInfo, ref deviceInterfaceData, ref deviceInterfaceDetailData, requiredSize, out requiredSize, ref deviceInfoData1))
                {
                    devDetail = deviceInterfaceDetailData;
                    deviceInfoData = deviceInfoData1;
                }
            }
            return true;
        }

        public static bool NextInterfaceDeviceHandle(
          IntPtr hDevInfo,
          ref Guid ClassGuid,
          ref bool readable,
          ref uint index,
          ref IntPtr hFile)
        {
            //Native.SP_DEVICE_INTERFACE_DETAIL_DATA interfaceDetailData = new Native.SP_DEVICE_INTERFACE_DETAIL_DATA();
            //Native.SP_DEVINFO_DATA spDevinfoData = new Native.SP_DEVINFO_DATA();
            Native.SP_DEVINFO_DATA deviceInfoData;
            do
            {
                Native.SP_DEVICE_INTERFACE_DETAIL_DATA devDetail = new Native.SP_DEVICE_INTERFACE_DETAIL_DATA();
                deviceInfoData = new Native.SP_DEVINFO_DATA();
                if (!HWID.NextInterface(hDevInfo, ref ClassGuid, ref devDetail, ref deviceInfoData, ref index))
                    return false;
                ++index;
                if (deviceInfoData.cbSize >= 0U && !HWID.IsSoftwareDevice(new IntPtr((long)deviceInfoData.DevInst)))
                    hFile = Native.CreateFileW(devDetail.DevicePath, 0U, 3U, 0U, 3U, 0U, 0U);
            }
            while (hFile == IntPtr.Zero || hFile.ToInt32() == -1);
            if (hFile != IntPtr.Zero && hFile.ToInt32() != -1)
            {
                readable = HWID.HwidGetPnPRemovalPolicy(hDevInfo, deviceInfoData);
                if (readable)
                {
                }
            }
            else
                hDevInfo = IntPtr.Zero;
            return true;
        }

        private static List<ushort> EnumInterfaces(ref Guid ClassGuid, uint dwIoControlCode)
        {
            uint index = 0;
            bool readable = false;
            IntPtr zero = IntPtr.Zero;
            List<ushort> ushortList = new List<ushort>();
            try
            {
                IntPtr classDevsW = Native.SetupDiGetClassDevsW(ref ClassGuid, (string)null, IntPtr.Zero, 18);
                if (classDevsW != IntPtr.Zero)
                {
                    ushort num1;
                    while (true)
                    {
                        if (zero != IntPtr.Zero && zero.ToInt32() != -1)
                        {
                            try
                            {
                                Native.CloseHandle(zero);
                                zero = IntPtr.Zero;
                            }
                            catch
                            {
                                //Debug.Print(ex.ToString());
                                zero = IntPtr.Zero;
                            }
                        }
                        Native.SP_DEVICE_INTERFACE_DATA structure1 = new Native.SP_DEVICE_INTERFACE_DATA();
                        structure1.cbSize = (uint)Marshal.SizeOf<Native.SP_DEVICE_INTERFACE_DATA>(structure1);
                        if (HWID.NextInterfaceDeviceHandle(classDevsW, ref ClassGuid, ref readable, ref index, ref zero))
                        {
                            if (zero != IntPtr.Zero && zero.ToInt32() != -1)
                            {
                                switch (dwIoControlCode)
                                {
                                    case 1507330:
                                        IntPtr num2 = Marshal.AllocHGlobal(6);
                                        uint lpBytesReturned1 = 0;
                                        uint lpInBuffer1 = 16843009;
                                        if (!Native.DeviceIoControl(zero, 1507330U, ref lpInBuffer1, 4, num2, 6, out lpBytesReturned1, IntPtr.Zero))
                                            Marshal.GetLastWin32Error();
                                        if (lpBytesReturned1 == 6U)
                                        {
                                            byte[] numArray = new byte[6];
                                            Marshal.Copy(num2, numArray, 0, numArray.Length);
                                            ushort num3 = HWID.AddInstanceHash(numArray, 6U, true);
                                            if (!readable)
                                                ++num3;
                                            if (!ushortList.Contains(num3))
                                                ushortList.Add(num3);
                                        }
                                        Marshal.FreeHGlobal(num2);
                                        break;

                                    case 2954240:
                                        if (readable)
                                        {
                                            IntPtr num4 = Marshal.AllocHGlobal(1024);
                                            uint lpBytesReturned2 = 0;
                                            uint lpInBuffer2 = 0;
                                            if (!Native.DeviceIoControl(zero, 2954240U, ref lpInBuffer2, 12, num4, 1024, out lpBytesReturned2, IntPtr.Zero))
                                                Marshal.GetLastWin32Error();
                                            Native.STORAGE_DEVICE_DESCRIPTOR structure2 = (Native.STORAGE_DEVICE_DESCRIPTOR)Marshal.PtrToStructure(num4, typeof(Native.STORAGE_DEVICE_DESCRIPTOR));
                                            int num5 = Marshal.SizeOf<Native.STORAGE_DEVICE_DESCRIPTOR>(structure2);
                                            int length = (int)structure2.Size - num5;
                                            structure2.RawDeviceProperties = new byte[length];
                                            Marshal.Copy(new IntPtr(num4.ToInt64() + (long)num5), structure2.RawDeviceProperties, 0, length);
                                            string empty = string.Empty;
                                            string s = string.Empty;
                                            if (structure2.VendorIdOffset > 0U)
                                            {
                                                int offset = (int)structure2.VendorIdOffset - Marshal.SizeOf<Native.STORAGE_DEVICE_DESCRIPTOR>(structure2);
                                                string str = BitUtil.ReadNullTerminatedAnsiString(structure2.RawDeviceProperties, offset);
                                                while (str.EndsWith("  "))
                                                    str = str.Remove(str.Length - 1);
                                                empty += str;
                                            }
                                            if (structure2.ProductIdOffset > 0U)
                                            {
                                                int offset = (int)structure2.ProductIdOffset - Marshal.SizeOf<Native.STORAGE_DEVICE_DESCRIPTOR>(structure2);
                                                string str = BitUtil.ReadNullTerminatedAnsiString(structure2.RawDeviceProperties, offset);
                                                empty += str.Trim();
                                            }
                                            if (structure2.SerialNumberOffset > 0U && structure2.SerialNumberOffset != uint.MaxValue)
                                            {
                                                int offset = (int)structure2.SerialNumberOffset - Marshal.SizeOf<Native.STORAGE_DEVICE_DESCRIPTOR>(structure2);
                                                s = BitUtil.ReadNullTerminatedAnsiString(structure2.RawDeviceProperties, offset);
                                            }
                                            if (!string.IsNullOrEmpty(empty) && !string.IsNullOrEmpty(s))
                                            {
                                                byte[] array = ((IEnumerable<byte>)new byte[1].Concats<byte>(Encoding.UTF8.GetBytes(empty)).Concats<byte>(new byte[1]).Concats<byte>(Encoding.UTF8.GetBytes(s)).Concats<byte>(new byte[1])).ToArray<byte>();
                                                ushort num6 = HWID.AddInstanceHash(array, (uint)array.Length, true);
                                                if (!readable)
                                                    ++num6;
                                                if (!ushortList.Contains(num6))
                                                    ushortList.Add(num6);
                                            }
                                            break;
                                        }
                                        break;

                                    case 4259840:
                                        num1 = HWID.EnumInterfaceCallback(zero);
                                        if (num1 <= (ushort)0)
                                            break;
                                        goto label_28;
                                }
                            }
                        }
                        else
                            goto label_45;
                    }
                label_28:
                    if (!readable)
                        ++num1;
                    if (!ushortList.Contains(num1))
                        ushortList.Add(num1);
                    return ushortList;
                }
            label_45:
                Native.SetupDiDestroyDeviceInfoList(classDevsW);
            }
            catch
            {
                //Debug.Print(ex.ToString());
            }
            return ushortList;
        }

        private static ushort AddInstanceHash(byte[] buffer, uint cbsize, bool readable)
        {
            uint[] numArray = HWID.SHA256Init();
            HWID.SHA256Update(ref numArray, buffer, cbsize);
            ushort num = (ushort)(HWID.SHA256Final(ref numArray)[0] & 65534U);
            if (!readable)
                num |= (ushort)1;
            return num;
        }

        private static bool IsSoftwareDevice(IntPtr DevInst)
        {
            IntPtr num = Marshal.AllocHGlobal(206);
            int pulLength = 402;
            int pulRegDataType = 0;
            if (Native.CM_Get_DevNode_Registry_PropertyW(DevInst, 23, ref pulRegDataType, num, ref pulLength, 0) == 0)
            {
                if (pulRegDataType != 1 || pulLength < 2 || Marshal.PtrToStringUni(num).ToString().Trim() == "SWD")
                    return true;
                int status = 0;
                int probNum = 0;
                if (Native.CM_Get_DevNode_Status(ref status, ref probNum, DevInst, 0) == 0 && (status & 1) != 0)
                {
                    IntPtr pdnDevInst = new IntPtr();
                    if (Native.CM_Get_Parent(ref pdnDevInst, DevInst, 0) == 0 && Native.CM_Get_Device_IDW(pdnDevInst, num, 201U, 0U) == 0 && Marshal.PtrToStringUni(num).Contains("HTREE\\ROOT\\0"))
                        return true;
                }
            }
            return false;
        }

        private static List<ushort> CollectInternal(ref Guid ClassGuid, bool UnknownDevice)
        {
            List<ushort> ushortList = new List<ushort>();
            Guid empty = Guid.Empty;
            IntPtr classDevsW = Native.SetupDiGetClassDevsW(ref ClassGuid, (string)null, IntPtr.Zero, UnknownDevice ? 18 : 6);
            if (classDevsW != IntPtr.Zero)
            {
                Native.SP_DEVINFO_DATA devData = new Native.SP_DEVINFO_DATA();
                devData.cbSize = (uint)Marshal.SizeOf<Native.SP_DEVINFO_DATA>(new Native.SP_DEVINFO_DATA());
                for (uint MemberIndex = 0; Native.SetupDiEnumDeviceInfo(classDevsW, MemberIndex, ref devData); ++MemberIndex)
                {
                    if (Native.SetupDiEnumDeviceInfo(classDevsW, MemberIndex, ref devData))
                    {
                        Guid guid = Guid.Empty;
                        if (UnknownDevice)
                        {
                            int cbsize = 0;
                            byte[] buffer = new byte[0];
                            if (HWID.HwidGetPnPDeviceRegistryProperty(classDevsW, devData, ref cbsize, ref buffer) && ((long)cbsize & 4294967294L) == 78L)
                                guid = new Guid(Encoding.Unicode.GetString(buffer).Replace("\0", ""));
                        }
                        else
                            guid = ClassGuid;
                        if (devData.ClassGuid == guid && !HWID.IsSoftwareDevice(new IntPtr((long)devData.DevInst)) && devData.Reserved != IntPtr.Zero)
                        {
                            int requiredSize = 0;
                            IntPtr num1 = Marshal.AllocHGlobal(512);
                            if (Native.SetupDiGetDeviceRegistryProperty(classDevsW, ref devData, 1U, 0, num1, 2048U, ref requiredSize))
                            {
                                int cbsize = Marshal.PtrToStringUni(num1).Length * 2 + 2;
                                byte[] numArray = new byte[requiredSize];
                                Marshal.Copy(num1, numArray, 0, requiredSize);
                                bool pnPremovalPolicy = HWID.HwidGetPnPRemovalPolicy(classDevsW, devData);
                                ushort num2 = HWID.AddInstanceHash(numArray, (uint)cbsize, pnPremovalPolicy);
                                if (!ushortList.Contains(num2))
                                    ushortList.Add(num2);
                            }
                            Marshal.FreeHGlobal(num1);
                        }
                    }
                }
            }
            Native.SetupDiDestroyDeviceInfoList(classDevsW);
            return ushortList;
        }

        private static uint[] SHA256Init()
        {
            uint[] dst = new uint[112];
            uint[] src = new uint[10]
            {
        1779033703U,
        3144134277U,
        1013904242U,
        2773480762U,
        1359893119U,
        2600822924U,
        528734635U,
        1541459225U,
        0U,
        0U
            };
            Buffer.BlockCopy((Array)src, 0, (Array)dst, 0, src.Length * 4);
            return dst;
        }

        private static void SHA256Update(ref uint[] SHA256Init, byte[] buffer, uint cbsize)
        {
            int num1 = 0;
            byte[] numArray1 = new byte[0];
            if (buffer.Length % 4 != 0)
                buffer = ((IEnumerable<byte>)buffer).Concat<byte>((IEnumerable<byte>)new byte[4 - buffer.Length % 4]).ToArray<byte>();
            uint[] numArray2 = new uint[buffer.Length / 4];
            Buffer.BlockCopy((Array)buffer, 0, (Array)numArray2, 0, buffer.Length);
            byte[] array = ((IEnumerable<uint>)SHA256Init).SelectMany<uint, byte>(new Func<uint, IEnumerable<byte>>(BitConverter.GetBytes)).ToArray<byte>();
            uint num2 = cbsize + (uint)array[36];
            int num3 = (int)array[36] & 63;
            int num4 = (int)array[36] & 63;
            array[36] = (byte)num2;
            SHA256Init[9] = (uint)(byte)num2;
            if (num2 < cbsize)
                ++array[32];
            long num5 = (long)num3 + (long)cbsize;
            if (num3 != 0 && num5 == (long)num3 + (long)cbsize && (uint)((ulong)num3 + (ulong)cbsize) >= 64U)
            {
                Buffer.BlockCopy((Array)buffer, 0, (Array)SHA256Init, num3 + 40, 64 - num3);
                cbsize = (uint)((ulong)num5 - 64UL);
                uint[] numArray3 = new uint[(SHA256Init.Length - 40) * 4];
                Buffer.BlockCopy((Array)SHA256Init, 40, (Array)numArray3, 0, (SHA256Init.Length - 40) * 4);
                HWID.SHA256Transform(ref SHA256Init, numArray3);
                numArray1 = new byte[buffer.Length - (64 - num4)];
                Buffer.BlockCopy((Array)buffer, 64 - num4, (Array)numArray1, 0, buffer.Length - (64 - num4));
                num1 = numArray1.Length;
            }
            if (num1 >= 3 && cbsize >= 64U)
            {
                uint[] numArray4 = new uint[array.Length - 40];
#pragma warning disable CA2018 // “Buffer.BlockCopy” 需要为 “count” 参数复制字节数
                Buffer.BlockCopy((Array)SHA256Init, 40, (Array)numArray4, 0, numArray4.Length);
#pragma warning restore CA2018 // “Buffer.BlockCopy” 需要为 “count” 参数复制字节数
                int num6 = (int)(cbsize >> 6);
                do
                {
                    if (cbsize > 0U)
                    {
                        Buffer.BlockCopy((Array)numArray1, 0, (Array)numArray4, 0, 64);
                        HWID.SHA256Transform(ref SHA256Init, numArray4);
                        Buffer.BlockCopy((Array)array, 0, (Array)numArray4, 0, 40);
                        Buffer.BlockCopy((Array)buffer, 64, (Array)numArray2, 0, buffer.Length - 64);
                        buffer = ((IEnumerable<byte>)buffer).Skip<byte>(64).ToArray<byte>();
                        cbsize -= 64U;
                        --num6;
                    }
                }
                while (num6 != 0);
            }
            if (cbsize >= 64U)
            {
                int num7 = (int)(cbsize >> 6);
                do
                {
                    HWID.SHA256Transform(ref SHA256Init, numArray2);
                    cbsize -= 64U;
                    if (cbsize > 0U)
                    {
                        Buffer.BlockCopy((Array)buffer, 64, (Array)numArray2, 0, buffer.Length - 64);
                        buffer = ((IEnumerable<byte>)buffer).Skip<byte>(64).ToArray<byte>();
                        --num7;
                    }
                }
                while (num7 != 0);
            }
            if (cbsize <= 0U)
                return;
            Buffer.BlockCopy((Array)numArray2, 0, (Array)SHA256Init, num4 + 40, (int)cbsize);
        }

        private static uint[] SHA256Transform(ref uint[] SHA256Init, uint[] buffer)
        {
            uint[] src = SHA256Init;
            uint[] numArray1 = new uint[16];
            int index1 = 0;
            uint num1 = 0;
            uint num2 = 0;
            for (; index1 < 16; ++index1)
            {
                byte[] array = ((IEnumerable<byte>)BitConverter.GetBytes(buffer[index1])).Reverse<byte>().ToArray<byte>();
                numArray1[index1] = BitConverter.ToUInt32(array, 0);
            }
            uint num3 = SHA256Init[1];
            uint num4 = SHA256Init[4];
            uint num5 = SHA256Init[3];
            uint num6 = SHA256Init[0];
            uint num7 = SHA256Init[2];
            uint num8 = SHA256Init[5];
            uint num9 = SHA256Init[6];
            uint num10 = SHA256Init[7];
            uint index2 = 0;
            uint num11 = num3;
            uint num12 = num4;
            uint num13 = num6;
            while (true)
            {
                uint num14 = (uint)((int)num10 + (int)numArray1[(int)index2] + (int)HWID.SHA256Magic[(int)index2] + ((int)num12 & (int)num8 ^ (int)num9 & ~(int)num4) + ((int)BitUtil.RotateRight(num4, 6) ^ (int)BitUtil.RotateRight(num4, 11) ^ (int)BitUtil.RotateRight(num4, 25)));
                uint num15 = num14 + num5;
                uint num16 = num13 ^ num3;
                uint num17 = num13 & num3;
                uint num18 = (uint)((int)num14 + ((int)BitUtil.RotateRight(num13, 2) ^ (int)BitUtil.RotateRight(num13, 13) ^ (int)BitUtil.RotateRight(num13, 22)) + ((int)num17 ^ (int)num7 & (int)num16));
                uint num19 = (uint)((int)num9 + (int)numArray1[1 + (int)index2] + (int)HWID.SHA256Magic[(int)index2 + 1] + ((int)num15 & (int)num12 ^ (int)num8 & ~(int)num15) + ((int)BitUtil.RotateRight(num15, 6) ^ (int)BitUtil.RotateRight(num15, 11) ^ (int)BitUtil.RotateRight(num15, 25)));
                uint num20 = num19 + num7;
                uint num21 = (uint)((int)num19 + ((int)BitUtil.RotateRight(num18, 2) ^ (int)BitUtil.RotateRight(num18, 13) ^ (int)BitUtil.RotateRight(num18, 22)) + ((int)num17 ^ (int)num18 & (int)num16));
                uint num22 = num21;
                uint num23 = (uint)((int)num8 + (int)numArray1[2 + (int)index2] + (int)HWID.SHA256Magic[(int)index2 + 2] + ((int)num20 & (int)num15 ^ (int)num12 & ~(int)num20) + ((int)BitUtil.RotateRight(num20, 6) ^ (int)BitUtil.RotateRight(num20, 11) ^ (int)BitUtil.RotateRight(num20, 25)));
                uint num24 = num23 + num11;
                uint num25 = (uint)((int)num23 + ((int)BitUtil.RotateRight(num22, 2) ^ (int)BitUtil.RotateRight(num22, 13) ^ (int)BitUtil.RotateRight(num22, 22)) + ((int)num13 & (int)num21 ^ (int)num18 & ((int)num13 ^ (int)num21)));
                uint num26 = (uint)((int)num12 + (int)numArray1[3 + (int)index2] + (int)HWID.SHA256Magic[(int)index2 + 3] + ((int)num24 & (int)num20 ^ (int)num15 & ~(int)num24) + ((int)BitUtil.RotateRight(num24, 6) ^ (int)BitUtil.RotateRight(num24, 11) ^ (int)BitUtil.RotateRight(num24, 25)));
                uint num27 = num26 + num13;
                uint num28 = (uint)((int)num26 + ((int)BitUtil.RotateRight(num25, 2) ^ (int)BitUtil.RotateRight(num25, 13) ^ (int)BitUtil.RotateRight(num25, 22)) + ((int)num25 & (int)num21 ^ (int)num18 & ((int)num25 ^ (int)num21)));
                uint num29 = (uint)((int)num15 + (int)numArray1[4 + (int)index2] + (int)HWID.SHA256Magic[(int)index2 + 4] + ((int)num27 & (int)num24 ^ (int)num20 & ~(int)num27) + ((int)BitUtil.RotateRight(num27, 6) ^ (int)BitUtil.RotateRight(num27, 11) ^ (int)BitUtil.RotateRight(num27, 25)));
                num10 = num29 + num18;
                num5 = (uint)((int)num29 + ((int)BitUtil.RotateRight(num28, 2) ^ (int)BitUtil.RotateRight(num28, 13) ^ (int)BitUtil.RotateRight(num28, 22)) + ((int)num28 & (int)num25 ^ (int)num21 & ((int)num28 ^ (int)num25)));
                uint num30 = (uint)((int)num20 + (int)numArray1[5 + (int)index2] + (int)HWID.SHA256Magic[(int)index2 + 5] + ((int)num27 & (int)num10 ^ (int)num24 & ~(int)num10) + ((int)BitUtil.RotateRight(num10, 6) ^ (int)BitUtil.RotateRight(num10, 11) ^ (int)BitUtil.RotateRight(num10, 25)));
                num9 = num30 + num22;
                uint num31 = (uint)((int)num30 + ((int)BitUtil.RotateRight(num5, 2) ^ (int)BitUtil.RotateRight(num5, 13) ^ (int)BitUtil.RotateRight(num5, 22)) + ((int)num5 & (int)num28 ^ (int)num25 & ((int)num5 ^ (int)num28)));
                num7 = num31;
                uint num32 = (uint)((int)num24 + (int)numArray1[6 + (int)index2] + (int)HWID.SHA256Magic[(int)index2 + 6] + ((int)num9 & (int)num10 ^ (int)num27 & ~(int)num9) + ((int)BitUtil.RotateRight(num9, 6) ^ (int)BitUtil.RotateRight(num9, 11) ^ (int)BitUtil.RotateRight(num9, 25)));
                num8 = num32 + num25;
                num11 = (uint)((int)num32 + ((int)BitUtil.RotateRight(num31, 2) ^ (int)BitUtil.RotateRight(num31, 13) ^ (int)BitUtil.RotateRight(num31, 22)) + ((int)num31 & (int)num5 ^ (int)num28 & ((int)num31 ^ (int)num5)));
                uint num33 = (uint)((int)numArray1[7 + (int)index2] + (int)HWID.SHA256Magic[(int)index2 + 7] + ((int)num8 & (int)num9 ^ (int)num10 & ~(int)num8) + ((int)BitUtil.RotateRight(num8, 6) ^ (int)BitUtil.RotateRight(num8, 11) ^ (int)BitUtil.RotateRight(num8, 25)));
                index2 += 8U;
                uint num34 = num27 + num33;
                num12 = num34 + num28;
                uint num35 = num34 + (BitUtil.RotateRight(num11, 2) ^ BitUtil.RotateRight(num11, 13) ^ BitUtil.RotateRight(num11, 22));
                uint num36 = (uint)((int)num11 & (int)num31 ^ (int)num5 & ((int)num11 ^ (int)num31));
                uint num37 = num35 + num36;
                num13 = num35 + num36;
                if (index2 < 16U)
                {
                    num4 = num12;
                    num3 = num11;
                }
                else
                    break;
            }
            if (index2 < 64U)
            {
                char ch1 = (char)(index2 - 2U);
                int num38 = (int)index2 - 7;
                char ch2 = (char)(index2 + 1U);
                int num39 = (int)index2 - 2;
                int num40 = (int)index2 + 1;
                do
                {
                    uint num41 = numArray1[(int)index2 & 15];
                    char ch3 = (char)num38;
                    int num42 = num38 + 1;
                    uint num43 = num41 + (uint)((int)numArray1[(int)ch3 & 15] + ((int)(numArray1[(int)ch2 & 15] >> 3) ^ (int)BitUtil.RotateRight(numArray1[(int)ch2 & 15], 7) ^ (int)BitUtil.RotateRight(numArray1[(int)ch2 & 15], 18)) + ((int)(numArray1[(int)ch1 & 15] >> 10) ^ (int)BitUtil.RotateRight(numArray1[(int)ch1 & 15], 17) ^ (int)BitUtil.RotateRight(numArray1[(int)ch1 & 15], 19)));
                    numArray1[(int)index2 & 15] = num43;
                    uint num44 = (uint)((int)num10 + (int)num43 + (int)HWID.SHA256Magic[(int)index2] + ((int)num12 & (int)num8 ^ (int)num9 & ~(int)num12) + ((int)BitUtil.RotateRight(num12, 6) ^ (int)BitUtil.RotateRight(num12, 11) ^ (int)BitUtil.RotateRight(num12, 25)));
                    uint num45 = num44 + num5;
                    int num46 = num40 + 1;
                    int num47 = num39 + 1;
                    int num48 = num47;
                    uint num49 = (uint)((int)num44 + ((int)BitUtil.RotateRight(num13, 2) ^ (int)BitUtil.RotateRight(num13, 13) ^ (int)BitUtil.RotateRight(num13, 22)) + ((int)num13 & (int)num11 ^ (int)num7 & ((int)num13 ^ (int)num11)));
                    uint num50 = numArray1[(int)index2 + 2 & 15];
                    uint num51 = numArray1[(int)index2 + 1 & 15];
                    int num52 = num42;
                    int num53 = num52 + 1;
                    int index3 = num52 & 15;
                    uint num54 = num51 + (uint)((int)numArray1[index3] + ((int)(num50 >> 3) ^ (int)BitUtil.RotateRight(num50, 7) ^ (int)BitUtil.RotateRight(num50, 18)) + ((int)(numArray1[num47 & 15] >> 10) ^ (int)BitUtil.RotateRight(numArray1[num47 & 15], 17) ^ (int)BitUtil.RotateRight(numArray1[num47 & 15], 19)));
                    numArray1[(int)index2 + 1 & 15] = num54;
                    uint num55 = (uint)((int)num9 + (int)num54 + (int)HWID.SHA256Magic[(int)index2 + 1] + ((int)num45 & (int)num12 ^ (int)num8 & ~(int)num45) + ((int)BitUtil.RotateRight(num45, 6) ^ (int)BitUtil.RotateRight(num45, 11) ^ (int)BitUtil.RotateRight(num45, 25)));
                    uint num56 = num55 + num7;
                    uint num57 = (uint)((int)num55 + ((int)BitUtil.RotateRight(num49, 2) ^ (int)BitUtil.RotateRight(num49, 13) ^ (int)BitUtil.RotateRight(num49, 22)) + ((int)num13 & (int)num11 ^ (int)num49 & ((int)num13 ^ (int)num11)));
                    int num58 = num46 + 1;
                    uint num59 = numArray1[(int)index2 + 3 & 15];
                    uint num60 = numArray1[(int)index2 + 2 & 15];
                    int num61 = num53;
                    int num62 = num61 + 1;
                    int index4 = num61 & 15;
                    uint num63 = num60 + (uint)((int)numArray1[index4] + ((int)(num59 >> 3) ^ (int)BitUtil.RotateRight(num59, 7) ^ (int)BitUtil.RotateRight(num59, 18)) + ((int)(numArray1[num47 + 1 & 15] >> 10) ^ (int)BitUtil.RotateRight(numArray1[num47 + 1 & 15], 17) ^ (int)BitUtil.RotateRight(numArray1[num47 + 1 & 15], 19)));
                    numArray1[(int)index2 + 2 & 15] = num63;
                    uint num64 = (uint)((int)num8 + (int)num63 + (int)HWID.SHA256Magic[(int)index2 + 2] + ((int)num56 & (int)num45 ^ (int)num12 & ~(int)num56) + ((int)BitUtil.RotateRight(num56, 6) ^ (int)BitUtil.RotateRight(num56, 11) ^ (int)BitUtil.RotateRight(num56, 25)));
                    uint num65 = num64 + num11;
                    int num66 = (int)BitUtil.LOBYTE(num58 + 1);
                    int num67 = (int)BitUtil.LOBYTE(num47 + 2);
                    uint num68 = (uint)((int)num64 + ((int)BitUtil.RotateRight(num57, 2) ^ (int)BitUtil.RotateRight(num57, 13) ^ (int)BitUtil.RotateRight(num57, 22)) + ((int)num13 & (int)num57 ^ (int)num49 & ((int)num13 ^ (int)num57)));
                    int num69 = (int)numArray1[(int)index2 + 3 & 15];
                    uint[] numArray2 = numArray1;
                    int num70 = num62;
                    int num71 = num70 + 1;
                    int index5 = num70 & 15;
                    int num72 = (int)numArray2[index5] + ((int)(numArray1[num66 & 15] >> 3) ^ (int)BitUtil.RotateRight(numArray1[num66 & 15], 7) ^ (int)BitUtil.RotateRight(numArray1[num66 & 15], 18)) + ((int)(numArray1[num67 & 15] >> 10) ^ (int)BitUtil.RotateRight(numArray1[num67 & 15], 17) ^ (int)BitUtil.RotateRight(numArray1[num67 & 15], 19));
                    uint num73 = (uint)(num69 + num72);
                    numArray1[(int)index2 + 3 & 15] = num73;
                    uint num74 = (uint)((int)num12 + (int)num73 + (int)HWID.SHA256Magic[(int)index2 + 3] + ((int)num65 & (int)num56 ^ (int)num45 & ~(int)num65) + ((int)BitUtil.RotateRight(num65, 6) ^ (int)BitUtil.RotateRight(num65, 11) ^ (int)BitUtil.RotateRight(num65, 25)));
                    uint num75 = num74 + num13;
                    int num76 = (int)BitUtil.LOBYTE(num58 + 2);
                    uint num77 = (uint)((int)num74 + ((int)BitUtil.RotateRight(num68, 2) ^ (int)BitUtil.RotateRight(num68, 13) ^ (int)BitUtil.RotateRight(num68, 22)) + ((int)num68 & (int)num57 ^ (int)num49 & ((int)num68 ^ (int)num57)));
                    int num78 = (int)numArray1[(int)index2 + 4 & 15];
                    uint[] numArray3 = numArray1;
                    int num79 = num71;
                    int num80 = num79 + 1;
                    int index6 = num79 & 15;
                    int num81 = (int)numArray3[index6] + ((int)(numArray1[num76 & 15] >> 3) ^ (int)BitUtil.RotateRight(numArray1[num76 & 15], 7) ^ (int)BitUtil.RotateRight(numArray1[num76 & 15], 18)) + ((int)(numArray1[num48 + 3 & 15] >> 10) ^ (int)BitUtil.RotateRight(numArray1[num48 + 3 & 15], 17) ^ (int)BitUtil.RotateRight(numArray1[num48 + 3 & 15], 19));
                    uint num82 = (uint)(num78 + num81);
                    numArray1[(int)index2 + 4 & 15] = num82;
                    uint num83 = (uint)((int)num45 + (int)num82 + (int)HWID.SHA256Magic[(int)index2 + 4] + ((int)num75 & (int)num65 ^ (int)num56 & ~(int)num75) + ((int)BitUtil.RotateRight(num75, 6) ^ (int)BitUtil.RotateRight(num75, 11) ^ (int)BitUtil.RotateRight(num75, 25)));
                    num10 = num83 + num49;
                    int num84 = (int)BitUtil.LOBYTE(num58 + 3);
                    uint num85 = (uint)((int)num83 + ((int)BitUtil.RotateRight(num77, 2) ^ (int)BitUtil.RotateRight(num77, 13) ^ (int)BitUtil.RotateRight(num77, 22)) + ((int)num77 & (int)num68 ^ (int)num57 & ((int)num77 ^ (int)num68)));
                    uint num86 = numArray1[(int)index2 + 5 & 15] + (uint)((int)numArray1[num80 & 15] + ((int)(numArray1[num84 & 15] >> 3) ^ (int)BitUtil.RotateRight(numArray1[num84 & 15], 7) ^ (int)BitUtil.RotateRight(numArray1[num84 & 15], 18)) + ((int)(numArray1[num48 + 4 & 15] >> 10) ^ (int)BitUtil.RotateRight(numArray1[num48 + 4 & 15], 17) ^ (int)BitUtil.RotateRight(numArray1[num48 + 4 & 15], 19)));
                    numArray1[(int)index2 + 5 & 15] = num86;
                    uint num87 = (uint)((int)num56 + (int)num86 + (int)HWID.SHA256Magic[(int)index2 + 5] + ((int)num75 & (int)num10 ^ (int)num65 & ~(int)num10) + ((int)BitUtil.RotateRight(num10, 6) ^ (int)BitUtil.RotateRight(num10, 11) ^ (int)BitUtil.RotateRight(num10, 25)));
                    num9 = num87 + num57;
                    int num88 = num80 + 1;
                    int num89 = (int)BitUtil.LOBYTE(num58 + 4);
                    num7 = (uint)((int)num87 + ((int)BitUtil.RotateRight(num85, 2) ^ (int)BitUtil.RotateRight(num85, 13) ^ (int)BitUtil.RotateRight(num85, 22)) + ((int)num85 & (int)num77 ^ (int)num68 & ((int)num85 ^ (int)num77)));
                    uint num90 = numArray1[(int)index2 + 6 & 15] + (uint)((int)numArray1[num88 & 15] + ((int)(numArray1[num89 & 15] >> 3) ^ (int)BitUtil.RotateRight(numArray1[num89 & 15], 7) ^ (int)BitUtil.RotateRight(numArray1[num89 & 15], 18)) + ((int)(numArray1[num48 + 5 & 15] >> 10) ^ (int)BitUtil.RotateRight(numArray1[num48 + 5 & 15], 17) ^ (int)BitUtil.RotateRight(numArray1[num48 + 5 & 15], 19)));
                    numArray1[(int)index2 + 6 & 15] = num90;
                    uint num91 = (uint)((int)num65 + (int)num90 + (int)HWID.SHA256Magic[(int)index2 + 6] + ((int)num9 & (int)num10 ^ (int)num75 & ~(int)num9) + ((int)BitUtil.RotateRight(num9, 6) ^ (int)BitUtil.RotateRight(num9, 11) ^ (int)BitUtil.RotateRight(num9, 25)));
                    num8 = num91 + num68;
                    int num92 = num88 + 1;
                    int num93 = num58 + 5;
                    int num94 = num93;
                    int num95 = num48 + 6;
                    int num96 = num95;
                    num11 = (uint)((int)num91 + ((int)BitUtil.RotateRight(num7, 2) ^ (int)BitUtil.RotateRight(num7, 13) ^ (int)BitUtil.RotateRight(num7, 22)) + ((int)num7 & (int)num85 ^ (int)num77 & ((int)num7 ^ (int)num85)));
                    uint num97 = numArray1[(int)index2 + 7 & 15] + (uint)((int)numArray1[num92 & 15] + ((int)(numArray1[num93 & 15] >> 3) ^ (int)BitUtil.RotateRight(numArray1[num93 & 15], 7) ^ (int)BitUtil.RotateRight(numArray1[num93 & 15], 18)) + ((int)(numArray1[num95 & 15] >> 10) ^ (int)BitUtil.RotateRight(numArray1[num95 & 15], 17) ^ (int)BitUtil.RotateRight(numArray1[num95 & 15], 19)));
                    numArray1[(int)index2 + 7 & 15] = num97;
                    uint num98 = (uint)((int)num97 + (int)HWID.SHA256Magic[(int)index2 + 7] + ((int)num8 & (int)num9 ^ (int)num10 & ~(int)num8) + ((int)BitUtil.RotateRight(num8, 6) ^ (int)BitUtil.RotateRight(num8, 11) ^ (int)BitUtil.RotateRight(num8, 25)));
                    num2 = num7;
                    uint num99 = num75 + num98;
                    num12 = num99 + num77;
                    num5 = num85;
                    index2 += 8U;
                    num1 = (uint)((int)num99 + ((int)BitUtil.RotateRight(num11, 2) ^ (int)BitUtil.RotateRight(num11, 13) ^ (int)BitUtil.RotateRight(num11, 22)) + ((int)num11 & (int)num7 ^ (int)num85 & ((int)num11 ^ (int)num7)));
                    ch2 = (char)(num94 + 1);
                    num38 = num92 + 1;
                    ch1 = (char)(num95 + 1);
                    num13 = num1;
                    num40 = num94 + 1;
                    num39 = num96 + 1;
                }
                while (index2 < 64U);
            }
            uint num100 = num1 + num6;
            src[0] = num100;
            src[3] += num5;
            src[2] += num2;
            src[1] += num11;
            src[4] += num12;
            src[5] += num8;
            src[6] += num9;
            src[7] += num10;
            Buffer.BlockCopy((Array)src, 0, (Array)SHA256Init, 0, src.Length * 4);
            return numArray1;
        }

        private static uint[] SHA256Final(ref uint[] bSHA256Init)
        {
            uint cbsize = (uint)(64 - ((int)bSHA256Init[9] & 63));
            if (cbsize <= 8U)
                cbsize += 64U;
            byte[] first = new byte[(int)cbsize - 8];
            first[0] = (byte)128;
            uint num1 = bSHA256Init[9] >> 29 | 8U * bSHA256Init[8];
            uint num2 = 8U * bSHA256Init[9];
            byte[] array = ((IEnumerable<byte>)first).Concat<byte>((IEnumerable<byte>)((IEnumerable<byte>)BitConverter.GetBytes(num1)).Reverse<byte>().ToArray<byte>()).Concat<byte>((IEnumerable<byte>)((IEnumerable<byte>)BitConverter.GetBytes(num2)).Reverse<byte>().ToArray<byte>()).ToArray<byte>();
            HWID.SHA256Update(ref bSHA256Init, array, cbsize);
            uint[] numArray = new uint[8];
            int index = 0;
            do
            {
                uint uint32 = BitConverter.ToUInt32(((IEnumerable<byte>)BitConverter.GetBytes(bSHA256Init[index])).Reverse<byte>().ToArray<byte>(), 0);
                numArray[index] = uint32;
                ++index;
            }
            while (index < 8);
            bSHA256Init[8] = 0U;
            bSHA256Init[9] = 0U;
            bSHA256Init[0] = 1779033703U;
            bSHA256Init[1] = 3144134277U;
            bSHA256Init[2] = 1013904242U;
            bSHA256Init[3] = 2773480762U;
            bSHA256Init[4] = 1359893119U;
            bSHA256Init[5] = 2600822924U;
            bSHA256Init[6] = 528734635U;
            bSHA256Init[7] = 1541459225U;
            int num3 = 16;
            int num4 = 0;
            do
            {
                bSHA256Init[10 + num4] = 0U;
                ++num4;
                --num3;
            }
            while (num3 != 0);
            return numArray;
        }
    }
}