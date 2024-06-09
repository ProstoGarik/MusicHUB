using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows.Forms;
using System.IO;
using NAudio.Wave;
using System.Collections;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        HubConnection hubConnection;
        public string message = "Hello!";
        public string recievedName;
        public string sendedName;
        public string filePath;
        public byte[] tempBytes;
        public byte[] sendedBytes;
        public byte[] recievedBytes;

        public Form1()
        {
            InitializeComponent();
            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7196/chat").Build();
            hubConnection.Closed += HubConnetction_Closed;
            tempBytes = new byte[] { };
            sendedBytes = new byte[] { };
            recievedBytes = new byte[] { };
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
            this.DoubleBuffered = true;
            hubConnection.On<string>("RecieveMessage", message => { UpdateListBox(message); });

            hubConnection.On<List<byte>>("RecieveBytes", bytes => {
                //if (listBox1.InvokeRequired)
                //{
                //    // Если вызов осуществляется из другого потока, используйте метод Invoke
                //    listBox1.Invoke(new Action<string>(UpdateListBox), "В bytes " + bytes.Length + " байт\n " +
                //        "В recievedBytes " + recievedBytes.Length + " байт");
                //}
                //else
                //{
                //    // Добавьте сообщение в listBox1
                //    listBox1.Items.Add("В bytes " + bytes.Length + " байт\n" +
                //        "В recievedBytes " + recievedBytes.Length + " байт");
                //}

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

                    //Get the path of specified file
                    FilePath2.Text = openFileDialog.FileName;

                    //Read the contents of the file into a stream

                }
            }
        }

        private void SendFileButton_Click(object sender, EventArgs e)
        {
            LabelSend2.Text = "Отправляется...";
            List<byte> tempByteChunk = new List<byte>(5000);
            for (int i = 0; i < tempBytes.Length; i++)
            {
                tempByteChunk.Add(tempBytes[i]);
                hubConnection.InvokeAsync("SendBytes", tempByteChunk.ToArray());
            }
            
            LabelSend2.Text = "Готово";
        }

        private void PlayReciewved_Click(object sender, EventArgs e)
        {
            FileRecieved2.Text = "Получаем...";
            System.IO.File.WriteAllBytes("F:\\TempFiles\\mymusic.wav", recievedBytes.ToArray());
            FileRecieved2.Text = "Получено";
        }
    }
}
