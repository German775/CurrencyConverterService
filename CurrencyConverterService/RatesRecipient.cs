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
    public class RatesRecipient
    {
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
                        new Logging().AddError(exception.ToString());
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
            var currencyNumbers1 = new Dictionary<int, int>();
            foreach (var currencyDB in new Context().Currencies)
            {
                try
                {
                    if ((new List<string>().Where(name => name == currencyDB.Name).Count() > 0))
                    {
                        continue;
                    }
                    else
                    {
                        for (var i = 0; i < ((IEnumerable<Currency>) new Context().Currencies).Where(Currencyscy => Currencyscy.Name == currencyDB.Name).Select(Currencyscy => Currencyscy.Id).Count(); i++)
                        {
                            currencyNumbers1.Add(((IEnumerable<Currency>) new Context().Currencies).Where(Currencyscy => Currencyscy.Name == currencyDB.Name).
                                Select(currencyscy => currencyscy.Id).ElementAt(i), new CurrencysRecipient().GetCurrencysNBRB().
                                    Where(currencyscy => currencyscy.Cur_Abbreviation == currencyDB.Name).
                                    Select(currencyscy => currencyscy.Cur_ID).ElementAt(i));
                        }
                        new List<string>().Add(currencyDB.Name);
                    }
                }
                catch (Exception exception)
                {
                    new Logging().AddError(exception.ToString());
                }
            }
            return currencyNumbers1;
        }

        private DateTime LastDate(int currencyNumber)
        {
            IEnumerable<Rates> rates = new Context().Rates;
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
