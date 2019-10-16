using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using HammondsIBMS_Domain.Model.Supplier;

namespace HammondsIBMS_Domain.Values
{
    public class ForeignCurrency
    {
        public decimal AmountNative { get; set; }
        public string Abbreviation { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Symbol { get; set; }
        public decimal AmountGBP
        {
            get { return AmountNative/(ExchangeRate==0?1:ExchangeRate); }
        }

        public ForeignCurrency()
        {
            
        }

        public ForeignCurrency(decimal amountNative, ForeignCurrency existingForeignCurrency)
        {
            AmountNative = amountNative;
            Symbol = existingForeignCurrency.Symbol;
            ExchangeRate = existingForeignCurrency.ExchangeRate;
            Abbreviation = existingForeignCurrency.Abbreviation;
        }

        public ForeignCurrency(decimal amountNative,string symbol,decimal exchangeRate,string abbreviation)
        {
            AmountNative = amountNative;
            Symbol = symbol;
            ExchangeRate = exchangeRate;
            Abbreviation = abbreviation;
        } 
        
        public ForeignCurrency(ExchangeRate exchangeRate)
        {
            UpdateDenomination(exchangeRate);
        }

        public ForeignCurrency UpdateDenomination(ExchangeRate exchangeRate)
        {
            Abbreviation = exchangeRate.Abbreviation;
            ExchangeRate = exchangeRate.RateToGBP;
            Symbol = exchangeRate.Symbol;
            return this;
        }

        public ForeignCurrency UpdateDenomination(ForeignCurrency fc)
        {
            Abbreviation = fc.Abbreviation;
            ExchangeRate = fc.ExchangeRate;
            Symbol = fc.Symbol;
            return this;
        }

    }



}