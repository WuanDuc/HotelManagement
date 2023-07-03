using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HotelManagement.ViewModel.AdminVM.StatisticalManagementVM
{
    public partial class StatisticalManagementVM : BaseVM
    {
        private List<RoomTypeDTO> _ListRoomTypeRevenue;
        public List<RoomTypeDTO> ListRoomTypeRevenue
        {
            get { return _ListRoomTypeRevenue; }
            set { _ListRoomTypeRevenue = value; OnPropertyChanged(); }
        }
        private List<string> _ListFilterYear2;
        public List<string> ListFilterYear2
        {
            get { return _ListFilterYear2; }
            set { _ListFilterYear2 = value; OnPropertyChanged(); }
        }
        private string _SelectedYear2;
        public string SelectedYear2
        {
            get { return _SelectedYear2; }
            set { _SelectedYear2 = value; OnPropertyChanged(); }
        }
        private List<string> _ListFilterMonth2;
        public List<string> ListFilterMonth2
        {
            get { return _ListFilterMonth2; }
            set { _ListFilterMonth2 = value; OnPropertyChanged(); }
        }
        private string _SelectedMonth2;
        public string SelectedMonth2
        {
            get { return _SelectedMonth2; }
            set { _SelectedMonth2 = value; OnPropertyChanged(); }
        }
        private List<string> _ListFilterYear3;
        public List<string> ListFilterYear3
        {
            get { return _ListFilterYear3; }
            set { _ListFilterYear3 = value; OnPropertyChanged(); }
        }
        private string _SelectedYear3;
        public string SelectedYear3
        {
            get { return _SelectedYear3; }
            set { _SelectedYear3 = value; OnPropertyChanged(); }
        }
        private List<string> _ListFilterMonth3;
        public List<string> ListFilterMonth3
        {
            get { return _ListFilterMonth3; }
            set { _ListFilterMonth3 = value; OnPropertyChanged(); }
        }
        private string _SelectedMonth3;
        public string SelectedMonth3
        {
            get { return _SelectedMonth3; }
            set { _SelectedMonth3 = value; OnPropertyChanged(); }
        }
        private List<ServiceTypeDTO> _ListServiceTypeRevenue;
        public List<ServiceTypeDTO> ListServiceTypeRevenue
        {
            get { return _ListServiceTypeRevenue; }
            set { _ListServiceTypeRevenue = value; OnPropertyChanged(); }
        }

        private SeriesCollection _RoomTypeRevenuePieChart;
        public SeriesCollection RoomTypeRevenuePieChart
        {
            get { return _RoomTypeRevenuePieChart; }
            set { _RoomTypeRevenuePieChart = value; OnPropertyChanged(); }
        }

        private SeriesCollection _ServiceTypeRevenuePieChart;
        public SeriesCollection ServiceTypeRevenuePieChart
        {
            get { return _ServiceTypeRevenuePieChart; }
            set { _ServiceTypeRevenuePieChart = value; OnPropertyChanged(); }
        }

        public async Task ChangeRoomTypeRevenue()
        {
            int year = int.Parse(SelectedYear2.Substring(4));
            int month = int.Parse(SelectedMonth2.Substring(6));
            ListRoomTypeRevenue = await OverviewStatisticService.Ins.GetListRoomTypeRevenue(year, month);
            RoomTypeRevenuePieChart = await OverviewStatisticService.Ins.GetDataRoomTypePieChart(year, month);
        }
        
            public async Task ChangeServiceTypeRevenue()
        {
            int year = int.Parse(SelectedYear3.Substring(4));
            int month = int.Parse(SelectedMonth3.Substring(6));
            ListServiceTypeRevenue = await OverviewStatisticService.Ins.GetListServiceTypeRevenue(year, month);
            ServiceTypeRevenuePieChart = await OverviewStatisticService.Ins.GetDataServiceTypePieChart(year, month);
        }
    }
}
