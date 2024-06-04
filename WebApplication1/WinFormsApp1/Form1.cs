using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        HubConnection hubConnection;
        public string message = "Hello!";
        public Form1()
        {
            InitializeComponent();
            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7196/chat").Build();
            hubConnection.Closed += HubConnetction_Closed;
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
    }
}
