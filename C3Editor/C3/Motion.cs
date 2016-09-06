// Decompiled with JetBrains decompiler
// Type: Conquer.C3.Motion
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Conquer.Enum;
using System.Collections.Generic;
using System.IO;

namespace Conquer.C3
{
  public class Motion
  {
    public List<KeyFrame> KeyFrames = new List<KeyFrame>();
    public List<float> Morphs = new List<float>();
    public int Unknown;
    public int BoneCount;
    public int FrameCount;
    public KeyFrameType FrameType;

    public static Motion ReadMotion(BinaryReader reader)
    {
      Motion motion = new Motion();
      motion.Unknown = reader.ReadInt32();
      motion.BoneCount = reader.ReadInt32();
      motion.FrameCount = reader.ReadInt32();
      motion.FrameType = (KeyFrameType) reader.ReadInt32();
      switch (motion.FrameType)
      {
        case KeyFrameType.KKEY:
          int num1 = reader.ReadInt32();
          for (int index = 0; index < num1; ++index)
            motion.KeyFrames.Add(KeyFrame.ReadKKeyFrame(reader, motion.BoneCount));
          break;
        case KeyFrameType.XKEY:
          int num2 = reader.ReadInt32();
          for (int index = 0; index < num2; ++index)
            motion.KeyFrames.Add(KeyFrame.ReadXKeyFrame(reader, motion.BoneCount));
          break;
      }
      uint num3 = reader.ReadUInt32();
      for (int index1 = 0; (long) index1 < (long) num3; ++index1)
      {
        for (int index2 = 0; index2 < motion.BoneCount; ++index2)
          motion.Morphs.Add(reader.ReadSingle());
      }
      return motion;
    }

    public void WriteMotion(BinaryWriter writer)
    {
      writer.Write(1230262093);
      writer.Write(this.Unknown);
      writer.Write(this.BoneCount);
      writer.Write(this.FrameCount);
      writer.Write((int) this.FrameType);
      writer.Write(this.KeyFrames.Count);
      foreach (KeyFrame keyFrame in this.KeyFrames)
      {
        switch (this.FrameType)
        {
          case KeyFrameType.KKEY:
            keyFrame.WriteKKeyFrame(writer);
            continue;
          case KeyFrameType.XKEY:
            keyFrame.WriteXKeyFrame(writer);
            continue;
          default:
            continue;
        }
      }
      writer.Write(this.Morphs.Count);
      foreach (float morph in this.Morphs)
        writer.Write(morph);
    }
  }
}
