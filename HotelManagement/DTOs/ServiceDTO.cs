using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HotelManagement.DTOs
{
    public class ServiceDTO : INotifyPropertyChanged
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

        private ImageSource serviceAvatar;
        public ImageSource ServiceAvatar
        {
            get { return serviceAvatar; }
            set { SetField(ref serviceAvatar, value, "ServiceAvatar"); }
        }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceType { get; set; }
        public double ServicePrice { get; set; }
        public byte[] ServiceAvatarData { get; set; }

        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { SetField(ref quantity, value, "Quantity"); }
        }
        private double importPrice;
        public double ImportPrice
        {
            get { return importPrice; }
            set { SetField(ref importPrice, value, "ImportPrice"); }
        }

        private int importQuantity;
        public int ImportQuantity
        {
            get { return importQuantity; }
            set { SetField(ref importQuantity, value, "ImportQuantity"); }
        }
        public string PriceStr { get; set; }
        public string Unit { get; set; }

        public ServiceDTO()
        {
        }
        public ServiceDTO(string serviceId, string serviceName, string serviceType, double servicePrice,int quantity, byte[] serviceAvatarData, ImageSource serviceAvatar)
        {
            ServiceId = serviceId;
            ServiceName = serviceName;
            ServiceType = serviceType;
            ServicePrice = servicePrice;
            Quantity = quantity;
            ServiceAvatarData = serviceAvatarData;
            ServiceAvatar = serviceAvatar;
            PriceStr = Helper.FormatVNMoney((float)ServicePrice);
        }
        public ServiceDTO(ServiceDTO s)
        {
            ServiceId = s.ServiceId;
            ServiceName = s.ServiceName;
            ServiceType = s.ServiceType;
            ServicePrice = s.ServicePrice;
            Quantity = s.Quantity;
            ServiceAvatarData = s.ServiceAvatarData;
            ServiceAvatar = s.ServiceAvatar;
            PriceStr = Helper.FormatVNMoney((float)ServicePrice);
        }

        public void FormatStringUnitAndPrice()
        {
            PriceStr = Helper.FormatVNMoney((float)ServicePrice);
            if (ServiceName == "Giặt sấy")
            {
                Unit = "Kilogram";
            }
            if (ServiceName == "Dọn dẹp")
            {
                Unit = "Lần";
            }
        }
        public void SetAvatar()
        {
            if(ServiceAvatarData != null)
                ServiceAvatar = LoadAvatarImage(ServiceAvatarData);
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
            ServiceAvatarData = photo_aray;
            ServiceAvatar = _image;
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
    }
}
