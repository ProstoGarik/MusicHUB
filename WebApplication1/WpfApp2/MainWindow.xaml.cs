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

namespace WpfApp2
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
        public MainWindow()
        {
            InitializeComponent();

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
            trackFullLengthTextBlock.Text = mediaPlayer.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + mediaPlayer.NaturalDuration.TimeSpan.Seconds.ToString();
            trackPositionSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
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
            catch (Exception ex)
            {
                //listBox1.Items.Add(ex.Message);
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
                if (count == recievedBytesDisplayCover.Count)
                {
                    switch (index)
                    {
                        case 0:
                            ApplyDisplayCover(0);
                            try
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    displayCoverImage0.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\RunningTemp\\display1.png")));
                                });
                                recievedBytesDisplayCover = new List<byte>();
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                            break;
                        case 1:
                            ApplyDisplayCover(1);
                            try
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    displayCoverImage1.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\RunningTemp\\display2.png")));
                                });
                                recievedBytesDisplayCover = new List<byte>();
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                            
                            break;
                        case 2:
                            ApplyDisplayCover(2);
                            try
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    displayCoverImage2.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\RunningTemp\\display3.png")));
                                });
                                recievedBytesDisplayCover = new List<byte>();
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            
                            break;
                        case 3:
                            ApplyDisplayCover(3);
                            try
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    displayCoverImage3.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\..\\RunningTemp\\display4.png")));
                                });
                                recievedBytesDisplayCover = new List<byte>();
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            
                            break;
                        default:
                            break;
                    }
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
                        });
                        break;
                    case 1:
                        Dispatcher.Invoke(() =>
                        {
                            displayNameTextBlock1.Text = name;
                        });
                        break;
                    case 2:
                        Dispatcher.Invoke(() =>
                        {
                            displayNameTextBlock2.Text = name;
                        });
                        break;
                    case 3:
                        Dispatcher.Invoke(() =>
                        {
                            displayNameTextBlock3.Text = name;
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
                            displayDateTextBlock0.Text = date.Day + "." + date.Month + "." + date.Year + " " + date.Hour + ":" + minute;
                        });
                        break;
                    case 2:
                        Dispatcher.Invoke(() =>
                        {
                            displayDateTextBlock0.Text = date.Day + "." + date.Month + "." + date.Year + " " + date.Hour + ":" + minute;
                        });
                        break;
                    case 3:
                        Dispatcher.Invoke(() =>
                        {
                            displayDateTextBlock0.Text = date.Day + "." + date.Month + "." + date.Year + " " + date.Hour + ":" + minute;
                        });
                        break;
                    default:
                        break;
                }
            });

            await hubConnection.InvokeAsync("GetCoverForDisplay", displayStartIndex);
            await hubConnection.InvokeAsync("GetNamesForDisplay", displayStartIndex);
            await hubConnection.InvokeAsync("GetArtistsForDisplay", displayStartIndex);
            await hubConnection.InvokeAsync("GetDatesForDisplay", displayStartIndex);
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

        private void chooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? success = dialog.ShowDialog();
            if(success == true)
            {
                tempBytes = File.ReadAllBytes(dialog.FileName);
            }
        }

        private void chooseFileCoverButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? success = dialog.ShowDialog();
            if (success == true)
            {
                tempBytesCover = File.ReadAllBytes(dialog.FileName);
            }
        }

        private void sendFileButton_Click(object sender, RoutedEventArgs e)
        {
            List<List<byte>> listsList = SplitList(tempBytes.ToList(), 1000000);
            foreach (List<byte> list in listsList)
            {
                hubConnection.InvokeAsync("SendAudioBytes", list, trackName.Text, trackArtist.Text);
            }

            List<List<byte>> listsListCover = SplitList(tempBytesCover.ToList(), 1000000);
            foreach (List<byte> list2 in listsListCover)
            {
                hubConnection.InvokeAsync("SendCoverBytes", list2, trackName.Text, trackArtist.Text);
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
            mediaPlayer.Stop();
            mediaPlayer.Close();
            recievedBytes = new List<byte> { };
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
                if (currentTime.TotalSeconds <= 0)
                {
                    timer.Stop();
                    currentTime = TimeSpan.Zero;
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
            ClearPlayer();
            hubConnection.InvokeAsync("GetAudioBytes", displayNameTextBlock0.Text);
            trackRecievedCover.Source = displayCoverImage0.Source;
            trackRecievedName.Text = displayNameTextBlock0.Text;
            trackRecievedArtist.Text = displayArtistTextBlock0.Text;
            
        }

        private void displayCoverImage1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClearPlayer();
            hubConnection.InvokeAsync("GetAudioBytes", displayNameTextBlock1.Text);
            trackRecievedCover.Source = displayCoverImage1.Source;
            trackRecievedName.Text = displayNameTextBlock1.Text;
            trackRecievedArtist.Text = displayArtistTextBlock1.Text;
        }

        private void displayCoverImage2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClearPlayer();
            hubConnection.InvokeAsync("GetAudioBytes", displayNameTextBlock2.Text);
            trackRecievedCover.Source = displayCoverImage2.Source;
            trackRecievedName.Text = displayNameTextBlock2.Text;
            trackRecievedArtist.Text = displayArtistTextBlock2.Text;
        }

        private void displayCoverImage3_MouseUp(object sender, MouseButtonEventArgs e)
        {
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

        
    }
}