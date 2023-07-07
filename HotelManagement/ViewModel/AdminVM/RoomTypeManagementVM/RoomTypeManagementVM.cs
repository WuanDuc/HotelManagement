using HotelManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using HotelManagement.Model.Services;
using HotelManagement.View.CustomMessageBoxWindow;
using Microsoft.Office.Interop.Excel;
using HotelManagement.View.Admin.RoomTypeManagement;
using System.Windows;
using HotelManagement.Utils;

namespace HotelManagement.ViewModel.AdminVM.RoomTypeManagementVM
{
    public partial class RoomTypeManagementVM : BaseVM
    {
        private string _roomTypeID;
        public string RoomTypeID
        {
            get { return _roomTypeID; }
            set { _roomTypeID = value; OnPropertyChanged(); }
        }

        private string _roomTypeName;
        public string RoomTypeName
        {
            get { return _roomTypeName; }
            set { _roomTypeName = value; OnPropertyChanged(); }
        }

        private double _roomTypePrice;
        public double RoomTypePrice
        {
            get { return _roomTypePrice; }
            set { _roomTypePrice = value; OnPropertyChanged(); }
        }

        private string _roomTypeNote;
        public string RoomTypeNote
        {
            get { return _roomTypeNote; }
            set { _roomTypeNote = value; OnPropertyChanged(); }
        }

        private RoomTypeDTO _selectedItem;
        public RoomTypeDTO SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        private bool isloadding;
        public bool IsLoadding
        {
            get { return isloadding; }
            set { isloadding = value; OnPropertyChanged(); }
        }

        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }

        private ObservableCollection<RoomTypeDTO> _roomTypeList;
        public ObservableCollection<RoomTypeDTO> RoomTypeList
        {
            get => _roomTypeList;
            set
            {
                _roomTypeList = value;
                OnPropertyChanged();
            }
        }

        public ICommand FirstLoadCM { get; set; }
        public ICommand CloseCM { get; set; }
        public ICommand LoadNoteRoomTypeCM { get; set; }
        public ICommand SaveRoomTypeCM { get; set; }
        public ICommand UpdateRoomTypeCM { get; set; }

        public RoomTypeManagementVM()
        {
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                RoomTypeList = new ObservableCollection<RoomTypeDTO>();
                try
                {
                    IsLoadding = true;
                    RoomTypeList = new ObservableCollection<RoomTypeDTO>(await Task.Run(() => RoomTypeService.Ins.GetAllRoomType()));
                    IsLoadding = false;
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
            });
            LoadEditRoomTypeCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                EditRoom w1 = new EditRoom();
                LoadEditRoomType();
                w1.ShowDialog();
            });
            LoadNoteRoomTypeCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NoteRoomType w1 = new NoteRoomType();
                RoomTypeNote = SelectedItem.RoomTypeNote;
                w1.ShowDialog();
            });
            UpdateRoomTypeCM = new RelayCommand<System.Windows.Window>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await UpdateRoomTypeFunc(p);
                IsSaving = false;
            });
            CloseCM = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) =>
            {
                SelectedItem = null;
                p.Close();
            });
        }

        public async void ReloadListView()
        {
            RoomTypeList = new ObservableCollection<RoomTypeDTO>();
            try
            {
                IsLoadding = true;
                RoomTypeList = new ObservableCollection<RoomTypeDTO>(await Task.Run(() => RoomTypeService.Ins.GetAllRoomType()));
                IsLoadding = false;
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
        public void LoadRoomTypeListView(Operation oper = Operation.READ, RoomTypeDTO rt = null)
        {
            switch (oper)
            {
                case Operation.CREATE:
                    RoomTypeList.Add(rt);
                    break;
                case Operation.UPDATE:
                    var movieFound = RoomTypeList.FirstOrDefault(x => x.RoomTypeId == rt.RoomTypeId);
                    RoomTypeList[RoomTypeList.IndexOf(movieFound)] = rt;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < RoomTypeList.Count; i++)
                    {
                        if (RoomTypeList[i].RoomTypeId == SelectedItem?.RoomTypeId)
                        {
                            RoomTypeList.Remove(RoomTypeList[i]);
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
            RoomTypeName = null;
            RoomTypeNote = null;
            RoomTypePrice = 0;
        }
        public bool IsValidData()
        {
            return !string.IsNullOrEmpty(RoomTypeName) && !string.IsNullOrEmpty(RoomTypeNote);
        }

    }
}
