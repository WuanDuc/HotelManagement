using CinemaManagementProject.Utilities;
using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utilities;
using HotelManagement.Utils;
using HotelManagement.View.Staff.TroubleReport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ComboBox = System.Windows.Controls.ComboBox;

namespace HotelManagement.ViewModel.StaffVM.TroubleReportVM
{
    public partial class TroubleReportVM : BaseVM
    {
        private StaffDTO currentStaff;
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
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        private string _desription;
        public string Desription
        {
            get { return _desription; }
            set { _desription = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _reason;
        public ComboBoxItem Reason
        {
            get { return _reason; }
            set { _reason = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _level;
        public ComboBoxItem Level
        {
            get { return _level; }
            set { _level = value; OnPropertyChanged(); }
        }
        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged(); }
        }
        private string _rentalContractId;
        public string RentalContractId
        {
            get { return _rentalContractId; }
            set { _rentalContractId = value; OnPropertyChanged(); }
        }
        private List<string> _ListRentalContractId;
        public List<string> ListRentalContractId
        {
            get { return _ListRentalContractId; }
            set { _ListRentalContractId = value; OnPropertyChanged(); }
        }
        private ImageSource _imgTrouble;
        public ImageSource ImageTrouble
        {
            get { return _imgTrouble; }
            set { _imgTrouble = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _selectedfilteritem;
        public ComboBoxItem SelectedFilteritem
        {
            get { return _selectedfilteritem; }
            set { _selectedfilteritem = value; OnPropertyChanged(); }
        }
        private bool IsImageChanged = false;
        private string filepath;
        public ICommand FirstLoadCM { get; set; }
        public ICommand OpenReportWindow { get; set; }
        public ICommand UploadImgCM { get; set; }
        public ICommand AddTroubleCM { get; set; }
        public ICommand OpenEditTroubleReportCM { get; set; }
        public ICommand UpdateReportTbCM { get; set; }
        public ICommand FilterListTroubleCM { get; set; }
        public ICommand LoadDetailTroubleWindowCM { get; set; }
        public TroubleReportVM()
        {
            if (AdminVM.AdminVM.CurrentStaff != null) { currentStaff = AdminVM.AdminVM.CurrentStaff; }
            if (StaffVM.CurrentStaff != null) { currentStaff = StaffVM.CurrentStaff; }
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                TroubleList = new ObservableCollection<TroubleDTO>(await TroubleService.Ins.GetAllTrouble());

            });
            OpenReportWindow = new RelayCommand<object>(p => true, async p =>
            {
                AddTroubleWindow wd = new AddTroubleWindow();
                wd.staffname.Text = currentStaff.StaffName;
                StartDate = DateTime.Now;
                wd.startdate.Text = StartDate.ToString();
                wd.status.Text = STATUS.WAITING;
                ListRentalContractId = new List<string>(await TroubleService.Ins.GetCurrentListRentalContractId());
                ResetData();
                wd.ShowDialog();
            });
            UploadImgCM = new RelayCommand<Window>(p => true, async p =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png";
                if (openfile.ShowDialog() == DialogResult.OK)
                {
                    IsImageChanged = true;
                    filepath = openfile.FileName;
                    LoadImage();
                    return;
                }
                IsImageChanged = false;
            });
            AddTroubleCM = new RelayCommand<Window>(p => { if (IsSaving) return false; return true; }, async p =>
            {
                IsSaving = true;
                await AddTrouble(p);
                ReloadAsync();
                IsSaving = false;
            });
            OpenEditTroubleReportCM = new RelayCommand<object>(p => true, async p =>
            {
                EditTroubleReportWindow wd = new EditTroubleReportWindow();
                ResetData();
                TroubleId = SelectedItem.TroubleId;
                ImageTrouble = Helper.LoadBitmapImage(SelectedItem.Avatar);
                wd.staffname.Text = await TroubleService.Ins.GetStaffNameById(SelectedItem.StaffId);
                wd.startdate.Text = SelectedItem.StartDate.ToString();
                Title = SelectedItem.Title;
                wd.level.Text = SelectedItem.Level;
                Desription = SelectedItem.Description;
                if (SelectedItem.Reason == REASON.BYCUSTOMER)
                {
                    wd.txtboxMPT.Visibility = Visibility.Visible;
                    RentalContractId = (await TroubleService.Ins.GetTroubleByCus(SelectedItem.TroubleId)).RentalContractId;
                }
                else
                {
                    wd.txtboxMPT.Visibility = Visibility.Collapsed;
                }
                wd.reason.Text = SelectedItem.Reason;
                wd.ShowDialog();
            });
            UpdateReportTbCM = new RelayCommand<Window>(p => { if (IsSaving) return false; return true; }, async p =>
            {
                IsSaving = true;
                await UpdateRPTrouble(p);
                ReloadAsync();
                IsSaving = false;
            });
            FilterListTroubleCM = new RelayCommand<ComboBox>(p => true, async p =>
            {
                ReloadAsync();
            });
            LoadDetailTroubleWindowCM = new RelayCommand<System.Windows.Controls.ListView>((p) => { return true; }, async (p) =>
            {
                if (SelectedItem is null) return;

                if (SelectedItem.Status == STATUS.IN_PROGRESS || SelectedItem.Status == STATUS.DONE)
                {
                    ViewInfomationWindow w = new ViewInfomationWindow();
                    w.staffname.Text = await TroubleService.Ins.GetStaffNameById(SelectedItem.StaffId);
                    if (SelectedItem.Reason == REASON.BYCUSTOMER)
                    {
                        w.gridmsp.Visibility = Visibility.Visible;
                        w.giadudoangrid.Visibility = Visibility.Visible;
                        w.rentalcontractid.Text = (await TroubleService.Ins.GetTroubleByCus(SelectedItem.TroubleId)).RentalContractId;
                        w.PredictPrice.Text = (await TroubleService.Ins.GetTroubleByCus(SelectedItem.TroubleId)).PredictedPrice.ToString();
                    }
                    w.ShowDialog();
                    return;
                }
                if (SelectedItem.Status == STATUS.PREDIT)
                {
                    ViewInfomationWindow w = new ViewInfomationWindow();
                    w.staffname.Text = await TroubleService.Ins.GetStaffNameById(SelectedItem.StaffId);
                    if (SelectedItem.Reason == REASON.BYCUSTOMER)
                    {
                        w.gridmsp.Visibility = Visibility.Visible;
                        w.giadudoangrid.Visibility = Visibility.Visible;
                    }
                    w.gridprice.Visibility = Visibility.Collapsed;
                    w.gridfinishdate.Visibility = Visibility.Collapsed;
                    w.gridfixdate.Visibility = Visibility.Collapsed;
                    w.ShowDialog();
                    return;
                }
                if (SelectedItem.Status == STATUS.CANCLE)
                {
                    return;
                }
            });
        }

        private async void ReloadAsync()
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
        }

        private void ResetData()
        {
            RentalContractId = null;
            Title = null;
            Desription = null;
            Level = null;
            Status = null;
            ImageTrouble = null;
        }
        private void LoadImage()
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(filepath, UriKind.RelativeOrAbsolute);
            _image.EndInit();
            ImageTrouble = _image;
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

                default:
                    break;
            }
        }

    }
}
