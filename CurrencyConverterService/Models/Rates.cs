using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CurrencyConverterService.Models
{
    public class Rates
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "Cur_ID")]
        public int CurrencyId { get; set; }

        [JsonProperty(PropertyName = "Cur_OfficialRate")]
        public double Ask { get; set; }
        public double Bit { get; set; }
        public DateTime Date { get; set; }
        public Currency Currency { get; set; }
    }
}
