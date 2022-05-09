// Decompiled with JetBrains decompiler
// Type: VRSAVaultSignPKCS.VRSA
// Assembly: VRSAVaultSignPKCS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835E1F41-447B-4B59-919B-3F453537ACCB
// Assembly location: D:\downloads\Programs\VRSAVaultSignPKCS-cleaned.dll

using System;
using System.Runtime.InteropServices;

namespace HWIDEx
{
  public class VRSA
  {
    public static byte[] SignPKCS(byte[] HashArray)
    {
      byte[] DST = new byte[256];
      uint outSize = 256;
      DLLFromMemory dllFromMemory = new DLLFromMemory(SPPClient.Properties.Resources.HWID);
      byte[] numArray;
      if (((VRSA.VRSAVaultSignPKCS86) Marshal.GetDelegateForFunctionPointer(new IntPtr(dllFromMemory.pCode.ToInt32() + 313463), typeof (VRSA.VRSAVaultSignPKCS86)))(IntPtr.Zero, IntPtr.Zero, HashArray, HashArray.Length, DST, ref outSize) == 0)
      {
        dllFromMemory.Close();
        numArray = DST;
      }
      else
      {
        dllFromMemory.Close();
        numArray = (byte[]) null;
      }
      return numArray;
    }

    [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode)]
    private delegate int VRSAVaultSignPKCS86(
      IntPtr pVbnRsaVault_ModExpPriv_clear,
      IntPtr handle,
      byte[] dwbyte,
      int dwsize,
      byte[] DST,
      ref uint outSize);
  }
}
