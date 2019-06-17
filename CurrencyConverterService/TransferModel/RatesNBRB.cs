using System;

namespace CurrencyConverterService.Models
{
    public class RatesNBRB
    {
        public int Cur_ID { get; set; }
        public double Cur_OfficialRate { get; set; }
        public DateTime Date { get; set; }
        public Currency Currency { get; set; }
    }
}
