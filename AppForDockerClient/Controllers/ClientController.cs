using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForDockerClient.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private Random _random;
        private readonly object _syncObj = new object();

        [HttpGet]
        public async Task<string> GetInfo()
        {
            try
            {
                HttpClient client = new HttpClient();
                Uri url = new Uri("http://172.17.0.4:80/api/values");
                var response = await client.GetAsync(url);
                var contents = await response.Content.ReadAsStringAsync();
                return contents + $" - My client IP {GetLocalIpAddress()} - random number {GenerateRandomNumber()}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.Message;
            }
           
        }

        [HttpGet]
        [Route("HealthCheck")]
        public string HealthCheck()
        {
            return $" - My client IP {GetLocalIpAddress()} - random number {GenerateRandomNumber()}";
        }
        public string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }


        private int GenerateRandomNumber()
        {
            lock (_syncObj)
            {
                if (_random == null)
                    _random = new Random();
                return _random.Next();
            }
        }
    }
}