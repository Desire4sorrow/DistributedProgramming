using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Server
{
    class Program
    {
        private const string CorrValue = "Invalid args. It didn't started";
        private const string InvalValue = "Correct args. It correctly started";

        public static List<string> msgCollection = new List<string>();
        public static void StartListening(string ipNumber)
        {

            // Разрешение сетевых имён
            
            // Привязываем сокет ко всем интерфейсам на текущей машинe
            IPAddress ipAddress = IPAddress.Any; 
            
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Convert.ToInt32(ipNumber));

            // CREATE
            Socket listener = new Socket(
                ipAddress.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

            try
            {
                // BIND
                listener.Bind(localEndPoint);

                // LISTEN
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Ожидание соединения клиента...");
                    // ACCEPT
                    Socket handler = listener.Accept();

                    Console.WriteLine("Получение данных...");
                    byte[] buf = new byte[1024];
                    string data = null;
                    while (true)
                    {
                        // RECEIVE
                        int bytesRec = handler.Receive(buf);

                        data += Encoding.UTF8.GetString(buf, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                    data = data.Remove(data.Length - 5, 5);
                    msgCollection.Add(data);

                    Console.WriteLine("Полученный текст: {0}", data);

                    // Отправляем текст обратно клиенту
                    byte[] msg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(msgCollection));

                    // SEND
                    handler.Send(msg);

                    // RELEASE
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Запуск сервера...");
            if (args.Length != 1)
            {
                Console.WriteLine(CorrValue);
            }
            else
            {
                StartListening((args[0]));
                Console.WriteLine(InvalValue);
            }

            Console.WriteLine("\nНажмите ENTER чтобы выйти...");
            Console.Read();
        }
    }
}
