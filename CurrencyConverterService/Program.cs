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

namespace CurrencyConverterService
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            RatesRecipient ratesRecipient = new RatesRecipient();
            RatesRefreshing ratesRefreshing = new RatesRefreshing();
            ratesRefreshing.AddRates(ratesRecipient.GetRatesFromNBRB());
            */
            Service service = new Service();
            service.Start();
        }
    }
}
