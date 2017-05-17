using System;
using NAudio.Wave;
using System.Threading.Tasks;
namespace Audio_Record
{
    class Program
    {
       
        static void Main(string[] args)
        {
            RecordService rec = new RecordService();
             
            Console.WriteLine("녹음 시작 : p \n녹음 종료 : s\n프로그램 종료 : z");

            int operationCode = 0;
            while ((operationCode = Console.Read()) != 'z')
            {
                switch (operationCode)
                {
                    case 's':
                        rec.RecordStop();
                        break;

                    case 'p':
                        rec.RecordStart();
                        break;

                    default:
                        break;
                }
            }
        }

       
    }
}


