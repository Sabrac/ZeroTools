// Decompiled with JetBrains decompiler
// Type: Conquer.C3.ShapeMotion
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace Conquer.C3
{
  public class ShapeMotion
  {
    public List<Matrix> Transforms = new List<Matrix>();
    public int Header;

    public static ShapeMotion ReadShapeMotion(BinaryReader reader)
    {
      ShapeMotion shapeMotion = new ShapeMotion();
      reader.ReadInt32();
      shapeMotion.Header = reader.ReadInt32();
      int num = reader.ReadInt32();
      for (int index = 0; index < num; ++index)
        shapeMotion.Transforms.Add(new Matrix(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
      return shapeMotion;
    }

    public void WriteShapeMotion(BinaryWriter writer)
    {
      writer.Write(1414483283);
      writer.Write(this.Header);
      writer.Write(this.Transforms.Count);
      foreach (Matrix transform in this.Transforms)
      {
        writer.Write(transform.M11);
        writer.Write(transform.M12);
        writer.Write(transform.M13);
        writer.Write(transform.M14);
        writer.Write(transform.M21);
        writer.Write(transform.M22);
        writer.Write(transform.M23);
        writer.Write(transform.M24);
        writer.Write(transform.M31);
        writer.Write(transform.M32);
        writer.Write(transform.M33);
        writer.Write(transform.M34);
        writer.Write(transform.M41);
        writer.Write(transform.M42);
        writer.Write(transform.M43);
        writer.Write(transform.M44);
      }
    }
  }
}
