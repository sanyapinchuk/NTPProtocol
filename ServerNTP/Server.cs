using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpSample
{
    class Chat
    {
        /*private static IPAddress remoteIPAddress;
        private static int remotePort;
        private static int localPort;*/

        [STAThread]
        static void Main(string[] args)
        {
            string address = "127.0.0.1";
            IPAddress ipAddress = IPAddress.Parse(address);
            try
            {

                // Создаем поток для прослушивания
                Thread tRec = new Thread(new ThreadStart(Receiver));
                tRec.Start();

                while (true)
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
        }

        private static void Send(IPEndPoint endPoint)
        {
            var data = DateTime.Now;    
            Console.WriteLine("NOWTIME: " + data.ToString());
            // Создаем UdpClient
            UdpClient sender = new UdpClient();

            // Создаем endPoint по информации об удаленном хосте
            //IPEndPoint endPoint = new IPEndPoint(ipAddress, 123);

            try
            {
                // Преобразуем данные в массив байтов
                byte[] bytes = BitConverter.GetBytes(data.Ticks);

                Console.WriteLine("SERVER: send bytes: " + bytes);
                // Отправляем данные
                sender.Send(bytes, bytes.Length, endPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
            finally
            {
                // Закрыть соединение
                sender.Close();
            }
        }

        public static void Receiver()
        {
            // Создаем UdpClient для чтения входящих данных
            UdpClient receivingUdpClient = new UdpClient(123);

            IPEndPoint RemoteIpEndPoint = null;

            try
            {
                Console.WriteLine(
                   "\n-----------*******Общий чат*******-----------");

                while (true)
                {
                    // Ожидание дейтаграммы
                    byte[] receiveBytes = receivingUdpClient.Receive(
                       ref RemoteIpEndPoint);
                    Console.WriteLine("SERVER: gets:  " + receiveBytes);
                    Send(RemoteIpEndPoint);

                    // Преобразуем и отображаем данные
                    /*string returnData = Encoding.UTF8.GetString(receiveBytes);
                    Console.WriteLine(" --> " + returnData.ToString());*/
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
        }
    }
}