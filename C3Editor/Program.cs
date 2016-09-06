// Decompiled with JetBrains decompiler
// Type: Conquer.Program
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using System;

namespace Conquer
{
  internal static class Program
  {
    [STAThread]
    private static void Main(string[] args)
    {
      using (ConquerGame conquerGame = new ConquerGame())
        conquerGame.Run();
    }
  }
}
