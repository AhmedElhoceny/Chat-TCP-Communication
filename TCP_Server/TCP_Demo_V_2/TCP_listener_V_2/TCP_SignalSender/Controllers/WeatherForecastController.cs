using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Net;

namespace TCP_SignalSender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private TcpListener listener;
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            listener = new TcpListener(IPAddress.Any, 1337);
            listener.Start();
        }

        [HttpPost(Name = "SendMessage")]
        public string SendMessage(string Message)
        {
            try
            {
                var tcpClient = listener.AcceptTcpClientAsync().Result;
                using NetworkStream ns = tcpClient.GetStream();
                using StreamWriter sw = new StreamWriter(ns);

                sw.WriteLine("Hello My Friend Again.........");

                sw.Flush();
                return "Done";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}