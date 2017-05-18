using NAudio.Wave;
using System;
using System.Threading.Tasks;

namespace Audio_Record
{
    internal class RecordService
    {
        private IWaveIn _wi;
        private WaveFileWriter _wfw;

        private WaveOutEvent _woe;
        private BufferedWaveProvider _buffer;

        private readonly string _fileName    = RecordConfig.FileName;
        private readonly string _filePath    = RecordConfig.FilePath;
        private readonly int    _samplelate  = RecordConfig.Samplelate;
        private readonly int    _mode        = RecordConfig.Mode;

        public void RecordStart()
        {
            Task.Factory.StartNew(Run);
        }

        public void RecordStop()
        {
            _wi.StopRecording();
        }

        private void InitWi()
        {
            _wi = new WaveIn(WaveCallbackInfo.FunctionCallback());
            _wi.DataAvailable += Wi_DataAvailable;
            _wi.RecordingStopped += Wi_RecordingStopped;
            _wi.WaveFormat = new WaveFormat(_samplelate, _mode);
            _wfw = new WaveFileWriter(_filePath + _fileName, _wi.WaveFormat);
            _buffer = new BufferedWaveProvider(_wi.WaveFormat);
            _woe = new WaveOutEvent();
            _woe.Init(_buffer);



        }

        private static void PrintWaveDevice()
        {
            var devcount = WaveIn.DeviceCount;
            Console.Out.WriteLine("Device Count: {0}", devcount);
            for (var c = 0; c < devcount; ++c)
            {
                var info = WaveIn.GetCapabilities(c);
                Console.Out.WriteLine("{0}, {1}", info.ProductName, info.Channels);
            }
        }

        private void Run()
        {
            InitWi();
            PrintWaveDevice();
            _wi.StartRecording();
        }

   
        #region Event
        private void Wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            //currently the ACK is disabled because with the PCM data being so big it causes lag on audio
            byte[] data = Codec.Compress(e.Buffer);
            _buffer.AddSamples(Codec.Decompress(data), 0, e.BytesRecorded);
            _woe.Play();
           
            //int ret = SendData(Protocol.SEND_DATA, data);
            //Debug.WriteLine("Compressed size: {0:F2}%",100 * ((double)data.Length / (double)e.Buffer.Length));
            
            //_wfw.Write(e.Buffer, 0, e.BytesRecorded);
            //_wfw.Flush();
        }

        
        
        private void Wi_RecordingStopped(object sender, StoppedEventArgs sargs)
        {
            Console.WriteLine("recording is stoped");
            _wi.Dispose();
            _wfw.Close();
            
            //Encoder.ConvertWavStreamToMp3File(_filePath+_fileName, _filePath+"test.mp3");
        }
        #endregion
    }
}
