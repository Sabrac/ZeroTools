// Decompiled with JetBrains decompiler
// Type: Conquer.C3.Line
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace Conquer.C3
{
  public class Line
  {
    public List<Vector3> Vectors = new List<Vector3>();

    public static Line ReadLine(BinaryReader reader)
    {
      Line line = new Line();
      int num = reader.ReadInt32();
      for (int index = 0; index < num; ++index)
        line.Vectors.Add(new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
      return line;
    }

    public void WriteLine(BinaryWriter writer)
    {
      writer.Write(this.Vectors.Count);
      foreach (Vector3 vector in this.Vectors)
      {
        writer.Write(vector.X);
        writer.Write(vector.Y);
        writer.Write(vector.Z);
      }
    }
  }
}
