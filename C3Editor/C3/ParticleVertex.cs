// Decompiled with JetBrains decompiler
// Type: Conquer.C3.ParticleVertex
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Microsoft.Xna.Framework;
using System.IO;

namespace Conquer.C3
{
  public class ParticleVertex
  {
    public Vector3 Position;
    public int Color;
    public Vector2 UV;

    public static ParticleVertex ReadParticleVertex(BinaryReader reader)
    {
      return new ParticleVertex()
      {
        Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
        Color = reader.ReadInt32(),
        UV = new Vector2(reader.ReadSingle(), reader.ReadSingle())
      };
    }

    public void WriteParticleVertex(BinaryWriter writer)
    {
      writer.Write(this.Position.X);
      writer.Write(this.Position.Y);
      writer.Write(this.Position.Z);
      writer.Write(this.Color);
      writer.Write(this.UV.X);
      writer.Write(this.UV.Y);
    }
  }
}
