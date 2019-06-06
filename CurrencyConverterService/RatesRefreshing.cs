using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverterService.Models
{
    class RatesRefreshing
    {
        Context dataBase;
        Logging logging;
        public RatesRefreshing()
        {
            dataBase = new Context();
            this.logging = new Logging();
        }
        public void AddRates(List<Rates> rates)
        {
            try
            {
                foreach (var rate in rates)
                {
                    dataBase.Rates.Add(rate);
                }
                dataBase.SaveChanges();
                logging.AddInformation("Rates table updated");
            }
            catch (Exception exception)
            {
                logging.AddError(exception.ToString());
            }
        }
    }
}
