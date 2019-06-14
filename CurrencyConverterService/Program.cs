using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using CurrencyConverterService.Models;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConverterService
{
    class Program
    {
        static void Main(string[] args)
        {
            new Service().Start();
        }
    }
}
