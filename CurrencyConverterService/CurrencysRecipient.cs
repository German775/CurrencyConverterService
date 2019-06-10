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
        public List<CurrencyNBRB> currencyNBRB;
        const string Http = "http://www.nbrb.by/API/ExRates/Currencies";

        public CurrencysRecipient()
        {
            this.currencyNBRB = new List<CurrencyNBRB>();
        }

        public List<CurrencyNBRB> GetCurrencysNBRB()
        {
            try
            {
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
                var logging = new Logging();
                logging.AddError(exception.ToString());
            }
            return currencyNBRB;
        }
    }
}
