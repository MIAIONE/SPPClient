// Decompiled with JetBrains decompiler
// Type: HwidGetCurrentEx.BitUtil
// Assembly: HwidGetCurrentEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 200C1AD7-2186-49E5-9EB2-5AB7013ECA80 Assembly location: D:\downloads\Programs\HwidGetCurrentEx.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace HWIDEx
{
    internal static class BitUtil
    {
        public static string ReadNullTerminatedAnsiString(byte[] buffer, int offset)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (char ch = (char)buffer[offset]; ch > char.MinValue; ch = (char)buffer[offset])
            {
                stringBuilder.Append(ch);
                ++offset;
            }
            return stringBuilder.ToString();
        }

        public static byte[] StrToByteArray(string str)
        {
            Dictionary<string, byte> dictionary = new Dictionary<string, byte>();
            for (int index = 0; index <= (int)byte.MaxValue; ++index)
                dictionary.Add(index.ToString("X2"), (byte)index);
            List<byte> byteList = new List<byte>();
            for (int startIndex = 0; startIndex < str.Length; startIndex += 2)
                byteList.Add(dictionary[str.Substring(startIndex, 2)]);
            return byteList.ToArray();
        }

        public static ulong array2ulong(byte[] bytes, int start, int length)
        {
            bytes = ((IEnumerable<byte>)bytes).Skip<byte>(start).Take<byte>(length).ToArray<byte>();
            ulong num1 = 0;
            foreach (byte num2 in bytes)
                num1 = num1 * 256UL + (ulong)num2;
            return num1;
        }

        public static T[] Concats<T>(this T[] array1, params T[] array2) => BitUtil.ConcatArray<T>(array1, array2);

        public static T[] ConcatArray<T>(params T[][] arrays)
        {
            int index1;
            int length;
            for (length = index1 = 0; index1 < arrays.Length; ++index1)
                length += arrays[index1].Length;
            T[] objArray = new T[length];
            int index2;
            for (int index3 = index2 = 0; index2 < arrays.Length; ++index2)
            {
                arrays[index2].CopyTo((Array)objArray, index3);
                index3 += arrays[index2].Length;
            }
            return objArray;
        }

        public static byte LOBYTE(int a) => (byte)((uint)(short)a & (uint)byte.MaxValue);

        public static short MAKEWORD(byte a, byte b) => (short)((int)(byte)((uint)a & (uint)byte.MaxValue) | (int)(byte)((uint)b & (uint)byte.MaxValue) << 8);

        public static byte LOBYTE(short a) => (byte)((uint)a & (uint)byte.MaxValue);

        public static byte HIBYTE(short a) => (byte)((uint)a >> 8);

        public static int MAKELONG(short a, short b) => (int)a & (int)ushort.MaxValue | ((int)b & (int)ushort.MaxValue) << 16;

        public static short HIWORD(int a) => (short)(a >> 16);

        public static short LOWORD(int a) => (short)(a & (int)ushort.MaxValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(uint value, int offset) => value << offset | value >> 32 - offset;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateLeft64(ulong value, int offset) => value << offset | value >> 64 - offset;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateRight(uint value, int offset) => value >> offset | value << 32 - offset;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateRight64(ulong value, int offset) => value >> offset | value << 64 - offset;

        public static int HIDWORD(long intValue) => Convert.ToInt32(intValue >> 32);

        public static int LODWORD(long intValue) => Convert.ToInt32(intValue << 32 >> 32);

        public static short PAIR(sbyte high, sbyte low) => (short)((int)high << 8 | (int)(byte)low);

        public static int PAIR(short high, int low) => (int)high << 16 | (int)(ushort)low;

        public static long PAIR(int high, long low) => (long)high << 32 | (long)(uint)low;

        public static ushort PAIR(byte high, ushort low) => (ushort)((uint)high << 8 | (uint)(byte)low);

        public static uint PAIR(ushort high, uint low) => (uint)high << 16 | (uint)(ushort)low;

        public static ulong PAIR(uint high, ulong low) => (ulong)high << 32 | (ulong)(uint)low;

        public static int adc(uint first, uint second, ref uint carry)
        {
            uint carry1 = 0;
            if (carry == 0U)
            {
                uint num = first + second;
                carry = num >= first || num >= second ? 0U : 1U;
                return (int)num;
            }
            uint num1 = (uint)BitUtil.adc(first, second, ref carry1);
            if (carry > 0U)
            {
                ++num1;
                carry1 |= num1 == 0U ? 1U : 0U;
            }
            carry = carry1;
            return (int)num1;
        }
    }
}