using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utilities;
using HotelManagement.Utils;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.View.Staff.RoomCatalogManagement.RoomInfo;
using HotelManagement.View.Staff.RoomCatalogManagement.RoomPayment;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelManagement.ViewModel.StaffVM.RoomCatalogManagementVM
{
    public partial class RoomCatalogManagementVM : BaseVM
    {


        private List<BillDTO> _ListBill;
        public List<BillDTO> ListBill
        {
            get { return _ListBill; }
            set { _ListBill = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _ListRoomByCustomer;
        public ObservableCollection<string> ListRoomByCustomer
        {
            get { return _ListRoomByCustomer; }
            set { _ListRoomByCustomer = value; OnPropertyChanged(); }
        }
        private ObservableCollection<string> _ListPaymentRoomNumber;
        public ObservableCollection<string> ListPaymentRoomNumber
        {
            get { return _ListPaymentRoomNumber; }
            set { _ListPaymentRoomNumber = value; OnPropertyChanged(); }
        }
        private List<RentalContractDTO> _ListRentalContractByCustomer;
        public List<RentalContractDTO> ListRentalContractByCustomer
        {
            get { return _ListRentalContractByCustomer; }
            set { _ListRentalContractByCustomer = value; OnPropertyChanged(); }
        }
        private ObservableCollection<BillDTO> _ListBillByListRentalContract;
        public ObservableCollection<BillDTO> ListBillByListRentalContract
        {
            get { return _ListBillByListRentalContract; }
            set { _ListBillByListRentalContract = value; OnPropertyChanged(); }
        }

        private BillDTO _BillPayment;
        public BillDTO BillPayment
        {
            get { return _BillPayment; }
            set { _BillPayment = value; OnPropertyChanged(); }
        }
        private BillDTO _SelectedRoomBill;
        public BillDTO SelectedRoomBill
        {
            get { return _SelectedRoomBill; }
            set { _SelectedRoomBill = value; OnPropertyChanged(); }
        }
        private string _StaffName;
        public string StaffName
        {
            get { return _StaffName; }
            set { _StaffName = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TroubleByCustomerDTO> _ListTroubleByCustomer;
        public ObservableCollection<TroubleByCustomerDTO> ListTroubleByCustomer
        {
            get { return _ListTroubleByCustomer; }
            set { _ListTroubleByCustomer = value; OnPropertyChanged(); }
        }
        public string CreateDateStr
        {
            get
            {
                return DateTime.Today.ToString("dd/MM/yyyy");
            }
        }
        private double _TotalMoneyPayment;
        public double TotalMoneyPayment
        {
            get { return _TotalMoneyPayment; }
            set { _TotalMoneyPayment = value; OnPropertyChanged(); }
        }
        private string _TotalMoneyPaymentStr;
        public string TotalMoneyPaymentStr
        {
            get { return _TotalMoneyPaymentStr; }
            set { _TotalMoneyPaymentStr = value; OnPropertyChanged(); }
        }
        public string HotelPhone
        {
            get
            {
                return HOTEL_INFO.PHONE;
            }
        }
        private double _TotalMoneyPaymentRoomGroup;
        public double TotalMoneyPaymentRoomGroup
        {
            get { return _TotalMoneyPaymentRoomGroup; }
            set { _TotalMoneyPaymentRoomGroup = value; OnPropertyChanged(); }
        }
        private string _TotalMoneyPaymentRoomGroupStr;
        public string TotalMoneyPaymentRoomGroupStr
        {
            get { return _TotalMoneyPaymentRoomGroupStr; }
            set { _TotalMoneyPaymentRoomGroupStr = value; OnPropertyChanged(); }
        }

        public async Task Payment()
        {
            ListRentalContractByCustomer = new List<RentalContractDTO>(await RentalContractService.Ins.GetRentalContractByCustomer(SelectedRoom.CustomerId));
            ListBillByListRentalContract = new ObservableCollection<BillDTO>(await BillService.Ins.GetBillByListRentalContract(ListRentalContractByCustomer));
            ListPaymentRoomNumber = new ObservableCollection<string>();

            if (ListRentalContractByCustomer != null)
            {
                if (ListRentalContractByCustomer.Count > 1)
                {
                    OptionPayment wd = new OptionPayment();
                    wd.lbCustomerName.Text = SelectedRoom.CustomerName;
                    ListRoomByCustomer = new ObservableCollection<string>(ListRentalContractByCustomer.Select(x => "Phòng " + x.RoomNumber).ToList());
                    wd.ShowDialog();
                }
                else
                {
                    RoomBill wd = new RoomBill();
                    BillPayment = ListBillByListRentalContract[0];
                    ListTroubleByCustomer = new ObservableCollection<TroubleByCustomerDTO>(BillPayment.ListTroubleByCustomer);
                    if (ListTroubleByCustomer.Count == 0)
                    {
                        wd.wrapperTrouble.Visibility = System.Windows.Visibility.Collapsed;
                    }

                    TotalMoneyPayment = 0;
                    wd.ShowDialog();
                }
            }
        }
        public async Task ChooseRoomPayment()
        {
            //string res = "";
            //foreach (var item in ListPaymentRoomNumber) { res += item + " "; }
            //MessageBox.Show(res);
            var list = new ObservableCollection<BillDTO>(await BillService.Ins.GetBillByListRentalContract(ListRentalContractByCustomer));
            ListBillByListRentalContract = new ObservableCollection<BillDTO>(list.Where(x => ListPaymentRoomNumber.Contains(x.RoomNumber.ToString())).ToList());
        }
        public async Task SaveBillFunc(RoomBill p)
        {
            BillDTO roomBillDTO = new BillDTO
            {
                RentalContractId = BillPayment.RentalContractId,
                StaffId = CurrentStaff.StaffId,
                NumberOfRentalDays = BillPayment.DayNumber,
                ServicePrice = BillPayment.ServicePriceTemp,
                TroublePrice = BillPayment.TroublePriceTemp,
                TotalPrice = BillPayment.TotalPriceTemp,
                CreateDate = DateTime.Now,
            };
            (bool isSucceed, string message) = await BillService.Ins.SaveBill(roomBillDTO);
            if (isSucceed)
            {
                CustomMessageBox.ShowOk(message, "Thông báo", "Ok", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                (bool isSucceed2, string message2) = await RoomService.Ins.ChangeRoomStatus(BillPayment.RoomId, BillPayment.RentalContractId);
                if (isSucceed2)
                {

                    p.Close();
                    if (roomGroupPayment != null) roomGroupPayment.Close();
                    RoomWindow.Close();
                    RefreshCM.Execute(MainPage);
                }
                else
                {
                    CustomMessageBox.ShowOk(message2, "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
                p.Close();
            }
            else
            {
                CustomMessageBox.ShowOk(message, "Thông báo", "Ok", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
            }
        }
        public async Task LoadRoomBillFunc()
        {
            TotalMoneyPayment = (double)BillPayment.TotalPriceTemp;
            TotalMoneyPaymentStr = Helper.FormatVNMoney2(TotalMoneyPayment);

        }
        private void FormatMoney(double money)
        {

            TotalMoneyPaymentRoomGroupStr = Helper.FormatVNMoney2(money);
        }
    }
}
