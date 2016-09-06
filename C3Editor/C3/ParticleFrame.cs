// Decompiled with JetBrains decompiler
// Type: Conquer.C3.ParticleFrame
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Microsoft.Xna.Framework;
using System;
using System.IO;

namespace Conquer.C3
{
  public class ParticleFrame
  {
    public int ParticleCount;
    public Vector3 Position;
    public float Age;
    public float Size;
    public Matrix Emitter;

    public static ParticleFrame ReadParticleFrame(BinaryReader reader)
    {
      return new ParticleFrame()
      {
        ParticleCount = reader.ReadInt32(),
        Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
        Age = reader.ReadSingle(),
        Size = reader.ReadSingle(),
        Emitter = new Matrix(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle())
      };
    }

    public void WriteFrame(BinaryWriter writer)
    {
      throw new NotImplementedException("Writing ParticleFrames is not yet supported");
    }
  }
}
