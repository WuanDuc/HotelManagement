using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.Admin.RoomManagement;
using HotelManagement.View.Admin.RoomTypeManagement;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.ViewModel.AdminVM.RoomTypeManagementVM;
using MaterialDesignThemes.Wpf;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HotelManagement.ViewModel.AdminVM.RoomManagementVM
{
    public partial class RoomManagementVM : BaseVM
    {
        public Frame mainFrame { get; set; }
        public Card ButtonView { get; set; }
        private string _RoomId;
        public string RoomId
        {
            get { return _RoomId; }
            set { _RoomId = value; OnPropertyChanged(); }
        }
        private string _roomTypeID;
        public string RoomTypeID
        {
            get { return _roomTypeID; }
            set { _roomTypeID = value; OnPropertyChanged(); }
        }

        private int _roomNumber;
        public int RoomNumber
        {
            get { return _roomNumber; }
            set { _roomNumber = value; OnPropertyChanged(); }
        }

        private string _roomNote;
        public string RoomNote
        {
            get { return _roomNote; }
            set { _roomNote = value; OnPropertyChanged(); }
        }

        private string _RoomStatus;
        public string RoomStatus
        {
            get { return _RoomStatus; }
            set { _RoomStatus = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _cbRoomType;
        public ComboBoxItem CbRoomType
        {
            get { return _cbRoomType; }
            set { _cbRoomType = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _cbRoomTinhTrang;
        public ComboBoxItem CbRoomTinhTrang
        {
            get { return _cbRoomTinhTrang; }
            set { _cbRoomTinhTrang = value; OnPropertyChanged(); }
        }

        private RoomDTO _selectedRoomItem;
        public RoomDTO SelectedRoomItem
        {
            get { return _selectedRoomItem; }
            set { _selectedRoomItem = value; OnPropertyChanged(); }
        }

        private bool isloaddingRoom;
        public bool IsLoaddingRoom
        {
            get { return isloaddingRoom; }
            set { isloaddingRoom = value; OnPropertyChanged(); }
        }

        private bool isSavingRoom;
        public bool IsSavingRoom
        {
            get { return isSavingRoom; }
            set { isSavingRoom = value; OnPropertyChanged(); }
        }

        private ObservableCollection<RoomDTO> _roomList;
        public ObservableCollection<RoomDTO> RoomList
        {
            get => _roomList;
            set
            {
                _roomList = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadRoomTypeCM { get; set; }
        public ICommand LoadRoomCM { get; set; }
        public ICommand FirstLoadRoomCM { get; set; }
        public ICommand LoadViewCM { get; set; }
        public ICommand StoreButtonNameCM { get; set; }
        public ICommand CloseRoomCM { get; set; }
        public ICommand LoadDeleteRoomCM { get; set; }
        public ICommand LoadNoteRoomCM { get; set; }
        public ICommand SaveRoomCM { get; set; }
        public ICommand UpdateRoomCM { get; set; }
        public ICommand FirstLoadRoomTypeCM { get; set; }
        public ICommand CloseRoomTypeCM { get; set; }
        public ICommand LoadNoteRoomTypeCM { get; set; }
        public ICommand SaveRoomTypeCM { get; set; }
        public ICommand UpdateRoomTypeCM { get; set; }
        public ICommand LoadEditRoomTypeCM { get; set; }
        public RoomManagementVM()
        {


            LoadViewCM = new RelayCommand<Frame>((p) => { return true; },  (p) =>
            {
                mainFrame = p;
            });
            StoreButtonNameCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ButtonView = p;
                p.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
                p.SetValue(ElevationAssist.ElevationProperty, Elevation.Dp3);
            });

            LoadRoomTypeCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ChangeView(p);
                mainFrame.Content = new View.Admin.RoomTypeManagement.RoomTypeManagementPage();
            });

            LoadRoomCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ChangeView(p);
                mainFrame.Content = new RoomManagementPage();
            });

            FirstLoadRoomCM = new RelayCommand<System.Windows.Controls.Page>((p) => { return true; }, async (p) =>
            {
                RoomList = new ObservableCollection<RoomDTO>();
                try
                {
                    IsLoaddingRoom = true;
                    RoomList = new ObservableCollection<RoomDTO>(await Task.Run(() => RoomService.Ins.GetAllRoom()));
                    IsLoaddingRoom = false;
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

            LoadAddRoomCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                RenewWindowData();
                AddNewRoom addRoomType = new AddNewRoom();
                addRoomType.ShowDialog();
            });
            LoadEditRoomCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                View.Admin.RoomManagement.EditRoom w1 = new View.Admin.RoomManagement.EditRoom();
                LoadEditRoom(w1);
                w1.ShowDialog();
            });
            LoadNoteRoomCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NoteRoom w1 = new NoteRoom();
                RoomNote = SelectedRoomItem.Note;
                w1.ShowDialog();
            });
            LoadDeleteRoomCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                string message = "Bạn có chắc muốn xoá phim này không? Dữ liệu không thể phục hồi sau khi xoá!";
                CustomMessageBoxResult kq = CustomMessageBox.ShowOkCancel(message, "Cảnh báo", "Xác nhận", "Hủy", CustomMessageBoxImage.Warning);

                if (kq == CustomMessageBoxResult.OK)
                {
                    IsLoaddingRoom = true;

                    (bool successDeleteRoom, string messageFromDelRoom) = await RoomService.Ins.DeleteRoom(SelectedRoomItem.RoomId);

                    IsLoaddingRoom = false;

                    if (successDeleteRoom)
                    {
                        LoadRoomListView(Operation.DELETE);
                        SelectedRoomItem = null;
                        CustomMessageBox.ShowOk(messageFromDelRoom, "Thông báo", "OK", CustomMessageBoxImage.Success);
                    }
                    else
                    {
                        CustomMessageBox.ShowOk(messageFromDelRoom, "Lỗi", "OK", CustomMessageBoxImage.Error);
                    }
                }
            });
            UpdateRoomCM = new RelayCommand<System.Windows.Window>((p) => { if (IsSavingRoom) return false; return true; }, async (p) =>
            {
                IsSavingRoom = true;
                await UpdateRoomFunc(p);
                IsSavingRoom = false;
            });
            SaveRoomCM = new RelayCommand<System.Windows.Window>((p) => { if (IsSavingRoom) return false; return true; }, async (p) =>
            {
                IsSavingRoom = true;

                await SaveRoomFunc(p);

                IsSavingRoom = false;
            });

            CloseRoomCM = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) =>
            {
                SelectedRoomItem = null;
                p.Close();
            });


            FirstLoadRoomTypeCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                RoomTypeList = new ObservableCollection<RoomTypeDTO>();
                try
                {
                    IsLoadingRoomType = true;
                    RoomTypeList = new ObservableCollection<RoomTypeDTO>(await Task.Run(() => RoomTypeService.Ins.GetAllRoomType()));
                    IsLoadingRoomType = false;
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
                EditRoomType w1 = new EditRoomType();
                LoadEditRoomType();
                w1.ShowDialog();
            });
            LoadNoteRoomTypeCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                View.Admin.RoomManagement.NoteRoomType w1 = new View.Admin.RoomManagement.NoteRoomType();
                RoomTypeNote = SelectedRoomTypeItem.RoomTypeNote;
                w1.ShowDialog();
            });
            UpdateRoomTypeCM = new RelayCommand<System.Windows.Window>((p) => { if (IsSavingRoomType) return false; return true; }, async (p) =>
            {
                IsSavingRoomType = true;
                await UpdateRoomTypeFunc(p);
                IsSavingRoomType = false;
            });
            CloseRoomTypeCM = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) =>
            {
                SelectedRoomTypeItem = null;
                p.Close();
            });

        }


        public async void ReloadListView()
        {
            RoomList = new ObservableCollection<RoomDTO>();
            try
            {
                IsLoadingRoomType = true;
                RoomList = new ObservableCollection<RoomDTO>(await RoomService.Ins.GetAllRoom());
                IsLoadingRoomType = false;
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
        public void LoadRoomListView(Operation oper = Operation.READ, RoomDTO r = null)
        {

            switch (oper)
            {
                case Operation.CREATE:
                    RoomList.Add(r);
                    break;
                case Operation.UPDATE:
                    var roomFound = RoomList.FirstOrDefault(x => x.RoomId == r.RoomId);
                    RoomList[RoomList.IndexOf(roomFound)] = r;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < RoomList.Count; i++)
                    {
                        if (RoomList[i].RoomTypeId == SelectedRoomItem?.RoomTypeId)
                        {
                            RoomList.Remove(RoomList[i]);
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

            RoomId = null;
            RoomNumber = 0;
            RoomNote = null;
            RoomTypeID = null;
            RoomStatus = "Phòng trống";
            CbRoomTinhTrang = null;
            CbRoomType = null;
        }
        public bool IsValidData()
        {
            if (!string.IsNullOrEmpty(RoomNote) &&
                !string.IsNullOrEmpty(RoomStatus) &&
                CbRoomTinhTrang != null &&
                CbRoomType != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



       
            private string _roomType2ID;
            public string RoomType2ID
            {
                get { return _roomType2ID; }
                set { _roomType2ID = value; OnPropertyChanged(); }
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

            private RoomTypeDTO _selectedRoomTypeItem;
            public RoomTypeDTO SelectedRoomTypeItem
        {
                get { return _selectedRoomTypeItem; }
                set { _selectedRoomTypeItem = value; OnPropertyChanged(); }
            }

            private bool isLoadingRoomType;
            public bool IsLoadingRoomType
            {
                get { return isLoadingRoomType; }
                set { isLoadingRoomType = value; OnPropertyChanged(); }
            }

            private bool isSavingRoomType;
            public bool IsSavingRoomType
        {
                get { return isSavingRoomType; }
                set { isSavingRoomType = value; OnPropertyChanged(); }
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

          

          
               

            public async void ReloadListViewRoomType()
            {
                RoomTypeList = new ObservableCollection<RoomTypeDTO>();
                try
                {
                    IsLoadingRoomType = true;
                    RoomTypeList = new ObservableCollection<RoomTypeDTO>(await Task.Run(() => RoomTypeService.Ins.GetAllRoomType()));
                    IsLoadingRoomType = false;
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
                            if (RoomTypeList[i].RoomTypeId == SelectedRoomTypeItem?.RoomTypeId)
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
            public void RenewWindowDataRoomType()
            {
                RoomTypeName = null;
                RoomTypeNote = null;
                RoomTypePrice = 0;
            }
            public bool IsValidDataRoomType()
            {
                return !string.IsNullOrEmpty(RoomTypeName) && !string.IsNullOrEmpty(RoomTypeNote);
            }


        public void LoadEditRoomType()
        {
            RoomTypeID = SelectedRoomTypeItem.RoomTypeId;
            RoomTypeName = SelectedRoomTypeItem.RoomTypeName;
            RoomTypePrice = SelectedRoomTypeItem.RoomTypePrice;
            RoomTypeNote = SelectedRoomTypeItem.RoomTypeNote;
        }
        public async Task UpdateRoomTypeFunc(System.Windows.Window p)
        {

            if (RoomTypeID != null && IsValidData())
            {
                RoomTypeDTO roomType = new RoomTypeDTO
                {
                    RoomTypeId = RoomTypeID,
                    RoomTypeName = RoomTypeName,
                    RoomTypePrice = RoomTypePrice,
                    RoomTypeNote = RoomTypeNote,
                };

                (bool successUpdateRoomType, string messageFromUpdateRoomType) = await RoomTypeService.Ins.UpdateRoomType(roomType);

                if (successUpdateRoomType)
                {
                    isSavingRoomType = false;
                    CustomMessageBox.ShowOk(messageFromUpdateRoomType, "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    LoadRoomTypeListView(Operation.UPDATE, roomType);
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(messageFromUpdateRoomType, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
            }
        }


      
      
        public void ChangeView(Card p)
        {
            ButtonView.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            ButtonView.SetValue(ElevationAssist.ElevationProperty, Elevation.Dp3);
            ButtonView = p;
            p.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            p.SetValue(ElevationAssist.ElevationProperty, Elevation.Dp3);
        }

    }
}
