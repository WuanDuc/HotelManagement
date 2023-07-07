using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.BookingRoomManagement;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.ViewModel.AdminVM;
using IronXL.Formatting;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotelManagement.ViewModel.BookingRoomManagementVM
{
    public partial class BookingRoomManagementVM:BaseVM
    {
        public StaffDTO currentStaff;
       
        private string _staffName { get; set; }
        public string StaffName
        {
            set
            {
                _staffName = value;
                OnPropertyChanged();
            }
            get { return _staffName; }
        }

        private string _StaffId { get; set; }
        public string StaffId
        {
            set
            {
                _StaffId = value;
                OnPropertyChanged();
            }
            get { return _StaffId; }
        }

        private string _customerName;
        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; OnPropertyChanged(); }
        }
        private string CustomerId;

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        private string _CCCD;
        public string CCCD
        {
            get { return _CCCD; }
            set { _CCCD = value; OnPropertyChanged(); }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; OnPropertyChanged(); }
        }

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set { _Address = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _Gender;
        public ComboBoxItem Gender
        {
            get { return _Gender; }
            set { _Gender = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _CustomerType;
        public ComboBoxItem CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; OnPropertyChanged(); }
        }

        private DateTime _StartDate;
        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; OnPropertyChanged(); }
        }
        private RentalContractDTO  _RentalContract;
        public RentalContractDTO RentalContract
        {
            get { return _RentalContract; }
            set { _RentalContract = value; OnPropertyChanged(); }
        }
        private List<RoomCustomerDTO> _ListCustomer;
        public List<RoomCustomerDTO> ListCustomer
        {
            get { return _ListCustomer; }
            set { _ListCustomer = value; OnPropertyChanged(); }
        }


        private DateTime _DayOfBirth;
        public DateTime DayOfBirth
        {
            get { return _DayOfBirth; }
            set { _DayOfBirth = value; OnPropertyChanged(); }
        }

        private DateTime _CheckoutDate;
        public DateTime CheckoutDate
        {
            get { return _CheckoutDate; }
            set { _CheckoutDate = value; OnPropertyChanged(); }
        }
        private string _RentalContractId;
        public string RentalContractId
        {
            get { return _RentalContractId; }
            set { _RentalContractId = value; OnPropertyChanged(); }
        }
        

        private DateTime _StartTime;
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _PersonNumber;
        public ComboBoxItem PersonNumber
        {
            get { return _PersonNumber; }
            set { _PersonNumber = value; OnPropertyChanged(); }
        }

        private ObservableCollection<RoomDTO> _ListReadyRoom;
        public ObservableCollection<RoomDTO> ListReadyRoom
        {
            get => _ListReadyRoom;
            set
            {
                _ListReadyRoom = value;
                OnPropertyChanged();
            }
        }
        public static ObservableCollection<RentalContractDTO> GetAllBookingRoom { get; set; }
        private ObservableCollection<RentalContractDTO> _BookingRoomList;
        public ObservableCollection<RentalContractDTO> BookingRoomList
        {
            get => _BookingRoomList;
            set
            {
                _BookingRoomList = value;
                OnPropertyChanged();
            }
        }
        
        private RoomDTO _SelectedRoom;
        public RoomDTO SelectedRoom
        {
            get => _SelectedRoom;
            set
            {
                _SelectedRoom = value;
                OnPropertyChanged();
            }
        }

        private RentalContractDTO _SelectedItem;
        public RentalContractDTO SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
            }
        }

        private ComboBoxItem _filtercbbItem;
        public ComboBoxItem FiltercbbItem
        {
            get => _filtercbbItem;
            set { _filtercbbItem = value; OnPropertyChanged(); }
        }

        public ICommand CloseCM { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand SelectedTimeChangedCM { get; set; }
        public ICommand LoadBookingCM { get; set; }
        public ICommand ConfirmBookingRoomCM { get; set; }
        public ICommand ExportExcelFileCM { get; set; }
        public ICommand LoadDeleteRentalContractRoomCM { get; set; }
        public ICommand LoadDetailRentalContractRoomCM { get; set; }
        public ICommand FilterListRentalContractCommand { get; set; }
        public ICommand CheckCCCDCM { get; set; }
        
        public BookingRoomManagementVM() 
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            FirstLoadCM = new RelayCommand<Window>((p) => { return true; },async (p) =>
            {
                StartDate = DateTime.Today;
                CheckoutDate = DateTime.Today.AddDays(1);
                StartTime = DateTime.Now;
                DayOfBirth = DateTime.Now;
                SelectedRoom = null;
                if(AdminVM.AdminVM.CurrentStaff != null)
                    currentStaff = AdminVM.AdminVM.CurrentStaff;
                else
                    currentStaff = StaffVM.StaffVM.CurrentStaff;
                StaffName = currentStaff.StaffName;
                StaffId = currentStaff.StaffId;
                await  LoadReadyRoom();

                await LoadBookingRoom();

            });
            FilterListRentalContractCommand = new RelayCommand<System.Windows.Controls.ComboBox>((p) => { return true; }, (p) =>
            {
                FilterRentalContractList();
            });
            LoadBookingCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                RenewWindowData();
                Booking booking = new Booking();
                CheckoutDate = StartDate.AddDays(1);
                isExistCustomer = false;
                booking.ShowDialog();
            });

            LoadDetailRentalContractRoomCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                DetailRent r = new DetailRent();
                RentalContract = await RentalContractService.Ins.GetRentalContractById(SelectedItem.RentalContractId);
                ListCustomer = await RoomCustomerService.Ins.GetCustomersOfRoom(SelectedItem.RentalContractId);
                r.ShowDialog();
            });
            SelectedTimeChangedCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
               
                await LoadReadyRoom();
            });
            
            ConfirmBookingRoomCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                (bool isvalid, string error) = ValidateBooking();
                if (isvalid)
                {
                    if(!isExistCustomer)
                    {
                        await SaveCustomer();
                    }
                    else
                    {
                        CustomerId = (await CustomerService.Ins.GetCustomerByCCCD(CCCD)).CustomerId;
                    }
                    await SaveRentalContract(p);
                    await LoadBookingRoom();
                }
                else
                {
                    CustomMessageBox.ShowOk(error, "Cảnh báo", "OK", CustomMessageBoxImage.Warning);
                }
            });
            LoadDeleteRentalContractRoomCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                string message = "Bạn có chắc muốn xoá phiếu thuê này không? Dữ liệu không thể phục hồi sau khi xoá!";
                CustomMessageBoxResult kq = CustomMessageBox.ShowOkCancel(message, "Cảnh báo", "Xác nhận", "Hủy", CustomMessageBoxImage.Warning);

                if (kq == CustomMessageBoxResult.OK)
                {
                    if (SelectedItem.Validated == true)
                    {
                        if ((await BookingRoomService.Ins.GetRoomStatusBy(SelectedItem.RentalContractId)) == ROOM_STATUS.BOOKED)
                        {
                            CustomMessageBoxResult kqq = CustomMessageBox.ShowOkCancel("Khách chưa nhận phòng. Bạn có muốn xóa phiếu thuê không!", "Thông báo", "Có", "Không", CustomMessageBoxImage.Warning);
                            if (kqq == CustomMessageBoxResult.OK)
                            {
                                (bool successDeleteRentalContractBooked, string messageFromDelRentalContractBooked) = await BookingRoomService.Ins.DeleteRentalContractBooked(SelectedItem.RentalContractId);
                                if (successDeleteRentalContractBooked)
                                {
                                    LoadBookingRoomListView(Operation.DELETE);
                                    SelectedItem = null;
                                    CustomMessageBox.ShowOk(messageFromDelRentalContractBooked, "Thông báo", "OK", CustomMessageBoxImage.Success);
                                }
                                else
                                {
                                    CustomMessageBox.ShowOk(messageFromDelRentalContractBooked, "Lỗi", "OK", CustomMessageBoxImage.Error);
                                }
                            }

                        }
                        else
                        {
                            CustomMessageBox.ShowOk("Khách đã nhận phòng. Bạn không thể xóa phiếu thuê này!", "Thông báo", "OK", CustomMessageBoxImage.Warning);
                        }
                        return;
                    }
                    (bool successDeleteRentalContract, string messageFromDelRentalContract) = await BookingRoomService.Ins.DeleteRentalContractOutDate(SelectedItem.RentalContractId);
                    if (successDeleteRentalContract)
                    {
                        LoadBookingRoomListView(Operation.DELETE);
                        SelectedItem = null;
                        CustomMessageBox.ShowOk(messageFromDelRentalContract, "Thông báo", "OK", CustomMessageBoxImage.Success);
                    }
                    else
                    {
                        CustomMessageBox.ShowOk(messageFromDelRentalContract, "Lỗi", "OK", CustomMessageBoxImage.Error);
                    }
                }
            });

            CheckCCCDCM = new RelayCommand<Booking>((p) => { return true; }, async (p) =>
            {
                await CheckCCCD(CCCD,p);
            });
            ExportExcelFileCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                ExportToFileFunc();
            });

            CloseCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
        public async Task LoadBookingRoom()
        {
            BookingRoomList = new ObservableCollection<RentalContractDTO>();
            GetAllBookingRoom = new ObservableCollection<RentalContractDTO>();
            await GetData();
        }
        public async Task GetData()
        {
            GetAllBookingRoom = new ObservableCollection<RentalContractDTO>(await BookingRoomService.Ins.GetBookingList());
            BookingRoomList = new ObservableCollection<RentalContractDTO>();

            foreach (RentalContractDTO rentalContractDTO in GetAllBookingRoom)
            {
                RentalContractDTO newRT = new RentalContractDTO
                {
                    RentalContractId = rentalContractDTO.RentalContractId,
                    StaffName = rentalContractDTO.StaffName,
                    CustomerName = rentalContractDTO.CustomerName,
                    StartDate = rentalContractDTO.StartDate,
                    CheckOutDate = rentalContractDTO.CheckOutDate,
                    StartTime = rentalContractDTO.StartTime,
                    Validated = rentalContractDTO.Validated,
                };
                BookingRoomList.Add(newRT);
            }
        }
        public void FilterRentalContractList()
        {
            BookingRoomList = new ObservableCollection<RentalContractDTO>();

            if (FiltercbbItem.Tag.ToString() == "Tất cả")
            {
                foreach (RentalContractDTO rentalContractDTO in GetAllBookingRoom)
                {
                    RentalContractDTO newRT = new RentalContractDTO
                    {
                        RentalContractId = rentalContractDTO.RentalContractId,
                        StaffName = rentalContractDTO.StaffName,
                        CustomerName = rentalContractDTO.CustomerName,
                        StartDate = rentalContractDTO.StartDate,
                        CheckOutDate = rentalContractDTO.CheckOutDate,
                        StartTime = rentalContractDTO.StartTime,
                    };

                    BookingRoomList.Add(newRT);
                }
            }
            if (FiltercbbItem.Tag.ToString() == "Còn hiệu lực")
            {
                foreach (RentalContractDTO rentalContractDTO in GetAllBookingRoom)
                {
                    if (rentalContractDTO.Validated == true)
                    {
                        RentalContractDTO newRT = new RentalContractDTO
                        {
                            RentalContractId = rentalContractDTO.RentalContractId,
                            StaffName = rentalContractDTO.StaffName,
                            CustomerName = rentalContractDTO.CustomerName,
                            StartDate = rentalContractDTO.StartDate,
                            CheckOutDate = rentalContractDTO.CheckOutDate,
                            StartTime = rentalContractDTO.StartTime,
                        };
                        BookingRoomList.Add(newRT);
                    }
                }
            }

            if (FiltercbbItem.Tag.ToString() == "Hết hiệu lực")
            {
                foreach (RentalContractDTO rentalContractDTO in GetAllBookingRoom)
                {
                    if (rentalContractDTO.Validated == false)
                    {
                        RentalContractDTO newRT = new RentalContractDTO
                        {
                            RentalContractId = rentalContractDTO.RentalContractId,
                            StaffName = rentalContractDTO.StaffName,
                            CustomerName = rentalContractDTO.CustomerName,
                            StartDate = rentalContractDTO.StartDate,
                            CheckOutDate = rentalContractDTO.CheckOutDate,
                            StartTime = rentalContractDTO.StartTime,
                        };
                        BookingRoomList.Add(newRT);
                    }
                }
            }

        }
        public void ExportToFileFunc()
        {
            SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true };
            if (sfd.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                app.Visible = false;
                Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(1);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];


                ws.Cells[1, 1] = "Mã phiêu thuê";
                ws.Cells[1, 2] ="Tên khách hàng";
                ws.Cells[1, 3] = "Ngày bắt đầu thuê";
                ws.Cells[1, 4] = "Ngày kết thúc thuê";
                ws.Cells[1, 5] = "Tên nhân viên";

                int i2 = 2;
                foreach (var item in BookingRoomList)
                {

                    ws.Cells[i2, 1] = item.RentalContractId;
                    ws.Cells[i2, 2] = item.CustomerName;
                    ws.Cells[i2, 3] = item.StartDateStr;
                    ws.Cells[i2, 4] = item.CheckOutDateStr;
                    ws.Cells[i2, 5] = item.StaffName;
                    i2++;
                }
                ws.SaveAs(sfd.FileName);
                wb.Close();
                app.Quit();

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

                CustomMessageBox.ShowOk("Xuất file thành công","Thông báo", "OK", CustomMessageBoxImage.Success);

            }
        }
        public void LoadBookingRoomListView(Operation oper = Operation.READ, RentalContractDTO r = null)
        {

            switch (oper)
            {
                case Operation.CREATE:
                    BookingRoomList.Add(r);
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < BookingRoomList.Count; i++)
                    {
                        if (BookingRoomList[i].RentalContractId == SelectedItem?.RentalContractId)
                        {
                            BookingRoomList.Remove(BookingRoomList[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void RenewWindowData()
        {
            CustomerName = null;
            CCCD = null;
            PhoneNumber = null;
            Email = null;
            Address = null;
            StartDate = DateTime.Today;
            CheckoutDate = DateTime.Today.AddDays(1);
            StartTime = DateTime.Now;
            DayOfBirth = DateTime.Now;
            PersonNumber = null;
            CustomerType = null;
            Gender = null;
        }
    }
}
