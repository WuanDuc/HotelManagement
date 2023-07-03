using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Model.Services;
using HotelManagement.Utilities;
using HotelManagement.Utils;
using HotelManagement.View.BookingRoomManagement;
using HotelManagement.View.CustomMessageBoxWindow;
using IronXL.Formatting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace HotelManagement.ViewModel.BookingRoomManagementVM
{
    public partial class BookingRoomManagementVM : BaseVM
    {
        private bool isExistCustomer = false;
        public (bool isvalid, string error) ValidateBooking()
        {
            if (string.IsNullOrEmpty(CustomerName) ||
                string.IsNullOrEmpty(CCCD) ||
                string.IsNullOrEmpty(PhoneNumber) ||
                string.IsNullOrEmpty(Email) ||
                string.IsNullOrEmpty(Address) ||
                string.IsNullOrEmpty(Gender.Content.ToString()) ||
                string.IsNullOrEmpty(CustomerType.Content.ToString()))
            {
                return (false, "Vui lòng nhập đủ thông tin khách hàng!");
            }
            else
            {
                if (Email != null)
                {
                    if (Email.Trim() == "") Email = null;
                    else
                    {
                        if (!Utilities.RegexUtilities.IsValidEmail(Email))
                        {
                            return (false, "Email không hợp lệ");
                        }
                    }
                }
                if (!Helper.IsPhoneNumberTinh(PhoneNumber)) return (false, "Số điện thoại không hợp lệ!");
                (bool isv, string err) = IsValidAge((DateTime)DayOfBirth);
                if (!isv) return (false, err);
                if (StartDate >= CheckoutDate) return (false, "Vui lòng kiểm tra lại ngày bắt đầu thuê và ngày trả phòng!");
                if (SelectedRoom is null) return (false, "Vui lòng chọn phòng để đặt!");
                return (true, null);
            }
        }
        
        private (bool isvalid, string err) IsValidAge(DateTime birthday)
        {
            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - birthday.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (birthday.DayOfYear > today.DayOfYear) age--;

            if (age < 16) return (false, "Khách hàng chưa đủ 16 tuổi!");
            return (true, null);
        }

        public async Task SaveRentalContract(Window p)
        {
            RentalContractDTO temp = new RentalContractDTO
            {
                CheckOutDate = CheckoutDate,
                StartDate = StartDate,
                StartTime = new TimeSpan(0, StartTime.TimeOfDay.Hours, StartTime.TimeOfDay.Minutes, StartTime.TimeOfDay.Seconds, 0),
                StaffId = StaffId,
                CustomerId = CustomerId,
                RoomId = SelectedRoom.RoomId,
                Validated = true,
            };
            (bool isSucsses, string message) = await BookingRoomService.Ins.SaveBooking(temp);
            if (isSucsses)
            {
                CustomMessageBox.ShowOk(message, "Thong bao", "Ok", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                p.Close();
            }
            else
            {
                CustomMessageBox.ShowOk(message, "Lỗi", "OK", CustomMessageBoxImage.Error);
            }
        }
        public async Task SaveCustomer()
        {
            CustomerDTO customerDTO = new CustomerDTO
            {
                CustomerName = CustomerName,
                PhoneNumber = PhoneNumber,
                DateOfBirth = (DateTime)DayOfBirth,
                Email = Email,
                CCCD = CCCD,
                CustomerType = CustomerType.Content.ToString(),
                Gender = Gender.Content.ToString(),
                CustomerAddress = Address,
                IsDeleted = false,
            };
            (bool isSucsses, string message, string customerId) = await BookingRoomService.Ins.SaveCustomer(customerDTO);
            if (isSucsses)
            {
                CustomerId = customerId;
            }
            else
            {
                CustomMessageBox.ShowOk(message, "Lỗi", "Ok", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
            }
        }
        public async Task LoadReadyRoom()
        {
            ListReadyRoom = new ObservableCollection<RoomDTO>(await BookingRoomService.Ins.GetListReadyRoom(StartDate,StartTime,CheckoutDate));
        }
        public async Task CheckCCCD(string cccd, Booking b)
        {
            
            foreach(var i in cccd)
            {
                if( !"0123456789".Contains(i))
                {
                    CustomMessageBox.ShowOk("Sai định dạng CCCD!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                    return; 
                }
            }
            if (cccd.Length != 12)
            {
                CustomMessageBox.ShowOk("Sai định dạng CCCD!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                return;
            }

            (bool isExist, CustomerDTO cus) = await BookingRoomService.Ins.CheckCCCD(cccd);

            if (isExist)
            {
                CCCD = cus.CCCD;
                CustomerName = cus.CustomerName;
                Email = cus.Email;
                Address = cus.CustomerAddress;
                DayOfBirth = (DateTime)cus.DateOfBirth;
                if (cus.CustomerType == "Nội địa")
                    b.cbbCusType.SelectedIndex = 0;
                else
                    b.cbbCusType.SelectedIndex = 1;

                if (cus.Gender == "Nam")
                    b.cbbGender.SelectedIndex = 0;
                else
                    b.cbbGender.SelectedIndex = 1;
                PhoneNumber = cus.PhoneNumber;
                isExistCustomer = true;
            }
            else
            {
                RenewWindowData();
                CCCD = cccd;
                b.notifyNotExist.Visibility = Visibility.Visible;
            }
        }
    }
}
