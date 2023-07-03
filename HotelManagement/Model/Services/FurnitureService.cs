using HotelManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Win32;
using System.Runtime.Remoting.Contexts;
using System.Collections.ObjectModel;
using System.Windows.Automation.Peers;
using System.Data;
using HotelManagement.View.Admin;
using HotelManagement.View.Staff;
using HotelManagement.ViewModel.AdminVM;
using HotelManagement.ViewModel.StaffVM;

namespace HotelManagement.Model.Services
{
    public class FurnitureService
    {
        private FurnitureService() { }
        private static FurnitureService _ins;
        public static FurnitureService Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new FurnitureService();
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<(bool , string ,List<FurnitureDTO>)> GetAllFurniture()
        {
            try
            {
                List<FurnitureDTO> listFurniture = new List<FurnitureDTO>();
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    listFurniture = await (
                        from p in db.Furnitures
                        select new FurnitureDTO
                        {
                            FurnitureID = p.FurnitureId,
                            FurnitureAvatarData = p.FurnitureAvatar,
                            FurnitureName = p.FurnitureName,
                            FurnitureType = p.FurnitureType,
                            Quantity = (int)p.FurnitureStorage.QuantityFurniture,
                        }
                    ).ToListAsync();
                }
                for (int i = 0; i < listFurniture.Count; i++)
                    listFurniture[i].SetAvatar();
                return (true, "", listFurniture);
            }
            catch(EntityException e)
            {
                return (false, "Mất kết nối cơ sở dữ liệu", null);
            }
            catch(Exception ex)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }

        public List<FurnitureDTO> GetAllFurnitureByType(string typeSelection, ObservableCollection<FurnitureDTO> allFurniture)
        {
            try
            {
                return allFurniture.Where(item => item.FurnitureType == typeSelection).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<string>> GetAllFurnitureType()
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                   var list = await db.Furnitures.Select(x => x.FurnitureType).ToListAsync();
                    list.Insert(0, "Tất cả");
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(bool, string)> SaveEditFurniture(FurnitureDTO furnitureSelected)
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    Furniture CheckFurnitureName = await db.Furnitures.FirstOrDefaultAsync(item => item.FurnitureName.Equals(furnitureSelected.FurnitureName) && item.FurnitureId != furnitureSelected.FurnitureID);
                    if (CheckFurnitureName != null)
                        return (false, "Đã có tiện nghi trong cơ sở  dữ liệu");

                    Furniture furniture = await db.Furnitures.FirstOrDefaultAsync(item => item.FurnitureId == furnitureSelected.FurnitureID);
                    if(furniture == null) 
                    {
                        return (false, "Không tìm thấy tiện nghi");
                    }
                    furniture.FurnitureName = furnitureSelected.FurnitureName;
                    furniture.FurnitureType = furnitureSelected.FurnitureType;
                    furniture.FurnitureStorage.QuantityFurniture = furnitureSelected.Quantity;
                    furniture.FurnitureAvatar = furnitureSelected.FurnitureAvatarData;

                    await db.SaveChangesAsync();
                    return (true, "Cập nhật thành công");
                }
            }
            catch(EntityException ex) 
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch(Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        public async Task<(bool, string, string)> AddFurniture(FurnitureDTO furnitureSelected)
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    Furniture furniture = await db.Furnitures.FirstOrDefaultAsync(item => item.FurnitureName == furnitureSelected.FurnitureName && item.FurnitureType == furnitureSelected.FurnitureType);
                    
                    if (furniture != null)
                    {
                        return (false, "Đã có tiện nghi trong cơ sở dữ liệu", "-1");
                    }
                    int ID = db.Furnitures.ToList().Count();
                    string mainID;
                    if (ID == 0)
                        mainID = "0001";
                    else
                    {
                        ID = int.Parse(db.Furnitures.ToList().Max(item => item.FurnitureId));
                        mainID = getID(++ID);
                    }
                    Furniture fnt = new Furniture
                    {
                        FurnitureId = mainID,
                        FurnitureName = furnitureSelected.FurnitureName,
                        FurnitureType = furnitureSelected.FurnitureType,
                        FurnitureAvatar = furnitureSelected.FurnitureAvatarData,
                    };
                    FurnitureStorage furnitureStorage = new FurnitureStorage
                    {
                        FurnitureId = fnt.FurnitureId,
                        QuantityFurniture = 0,
                    };
                    db.Furnitures.Add(fnt);
                    db.FurnitureStorages.Add(furnitureStorage);
                    await db.SaveChangesAsync();
                    return (true, "Thêm tiện nghi thành công", mainID);
                }
            }
            catch (EntityException ex)
            {
                return (false, "Mất kết nối cơ sở dữ liệu", "-1");
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", "-1");
            }
        }

        public async Task<(bool, string)> DeleteFurniture(FurnitureDTO furnitureSelected)
        {
            try
            {
                using(HotelManagementEntities db = new HotelManagementEntities())
                {
                    Furniture furniture = await db.Furnitures.FirstOrDefaultAsync(item => item.FurnitureId == furnitureSelected.FurnitureID);
                    if (furniture == null)
                        return (false, "Không tìm thấy tiện nghi trong cơ sở dữ liệu");

                    db.FurnitureStorages.Remove(furniture.FurnitureStorage);
                    db.Furnitures.Remove(furniture);

                    await db.SaveChangesAsync();
                    return (true, "Xóa tiện nghi thành công");
                }    
            }
            catch(EntityException e)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        
        public async Task<(bool, string)> ImportFurniture(FurnitureDTO furnitureSelected, string importPrice, string importQuantity)
        {
            try
            {
                using(HotelManagementEntities db = new HotelManagementEntities())
                {
                    Furniture furniture = await db.Furnitures.FirstOrDefaultAsync(item => item.FurnitureId == furnitureSelected.FurnitureID);
                    if (furniture == null)
                        return (false, "Không tìm thấy tiện nghi trong cơ sở dữ liệu");

                    int ID = db.FurnitureReceipts.ToList().Count();
                    string mainID;
                    if (ID == 0)
                        mainID = "0001";
                    else
                    {
                        ID = int.Parse(db.FurnitureReceipts.ToList().Max(item => item.FurnitureReceiptId));
                        mainID = getID(++ID);
                    }

                    string id = "";

                    if (AdminVM.CurrentStaff != null)
                    {
                        id = AdminVM.CurrentStaff.StaffId;
                    }
                    else
                    {
                        id = StaffVM.CurrentStaff.StaffId;
                    }
                    FurnitureReceipt furnitureReceipt = new FurnitureReceipt
                    {
                        FurnitureReceiptId = mainID,
                        FurnitureId = furniture.FurnitureId,
                        StaffId = id,
                        ImportPrice = float.Parse(importPrice),
                        Quantity = int.Parse(importQuantity),
                        CreateAt = DateTime.Now,
                    };
                    db.FurnitureReceipts.Add(furnitureReceipt);
                    furniture.FurnitureStorage.QuantityFurniture = furniture.FurnitureStorage.QuantityFurniture + int.Parse(importQuantity);
                    await db.SaveChangesAsync();
                    return (true, "Nhập sản phẩm thành công");
                }    
            }
            catch(EntityException e)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }

        public async Task<(bool, string, List<FurnitureDTO>)> ImportListFurniture(ObservableCollection<FurnitureDTO> orderList)
        {
            try
            {
                List<FurnitureDTO> listFurniture = new List<FurnitureDTO>(orderList);
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    int Length = orderList.Count;
                    int ID = db.FurnitureReceipts.ToList().Count();
                    string mainID;
                    if (ID == 0)
                        mainID = "0001";
                    else
                    {
                        ID = int.Parse(db.FurnitureReceipts.ToList().Max(item => item.FurnitureReceiptId));
                    }
                    string id = "";

                    if (AdminVM.CurrentStaff != null)
                    {
                        id = AdminVM.CurrentStaff.StaffId;
                    }
                    else
                    {
                        id = StaffVM.CurrentStaff.StaffId;
                    }
                    for (int i = 0; i < Length; i++)
                    {
                        FurnitureDTO temp = orderList[i];
                        Furniture furniture = await db.Furnitures.FirstOrDefaultAsync(item => item.FurnitureId == temp.FurnitureID);
                        if (furniture == null)
                            return (false, "Không tìm thấy tiện nghi " + furniture.FurnitureName + " trong cơ sở dữ liệu",null);

                        mainID = getID(++ID);

                        FurnitureReceipt furnitureReceipt = new FurnitureReceipt
                        {
                            FurnitureReceiptId = mainID,
                            FurnitureId = furniture.FurnitureId,
                            StaffId = id,
                            ImportPrice = temp.ImportPrice,
                            Quantity = temp.ImportQuantity,
                            CreateAt = DateTime.Now,
                        };
                        db.FurnitureReceipts.Add(furnitureReceipt);
                        furniture.FurnitureStorage.QuantityFurniture = furniture.FurnitureStorage.QuantityFurniture + temp.ImportQuantity;
                        listFurniture[i].Quantity = (int)furniture.FurnitureStorage.QuantityFurniture;
                    }    
                    await db.SaveChangesAsync();
                    return (true, "Nhập tiện nghi thành công", listFurniture);
                }
            }
            catch (EntityException e)
            {
                return (false, "Mất kết nối cơ sở dữ liệu", null);
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
        public ImageSource LoadImage(string ImagePath)
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(ImagePath, UriKind.RelativeOrAbsolute);
            _image.EndInit();
            return _image;
        }
        public string getID(int id)
        {
            if (id < 10)
                return "000" + id;
            if (id < 100)
                return "00" + id;
            if (id < 1000)
                return "0" + id;

            return id.ToString();
        }
        public BitmapImage LoadAvatarImage(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;

            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();

            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();

            return bitmapImage;
        }
        public async Task<List<ImportProductDTO>> GetListImportFuniture()
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    List<ImportProductDTO> ImportFuniture = await (
                                                            from g in db.FurnitureReceipts
                                                            join s in db.Furnitures
                                                            on g.FurnitureId equals s.FurnitureId into gs
                                                            from s in gs.DefaultIfEmpty()
                                                            join st in db.Staffs
                                                            on g.StaffId equals st.StaffId into gst
                                                            from st in gst.DefaultIfEmpty()
                                                            orderby g.CreateAt descending
                                                            select new ImportProductDTO
                                                            {
                                                                ImportId = g.FurnitureId,
                                                                ProductName = s.FurnitureName,
                                                                ProductImportQuantity = (int)g.Quantity,
                                                                ProductImportPrice = (float)g.ImportPrice,
                                                                StaffName = st.StaffName,
                                                                CreatedDate = (DateTime)g.CreateAt,
                                                                typeimport = 1
                                                            }
                                                            ).ToListAsync();
                    return ImportFuniture;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<ImportProductDTO>> GetListImportFuniture(int month)
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    List<ImportProductDTO> ImportFuniture = await (
                                                            from g in db.FurnitureReceipts
                                                            join s in db.Furnitures
                                                            on g.FurnitureId equals s.FurnitureId into gs
                                                            from s in gs.DefaultIfEmpty()
                                                            join st in db.Staffs
                                                            on g.StaffId equals st.StaffId into gst
                                                            from st in gst.DefaultIfEmpty()
                                                            where ((DateTime)g.CreateAt).Year == DateTime.Today.Year && ((DateTime)g.CreateAt).Month == month
                                                            orderby g.CreateAt descending
                                                            select new ImportProductDTO
                                                            {
                                                                ImportId = g.FurnitureId,
                                                                ProductName = s.FurnitureName,
                                                                ProductImportQuantity = (int)g.Quantity,
                                                                ProductImportPrice = (float)g.ImportPrice,
                                                                StaffName = st.StaffName,
                                                                CreatedDate = (DateTime)g.CreateAt,
                                                                typeimport = 1
                                                            }
                                                            ).ToListAsync();
                    return ImportFuniture;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
