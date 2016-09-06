// Decompiled with JetBrains decompiler
// Type: Conquer.C3.Shape
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Conquer.C3
{
  public class Shape
  {
    public List<Line> Lines = new List<Line>();
    public int Header;
    public string Name;
    public string TextureName;
    public int Unknown;
    public ShapeMotion Motion;

    public static Shape ReadShape(BinaryReader reader)
    {
      Shape shape = new Shape();
      shape.Header = reader.ReadInt32();
      shape.Name = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadInt32()));
      int num = reader.ReadInt32();
      for (int index = 0; index < num; ++index)
        shape.Lines.Add(Line.ReadLine(reader));
      shape.TextureName = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadInt32()));
      shape.Unknown = reader.ReadInt32();
      shape.Motion = ShapeMotion.ReadShapeMotion(reader);
      return shape;
    }

    public void WriteShape(BinaryWriter writer)
    {
      writer.Write(1346455635);
      writer.Write(this.Header);
      writer.Write(this.Name.Length);
      writer.Write(Encoding.ASCII.GetBytes(this.Name));
      writer.Write(this.Lines.Count);
      foreach (Line line in this.Lines)
        line.WriteLine(writer);
      writer.Write(this.TextureName.Length);
      writer.Write(Encoding.ASCII.GetBytes(this.TextureName));
      writer.Write(this.Unknown);
      if (this.Motion == null)
        return;
      this.Motion.WriteShapeMotion(writer);
    }
  }
}
