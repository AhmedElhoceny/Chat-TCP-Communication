using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCP_listener_V_2
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private TcpClient client;
        private NetworkStream networkStreamimg;
        TcpListener listener;
        Socket s;
        StringBuilder recievedMessage = new StringBuilder();
        private readonly IConfiguration configuration;
        public Worker(IConfiguration configuration)
        {
            this.configuration = configuration;
            listener = new TcpListener(IPAddress.Parse("192.168.1.158"), 8001);
            listener.Start();
            s = listener.AcceptSocket();
            Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                
                byte[] b = new byte[100];
                int k = s.Receive(b);
                Console.WriteLine("Recieved...");
                for (int i = 0; i < k; i++)
                {
                    recievedMessage.Append(Convert.ToChar(b[i]));
                }
                Console.WriteLine(recievedMessage);
                recievedMessage.Clear();
                ASCIIEncoding asen = new ASCIIEncoding();
                s.Send(asen.GetBytes("The string was recieved by the server."));
                Console.WriteLine("\nSent Acknowledgement");


                await Task.Delay(1000);
            }
        }

    }
}