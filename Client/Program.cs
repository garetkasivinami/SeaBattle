using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Client
{
    class Program
    {
        public static Random Random = new Random();
        public static int MyId;
        public static int connectID;
        public static int IdGame;
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            Connect(new ConnectClient(out MyId));
            Console.ReadKey();
        }
        public static void Connect(ConnectClient connectClient)
        {
            Console.WriteLine("Your ID: {0}", MyId);
            Console.WriteLine("Оберіть режим роботи: 1 - пошук, 2 - очікування");
            if (Console.ReadLine() == "1")
            {
                int number;
                bool res = true;
                do
                {
                    Console.Write(connectClient.GetWaiters());
                    Console.WriteLine("Очікується номер...");
                    number = int.Parse(Console.ReadLine());
                    if (number == -1)
                    {
                        res = true;
                        continue;
                    }
                    res = connectClient.CreateConnection(number, out IdGame);
                } while (!res);
                connectID = number;
            } else
            {
                while(true)
                {
                    string res = connectClient.WaitData();
                    string[] wants = res.Split(' ');
                    if (wants[0] == "CI")
                    {
                        connectID = int.Parse(wants[1]);
                        break;
                    }
                }
                IdGame = connectClient.GetGameId(connectID);
            }
            Console.WriteLine("Connection successful!{0} {1}", MyId, connectID);
            Game(connectClient);
        }
        static void Game (ConnectClient connectClient)
        {
            char x; int y;
            VirtualArea MyArea = new VirtualArea();
            VirtualArea EnemyArea = new VirtualArea();
            //WebSetShips(MyArea, connectClient);
            RandomWebArea(MyArea, connectClient);
            bool GameIsPlaying = true;
            connectClient.WaitStartGame();
            while (GameIsPlaying)
            {
                bool ImActive = connectClient.GetState();
                connectClient.GetData(MyArea, EnemyArea);
                PrintGame(MyArea, EnemyArea);
                if (ImActive)
                {
                    Console.WriteLine("Wait turn...");
                    Console.Write("x = ");
                    x = Console.ReadKey().KeyChar;
                    Console.Write("\ny = ");
                    y = int.Parse(Console.ReadLine());
                    connectClient.Fire(x - 'a', y - 1);
                } else
                {
                    connectClient.WaitData();
                }
            }
        }
        static int GetSize(int number)
        {
            if (number == 0)
            {
                return 4;
            }
            else if (number < 3)
            {
                return 3;
            }
            else if (number < 6)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
        static void PrintGame(VirtualArea first, VirtualArea second)
        {
            Console.Clear();
            Console.WriteLine("*РЕЖИМ ГРИ*");
            Console.WriteLine("   A B C D E F G H I J       A B C D E F G H I J");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{i + 1:D2} " + first.CreateLine(i).ToString() + $" | {i + 1:D2} " + second.CreateLine(i).ToString() + '|');
            }
            //Console.WriteLine($"Dead ships {first.DeadShips:D2}                Dead ships {second.DeadShips:D2}");
        }
        //public static void WebGame(ConnectClient connectClient)
        //{
        //    int yourId, enemyId;
        //    VirtualArea first = connectClient.CreateArea(out yourId);
        //    RandomWebArea(first, connectClient, yourId);
        //    Console.WriteLine($"Your id {yourId}");
        //    Console.Write("Write enemy id:");
        //    enemyId = int.Parse(Console.ReadLine());
        //    VirtualArea second = new VirtualArea(connectClient.GetArea(enemyId, yourId));
        //    int progress = 0;
        //    bool gameisPlaying = true;
        //    while (gameisPlaying)
        //    {
        //        bool isPlayer = true;//connectClient.ImPlayer(enemyId);
        //        PrintGame(first, second);
        //        if (isPlayer)
        //        {
        //            char x; int y;
        //            progress++;
        //            Console.WriteLine($"Progress: {progress:D3}");
        //            Console.Write("x = ");
        //            x = Console.ReadKey().KeyChar;
        //            Console.Write("\ny = ");
        //            string temp;
        //            //if (!int.TryParse(Console.ReadLine(), out y) || connectClient.Fire(x - 'a', y - 1, enemyId, out temp) != 1)
        //            //{
        //            //    progress--;
        //            //    continue;
        //            //}
        //            //second.Update(temp);
        //        }
        //        else
        //        {
        //            int id;
        //            string wait;
        //            //while (connectClient.WaitData(out wait, out id))
        //            //{
        //            //    PrintGame(first, second);
        //            //    if (id == yourId)
        //            //    {
        //            //        first.Update(wait);
        //            //    }
        //            //    else
        //            //    {
        //            //        second.Update(wait);
        //            //    }
        //            //}
        //        }
        //    }
        //    PrintGame(first, second);
        //    Console.WriteLine($"Progress: {progress:D3}");
        //    Console.WriteLine("Кінець гри!");
        //    Console.ReadKey();
        //}
        static void PrintStartWebGame(VirtualArea area)
        {
            Console.Clear();
            Console.WriteLine("*РЕЖИМ ВСТАНОВЛЕННЯ КОРАБЛІВ*");
            Console.WriteLine("   A B C D E F G H I J");
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"{i + 1:D2}|");
                Console.WriteLine(area.CreateLine(i).ToString() + '|');
            }
        }
        public static void WebSetShips(VirtualArea area, ConnectClient connectClient)
        {
            char x;
            int y, rotation, size;
            Ship[] ships = new Ship[10];
            for (int i = 0; i < 10; i++)
            {
                size = GetSize(i);
                PrintStartWebGame(area);
                Console.WriteLine($"Розмір корабля: {size}");
                Console.WriteLine("Встановлення нового корабля");
                Console.Write("Напишіть координати корабля (маленька літера та координата)\nx = ");
                x = Console.ReadKey().KeyChar;
                Console.Write("\ny = ");
                if (!int.TryParse(Console.ReadLine(), out y))
                {
                    continue;
                }
                Console.Write("Вкажіть поворот корабля (0 - горизонтально, 1 - вертикально):");
                rotation = int.Parse(Console.ReadLine());
                ships[i] = new Ship(x - 'a', y - 1, rotation, size);
                if (area.SetShip(ships[i]))
                {
                    i--;
                }
            }
            PrintStartWebGame(area);
            connectClient.SendArea(ships);
        }
        static void RandomWebArea(VirtualArea area, ConnectClient connectClient)
        {
            Ship[] ships = new Ship[10];
            for (int i = 0; i < 10; i++)
            {
                ships[i] = new Ship(Random.Next(0, 10), Random.Next(0, 10), Random.Next(0, 2), GetSize(i));
                while (area.SetShip(ships[i]))
                {
                    ships[i].X = Random.Next(0, 10);
                    ships[i].Y = Random.Next(0, 10);
                    ships[i].Rotation = Random.Next(0, 2);
                }
            }
            PrintStartWebGame(area);
            connectClient.SendArea(ships);
        }
    }
}
