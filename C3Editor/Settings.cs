// Decompiled with JetBrains decompiler
// Type: Conquer.Settings
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Conquer
{
  public static class Settings
  {
    public static GameScene Scene = new GameScene();
    public static int Screen_Width = 1200;
    public static int Screen_Height = 768;
    public static Manager GUI;
    public static GraphicsDevice Graphics;
    public static string Conquer_Path;
    public static string Default_Mesh;
    public static string Default_Texture;
    public static string Gender;

    static Settings()
    {
      if (File.Exists("settings.txt"))
        return;
      using (StreamWriter text = File.CreateText("settings.txt"))
      {
        text.WriteLine("CONQUER_FOLDER=conquer/");
        text.WriteLine("DEFAULT_MESH=NULL");
        text.WriteLine("DEFAULT_TEXTURE=NULL");
        text.WriteLine("LOAD_GENDER=1");
        text.WriteLine("SCREEN_WIDTH=1200");
        text.WriteLine("SCREEN_HEIGHT=768");
        text.Close();
      }
    }

    public static void Read()
    {
      using (StreamReader streamReader = new StreamReader((Stream) File.OpenRead("settings.txt")))
      {
        string str1 = streamReader.ReadToEnd().Replace("\r\n", "\r");
        char[] chArray1 = new char[1]{ '\r' };
        foreach (string str2 in str1.Split(chArray1))
        {
          char[] chArray2 = new char[1]{ '=' };
          string[] strArray = str2.Split(chArray2);
          if (strArray.Length >= 2)
          {
            switch (strArray[0].ToUpper())
            {
              case "CONQUER_FOLDER":
                Settings.Conquer_Path = strArray[1];
                break;
              case "DEFAULT_MESH":
                Settings.Default_Mesh = strArray[1];
                break;
              case "DEFAULT_TEXTURE":
                Settings.Default_Texture = strArray[1];
                break;
              case "LOAD_GENDER":
                Settings.Gender = strArray[1];
                break;
              case "SCREEN_HEIGHT":
                int.TryParse(strArray[1], out Settings.Screen_Height);
                break;
              case "SCREEN_WIDTH":
                int.TryParse(strArray[1], out Settings.Screen_Width);
                break;
            }
          }
        }
        streamReader.Close();
      }
    }

    public static void Save()
    {
      using (StreamWriter streamWriter = new StreamWriter((Stream) File.Open("settings.txt", FileMode.Create)))
      {
        streamWriter.WriteLine("CONQUER_FOLDER=" + Settings.Conquer_Path);
        streamWriter.WriteLine("DEFAULT_MESH=" + Settings.Default_Mesh);
        streamWriter.WriteLine("DEFAULT_TEXTURE=" + Settings.Default_Texture);
        streamWriter.WriteLine("LOAD_GENDER=" + Settings.Gender);
        streamWriter.WriteLine("SCREEN_HEIGHT=" + (object) Settings.Screen_Height);
        streamWriter.WriteLine("SCREEN_WIDTH=" + (object) Settings.Screen_Width);
        streamWriter.Close();
      }
    }
  }
}
