using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using CurrencyConverterService.Models;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace CurrencyConverterService.Models
{
    class RatesRecipient
    {
        Context dataBase;

        public RatesRecipient()
        {
            this.dataBase = new Context();
        }

        public List<Rates> GetRatesFromNBRB()
        {
            List<Rates> rates = new List<Rates>();
            foreach (var currencyNumber in currencyNumbers())
            {
                if (LastDate(currencyNumber).ToString("yyyy-M-d") != DateTime.Now.ToString("yyyy-M-d"))
                {
                    string Http = $"http://www.nbrb.by/API/ExRates/Rates/Dynamics/" +
                    $"{currencyNumber}?startDate={LastDate(currencyNumber).ToString("yyyy-M-d")}&endDate={DateTime.Now.ToString("yyyy-M-d")}";
                    WebRequest request = WebRequest.Create(Http);
                    request.Method = "GET";
                    HttpWebResponse responseObjectGet = null;
                    responseObjectGet = (HttpWebResponse)request.GetResponse();
                    string json = null;
                    using (Stream stream = responseObjectGet.GetResponseStream())
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        json = streamReader.ReadToEnd();
                        streamReader.Close();
                    }
                    foreach (var rate in JsonConvert.DeserializeObject<List<Rates>>(json))
                    {
                        rate.Bit = rate.Ask;
                        rate.CurrencyId = currencyNumber;
                        rates.Add(rate);
                    }
                }
                else if (LastDate(currencyNumber).ToString("yyyy-M-d") == DateTime.Now.ToString("yyyy-M-d"))
                {
                    continue;
                }
            }
            return rates;
        }

        private List<int> currencyNumbers()
        {
            IEnumerable<Currency> currencies = dataBase.Currencies;
            List<int> currencysNumbers = new List<int>();
            foreach (var currencie in currencies)
            {
                currencysNumbers.Add(currencie.Id);
            }
            return currencysNumbers;
        }

        private DateTime LastDate(int currencyNumber)
        {
            IEnumerable<Rates> rates = dataBase.Rates;
            var Date = rates.Where(rate => rate.CurrencyId == currencyNumber).OrderByDescending(rate => rate.Date);
            if (Date.Count() == 0)
            {
                DateTime date = new DateTime(2019,1,1);
                return date;
            }
            else
            {
                return Date.First().Date;
            }
        }
    }
}
