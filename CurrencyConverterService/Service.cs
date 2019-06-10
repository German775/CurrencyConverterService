using CurrencyConverterService.Configuration;
using CurrencyConverterService.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace CurrencyConverterService
{
    public class Service
    {
        public static bool ServiceWork = true;
        public Logging Logging;
        public CurrencyRefreshing CurrencyRefreshing;
        public RatesRecipient RatesRecipient;
        public RatesRefreshing RatesRefreshing;
        public Service()
        {
            this.Logging = new Logging();
            this.CurrencyRefreshing = new CurrencyRefreshing();
            this.RatesRecipient = new RatesRecipient();
            this.RatesRefreshing = new RatesRefreshing();
        }

        public void Start()
        {
            Logging.AddInformation("Servis start");
            CurrencyRefreshing.AddCurrencyForNBRB(new CurrencysRecipient().GetCurrencysNBRB());

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
                Logging.AddError(exception.ToString());
                return null;
            }
        }

        private void RunService()
        {
            RatesRefreshing.AddRates(RatesRecipient.GetRatesFromNBRB());
        }
    }
}
