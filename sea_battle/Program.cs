using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sea_battle
{
    class Program
    {
        public static List<Area> Areas = new List<Area>();
        public static Random Random = new Random();
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            Web.AdminWork();
            Console.ReadKey();
        }
    }
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
        public Ship(string data) : this(data[0] - '0', data[1] - '0', data[2] - '0', data[3] - '0')
        {

        }
        public bool CheckState()
        {
            int counter = 0;
            for (int i = 0; i < State.Length; i++)
            {
                if (State[i] == 1)
                {
                    counter++;
                }
            }
            if (counter == State.Length)
            {
                IsDead = true;
                return true;
            }
            return false;
        }
    }

}
