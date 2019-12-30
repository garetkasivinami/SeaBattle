using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public class ConnectClient
    {
        const int port = 8888;
        const string address = "127.0.0.1";
        TcpClient client;
        NetworkStream stream;
        byte[] data;
        public ConnectClient(out int id)
        {
            client = new TcpClient(address, port);
            stream = client.GetStream();
            SendMessage("ID");
            string res = GetMessage();
            id = int.Parse(res);
        }
        public void SendMessage(string message)
        {
            data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
        public string GetMessage()
        {
            data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);
            return builder.ToString();
        }
        public string WaitData()
        {
            string res;
            do
            {
                SendMessage("WA");
                res = GetMessage();
                Console.WriteLine(res);
            } while (res == "check");
            return res;
        }
        public string GetWaiters()
        {
            SendMessage("GW");
            return GetMessage();
        }
        public bool CreateConnection(int id, out int gId)
        {
            SendMessage($"CI {id}");
            string res = GetMessage();
            string[] wants = res.Split(' ');
            if (wants[0] == "exce")
            {
                gId = int.Parse(wants[1]);
                return true;
            } else {
                gId = 0;
                return false;
            }
        }
        public int GetGameId(int secondId)
        {
            SendMessage($"GI {secondId}");
            string res = GetMessage();
            return int.Parse(res);
        }
        public bool GetState()
        {
            SendMessage("GS");
            string res = GetMessage();
            if (res == "ok")
            {
                return true;
            } else
            {
                return false;
            }
        }
        public bool SendArea(Ship[] ships)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SE");
            for (int i = 0; i < 10; i++)
            {
                builder.Append($" {ships[i].X}{ships[i].Y}{ships[i].Rotation}{ships[i].State.Length}");
            }
            SendMessage(builder.ToString());
            string res = GetMessage();
            if (res == "ok")
            {
                return true;
            } else
            {
                return false;
            }
        }
        public void WaitStartGame()
        {
            SendMessage("CH");
            string res = GetMessage();
            if (res == "no")
            {
                WaitData();
            }
        }
        public void GetData(VirtualArea first, VirtualArea second)
        {
            SendMessage("GD");
            string res = GetMessage();
            string[] wants = res.Split(' ');
            first.Update(wants[0]);
            second.Update(wants[1]);
        }
        public bool Fire(int x, int y)
        {
            SendMessage($"FI {x} {y}");
            if (GetMessage() == "ok")
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
