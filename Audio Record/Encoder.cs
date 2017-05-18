#define _LZ4_
using System.IO;
using NAudio.Lame;
using NAudio.Wave;
using System;
using Lz4Net;

namespace Audio_Record
{
    internal static class Encoder
    {
        public static void ConvertWavStreamToMp3File(string wavFileName, string mp3FileName)
        {
            var wfr = new WaveFileReader(wavFileName);
            wfr.Seek(0, SeekOrigin.Begin);

            using (var mp3FileWriter = new LameMP3FileWriter(mp3FileName, wfr.WaveFormat, LAMEPreset.VBR_90))
            {

                wfr.CopyTo(mp3FileWriter);

            }
        }

        public static byte[] Compress(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("input data to compress should not be null");

            byte[] compress = null;
            compress = Lz4.CompressBytes(data, Lz4Mode.Fast);
            return compress;
        }
        public static byte[] Decompress(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("input data to decompress should not be null");

            byte[] decompressed = null;
            decompressed = Lz4.DecompressBytes(data);
            return decompressed;
        }
    }
}