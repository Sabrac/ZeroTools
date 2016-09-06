// Decompiled with JetBrains decompiler
// Type: Conquer.C3.KeyFrame
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace Conquer.C3
{
  public class KeyFrame
  {
    public List<Matrix> Transforms = new List<Matrix>();
    public int Position;

    public static KeyFrame ReadKKeyFrame(BinaryReader reader, int boneCount)
    {
      KeyFrame keyFrame = new KeyFrame();
      keyFrame.Position = reader.ReadInt32();
      for (int index = 0; index < boneCount; ++index)
        keyFrame.Transforms.Add(new Matrix(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
      return keyFrame;
    }

    public static KeyFrame ReadXKeyFrame(BinaryReader reader, int boneCount)
    {
      KeyFrame keyFrame = new KeyFrame();
      keyFrame.Position = (int) reader.ReadInt16();
      for (int index = 0; index < boneCount; ++index)
        keyFrame.Transforms.Add(new Matrix(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), 0.0f, reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), 0.0f, reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), 0.0f, reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), 1f));
      return keyFrame;
    }

    public void WriteKKeyFrame(BinaryWriter writer)
    {
      writer.Write(this.Position);
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

    public void WriteXKeyFrame(BinaryWriter writer)
    {
      writer.Write((short) this.Position);
      foreach (Matrix transform in this.Transforms)
      {
        writer.Write(transform.M11);
        writer.Write(transform.M12);
        writer.Write(transform.M13);
        writer.Write(transform.M21);
        writer.Write(transform.M22);
        writer.Write(transform.M23);
        writer.Write(transform.M31);
        writer.Write(transform.M32);
        writer.Write(transform.M33);
        writer.Write(transform.M41);
        writer.Write(transform.M42);
        writer.Write(transform.M43);
      }
    }
  }
}
