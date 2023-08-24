using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDPServerFormApp
{
    public partial class Form1 : Form
    {
        UdpClient server;
        IPEndPoint endPoint;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            server = new UdpClient(int.Parse(txtServerPort.Text));
            endPoint = new IPEndPoint(IPAddress.Any, 0);

            Thread thread = new Thread(new ThreadStart(ServerStart));
            thread.Start();
            btnStart.Enabled = false;

            WriteLog("Server Başlatıldı");
            WriteLog("-----------------------------------------------");
        }

        private void ServerStart()
        {
            while (true) 
            {
                byte[] buffer = server.Receive(ref endPoint);

                string[] msg = Encoding.UTF8.GetString(buffer).Split('/');

                int clientPort = int.Parse(msg[0]);
                string clientHostName = msg[1];
                string request = msg[2];

                WriteLog($"{clientPort} numaralı istemci portu, {clientHostName}-> Servera ->\n {request}");

                string response = $"{clientPort} portundan veri iletildi.";

                buffer = Encoding.UTF8.GetBytes(response);
                server.Send(buffer,buffer.Length,clientHostName,clientPort);

                WriteLog($"Bilgi: {clientPort} Serverdan {clientHostName} numaralı istemciye cevap gönderildi.");
                WriteLog("---------------------------------------------------------------------------------");

            }
        }

        private void WriteLog(string msg)
        {
            MethodInvoker invoker = new MethodInvoker(delegate
            {
                txtLog.Text += $"{msg}.{Environment.NewLine}";
            });
            this.BeginInvoke(invoker);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            
        }
    }
}
