using HotelManagement.Model;
using HotelManagement.Model.Services;
using HotelManagement.Utilities;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace HotelManagement.ViewModel.AdminVM.StatisticalManagementVM
{
    public partial class StatisticalManagementVM : BaseVM
    {
        private SeriesCollection _InComeData;
        public SeriesCollection InComeData
        {
            get { return _InComeData; }
            set { _InComeData = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _SelectedIncomePeriod;
        public ComboBoxItem SelectedIncomePeriod
        {
            get { return _SelectedIncomePeriod; }
            set { _SelectedIncomePeriod = value; OnPropertyChanged(); }
        }

        private string _SelectedIncomeTime;
        public string SelectedIncomeTime
        {
            get { return _SelectedIncomeTime; }
            set { _SelectedIncomeTime = value; OnPropertyChanged(); }
        }

        //private int selectedYear;
        //public int SelectedYear
        //{
        //    get { return selectedYear; }
        //    set { selectedYear = value; }
        //}
        private string _TimeBox;
        public string TimeBox
        {
            get { return _TimeBox; }
            set { _TimeBox = value; OnPropertyChanged(); }
        }


        private string _TrueIncome;
        public string TrueIncome
        {
            get { return _TrueIncome; }
            set { _TrueIncome = value; OnPropertyChanged(); }
        }

        private string _TotalIn;
        public string TotalIn
        {
            get { return _TotalIn; }
            set { _TotalIn = value; OnPropertyChanged(); }
        }

        //MID CARD========================
        private string _TotalOut;
        public string TotalOut
        {
            get { return _TotalOut; }
            set { _TotalOut = value; OnPropertyChanged(); }
        }

        private float _TotalInPc;  //this is for the horizontial bar, just for displaying
        public float TotalInPc
        {
            get { return _TotalInPc; }
            set { _TotalInPc = value; OnPropertyChanged(); }
        }

        private float _TotalOutPc;
        public float TotalOutPc
        {
            get { return _TotalOutPc; }
            set { _TotalOutPc = value; OnPropertyChanged(); }
        }

        private int totalBill;
        public int TotalBill
        {
            get { return totalBill; }
            set { totalBill = value; OnPropertyChanged(); }
        }

        private string rentalReve;
        public string RentalReve
        {
            get { return rentalReve; }
            set { rentalReve = value; OnPropertyChanged(); }
        }

        private string serviceReve;
        public string ServiceReve
        {
            get { return serviceReve; }
            set { serviceReve = value; OnPropertyChanged(); }
        }

        private string serviceExpe;
        public string ServiceExpe
        {
            get { return serviceExpe; }
            set { serviceExpe = value; OnPropertyChanged(); }
        }

        private string repairExpe;
        public string RepairExpe
        {
            get { return repairExpe; }
            set { repairExpe = value; OnPropertyChanged(); }
        }

        private string furnitureExpe;
        public string FurnitureExpe
        {
            get { return furnitureExpe; }
            set { furnitureExpe = value; OnPropertyChanged(); }
        }

        private string rentalPc;
        public string RentalPc
        {
            get { return rentalPc; }
            set { rentalPc = value; OnPropertyChanged(); }
        }

        private string servicePc;
        public string ServicePc
        {
            get { return servicePc; }
            set { servicePc = value; OnPropertyChanged(); }
        }

        private string furniturePc;
        public string FurniturePc
        {
            get { return furniturePc; }
            set { furniturePc = value; OnPropertyChanged(); }
        }

        private string serviceExPc;
        public string ServiceExPc
        {
            get { return serviceExPc; }
            set { serviceExPc = value; OnPropertyChanged(); }
        }

        private string repairPc;
        public string RepairPc
        {
            get { return repairPc; }
            set { repairPc = value; OnPropertyChanged(); }
        }

        private string reveRate;
        public string ReveRate
        {
            get { return reveRate; }
            set { reveRate = value; OnPropertyChanged(); }
        }

        private string expeRate;
        public string ExpeRate
        {
            get { return expeRate; }
            set { expeRate = value; OnPropertyChanged(); }
        }


        private int _LabelMaxValue;
        public int LabelMaxValue
        {
            get { return _LabelMaxValue; }
            set { _LabelMaxValue = value; OnPropertyChanged(); }
        }
        private List<string> _ListFilterYear;
        public List<string> ListFilterYear
        {
            get { return _ListFilterYear; }
            set { _ListFilterYear = value; OnPropertyChanged(); }
        }
        private string _SelectedYear;
        public string SelectedYear
        {
            get { return _SelectedYear; }
            set { _SelectedYear = value; OnPropertyChanged(); }
        }
        private List<string> _ListFilterMonth;
        public List<string> ListFilterMonth
        {
            get { return _ListFilterMonth; }
            set { _ListFilterMonth = value; OnPropertyChanged(); }
        }
        private string _SelectedMonth;
        public string SelectedMonth
        {
            get { return _SelectedMonth; }
            set { _SelectedMonth = value; OnPropertyChanged(); }
        }
       

        public void CalculateTrueIncome(List<double> l1, List<double> l2)
        {

            float totalin = 0, totalout = 0;

            foreach (float item in l1)
                totalin += item;
            foreach (float item in l2)
                totalout += item;

            float trueincome = totalin - totalout;
            FindMaxPercentage(totalin, totalout);


            TrueIncome = Helper.FormatVNMoney(trueincome);
            TotalIn = Helper.FormatVNMoney(totalin);
            TotalOut = Helper.FormatVNMoney(totalout);
        }

        public void FindMaxPercentage(float _in, float _out)
        {
            if (_in != 0 && _out != 0)
            {
                if (_in >= _out)
                {
                    TotalInPc = 100;
                    TotalOutPc = _out / _in * 100;
                }
                else
                {
                    TotalOutPc = 100;
                    TotalInPc = _in / _out * 100;
                }
            }
            else
            {
                if (_in == 0 && _out == 0)
                {
                    TotalInPc = 10;
                    TotalOutPc = 10;
                    return;
                }
                if (_in == 0)
                {
                    TotalInPc = 10;
                    TotalOutPc = 90;
                    return;
                }
                if (_out == 0)
                {
                    TotalInPc = 90;
                    TotalOutPc = 10;
                    return;
                }

            }

        }
        public void Calculate_RevExpPercentage(double a1, double a2, double a3, double a4, double a5)
        {
            Calculate_RevPercentage(a1, a2);
            Calculate_ExpPercentage(a3, a4,a5);
        }
        public void Calculate_RevPercentage(double a1, double a2)
        {
            if (a1 == 0)
            {
                if (a2 == 0)
                    RentalPc = ServicePc = "0%";
                else
                {
                    RentalPc = "0%";
                    ServicePc = "100%";
                }
                return;
            }
            if (a2 == 0)
            {
                if (a1 == 0)
                    RentalPc = ServicePc = "0%";
                else
                {
                    RentalPc = "100%";
                    ServicePc = "0%";
                }
                return;
            }

            RentalPc = Math.Round(a1 / (a1 + a2) * 100, 2).ToString() + "%";
            ServicePc = Math.Round(a2 / (a1 + a2) * 100, 2).ToString() + "%";
        }
        public void Calculate_ExpPercentage(double a3, double a4, double a5)
        {
            if (a3 == 0 && a4 == 0 && a5 == 0)
            {
                ServiceExPc = RepairPc = FurniturePc = "0%";
                return;
            }
            
            ServiceExPc = Math.Round(a3 / (a3 + a4 + a5) * 100, 2).ToString() + "%";
            RepairPc = Math.Round(a4 / (a3 + a4 + a5) * 100, 2).ToString() + "%";
            FurniturePc = Math.Round(a5 / (a3 + a4 + a5) * 100, 2).ToString() + "%";
            
        }
        public async Task LoadIncomeByMonth()
        {
     
            try
            {

                int year = int.Parse(SelectedYear.Substring(4));
                int month = int.Parse(SelectedMonth.Substring(6));
                LabelMaxValue = DayOfMonth(year,month);
                TotalBill = await OverviewStatisticService.Ins.GetBillQuantity(year, month);
                (List<double> dailyRevenue, double MonthServiceReve, double MonthRentalReve, string MonthRateStr) = await Task.Run(() => OverviewStatisticService.Ins.GetRevenueByMonth(year, month));
                (List<double> dailyExpense, double MonthServiceExpense, double MonthRepairCost, double FurnitureExpense, string MonthExpenseRateStr) = await Task.Run(() => OverviewStatisticService.Ins.GetExpenseByMonth(year,month));
                RentalReve = Helper.FormatVNMoney(MonthRentalReve);
                ServiceReve = Helper.FormatVNMoney(MonthServiceReve);
                ServiceExpe = Helper.FormatVNMoney(MonthServiceExpense);
                RepairExpe = Helper.FormatVNMoney(MonthRepairCost);
                FurnitureExpe = Helper.FormatVNMoney(FurnitureExpense);
                ReveRate = MonthRateStr;
                ExpeRate = MonthExpenseRateStr;

                dailyRevenue.Insert(0, 0);
                dailyExpense.Insert(0, 0);

                CalculateTrueIncome(dailyRevenue, dailyExpense);
                Calculate_RevExpPercentage(MonthRentalReve, MonthServiceReve, MonthServiceExpense, MonthRepairCost, FurnitureExpense);

                for (int i = 1; i <= dailyRevenue.Count - 1; i++)
                {
                    dailyRevenue[i] /= 1000000;
                    dailyExpense[i] /= 1000000;
                }

                InComeData = new SeriesCollection
            {
            new LineSeries
            {
                Title = "Thu",
                Values = new ChartValues<double>(dailyRevenue),
                Fill = Brushes.Transparent,
            },
            new LineSeries
            {
                Title = "Chi",
                Values = new ChartValues<double>(dailyExpense),
                Fill = Brushes.Transparent,
            }
            };
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
            }
        }

        private int DayOfMonth(int year, int month)
        {
            switch (month)
            {
                case 1: case 3: case 5: case 7 : case 8: case 10: case 12:
                    {
                        return 31;
                    }
                case 2:
                    {
                        int plus = 0;
                        if (year % 400 == 0 || (year % 4 == 0 && year % 100 != 0)) plus = 1;
                        return 28 + plus;
                    }
                default:
                    {
                        return 30;
                    }
            }
        }
    }
}
