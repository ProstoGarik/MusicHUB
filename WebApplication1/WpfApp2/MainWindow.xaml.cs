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
        public string recievedTrackName;
        public MediaPlayer mediaPlayer;
        public bool isPlaying;
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
            mediaPlayer = new MediaPlayer();
        }
        public async Task HubConnetction_Closed(Exception? arg)
        {
            listBox1.Items.Add("Соединение разорвано, переподключение...");
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await hubConnection.StartAsync();
        }
        private void UpdateListBox(string message)
        {
            if (!listBox1.Dispatcher.CheckAccess())
            {
                listBox1.Dispatcher.Invoke(new Action<string>(UpdateListBox), message);
            }
            else
            {
                listBox1.Items.Add(message);
            }
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            hubConnection.On<string>("RecieveMessage", message => { UpdateListBox(message); });

            hubConnection.On<List<byte>>("RecieveBytes", bytes => {
                recievedBytes.AddRange(bytes);
                if (!listBox1.Dispatcher.CheckAccess())
                {
                    listBox1.Dispatcher.Invoke(new Action<string>(UpdateListBox), "Получено " + bytes.Count() + " байт. Всего " + recievedBytes.Count() + " байт");
                }
                else
                {
                    listBox1.Items.Add("Получено " + bytes.Count() + " байт. Всего " + recievedBytes.Count() + " байт");
                }
            });

            hubConnection.On<List<byte>>("RecieveBytesCover", bytes =>
            {
                recievedBytesCover.AddRange(bytes);

            });
            try
            {
                await hubConnection.StartAsync();

            }
            catch (Exception ex)
            {
                listBox1.Items.Add(ex.Message);
            }

            hubConnection.On<string>("RecieveName", name =>
            {
                recievedTrackName = name;
            });
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
            sendFileStateTextBlock.Text = "Отправляется...";
            List<List<byte>> listsList = SplitList(tempBytes.ToList(), 5000);
            foreach (List<byte> list in listsList)
            {
                hubConnection.InvokeAsync("SendBytes", list);
            }

            List<List<byte>> listsListCover = SplitList(tempBytesCover.ToList(), 5000);
            foreach (List<byte> list2 in listsListCover)
            {
                hubConnection.InvokeAsync("SendBytesCover", list2);
            }

            hubConnection.InvokeAsync("SendName", trackName.Text);

            sendFileStateTextBlock.Text = "Готово";
        }

        private void getFileButton_Click(object sender, RoutedEventArgs e)
        {
            getFileStateTextBlock.Text = "Получаем...";
            System.IO.File.WriteAllBytes("F:\\TempFiles\\mymusic.wav", recievedBytes.ToArray());
            System.IO.File.WriteAllBytes("F:\\TempFiles\\mycover.png", recievedBytesCover.ToArray());
            trackRecievedCover.Source = new BitmapImage(new Uri("F:\\TempFiles\\mycover.png"));
            trackRecievedName.Text = recievedTrackName;
            mediaPlayer.Open(new Uri("F:\\TempFiles\\mymusic.wav"));
            getFileStateTextBlock.Text = "Получено";
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