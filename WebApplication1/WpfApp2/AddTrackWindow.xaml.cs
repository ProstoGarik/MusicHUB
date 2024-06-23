using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для AddTrackWindow.xaml
    /// </summary>
    public partial class AddTrackWindow : Window
    {
        MainWindow mainWindow1;
        string audioPath;
        string coverPath;
        public AddTrackWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            mainWindow1 = mainWindow;
            Loaded += AddTrackWindow_Loaded;
        }

        private void AddTrackWindow_Loaded(object sender, RoutedEventArgs e)
        {
            trackCoverImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\Resources\\UI\\defaultIcon.png")));
        }

        private void exitTrackAdding_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void chooseAudioButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? success = dialog.ShowDialog();
            if (success == true)
            {
                audioPath = dialog.FileName;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? success = dialog.ShowDialog();
            if (success == true)
            {
                coverPath = dialog.FileName;
                trackCoverImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(dialog.FileName)));
            }
        }

        private void addTrackButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow1.addTrack(trackName.Text, trackArtist.Text, audioPath, coverPath);
            Close();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            chooseCoverOverlay.Visibility = Visibility.Visible;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            chooseCoverOverlay.Visibility = Visibility.Collapsed;
        }

        private void trackName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            trackName.Text = string.Empty;
        }

        private void trackArtist_MouseDown(object sender, MouseButtonEventArgs e)
        {
            trackArtist.Text = string.Empty;
        }
    }
}
