using System;
using System.Text.RegularExpressions;
using HammondsIBMS_Domain.Values;
using System.Diagnostics.Contracts;

namespace HammondsIBMS_Domain.Entities.Invoicing
{
    public class BillCycle
    {

        public DateTime Date {
            get 
            {
                if(_year==0 || _month==0)
                    return ConstantValues.MinDate;
                return new DateTime(_year,_month,1); 
            }
        }

        public int BillCycleNo { get; internal set; }

       

        private int _year;
        public int Year
        {
            get { return _year; }
            set
            {
                //DiagContract.Requires<ArgumentOutOfRangeException>(value > 1970 && value < 2050);
                _year = value;
                Setup(_year, _month);
            }
        }

        private int _month;
        public int Month
        {
            get { return _month; }
            set
            {
                //DiagContract.Requires<ArgumentOutOfRangeException>(value > 0 && value < 12);
                _month = value;
                Setup(_year, _month);
            }
        }

        public BillCycle(int year,int month)
        {
            Year = year;
            Month = month;
        }

        public BillCycle(string billCycle)
        {
            Contract.Assert(Regex.IsMatch(billCycle, @"^2[0-9]{3}-(0[1-9]|1[0-2])"),"String billCycle does not conform to format 2yyy-mm e.g. 2010-04");
            Year = int.Parse(billCycle.Substring(0, 4));
            Month = int.Parse(billCycle.Substring(5, 2));
        }

        public BillCycle(DateTime date):this(date.Year,date.Month)
        {
        }

        public BillCycle()
        {
            
        }

        public BillCycle(int billCycleNo)
        {
            var year = (billCycleNo/12) + ConstantValues.MinDate.Year;
            var month = billCycleNo%12;
            Year = month>0?year:year-1;
            Month = month>0?month:month+1;
        }

        public BillCycle AddCycles(int noOfCycles)
        {
            return new BillCycle(this.BillCycleNo+noOfCycles);
        }

        public new string ToString()
        {
            return Year + "-" + Month.ToString("00");
        }

        public string Formated
        {
            get { return Year + "-" + Month.ToString("00"); }
        }

        private void Setup(int year,int month)
        {
            if (month == 0 || year == 0)
                return;
            //Date=new DateTime(year,month,1);
            BillCycleNo = (year - ConstantValues.MinDate.Year)*12 + month;
        }
    }
}