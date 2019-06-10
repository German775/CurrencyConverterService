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
        Context _dataBase;
        Logging _logging;
        public CurrencyRefreshing()
        {
            this._dataBase = new Context();
            this._logging = new Logging();
        }
        public void AddCurrencyForNBRB(IEnumerable<CurrencyNBRB> currencysNBRB)
        {
            try
            {
                const int numberAllCurrencies = 225;
                IEnumerable<Bank> banks = _dataBase.Banks;
                IEnumerable<Currency> currenciesNBRB = _dataBase.Currencies;
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
                        var currency = new Currency() { BankId = 1, Name = currencyNBRB.Cur_Abbreviation};
                        _dataBase.Currencies.Add(currency);
                    }
                    _dataBase.SaveChanges();
                    _logging.AddInformation("Currency table updated");
                }
            }
            catch (Exception exception)
            {
                _logging.AddError(exception.ToString());
            }
        }
    }
}
