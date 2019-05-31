using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverterService.Models
{
    class RatesRefreshing
    {
        Context dataBase;
        public RatesRefreshing()
        {
            dataBase = new Context();
        }
        public void AddRates(List<Rates> rates)
        {
            foreach (var rate in rates)
            {
                dataBase.Rates.Add(rate);
            }
            dataBase.SaveChanges();
        }
    }
}
