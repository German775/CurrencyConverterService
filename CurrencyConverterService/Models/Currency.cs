using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CurrencyConverterService.Models
{
    class Currency
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public string Name { get; set; }
        public Bank Bank { get; set; }
        public List<Rates> Rates { get; set; }
    }
}
