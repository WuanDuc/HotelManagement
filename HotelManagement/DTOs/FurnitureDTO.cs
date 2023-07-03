using CinemaManagementProject.Utilities;
using HotelManagement.Utilities;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HotelManagement.DTOs
{
    public class FurnitureDTO : INotifyPropertyChanged
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

        public byte[] FurnitureAvatarData { get; set; }
        private ImageSource furnitureAvatar;
        public ImageSource FurnitureAvatar
        {
            get { return furnitureAvatar; }
            set { SetField(ref furnitureAvatar, value, "FurnitureAvatar"); }
        }
        public string FurnitureName { get; set; }
        public string FurnitureID { get; set; }
        public string FurnitureType { get; set; }
        public int Quantity { get; set; }

        private float importPrice;
        public float ImportPrice
        {
            get { return importPrice; }
            set { SetField(ref importPrice, value, "ImportQuantity"); }
        }

        private int importQuantity;
        public int ImportQuantity
        {
            get { return importQuantity; }
            set { SetField(ref importQuantity, value, "ImportQuantity"); }
        }

        public float TotalImportPrice { get; set; }

        public string TotalImportPriceStr { get;  set; }

        public int inUseQuantity;
        public int InUseQuantity
        {
            get { return inUseQuantity; }
            set { SetField(ref inUseQuantity, value, "InUseQuantity"); }
        }
        public int TotalUseQuantity { get; set; }

        private int remainingQuantity;
        public int RemainingQuantity
        {
            get { return remainingQuantity; }
            set { SetField(ref remainingQuantity, value, "RemainingQuantity"); }
        }

        private int quantityImportRoom;
        public int QuantityImportRoom
        {
            get { return quantityImportRoom; }
            set { SetField(ref quantityImportRoom, value, "QuantityImportRoom"); }
        }

        private int deleteInRoomQuantity;
        public int DeleteInRoomQuantity
        {
            get { return deleteInRoomQuantity; }
            set { SetField(ref deleteInRoomQuantity, value, "DeleteInRoomQuantity"); }
        }
        private bool isSelectedDelete;
        public bool IsSelectedDelete
        {
            get { return isSelectedDelete; }
            set { SetField(ref isSelectedDelete, value, "IsSelectedDelete"); }
        }
        public string DisplayQuantity { get; set; }

        public FurnitureDTO()
        {
            FurnitureName = "";
            FurnitureType = "";
            Quantity = 0;
        }
        public FurnitureDTO(FurnitureDTO furniture)
        {
            this.FurnitureID = furniture.FurnitureID;
            this.FurnitureAvatar = furniture.FurnitureAvatar;
            this.FurnitureName = furniture.FurnitureName;
            this.FurnitureType = furniture.FurnitureType;
            this.FurnitureAvatarData = furniture.FurnitureAvatarData;
            this.Quantity = furniture.Quantity;
            this.TotalUseQuantity = furniture.TotalUseQuantity;
            this.InUseQuantity = furniture.InUseQuantity;
            this.QuantityImportRoom = furniture.QuantityImportRoom;
            this.RemainingQuantity = furniture.RemainingQuantity;
        }
        public void SetTotalImportPrice()
        {
            TotalImportPrice = ImportQuantity * ImportPrice;
            TotalImportPriceStr = Helper.FormatVNMoney(TotalImportPrice);
        }

        public bool IsDeleteLessThanInUse()
        {
            return DeleteInRoomQuantity <= InUseQuantity;
        }

        public void SetRemaining()
        {
            RemainingQuantity = Quantity - TotalUseQuantity;
        }
        public void SetInUseQuantity(int number)
        {
            InUseQuantity = DeleteInRoomQuantity = number;
        }
        public void IncreaseImport(int length)
        {
            RemainingQuantity -= length;
            QuantityImportRoom += length;
        }
        public void DecreaseImport(int length)
        {
            RemainingQuantity += length;
            QuantityImportRoom -= length;
        }

        public void SetAvatar()
        {
            FurnitureAvatar = LoadAvatarImage(FurnitureAvatarData);
        }
        public void SetAvatar(string filePath)
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
            _image.EndInit();

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] photo_aray = new byte[fs.Length];
            fs.Read(photo_aray, 0, photo_aray.Length);
            FurnitureAvatarData = photo_aray;
            FurnitureAvatar = _image;
        }
        public BitmapImage LoadAvatarImage(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;

            Image img = Image.FromStream(stream);

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();

            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();

            bitmapImage.Freeze();
            return bitmapImage;
        }

        public bool IsEmptyFurniture()
        {
            if (string.IsNullOrWhiteSpace(FurnitureName))
                return true;
            if (string.IsNullOrWhiteSpace(FurnitureType))
                return true;
            if (string.IsNullOrWhiteSpace(DisplayQuantity))
                return true;
            if (FurnitureAvatarData == null ||FurnitureAvatarData.Length == 0)
                return true;
            return false;   
        }
    }
}
