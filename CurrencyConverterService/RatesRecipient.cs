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
            var rates = new List<Rates>();
            foreach (var currencyNumber in CurrencyNumbers())
            {
                if (LastDate(currencyNumber.Key).ToString("yyyy-M-d") != DateTime.Today.AddDays(+1).ToString("yyyy-M-d"))
                {
                    try
                    {
                        var Http = $"http://www.nbrb.by/API/ExRates/Rates/Dynamics/" +
                        $"{currencyNumber.Value}?startDate={LastDate(currencyNumber.Key).ToString("yyyy-M-d")}&endDate={DateTime.Now.ToString("yyyy-M-d")}";
                        var request = WebRequest.Create(Http);
                        request.Method = "GET";
                        HttpWebResponse responseObjectGet = null;
                        responseObjectGet = (HttpWebResponse)request.GetResponse();
                        string json = null;
                        using (var stream = responseObjectGet.GetResponseStream())
                        {
                            var streamReader = new StreamReader(stream);
                            json = streamReader.ReadToEnd();
                            streamReader.Close();
                        }
                        foreach (var rate in JsonConvert.DeserializeObject<List<Rates>>(json))
                        {
                            rate.Bit = rate.Ask;
                            rate.CurrencyId = currencyNumber.Key;
                            rates.Add(rate);
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                    
                }
                else if (LastDate(currencyNumber.Key).ToString("yyyy-M-d") == DateTime.Now.AddDays(+1).ToString("yyyy-M-d"))
                {
                    continue;
                }
            }
            return rates;
        }
        
        private Dictionary<int, int> CurrencyNumbers()
        {
            var currencys = new CurrencysRecipient();
            IEnumerable<Currency> currencysDB = dataBase.Currencies;
            var currencyNumbers = new Dictionary<int, int>();
            var CurrencysNBRB = currencys.GetCurrencysNBRB();
            var Name = new List<string>();

            foreach (var currencyDB in currencysDB)
            {
                try
                {
                    if ((Name.Where(name => name == currencyDB.Name).Count() > 0))
                    {
                        continue;
                    }
                    else
                    {
                        var currencysIdBD = currencysDB.Where(currencyscy => currencyscy.Name == currencyDB.Name).Select(currencyscy => currencyscy.Id);
                        var currencysId = CurrencysNBRB.Where(currencyscy => currencyscy.Cur_Abbreviation == currencyDB.Name).Select(currencyscy => currencyscy.Cur_ID);
                        for (var i = 0; i < currencysIdBD.Count(); i++)
                        {
                            currencyNumbers.Add(currencysIdBD.ElementAt(i), currencysId.ElementAt(i));
                        }
                        Name.Add(currencyDB.Name);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
            return currencyNumbers;
        }

        private DateTime LastDate(int currencyNumber)
        {
            IEnumerable<Rates> rates = dataBase.Rates;
            var Date = rates.Where(rate => rate.CurrencyId == currencyNumber).OrderByDescending(rate => rate.Date);
            if (Date.Count() == 0)
            {
                var date = new DateTime(2019, 1, 1);
                return date;
            }
            else
            {
                return Date.First().Date.AddDays(+1);
            }
        }
    }
}
