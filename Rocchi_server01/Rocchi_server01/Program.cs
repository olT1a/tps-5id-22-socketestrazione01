using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SynchronousSocketListener
{
    public static string data = null;
    public static void StartListening()
    {
        byte[] bytes = new Byte[1024];
        byte[] msg;
        IPAddress ipAddress = System.Net.IPAddress.Parse("127.0.0.1");
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 5000);
        Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                Socket handler = listener.Accept();
                data = null;

                while (true)
                {
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    break;
                }
                Random r = new Random();
                int n = r.Next(1, 10);
                if (n > Convert.ToInt32(data))
                {
                    msg = Encoding.ASCII.GetBytes(n.ToString() + " e' il numero casuale");
                    handler.Send(msg);
                }
                else if (n == Convert.ToInt32(data))
                {
                    msg = Encoding.ASCII.GetBytes(n.ToString() + " i numeri sono uguali");
                    handler.Send(msg);
                }
                else
                {
                    msg = Encoding.ASCII.GetBytes(data.ToString() + " e' il numero inserito dall'utente");
                    handler.Send(msg);
                }
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();

    }

    public static int Main(String[] args)
    {
        StartListening();
        return 0;
    }
}
