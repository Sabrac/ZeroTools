// Decompiled with JetBrains decompiler
// Type: Conquer.DDSLib
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Conquer
{
  public static class DDSLib
  {
    private const int DDSD_CAPS = 1;
    private const int DDSD_HEIGHT = 2;
    private const int DDSD_WIDTH = 4;
    private const int DDSD_PITCH = 8;
    private const int DDSD_PIXELFORMAT = 4096;
    private const int DDSD_MIPMAPCOUNT = 131072;
    private const int DDSD_LINEARSIZE = 524288;
    private const int DDSD_DEPTH = 8388608;
    private const int DDPF_ALPHAPIXELS = 1;
    private const int DDPF_ALPHA = 2;
    private const int DDPF_FOURCC = 4;
    private const int DDPF_RGB = 64;
    private const int DDPF_YUV = 512;
    private const int DDPF_LUMINANCE = 8192;
    private const int DDPF_Q8W8V8U8 = 524288;
    private const int DDSCAPS_COMPLEX = 8;
    private const int DDSCAPS_MIPMAP = 4194304;
    private const int DDSCAPS_TEXTURE = 4096;
    private const int DDSCAPS2_CUBEMAP = 512;
    private const int DDSCAPS2_CUBEMAP_POSITIVEX = 1024;
    private const int DDSCAPS2_CUBEMAP_NEGATIVEX = 2048;
    private const int DDSCAPS2_CUBEMAP_POSITIVEY = 4096;
    private const int DDSCAPS2_CUBEMAP_NEGATIVEY = 8192;
    private const int DDSCAPS2_CUBEMAP_POSITIVEZ = 16384;
    private const int DDSCAPS2_CUBEMAP_NEGATIVEZ = 32768;
    private const int DDSCAPS2_VOLUME = 2097152;
    private const uint DDS_MAGIC = 542327876;
    [ThreadStatic]
    private static byte[] mipData;

    private static bool IsCubemapTest(int ddsCaps1, int ddsCaps2)
    {
      return (ddsCaps1 & 8) != 0 && (ddsCaps2 & 512) != 0;
    }

    private static bool IsVolumeTextureTest(int ddsCaps1, int ddsCaps2)
    {
      return (ddsCaps2 & 2097152) != 0;
    }

    private static bool IsCompressedTest(uint pfFlags)
    {
      return ((int) pfFlags & 4) != 0;
    }

    private static bool HasAlphaTest(uint pfFlags)
    {
      return ((int) pfFlags & 1) != 0;
    }

    private static int MipMapSize(int map, int size)
    {
      for (int index = 0; index < map; ++index)
        size >>= 1;
      if (size <= 0)
        return 1;
      return size;
    }

    private static DDSLib.LoadSurfaceFormat GetLoadSurfaceFormat(uint pixelFlags, uint pixelFourCC, int bitCount, uint rBitMask, uint gBitMask, uint bBitMask, uint aBitMask, DDSLib.FourCC compressionFormat)
    {
      switch ((DDSLib.FourCC) pixelFourCC)
      {
        case DDSLib.FourCC.D3DFMT_A16B16G16R16:
          return DDSLib.LoadSurfaceFormat.A16B16G16R16;
        case DDSLib.FourCC.D3DFMT_G32R32F:
          return DDSLib.LoadSurfaceFormat.G32R32F;
        case DDSLib.FourCC.D3DFMT_G16R16F:
          return DDSLib.LoadSurfaceFormat.G16R16F;
        case DDSLib.FourCC.D3DFMT_Q8W8V8U8:
          return DDSLib.LoadSurfaceFormat.Q8W8V8U8;
        case DDSLib.FourCC.D3DFMT_CxV8U8:
          return DDSLib.LoadSurfaceFormat.CxV8U8;
        case DDSLib.FourCC.D3DFMT_A16B16G16R16F:
          return DDSLib.LoadSurfaceFormat.A16B16G16R16F;
        case DDSLib.FourCC.D3DFMT_A32B32G32R32F:
          return DDSLib.LoadSurfaceFormat.A32B32G32R32F;
        case DDSLib.FourCC.D3DFMT_R32F:
          return DDSLib.LoadSurfaceFormat.R32F;
        case DDSLib.FourCC.D3DFMT_R16F:
          return DDSLib.LoadSurfaceFormat.R16F;
        default:
          if (((int) pixelFlags & 4) != 0)
          {
            if ((int) pixelFourCC == 827611204)
              return DDSLib.LoadSurfaceFormat.Dxt1;
            if ((int) pixelFourCC == 861165636 || (int) pixelFourCC == 844388420)
              return DDSLib.LoadSurfaceFormat.Dxt3;
            if ((int) pixelFourCC == 894720068 || (int) pixelFourCC == 877942852)
              return DDSLib.LoadSurfaceFormat.Dxt5;
          }
          if (((int) pixelFlags & 64) != 0)
          {
            if ((int) pixelFlags == 64 && bitCount == 16 && ((int) pixelFourCC == 0 && (int) rBitMask == 31744) && ((int) gBitMask == 992 && (int) bBitMask == 31) && (int) aBitMask == 0)
              return DDSLib.LoadSurfaceFormat.RGB555;
            if ((int) pixelFlags == 65 && bitCount == 32 && ((int) pixelFourCC == 0 && (int) rBitMask == 16711680) && ((int) gBitMask == 65280 && (int) bBitMask == (int) byte.MaxValue) && (int) aBitMask == -16777216)
              return DDSLib.LoadSurfaceFormat.A8R8G8B8;
            if ((int) pixelFlags == 64 && bitCount == 32 && ((int) pixelFourCC == 0 && (int) rBitMask == 16711680) && ((int) gBitMask == 65280 && (int) bBitMask == (int) byte.MaxValue) && (int) aBitMask == 0)
              return DDSLib.LoadSurfaceFormat.X8R8G8B8;
            if ((int) pixelFlags == 65 && bitCount == 32 && ((int) pixelFourCC == 0 && (int) rBitMask == (int) byte.MaxValue) && ((int) gBitMask == 65280 && (int) bBitMask == 16711680) && (int) aBitMask == -16777216)
              return DDSLib.LoadSurfaceFormat.A8B8G8R8;
            if ((int) pixelFlags == 64 && bitCount == 32 && ((int) pixelFourCC == 0 && (int) rBitMask == (int) byte.MaxValue) && ((int) gBitMask == 65280 && (int) bBitMask == 16711680) && (int) aBitMask == 0)
              return DDSLib.LoadSurfaceFormat.X8B8G8R8;
            if ((int) pixelFlags == 65 && bitCount == 16 && ((int) pixelFourCC == 0 && (int) rBitMask == 31744) && ((int) gBitMask == 992 && (int) bBitMask == 31) && (int) aBitMask == 32768)
              return DDSLib.LoadSurfaceFormat.Bgra5551;
            if ((int) pixelFlags == 65 && bitCount == 16 && ((int) pixelFourCC == 0 && (int) rBitMask == 3840) && ((int) gBitMask == 240 && (int) bBitMask == 15) && (int) aBitMask == 61440)
              return DDSLib.LoadSurfaceFormat.Bgra4444;
            if ((int) pixelFlags == 64 && bitCount == 24 && ((int) pixelFourCC == 0 && (int) rBitMask == 16711680) && ((int) gBitMask == 65280 && (int) bBitMask == (int) byte.MaxValue) && (int) aBitMask == 0)
              return DDSLib.LoadSurfaceFormat.R8G8B8;
            if ((int) pixelFlags == 64 && bitCount == 16 && ((int) pixelFourCC == 0 && (int) rBitMask == 63488) && ((int) gBitMask == 2016 && (int) bBitMask == 31) && (int) aBitMask == 0)
              return DDSLib.LoadSurfaceFormat.Bgr565;
            if ((int) pixelFlags == 2 && bitCount == 8 && ((int) pixelFourCC == 0 && (int) rBitMask == 0) && ((int) gBitMask == 0 && (int) bBitMask == 0) && (int) aBitMask == (int) byte.MaxValue)
              return DDSLib.LoadSurfaceFormat.Alpha8;
            if ((int) pixelFlags == 64 && bitCount == 32 && ((int) pixelFourCC == 0 && (int) rBitMask == (int) ushort.MaxValue) && ((int) gBitMask == -65536 && (int) bBitMask == 0) && (int) aBitMask == 0)
              return DDSLib.LoadSurfaceFormat.G16R16;
            if ((int) pixelFlags == 65 && bitCount == 32 && ((int) pixelFourCC == 0 && (int) rBitMask == 1072693248) && ((int) gBitMask == 1047552 && (int) bBitMask == 1023) && (int) aBitMask == -1073741824)
              return DDSLib.LoadSurfaceFormat.A2B10G10R10;
          }
          return (int) pixelFlags == 524288 && bitCount == 32 && ((int) pixelFourCC == 0 || (int) pixelFourCC == 63) && ((int) rBitMask == (int) byte.MaxValue && (int) gBitMask == 65280 && (int) bBitMask == 16711680) && (int) aBitMask == -16777216 ? DDSLib.LoadSurfaceFormat.Q8W8V8U8 : DDSLib.LoadSurfaceFormat.Unknown;
      }
    }

    private static DDSLib.FourCC GetCompressionFormat(uint pixelFlags, uint pixelFourCC)
    {
      if (((int) pixelFlags & 4) != 0)
        return (DDSLib.FourCC) pixelFourCC;
      return (DDSLib.FourCC) 0;
    }

    private static int MipMapSizeInBytes(int map, int width, int height, bool isCompressed, DDSLib.FourCC compressionFormat, int depth)
    {
      width = DDSLib.MipMapSize(map, width);
      height = DDSLib.MipMapSize(map, height);
      if (compressionFormat == DDSLib.FourCC.D3DFMT_R32F)
        return width * height * 4;
      if (compressionFormat == DDSLib.FourCC.D3DFMT_R16F)
        return width * height * 2;
      if (compressionFormat == DDSLib.FourCC.D3DFMT_A32B32G32R32F)
        return width * height * 16;
      if (compressionFormat == DDSLib.FourCC.D3DFMT_A16B16G16R16F)
        return width * height * 8;
      if (compressionFormat == DDSLib.FourCC.D3DFMT_CxV8U8)
        return width * height * 2;
      if (compressionFormat == DDSLib.FourCC.D3DFMT_Q8W8V8U8 || compressionFormat == DDSLib.FourCC.D3DFMT_G16R16F)
        return width * height * 4;
      if (compressionFormat == DDSLib.FourCC.D3DFMT_G32R32F || compressionFormat == DDSLib.FourCC.D3DFMT_A16B16G16R16)
        return width * height * 8;
      if (!isCompressed)
        return width * height * (depth / 8);
      int num = compressionFormat == DDSLib.FourCC.D3DFMT_DXT1 ? 8 : 16;
      return (width + 3) / 4 * ((height + 3) / 4) * num;
    }

    private static void GetMipMaps(int offsetInStream, int map, bool hasMipMaps, int width, int height, bool isCompressed, DDSLib.FourCC compressionFormat, int rgbBitCount, bool partOfCubeMap, BinaryReader reader, DDSLib.LoadSurfaceFormat loadSurfaceFormat, ref byte[] data, out int numBytes)
    {
      int num1 = 128 + offsetInStream;
      for (int map1 = 0; map1 < map; ++map1)
        num1 += DDSLib.MipMapSizeInBytes(map1, width, height, isCompressed, compressionFormat, rgbBitCount);
      reader.BaseStream.Seek((long) num1, SeekOrigin.Begin);
      numBytes = DDSLib.MipMapSizeInBytes(map, width, height, isCompressed, compressionFormat, rgbBitCount);
      if (!isCompressed && rgbBitCount == 24)
        numBytes += numBytes / 3;
      if (data == null || data.Length < numBytes)
        data = new byte[numBytes];
      if (!isCompressed && loadSurfaceFormat == DDSLib.LoadSurfaceFormat.R8G8B8)
      {
        int index = 0;
        while (index < numBytes)
        {
          data[index] = reader.ReadByte();
          data[index + 1] = reader.ReadByte();
          data[index + 2] = reader.ReadByte();
          data[index + 3] = byte.MaxValue;
          index += 4;
        }
      }
      else
        reader.Read(data, 0, numBytes);
      if (loadSurfaceFormat == DDSLib.LoadSurfaceFormat.X8R8G8B8 || loadSurfaceFormat == DDSLib.LoadSurfaceFormat.X8B8G8R8)
      {
        int num2 = 0;
        while (num2 < numBytes)
        {
          data[num2 + 3] = byte.MaxValue;
          num2 += 4;
        }
      }
      if (loadSurfaceFormat != DDSLib.LoadSurfaceFormat.A8R8G8B8 && loadSurfaceFormat != DDSLib.LoadSurfaceFormat.X8R8G8B8 && loadSurfaceFormat != DDSLib.LoadSurfaceFormat.R8G8B8)
        return;
      if ((rgbBitCount == 32 || rgbBitCount == 24 ? 4 : 3) == 3)
      {
        int index = 0;
        while (index < numBytes - 2)
        {
          byte num2 = data[index];
          byte num3 = data[index + 2];
          data[index] = num3;
          data[index + 2] = num2;
          index += 3;
        }
      }
      else
      {
        int index = 0;
        while (index < numBytes - 3)
        {
          byte num2 = data[index];
          byte num3 = data[index + 2];
          data[index] = num3;
          data[index + 2] = num2;
          index += 4;
        }
      }
    }

    private static bool CheckFullMipChain(int width, int height, int numMip)
    {
      int num1 = Math.Max(width, height);
      int num2 = 0;
      while (num1 > 1)
      {
        num1 /= 2;
        ++num2;
      }
      return num2 <= numMip;
    }

    public static void DDSFromFile(string fileName, GraphicsDevice device, bool loadMipMap, out Texture2D texture)
    {
      Stream stream = (Stream) File.OpenRead(fileName);
      Texture texture1;
      DDSLib.InternalDDSFromStream(stream, device, 0, loadMipMap, out texture1);
      stream.Close();
      texture = texture1 as Texture2D;
      if (texture == null)
        throw new Exception("The data in the stream contains a TextureCube not Texture2D");
    }

    public static void DDSFromFile(string fileName, GraphicsDevice device, bool loadMipMap, out TextureCube texture)
    {
      Stream stream = (Stream) File.OpenRead(fileName);
      Texture texture1;
      DDSLib.InternalDDSFromStream(stream, device, 0, loadMipMap, out texture1);
      stream.Close();
      texture = texture1 as TextureCube;
      if (texture == null)
        throw new Exception("The data in the stream contains a Texture2D not TextureCube");
    }

    public static void DDSFromFile(string fileName, GraphicsDevice device, bool loadMipMap, out Texture3D texture)
    {
      Stream stream = (Stream) File.OpenRead(fileName);
      Texture texture1;
      DDSLib.InternalDDSFromStream(stream, device, 0, loadMipMap, out texture1);
      stream.Close();
      texture = texture1 as Texture3D;
      if (texture == null)
        throw new Exception("The data in the stream contains a Texture2D not TextureCube");
    }

    public static void DDSFromStream(Stream stream, GraphicsDevice device, int streamOffset, bool loadMipMap, out Texture2D texture)
    {
      Texture texture1;
      DDSLib.InternalDDSFromStream(stream, device, streamOffset, loadMipMap, out texture1);
      texture = texture1 as Texture2D;
      if (texture == null)
        throw new Exception("The data in the stream contains a TextureCube not Texture2D");
    }

    public static void DDSFromStream(Stream stream, GraphicsDevice device, int streamOffset, bool loadMipMap, out TextureCube texture)
    {
      Texture texture1;
      DDSLib.InternalDDSFromStream(stream, device, streamOffset, loadMipMap, out texture1);
      texture = texture1 as TextureCube;
      if (texture == null)
        throw new Exception("The data in the stream contains a Texture2D not TextureCube");
    }

    public static void DDSFromStream(Stream stream, GraphicsDevice device, int streamOffset, bool loadMipMap, out Texture3D texture)
    {
      Texture texture1;
      DDSLib.InternalDDSFromStream(stream, device, streamOffset, loadMipMap, out texture1);
      texture = texture1 as Texture3D;
      if (texture == null)
        throw new Exception("The data in the stream contains a Texture2D not TextureCube");
    }

    private static SurfaceFormat SurfaceFormatFromLoadFormat(DDSLib.LoadSurfaceFormat loadSurfaceFormat, DDSLib.FourCC compressionFormat, uint pixelFlags, int rgbBitCount)
    {
      switch (loadSurfaceFormat)
      {
        case DDSLib.LoadSurfaceFormat.Unknown:
          switch (compressionFormat)
          {
            case DDSLib.FourCC.D3DFMT_DXT3:
              return SurfaceFormat.Dxt3;
            case DDSLib.FourCC.D3DFMT_DXT5:
              return SurfaceFormat.Dxt5;
            case (DDSLib.FourCC) 0:
              if (rgbBitCount == 8)
                return SurfaceFormat.Alpha8;
              if (rgbBitCount == 16)
                return DDSLib.HasAlphaTest(pixelFlags) ? SurfaceFormat.Bgr565 : SurfaceFormat.Bgra4444;
              if (rgbBitCount == 32 || rgbBitCount == 24)
                return SurfaceFormat.Color;
              throw new Exception("Unsuported format");
            case DDSLib.FourCC.D3DFMT_DXT1:
              return SurfaceFormat.Dxt1;
            default:
              throw new Exception("Unsuported format");
          }
        case DDSLib.LoadSurfaceFormat.Dxt1:
          return SurfaceFormat.Dxt1;
        case DDSLib.LoadSurfaceFormat.Dxt3:
          return SurfaceFormat.Dxt3;
        case DDSLib.LoadSurfaceFormat.Dxt5:
          return SurfaceFormat.Dxt5;
        case DDSLib.LoadSurfaceFormat.R8G8B8:
          return SurfaceFormat.Color;
        case DDSLib.LoadSurfaceFormat.Bgra5551:
          return SurfaceFormat.Bgra5551;
        case DDSLib.LoadSurfaceFormat.Bgra4444:
          return SurfaceFormat.Bgra4444;
        case DDSLib.LoadSurfaceFormat.Bgr565:
          return SurfaceFormat.Bgr565;
        case DDSLib.LoadSurfaceFormat.Alpha8:
          return SurfaceFormat.Alpha8;
        case DDSLib.LoadSurfaceFormat.X8R8G8B8:
          return SurfaceFormat.Color;
        case DDSLib.LoadSurfaceFormat.A8R8G8B8:
          return SurfaceFormat.Color;
        case DDSLib.LoadSurfaceFormat.A8B8G8R8:
          return SurfaceFormat.Color;
        case DDSLib.LoadSurfaceFormat.X8B8G8R8:
          return SurfaceFormat.Color;
        case DDSLib.LoadSurfaceFormat.R32F:
          return SurfaceFormat.Single;
        case DDSLib.LoadSurfaceFormat.R16F:
          return SurfaceFormat.HalfSingle;
        case DDSLib.LoadSurfaceFormat.A32B32G32R32F:
          return SurfaceFormat.Vector4;
        case DDSLib.LoadSurfaceFormat.A16B16G16R16F:
          return SurfaceFormat.HalfVector4;
        case DDSLib.LoadSurfaceFormat.Q8W8V8U8:
          return SurfaceFormat.NormalizedByte4;
        case DDSLib.LoadSurfaceFormat.CxV8U8:
          return SurfaceFormat.NormalizedByte2;
        case DDSLib.LoadSurfaceFormat.G16R16F:
          return SurfaceFormat.HalfVector2;
        case DDSLib.LoadSurfaceFormat.G32R32F:
          return SurfaceFormat.Vector2;
        case DDSLib.LoadSurfaceFormat.G16R16:
          return SurfaceFormat.Rg32;
        case DDSLib.LoadSurfaceFormat.A2B10G10R10:
          return SurfaceFormat.Rgba1010102;
        case DDSLib.LoadSurfaceFormat.A16B16G16R16:
          return SurfaceFormat.Rgba64;
        default:
          throw new Exception(((int) loadSurfaceFormat).ToString() + " is an unsuported format");
      }
    }

    private static TextureCube GenerateNewCubeTexture(DDSLib.LoadSurfaceFormat loadSurfaceFormat, DDSLib.FourCC compressionFormat, GraphicsDevice device, int width, bool hasMipMaps, uint pixelFlags, int rgbBitCount)
    {
      SurfaceFormat format = DDSLib.SurfaceFormatFromLoadFormat(loadSurfaceFormat, compressionFormat, pixelFlags, rgbBitCount);
      TextureCube textureCube = new TextureCube(device, width, hasMipMaps, format);
      if (textureCube.Format != format)
        throw new Exception("Can't generate a " + (object) format + " surface.");
      return textureCube;
    }

    private static Texture2D GenerateNewTexture2D(DDSLib.LoadSurfaceFormat loadSurfaceFormat, DDSLib.FourCC compressionFormat, GraphicsDevice device, int width, int height, bool hasMipMaps, uint pixelFlags, int rgbBitCount)
    {
      SurfaceFormat format = DDSLib.SurfaceFormatFromLoadFormat(loadSurfaceFormat, compressionFormat, pixelFlags, rgbBitCount);
      Texture2D texture2D = new Texture2D(device, width, height, hasMipMaps, format);
      if (texture2D.Format != format)
        throw new Exception("Can't generate a " + (object) format + " surface.");
      return texture2D;
    }

    private static Texture3D GenerateNewTexture3D(DDSLib.LoadSurfaceFormat loadSurfaceFormat, DDSLib.FourCC compressionFormat, GraphicsDevice device, int width, int height, int depth, bool hasMipMaps, uint pixelFlags, int rgbBitCount)
    {
      SurfaceFormat format = DDSLib.SurfaceFormatFromLoadFormat(loadSurfaceFormat, compressionFormat, pixelFlags, rgbBitCount);
      Texture3D texture3D = new Texture3D(device, width, height, depth, hasMipMaps, format);
      if (texture3D.Format != format)
        throw new Exception("Can't generate a " + (object) format + " surface.");
      return texture3D;
    }

    private static void InternalDDSFromStream(Stream stream, GraphicsDevice device, int streamOffset, bool loadMipMap, out Texture texture)
    {
      if (stream == null)
        throw new Exception("Can't read from a null stream");
      BinaryReader reader = new BinaryReader(stream);
      if ((long) streamOffset > reader.BaseStream.Length)
        throw new Exception("The stream you offered is smaller then the offset you are proposing for it.");
      reader.BaseStream.Seek((long) streamOffset, SeekOrigin.Begin);
      if ((int) reader.ReadUInt32() != 542327876)
        throw new Exception("Can't open non DDS data.");
      reader.BaseStream.Position += 8L;
      int num1 = reader.ReadInt32();
      int num2 = reader.ReadInt32();
      reader.BaseStream.Position += 4L;
      int num3 = reader.ReadInt32();
      int numMip = reader.ReadInt32();
      reader.BaseStream.Position += 48L;
      uint num4 = reader.ReadUInt32();
      uint pixelFourCC = reader.ReadUInt32();
      int num5 = reader.ReadInt32();
      uint rBitMask = reader.ReadUInt32();
      uint gBitMask = reader.ReadUInt32();
      uint bBitMask = reader.ReadUInt32();
      uint aBitMask = reader.ReadUInt32();
      int ddsCaps1 = reader.ReadInt32();
      int ddsCaps2 = reader.ReadInt32();
      reader.BaseStream.Position += 12L;
      bool partOfCubeMap = DDSLib.IsCubemapTest(ddsCaps1, ddsCaps2);
      bool flag1 = DDSLib.IsVolumeTextureTest(ddsCaps1, ddsCaps2);
      DDSLib.FourCC compressionFormat = DDSLib.GetCompressionFormat(num4, pixelFourCC);
      if (compressionFormat == DDSLib.FourCC.DX10)
        throw new Exception("The Dxt 10 header reader is not implemented");
      DDSLib.LoadSurfaceFormat loadSurfaceFormat = DDSLib.GetLoadSurfaceFormat(num4, pixelFourCC, num5, rBitMask, gBitMask, bBitMask, aBitMask, compressionFormat);
      bool isCompressed = DDSLib.IsCompressedTest(num4);
      bool flag2 = DDSLib.CheckFullMipChain(num2, num1, numMip);
      bool hasMipMaps1 = numMip > 0;
      bool hasMipMaps2 = flag2 & loadMipMap;
      if (partOfCubeMap)
      {
        TextureCube newCubeTexture = DDSLib.GenerateNewCubeTexture(loadSurfaceFormat, compressionFormat, device, num2, hasMipMaps2, num4, num5);
        int num6 = 0;
        if (numMip == 0)
          numMip = 1;
        if (!hasMipMaps2)
        {
          for (int map = 0; map < numMip; ++map)
            num6 += DDSLib.MipMapSizeInBytes(map, num2, num1, isCompressed, compressionFormat, num5);
        }
        for (int index = 0; index < numMip; ++index)
        {
          int numBytes = 0;
          byte[] mipData = DDSLib.mipData;
          DDSLib.GetMipMaps(streamOffset, index, hasMipMaps1, num2, num1, isCompressed, compressionFormat, num5, partOfCubeMap, reader, loadSurfaceFormat, ref mipData, out numBytes);
          DDSLib.mipData = mipData;
          if (hasMipMaps2)
            num6 += numBytes;
          if (index == 0 || hasMipMaps2)
            newCubeTexture.SetData<byte>(CubeMapFace.PositiveX, index, new Rectangle?(), mipData, 0, numBytes);
          else
            break;
        }
        for (int index = 0; index < numMip; ++index)
        {
          int numBytes = 0;
          byte[] mipData = DDSLib.mipData;
          DDSLib.GetMipMaps(num6 + streamOffset, index, hasMipMaps1, num2, num1, isCompressed, compressionFormat, num5, partOfCubeMap, reader, loadSurfaceFormat, ref mipData, out numBytes);
          DDSLib.mipData = mipData;
          if (index == 0 || hasMipMaps2)
            newCubeTexture.SetData<byte>(CubeMapFace.NegativeX, index, new Rectangle?(), mipData, 0, numBytes);
          else
            break;
        }
        for (int index = 0; index < numMip; ++index)
        {
          int numBytes = 0;
          byte[] mipData = DDSLib.mipData;
          DDSLib.GetMipMaps(num6 * 2 + streamOffset, index, hasMipMaps1, num2, num1, isCompressed, compressionFormat, num5, partOfCubeMap, reader, loadSurfaceFormat, ref mipData, out numBytes);
          DDSLib.mipData = mipData;
          if (index == 0 || hasMipMaps2)
            newCubeTexture.SetData<byte>(CubeMapFace.PositiveY, index, new Rectangle?(), mipData, 0, numBytes);
          else
            break;
        }
        for (int index = 0; index < numMip; ++index)
        {
          int numBytes = 0;
          byte[] mipData = DDSLib.mipData;
          DDSLib.GetMipMaps(num6 * 3 + streamOffset, index, hasMipMaps1, num2, num1, isCompressed, compressionFormat, num5, partOfCubeMap, reader, loadSurfaceFormat, ref mipData, out numBytes);
          DDSLib.mipData = mipData;
          if (index == 0 || hasMipMaps2)
            newCubeTexture.SetData<byte>(CubeMapFace.NegativeY, index, new Rectangle?(), mipData, 0, numBytes);
          else
            break;
        }
        for (int index = 0; index < numMip; ++index)
        {
          int numBytes = 0;
          byte[] mipData = DDSLib.mipData;
          DDSLib.GetMipMaps(num6 * 4 + streamOffset, index, hasMipMaps1, num2, num1, isCompressed, compressionFormat, num5, partOfCubeMap, reader, loadSurfaceFormat, ref mipData, out numBytes);
          DDSLib.mipData = mipData;
          if (index == 0 || hasMipMaps2)
            newCubeTexture.SetData<byte>(CubeMapFace.PositiveZ, index, new Rectangle?(), mipData, 0, numBytes);
          else
            break;
        }
        for (int index = 0; index < numMip; ++index)
        {
          int numBytes = 0;
          byte[] mipData = DDSLib.mipData;
          DDSLib.GetMipMaps(num6 * 5 + streamOffset, index, hasMipMaps1, num2, num1, isCompressed, compressionFormat, num5, partOfCubeMap, reader, loadSurfaceFormat, ref mipData, out numBytes);
          DDSLib.mipData = mipData;
          if (index == 0 || hasMipMaps2)
            newCubeTexture.SetData<byte>(CubeMapFace.NegativeZ, index, new Rectangle?(), mipData, 0, numBytes);
          else
            break;
        }
        texture = (Texture) newCubeTexture;
      }
      else if (flag1)
      {
        Texture3D newTexture3D = DDSLib.GenerateNewTexture3D(loadSurfaceFormat, compressionFormat, device, num2, num1, num3, hasMipMaps2, num4, num5);
        int offsetInStream = streamOffset;
        for (int index = 0; index < newTexture3D.LevelCount; ++index)
        {
          int num6 = DDSLib.MipMapSize(index, num2);
          int num7 = DDSLib.MipMapSize(index, num1);
          int num8 = DDSLib.MipMapSize(index, num3);
          for (int front = 0; front < num8; ++front)
          {
            int numBytes = 0;
            byte[] mipData = DDSLib.mipData;
            DDSLib.GetMipMaps(offsetInStream, 0, hasMipMaps1, num6, num7, isCompressed, compressionFormat, num5, partOfCubeMap, reader, loadSurfaceFormat, ref mipData, out numBytes);
            offsetInStream += numBytes;
            DDSLib.mipData = mipData;
            newTexture3D.SetData<byte>(index, 0, 0, num6, num7, front, front + 1, mipData, 0, numBytes);
          }
        }
        texture = (Texture) newTexture3D;
      }
      else
      {
        Texture2D newTexture2D = DDSLib.GenerateNewTexture2D(loadSurfaceFormat, compressionFormat, device, num2, num1, hasMipMaps2, num4, num5);
        for (int index = 0; index < newTexture2D.LevelCount; ++index)
        {
          int numBytes = 0;
          byte[] mipData = DDSLib.mipData;
          DDSLib.GetMipMaps(streamOffset, index, hasMipMaps1, num2, num1, isCompressed, compressionFormat, num5, partOfCubeMap, reader, loadSurfaceFormat, ref mipData, out numBytes);
          DDSLib.mipData = mipData;
          newTexture2D.SetData<byte>(index, new Rectangle?(), mipData, 0, numBytes);
        }
        texture = (Texture) newTexture2D;
      }
    }

    private static bool IsXNATextureCompressed(Texture texture)
    {
      return texture.Format == SurfaceFormat.Dxt1 || texture.Format == SurfaceFormat.Dxt3 || texture.Format == SurfaceFormat.Dxt5;
    }

    private static DDSLib.FourCC XNATextureFourCC(Texture texture)
    {
      if (texture.Format == SurfaceFormat.Rgba64)
        return DDSLib.FourCC.D3DFMT_A16B16G16R16;
      if (texture.Format == SurfaceFormat.Vector4)
        return DDSLib.FourCC.D3DFMT_A32B32G32R32F;
      if (texture.Format == SurfaceFormat.Vector2)
        return DDSLib.FourCC.D3DFMT_G32R32F;
      if (texture.Format == SurfaceFormat.HalfVector2)
        return DDSLib.FourCC.D3DFMT_G16R16F;
      if (texture.Format == SurfaceFormat.NormalizedByte4)
        return DDSLib.FourCC.D3DFMT_Q8W8V8U8;
      if (texture.Format == SurfaceFormat.NormalizedByte2)
        return DDSLib.FourCC.D3DFMT_CxV8U8;
      if (texture.Format == SurfaceFormat.HalfVector4)
        return DDSLib.FourCC.D3DFMT_A16B16G16R16F;
      if (texture.Format == SurfaceFormat.Single)
        return DDSLib.FourCC.D3DFMT_R32F;
      if (texture.Format == SurfaceFormat.HalfSingle)
        return DDSLib.FourCC.D3DFMT_R16F;
      if (texture.Format == SurfaceFormat.Dxt1)
        return DDSLib.FourCC.D3DFMT_DXT1;
      if (texture.Format == SurfaceFormat.Dxt3)
        return DDSLib.FourCC.D3DFMT_DXT3;
      return texture.Format == SurfaceFormat.Dxt5 ? DDSLib.FourCC.D3DFMT_DXT5 : (DDSLib.FourCC) 0;
    }

    private static int XNATextureColorDepth(Texture texture)
    {
      return DDSLib.XNATextureNumBytesPerPixel(texture) * 8;
    }

    private static int XNATextureNumBytesPerPixel(Texture texture)
    {
      int num;
      switch (texture.Format)
      {
        case SurfaceFormat.Color:
        case SurfaceFormat.NormalizedByte4:
        case SurfaceFormat.Rgba1010102:
        case SurfaceFormat.Rg32:
        case SurfaceFormat.Single:
        case SurfaceFormat.HalfVector2:
          num = 4;
          break;
        case SurfaceFormat.Bgr565:
        case SurfaceFormat.Bgra5551:
        case SurfaceFormat.Bgra4444:
        case SurfaceFormat.NormalizedByte2:
        case SurfaceFormat.HalfSingle:
          num = 2;
          break;
        case SurfaceFormat.Dxt1:
        case SurfaceFormat.Dxt3:
        case SurfaceFormat.Dxt5:
          num = 0;
          break;
        case SurfaceFormat.Rgba64:
        case SurfaceFormat.Vector2:
        case SurfaceFormat.HalfVector4:
          num = 8;
          break;
        case SurfaceFormat.Alpha8:
          num = 1;
          break;
        case SurfaceFormat.Vector4:
          num = 16;
          break;
        default:
          throw new Exception(((int) texture.Format).ToString() + " has no save as DDS support.");
      }
      return num;
    }

    private static void GenerateDdspf(SurfaceFormat fileFormat, out uint flags, out uint rgbBitCount, out uint rBitMask, out uint gBitMask, out uint bBitMask, out uint aBitMask, out uint fourCC)
    {
      switch (fileFormat)
      {
        case SurfaceFormat.Color:
          flags = 65U;
          rgbBitCount = 32U;
          fourCC = 0U;
          rBitMask = 16711680U;
          gBitMask = 65280U;
          bBitMask = (uint) byte.MaxValue;
          aBitMask = 4278190080U;
          break;
        case SurfaceFormat.Bgr565:
          flags = 64U;
          fourCC = 0U;
          rgbBitCount = 16U;
          rBitMask = 63488U;
          gBitMask = 2016U;
          bBitMask = 31U;
          aBitMask = 0U;
          break;
        case SurfaceFormat.Bgra5551:
          flags = 65U;
          rgbBitCount = 16U;
          fourCC = 0U;
          rBitMask = 31744U;
          gBitMask = 992U;
          bBitMask = 31U;
          aBitMask = 32768U;
          break;
        case SurfaceFormat.Bgra4444:
          flags = 65U;
          rgbBitCount = 16U;
          fourCC = 0U;
          rBitMask = 3840U;
          gBitMask = 240U;
          bBitMask = 15U;
          aBitMask = 61440U;
          break;
        case SurfaceFormat.Dxt1:
        case SurfaceFormat.Dxt3:
        case SurfaceFormat.Dxt5:
          flags = 4U;
          rgbBitCount = 0U;
          rBitMask = 0U;
          gBitMask = 0U;
          bBitMask = 0U;
          aBitMask = 0U;
          fourCC = 0U;
          if (fileFormat == SurfaceFormat.Dxt1)
            fourCC = 827611204U;
          if (fileFormat == SurfaceFormat.Dxt3)
            fourCC = 861165636U;
          if (fileFormat != SurfaceFormat.Dxt5)
            break;
          fourCC = 894720068U;
          break;
        case SurfaceFormat.NormalizedByte2:
          flags = 4U;
          fourCC = 117U;
          rgbBitCount = 16U;
          rBitMask = (uint) byte.MaxValue;
          gBitMask = 65280U;
          bBitMask = 0U;
          aBitMask = 0U;
          break;
        case SurfaceFormat.NormalizedByte4:
          flags = 524288U;
          fourCC = 63U;
          rgbBitCount = 32U;
          rBitMask = (uint) byte.MaxValue;
          gBitMask = 65280U;
          bBitMask = 16711680U;
          aBitMask = 4278190080U;
          break;
        case SurfaceFormat.Rgba1010102:
          flags = 65U;
          fourCC = 0U;
          rgbBitCount = 32U;
          rBitMask = 1072693248U;
          gBitMask = 1047552U;
          bBitMask = 1023U;
          aBitMask = 3221225472U;
          break;
        case SurfaceFormat.Rg32:
          flags = 64U;
          fourCC = 0U;
          rgbBitCount = 32U;
          rBitMask = (uint) ushort.MaxValue;
          gBitMask = 4294901760U;
          bBitMask = 0U;
          aBitMask = 0U;
          break;
        case SurfaceFormat.Rgba64:
          flags = 4U;
          fourCC = 36U;
          rgbBitCount = 64U;
          rBitMask = 0U;
          gBitMask = 0U;
          bBitMask = 0U;
          aBitMask = 0U;
          break;
        case SurfaceFormat.Alpha8:
          flags = 2U;
          fourCC = 0U;
          rgbBitCount = 8U;
          rBitMask = 0U;
          gBitMask = 0U;
          bBitMask = 0U;
          aBitMask = (uint) byte.MaxValue;
          break;
        case SurfaceFormat.Single:
          flags = 4U;
          fourCC = 114U;
          rgbBitCount = 0U;
          rBitMask = 0U;
          gBitMask = 0U;
          bBitMask = 0U;
          aBitMask = 0U;
          break;
        case SurfaceFormat.Vector2:
          flags = 4U;
          fourCC = 115U;
          rgbBitCount = 0U;
          rBitMask = 0U;
          gBitMask = 0U;
          bBitMask = 0U;
          aBitMask = 0U;
          break;
        case SurfaceFormat.Vector4:
          flags = 4U;
          fourCC = 116U;
          rgbBitCount = 0U;
          rBitMask = 0U;
          gBitMask = 0U;
          bBitMask = 0U;
          aBitMask = 0U;
          break;
        case SurfaceFormat.HalfSingle:
          flags = 4U;
          fourCC = 111U;
          rgbBitCount = 0U;
          rBitMask = 0U;
          gBitMask = 0U;
          bBitMask = 0U;
          aBitMask = 0U;
          break;
        case SurfaceFormat.HalfVector2:
          flags = 4U;
          fourCC = 112U;
          rgbBitCount = 0U;
          rBitMask = 0U;
          gBitMask = 0U;
          bBitMask = 0U;
          aBitMask = 0U;
          break;
        case SurfaceFormat.HalfVector4:
          flags = 4U;
          fourCC = 113U;
          rgbBitCount = 0U;
          rBitMask = 0U;
          gBitMask = 0U;
          bBitMask = 0U;
          aBitMask = 0U;
          break;
        default:
          throw new Exception("Unsuported format");
      }
    }

    private static void WriteTexture(BinaryWriter writer, CubeMapFace face, Texture texture, bool saveMipMaps, int width, int height, bool isCompressed, DDSLib.FourCC fourCC, int rgbBitCount)
    {
      int levelCount = texture.LevelCount;
      int num1 = saveMipMaps ? levelCount : 1;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        int length = DDSLib.MipMapSizeInBytes(index1, width, height, isCompressed, fourCC, rgbBitCount);
        byte[] numArray = DDSLib.mipData;
        if (numArray == null || numArray.Length < length)
          numArray = new byte[length];
        if (texture is TextureCube)
          (texture as TextureCube).GetData<byte>(face, numArray);
        if (texture is Texture2D)
          (texture as Texture2D).GetData<byte>(index1, new Rectangle?(), numArray, 0, length);
        if (texture.Format == SurfaceFormat.Color)
        {
          int index2 = 0;
          while (index2 < length - 3)
          {
            byte num2 = numArray[index2];
            byte num3 = numArray[index2 + 2];
            numArray[index2] = num3;
            numArray[index2 + 2] = num2;
            index2 += 4;
          }
        }
        writer.Write(numArray, 0, length);
        DDSLib.mipData = numArray;
      }
    }

    private static void WriteTexture(BinaryWriter writer, CubeMapFace face, Texture texture, int mipLevel, int depth, int width, int height, bool isCompressed, DDSLib.FourCC fourCC, int rgbBitCount)
    {
      int length = DDSLib.MipMapSizeInBytes(mipLevel, width, height, isCompressed, fourCC, rgbBitCount);
      byte[] numArray = DDSLib.mipData;
      if (numArray == null || numArray.Length < length)
        numArray = new byte[length];
      if (texture is TextureCube)
        (texture as TextureCube).GetData<byte>(face, numArray);
      if (texture is Texture2D)
        (texture as Texture2D).GetData<byte>(mipLevel, new Rectangle?(), numArray, 0, length);
      if (texture is Texture3D)
      {
        Texture3D texture3D = texture as Texture3D;
        int right = DDSLib.MipMapSize(mipLevel, width);
        int bottom = DDSLib.MipMapSize(mipLevel, height);
        texture3D.GetData<byte>(mipLevel, 0, 0, right, bottom, depth, depth + 1, numArray, 0, length);
      }
      if (texture.Format == SurfaceFormat.Color)
      {
        int index = 0;
        while (index < length - 3)
        {
          byte num1 = numArray[index];
          byte num2 = numArray[index + 2];
          numArray[index] = num2;
          numArray[index + 2] = num1;
          index += 4;
        }
      }
      writer.Write(numArray, 0, length);
      DDSLib.mipData = numArray;
    }

    public static void DDSToStream(Stream stream, int streamOffset, bool saveMipMaps, Texture texture)
    {
      if (stream == null)
        throw new Exception("Can't write to a null stream");
      if (texture == null || texture.IsDisposed)
        throw new Exception("Can't read from a null texture.");
      Texture2D texture2D = texture as Texture2D;
      Texture3D texture3D = texture as Texture3D;
      TextureCube textureCube = texture as TextureCube;
      BinaryWriter writer = new BinaryWriter(stream);
      writer.BaseStream.Seek((long) streamOffset, SeekOrigin.Begin);
      writer.Write(542327876U);
      writer.Write(124);
      int num1 = 4103;
      bool isCompressed = DDSLib.IsXNATextureCompressed(texture);
      int num2 = isCompressed ? num1 | 524288 : num1 | 8;
      if (texture.LevelCount > 1 && saveMipMaps)
        num2 |= 131072;
      if (texture3D != null)
        num2 |= 8388608;
      writer.Write(num2);
      int width = 1;
      int height = 1;
      if (texture2D != null)
      {
        width = texture2D.Width;
        height = texture2D.Height;
      }
      if (texture3D != null)
      {
        width = texture3D.Width;
        height = texture3D.Height;
      }
      if (textureCube != null)
      {
        width = textureCube.Size;
        height = textureCube.Size;
      }
      writer.Write(height);
      writer.Write(width);
      uint num3 = !isCompressed ? (uint) (width * DDSLib.XNATextureNumBytesPerPixel(texture)) : (uint) ((width + 3) / 4 * ((height + 3) / 4) * (texture.Format != SurfaceFormat.Dxt1 ? 8 : 16));
      writer.Write(num3);
      if (texture3D != null)
        writer.Write(texture3D.Depth);
      else
        writer.Write(0);
      int num4 = texture.LevelCount == 1 ? 0 : texture.LevelCount;
      if (!saveMipMaps)
        num4 = 0;
      writer.Write(num4);
      for (int index = 0; index < 11; ++index)
        writer.Write(0);
      uint flags;
      uint rgbBitCount;
      uint rBitMask;
      uint gBitMask;
      uint bBitMask;
      uint aBitMask;
      uint fourCC;
      DDSLib.GenerateDdspf(texture.Format, out flags, out rgbBitCount, out rBitMask, out gBitMask, out bBitMask, out aBitMask, out fourCC);
      writer.Write(32);
      writer.Write(flags);
      writer.Write(fourCC);
      writer.Write(rgbBitCount);
      writer.Write(rBitMask);
      writer.Write(gBitMask);
      writer.Write(bBitMask);
      writer.Write(aBitMask);
      uint num5 = 4096;
      if (texture.LevelCount > 1 && saveMipMaps)
        num5 = num5 | 4194304U | 8U;
      if (textureCube != null || texture3D != null)
        num5 |= 8U;
      writer.Write(num5);
      uint num6 = 0;
      if (textureCube != null)
        num6 = num6 | 512U | 2048U | 8192U | 32768U | 1024U | 4096U | 16384U;
      if (texture3D != null)
        num6 |= 2097152U;
      writer.Write(num6);
      for (int index = 0; index < 3; ++index)
        writer.Write(0);
      if (texture2D != null)
        DDSLib.WriteTexture(writer, CubeMapFace.PositiveX, texture, saveMipMaps, width, height, isCompressed, (DDSLib.FourCC) fourCC, (int) rgbBitCount);
      if (textureCube != null)
      {
        DDSLib.WriteTexture(writer, CubeMapFace.PositiveX, texture, saveMipMaps, width, height, isCompressed, (DDSLib.FourCC) fourCC, (int) rgbBitCount);
        DDSLib.WriteTexture(writer, CubeMapFace.NegativeX, texture, saveMipMaps, width, height, isCompressed, (DDSLib.FourCC) fourCC, (int) rgbBitCount);
        DDSLib.WriteTexture(writer, CubeMapFace.PositiveY, texture, saveMipMaps, width, height, isCompressed, (DDSLib.FourCC) fourCC, (int) rgbBitCount);
        DDSLib.WriteTexture(writer, CubeMapFace.NegativeY, texture, saveMipMaps, width, height, isCompressed, (DDSLib.FourCC) fourCC, (int) rgbBitCount);
        DDSLib.WriteTexture(writer, CubeMapFace.PositiveZ, texture, saveMipMaps, width, height, isCompressed, (DDSLib.FourCC) fourCC, (int) rgbBitCount);
        DDSLib.WriteTexture(writer, CubeMapFace.NegativeZ, texture, saveMipMaps, width, height, isCompressed, (DDSLib.FourCC) fourCC, (int) rgbBitCount);
      }
      if (texture3D == null)
        return;
      for (int index = 0; index < texture3D.LevelCount; ++index)
      {
        int num7 = DDSLib.MipMapSize(index, texture3D.Depth);
        for (int depth = 0; depth < num7; ++depth)
          DDSLib.WriteTexture(writer, CubeMapFace.PositiveX, texture, index, depth, width, height, isCompressed, (DDSLib.FourCC) fourCC, (int) rgbBitCount);
      }
    }

    public static void DDSToFile(string fileName, bool saveMipMaps, Texture texture, bool throwExceptionIfFileExist)
    {
      if (throwExceptionIfFileExist && File.Exists(fileName))
        throw new Exception("The file allready exists and \"throwExceptionIfFileExist\" is true");
      Stream stream = (Stream) null;
      try
      {
        stream = (Stream) File.Create(fileName);
        DDSLib.DDSToStream(stream, 0, saveMipMaps, texture);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        if (stream != null)
          stream.Close();
      }
    }

    public static int GetDataByteSize(Texture2D texture, int mipMapLevel)
    {
      if (texture.LevelCount <= mipMapLevel)
        return -1;
      return DDSLib.MipMapSizeInBytes(mipMapLevel, texture.Width, texture.Height, DDSLib.IsXNATextureCompressed((Texture) texture), DDSLib.XNATextureFourCC((Texture) texture), DDSLib.XNATextureColorDepth((Texture) texture));
    }

    public static int GetDataByteSize(TextureCube texture, int mipMapLevel)
    {
      if (texture.LevelCount <= mipMapLevel)
        return -1;
      return DDSLib.MipMapSizeInBytes(mipMapLevel, texture.Size, texture.Size, DDSLib.IsXNATextureCompressed((Texture) texture), DDSLib.XNATextureFourCC((Texture) texture), DDSLib.XNATextureColorDepth((Texture) texture));
    }

    [Flags]
    private enum FourCC : uint
    {
      D3DFMT_DXT1 = 827611204,
      D3DFMT_DXT2 = 844388420,
      D3DFMT_DXT3 = 861165636,
      D3DFMT_DXT4 = 877942852,
      D3DFMT_DXT5 = 894720068,
      DX10 = 808540228,
      DXGI_FORMAT_BC4_UNORM = 1429488450,
      DXGI_FORMAT_BC4_SNORM = 1395934018,
      DXGI_FORMAT_BC5_UNORM = 843666497,
      DXGI_FORMAT_BC5_SNORM = 1395999554,
      D3DFMT_R8G8_B8G8 = 1195525970,
      D3DFMT_G8R8_G8B8 = 1111970375,
      D3DFMT_A16B16G16R16 = 36,
      D3DFMT_Q16W16V16U16 = 110,
      D3DFMT_R16F = 111,
      D3DFMT_G16R16F = 112,
      D3DFMT_A16B16G16R16F = 113,
      D3DFMT_R32F = 114,
      D3DFMT_G32R32F = 115,
      D3DFMT_A32B32G32R32F = 116,
      D3DFMT_UYVY = 1498831189,
      D3DFMT_YUY2 = 844715353,
      D3DFMT_CxV8U8 = 117,
      D3DFMT_Q8W8V8U8 = 63,
    }

    private enum LoadSurfaceFormat
    {
      Unknown,
      Dxt1,
      Dxt3,
      Dxt5,
      R8G8B8,
      B8G8R8,
      Bgra5551,
      Bgra4444,
      Bgr565,
      Alpha8,
      X8R8G8B8,
      A8R8G8B8,
      A8B8G8R8,
      X8B8G8R8,
      RGB555,
      R32F,
      R16F,
      A32B32G32R32F,
      A16B16G16R16F,
      Q8W8V8U8,
      CxV8U8,
      G16R16F,
      G32R32F,
      G16R16,
      A2B10G10R10,
      A16B16G16R16,
    }
  }
}
