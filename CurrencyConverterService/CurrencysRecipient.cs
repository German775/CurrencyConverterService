using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace CurrencyConverterService
{
    class CurrencysRecipient
    {
        public List<CurrencyNBRB> GetCurrencysNBRB()
        {
            var currencyNBRB = new List<CurrencyNBRB>();
            try
            {
                var Http = "http://www.nbrb.by/API/ExRates/Currencies";
                var request = WebRequest.Create(Http);
                request.Method = "GET";
                HttpWebResponse responseObjectGet = null;
                responseObjectGet = (HttpWebResponse)request.GetResponse();
                string json = null;
                using (Stream stream = responseObjectGet.GetResponseStream())
                {
                    var streamReader = new StreamReader(stream);
                    json = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                foreach (var currency in JsonConvert.DeserializeObject<List<CurrencyNBRB>>(json))
                {
                    currencyNBRB.Add(currency);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            return currencyNBRB;
        }
    }
}
