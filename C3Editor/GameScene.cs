// Decompiled with JetBrains decompiler
// Type: Conquer.GameScene
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Conquer
{
  public class GameScene
  {
    public List<GameModel> Models = new List<GameModel>();

    public void LoadModel(string path)
    {
      if (File.Exists(path))
      {
        this.Models.Add(new GameModel(path, "null"));
      }
      else
      {
        int num = (int) MessageBox.Show("Error: Path is not valid: " + path);
      }
    }
  }
}
