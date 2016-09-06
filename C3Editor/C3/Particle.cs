// Decompiled with JetBrains decompiler
// Type: Conquer.C3.Particle
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Conquer.C3
{
  public class Particle
  {
    public List<ParticleVertex> Vertexes = new List<ParticleVertex>();
    public List<short> Indexes = new List<short>();
    public List<ParticleFrame> Frames = new List<ParticleFrame>();
    public int Header;
    public string Name;
    public string TextureName;
    public int RowCount;
    public int ParticleCount;
    public int FrameCount;

    public static Particle ReadParticle(BinaryReader reader)
    {
      Particle particle = new Particle();
      particle.Header = reader.ReadInt32();
      particle.Name = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadInt32()));
      particle.TextureName = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadInt32()));
      particle.RowCount = reader.ReadInt32();
      particle.ParticleCount = reader.ReadInt32();
      particle.FrameCount = reader.ReadInt32();
      for (int index = 0; index < particle.FrameCount; ++index)
      {
        reader.ReadInt32();
        particle.Frames.Add(ParticleFrame.ReadParticleFrame(reader));
      }
      return particle;
    }

    public void WriteLine(BinaryWriter writer)
    {
      writer.Write(this.Header);
      writer.Write(this.Name.Length);
      writer.Write(Encoding.ASCII.GetBytes(this.Name));
      writer.Write(this.TextureName.Length);
      writer.Write(Encoding.ASCII.GetBytes(this.TextureName));
      writer.Write(this.RowCount);
      writer.Write(this.ParticleCount);
      writer.Write(this.FrameCount);
    }
  }
}
