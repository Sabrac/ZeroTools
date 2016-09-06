// Decompiled with JetBrains decompiler
// Type: Conquer.ArcBallCamera
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Microsoft.Xna.Framework;

namespace Conquer
{
  public class ArcBallCamera
  {
    private bool viewMatrixDirty = true;
    private bool projectionMatrixDirty = true;
    public float MinPitch = -1.270796f;
    public float MaxPitch = 1.270796f;
    public float MinZoom = 1f;
    public float MaxZoom = float.MaxValue;
    private float zoom = 1f;
    private float pitch;
    private float yaw;
    private float fieldOfView;
    private float aspectRatio;
    private float nearPlane;
    private float farPlane;
    private Vector3 position;
    private Vector3 lookAt;
    private Matrix viewMatrix;
    private Matrix projectionMatrix;

    public float Pitch
    {
      get
      {
        return this.pitch;
      }
      set
      {
        this.viewMatrixDirty = true;
        this.pitch = MathHelper.Clamp(value, this.MinPitch, this.MaxPitch);
      }
    }

    public float Yaw
    {
      get
      {
        return this.yaw;
      }
      set
      {
        this.viewMatrixDirty = true;
        this.yaw = value;
      }
    }

    public float FieldOfView
    {
      get
      {
        return this.fieldOfView;
      }
      set
      {
        this.projectionMatrixDirty = true;
        this.fieldOfView = value;
      }
    }

    public float AspectRatio
    {
      get
      {
        return this.aspectRatio;
      }
      set
      {
        this.projectionMatrixDirty = true;
        this.aspectRatio = value;
      }
    }

    public float NearPlane
    {
      get
      {
        return this.nearPlane;
      }
      set
      {
        this.projectionMatrixDirty = true;
        this.nearPlane = value;
      }
    }

    public float FarPlane
    {
      get
      {
        return this.farPlane;
      }
      set
      {
        this.projectionMatrixDirty = true;
        this.farPlane = value;
      }
    }

    public float Zoom
    {
      get
      {
        return this.zoom;
      }
      set
      {
        this.viewMatrixDirty = true;
        this.zoom = MathHelper.Clamp(value, this.MinZoom, this.MaxZoom);
      }
    }

    public Vector3 Position
    {
      get
      {
        if (this.viewMatrixDirty)
          this.ReCreateViewMatrix();
        return this.position;
      }
    }

    public Vector3 LookAt
    {
      get
      {
        return this.lookAt;
      }
      set
      {
        this.viewMatrixDirty = true;
        this.lookAt = value;
      }
    }

    public Matrix ViewProjectionMatrix
    {
      get
      {
        return this.ViewMatrix * this.ProjectionMatrix;
      }
    }

    public Matrix ViewMatrix
    {
      get
      {
        if (this.viewMatrixDirty)
          this.ReCreateViewMatrix();
        return this.viewMatrix;
      }
    }

    public Matrix ProjectionMatrix
    {
      get
      {
        if (this.projectionMatrixDirty)
          this.ReCreateProjectionMatrix();
        return this.projectionMatrix;
      }
    }

    public ArcBallCamera(float aspectRation, Vector3 lookAt)
      : this(aspectRation, 0.7853982f, lookAt, Vector3.Up, 0.1f, float.MaxValue)
    {
    }

    public ArcBallCamera(float aspectRatio, float fieldOfView, Vector3 lookAt, Vector3 up, float nearPlane, float farPlane)
    {
      this.aspectRatio = aspectRatio;
      this.fieldOfView = fieldOfView;
      this.lookAt = lookAt;
      this.nearPlane = nearPlane;
      this.farPlane = farPlane;
      this.zoom = 100f;
    }

    private void ReCreateViewMatrix()
    {
      this.position = Vector3.Transform(Vector3.Backward, Matrix.CreateFromYawPitchRoll(this.yaw, this.pitch, 0.0f));
      this.position *= this.zoom;
      this.position += this.lookAt;
      this.viewMatrix = Matrix.CreateLookAt(this.position, this.lookAt, Vector3.Up);
      this.viewMatrixDirty = false;
    }

    private void ReCreateProjectionMatrix()
    {
      this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(0.7853982f, this.AspectRatio, this.nearPlane, this.farPlane);
      this.projectionMatrixDirty = false;
    }

    public void MoveCameraRight(float amount)
    {
      Vector3 vector3 = Vector3.Cross(Vector3.Normalize(this.LookAt - this.Position), Vector3.Up);
      vector3.Y = 0.0f;
      vector3.Normalize();
      this.LookAt += vector3 * amount;
    }

    public void MoveCameraForward(float amount)
    {
      Vector3 vector3 = Vector3.Normalize(this.LookAt - this.Position);
      vector3.Y = 0.0f;
      vector3.Normalize();
      this.LookAt += vector3 * amount;
    }
  }
}
