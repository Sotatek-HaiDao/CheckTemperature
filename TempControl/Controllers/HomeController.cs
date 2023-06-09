﻿using Azure.Core.Pipeline;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TempControl.Models;

namespace TempControl.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private static readonly HttpClient singletonHttpClientInstance = new HttpClient();
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {                
                var cred = new ManagedIdentityCredential();
                var client = new DigitalTwinsClient(
                new Uri(_configuration["DigitalTwinsUrl"]),
                cred,
                new DigitalTwinsClientOptions
                {
                    Transport = new HttpClientTransport(singletonHttpClientInstance)
                });
                var data = await client.GetDigitalTwinAsync<TwinsModel>("Factory");
                return View(data.Value);         
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}