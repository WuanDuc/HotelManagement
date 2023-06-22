using HotelManagement.View.CustomMessageBoxWindow;
using System;
using System.Threading.Tasks;
using HotelManagement.Utilities;
using Window = System.Windows.Window;
using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utils;

namespace HotelManagement.ViewModel.AdminVM.CustomerManagementVM
{
    public partial class CustomerManagementVM : BaseVM
    {
        public async Task EditCustomer(Window p)
        {
            if (Email != null)
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
            (bool isvalid, string error) = IsValidData();
            if (isvalid)
            {
                CustomerDTO customerDTO = new CustomerDTO();
                customerDTO.CustomerId = CustomerId;
                customerDTO.CustomerName = FullName;
                customerDTO.PhoneNumber = Phonenumber;
                customerDTO.DateOfBirth = (DateTime)Birthday;
                customerDTO.Email = Email;
                customerDTO.CCCD = Cccd;
                customerDTO.CustomerType = CustomerType.Tag.ToString();
                customerDTO.Gender = Gender.Tag.ToString();
                customerDTO.CustomerAddress = Address;
                customerDTO.IsDeleted = false;
                (bool isval, string mess) = await CustomerService.Ins.UpdateCustomerInfo(customerDTO);
                if (isval)
                {
                    LoadCustomerList(Operation.UPDATE, customerDTO);
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
    
    }
}
