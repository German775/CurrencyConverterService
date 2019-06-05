using System;
using System.Collections.Generic;
using System.Text;
using CurrencyConverterService.Models;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace CurrencyConverterService
{
    class Service
    {
        static bool serviceWork = true;
        const int interwal = 100000;
        StreamWriter logging = new StreamWriter("Log.txt", FileMode.OpenOrCreate);

        public void Start()
        {
            Console.WriteLine("Servis start");
            logging.WriteLine();
            var currencys = new CurrencysRecipient();
            var currencyRefreshing = new CurrencyRefreshing();
            currencyRefreshing.AddCurrencyForNBRB(currencys.GetCurrencysNBRB());
            while (serviceWork)
            {
                Run();                    
                Thread.Sleep(interwal);
            }
        }

        private void Run()
        {
            var ratesRecipient = new RatesRecipient();
            var ratesRefreshing = new RatesRefreshing();
            ratesRefreshing.AddRates(ratesRecipient.GetRatesFromNBRB());
            Console.WriteLine("Updating the NBRB database");
        }
    }
}
