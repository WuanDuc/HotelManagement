using HotelManagement.View.CustomMessageBoxWindow;
using System;
using System.Threading.Tasks;
using HotelManagement.Utilities;
using Window = System.Windows.Window;
using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using System.Linq;

namespace HotelManagement.ViewModel.AdminVM.CustomerManagementVM
{
    public partial class CustomerManagementVM : BaseVM
    {
        public async Task AddNewCustomer(Window p)
        {
            if(Email != null)
            {
                if (Email.Trim() == "") Email = null;
                else
                {
                    if (!Utilities.RegexUtilities.IsValidEmail(Email))
                    {
                        CustomMessageBox.ShowOk("Email không hợp lệ", "Cảnh báo", "OK", CustomMessageBoxImage.Warning);
                        return;
                    }
                }
            }
            foreach (var i in Cccd)
            {
                if (!"0123456789".Contains(i))
                {
                    CustomMessageBox.ShowOk("Sai định dạng CCCD!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                    return;
                }
            }
            if (Cccd.Length != 12)
            {
                CustomMessageBox.ShowOk("Sai định dạng CCCD!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                return;
            }
            (bool isvalid, string error) = IsValidData();
            if (isvalid)
            {
                CustomerDTO customerDTO = new CustomerDTO();
                customerDTO.CustomerName = FullName;
                customerDTO.PhoneNumber = Phonenumber;
                customerDTO.DateOfBirth = (DateTime)Birthday;
                customerDTO.Email = Email;
                customerDTO.CCCD = Cccd;
                customerDTO.CustomerType = CustomerType.Tag.ToString();
                customerDTO.Gender = Gender.Tag.ToString();
                customerDTO.CustomerAddress = Address;
                customerDTO.IsDeleted = false;

                (bool isSucessed, string mess, CustomerDTO newcus) = await CustomerService.Ins.AddCustomer(customerDTO);
                if (isSucessed)
                {
                    LoadCustomerList(Operation.CREATE, newcus);
                    p.Close();
                    CustomMessageBox.ShowOk(mess, "Thông báo", "OK", CustomMessageBoxImage.Success);
                }
                else
                {
                    CustomMessageBox.ShowOk(mess, "Lỗi", "OK", CustomMessageBoxImage.Error);
                }

            }
            else
            {
                CustomMessageBox.ShowOk(error, "Cảnh báo", "OK", CustomMessageBoxImage.Warning);
            }

        }

        private (bool isvalid, string error)  IsValidData()
        {
            if(String.IsNullOrEmpty(FullName)|| String.IsNullOrEmpty(Phonenumber)  || String.IsNullOrEmpty(Address)||String.IsNullOrEmpty(Cccd)|| Gender is null || CustomerType is null|| Birthday is null)
            {
                return (false, "Vui lòng nhập đủ thông tin khách hàng!");
            }
            (bool isv, string err)= IsValidAge((DateTime)Birthday);
            if (!isv) return (false, err);
            if (!Helper.IsPhoneNumber(Phonenumber)) return (false, "Số điện thoại không hợp lệ!");
            return (true, null);
        }
        private (bool isvalid, string err)  IsValidAge( DateTime birthday)
        {
            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - birthday.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (birthday.DayOfYear > today.DayOfYear) age--;

            if (age < 16) return (false,  "Khách hàng chưa đủ 16 tuổi!" );
            return (true, null);
        }
    }
}
