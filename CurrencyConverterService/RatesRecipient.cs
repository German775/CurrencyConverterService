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
        Logging logging;
        List<Rates> rates;
        CurrencysRecipient currencys;
        IEnumerable<Currency> currencysDB;
        Dictionary<int, int> currencyNumbers;
        List<string> Name;
        public RatesRecipient()
        {
            this.dataBase = new Context();
            this.logging = new Logging();
            this.rates = new List<Rates>();
            this.currencys = new CurrencysRecipient();
            this.currencyNumbers = new Dictionary<int, int>();
            this.Name = new List<string>();
        }

        public List<Rates> GetRatesFromNBRB()
        {
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
                        logging.AddError(exception.ToString());
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
            currencysDB = dataBase.Currencies;
            var currencysNBRB = currencys.GetCurrencysNBRB();
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
                        var currencysId = currencysNBRB.Where(currencyscy => currencyscy.Cur_Abbreviation == currencyDB.Name).Select(currencyscy => currencyscy.Cur_ID);
                        for (var i = 0; i < currencysIdBD.Count(); i++)
                        {
                            currencyNumbers.Add(currencysIdBD.ElementAt(i), currencysId.ElementAt(i));
                        }
                        Name.Add(currencyDB.Name);
                    }
                }
                catch (Exception exception)
                {
                    logging.AddError(exception.ToString());
                }
            }
            return currencyNumbers;
        }

        private DateTime LastDate(int currencyNumber)
        {
            IEnumerable<Rates> rates = dataBase.Rates;
            var date = rates.Where(rate => rate.CurrencyId == currencyNumber).OrderByDescending(rate => rate.Date);
            if (date.Count() == 0)
            {
                var startDate = new DateTime(2019, 1, 1);
                return startDate;
            }
            else
            {
                return date.First().Date.AddDays(+1);
            }
        }
    }
}
