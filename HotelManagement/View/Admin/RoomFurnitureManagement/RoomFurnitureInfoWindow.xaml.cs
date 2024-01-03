using HotelManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace HotelManagement.View.Admin.RoomFurnitureManagement
{
    /// <summary>
    /// Interaction logic for RoomFurnitureInfoWindow.xaml
    /// </summary>
    public partial class RoomFurnitureInfoWindow : Window
    {
        public RoomFurnitureInfoWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 150);
            timer.Tick += ((sender, e) =>
            {

                if (scrollNote.VerticalOffset < scrollNote.ScrollableHeight)
                {
                    scrollNote.ScrollToVerticalOffset(scrollNote.VerticalOffset + 1);
                }
                else
                {
                    scrollNote.ScrollToTop();
                    timer.Stop();
                }    

            });
            timer.Start();
            
            timer2.Interval = new TimeSpan(0, 0, 1);
            timer2.Tick += ((s, se) =>
            {
                SetDeleteControl();
                timer2.Stop();
            });

        }
        DispatcherTimer timer2 = new DispatcherTimer();
        List<CheckBox> checkboxList = new List<CheckBox>();
        List<StackPanel> stackPanelList = new List<StackPanel>();

        private void RoomFurnitureInfoWD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void RoundBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                timer2.Start();
                Grid grid = sender as Grid;
                Rectangle rec = (Rectangle)grid.FindName("Mask");
                rec.Opacity = 0.15;
            }
        }
        public void SetDeleteControl()
        {
            ListRadio.Visibility = Visibility.Visible;
            TopDeleteControl.Visibility = Visibility.Visible;
            BottomDeleteControl.Visibility = Visibility.Visible;
        }

        private void PackIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListRadio.Visibility = Visibility.Collapsed;
            TopDeleteControl.Visibility = Visibility.Collapsed;
            BottomDeleteControl.Visibility = Visibility.Collapsed;
            for (int i = 0; i < checkboxList.Count; i++)
                checkboxList[i].IsChecked = false;
            foreach (StackPanel item in stackPanelList)
                item.Visibility = Visibility.Collapsed;
        }

        private void RoundBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            timer2.Stop();
            Grid grid = sender as Grid;
            Rectangle rec = (Rectangle)grid.FindName("Mask");
            rec.Opacity = 0;
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            checkboxList.Add(sender as CheckBox);
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < checkboxList.Count; i++)
                checkboxList[i].IsChecked = true;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void DeleteCountBox_Loaded(object sender, RoutedEventArgs e)
        {
            stackPanelList.Add(sender as StackPanel);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            stackPanelList[checkboxList.IndexOf(sender as CheckBox)].Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            stackPanelList[checkboxList.IndexOf(sender as CheckBox)].Visibility = Visibility.Collapsed;
        }

        private void TextCountBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DeleteText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkboxList.ForEach(item => item.IsChecked = false);
        }

        private void SearchBox_SearchTextChange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FurnitureStack.ItemsSource);
            if (view != null)
            {
                view.Filter = Filter;
                CollectionViewSource.GetDefaultView(FurnitureStack.ItemsSource).Refresh();
            }
        }
        private bool Filter(object item)
        {
                return true;
        }
    }
}
