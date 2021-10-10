using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


namespace WindowsFormsApp1
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }

        private void btnsend_Click(object sender, EventArgs e)
        {
            Send();
            AddMessage(Messagebox.Text);
        }
        
        IPEndPoint IP;
        Socket client;
        
        void Connect()
        {
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                client.Connect(IP);
            } catch { MessageBox.Show("Hk thể kết nối", "Lỗi r hmu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Thread ngờenghe = new Thread(Receive);
            ngờenghe.IsBackground = true;
            ngờenghe.Start();

        }
        
        void close()
        {
            client.Close();
        }
        
        void Send()
        {
            if (Messagebox.Text != string.Empty)
                client.Send(Serialize(Messagebox.Text));
        
        }
        
        void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);

                    string message = (string)Deserialize(data);

                    AddMessage(message);
                }
            } catch { close(); }
        }

        void AddMessage(string hmu)
        {
            listView1.Items.Add(new ListViewItem() { Text = hmu });
            Messagebox.Clear();
        }
        
        byte[] Serialize(object obj)
        {
            MemoryStream strim = new MemoryStream();
            BinaryFormatter phomat = new BinaryFormatter();

            phomat.Serialize(strim, obj);
            return strim.ToArray();

        }
        
        object Deserialize(byte[] data)
        {
            MemoryStream strim = new MemoryStream(data);
            BinaryFormatter phomat = new BinaryFormatter();

            return phomat.Deserialize(strim);
        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            close();
        }
    }
}
