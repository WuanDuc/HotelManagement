using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HotelManagement.Utils;
using MaterialDesignThemes.Wpf;

namespace HotelManagement.DTOs
{
    public class FurnituresRoomDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        public string RoomId { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public string RoomStatus { get; set; }
        public string CustomerName { get; set; }
        public string Note { get; set; }
        public int CustomerQuantity { get; set; }

       

        public FurnituresRoomDTO() { }
        public FurnituresRoomDTO(FurnituresRoomDTO furnituresRoomDTO)
        {
            RoomId = furnituresRoomDTO.RoomId;
            RoomNumber = furnituresRoomDTO.RoomNumber;
            RoomType = furnituresRoomDTO.RoomType;
            RoomStatus = furnituresRoomDTO.RoomStatus;
            CustomerName = furnituresRoomDTO.CustomerName;
            Note = furnituresRoomDTO.Note;
            CustomerQuantity = furnituresRoomDTO.CustomerQuantity;
        }

        private Brush backgroundRoomBrush;
        public Brush BackgroundRoomBrush
        {
            get { return backgroundRoomBrush; }
            set { SetField(ref backgroundRoomBrush, value, "BackgroundRoomBrush"); }
        }

        private ObservableCollection<FurnitureDTO> listFurnitureRoom;
        public ObservableCollection<FurnitureDTO> ListFurnitureRoom
        {
            get { return listFurnitureRoom; }

            set { SetField(ref listFurnitureRoom, value, "ListFurnitureRoom"); }
        }

        private int allFurnitureQuantity;
        public int AllFurnitureQuantity
        {
            get { return allFurnitureQuantity; }

            set { SetField(ref allFurnitureQuantity, value, "AllFurnitureQuantity"); }
        }
        private string allFurnitureString;
        public string AllFurnitureString
        {
            get { return allFurnitureString; }

            set { SetField(ref allFurnitureString, value, "AllFurnitureString"); }
        }


        private ObservableCollection<bool> roomCusList;
        public ObservableCollection<bool> RoomCusList
        {
            get { return roomCusList; }
            set { SetField(ref roomCusList, value, "RoomCusList"); }
        }

        private bool isEmptyRoom;
        public bool IsEmptyRoom
        {
            get { return isEmptyRoom; }
            set { SetField(ref isEmptyRoom, value, "IsEmptyRoom"); }
        }

        public void SetOtherProperty()
        {
            var converter = new BrushConverter();
            if (RoomStatus == ROOM_STATUS.RENTING)
            {
                IsEmptyRoom = false;
                BackgroundRoomBrush = (Brush)converter.ConvertFromString("#FED600");
            }
            else
            {
                IsEmptyRoom = true;
                BackgroundRoomBrush = (Brush)converter.ConvertFromString("#009099");
            }

            BackgroundRoomBrush.Freeze();

            RoomCusList = new ObservableCollection<bool>();
            for (int i = 0; i < CustomerQuantity; i++)
            {
                RoomCusList.Add(true);
            }
        }

        public void SetQuantityAndStringTypeFurniture()
        {
            List<string> furnitureType = ListFurnitureRoom.Select(item => item.FurnitureType).Distinct().ToList();
            int length = furnitureType.Count();
            AllFurnitureQuantity = 0;
            AllFurnitureString = "";
            for (int i = 0; i < length; i++)
            {
                AllFurnitureQuantity += 1;
                if(i == 0)
                    AllFurnitureString += furnitureType[i];
                else
                    AllFurnitureString += (", " + furnitureType[i]);
            }
        }
        public void DeleteListFurniture(ObservableCollection<FurnitureDTO> listDelete)
        {
            foreach (FurnitureDTO item in listDelete)
            {
                if (item.DeleteInRoomQuantity == item.InUseQuantity)
                    ListFurnitureRoom.Remove(item);
                else
                {
                    item.InUseQuantity -= item.DeleteInRoomQuantity;
                    item.DeleteInRoomQuantity = item.InUseQuantity;
                }    
            }    
        }
    }
}
