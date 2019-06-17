using System;
using System.Collections.Generic;
using System.Linq;
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
            var data = dataBase.Rates.Max(rate => rate.Date);
            new Logging().AddInformation($"Rates table updated to: {data}");
        }
    }
}
