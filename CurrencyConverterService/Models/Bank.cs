using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyConverterService.Models
{
    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Currency> Currency { get; set; }
    }
}
