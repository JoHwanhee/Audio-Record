using NAudio.Wave;
using System;
using System.Threading.Tasks;

namespace Audio_Record
{
    class RecordService
    {
        private IWaveIn wi;
        private WaveFileWriter wfw;

        private readonly string fileName    = RecordConfig.FileName;
        private readonly string filePath    = RecordConfig.FilePath;
        private readonly int    samplelate  = RecordConfig.Samplelate;
        private readonly int    mode        = RecordConfig.Mode;

        public void RecordStart()
        {
            Task.Factory.StartNew(run);
        }

        public void RecordStop()
        {
            wi.StopRecording();
        }

        private void initWI()
        {
            wi = new WaveIn(WaveCallbackInfo.FunctionCallback());
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
            wi.RecordingStopped += new EventHandler<StoppedEventArgs>(wi_RecordingStopped);
            wi.WaveFormat = new WaveFormat(samplelate, mode);
            wfw = new WaveFileWriter(filePath + fileName, wi.WaveFormat);
        }

        private void printWaveDevice()
        {
            int devcount = WaveIn.DeviceCount;
            Console.Out.WriteLine("Device Count: {0}", devcount);
            for (int c = 0; c < devcount; ++c)
            {
                WaveInCapabilities info = WaveIn.GetCapabilities(c);
                Console.Out.WriteLine("{0}, {1}", info.ProductName, info.Channels);
            }
        }

        private void run()
        {
            initWI();
            printWaveDevice();
            wi.StartRecording();
        }

        #region Event
        private void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            wfw.Write(e.Buffer, 0, e.BytesRecorded);
            wfw.Flush();
        }

        private void wi_RecordingStopped(object sender, StoppedEventArgs sargs)
        {
            Console.WriteLine("recording is stoped");
            wi.Dispose();
            wfw.Close();
        }
        #endregion
    }
}
