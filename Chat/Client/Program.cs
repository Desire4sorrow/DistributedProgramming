using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Client
{
    class Program
    {
        private const string defaultHost = "localhost";
        public static void StartClient(string server, int ipNumber, string message)
        {
            try
            {
                // Разрешение сетевых имён
                IPAddress ipAddress;
                //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                //IPAddress ipAddress = ipHostInfo.AddressList[0];
                if (server == defaultHost)
                {
                    ipAddress = IPAddress.Loopback;
                }
                else
                {
                    ipAddress = IPAddress.Parse(server);
                }

                IPEndPoint remoteEP = new IPEndPoint(ipAddress, ipNumber);

                // CREATE
                Socket sender = new Socket(
                    ipAddress.AddressFamily,
                    SocketType.Stream, 
                    ProtocolType.Tcp);

                try
                {
                    // CONNECT
                    sender.Connect(remoteEP);

                    // Подготовка данных к отправке

                    // SEND
                    int bytesSent = sender.Send(Encoding.UTF8.GetBytes(message));

                    // RECEIVE
                    byte[] buf = new byte[1024];

                    StringBuilder sb = new StringBuilder(); //для использования изменяемого прототипа строки(послед. символов)

                    do
                    {
                        int bytes = sender.Receive(buf, buf.Length, 0);
                        sb.Append(Encoding.UTF8.GetString(buf, 0, bytes));
                    }
                    while (sender.Available > 0);
                    
                    var sbCollection = JsonSerializer.Deserialize<List<string>>(sb.ToString());
                    foreach (var element in sbCollection)
                    {
                        Console.WriteLine(element);
                    }

                    // RELEASE
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Incorrect args");
            }
            else
            {
                StartClient(args[0], Int32.Parse(args[1]), args[2]);
            }
        }
    }
}
