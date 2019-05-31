using System;
using System.Collections.Generic;
using System.Text;
using CurrencyConverterService.Models;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyConverterService
{
    class Service
    {
        static bool serviceWork = true;
        const int interwal = 100000;

        public void Start()
        {
            Console.WriteLine("Servis start");
            while (serviceWork)
            {
                try
                {
                    Run();                    
                    Thread.Sleep(interwal);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private void Run()
        {
            RatesRecipient ratesRecipient = new RatesRecipient();
            RatesRefreshing ratesRefreshing = new RatesRefreshing();
            ratesRefreshing.AddRates(ratesRecipient.GetRatesFromNBRB());
            Console.WriteLine("Received data from NBRB");
        }
    }
}
