using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Ship
    {
        public int X, Y;
        public int Rotation;
        public int[] State;
        public bool IsDead = false;
        public Ship(int x, int y, int rotation, int lenght)
        {
            X = x;
            Y = y;
            Rotation = rotation;
            State = new int[lenght];
        }
    }
    public class VirtualArea
    {
        int[,] fields;
        public VirtualArea()
        {
            fields = new int[10,10];
        }
        public VirtualArea(string data)
        {
            fields = new int[10, 10];
            for (int i = 0; i < 100; i++)
            {
                fields[i / 10, i % 10] = data[i] - '0';
            }
        }
        public StringBuilder CreateLine(int line)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                switch (fields[i, line])
                {
                    case 0:
                        stringBuilder.Append("  ");
                        break;
                    case 1:
                        stringBuilder.Append("▓▓");
                        break;
                    case 2:
                        stringBuilder.Append("▒▒");
                        break;
                    case 3:
                        stringBuilder.Append("><");
                        break;
                }
            }
            return stringBuilder;
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
                    if (fields[i, j] != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
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
                    fields[ship.X + i, ship.Y] = 1;
                }
                else
                {
                    fields[ship.X, ship.Y + i] = 1;
                }
            }
            return false;
        }
        public void Update(string data)
        {
            for (int i = 0; i < 100; i++)
            {
                if (!(fields[i / 10, i % 10] == 1 && data[i] == '0'))
                {
                    fields[i / 10, i % 10] = data[i] - '0';
                }
            }
        }
    }
}
