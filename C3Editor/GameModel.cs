// Decompiled with JetBrains decompiler
// Type: Conquer.GameModel
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Conquer.C3;
using Conquer.Enum;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Conquer
{
  public class GameModel
  {
    public List<Shape> Shapes = new List<Shape>();
    public List<Physics> PhysicalObjects = new List<Physics>();
    public List<Motion> Motions = new List<Motion>();
    public int ID;
    public string Name;
    public string Header;
    public Texture2D Texture;

    public GameModel(string meshPath, string texturePath = "null")
    {
      using (BinaryReader reader = new BinaryReader((Stream) File.OpenRead(meshPath)))
      {
        this.Header = Encoding.ASCII.GetString(reader.ReadBytes(16));
        C3PartType type = C3PartType.START;
        while (type != C3PartType.EXIT && reader.BaseStream.Position + 4L <= reader.BaseStream.Length)
        {
          type = (C3PartType) reader.ReadInt32();
          Console.WriteLine("Loading: " + (object) type);
          switch (type)
          {
            case C3PartType.PHY4:
            case C3PartType.PHY:
            case C3PartType.PHY3:
              this.PhysicalObjects.Add(Physics.ReadPhysics(reader, type));
              break;
            case C3PartType.MOTI:
              this.Motions.Add(Motion.ReadMotion(reader));
              break;
            case C3PartType.SHAP:
              this.Shapes.Add(Shape.ReadShape(reader));
              break;
            default:
              reader.BaseStream.Position -= 4L;
              int num = (int) MessageBox.Show("Warning: This file was not completely loaded. Please avoid exporting this model or it may become corrupted.");
              type = C3PartType.EXIT;
              break;
          }
        }
        reader.Close();
      }
      meshPath = meshPath.Replace('\\', '/');
      int num1 = meshPath.LastIndexOf('/');
      string s = meshPath.Substring(num1 + 1, meshPath.Length - num1 - 4);
      int.TryParse(s, out this.ID);
      if (File.Exists(meshPath.Substring(0, meshPath.Length - 3) + ".dds"))
        DDSLib.DDSFromFile(meshPath.Substring(0, meshPath.Length - 3) + ".dds", Settings.Graphics, false, out this.Texture);
      else if (File.Exists(Settings.Conquer_Path + "c3/texture/" + s + ".dds"))
        DDSLib.DDSFromFile(Settings.Conquer_Path + "c3/texture/" + s + ".dds", Settings.Graphics, false, out this.Texture);
      else if (File.Exists(Settings.Conquer_Path + "c3/texture/" + (object) (this.ID / 10 * 10) + ".dds"))
        DDSLib.DDSFromFile(Settings.Conquer_Path + "c3/texture/" + (object) (this.ID / 10 * 10) + ".dds", Settings.Graphics, false, out this.Texture);
      this.Name = this.ID.ToString();
      foreach (Physics physicalObject in this.PhysicalObjects)
        Settings.GUI.AddPhysicalObject(this.Name, physicalObject);
      foreach (Shape shape in this.Shapes)
        Settings.GUI.AddShape(this.Name, shape);
    }

    public void WriteModel(BinaryWriter writer)
    {
      writer.Write(Encoding.ASCII.GetBytes(this.Header));
      foreach (Physics physicalObject in this.PhysicalObjects)
        physicalObject.WritePhysics(writer);
      foreach (Motion motion in this.Motions)
        motion.WriteMotion(writer);
      writer.Close();
    }
  }
}
