using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audio_Record
{
    class RecordService
    {
        private IWaveIn wi;
        private WaveFileWriter wfw;

        private readonly string fileName = RecordConfig.FileName;
        private readonly int samplelate = RecordConfig.Samplelate;
        private readonly int mode = RecordConfig.Mode;

        public void AsyncStart()
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
            wfw = new WaveFileWriter(fileName, wi.WaveFormat);
        }

        private void wi_printWaveDevice()
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
            wi_printWaveDevice();
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
