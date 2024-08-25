using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;
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
using TagLib;

namespace MusicHUBClient
{
    /// <summary>
    /// Логика взаимодействия для AddTrackWindow.xaml
    /// </summary>
    public partial class AddTrackWindow : Window
    {
        MainWindow mainWindow1;
        string audioPath;
        byte[] coverBytes;
        private bool trackNameCheck = false;
        private bool trackArtistCheck = false;
        private bool trackAudioCheck = false;
        public AddTrackWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            mainWindow1 = mainWindow;
            Loaded += AddTrackWindow_Loaded;
        }

        private void AddTrackWindow_Loaded(object sender, RoutedEventArgs e)
        {
            trackCoverImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\Resources\\UI\\defaultIcon.png")));
            coverBytes = System.IO.File.ReadAllBytes(System.IO.Path.GetFullPath("..\\..\\..\\Resources\\UI\\defaultIcon.png"));
            addTrackButton.Visibility = Visibility.Collapsed;
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
                trackAudioCheck = true;
                try {
                    audioPath = dialog.FileName;
                    TagLib.File tagFile = TagLib.File.Create(audioPath);
                    if (tagFile.Tag.Performers != null && tagFile.Tag.Performers.Length > 0)
                    {
                        trackArtist.Text = string.Join(", ", tagFile.Tag.Performers);
                    }
                    if(tagFile.Tag.Title!=null)
                    {
                        trackName.Text = tagFile.Tag.Title;
                    }
                    if(tagFile.Tag.Pictures.FirstOrDefault()!=null)
                    {
                        trackCoverImage.Source = BitmapFrame.Create(new MemoryStream(tagFile.Tag.Pictures.FirstOrDefault().Data.Data),
                        BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                        coverBytes = tagFile.Tag.Pictures.FirstOrDefault().Data.Data;

                    }
                }
                catch { audioPath = dialog.FileName; }
            }
            checkForAdd();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? success = dialog.ShowDialog();
            if (success == true)
            {
                coverBytes = System.IO.File.ReadAllBytes(dialog.FileName);
                trackCoverImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(dialog.FileName)));
            }
        }

        private void addTrackButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow1.addTrack(trackName.Text, trackArtist.Text, audioPath, coverBytes);
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

        private void trackName_TextChanged(object sender, TextChangedEventArgs e)
        {
            tempTextName.Text = "";
            if(trackName.Text.Trim() == "")
            {
                trackNameCheck = false;
            }
            else
            {
                trackNameCheck = true;
            }
            checkForAdd();
        }

        private void trackArtist_TextChanged(object sender, TextChangedEventArgs e)
        {
            tempTextArtist.Text = "";
            if (trackArtist.Text.Trim() == "")
            {
                trackArtistCheck = false;
            }
            else
            {
                trackArtistCheck = true;
            }
            checkForAdd();
        }

        private void checkForAdd()
        {
            if(trackNameCheck == true && trackAudioCheck == true && trackArtistCheck == true)
            {
                addTrackButton.Visibility = Visibility.Visible;
            }
            else
            {
                addTrackButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
