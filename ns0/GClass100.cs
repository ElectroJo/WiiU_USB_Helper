// Decompiled with JetBrains decompiler
// Type: ns0.GClass100
// Assembly: WiiU_USB_Helper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8A8903FC-BD1C-4FCE-9A9B-6F50F8E0D0D6
// Assembly location: C:\0.6.1.655\WiiU_USB_Helper-cleaned.exe

using NusHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ns0
{
  public sealed class TMDExcractionAndProcessing : IDisposable
  {
    private byte[] byte_3 = new byte[64];
    private byte[] byte_4 = new byte[60];
    private byte[] byte_6 = new byte[58];
    private byte[] byte_7 = new byte[256];
    private uint uint_1 = 65537;
    private uint uint_0;
    private ushort ushort_2;
    private byte byte_2;
    private List<GClass101> list_0;
    private ushort ushort_3;
    private ushort ushort_4;
    private ushort ushort_5;
    private byte byte_5;
    private ushort ushort_6;
    private byte byte_8;
    private uint uint_2;
    private byte byte_9;
    private bool bool_0;

    public static byte[] Byte_0
    {
      get
      {
        return GClass97.byte_0;
      }
    }

    public byte[] Certificate1 { get; } = new byte[1024];

    public byte[] Certificate2 { get; } = new byte[768];

    public GClass101[] GClass101_0
    {
      get
      {
        return this.list_0.ToArray();
      }
      set
      {
        this.list_0 = new List<GClass101>((IEnumerable<GClass101>) value);
        this.NumOfContents = (ushort) value.Length;
      }
    }

    public ushort NumOfContents { get; private set; }

    public ulong TitleId { get; set; }

    public ushort TitleVersion { get; set; }

    private void method_0(Stream TMD_Stream, SystemType SystemType)
    {
      TMD_Stream.Seek(0L, SeekOrigin.Begin);
      byte[] buffer1 = new byte[8];
      TMD_Stream.Read(buffer1, 0, 4); //Reads the Signature Type https://3dbrew.org/wiki/Title_metadata#Signature_Type
      this.uint_1 = GClass27.ToUIntNetworkBytes(BitConverter.ToUInt32(buffer1, 0));
      TMD_Stream.Read(this.byte_7, 0, this.byte_7.Length); //Maybe Reads the signature?
      TMD_Stream.Read(this.byte_4, 0, this.byte_4.Length);
      TMD_Stream.Read(this.byte_3, 0, this.byte_3.Length);
      TMD_Stream.Read(buffer1, 0, 4);
      this.byte_9 = buffer1[0];
      this.byte_2 = buffer1[1];
      this.byte_8 = buffer1[2];
      this.byte_5 = buffer1[3];
      TMD_Stream.Read(buffer1, 0, 8); //Reads past System Version
      TMD_Stream.Read(buffer1, 0, 8); //Reads Title ID
      this.TitleId = GClass27.ToULongNetworkBytes(BitConverter.ToUInt64(buffer1, 0)); //Stores Title ID
      TMD_Stream.Read(buffer1, 0, 4); //Reads Title Type?
      this.uint_2 = GClass27.ToUIntNetworkBytes(BitConverter.ToUInt32(buffer1, 0)); //Stores Title Type
      TMD_Stream.Read(buffer1, 0, 2); //Group ID
      this.ushort_3 = GClass27.ToUShortNetworkBytes(BitConverter.ToUInt16(buffer1, 0));
      TMD_Stream.Read(buffer1, 0, 2); 
      this.ushort_4 = GClass27.ToUShortNetworkBytes(BitConverter.ToUInt16(buffer1, 0));
      TMD_Stream.Read(buffer1, 0, 2); 
      this.ushort_6 = GClass27.ToUShortNetworkBytes(BitConverter.ToUInt16(buffer1, 0));
      TMD_Stream.Read(this.byte_6, 0, this.byte_6.Length);
      TMD_Stream.Read(buffer1, 0, 4);
      this.uint_0 = GClass27.ToUIntNetworkBytes(BitConverter.ToUInt32(buffer1, 0));
      TMD_Stream.Read(buffer1, 0, 8); //Read Title Version, Content Count, Boot Content, and Padding
      this.TitleVersion = GClass27.ToUShortNetworkBytes(BitConverter.ToUInt16(buffer1, 0));
      this.NumOfContents = GClass27.ToUShortNetworkBytes(BitConverter.ToUInt16(buffer1, 2));
      this.ushort_2 = GClass27.ToUShortNetworkBytes(BitConverter.ToUInt16(buffer1, 4)); //Probably Boot Content
      this.ushort_5 = GClass27.ToUShortNetworkBytes(BitConverter.ToUInt16(buffer1, 6)); //Probably Padding?
      if (SystemType != SystemType.SystemWii)
        TMD_Stream.Position = 2820L;
      this.list_0 = new List<GClass101>();
      for (int index = 0; index < (int) this.NumOfContents; ++index)
      {
        GClass101 gclass101;
        if (SystemType != SystemType.SystemWiiU && SystemType != SystemType.SystemWii)
        {
          if (SystemType != SystemType.System3DS)
            throw new NotImplementedException();
          GClass102 gclass102 = new GClass102();
          gclass102.Hash = new byte[32];
          gclass101 = (GClass101) gclass102;
        }
        else
        {
          GClass103 gclass103 = new GClass103();
          gclass103.Hash = new byte[20];
          gclass101 = (GClass101) gclass103;
        }
        TMD_Stream.Read(buffer1, 0, 8);
        gclass101.ContentId = GClass27.ToUIntNetworkBytes(BitConverter.ToUInt32(buffer1, 0));
        gclass101.Index = GClass27.ToUShortNetworkBytes(BitConverter.ToUInt16(buffer1, 4));
        gclass101.GEnum6_0 = (GEnum6) GClass27.ToUShortNetworkBytes(BitConverter.ToUInt16(buffer1, 6));
        TMD_Stream.Read(buffer1, 0, 8);
        gclass101.Size = new DataSize(GClass27.ToULongNetworkBytes(BitConverter.ToUInt64(buffer1, 0)));
        TMD_Stream.Read(gclass101.Hash, 0, gclass101.Hash.Length);
        this.list_0.Add(gclass101);
        if (SystemType == SystemType.SystemWiiU)
        {
          byte[] buffer2 = new byte[12];
          TMD_Stream.Read(buffer2, 0, 12);
        }
      }
      TMD_Stream.Read(this.Certificate1, 0, this.Certificate1.Length);
      TMD_Stream.Read(this.Certificate2, 0, this.Certificate2.Length);
    }

    public void Dispose()
    {
      this.method_1(true);
      GC.SuppressFinalize((object) this);
    }

    ~TMDExcractionAndProcessing()
    {
      this.method_1(false);
    }

    private void method_1(bool Should_Wipe_Vars_Bool)
    {
      if (Should_Wipe_Vars_Bool && !this.bool_0)
      {
        this.byte_7 = (byte[]) null;
        this.byte_4 = (byte[]) null;
        this.byte_3 = (byte[]) null;
        this.byte_6 = (byte[]) null;
        this.list_0.Clear();
        this.list_0 = (List<GClass101>) null;
      }
      this.bool_0 = true;
    }

    public DataSize DataSize_0
    {
      get
      {
        return ((IEnumerable<GClass101>) this.GClass101_0).Aggregate<GClass101, DataSize>(new DataSize(0UL), (Func<DataSize, GClass101, DataSize>) ((dataSize_0, gclass101_0) => dataSize_0 + (gclass101_0.Size + 20UL * (gclass101_0.Size.TotalBytes / 256000000UL))));
      }
    }

    public static TMDExcractionAndProcessing smethod_0(string string_0, SystemType SystemType)
    {
      return TMDExcractionAndProcessing.smethod_1(File.ReadAllBytes(string_0), SystemType);
    }

    public static TMDExcractionAndProcessing smethod_1(byte[] Nintendo_TMD, SystemType SystemType)
    {
      TMDExcractionAndProcessing gclass100 = new TMDExcractionAndProcessing();
      MemoryStream TMD_MemoryStream = new MemoryStream(Nintendo_TMD);
      try
      {
        gclass100.method_0((Stream) TMD_MemoryStream, SystemType);
      }
      catch
      {
        TMD_MemoryStream.Dispose();
        throw;
      }
      TMD_MemoryStream.Dispose();
      return gclass100;
    }
  }
}
