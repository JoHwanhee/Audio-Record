using System.IO;
using NAudio.Lame;
using NAudio.Wave;
using System;

namespace Audio_Record
{
    internal static class Codec
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

            byte[] compress = new byte[data.Length];
            //compress = Lz4.CompressBytes(data, Lz4Mode.Fast);

            for (int i = 0; i < data.Length; i++)
            {
                compress[i] = (byte)NAudio.Codecs.MuLawDecoder.MuLawToLinearSample(data[i]);
            }


            return compress;
        }
        public static byte[] Decompress(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("input data to decompress should not be null");

            byte[] decompressed = new byte[data.Length];
            //decompressed = Lz4.DecompressBytes(data);

            for (int i = 0; i < data.Length; i++)
            {
                decompressed[i] = (byte)NAudio.Codecs.MuLawDecoder.MuLawToLinearSample(data[i]);
            }

            return decompressed;
        }
    }
}