using HotelManagement.Components.Search;
using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.View.Admin;
using HotelManagement.View.Admin.RoomFurnitureManagement;
using HotelManagement.View.CustomMessageBoxWindow;
using IronXL.Formatting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace HotelManagement.ViewModel.AdminVM.RoomFurnitureManagementVM
{
    public partial class RoomFurnitureManagementVM : BaseVM
    {
        private bool isLoading { get; set; }
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; OnPropertyChanged(); }
        }

        private ObservableCollection<FurnituresRoomDTO> furnituresRoomList;
        public ObservableCollection<FurnituresRoomDTO> FurnituresRoomList
        {
            get { return furnituresRoomList; }
            set { furnituresRoomList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<FurnituresRoomDTO> allFurnituresRoom;
        public ObservableCollection<FurnituresRoomDTO> AllFurnituresRoom
        {
            get { return allFurnituresRoom; }
            set { allFurnituresRoom = value; OnPropertyChanged(); }
        }

        private FurnituresRoomDTO selectedFurnituresRoom { get; set; }
        public FurnituresRoomDTO SelectedFurnituresRoom
        {
            get { return selectedFurnituresRoom; }
            set { selectedFurnituresRoom = value; OnPropertyChanged(); }
        }

        private FurnituresRoomDTO furnituresRoomCache { get; set; }
        public FurnituresRoomDTO FurnituresRoomCache
        {
            get { return furnituresRoomCache; }
            set { furnituresRoomCache = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ComboBoxItem> listRoomType { get; set; }
        public ObservableCollection<ComboBoxItem> ListRoomType
        {
            get { return listRoomType; }
            set { listRoomType = value; OnPropertyChanged(); }
        }

        private ComboBoxItem selectedFilterTypeRoom { get; set; }
        public ComboBoxItem SelectedFilterTypeRoom
        {
            get { return selectedFilterTypeRoom; }
            set { selectedFilterTypeRoom = value; OnPropertyChanged(); }
        }
        private ComboBoxItem selectedFilterStatusRoom { get; set; }
        public ComboBoxItem SelectedFilterStatusRoom
        {
            get { return selectedFilterStatusRoom; }
            set { selectedFilterStatusRoom = value; OnPropertyChanged(); }
        }
        string type;
        string status;
        public ICommand FirstLoadCM { get; set; }
        public ICommand MainPageSelectionFilterChangeCM { get; set; }
        public ICommand OpenFurnituresInRoomWindow { get; set; }
        public ICommand CloseFurnitureRoomInfoCM { get; set; }
        public ICommand IncreaseQuantityOrderItem { get; set; }
        public ICommand DecreaseQuantityOrderItem { get; set; }
        public ICommand DeleteItemInBillStackCM { get; set; }
        public ICommand DeleteItemInRoomFurnitureInfoCM { get; set; }
        public ICommand OpenDeleteFurnitureInRoomCM { get; set; }
        public RoomFurnitureManagementVM()
        {
            AdminWindow tk = System.Windows.Application.Current.Windows.OfType<AdminWindow>().FirstOrDefault();
            
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                IsLoading = true;
                ListRoomType = new ObservableCollection<ComboBoxItem>();
                List<RoomTypeDTO> roomType = await Task.Run(() => RoomTypeService.Ins.GetAllRoomType());
                SetRoomTypeToCombobox(roomType);
                (bool isSuccess, string messageReturn, List<FurnituresRoomDTO> listFurnituresRoomReturn) = await Task.Run(() => FurnituresRoomService.Ins.GetAllFurnituresRoom());

                IsLoading = false;
                if (isSuccess)
                {
                    FurnituresRoomList = new ObservableCollection<FurnituresRoomDTO>(listFurnituresRoomReturn);
                    AllFurnituresRoom = new ObservableCollection<FurnituresRoomDTO>(listFurnituresRoomReturn);
                }
                else
                {
                    CustomMessageBox.ShowOk(messageReturn, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
            });

            MainPageSelectionFilterChangeCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedFilterTypeRoom == null)
                    type = "Tất cả";
                else
                    type = SelectedFilterTypeRoom.Tag.ToString();

                if (SelectedFilterStatusRoom == null)
                    status = "Tất cả";
                else
                    status = SelectedFilterStatusRoom.Tag.ToString();

                if (type == "Tất cả" && status == "Tất cả")
                    FurnituresRoomList = new ObservableCollection<FurnituresRoomDTO>(AllFurnituresRoom);
                else
                    if (type == "Tất cả")
                    FurnituresRoomList = new ObservableCollection<FurnituresRoomDTO>(AllFurnituresRoom.Where(item => item.RoomStatus == status));
                else
                    if (status == "Tất cả")
                    FurnituresRoomList = new ObservableCollection<FurnituresRoomDTO>(AllFurnituresRoom.Where(item => item.RoomType == type));
                else
                    FurnituresRoomList = new ObservableCollection<FurnituresRoomDTO>(AllFurnituresRoom.Where(item => item.RoomType == type && item.RoomStatus == status));

            });

            OpenFurnituresInRoomWindow = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedFurnituresRoom == null)
                    return;

                FurnituresRoomCache = new FurnituresRoomDTO(SelectedFurnituresRoom);

                IsLoading = true;

                RoomFurnitureInfoWindow roomFurnitureInfoWD = new RoomFurnitureInfoWindow();

                tk.MaskOverSideBar.Visibility = Visibility.Visible;

                roomFurnitureInfoWD.ShowDialog();

                IsLoading = false;
            });

            OpenDeleteFurnitureInRoomCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedFurnituresRoom == null)
                    return;

                FurnituresRoomCache = new FurnituresRoomDTO(SelectedFurnituresRoom);

                IsLoading = true;

                RoomFurnitureDeleteWindow roomFurnitureInfoWD = new RoomFurnitureDeleteWindow();

                tk.MaskOverSideBar.Visibility = Visibility.Visible;

                roomFurnitureInfoWD.ShowDialog();

                IsLoading = false;
            });

            FirstLoadInfoWindowCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (FurnituresRoomCache == null)
                    return;
                ListFurnitureNeedDelete = new ObservableCollection<FurnitureDTO>();

                IsLoading = true;

                await LoadFurniture();

                IsLoading = false;

            });

            CloseFurnitureRoomInfoCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                FurnituresRoomCache = null;
                p.Close();
                tk.MaskOverSideBar.Visibility = Visibility.Collapsed;
            });

            CloseFurnitureRoomDeleteCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                FurnituresRoomCache = null;
                p.Close();
                tk.MaskOverSideBar.Visibility = Visibility.Collapsed;
            });

            OpenImportFurnitureRoomCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (SelectedFurnituresRoom == null)
                    return;

                FurnituresRoomCache = new FurnituresRoomDTO(SelectedFurnituresRoom);

                IsLoading = true;

                OrderFurnitureList = new ObservableCollection<FurnitureDTO>();

                RoomFurnitureImportWindow roomFurnitureImportWindow = new RoomFurnitureImportWindow();

                roomFurnitureImportWindow.Show();

                IsLoading = false;
            });

            FirstLoadImportWindowCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                IsLoading = true;

                await LoadAllFurniture();

                IsLoading = false;
            });

            ChoosedFurnitureToListCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedFurniture == null)
                    return;

                FurnitureCache = SelectedFurniture;

                IsLoading = true;

                LoadFurnitureToList(FurnitureCache);

                IsLoading = false;
            });

            DecreaseQuantityOrderItem = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedFurniture == null)
                    return;
                furnitureCache = SelectedFurniture;
                if (furnitureCache.QuantityImportRoom <= 1)
                    OrderFurnitureList.Remove(furnitureCache);
                furnitureCache.DecreaseImport(1);
            });

            IncreaseQuantityOrderItem = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedFurniture == null)
                    return;
                furnitureCache = SelectedFurniture;
                if (furnitureCache.RemainingQuantity == 0)
                {
                    CustomMessageBox.ShowOk("Số lượng tiện nghi trong kho đã hết!", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                furnitureCache.IncreaseImport(1);
            });

            DeleteItemInBillStackCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedFurniture == null)
                    return;
                furnitureCache = SelectedFurniture;
                if (CustomMessageBox.ShowOkCancel("Bạn có muốn xóa tiện nghi này không?", "Cảnh báo", "Có", "Không", CustomMessageBoxImage.Warning)
                    == CustomMessageBoxResult.OK)
                {
                    OrderFurnitureList.Remove(furnitureCache);
                    furnitureCache.DecreaseImport(furnitureCache.QuantityImportRoom);
                }
            });

            ImportListFurnitureToRoomCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                IsLoading = true;

                await ImportListFurnitureToRoom(p);

                IsLoading = false;
            });

            SelectionFilterChangeCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItemFilter != null)
                {
                    if (SelectedItemFilter == "Tất cả")
                        FurnitureList = new ObservableCollection<FurnitureDTO>(AllFurniture);
                    else
                        FurnitureList = new ObservableCollection<FurnitureDTO>(FurnitureService.Ins.GetAllFurnitureByType(SelectedItemFilter, AllFurniture));
                }
            });

            DeleteItemInRoomFurnitureInfoCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (SelectedFurniture == null)
                    return;

                FurnitureCache = SelectedFurniture;

                if (CustomMessageBox.ShowOkCancel("Bạn có muốn xóa " + FurnitureCache.FurnitureName + " ra khỏi phòng " + FurnituresRoomCache.RoomNumber + " không?", "Cảnh báo", "Có", "Không", CustomMessageBoxImage.Warning)
                    == CustomMessageBoxResult.OK)
                {
                    (bool isSuccess, string messageReturn) = await Task.Run(() => FurnituresRoomService.Ins.DeleteFurnitureRoom(FurnituresRoomCache.RoomId, FurnitureCache));
                    if (isSuccess)
                    {
                        CustomMessageBox.ShowOk(messageReturn, "Thành công", "OK", CustomMessageBoxImage.Success);
                        FurnituresRoomCache.ListFurnitureRoom.Remove(furnitureCache);
                        FurnituresRoomCache.SetQuantityAndStringTypeFurniture();

                    }
                    else
                    {
                        CustomMessageBox.ShowOk(messageReturn, "Lỗi", "OK", CustomMessageBoxImage.Error);
                    }
                }
            });

            ChooseItemToListNeedDelete = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedFurniture == null)
                    return;
                FurnitureCache = SelectedFurniture;
                FurnitureCache.IsSelectedDelete = true;
                ListFurnitureNeedDelete.Add(FurnitureCache);
            });

            RemoveItemToListNeedDelete = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedFurniture == null)
                    return;
                FurnitureCache = SelectedFurniture;
                FurnitureCache.IsSelectedDelete = false;
                ListFurnitureNeedDelete.Remove(FurnitureCache);
            });
            bool isChooseAll = false;
            ChooseAllFurnitureToDeleteCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if(isChooseAll)
                {
                    ListFurnitureNeedDelete = new ObservableCollection<FurnitureDTO>(FurnituresRoomCache.ListFurnitureRoom);
                    foreach (var item in ListFurnitureNeedDelete)
                        item.IsSelectedDelete = true;
                    isChooseAll = false;
                }
                else
                {
                    ListFurnitureNeedDelete.Clear();
                    foreach (var item in ListFurnitureNeedDelete)
                        item.IsSelectedDelete = false;
                    isChooseAll = true ;
                }
            });

            DeleteListFurnitureCM = new RelayCommand<Window>((p) => { return true; },async (p) =>
            {
                IsLoading = true;

                Search textSearchFinal = (Search)p.FindName("SearchBox");
                string text = textSearchFinal.Text;

                FilterListFurnitureInRoomByKey(text);

                await DeleteListFurniture(p, tk);

                IsLoading = false;
            });
            CloseDeleteControlCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                ListFurnitureNeedDelete.Clear();
            });
            SelectionFilterInfoChangeCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItemFilter != null)
                {
                    if (SelectedItemFilter == "Tất cả")
                        FurnituresRoomCache.ListFurnitureRoom = new ObservableCollection<FurnitureDTO>(AllFurniture);
                    else
                        FurnituresRoomCache.ListFurnitureRoom = new ObservableCollection<FurnitureDTO>(FurnitureService.Ins.GetAllFurnitureByType(SelectedItemFilter, AllFurniture));
                }
            });
        }
        public void SetRoomTypeToCombobox(List<RoomTypeDTO> roomType)
        {
            ComboBoxItem cbiall = new ComboBoxItem();
            cbiall.Tag = "Tất cả";
            cbiall.Content = "Tất cả loại phòng";
            ListRoomType.Add(cbiall);
            for (int i = 0; i < roomType.Count(); i++)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Tag = cbi.Content = roomType[i].RoomTypeName;
                ListRoomType.Add(cbi);
            }
        }

        public void FilterListFurnitureInRoomByKey(string key)
        {
            ListFurnitureNeedDelete = new ObservableCollection<FurnitureDTO>(ListFurnitureNeedDelete.Where(item => item.FurnitureName.ToLower().Contains(key.ToLower())));
        }
    }
}
