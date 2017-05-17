using System;
using NAudio.Wave;
using System.Threading.Tasks;
namespace Audio_Record
{
    class Program
    {
        IWaveIn wi;
        WaveFileWriter wfw;
        static bool isStop;

        static void Main(string[] args)
        {
            Program pr = new Program();
            pr.asyncStart();

            int operationCode = 0;
            while ((operationCode = Console.Read()) != 'z')
            {
                switch (operationCode)
                {
                    case 's':
                        isStop = true;
                        break;
                }

            }

        }

        Program() { }

        void asyncStart()
        {
            Task.Factory.StartNew(
            wi_StartRecording
            );
        }

        void wi_StartRecording()
        {
            wi = new WaveIn(WaveCallbackInfo.FunctionCallback());
            int devcount = WaveIn.DeviceCount;
            Console.Out.WriteLine("Device Count: {0}.", devcount);
            for (int c = 0; c < devcount; ++c)
            {
                WaveInCapabilities info = WaveIn.GetCapabilities(c);
                Console.Out.WriteLine("{0}, {1}", info.ProductName, info.Channels);
            }

            //If I use this instance, I can not set the sampling rate because
            //by defaut, the device does not support it.
            //wi = new WasapiCapture();

            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
            //wi.RecordingStopped += new EventHandler<StoppedEventArgs>(wi_RecordingStopped);
            wi.WaveFormat = new WaveFormat(44100, 1);
            wfw = new WaveFileWriter(@"./record.wav", wi.WaveFormat);

            wi.StartRecording();
        }


        void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            wfw.Write(e.Buffer, 0, e.BytesRecorded);
            wfw.Flush();

            if (isStop)
            {
                Console.Write("End the record");
                wi.StopRecording();
                wi.Dispose();
                wfw.Close();
            }
        }


        void wi_RecordingStopped(StoppedEventArgs sargs)
        {
           
        }


    }
}


