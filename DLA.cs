using System;
namespace PorositySimulation
{
    class SimulationOfPorosityCluster
    {
        private int width;
        private int height;
        private int[,] field;
        private int porosity;
        private Random random = new Random();

        public SimulationOfPorosityCluster(int width, int height, int porosity)
        {
            this.width = width;
            this.height = height;
            this.field = new int[height, width];
            this.porosity = porosity;
        }

        public void Initialize()
        {
            int y0 = random.Next(1, height - 2);
            int x0 = random.Next(1, width - 2);
            field[y0, x0] = 2;
        }

        public void InitializeParticle()
        {
            int x = random.Next(0, width);
            int y = random.Next(0, height);

            while (field[y, x] != 0)
            {
                x = random.Next(0, width);
                y = random.Next(0, height);
            }

            field[y, x] = 1;
        }

        public void MoveParticle()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (field[y, x] == 1)
                    {
                        int dx = random.Next(-1, 2);
                        int dy = random.Next(-1, 2);

                        field[y, x] = 0;

                        int newX = x + dx;
                        int newY = y + dy;

                        if (newX >= width)
                            newX = width - 1;
                        else if (newX < 0)
                            newX = 0;

                        if (newY >= height)
                            newY = height - 1;
                        else if (newY < 0)
                            newY = 0;

                        if (field[newY, newX] == 0)
                            field[newY, newX] = 1;
                        else
                            field[y, x] = 1;
                    }
                }
            }
        }

        public int RunTransitionRule()
        {
            int massOfCluster = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (field[y, x] == 1)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            for (int dx = -1; dx <= 1; dx++)
                            {
                                int newX = x + dx;
                                int newY = y + dy;

                                if (newX >= width)
                                    newX = width - 1;
                                else if (newX < 0)
                                    newX = 0;

                                if (newY >= height)
                                    newY = height - 1;
                                else if (newY < 0)
                                    newY = 0;

                                if (field[newY, newX] == 2)
                                    field[y, x] = 2;
                            }
                        }
                    }
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (field[y, x] == 2)
                        massOfCluster++;
                }
            }

            return massOfCluster;
        }

        public void PrintField()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(field[y, x] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public bool NeedContinue()
        {
            int needToContinue = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (field[y, x] == 2)
                    {
                        needToContinue += 1;
                    }
                }
            }
            int actualPorosity = needToContinue / (height * width) * 100;
            int choice = 0;
            if (actualPorosity < porosity)
            {
                choice = 1;
            }
            if (choice == 1)
            {
                ;
                int no = 1;

                for (int i = 0; i < no; i++)
                    InitializeParticle();

                MoveParticle();
                RunTransitionRule();
                PrintField();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class Program
    {
        public const int POROSITY = 70;
        static void Main(string[] args)
        {
            int height = 10;
            int width = 10;
            int porosity = POROSITY;

            var simm = new SimulationOfPorosityCluster(width, height, porosity);
            simm.Initialize();
            int no = 1;

            for (int i = 0; i < no; i++)
                simm.InitializeParticle();

            simm.MoveParticle();
            simm.RunTransitionRule();
            simm.PrintField();

            while (simm.NeedContinue()) { }
        }
    }
}