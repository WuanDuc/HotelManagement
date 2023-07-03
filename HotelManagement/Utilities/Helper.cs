using HotelManagement.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace HotelManagement.Utilities
{
    public class Helper
    {
        public static bool IsPhoneNumberTinh(string number)
        {
            if (number is null) return false;
            return Regex.Match(number, "^(0|\\+84)(\\s|\\.)?((3[2-9])|(5[689])|(7[06-9])|(8[1-689])|(9[0-46-9]))(\\d)(\\s|\\.)?(\\d{3})(\\s|\\.)?(\\d{3})$").Success;
        }
        public static bool IsPhoneNumber(string number)
        {
            if (number is null) return false;
            return Regex.Match(number, @"(([03+[2-9]|05+[6|8|9]|07+[0|6|7|8|9]|08+[1-9]|09+[1-4|6-9]]){3})+[0-9]{7}\b").Success;
        }
        public static string FormatVNMoney(double money)
        {
            if (money == 0)
            {
                return "0 ₫";
            }
            return String.Format(CultureInfo.InvariantCulture,
                                "{0:#,#} đ", money);
        }
        public static string FormatVNMoney2(double money)
        {
            if (money == 0)
            {
                return "0 VNĐ";
            }
            return String.Format(CultureInfo.InvariantCulture,
                                "{0:#,#} VNĐ", money);
        }
        public static BitmapImage LoadBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
        public static bool CheckEmailStaff(string currentEmail)
        {
            using (var context = new HotelManagementEntities())
            {
                foreach(var staff in context.Staffs)
                {
                    if (staff.Email == currentEmail) return true;
                }
                return false;
            } 
        }

        internal static string GetEmailTemplatePath(string rESET_PASSWORD_FILE)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\EmailTemplates", $"{rESET_PASSWORD_FILE}" /*SelectedItem.Image*/);
        }
        public static class Number
        {
            public static bool IsNumeric(string value)
            {
                return value.All(char.IsNumber) ||
                     ((value.Substring(1, value.Length - 1).All(char.IsNumber) && value[0] == '-'));
            }
            public static bool IsPositive(string value)
            {
                return value[0] != '-' && value != "0";
            }

        }
        public static string ConvertDoubleToPercentageStr(double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero).ToString("P", CultureInfo.InvariantCulture);
        }
    }
    
}
