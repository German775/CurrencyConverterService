using System;
using System.Collections.Generic;
using System.Text;
using CurrencyConverterService.Models;
using System.Threading;
using System.Threading.Tasks;
using CurrencyConverterService.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace CurrencyConverterService
{
    class Service
    {
        public static bool ServiceWork = true;
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

            while (ServiceWork)
            {
                RunService();
                Thread.Sleep(Config().DataRefreshInterval);
            }
        }

        private Config Config()
        {
            try
            {
                string json;
                using (StreamReader jsonString = new StreamReader("Config.json"))
                {
                    json = jsonString.ReadToEnd();
                }
                return JsonConvert.DeserializeObject<Config>(json);
            }
            catch (Exception exception)
            {
                logging.AddError(exception.ToString());
                return null;
            }
        }

        private void RunService()
        {
            ratesRefreshing.AddRates(ratesRecipient.GetRatesFromNBRB());
        }
    }
}
