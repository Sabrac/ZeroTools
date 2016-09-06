// Decompiled with JetBrains decompiler
// Type: Conquer.C3.Frame
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using System.IO;

namespace Conquer.C3
{
  public class Frame
  {
    public int K;
    public float F;
    public int B;
    public int N;

    public static Frame ReadFrame(BinaryReader reader)
    {
      return new Frame()
      {
        K = reader.ReadInt32(),
        F = reader.ReadSingle(),
        B = reader.ReadInt32(),
        N = reader.ReadInt32()
      };
    }

    public void WriteFrame(BinaryWriter writer)
    {
      writer.Write(this.K);
      writer.Write(this.F);
      writer.Write(this.B);
      writer.Write(this.N);
    }
  }
}
