using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CurrencyConverterService.Models;
using System.Linq;

namespace CurrencyConverterService
{
    class CurrencyRefreshing
    {
        Context dataBase;
        public CurrencyRefreshing()
        {
            this.dataBase= new Context();
        }
        public void AddCurrencyForNBRB(List<CurrencyNBRB> currencysNBRB)
        {
            try
            {
                const int numberAllCurrencies = 225;
                IEnumerable<Bank> banks = dataBase.Banks;
                IEnumerable<Currency> currenciesNBRB = dataBase.Currencies;
                var bankNBRBId = banks.Where(bank => bank.Name == "NBRB").Select(bank => bank.Id);
                var amountBankCurrencies = currenciesNBRB.Where(currencie => currencie.BankId == bankNBRBId.FirstOrDefault()).Count();
                if (amountBankCurrencies == numberAllCurrencies)
                {
                    return;
                }
                else
                {
                    foreach (var currencyNBRB in currencysNBRB)
                    {
                        var currency = new Currency();
                        currency.Name = currencyNBRB.Cur_Abbreviation;
                        currency.BankId = 1;
                        dataBase.Currencies.Add(currency);
                    }
                    dataBase.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
