using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelManagement.ViewModel.AdminVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.ViewModel.AdminVM.Tests
{
    [TestClass()]
    public class AdminVMTests
    {
        //private AdminVM _adminVM;
        //[TestInitialize]
        //public void Setup()
        //{
        //    _adminVM = new AdminVM();
        //}
        //[TestMethod()]
        //public void AdminVMTest()
        //{

        //}

        [TestMethod()]
        public void setNavigateHelpScreenTest()
        {
            AdminVM _adminVM = new AdminVM();
            _adminVM.setNavigateHelpScreen();
            Assert.IsNotNull(_adminVM.CurrentView);
        }

        [TestMethod()]
        public void SetAvatarNameTest()
        {

        }

        [TestMethod()]
        public void LoadAvatarImageTest()
        {

        }
    }
}