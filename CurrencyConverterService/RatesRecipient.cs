using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace CurrencyConverterService.Models
{
    public class RatesRecipient
    {
        List<string> Name;
        public RatesRecipient()
        {
            this.Name = new List<string>();
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
                        foreach (var rateNBRB in JsonConvert.DeserializeObject<List<RatesNBRB>>(json))
                        {
                            var rate = new Rates(){ CurrencyId = currencyNumber.Key, Ask = rateNBRB.Cur_OfficialRate,
                                Bit = rateNBRB.Cur_OfficialRate, Date = rateNBRB.Date};
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
            var currencysNBRB = new CurrencysRecipient().GetCurrencysNBRB();
            var currencyNumbers = new Dictionary<int, int>();
            foreach (var currencyDB in new Context().Currencies)
            {
                try
                {
                    if ((Name.Where(name => name == currencyDB.Name).Count() > 0))
                    {
                        continue;
                    }
                    else
                    {
                        var currencysIdBD = ((IEnumerable<Currency>) new Context().Currencies).Where(currencyscy => currencyscy.Name == currencyDB.Name).Select(currencyscy => currencyscy.Id);
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
                    new Logging().AddError(exception.ToString());
                }
            }
            return currencyNumbers;
        }

        private DateTime LastDate(int currencyNumber)
        {
            var rates = new Context().Rates;
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
