using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelManagement.ViewModel.AdminVM.StatisticalManagementVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;

namespace HotelManagement.ViewModel.AdminVM.StatisticalManagementVM.Tests
{
    [TestClass()]
    public class IndexConverterTests
    {

        [TestMethod()]
        public void Convert_WithValidListViewItem_ReturnsCorrectIndex()
        {
            // Arrange
            IndexConverter indexConverter = new IndexConverter();
            ListViewItem listViewItem = new ListViewItem { Content = "Item 2" };
            ListView listView = new ListView();
            listView.Items.Add("Item 1");
            listView.Items.Add(listViewItem);
            listView.Items.Add("Item 3");
            // Act
            object result = indexConverter.Convert(listViewItem, typeof(string), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.AreEqual("2", result.ToString());
        }

        [TestMethod()]
        public void Convert_WithInvalidListViewItem_ReturnsNull()
        {
            // Arrange
            IndexConverter indexConverter = new IndexConverter();
            object invalidListViewItem = new object(); 

            // Act
            object result = indexConverter.Convert(invalidListViewItem, typeof(string), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void Convert_WithListViewItemNotInListView_ReturnsNull()
        {
            // Arrange
            IndexConverter indexConverter = new IndexConverter();
            ListViewItem listViewItem = new ListViewItem { Content = "Item 2" };
            ListView listView = new ListView();
            listView.Items.Add("Item 1");
            listView.Items.Add("Item 5");
            listView.Items.Add("Item 3");
            // Act
            object result = indexConverter.Convert(listViewItem, typeof(string), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.AreEqual("2", result.ToString());
        }

        [TestMethod()]
        public void ConvertBack_ThrowsNotImplementedException()
        {
            // Arrange
            IndexConverter converter = new IndexConverter();

            // Act & Assert
            Assert.ThrowsException<NotImplementedException>(() => converter.ConvertBack(null, null, null, null));
        }
    }
}