using System.IO;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        public List<byte> recievedBytesCover;
        public List<byte> recievedBytesDisplayCover;
        public MediaPlayer mediaPlayer;
        public bool isPlaying;
        int displayStartIndex = 0;
        private string TempFolderPath = "Z:\\TempFolder";
        public MainWindow()
        {
            InitializeComponent();

            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7196/chat").Build();
            hubConnection.Closed += HubConnetction_Closed;
            Loaded += MainWindow_Loaded;
            tempBytes = new byte[] { };
            tempBytesCover = new byte[] { };
            recievedBytes = new List<byte>();
            recievedBytesCover = new List<byte>();
            recievedBytesDisplayCover = new List<byte>();
            mediaPlayer = new MediaPlayer();
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

            hubConnection.On<List<byte>>("RecieveCoverBytes", bytes =>
            {
                recievedBytesCover.AddRange(bytes);
            });
            try
            {
                await hubConnection.StartAsync();

            }
            catch (Exception ex)
            {
                //listBox1.Items.Add(ex.Message);
            }

            hubConnection.On<int>("RecievingAudioDone", byteCount =>
            {
                if (byteCount == recievedBytes.Count)
                {
                    System.IO.File.WriteAllBytesAsync(TempFolderPath + "\\mymusic.wav", recievedBytes.ToArray());
                    Dispatcher.Invoke(() =>
                    {
                        mediaPlayer.Open(new Uri(TempFolderPath + "\\mymusic.wav"));
                        trackRecievedName.Text = trackName.Text;
                    });     
                }
            });

            hubConnection.On<int>("RecievingCoverDone", byteCount =>
            {
                if (byteCount == recievedBytesCover.Count)
                {
                    System.IO.File.WriteAllBytesAsync(TempFolderPath + "\\mycover.png", recievedBytesCover.ToArray());
                    Dispatcher.Invoke(() =>
                    {
                        trackRecievedCover.Source = new BitmapImage(new Uri(TempFolderPath + "\\mycover.png"));
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
                            System.IO.File.WriteAllBytesAsync(TempFolderPath + "\\display1.png", recievedBytesDisplayCover.ToArray());
                            try
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    displayCoverImage0.Source = new BitmapImage(new Uri(TempFolderPath + "\\display1.png"));
                                });
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            break;
                        case 1:
                            System.IO.File.WriteAllBytesAsync(TempFolderPath + "\\display2.png", recievedBytesDisplayCover.ToArray());
                            try
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    displayCoverImage1.Source = new BitmapImage(new Uri(TempFolderPath + "\\display2.png"));
                                });
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            
                            break;
                        case 2:
                            System.IO.File.WriteAllBytesAsync(TempFolderPath + "\\display3.png", recievedBytesDisplayCover.ToArray());
                            try
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    displayCoverImage2.Source = new BitmapImage(new Uri(TempFolderPath + "\\display3.png"));
                                });
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            
                            break;
                        case 3:
                            System.IO.File.WriteAllBytesAsync(TempFolderPath + "\\display4.png", recievedBytesDisplayCover.ToArray());
                            try
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    displayCoverImage3.Source = new BitmapImage(new Uri(TempFolderPath + "\\display4.png"));
                                });
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

            await hubConnection.InvokeAsync("GetCoverForDisplay", displayStartIndex);
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
                hubConnection.InvokeAsync("SendAudioBytes", list, trackName.Text);
            }

            List<List<byte>> listsListCover = SplitList(tempBytesCover.ToList(), 1000000);
            foreach (List<byte> list2 in listsListCover)
            {
                hubConnection.InvokeAsync("SendCoverBytes", list2, trackName.Text);
            }

            hubConnection.InvokeAsync("CreateTrack", trackName.Text);
        }

        private void getFileButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            mediaPlayer.Close();
            recievedBytes = new List<byte> { };
            recievedBytesCover = new List<byte> { };
            

            hubConnection.InvokeAsync("GetAudioBytes", trackName.Text);
            hubConnection.InvokeAsync("GetCoverBytes", trackName.Text);
            

        }

        private void showFileButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public static List<List<byte>> SplitList(List<byte> source, int size)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / size)
                .Select(g => g.Select(v => v.Value).ToList())
                .ToList();
        }

        private void playTrackButton_Click(object sender, RoutedEventArgs e)
        {
            if(!isPlaying)
            {
                mediaPlayer.Play();
            }
            else
            {
                mediaPlayer.Stop();
            }
            isPlaying = !isPlaying;
        }

    }
}