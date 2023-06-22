using HotelManagement.Model.Services;
using HotelManagement.View.Admin.StatisticalManagement;
using LiveCharts.Wpf;
using LiveCharts;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace HotelManagement.ViewModel.AdminVM.StatisticalManagementVM
{
    public partial class StatisticalManagementVM : BaseVM
    {
        public Frame mainFrame { get; set; }
        public Card ButtonView { get; set; }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; OnPropertyChanged(); }
        }
        bool isChange = false;

        public ICommand LoadViewCM { get; set; }
        public ICommand StoreButtonNameCM { get; set; }
        public ICommand LoadAllStatisticalCM { get; set; }
        public ICommand ChangeIncomePeriodCM { get; set; }

        public ICommand LoadRoomTypeAndServiceStatiscalCM { get; set; }
        
        public ICommand ChangeRoomTypeRevenueCM { get; set; }
        public ICommand ChangeServiceTypeRevenueCM { get; set; }
        public ICommand ChangeTimeCM { get; set; }
        public StatisticalManagementVM() 
        {
            InitCBB();

            LoadViewCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                mainFrame = p;
            });

            StoreButtonNameCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ButtonView = p;
                p.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
                p.SetValue(ElevationAssist.ElevationProperty, Elevation.Dp3);
            });

            LoadAllStatisticalCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                

                ChangeView(p);
                mainFrame.Content = new IncomeStatiscalManagement();
            });

            LoadRoomTypeAndServiceStatiscalCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                
                ChangeView(p);
                mainFrame.Content = new RoomTypeAndService();
            });

            //ChangeIncomePeriodCM = new RelayCommand<IncomeStatiscalManagement>((p) => { return true; }, async (p) =>
            //{
            //    TextBox tbReveRate = (TextBox)p.FindName("tbReveRate");
            //    TextBox tbExpeRate = (TextBox)p.FindName("tbExpeRate");
            //    tbExpeRate.Text = "";
            //    tbReveRate.Text = "";
            //    isChange = true;
            //    IsLoading = true;
            //    await ChangeIncomePeriod();
            //    ChangeViewTrend(p);
            //    IsLoading = false;
            //});
            ChangeRoomTypeRevenueCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await ChangeRoomTypeRevenue();
            });
            ChangeServiceTypeRevenueCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await ChangeServiceTypeRevenue();
            });
            ChangeTimeCM = new RelayCommand<IncomeStatiscalManagement>((p) => { return true; }, async (p) =>
            {
                isChange = true;
                await ChangeViewIncome(p);
            });

        }
        public void ChangeView(Card p)
        {
            ButtonView.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            ButtonView.SetValue(ElevationAssist.ElevationProperty, Elevation.Dp3);
            ButtonView = p;
            p.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            p.SetValue(ElevationAssist.ElevationProperty, Elevation.Dp3);
        }

        public void ChangeViewTrend(IncomeStatiscalManagement p)
        {
            if (isChange == false) return;
            TextBox tbReveRate = (TextBox)p.FindName("tbReveRate");
            TextBox tbExpeRate = (TextBox)p.FindName("tbExpeRate");

            if (!string.IsNullOrEmpty(ReveRate))
            {
                if (ReveRate.StartsWith("-"))
                {
                    if (ReveRate == "-2")
                    {
                        ReveRate = "tăng";
                        tbReveRate.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    else if (ReveRate == "-3")
                    {
                        ReveRate = "0%";
                        tbReveRate.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        ReveRate = "giảm " + ReveRate.Substring(1);
                        tbReveRate.Foreground = new SolidColorBrush(Colors.Red);
                    }
                }

                else
                {
                    ReveRate = "tăng " + ReveRate;
                    tbReveRate.Foreground = new SolidColorBrush(Colors.Green);
                }

            }
            if (!string.IsNullOrEmpty(ExpeRate))
            {
                if (ExpeRate.StartsWith("-"))
                {
                    if (ExpeRate == "-2")
                    {
                        ExpeRate = "tăng";
                        tbExpeRate.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    else if (ExpeRate == "-3")
                    {
                        ExpeRate = "0%";
                        tbExpeRate.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        ExpeRate = "giảm " + ExpeRate.Substring(1);
                        tbExpeRate.Foreground = new SolidColorBrush(Colors.Green);

                    }
                }

                else
                {
                    ExpeRate = "tăng " + ExpeRate;
                    tbExpeRate.Foreground = new SolidColorBrush(Colors.Red);
                }

            }
            if (isChange == true) isChange = false;
        }
        private async Task ChangeViewIncome(IncomeStatiscalManagement p)
        {
            await LoadIncomeByMonth();
            TimeBox = SelectedMonth.ToString() + " " + SelectedYear.ToString().ToLower();
            ChangeViewTrend(p);
        }
        private void InitCBB()
        {
            ListFilterYear = new List<string>(OverviewStatisticService.Ins.GetListFilterYear());
            SelectedYear = ListFilterYear[0];
            ListFilterMonth = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                ListFilterMonth.Add("Tháng " + i.ToString());
            }
            SelectedMonth = "Tháng " + (DateTime.Now.Month.ToString());

            ListFilterYear2 = new List<string>(OverviewStatisticService.Ins.GetListFilterYear());
            SelectedYear2 = ListFilterYear[0];
            ListFilterMonth2 = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                ListFilterMonth2.Add("Tháng " + i.ToString());
            }
            SelectedMonth2 = "Tháng " + (DateTime.Now.Month.ToString());

            ListFilterYear3 = new List<string>(OverviewStatisticService.Ins.GetListFilterYear());
            SelectedYear3 = ListFilterYear[0];
            ListFilterMonth3 = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                ListFilterMonth3.Add("Tháng " + i.ToString());
            }
            SelectedMonth3 = "Tháng " + (DateTime.Now.Month.ToString());
        }
    }
}
