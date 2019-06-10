using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverterService.Models
{
    public class RatesRefreshing
    {
        public void AddRates(IEnumerable<Rates> rates)
        {
            var dataBase = new Context();
            foreach (var rate in rates)
            {
                dataBase.Rates.Add(rate);
            }
            dataBase.SaveChanges();
            new Logging().AddInformation("Rates table updated");
        }
    }
}
