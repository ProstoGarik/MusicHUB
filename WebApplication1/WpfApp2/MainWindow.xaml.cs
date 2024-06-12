using System.IO;
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
        public List<byte> recievedBytes;
        public MainWindow()
        {
            InitializeComponent();
            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7196/chat").Build();
            hubConnection.Closed += HubConnetction_Closed;
            Loaded += MainWindow_Loaded;
            tempBytes = new byte[] { };
            recievedBytes = new List<byte>();
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
            try
            {
                await hubConnection.StartAsync();

            }
            catch (Exception ex)
            {
                listBox1.Items.Add(ex.Message);
            }
        }
        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            hubConnection.InvokeAsync("SendMessage", messageTextBox.Text);
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
                filePathTextBlock.Text = dialog.FileName;
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

            sendFileStateTextBlock.Text = "Готово";
        }

        private void getFileButton_Click(object sender, RoutedEventArgs e)
        {
            getFileStateTextBlock.Text = "Получаем...";
            System.IO.File.WriteAllBytes("Z:\\TempFolder\\mymusic.wav", recievedBytes.ToArray());
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
    }
}