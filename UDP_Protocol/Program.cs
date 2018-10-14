using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDP_Protocol
{
    class Program
    {
        static int localPort; //порт для приема сообщений
        static int remotePort; //порт для отправки сообщений
        static Socket listeningSocket;

        static void Main(string[] args)
        {
            Console.WriteLine("Введите порт для приема сообщений: ");
            localPort = Convert.ToInt32((Console.ReadLine()));

            Console.WriteLine("Введите порт длдя отправки сообщений");
            remotePort = Convert.ToInt32((Console.ReadLine()));
            Console.WriteLine("Введите сообщение и нажмите Enter");
            Console.ReadKey();
            try
            {
                listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Task listeningTask = new Task(Listen);
                listeningTask.Start();
                //отправка сообщений на разные порты
                while (true)
                {
                    string message = Console.ReadLine();
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    EndPoint remotePoint = new EndPoint(IPAddress.Parse("127.0.0.1"), remotePort);
                    listeningSocket.SendTo(data, remotePoint);

                }


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }


        }


        private static void Listen()
        {
            try
            {
                IPEndPoint localIp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), remotePort);
                listeningSocket.Bind(localIp);
                while (true)
                {
                    StringBuilder bilder = new StringBuilder();
                    int bytes = 0;
                    byte[] data = new byte[256];
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
                    do
                    {
                        bytes = listeningSocket.ReceiveFrom(data, ref remoteIp);
                        bilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (listeningSocket.Available > 0);


                    IPEndPoint remoteFullIp = remoteIp as IPEndPoint;
                    //выводим сообщение
                    Console.WriteLine($"{remoteFullIp.Address}: {remoteFullIp.Port}- {bilder.ToString()} ");


                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                listeningSocket?.Shutdown(SocketShutdown.Both);
                listeningSocket?.Close();
                listeningSocket = null;
            }
        }

    }
}




