using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows.Forms;
using System.IO;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        HubConnection hubConnection;
        public string message = "Hello!";
        public byte[] fileBytes;
        public byte[] recievedBytes;

        public Form1()
        {
            InitializeComponent();
            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7196/chat").Build();
            hubConnection.Closed += HubConnetction_Closed;
            fileBytes = new byte[] { };
        }

        public async Task HubConnetction_Closed(Exception? arg)
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await hubConnection.StartAsync();
        }
        private void UpdateListBox(string message)
        {
            if (listBox1.InvokeRequired)
            {
                // Если вызов осуществляется из другого потока, используйте метод Invoke
                listBox1.Invoke(new Action<string>(UpdateListBox), message);
            }
            else
            {
                // Добавьте сообщение в listBox1
                listBox1.Items.Add(message);
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            hubConnection.On<string>("RecieveMessage", message => { UpdateListBox(message); });

            hubConnection.On<byte[]>("RecieveBytes", (bytes) =>
            {
                recievedBytes = bytes;
            if (recievedBytes == null)
                {
                    FileRecieved2.Text = "True";
                }
            else
                {
                    FileRecieved2.Text = "False";
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
                openFileDialog.Filter = "mp3 файлы (*.mp3)|*.mp3|Все файлы (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    FilePath2.Text = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    fileBytes = File.ReadAllBytes(openFileDialog.FileName);

                }
            }
        }

        private void SendFileButton_Click(object sender, EventArgs e)
        {
            hubConnection.InvokeAsync("SendBytes", fileBytes);
        }

        private void PlayReciewved_Click(object sender, EventArgs e)
        {
            File.WriteAllBytes("somefile.mp3", recievedBytes);
        }
    }
}
