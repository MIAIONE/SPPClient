// Decompiled with JetBrains decompiler
// Type: HwidGetCurrentEx.StructHWID
// Assembly: HwidGetCurrentEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 200C1AD7-2186-49E5-9EB2-5AB7013ECA80 Assembly location: D:\downloads\Programs\HwidGetCurrentEx.dll

namespace HWIDEx
{
    public class StructHWID
    {
        public int cbsize;
        public short BiosHwidCount;
        public short MemoryHwidCount;
        public short CpuHwidCount;
        public short NdisHwidCount;
        public short HWProfileCount;
        public short GuidHwidCount;
        public short PcmciaHwidCount;
        public short BthPortHwidCount;
        public short ScsiAdapterHwidCount;
        public short DisplayHwidCount;
        public short DiskHwidCount;
        public short HdcHwidCount;
        public short WwanHwidCount;
        public short CdromHwidCount;
        public byte[] BiosHwidBlock;
        public byte[] MemoryHwidBlock;
        public byte[] CpuHwidBlock;
        public byte[] NdisHwidBlock;
        public byte[] HWProfileBlock;
        public byte[] GuidHwidBlock;
        public byte[] PcmciaHwidBlock;
        public byte[] BthPortHwidBlock;
        public byte[] ScsiAdapterHwidBlock;
        public byte[] DisplayHwidBlock;
        public byte[] DiskHwidBlock;
        public byte[] HdcHwidBlock;
        public byte[] WwanHwidBlock;
        public byte[] CdromHwidBlock;
    }
}