using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CurrencyConverterService.Models;
using System.Linq;

namespace CurrencyConverterService
{
    public class CurrencyRefreshing
    {
        public void AddCurrencyForNBRB(IEnumerable<CurrencyNBRB> currencysNBRB)
        {
            var logging = new Logging();
            try
            {
                const int numberAllCurrencies = 225;
                const string name = "NBRB";
                var dataBase = new Context();
                var banks = dataBase.Banks;
                var amountBankCurrencies = dataBase.Currencies.Where(currencie => currencie.BankId == banks.Where(bank => bank.Name == name).Select(bank => bank.Id).FirstOrDefault()).Count();
                if (amountBankCurrencies == numberAllCurrencies)
                {
                    return;
                }
                else
                {
                    foreach (var currencyNBRB in currencysNBRB)
                    {
                        var currency = new Currency() { BankId = 1, Name = currencyNBRB.Cur_Abbreviation};
                        dataBase.Currencies.Add(currency);
                    }
                    dataBase.SaveChanges();
                    logging.AddInformation("Currency table updated");
                }
            }
            catch (Exception exception)
            {
                logging.AddError(exception.ToString());
            }
        }
    }
}
