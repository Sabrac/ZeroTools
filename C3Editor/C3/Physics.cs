// Decompiled with JetBrains decompiler
// Type: Conquer.C3.Physics
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Conquer.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Conquer.C3
{
  public class Physics
  {
    public BlendState BlendMode = BlendState.NonPremultiplied;
    public bool IsShown = true;
    public bool WriteSID = false;
    public int FrameIndex;
    public C3PartType Type;
    public int Unknown;
    public string Name;
    public int BlendCount;
    public Vertex[] NormalVertexes;
    public Vertex[] AlphaVertexes;
    public short[] NormalIndexes;
    public short[] AlphaIndexes;
    public string TextureName;
    public Vector3 Min_Bound;
    public Vector3 Max_Bound;
    public int TextureRowCount;
    public Matrix World;
    public Frame[] Alphas;
    public Frame[] Draws;
    public Frame[] Teks;
    public int Step;
    public Vector2 UV_Step;
    public Motion Motion;

    public static Physics ReadPhysics(BinaryReader reader, C3PartType type)
    {
      Physics physics = new Physics();
      physics.Type = type;
      physics.Unknown = reader.ReadInt32();
      physics.Name = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadInt32()));
      switch (physics.Type)
      {
        case C3PartType.PHY:
          physics.BlendCount = reader.ReadInt32();
          physics.NormalVertexes = new Vertex[reader.ReadInt32()];
          physics.AlphaVertexes = new Vertex[reader.ReadInt32()];
          for (int index = 0; index < physics.NormalVertexes.Length; ++index)
            physics.NormalVertexes[index] = Vertex.ReadVertex2(reader);
          for (int index = 0; index < physics.AlphaVertexes.Length; ++index)
            physics.AlphaVertexes[index] = Vertex.ReadVertex2(reader);
          physics.NormalIndexes = new short[reader.ReadInt32() * 3];
          physics.AlphaIndexes = new short[reader.ReadInt32() * 3];
          for (int index = 0; index < physics.NormalIndexes.Length; ++index)
            physics.NormalIndexes[index] = reader.ReadInt16();
          for (int index = 0; index < physics.AlphaIndexes.Length; ++index)
            physics.AlphaIndexes[index] = reader.ReadInt16();
          physics.TextureName = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadInt32()));
          physics.Min_Bound = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
          physics.Max_Bound = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
          physics.World = new Matrix(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
          physics.TextureRowCount = reader.ReadInt32();
          physics.Alphas = new Frame[reader.ReadInt32()];
          for (int index = 0; index < ((IEnumerable<Frame>) physics.Alphas).Count<Frame>(); ++index)
            physics.Alphas[index] = Frame.ReadFrame(reader);
          physics.Draws = new Frame[reader.ReadInt32()];
          for (int index = 0; index < ((IEnumerable<Frame>) physics.Draws).Count<Frame>(); ++index)
            physics.Draws[index] = Frame.ReadFrame(reader);
          physics.Teks = new Frame[reader.ReadInt32()];
          for (int index = 0; index < ((IEnumerable<Frame>) physics.Teks).Count<Frame>(); ++index)
            physics.Teks[index] = Frame.ReadFrame(reader);
          if (reader.BaseStream.Position + 11L < reader.BaseStream.Length)
          {
            physics.Step = reader.ReadInt32();
            if (physics.Step == 1346720851)
              physics.UV_Step = new Vector2(reader.ReadSingle(), reader.ReadSingle());
          }
          if (reader.BaseStream.Position + 4L < reader.BaseStream.Length)
          {
            int num = reader.ReadInt32();
            physics.WriteSID = num == 1145656114;
            if (!physics.WriteSID)
              reader.BaseStream.Position -= 4L;
            break;
          }
          break;
        case C3PartType.PHY3:
          throw new NotImplementedException("PHY3 not yet implemented");
        case C3PartType.PHY4:
          physics.BlendCount = reader.ReadInt32();
          physics.NormalVertexes = new Vertex[reader.ReadInt32()];
          physics.AlphaVertexes = new Vertex[reader.ReadInt32()];
          for (int index = 0; index < physics.NormalVertexes.Length; ++index)
            physics.NormalVertexes[index] = Vertex.ReadVertex3(reader);
          for (int index = 0; index < physics.AlphaVertexes.Length; ++index)
            physics.AlphaVertexes[index] = Vertex.ReadVertex3(reader);
          physics.NormalIndexes = new short[reader.ReadInt32() * 3];
          physics.AlphaIndexes = new short[reader.ReadInt32() * 3];
          for (int index = 0; index < physics.NormalIndexes.Length; ++index)
            physics.NormalIndexes[index] = reader.ReadInt16();
          for (int index = 0; index < physics.AlphaIndexes.Length; ++index)
            physics.AlphaIndexes[index] = reader.ReadInt16();
          physics.TextureName = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadInt32()));
          physics.Min_Bound = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
          physics.Max_Bound = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
          physics.World = new Matrix(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
          physics.TextureRowCount = reader.ReadInt32();
          physics.Alphas = new Frame[reader.ReadInt32()];
          for (int index = 0; index < ((IEnumerable<Frame>) physics.Alphas).Count<Frame>(); ++index)
            physics.Alphas[index] = Frame.ReadFrame(reader);
          physics.Draws = new Frame[reader.ReadInt32()];
          for (int index = 0; index < ((IEnumerable<Frame>) physics.Draws).Count<Frame>(); ++index)
            physics.Draws[index] = Frame.ReadFrame(reader);
          physics.Teks = new Frame[reader.ReadInt32()];
          for (int index = 0; index < ((IEnumerable<Frame>) physics.Teks).Count<Frame>(); ++index)
            physics.Teks[index] = Frame.ReadFrame(reader);
          if (reader.BaseStream.Position + 11L < reader.BaseStream.Length)
          {
            physics.Step = reader.ReadInt32();
            if (physics.Step == 1346720851)
              physics.UV_Step = new Vector2(reader.ReadSingle(), reader.ReadSingle());
          }
          if (reader.BaseStream.Position + 4L < reader.BaseStream.Length)
          {
            int num = reader.ReadInt32();
            physics.WriteSID = num == 1145656114;
            if (!physics.WriteSID)
              reader.BaseStream.Position -= 4L;
            break;
          }
          break;
      }
      if (reader.ReadInt32() == 1230262093)
      {
        if (reader.BaseStream.Position + 16L < reader.BaseStream.Length)
          physics.Motion = Motion.ReadMotion(reader);
      }
      else
        reader.BaseStream.Position -= 4L;
      return physics;
    }

    public void WritePhysics(BinaryWriter writer)
    {
      writer.Write((int) this.Type);
      writer.Write(this.Unknown);
      writer.Write(this.Name.Length);
      writer.Write(Encoding.ASCII.GetBytes(this.Name));
      switch (this.Type)
      {
        case C3PartType.PHY4:
          writer.Write(this.BlendCount);
          writer.Write(this.NormalVertexes.Length);
          writer.Write(this.AlphaVertexes.Length);
          foreach (Vertex normalVertex in this.NormalVertexes)
            normalVertex.WriteVertex3(writer);
          foreach (Vertex alphaVertex in this.AlphaVertexes)
            alphaVertex.WriteVertex3(writer);
          writer.Write(this.NormalIndexes.Length / 3);
          writer.Write(this.AlphaIndexes.Length / 3);
          foreach (short normalIndex in this.NormalIndexes)
            writer.Write(normalIndex);
          foreach (short alphaIndex in this.AlphaIndexes)
            writer.Write(alphaIndex);
          writer.Write(this.TextureName.Length);
          writer.Write(Encoding.ASCII.GetBytes(this.TextureName));
          writer.Write(this.Min_Bound.X);
          writer.Write(this.Min_Bound.Y);
          writer.Write(this.Min_Bound.Z);
          writer.Write(this.Max_Bound.X);
          writer.Write(this.Max_Bound.Y);
          writer.Write(this.Max_Bound.Z);
          writer.Write(this.World.M11);
          writer.Write(this.World.M12);
          writer.Write(this.World.M13);
          writer.Write(this.World.M14);
          writer.Write(this.World.M21);
          writer.Write(this.World.M22);
          writer.Write(this.World.M23);
          writer.Write(this.World.M24);
          writer.Write(this.World.M31);
          writer.Write(this.World.M32);
          writer.Write(this.World.M33);
          writer.Write(this.World.M34);
          writer.Write(this.World.M41);
          writer.Write(this.World.M42);
          writer.Write(this.World.M43);
          writer.Write(this.World.M44);
          writer.Write(this.TextureRowCount);
          if (this.Alphas != null)
          {
            writer.Write(this.Alphas.Length);
            foreach (Frame alpha in this.Alphas)
              alpha.WriteFrame(writer);
          }
          else
            writer.Write(0);
          if (this.Draws != null)
          {
            writer.Write(this.Draws.Length);
            foreach (Frame draw in this.Draws)
              draw.WriteFrame(writer);
          }
          else
            writer.Write(0);
          if (this.Teks != null)
          {
            writer.Write(this.Teks.Length);
            foreach (Frame draw in this.Draws)
              draw.WriteFrame(writer);
          }
          else
            writer.Write(0);
          if (this.Step > 0)
          {
            writer.Write(this.Step);
            if (this.Step == 1346720851)
            {
              writer.Write(this.UV_Step.X);
              writer.Write(this.UV_Step.Y);
            }
          }
          if (this.WriteSID)
            writer.Write(1145656114);
          if (this.Motion == null)
            break;
          this.Motion.WriteMotion(writer);
          break;
      }
    }
  }
}
