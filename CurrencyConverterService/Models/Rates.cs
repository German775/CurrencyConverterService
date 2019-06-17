using System;

namespace CurrencyConverterService.Models
{
    public class Rates
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public double Ask { get; set; }
        public double Bit { get; set; }
        public DateTime Date { get; set; }
        public Currency Currency { get; set; }
    }
}
