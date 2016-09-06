// Decompiled with JetBrains decompiler
// Type: Conquer.C3.Vertex
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Conquer.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Conquer.C3
{
  public class Vertex
  {
    public VertexType Type;
    public Vector3 Position;
    public Vector3 Normal;
    public Vector3 Unknown1;
    public Vector3 Unknown2;
    public Vector2 UV;
    public int Color;
    public Vector2 Index;
    public Vector2 Weight;

    public static implicit operator VertexPositionNormalTexture(Vertex vertex)
    {
      return new VertexPositionNormalTexture(vertex.Position, vertex.Normal, vertex.UV);
    }

    public static Vertex ReadVertex2(BinaryReader data)
    {
      return new Vertex()
      {
        Type = VertexType.Vertex2,
        Position = new Vector3(data.ReadSingle(), data.ReadSingle(), data.ReadSingle()),
        Normal = new Vector3(data.ReadSingle(), data.ReadSingle(), data.ReadSingle()),
        Unknown1 = new Vector3(data.ReadSingle(), data.ReadSingle(), data.ReadSingle()),
        Unknown2 = new Vector3(data.ReadSingle(), data.ReadSingle(), data.ReadSingle()),
        UV = new Vector2(data.ReadSingle(), data.ReadSingle()),
        Color = data.ReadInt32(),
        Index = new Vector2(data.ReadSingle(), data.ReadSingle()),
        Weight = new Vector2(data.ReadSingle(), data.ReadSingle())
      };
    }

    public static Vertex ReadVertex3(BinaryReader data)
    {
      return new Vertex()
      {
        Type = VertexType.Vertex3,
        Position = new Vector3(data.ReadSingle(), data.ReadSingle(), data.ReadSingle()),
        UV = new Vector2(data.ReadSingle(), data.ReadSingle()),
        Color = data.ReadInt32(),
        Index = new Vector2(data.ReadSingle(), data.ReadSingle()),
        Weight = new Vector2(data.ReadSingle(), data.ReadSingle())
      };
    }

    public void WriteVertex2(BinaryWriter data)
    {
      data.Write(this.Position.X);
      data.Write(this.Position.Y);
      data.Write(this.Position.Z);
      data.Write(this.Normal.X);
      data.Write(this.Normal.Y);
      data.Write(this.Normal.Z);
      data.Write(this.Unknown1.X);
      data.Write(this.Unknown1.Y);
      data.Write(this.Unknown1.Z);
      data.Write(this.Unknown2.X);
      data.Write(this.Unknown2.Y);
      data.Write(this.Unknown2.Z);
      data.Write(this.UV.X);
      data.Write(this.UV.Y);
      data.Write(this.Color);
      data.Write(this.Index.X);
      data.Write(this.Index.Y);
      data.Write(this.Weight.X);
      data.Write(this.Weight.Y);
    }

    public void WriteVertex3(BinaryWriter data)
    {
      data.Write(this.Position.X);
      data.Write(this.Position.Y);
      data.Write(this.Position.Z);
      data.Write(this.UV.X);
      data.Write(this.UV.Y);
      data.Write(this.Color);
      data.Write(this.Index.X);
      data.Write(this.Index.Y);
      data.Write(this.Weight.X);
      data.Write(this.Weight.Y);
    }
  }
}
