using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sea_battle
{
    //public class Area
    //{
    //    public List<ClientObject> Clients = new List<ClientObject>();
    //    public Area Second;
    //    private int CurrentIndex;
    //    public int CurrentPlayer
    //    {
    //        get
    //        {
    //            return CurrentIndex;
    //        }
    //        set
    //        {
    //            if (value >= Clients.Count)
    //            {
    //                CurrentIndex = 0;
    //            }
    //        }
    //    }
    //    public bool ImNew = true;
    //    public static int Index = 0;
    //    public int Id;
    //    int[,] fields;
    //    public Ship[] ships = new Ship[10];
    //    public int CountShips = 0;
    //    public int DeadShips = 0;
    //    public virtual int this[int i, int y] {
    //        get
    //        {
    //            if (fields[i, y] == 1)
    //            {
    //                return 1;
    //            }
    //            return fields[i, y];
    //        }
    //    }
    //    public Area(int id)
    //    {
    //        Id = id;
    //        if (Index <= id)
    //        {
    //            Index = id + 1;
    //        }
    //        fields = new int[10, 10];
    //    }
    //    public Area()
    //    {
    //        Id = Index;
    //        Index++;
    //        fields = new int[10, 10];
    //    }
    //    public bool SetShip(int x, int y, int size, int rotation)
    //    {
    //        for (int i = 0; i < size; i += 2)
    //        {
    //            bool res = (rotation == 0) ? CheckAround(x + i, y) : CheckAround(x, y + i);
    //            if (res)
    //            {
    //                return true;
    //            }
    //            if (i == size - 2)
    //            {
    //                i--;
    //            }
    //        }
    //        ships[CountShips] = new Ship(x, y, rotation, size, this);
    //        for (int i = 0; i < size; i++)
    //        {
    //            if (rotation == 0)
    //            {
    //                fields[x + i, y] = 1;
    //            }
    //            else
    //            {
    //                fields[x, y + i] = 1;
    //            }
    //        }
    //        CountShips++;
    //        return false;
    //    }
    //    public bool SetShip(Ship ship)
    //    {
    //        for (int i = 0; i < ship.State.Length; i += 2)
    //        {
    //            bool res = (ship.Rotation == 0) ? CheckAround(ship.X + i, ship.Y) : CheckAround(ship.X, ship.Y + i);
    //            if (res)
    //            {
    //                return true;
    //            }
    //            if (i == ship.State.Length - 2)
    //            {
    //                i--;
    //            }
    //        }
    //        for (int i = 0; i < ship.State.Length; i++)
    //        {
    //            if (ship.Rotation == 0)
    //            {
    //                fields[ship.X + i, ship.Y] = 1;
    //            }
    //            else
    //            {
    //                fields[ship.X, ship.Y + i] = 1;
    //            }
    //        }
    //        return false;
    //    }
    //    public bool CheckAround(int x, int y)
    //    {
    //        if (x > 9 || y > 9)
    //        {
    //            return true;
    //        }
    //        for (int i = (x > 0) ? x - 1 : 0; i <= x + 1 && i < 10; i++)
    //        {
    //            for (int j = (y > 0) ? y - 1 : 0; j <= y + 1 && j < 10; j++)
    //            {
    //                if (fields[i, j] != 0)
    //                {
    //                    return true;
    //                }
    //            }
    //        }
    //        return false;
    //    }
    //    public StringBuilder CreateLine(int line, bool isPlayer)
    //    {
    //        StringBuilder stringBuilder = new StringBuilder();
    //        for (int i = 0; i < 10; i++)
    //        {
    //            switch (fields[i, line])
    //            {
    //                case 0:
    //                    stringBuilder.Append("  ");
    //                    break;
    //                case 1:
    //                    stringBuilder.Append((isPlayer) ? "▓▓" : "  ");
    //                    break;
    //                case 2:
    //                    stringBuilder.Append("▒▒");
    //                    break;
    //                case 3:
    //                    stringBuilder.Append("><");
    //                    break;
    //            }
    //        }
    //        return stringBuilder;
    //    }
    //    public virtual int Fire(int x, int y)
    //    {
    //        if (x < 0 || x > 9 || y < 0 || y > 9)
    //        {
    //            return -1;
    //        }
    //        if (fields[x, y] == 0)
    //        {
    //            fields[x, y] = 3;
    //            return 1;
    //        }
    //        else if (fields[x, y] == 1)
    //        {
    //            fields[x, y] = 2;
    //            DamageShip(x, y);
    //            return 0;
    //        }
    //        return -1;
    //    }
    //    private void DamageShip(int x, int y)
    //    {
    //        int temp;
    //        DeadShips = 0;
    //        for (int i = 0; i < 10; i++)
    //        {
    //            if (ships[i].Rotation == 0)
    //            {
    //                temp = x - ships[i].X;
    //                if (y == ships[i].Y && temp >= 0 && temp < ships[i].State.Length)
    //                {
    //                    ships[i].State[temp] = 1;
    //                    if (ships[i].CheckState())
    //                    {
    //                        DeadShipDamage(ships[i]);
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                temp = y - ships[i].Y;
    //                if (x == ships[i].X && temp >= 0 && temp < ships[i].State.Length)
    //                {
    //                    ships[i].State[temp] = 1;
    //                    if (ships[i].CheckState())
    //                    {
    //                        DeadShipDamage(ships[i]);
    //                    }
    //                }
    //            }
    //            if (ships[i].IsDead)
    //            {
    //                DeadShips++;
    //            }
    //        }
    //    }
    //    private void DeadShipDamage(Ship ship)
    //    {
    //        for (int i = 0; i < ship.State.Length; i += 2)
    //        {
    //            if (ship.Rotation == 0)
    //            {
    //                SetDamagedFieldsAround(ship.X + i, ship.Y);
    //            }
    //            else
    //            {
    //                SetDamagedFieldsAround(ship.X, ship.Y + i);
    //            }
    //            if (i == ship.State.Length - 2)
    //            {
    //                i--;
    //            }
    //        }
    //    }
    //    public void SetDamagedFieldsAround(int x, int y)
    //    {
    //        for (int i = (x > 0) ? x - 1 : 0; i <= x + 1 && i < 10; i++)
    //        {
    //            for (int j = (y > 0) ? y - 1 : 0; j <= y + 1 && j < 10; j++)
    //            {
    //                if (fields[i, j] == 0)
    //                {
    //                    fields[i, j] = 3;
    //                }
    //            }
    //        }
    //    }
    //    public override string ToString()
    //    {
    //        StringBuilder res = new StringBuilder();
    //        for (int i = 0; i < 10; i++)
    //        {
    //            for (int j = 0; j < 10; j++)
    //            {
    //                res.Append(this[i, j]);
    //            }
    //        }
    //        //res.Append(" " + Id);
    //        return res.ToString();
    //    }
    //    public void Synch()
    //    {
    //        CurrentPlayer = Second.Clients.IndexOf(Clients[0]);
    //    }
    //}
    public class TwinArea
    {
        public bool IsStart = false;
        public static int Index = 0;
        public int Id;
        public int activeIndex = 0;
        public ClientObject[] clientObjects = new ClientObject[2];
        public Area[] Areas = new Area[2];
        //public Ship[][] Ships = new Ship[2][];
        public TwinArea()
        {
            for (int i = 0; i < 2; i++)
            {
                Areas[i] = new Area(10);
            }
            Id = Index;
            Index++;
        }
    }
    public struct Area
    {
        public byte[,] data;
        public Ship[] ships;
        public byte this[int x, int y]
        {
            get
            {
                return data[x, y];
            }
            set
            {
                data[x, y] = value;
            }
        }
        public Area(int size)
        {
            data = new byte[size, size];
            ships = new Ship[10];
        }
        public bool SetShip(Ship ship)
        {
            for (int i = 0; i < ship.State.Length; i += 2)
            {
                bool res = (ship.Rotation == 0) ? CheckAround(ship.X + i, ship.Y) : CheckAround(ship.X, ship.Y + i);
                if (res)
                {
                    return true;
                }
                if (i == ship.State.Length - 2)
                {
                    i--;
                }
            }
            for (int i = 0; i < ship.State.Length; i++)
            {
                if (ship.Rotation == 0)
                {
                    data[ship.X + i, ship.Y] = 1;
                }
                else
                {
                    data[ship.X, ship.Y + i] = 1;
                }
            }
            return false;
        }
        public bool CheckAround(int x, int y)
        {
            if (x > 9 || y > 9)
            {
                return true;
            }
            for (int i = (x > 0) ? x - 1 : 0; i <= x + 1 && i < 10; i++)
            {
                for (int j = (y > 0) ? y - 1 : 0; j <= y + 1 && j < 10; j++)
                {
                    if (data[i, j] != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    stringBuilder.Append((this[i, j] == 1)? 0 : this[i, j]);
                }
            }
            Console.WriteLine(stringBuilder.ToString() + "$$$" + stringBuilder.Length);
            return stringBuilder.ToString();
        }
        public int Fire(int x, int y)
        {
            if (x < 0 || x > 9 || y < 0 || y > 9)
            {
                return -1;
            }
            if (data[x, y] == 0)
            {
                data[x, y] = 3;
                return 1;
            }
            else if (data[x, y] == 1)
            {
                data[x, y] = 2;
                DamageShip(x, y);
                return 0;
            }
            return -1;
        }
        private void DamageShip(int x, int y)
        {
            int temp;
            //DeadShips = 0;
            for (int i = 0; i < 10; i++)
            {
                if (ships[i].Rotation == 0)
                {
                    temp = x - ships[i].X;
                    if (y == ships[i].Y && temp >= 0 && temp < ships[i].State.Length)
                    {
                        ships[i].State[temp] = 1;
                        if (ships[i].CheckState())
                        {
                            DeadShipDamage(ships[i]);
                        }
                    }
                }
                else
                {
                    temp = y - ships[i].Y;
                    if (x == ships[i].X && temp >= 0 && temp < ships[i].State.Length)
                    {
                        ships[i].State[temp] = 1;
                        if (ships[i].CheckState())
                        {
                            DeadShipDamage(ships[i]);
                        }
                    }
                }
                if (ships[i].IsDead)
                {
                    //DeadShips++;
                }
            }
        }
        private void DeadShipDamage(Ship ship)
        {
            for (int i = 0; i < ship.State.Length; i += 2)
            {
                if (ship.Rotation == 0)
                {
                    SetDamagedFieldsAround(ship.X + i, ship.Y);
                }
                else
                {
                    SetDamagedFieldsAround(ship.X, ship.Y + i);
                }
                if (i == ship.State.Length - 2)
                {
                    i--;
                }
            }
        }
        public void SetDamagedFieldsAround(int x, int y)
        {
            for (int i = (x > 0) ? x - 1 : 0; i <= x + 1 && i < 10; i++)
            {
                for (int j = (y > 0) ? y - 1 : 0; j <= y + 1 && j < 10; j++)
                {
                    if (data[i, j] == 0)
                    {
                        data[i, j] = 3;
                    }
                }
            }
        }
    }
}
