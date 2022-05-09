// Decompiled with JetBrains decompiler
// Type: HwidGetCurrentEx.MinWinDef
// Assembly: HwidGetCurrentEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 200C1AD7-2186-49E5-9EB2-5AB7013ECA80 Assembly location: D:\downloads\Programs\HwidGetCurrentEx.dll

using System;

namespace HWIDEx
{
    public static class MinWinDef
    {
        internal static Func<object, object, object> MAKEWORD = (Func<object, object, object>)((a, b) => (object)(ushort)((uint)(byte)((ulong)a & (ulong)byte.MaxValue) | (uint)(byte)((ulong)b & (ulong)byte.MaxValue) << 8));
        internal static Func<object, object, object> MAKELONG = (Func<object, object, object>)((a, b) => (object)(ulong)((int)(ushort)((ulong)a & (ulong)byte.MaxValue) | (int)(byte)((ulong)b & (ulong)byte.MaxValue) << 8));
        internal static Func<object, object> LOWORD = (Func<object, object>)(l => (object)(ushort)((ulong)l & (ulong)ushort.MaxValue));
        internal static Func<object, object> HIWORD = (Func<object, object>)(l => (object)(ushort)((ulong)l >> 16 & (ulong)ushort.MaxValue));
        internal static Func<object, object> LOBYTE = (Func<object, object>)(w => (object)(byte)((ulong)w & (ulong)byte.MaxValue));
        internal static Func<object, object> HIBYTE = (Func<object, object>)(w => (object)(byte)((ulong)w >> 8 & (ulong)byte.MaxValue));
        internal static Func<object, object> GET_WHEEL_DELTA_WPARAM = (Func<object, object>)(wParam => (object)(short)MinWinDef.HIWORD(wParam));
        internal static Func<object, object> GET_KEYSTATE_WPARAM = (Func<object, object>)(wParam => MinWinDef.LOWORD(wParam));
        internal static Func<object, object> GET_NCHITTEST_WPARAM = (Func<object, object>)(wParam => (object)(short)MinWinDef.LOWORD(wParam));
        internal static Func<object, object> GET_XBUTTON_WPARAM = (Func<object, object>)(wParam => MinWinDef.HIWORD(wParam));
    }
}