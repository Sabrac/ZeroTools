// Decompiled with JetBrains decompiler
// Type: Conquer.ConquerGame
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Conquer.C3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Linq;
// using System.Windows.Forms;

namespace Conquer
{
  public class ConquerGame : Game
  {
    private bool showColorWheel = false;
    private bool mouseAim = false;
    private bool debugDraw = false;
    private Vector2 colorWheelLocation = Vector2.Zero;
    private Color selectedColor = Color.CornflowerBlue;
    private DateTime nextFrame = DateTime.Now;
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private MouseState mouseState;
    private KeyboardState keyboardState;
    private AlphaTestEffect effect;
    private ArcBallCamera camera;
    private Texture2D colorWheel;
    private Texture2D defaultTexture;
    private RasterizerState CurrentRasterizeState;
    private RasterizerState WireframeRasterizeState;
    private RasterizerState FillRasterizeState;

    public ConquerGame()
    {
      this.graphics = new GraphicsDeviceManager((Game) this);
      this.Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
      Settings.Read();
      Settings.Graphics = this.GraphicsDevice;
      Settings.GUI = new Manager();
      Settings.GUI.Show();
      this.graphics.PreferredBackBufferWidth = Math.Min(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, Settings.Screen_Width);
      this.graphics.PreferredBackBufferHeight = Math.Min(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, Settings.Screen_Height);
      this.graphics.ApplyChanges();
      this.IsMouseVisible = true;
      this.effect = new AlphaTestEffect(this.GraphicsDevice);
      this.camera = new ArcBallCamera(this.GraphicsDevice.Viewport.AspectRatio, Vector3.Zero);
      this.camera.Pitch = -3f;
      this.FillRasterizeState = new RasterizerState()
      {
        FillMode = FillMode.Solid,
        CullMode = CullMode.None
      };
      this.WireframeRasterizeState = new RasterizerState()
      {
        FillMode = FillMode.WireFrame,
        CullMode = CullMode.None
      };
      this.CurrentRasterizeState = this.FillRasterizeState;
      Viewport viewport = this.GraphicsDevice.Viewport;
      int x = viewport.Width / 2;
      viewport = this.GraphicsDevice.Viewport;
      int y = viewport.Height / 2;
      Mouse.SetPosition(x, y);
      this.mouseState = Mouse.GetState();
      this.keyboardState = Keyboard.GetState();
      base.Initialize();
    }

    protected override void LoadContent()
    {
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      Settings.Scene.LoadModel(Settings.Conquer_Path + Settings.Default_Mesh);
      this.centerCamera();
    }

    protected override void UnloadContent()
    {
    }

    private void centerCamera()
    {
      GameModel gameModel = Settings.Scene.Models.FirstOrDefault<GameModel>();
      if (gameModel == null)
        return;
      Physics physics = gameModel.PhysicalObjects.FirstOrDefault<Physics>();
      if (physics == null)
        return;
      this.camera.LookAt = (physics.Max_Bound + physics.Min_Bound) / 2f;
    }

    protected override void Update(GameTime gameTime)
    {
      if (!this.IsActive || Settings.GUI.Visible)
      {
        this.mouseAim = false;
      }
      else
      {
        Vector3 zero = Vector3.Zero;
        MouseState state1 = Mouse.GetState();
        if (!Settings.GUI.Visible && state1.LeftButton == ButtonState.Pressed && this.mouseState.LeftButton == ButtonState.Released)
        {
          this.mouseState = Mouse.GetState();
          if (this.showColorWheel && new Rectangle((int) this.colorWheelLocation.X, (int) this.colorWheelLocation.Y, this.colorWheel.Width, this.colorWheel.Height).Contains(new Point(this.mouseState.X, this.mouseState.Y)))
          {
            Color[] data = new Color[this.colorWheel.Width * this.colorWheel.Height];
            this.colorWheel.GetData<Color>(data);
            int index = this.mouseState.X + this.mouseState.Y * this.colorWheel.Width;
            if (index < data.Length)
              this.selectedColor = data[index];
          }
          else
            this.mouseAim = !this.mouseAim;
        }
        if (this.mouseAim)
        {
          this.camera.Yaw -= (float) (state1.X - this.mouseState.X) * (1f / 500f);
          this.camera.Pitch -= (float) (state1.Y - this.mouseState.Y) * (1f / 500f);
          this.camera.Zoom += (float) (this.mouseState.ScrollWheelValue - state1.ScrollWheelValue) * 0.05f;
          Viewport viewport = this.GraphicsDevice.Viewport;
          int x = viewport.Width / 2;
          viewport = this.GraphicsDevice.Viewport;
          int y = viewport.Height / 2;
          Mouse.SetPosition(x, y);
        }
        this.mouseState = Mouse.GetState();
        KeyboardState state2 = Keyboard.GetState();
        foreach (Keys pressedKey in state2.GetPressedKeys())
        {
          switch (pressedKey)
          {
            case Keys.S:
            case Keys.End:
            case Keys.Down:
              this.camera.Pitch -= 0.025f;
              break;
            case Keys.W:
            case Keys.Home:
            case Keys.Up:
              this.camera.Pitch += 0.025f;
              break;
            case Keys.LeftShift:
              --this.camera.Zoom;
              break;
            case Keys.Space:
              ++this.camera.Zoom;
              break;
            case Keys.PageDown:
            case Keys.Right:
            case Keys.D:
              this.camera.Yaw -= 0.025f;
              break;
            case Keys.Left:
            case Keys.Delete:
            case Keys.A:
              this.camera.Yaw += 0.025f;
              break;
          }
        }
        if (state2.IsKeyDown(Keys.Tab) && !this.keyboardState.IsKeyDown(Keys.Tab))
          this.CurrentRasterizeState = this.CurrentRasterizeState == this.WireframeRasterizeState ? this.FillRasterizeState : this.WireframeRasterizeState;
        if (state2.IsKeyDown(Keys.F10) && !this.keyboardState.IsKeyDown(Keys.F10))
        {
          int backBufferWidth = this.GraphicsDevice.PresentationParameters.BackBufferWidth;
          int backBufferHeight = this.GraphicsDevice.PresentationParameters.BackBufferHeight;
          this.Draw(new GameTime());
          int[] data = new int[backBufferWidth * backBufferHeight];
          this.GraphicsDevice.GetBackBufferData<int>(data);
          Texture2D texture2D = new Texture2D(this.GraphicsDevice, backBufferWidth, backBufferHeight, false, this.GraphicsDevice.PresentationParameters.BackBufferFormat);
          texture2D.SetData<int>(data);
          Stream stream = (Stream) File.OpenWrite("Screenshots/" + (object) Environment.TickCount + ".jpg");
          texture2D.SaveAsJpeg(stream, backBufferWidth, backBufferHeight);
          stream.Dispose();
          texture2D.Dispose();
        }
        if (state2.IsKeyDown(Keys.F1) && !this.keyboardState.IsKeyDown(Keys.F1))
          Settings.GUI.Show();
        if (state2.IsKeyDown(Keys.F5) && !this.keyboardState.IsKeyDown(Keys.F5))
        {
          System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
          string str = (Settings.Conquer_Path + "c3/weapon/").Replace('/', '\\');
          openFileDialog.InitialDirectory = str;
          if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
          {
            Settings.Scene.LoadModel(openFileDialog.FileName);
            this.centerCamera();
          }
        }
        if (state2.IsKeyDown(Keys.F2) && !this.keyboardState.IsKeyDown(Keys.F2))
        {
                    System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
          if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
          {
            Settings.Scene.LoadModel(openFileDialog.FileName);
            this.centerCamera();
          }
        }
        if (state2.IsKeyDown(Keys.F3) && !this.keyboardState.IsKeyDown(Keys.F3))
        {
                    System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
          if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
          {
            if (Settings.GUI.SelectedModel == null)
              Settings.GUI.SelectedModel = Settings.Scene.Models.FirstOrDefault<GameModel>();
            if (Settings.GUI.SelectedModel != null)
              DDSLib.DDSFromFile(openFileDialog.FileName, this.GraphicsDevice, false, out Settings.GUI.SelectedModel.Texture);
          }
        }
        if (state2.IsKeyDown(Keys.F12) && !this.keyboardState.IsKeyDown(Keys.F12))
        {
          Settings.Scene = new GameScene();
          Settings.GUI = new Manager();
        }
        foreach (GameModel model in Settings.Scene.Models)
        {
          foreach (Physics physicalObject in model.PhysicalObjects)
          {
            if (physicalObject.Motion != null && DateTime.Now > this.nextFrame)
            {
              this.nextFrame = DateTime.Now.AddMilliseconds(30.0);
              physicalObject.FrameIndex = (physicalObject.FrameIndex + 1) % physicalObject.Motion.KeyFrames.Count;
            }
            if (!(physicalObject.UV_Step == Vector2.Zero))
            {
              foreach (Vertex normalVertex in physicalObject.NormalVertexes)
                normalVertex.UV += physicalObject.UV_Step;
              foreach (Vertex alphaVertex in physicalObject.AlphaVertexes)
                alphaVertex.UV += physicalObject.UV_Step;
            }
          }
        }
        this.keyboardState = state2;
        base.Update(gameTime);
      }
    }

    protected override void Draw(GameTime gameTime)
    {
      this.GraphicsDevice.Clear(this.selectedColor);
      this.GraphicsDevice.RasterizerState = new RasterizerState()
      {
        CullMode = CullMode.None,
        FillMode = this.CurrentRasterizeState.FillMode,
        MultiSampleAntiAlias = true
      };
      this.effect.Projection = this.camera.ProjectionMatrix;
      this.effect.View = this.camera.ViewMatrix;
      this.GraphicsDevice.SamplerStates[0] = new SamplerState()
      {
        Filter = TextureFilter.Point,
        AddressU = TextureAddressMode.Wrap,
        AddressW = TextureAddressMode.Wrap,
        AddressV = TextureAddressMode.Wrap
      };
      this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
      foreach (GameModel model in Settings.Scene.Models)
      {
        foreach (Physics physicalObject in model.PhysicalObjects)
        {
          if (physicalObject.IsShown)
          {
            this.effect.Texture = model.Texture;
            this.effect.World = physicalObject.World * (physicalObject.Motion == null ? Matrix.Identity : physicalObject.Motion.KeyFrames.ElementAt<KeyFrame>(physicalObject.FrameIndex).Transforms[0]);
            foreach (EffectPass pass in this.effect.CurrentTechnique.Passes)
            {
              pass.Apply();
              this.GraphicsDevice.BlendState = physicalObject.BlendMode;
              if (physicalObject.NormalVertexes.Length > 0)
                this.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, Array.ConvertAll<Vertex, VertexPositionNormalTexture>(physicalObject.NormalVertexes, (Converter<Vertex, VertexPositionNormalTexture>) (I => (VertexPositionNormalTexture) I)), 0, physicalObject.NormalVertexes.Length, physicalObject.NormalIndexes, 0, physicalObject.NormalIndexes.Length / 3, VertexPositionNormalTexture.VertexDeclaration);
              if (physicalObject.AlphaVertexes.Length > 0)
                this.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, Array.ConvertAll<Vertex, VertexPositionNormalTexture>(physicalObject.AlphaVertexes, (Converter<Vertex, VertexPositionNormalTexture>) (I => (VertexPositionNormalTexture) I)), 0, physicalObject.AlphaVertexes.Length, physicalObject.AlphaIndexes, 0, physicalObject.AlphaIndexes.Length / 3, VertexPositionNormalTexture.VertexDeclaration);
            }
          }
        }
      }
      base.Draw(gameTime);
    }
  }
}
