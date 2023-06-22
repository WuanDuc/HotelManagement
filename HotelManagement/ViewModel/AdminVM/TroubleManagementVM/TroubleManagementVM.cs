using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utilities;
using HotelManagement.Utils;
using HotelManagement.View.Admin.TroubleManagement;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.View.Staff.TroubleReport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using ComboBox = System.Windows.Controls.ComboBox;

namespace HotelManagement.ViewModel.AdminVM.TroubleManagementVM
{
    public class TroubleManagementVM:BaseVM
    {
        private ObservableCollection<TroubleDTO> _troubleList;
        public ObservableCollection<TroubleDTO> TroubleList
        {
            get { return _troubleList; }
            set { _troubleList = value; OnPropertyChanged(); }
        }
        private TroubleDTO _selectedItem;
        public TroubleDTO SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        private bool _isSaving;
        public bool IsSaving
        {
            get { return _isSaving; }
            set { _isSaving = value; }
        }
        private string _troubleId;
        public string TroubleId
        {
            get { return _troubleId; }
            set { _troubleId = value; OnPropertyChanged(); }
        }
               
        private ComboBoxItem _status;
        public ComboBoxItem Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }
        private DateTime _fixedDate;
        public DateTime FixedDate
        {
            get { return _fixedDate; }
            set { _fixedDate = value; OnPropertyChanged(); }
        }
        private DateTime _finishDate;
        public DateTime FinishDate
        {
            get { return _finishDate; }
            set { _finishDate = value; OnPropertyChanged(); }
        }
        private double _price;
        public double Price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged(); }
        }
        private double _predictPrice;
        public double PredictPrice
        {
            get { return _predictPrice; }
            set { _predictPrice = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _selectedfilteritem;
        public ComboBoxItem SelectedFilteritem
        {
            get { return _selectedfilteritem; }
            set { _selectedfilteritem = value; OnPropertyChanged(); }
        }
        public ICommand FirstLoadCM { get; set; }
        public ICommand FilterListTroubleCM { get; set; }
        public ICommand OpenSolveTroubleWindowCM { get; set; }
        public ICommand UpdateTroubleCM { get; set; }
        public TroubleManagementVM()
        {
            FirstLoadCM = new RelayCommand<System.Windows.Controls.Page>((p) => { return true; }, async (p) =>
            {
                TroubleList = new ObservableCollection<TroubleDTO>(await TroubleService.Ins.GetAllTrouble());
            });
            FilterListTroubleCM = new RelayCommand<ComboBox>(p => true, async p =>
            {
                ObservableCollection<TroubleDTO> GetAllTrouble = new ObservableCollection<TroubleDTO>(await TroubleService.Ins.GetAllTrouble());

                if (SelectedFilteritem.Tag.ToString() == "Toàn bộ")
                {
                    TroubleList = new ObservableCollection<TroubleDTO>(GetAllTrouble);
                }
                else
                {
                    TroubleList = new ObservableCollection<TroubleDTO>(GetAllTrouble.Where(tr => tr.Status == SelectedFilteritem.Tag.ToString()));
                }
            });
            OpenSolveTroubleWindowCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                ChoseWindow();
            });
            UpdateTroubleCM = new RelayCommand<Window>(p => !IsSaving, async p =>
            {
                if (Status is null)
                {
                    CustomMessageBox.ShowOk("Không hợp lệ!", "Cảnh báo", "OK", CustomMessageBoxImage.Warning);                    
                    return;
                }
                IsSaving = true;
                await UpdateTrouble(p);
                IsSaving = false;
            });

        }
        public async Task UpdateTrouble(Window p)
        {
            if (Status.Tag.ToString() == STATUS.PREDIT)
            {
                TroubleDTO tb = new TroubleDTO
                {
                    TroubleId = TroubleId,
                    Status = Status.Tag.ToString()                    
                };
                (bool isSucess, string mess) = await TroubleService.Ins.UpdateTrouble(tb, PredictPrice);
                if (isSucess)
                {

                    CustomMessageBox.ShowOk(mess,  "Thông báo", "OK", CustomMessageBoxImage.Success);
                     ReloadAsync();
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(mess, "Lỗi"  , "OK", CustomMessageBoxImage.Error);
                }
            }
           else if (Status.Tag.ToString() == STATUS.IN_PROGRESS)
            {
                if (SelectedItem.StartDate > FixedDate)
                {
                    CustomMessageBox.ShowOk("Ngày không hợp lệ, vui lòng chọn lại!", "Lỗi", "OK", CustomMessageBoxImage.Error);
                    
                    return;
                }
                TroubleDTO tb = new TroubleDTO
                {
                    TroubleId = TroubleId,
                    FixedDate = FixedDate,
                    Status = Status.Tag.ToString()
                };
                (bool isSucess, string mess) = await TroubleService.Ins.UpdateTrouble(tb, PredictPrice);
                if (isSucess)
                {

                    CustomMessageBox.ShowOk(mess, "Thông báo", "OK", CustomMessageBoxImage.Success);
                    ReloadAsync();
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(mess, "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            }
           else if (Status.Tag.ToString() == STATUS.DONE)
            {
                if (SelectedItem.StartDate > FixedDate && FixedDate>FinishDate)
                {
                    CustomMessageBox.ShowOk("Ngày không hợp lệ, vui lòng chọn lại!", "Lỗi", "OK", CustomMessageBoxImage.Error);

                    return;
                }
                TroubleDTO tb = new TroubleDTO
                {
                    TroubleId = TroubleId,
                    FixedDate=FixedDate,
                    FinishDate=FinishDate,
                    Price= Price,
                    Status = Status.Tag.ToString()
                };
                (bool isSucess, string mess) = await TroubleService.Ins.UpdateTrouble(tb, PredictPrice);
                if (isSucess)
                {

                    CustomMessageBox.ShowOk(mess, "Thông báo", "OK", CustomMessageBoxImage.Success);
                    ReloadAsync();
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(mess, "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            }
          else  if (Status.Tag.ToString() == STATUS.CANCLE)
            {
                TroubleDTO tb = new TroubleDTO
                {
                    TroubleId = TroubleId,
                    Status = Status.Tag.ToString()
                };
                (bool isSucess, string mess) = await TroubleService.Ins.UpdateTrouble(tb);
                if (isSucess)
                {

                    CustomMessageBox.ShowOk(mess, "Thông báo", "OK", CustomMessageBoxImage.Success);
                    ReloadAsync();
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(mess, "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            }
        }
        
        private async void ChoseWindow()
        {
            Status = null;
            
            FixedDate = DateTime.Now;
            FinishDate = DateTime.Now;
            Price = 0;
            TroubleId = SelectedItem.TroubleId;
            if (SelectedItem.Status == STATUS.WAITING||SelectedItem.Status==STATUS.PREDIT)
            {
                PredictPrice = 0;
                EditTroubleWindow wd = new EditTroubleWindow();
                if (SelectedItem.Reason == REASON.BYCUSTOMER)
                {
                    wd.cbbStatusByCustomer.Visibility = Visibility.Visible;
                    wd.cbbStatus.Visibility = Visibility.Collapsed;
                    wd.gridbyHotel.Visibility = Visibility.Collapsed;
                    wd.gridByCus.Visibility = Visibility.Visible;
                    wd.predictgrid.Visibility = Visibility.Visible;
                    var pricebycus = (await TroubleService.Ins.GetTroubleByCus(SelectedItem.TroubleId)).PredictedPrice;
                    if (pricebycus != null) PredictPrice = (double)pricebycus;
                    wd.MSPgrid.Visibility=System.Windows.Visibility.Visible;
                    wd.rentalcontractid.Text = (await TroubleService.Ins.GetTroubleByCus(SelectedItem.TroubleId)).RentalContractId;
                }
                wd.staffname.Text = await TroubleService.Ins.GetStaffNameById(SelectedItem.StaffId);
                 wd.cbbStatus.Text = SelectedItem.Status;
                
                wd.ShowDialog();
            }
            else if (SelectedItem.Status == STATUS.IN_PROGRESS)
            {
                EditTrouble_InprocessWindow wd = new EditTrouble_InprocessWindow();
                if (SelectedItem.Reason == REASON.BYCUSTOMER)
                {
                    wd.predictgrid.Visibility = System.Windows.Visibility.Visible;
                    wd.MSPgrid.Visibility = System.Windows.Visibility.Visible;
                    wd.rentalcontractid.Text = (await TroubleService.Ins.GetTroubleByCus(SelectedItem.TroubleId)).RentalContractId;
                    PredictPrice = (double)(await TroubleService.Ins.GetTroubleByCus(SelectedItem.TroubleId)).PredictedPrice;
                }
                wd.staffname.Text = await TroubleService.Ins.GetStaffNameById(SelectedItem.StaffId);

                wd.ShowDialog();
            }
            else if(SelectedItem.Status == STATUS.DONE)
            {
                ViewInfomationWindow wd = new ViewInfomationWindow();
                if (SelectedItem.Reason == REASON.BYCUSTOMER)
                {
                    wd.giadudoangrid.Visibility = System.Windows.Visibility.Visible;
                    wd.gridmsp.Visibility = System.Windows.Visibility.Visible;
                    wd.rentalcontractid.Text = (await TroubleService.Ins.GetTroubleByCus(SelectedItem.TroubleId)).RentalContractId;
                    PredictPrice = (double)(await TroubleService.Ins.GetTroubleByCus(SelectedItem.TroubleId)).PredictedPrice;
                }
                wd.staffname.Text = await TroubleService.Ins.GetStaffNameById(SelectedItem.StaffId);

                wd.ShowDialog();
            }

        }

        private async void ReloadAsync()
        {
            TroubleList = new ObservableCollection<TroubleDTO>(await TroubleService.Ins.GetAllTrouble());
        }


        private void LoadTroubleList(Operation oper, TroubleDTO trouble = null)
        {
            switch (oper)
            {
                case Operation.CREATE:
                    TroubleList.Add(trouble);
                    break;
                case Operation.UPDATE:
                    var updtrouble = TroubleList.FirstOrDefault(s => s.StaffId == trouble.StaffId);
                    TroubleList[TroubleList.IndexOf(updtrouble)] = trouble;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < TroubleList.Count(); i++)
                    {
                        if (TroubleList[i].TroubleId == SelectedItem.TroubleId)
                        {
                            TroubleList.Remove(TroubleList[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
