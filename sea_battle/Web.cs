using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace sea_battle
{
    public static class Web
    {
        const int port = 8888;
        const string address = "127.0.0.1";
        public static List<ClientObject> ClientObjects = new List<ClientObject>();
        public static List<TwinArea> twinAreas = new List<TwinArea>();
        public static object LockObj = new object();
        public static void AdminWork()
        {
            Thread thread = new Thread(new ThreadStart(Work));
            thread.Start();
            while(true)
            {
                Thread.Sleep(10000); // test
                lock(LockObj)
                {
                    for (int i = 0; i < ClientObjects.Count; i++)
                    {
                        if (!ClientObjects[i].IsConnect)
                        {
                            ClientObjects.RemoveAt(i);
                            i--;
                        } else if (ClientObjects[i].IsWait)
                        {
                            ClientObjects[i].SendToWait("check");
                        }
                    }
                }
            }
        }
        private static void Work()
        {
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse(address);
                server = new TcpListener(localAddr, port);
                server.Start();
                Console.WriteLine("Ожидание подключений... ");
                while (true) {
                    TcpClient client = server.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);
                    lock (LockObj)
                    {
                        ClientObjects.Add(clientObject);
                    }
                    // создаем новый поток для обслуживания нового клиента
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    public class ClientObject
    {
        private ManualResetEvent Waiting = new ManualResetEvent(true);
        static int indexer = 0;
        public int Id;
        public TcpClient client;
        NetworkStream stream = null;
        public bool IsWait;
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
            Id = indexer;
            indexer++;
        }
        private string message;
        private string extraMessage = null;
        private TwinArea CurrentArea = null;
        public bool IsConnect = true;
        public int MyIndex;
        public void Process()
        {
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[64]; // буфер для получаемых данных
                bool resB;
                int index;
                while (true)
                {
                    IsWait = false;
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    message = builder.ToString();
                    Console.WriteLine(message + $" $$${Id}$$$");
                    string[] wants = message.Split(' ');
                    switch (wants[0]) {
                        case "ID":
                            message = Id.ToString();
                            break;
                        case "WA":
                            if (extraMessage == null)
                            {
                                IsWait = true;
                                Waiting.Reset();
                                Waiting.WaitOne();
                            }
                            message = extraMessage;
                            extraMessage = null;
                            break;
                        case "GW":
                            message = GetWaiters();
                            break;
                        case "CI":
                            if (ConnectId(int.Parse(wants[1])))
                            {
                                CurrentArea = new TwinArea();
                                MyIndex = 0;
                                CurrentArea.clientObjects[0] = this;
                                Web.twinAreas.Add(CurrentArea);
                                message = $"exce {CurrentArea.Id}";
                            } else
                            {
                                message = "bad";
                            }
                            break;
                        case "GI":
                            ClientObject clientObject = FindByID(int.Parse(wants[1]));
                            if (clientObject != null)
                            {
                                CurrentArea = clientObject.CurrentArea;
                                CurrentArea.clientObjects[1] = this;
                                MyIndex = 1;
                                message = clientObject.CurrentArea.Id.ToString();
                            } else
                            {
                                message = "error";
                            }
                            break;
                        case "GS":
                            if (CurrentArea.clientObjects[CurrentArea.activeIndex] == this)
                            {
                                message = "ok";
                            } else
                            {
                                message = "no";
                            }
                            if (message == "ok")
                            {
                                Console.WriteLine("LOL");
                            }
                            break;
                        case "SE":
                            message = SetData(wants);
                            break;
                        case "CH":
                            Console.WriteLine(CurrentArea.IsStart);
                            lock (Web.LockObj)
                            {
                                if (!CurrentArea.IsStart)
                                {
                                    CurrentArea.IsStart = true;
                                    message = "no";
                                }
                                else
                                {
                                    if (MyIndex == 0)
                                    {
                                        CurrentArea.clientObjects[1].SendToWait("ok");
                                    }
                                    else
                                    {
                                        CurrentArea.clientObjects[0].SendToWait("ok");
                                    }
                                    message = "ok";
                                }
                            }
                            if (message == "ok")
                            {
                                Console.WriteLine("LOL");
                            }
                            break;
                        case "GD":
                            message = CurrentArea.Areas[MyIndex].ToString();
                            index = (MyIndex == 1)? 0: 1;
                            message = message + " " + CurrentArea.Areas[index].ToString();
                            break;
                        case "FI":

                            index = (MyIndex == 0)?1 : 0;
                            bytes = CurrentArea.Areas[index].Fire(int.Parse(wants[1]), int.Parse(wants[2]));
                            if (bytes != -1)
                            {
                                if (bytes == 1)
                                {
                                    CurrentArea.activeIndex = index;
                                }
                                CurrentArea.clientObjects[index].SendToWait($"UP");
                                message = "ok";
                            } else
                            {
                                message = "no";
                            }
                            break;
                        default:
                            Console.WriteLine("Closing..." + wants[0] + $" $$${Id}$$$");
                            stream.Close();
                            client.Close();
                            return;
                    }
                    // отправляем обратно сообщение
                    data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
                IsConnect = false;
                Waiting.Set();
            }
        }
        public string SetData(string[] data)
        {
            Ship[] ships = new Ship[10];
            for (int i = 1; i < 11; i++)
            {
                ships[i - 1] = new Ship(data[i]);
                if (CurrentArea.Areas[MyIndex].SetShip(ships[i - 1]))
                {
                    return "no";
                }
            }
            CurrentArea.Areas[MyIndex].ships = ships;
            return "ok";
        }
        public string GetWaiters()
        {
            StringBuilder stringBuilder = new StringBuilder();
            lock (Web.LockObj)
            {
                for (int i = 0; i < Web.ClientObjects.Count; i++)
                {
                    if (Web.ClientObjects[i].IsWait && Web.ClientObjects[i].CurrentArea == null)
                    {
                        stringBuilder.Append($"ID: {Web.ClientObjects[i].Id}\n");
                    }
                }
            }
            return stringBuilder.ToString();
        }
        public void SendToWait(string information)
        {
            extraMessage = information;
            Waiting.Set();
            IsWait = false;
        }
        public bool ConnectId(int id)
        {
            lock(Web.LockObj)
            {
                for (int i = 0; i < Web.ClientObjects.Count; i++)
                {
                    if (Web.ClientObjects[i].Id == id)
                    {
                        if (Web.ClientObjects[i].IsWait && Web.ClientObjects[i].IsConnect)
                        {
                            Web.ClientObjects[i].SendToWait("check");
                            Web.ClientObjects[i].Waiting.WaitOne();
                            if (!Web.ClientObjects[i].IsConnect)
                            {
                                continue;
                            }
                            Web.ClientObjects[i].SendToWait($"CI {Id}");
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }
        public ClientObject FindByID(int id)
        {
            for(int i = 0; i < Web.ClientObjects.Count; i++)
            {
                if (Web.ClientObjects[i].Id == id)
                {
                    return Web.ClientObjects[i];
                }
            }
            return null;
        }
    }
}
