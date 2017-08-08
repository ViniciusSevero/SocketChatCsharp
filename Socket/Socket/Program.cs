using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Socket
{
    class Server
    {
        List<TcpClient> clientes = new List<TcpClient>();

        static void Main(string[] args)
        {
            Server self = new Server();

            TcpListener server = new TcpListener(IPAddress.Parse("10.3.2.16"), 666);

            server.Start();
            Console.WriteLine("Server up no endereço 127.0.0.1:666. Esperando uma conexão...");
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Alguém se conectou.");
                self.clientes.Add(client);
                self.messageListener(client.GetStream(), client);
            }
           
        }

        public void sendToAll(byte[] msg)
        {
            foreach(TcpClient cliente in this.clientes)
            {
                cliente.GetStream().Write(msg, 0, msg.Length);
                cliente.GetStream().Flush();
            }
        }

        public void messageListener(NetworkStream stream, TcpClient client)
        {
            new Thread(() =>
            {
                while (true)
                {
                    while (!stream.DataAvailable) ;
                    Byte[] bytes = new Byte[client.Available];
                    stream.Read(bytes, 0, bytes.Length);
                    String msg = Encoding.UTF8.GetString(bytes);
                    Console.WriteLine(msg);
                    this.sendToAll(bytes);
                }
            }).Start();
        }
    }

}
