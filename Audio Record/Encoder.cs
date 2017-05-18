using System.IO;
using NAudio.Lame;
using NAudio.Wave;

namespace Audio_Record
{
    static class Encoder
    {
        public static void ConvertWavStreamToMp3File(string wavFileName, string mp3FileName)
        {
            WaveFileReader wfr = new WaveFileReader(wavFileName);
            wfr.Seek(0, SeekOrigin.Begin);

            using (var retMs = new MemoryStream())
            using (var wtr = new LameMP3FileWriter(mp3FileName, wfr.WaveFormat, LAMEPreset.VBR_90))
            {
                wfr.CopyTo(wtr);
            }
        }
    }
}
