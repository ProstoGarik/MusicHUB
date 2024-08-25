using System.IO;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Win32;

namespace MusicHUBClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection hubConnection;
        public byte[] tempBytes;
        public byte[] tempBytesCover;
        public List<byte> recievedBytes;
        public List<byte> recievedBytesDisplayCover;
        public MediaPlayer mediaPlayer;
        public bool isPlaying;
        int displayStartIndex = 0;
        private string TempFolderPath = "..\\..\\..\\RunningTemp\\";
        private Uri TempAudioUri;
        DispatcherTimer timer;
        private bool timerPaused = false;
        AddTrackWindow addTrackWindow;
        public MainWindow()
        {
            InitializeComponent();

            System.IO.DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetFullPath("..\\..\\..\\RunningTemp\\"));
            foreach(FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7196/chat").Build();
            hubConnection.Closed += HubConnetction_Closed;
            Loaded += MainWindow_Loaded;
            tempBytes = new byte[] { };
            recievedBytes = new List<byte>();
            recievedBytesDisplayCover = new List<byte>();
            tempBytesCover = new byte[] { };
            mediaPlayer = new MediaPlayer();
            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            
            

            TempAudioUri = new Uri(System.IO.Path.GetFullPath("..\\..\\..\\RunningTemp\\mymusic.wav"));
        }

        private void MediaPlayer_MediaOpened(object? sender, EventArgs e)
        {
            string seconds = mediaPlayer.NaturalDuration.TimeSpan.Seconds < 10 ? "0" + mediaPlayer.NaturalDuration.TimeSpan.Seconds.ToString() : mediaPlayer.NaturalDuration.TimeSpan.Seconds.ToString();
            trackFullLengthTextBlock.Text = mediaPlayer.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + seconds;
            trackPositionSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            EndLoadingPlayer();
            timerPaused = false;
        }

        public async Task HubConnetction_Closed(Exception? arg)
        {
            //listBox1.Items.Add("Соединение разорвано, переподключение...");
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await hubConnection.StartAsync();
        }
        //private void UpdateListBox(string message)
        //{
        //    if (!listBox1.Dispatcher.CheckAccess())
        //    {
        //        listBox1.Dispatcher.Invoke(new Action<string>(UpdateListBox), message);
        //    }
        //    else
        //    {
        //        listBox1.Items.Add(message);
        //    }
        //}

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartLoading();
            //hubConnection.On<string>("RecieveMessage", message => { UpdateListBox(message); });
            hubConnection.On<List<byte>>("RecieveAudioBytes", bytes => {
                recievedBytes.AddRange(bytes);
                //if (!listBox1.Dispatcher.CheckAccess())
                //{
                //    listBox1.Dispatcher.Invoke(new Action<string>(UpdateListBox), "Получено " + bytes.Count() + " байт. Всего " + recievedBytes.Count() + " байт");
                //}
                //else
                //{
                //    listBox1.Items.Add("Получено " + bytes.Count() + " байт. Всего " + recievedBytes.Count() + " байт");
                //}
            });

            try
            {
                await hubConnection.StartAsync();

            }
            catch
            {
            }

            hubConnection.On<int>("RecievingAudioDone", async byteCount =>
            {
                if (byteCount == recievedBytes.Count)
                {
                    await System.IO.File.WriteAllBytesAsync(TempFolderPath + "\\mymusic.wav", recievedBytes.ToArray());
                    Dispatcher.Invoke(() =>
                    {
                        mediaPlayer.Open(TempAudioUri);   
                    });
                }
            });


            hubConnection.On<List<byte>>("RecieveDisplayCoverBytes", bytes =>
            {
                recievedBytesDisplayCover.AddRange(bytes);
            });

            hubConnection.On<int, int>("RecieveDisplayCoverBytesDone", (count, index) =>
            {
                switch (index)
                {
                    case 0:
                        ApplyDisplayCover(0);
                        Dispatcher.Invoke(() =>
                        {
                            displayCoverImage0.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\RunningTemp\\display1.png")));
                        });
                        recievedBytesDisplayCover = new List<byte>();
                        break;
                    case 1:
                        ApplyDisplayCover(1);
                        Dispatcher.Invoke(() =>
                        {
                            try
                            {
                                displayCoverImage1.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\RunningTemp\\display2.png")));
                            }
                            catch
                            {
                                Thread.Sleep(300);
                                ApplyDisplayCover(1);
                            }
                        });
                        recievedBytesDisplayCover = new List<byte>();
                        break;
                    case 2:
                        ApplyDisplayCover(2);
                        Dispatcher.Invoke(() =>
                        {
                            try
                            {
                                displayCoverImage2.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\RunningTemp\\display3.png")));
                            }
                            catch
                            {
                                Thread.Sleep(300);
                                ApplyDisplayCover(2);
                            }
                        });
                        recievedBytesDisplayCover = new List<byte>();
                        break;
                    case 3:
                        ApplyDisplayCover(3);
                        Dispatcher.Invoke(() =>
                        {
                            try
                            {
                                displayCoverImage3.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\RunningTemp\\display4.png")));
                            }
                            catch
                            {
                                Thread.Sleep(300);
                                ApplyDisplayCover(3);
                            }
                        });
                        recievedBytesDisplayCover = new List<byte>();
                        break;
                    default:
                        break;
                }
            });

            hubConnection.On<string, int>("RecieveDisplayName", (name, index) =>
            {
                switch (index)
                {
                    case 0:
                        Dispatcher.Invoke(() =>
                        {
                            displayNameTextBlock0.Text = name;
                            deleteTrackButton0.Visibility = Visibility.Visible;
                        });
                        break;
                    case 1:
                        Dispatcher.Invoke(() =>
                        {
                            displayNameTextBlock1.Text = name;
                            deleteTrackButton1.Visibility = Visibility.Visible;
                        });
                        break;
                    case 2:
                        Dispatcher.Invoke(() =>
                        {
                            displayNameTextBlock2.Text = name;
                            deleteTrackButton2.Visibility = Visibility.Visible;
                        });
                        break;
                    case 3:
                        Dispatcher.Invoke(() =>
                        {
                            displayNameTextBlock3.Text = name;
                            deleteTrackButton3.Visibility = Visibility.Visible;
                        });
                        break;
                    default:
                        break;
                }
            });

            hubConnection.On<string, int>("RecieveDisplayArtist", (artist, index) =>
            {
                switch (index)
                {
                    case 0:
                        Dispatcher.Invoke(() =>
                        {
                            displayArtistTextBlock0.Text = artist;
                        });
                        break;
                    case 1:
                        Dispatcher.Invoke(() =>
                        {
                            displayArtistTextBlock1.Text = artist;
                        });
                        break;
                    case 2:
                        Dispatcher.Invoke(() =>
                        {
                            displayArtistTextBlock2.Text = artist;
                        });
                        break;
                    case 3:
                        Dispatcher.Invoke(() =>
                        {
                            displayArtistTextBlock3.Text = artist;
                        });
                        break;
                    default:
                        break;
                }
            });

            hubConnection.On<DateTime, int>("RecieveDisplayDate", (date, index) =>
            {
                string minute = date.Minute < 10 ? "0" + date.Minute.ToString() : date.Minute.ToString();
                switch (index)
                {
                    case 0:
                        Dispatcher.Invoke(() =>
                        {       
                            displayDateTextBlock0.Text = date.Day + "." + date.Month + "." + date.Year + " " + date.Hour + ":" + minute;
                        });
                        break;
                    case 1:
                        Dispatcher.Invoke(() =>
                        {
                            displayDateTextBlock1.Text = date.Day + "." + date.Month + "." + date.Year + " " + date.Hour + ":" + minute;
                        });
                        break;
                    case 2:
                        Dispatcher.Invoke(() =>
                        {
                            displayDateTextBlock2.Text = date.Day + "." + date.Month + "." + date.Year + " " + date.Hour + ":" + minute;
                        });
                        break;
                    case 3:
                        Dispatcher.Invoke(() =>
                        {
                            displayDateTextBlock3.Text = date.Day + "." + date.Month + "." + date.Year + " " + date.Hour + ":" + minute;
                        });
                        break;
                    default:
                        break;
                }
            });

            hubConnection.On<TimeSpan, int>("RecieveDisplayDuration", (Duration, index) =>
            {
                string seconds = Duration.Seconds < 10 ? "0" + Duration.Seconds.ToString() : Duration.Seconds.ToString();
                switch (index)
                {          
                    case 0:
                        Dispatcher.Invoke(() =>
                        {
                            displayDurationTextBlock0.Text = Duration.Minutes.ToString() +":" + seconds; 
                        });
                        break;
                    case 1:
                        Dispatcher.Invoke(() =>
                        {
                            displayDurationTextBlock1.Text = Duration.Minutes.ToString() + ":" + seconds;
                        });
                        break;
                    case 2:
                        Dispatcher.Invoke(() =>
                        {
                            displayDurationTextBlock2.Text = Duration.Minutes.ToString() + ":" + seconds;
                        });
                        break;
                    case 3:
                        Dispatcher.Invoke(() =>
                        {
                            displayDurationTextBlock3.Text = Duration.Minutes.ToString() + ":" + seconds;
                        });
                        break;
                    default:
                        break;
                }
            });

            await GetDisplays();
            EndLoading();
        }
        private async Task ApplyDisplayCover(int displayNumber)
        {
            switch(displayNumber)
            {
                case 0: 
                    await System.IO.File.WriteAllBytesAsync(TempFolderPath + "\\display1.png", recievedBytesDisplayCover.ToArray());
                    break;
                case 1:
                    await System.IO.File.WriteAllBytesAsync(TempFolderPath + "\\display2.png", recievedBytesDisplayCover.ToArray());
                    break;
                case 2:
                    await System.IO.File.WriteAllBytesAsync(TempFolderPath + "\\display3.png", recievedBytesDisplayCover.ToArray());
                    break;
                case 3:
                    await System.IO.File.WriteAllBytesAsync(TempFolderPath + "\\display4.png", recievedBytesDisplayCover.ToArray());
                    break;
                default:
                    break;
            }
            
        }
        private void exitAppButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static List<List<byte>> SplitList(List<byte> source, int size)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / size)
                .Select(g => g.Select(v => v.Value).ToList())
                .ToList();
        }

        private void ClearPlayer()
        {
            timerPaused = true;
            isPlaying = false;
            mediaPlayer.Stop();
            mediaPlayer.Close();
            recievedBytes = new List<byte> { };
            playTrackImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\Resources\\UI\\playTrackIcon.png")));
        }
        private void playTrackButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isPlaying)
            {
                playTrackImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\Resources\\UI\\pauseTrackIcon.png")));
                if (timer == null)
                {
                    timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += Timer_Tick;
                }
                timer.Start();
                mediaPlayer.Play();
            }
            else
            {
                playTrackImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\Resources\\UI\\playTrackIcon.png")));
                mediaPlayer.Pause();
            }
            isPlaying = !isPlaying;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if(!timerPaused)
            {
                TimeSpan currentTime = mediaPlayer.Position;
                if (currentTime.TotalSeconds >= mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds)
                {
                    timer.Stop();
                    currentTime = TimeSpan.Zero;
                    playTrackImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\Resources\\UI\\playTrackIcon.png")));
                    isPlaying = false;
                    mediaPlayer.Stop();
                    mediaPlayer.Position = currentTime;
                }
                if (currentTime.Seconds < 10)
                {
                    trackCurrentLengthTextBlock.Text = currentTime.Minutes.ToString() + ":0" + currentTime.Seconds.ToString();
                }
                else
                {
                    trackCurrentLengthTextBlock.Text = currentTime.Minutes.ToString() + ":" + currentTime.Seconds.ToString();
                }
                trackPositionSlider.Value = currentTime.TotalSeconds;
            }
            
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void displayCoverImage0_MouseUp(object sender, MouseButtonEventArgs e)
        {
            StartLoadingPlayer();
            ClearPlayer();
            hubConnection.InvokeAsync("GetAudioBytes", displayNameTextBlock0.Text);
            trackRecievedCover.Source = displayCoverImage0.Source;
            trackRecievedName.Text = displayNameTextBlock0.Text;
            trackRecievedArtist.Text = displayArtistTextBlock0.Text;
            
        }

        private void displayCoverImage1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            StartLoadingPlayer();
            ClearPlayer();
            hubConnection.InvokeAsync("GetAudioBytes", displayNameTextBlock1.Text);
            trackRecievedCover.Source = displayCoverImage1.Source;
            trackRecievedName.Text = displayNameTextBlock1.Text;
            trackRecievedArtist.Text = displayArtistTextBlock1.Text;
            
        }

        private void displayCoverImage2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            StartLoadingPlayer();
            ClearPlayer();
            hubConnection.InvokeAsync("GetAudioBytes", displayNameTextBlock2.Text);
            trackRecievedCover.Source = displayCoverImage2.Source;
            trackRecievedName.Text = displayNameTextBlock2.Text;
            trackRecievedArtist.Text = displayArtistTextBlock2.Text;
        }

        private void displayCoverImage3_MouseUp(object sender, MouseButtonEventArgs e)
        {
            StartLoadingPlayer();
            ClearPlayer();
            hubConnection.InvokeAsync("GetAudioBytes", displayNameTextBlock3.Text);
            trackRecievedCover.Source = displayCoverImage3.Source;
            trackRecievedName.Text = displayNameTextBlock3.Text;
            trackRecievedArtist.Text = displayArtistTextBlock3.Text;
        }

        private void trackVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Volume = trackVolumeSlider.Value / 150.0;
            }
        }

        private void trackPositionSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            timerPaused = true;
        }
        private void trackPositionSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(trackPositionSlider.Value);
            timerPaused = false;
        }

        private void ClearDisplay()
        {
            displayCoverImage0.Source = null;
            displayCoverImage1.Source = null;
            displayCoverImage2.Source = null;
            displayCoverImage3.Source = null;
            displayNameTextBlock0.Text = string.Empty;
            displayNameTextBlock1.Text = string.Empty;
            displayNameTextBlock2.Text = string.Empty;
            displayNameTextBlock3.Text = string.Empty;
            displayArtistTextBlock0.Text = string.Empty;
            displayArtistTextBlock1.Text = string.Empty;
            displayArtistTextBlock2.Text = string.Empty;
            displayArtistTextBlock3.Text = string.Empty;
            displayDateTextBlock0.Text = string.Empty;
            displayDateTextBlock1.Text = string.Empty;
            displayDateTextBlock2.Text = string.Empty;
            displayDateTextBlock3.Text = string.Empty;
            displayDurationTextBlock0.Text = string.Empty;
            displayDurationTextBlock1.Text = string.Empty;
            displayDurationTextBlock2.Text = string.Empty;
            displayDurationTextBlock3.Text = string.Empty;
            deleteTrackButton0.Visibility = Visibility.Collapsed;
            deleteTrackButton1.Visibility = Visibility.Collapsed;
            deleteTrackButton2.Visibility = Visibility.Collapsed;
            deleteTrackButton3.Visibility = Visibility.Collapsed;
            tempBytes = new byte[] { };
            recievedBytes = new List<byte>();
            recievedBytesDisplayCover = new List<byte>();
            tempBytesCover = new byte[] { };
        }

        private async Task GetDisplays()
        {
            await hubConnection.InvokeAsync("GetCoverForDisplay", displayStartIndex);
            await hubConnection.InvokeAsync("GetNamesForDisplay", displayStartIndex);
            await hubConnection.InvokeAsync("GetArtistsForDisplay", displayStartIndex);
            await hubConnection.InvokeAsync("GetDatesForDisplay", displayStartIndex);
            await hubConnection.InvokeAsync("GetDurationForDisplay", displayStartIndex);
        }
        private async void moveDisplayDownButton_Click(object sender, RoutedEventArgs e)
        {
            StartLoading();
            ClearDisplay();
            displayStartIndex += 4;
            track0NumberTextBlock.Text = (1 + displayStartIndex).ToString();
            track1NumberTextBlock.Text = (2 + displayStartIndex).ToString();
            track2NumberTextBlock.Text = (3 + displayStartIndex).ToString();
            track3NumberTextBlock.Text = (4 + displayStartIndex).ToString();
            await GetDisplays();
            EndLoading();
        }

        

        private async void moveDisplayUpButton_Click(object sender, RoutedEventArgs e)
        {
            if(displayStartIndex!=0)
            {
                StartLoading();
                ClearDisplay();
                displayStartIndex -= 4;
                track0NumberTextBlock.Text = (1 + displayStartIndex).ToString();
                track1NumberTextBlock.Text = (2 + displayStartIndex).ToString();
                track2NumberTextBlock.Text = (3 + displayStartIndex).ToString();
                track3NumberTextBlock.Text = (4 + displayStartIndex).ToString();
                await GetDisplays();
                EndLoading();
            }
        }

        private void StartLoading()
        {
            LoadingScreen.Visibility = Visibility.Visible;
        }

        private void EndLoading()
        {
            LoadingScreen.Visibility = Visibility.Collapsed;
        }

        private void StartLoadingPlayer()
        {
            LoadingScreenPlayer.Visibility = Visibility.Visible;
            playTrackButton.Visibility = Visibility.Collapsed;
        }

        private void EndLoadingPlayer()
        {
            LoadingScreenPlayer.Visibility=Visibility.Collapsed;
            playTrackButton.Visibility = Visibility.Visible;
        }

        private async void deleteTrackButton0_Click_1(object sender, RoutedEventArgs e)
        {
            StartLoading();
            await hubConnection.InvokeAsync("DeleteTrack", displayNameTextBlock0.Text);
            ClearDisplay();
            await GetDisplays();
            EndLoading();
        }

        private async void deleteTrackButton1_Click(object sender, RoutedEventArgs e)
        {
            StartLoading();
            await hubConnection.InvokeAsync("DeleteTrack", displayNameTextBlock1.Text);
            ClearDisplay();
            await GetDisplays();
            EndLoading();
        }

        private async void deleteTrackButton2_Click(object sender, RoutedEventArgs e)
        {
            StartLoading();
            await hubConnection.InvokeAsync("DeleteTrack", displayNameTextBlock2.Text);
            ClearDisplay();
            await GetDisplays();
            EndLoading();
        }

        private async void deleteTrackButton3_Click(object sender, RoutedEventArgs e)
        {
            StartLoading();
            await hubConnection.InvokeAsync("DeleteTrack", displayNameTextBlock3.Text);
            ClearDisplay();
            await GetDisplays();
            EndLoading();
        }

        private void addTrackButton_Click(object sender, RoutedEventArgs e)
        {
            addTrackWindow = new AddTrackWindow(this);
            addTrackWindow.Show();
        }

        public async void addTrack(string name, string artist, string audioPath, byte[] coverBytes)
        {
            StartLoading();
            tempBytes = File.ReadAllBytes(audioPath);
            tempBytesCover = coverBytes;
            List<List<byte>> listsList = SplitList(tempBytes.ToList(), 1000000);
            foreach (List<byte> list in listsList)
            {
                await hubConnection.InvokeAsync("SendAudioBytes", list, name, artist);
            }

            List<List<byte>> listsListCover = SplitList(tempBytesCover.ToList(), 1000000);
            foreach (List<byte> list2 in listsListCover)
            {
                await hubConnection.InvokeAsync("SendCoverBytes", list2, name, artist);
            }

            ClearDisplay();
            await GetDisplays();
            EndLoading();
        }

        private void minimizeAppButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void maximizeeAppButton_Click(object sender, RoutedEventArgs e)
        {
            if(WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                maximizeImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\Resources\\UI\\maximizeIcon2.png")));
            }
            else
            {
                WindowState = WindowState.Normal;
                maximizeImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\Resources\\UI\\maximizeIcon.png")));
            }
        }
    }
}