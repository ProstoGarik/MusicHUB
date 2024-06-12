using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Linq;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        HubConnection hubConnection;
        public string message = "Hello!";
        public byte[] tempBytes;
        public List<byte> recievedBytes;

        public Form1()
        {
            InitializeComponent();
            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7196/chat").Build();
            hubConnection.Closed += HubConnetction_Closed;
            tempBytes = new byte[] { };
            recievedBytes = new List<byte> { };
        }

        public async Task HubConnetction_Closed(Exception? arg)
        {
            listBox1.Items.Add("Соединение разорвано, переподключение...");
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await hubConnection.StartAsync();
        }
        private void UpdateListBox(string message)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new Action<string>(UpdateListBox), message);
            }
            else
            {
                listBox1.Items.Add(message);
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            hubConnection.On<string>("RecieveMessage", message => { UpdateListBox(message); });

            hubConnection.On<List<byte>>("RecieveBytes", bytes => {
                recievedBytes.AddRange(bytes);
                if (listBox1.InvokeRequired)
                {
                    listBox1.Invoke(new Action<string>(UpdateListBox), "Получено " + bytes.Count() + " байт. Всего " + recievedBytes.Count() + " байт");
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

        private void button1_Click(object sender, EventArgs e)
        {
            hubConnection.InvokeAsync("SendMessage", textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "wav файлы (*.wav)|*.wav|Все файлы (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;


                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    tempBytes = File.ReadAllBytes(openFileDialog.FileName);

                    FilePath2.Text = openFileDialog.FileName;

                }
            }
        }

        private void SendFileButton_Click(object sender, EventArgs e)
        {
            LabelSend2.Text = "Отправляется...";
            List<List<byte>> listsList = SplitList(tempBytes.ToList(), 5000);
            foreach(List<byte> list in listsList)
            {
                hubConnection.InvokeAsync("SendBytes", list);
            }
            
            LabelSend2.Text = "Готово";
        }

        private void PlayReciewved_Click(object sender, EventArgs e)
        {
            FileRecieved2.Text = "Получаем...";
            System.IO.File.WriteAllBytes("Z:\\TempFolder\\mymusic.wav", recievedBytes.ToArray());
            FileRecieved2.Text = "Получено";
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
