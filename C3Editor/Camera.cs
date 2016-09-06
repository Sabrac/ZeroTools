// Decompiled with JetBrains decompiler
// Type: Conquer.Camera
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Conquer
{
  public class Camera
  {
    protected Vector3 _position = Vector3.Zero;
    private Vector3 _rotation = Vector3.Zero;
    private float _viewAngle = 0.7853982f;
    private float _nearPlane = 0.01f;
    private float _farPlane = 99999f;
    private Matrix _view;
    private Matrix _projection;

    public Matrix View
    {
      get
      {
        return this._view;
      }
      protected set
      {
        this._view = value;
      }
    }

    public Matrix Projection
    {
      get
      {
        return this._projection;
      }
      protected set
      {
        this._projection = value;
      }
    }

    public Vector3 Position
    {
      get
      {
        return this._position;
      }
      set
      {
        this._position = value;
        this.CalculateView();
      }
    }

    public float Yaw
    {
      get
      {
        return this._rotation.X;
      }
      set
      {
        this._rotation.X = value;
        this.CalculateView();
      }
    }

    public float Pitch
    {
      get
      {
        return this._rotation.Y;
      }
      set
      {
        this._rotation.Y = value;
        this.CalculateView();
      }
    }

    public float Roll
    {
      get
      {
        return this._rotation.Z;
      }
      set
      {
        this._rotation.Z = value;
        this.CalculateView();
      }
    }

    public Camera(Vector3 position, Vector3 target, GraphicsDevice graphics)
    {
      this._position = position;
      this._projection = Matrix.CreatePerspectiveFieldOfView(this._viewAngle, graphics.Viewport.AspectRatio, this._nearPlane, this._farPlane);
    }

    public void CalculateView()
    {
      Matrix matrix = Matrix.CreateRotationX(this.Yaw) * Matrix.CreateRotationY(this.Pitch) * Matrix.CreateRotationZ(this.Roll);
      this.View = Matrix.CreateLookAt(this.Position, this.Position + Vector3.Transform(Vector3.Forward, matrix), Vector3.Transform(Vector3.Up, matrix));
    }

    public void AddToCameraPosition(Vector3 vectorToAdd)
    {
      Matrix matrix = Matrix.CreateRotationX(this.Yaw) * Matrix.CreateRotationY(this.Pitch) * Matrix.CreateRotationZ(this.Roll);
      this.Position += 1f * Vector3.Transform(vectorToAdd, matrix);
      this.CalculateView();
    }
  }
}
