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
        public static bool serviceWork = true;
        public const int interwal = 100000;
        public Logging logging;
        public CurrencysRecipient currencys;
        public CurrencyRefreshing currencyRefreshing;
        public RatesRecipient ratesRecipient;
        public RatesRefreshing ratesRefreshing;
        public Service()
        {
            this.logging = new Logging();
            this.currencys = new CurrencysRecipient();
            this.currencyRefreshing = new CurrencyRefreshing();
            this.ratesRecipient = new RatesRecipient();
            this.ratesRefreshing = new RatesRefreshing();
        }

        public void Start()
        {
            logging.AddInformation("Servis start");
            currencyRefreshing.AddCurrencyForNBRB(currencys.GetCurrencysNBRB());
            while (serviceWork)
            {
                RunService();
                Thread.Sleep(interwal);
            }
        }

        private void RunService()
        {
            ratesRefreshing.AddRates(ratesRecipient.GetRatesFromNBRB());
        }
    }
}
