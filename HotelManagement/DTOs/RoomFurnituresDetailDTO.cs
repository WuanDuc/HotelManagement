using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HotelManagement.DTOs
{
    public class RoomFurnituresDetailDTO
    {
        public int RoomFurnituresDetailId { get; set; }
        public string FurnitureId { get; set; }
        public string FurnitureName { get; set; }
        public string FurnitureType { get; set; }
        public string RoomId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public ImageSource FurnitureAvatar { get; set; }
        public byte[] FurnitureAvatarData { get; set; }
        public void SetAvatar()
        {
            FurnitureAvatar = LoadAvatarImage(FurnitureAvatarData);
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
