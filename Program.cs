using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Assignment_3_Attempt_1
{
    class Program
    {
        static void Main(string[] args)
        {
            double standardDeviationZero = 10;
            double standardDeviationOne = 80;
            int popSize =100; //Size of initial array
            int upperBound = 30; //Range of values random initial values should be
            double[,] population = new double[popSize,4];  //[Adamantium, Unobtanium, Dilithium, Pandemonium]
            double[,] childPop = new double[popSize, 4];  //Array to hold children
            double[] fittestArr = new double[4];   //Array to hold "fittest" combination of weights
            int generations = 800;
            double fittest = 0;
            double weakest = 10000000000000;
            int counter = 0;
            int hallOfFameSize = Convert.ToInt16(0.1 * popSize);
            double[,] hallOfFame = new double[hallOfFameSize, 4];
            int[] hallOfShame = new int[Convert.ToInt16(0.1 * popSize)];

//Populate Original Population
            Random random = new Random();   //Decalre random number generator
            for(int rows = 0; rows < population.GetLength(0); rows++)  //Loop through rows {arrSize} times
            {
                for(int cols = 0; cols < population.GetLength(1); cols++)  //Loop through all 3 collumns
                {
                    population[rows, cols] = random.NextDouble()*upperBound;  //populate metalsArray
                    fittestArr[cols] = population[rows, cols];  //Save newest inidvivual to be added to initial Hall Of Fame
                }
                if (counter < hallOfFameSize)   //While there's still space in the Hall Of Fame
                    hallOfFame = populateHallOfFame(hallOfFame,fittestArr,counter); //Add latest individual to Hall Of Fame
                counter++;
            }
            hallOfFame = SortHallOfFame(hallOfFame);    //Sort Hall of Fame from smallest to largest
            disp2DArray(hallOfFame,2);
//Genetic Algorithm
            while(generations > 0)
            {
                double[,] childTemp = new double[2, 4];     //Children
                for (int y = 0; y < hallOfFame.GetLength(0); y++)
                    for (int x = 0; x < 4; x++)
                        hallOfFame[y, x] = population[y, x];
                for (int row = 0; row < population.GetLength(0); row = row +2)  //Do crossover 2 at a time
                {
                    double tempProfit1 = profit(population[row, 0], population[row, 1], population[row, 2], population[row, 3]); 
                    double tempProfit2 = profit(population[row + 1, 0], population[row + 1, 1], population[row + 1, 2], population[row + 1, 3]);
                    if (tempProfit1 < tempProfit2)  //Which is row has biggest profit?
                        tempProfit1 = tempProfit2;
                    if (tempProfit1 > fittest)  //If this row's profit is largest thus far, add it to HallOfFame
                    {
                        fittest = tempProfit1;
                        for (int i = 0; i < 4; i++)
                            fittestArr[i] = population[row, i];
                        hallOfFame = updateHallOfFame(fittestArr, hallOfFame);
                    }

                    for (int col = 0; col < 4; col++)
                    {
                        int[] uniform = new int[4];   //Declare random binary array
                        Random rnd = new Random();  //generate random number
                        for (int b = 0; b < 4; b++)
                            uniform[b] = Convert.ToInt16(Math.Round(1 - rnd.NextDouble()));   //Generate a random binary value

                        if (uniform[col] == 0)
                        {
                            childPop[row, col] = population[row,col] + mutate(standardDeviationZero);    //Add individual to the child population and mutate
                            childPop[row + 1, col] = population[row + 1, col] + mutate(standardDeviationZero);  //Add and mutate
                        }
                        else
                            if(uniform[col] == 1)
                        {
                            childPop[row, col] = population[row + 1, col] + mutate(standardDeviationOne);   //Add and mutate
                            childPop[row + 1, col] = population[row, col] + mutate(standardDeviationOne);   //Add and mutate
                        }
                    }
                }
                population = childPop;  //make the childn population the new parent population
                generations--;
                if (generations % 10 == 0)  //Display every 10 generations' profit
                    Console.WriteLine(fittest);
            }
            disp2DArray(hallOfFame, 2);
            Console.WriteLine();
            dispArr(fittestArr);
            Console.WriteLine("Best profit: {0:f1}", fittest);
            Console.ReadLine();
        }
        



/////////METHODS//////////METHODS//////////METHODS//////////METHODS////////////METHODS/////////////METHODS/////////////METHODS///////////
    //Hall of Fame
        //Populate Hall of fame
        private static double[,] populateHallOfFame(double[,] hallOfFame, double[] newIndividual, int counter)
        {
            for (int col = 0; col < 4; col++)
                hallOfFame[counter, col] = newIndividual[col];
            return hallOfFame;
        }
        //Sort hall of fame
        private static double[,] SortHallOfFame(double[,] hallOfFame)
        {
            int numOfRows = hallOfFame.GetLength(0);
            double[] profits = new double[numOfRows];
            double[,] tempHall = new double[numOfRows, 4];

            for(int row = 0; row < numOfRows; row++)
            {
                profits[row] = profit(hallOfFame[row, 0], hallOfFame[row, 1], hallOfFame[row, 2], hallOfFame[row, 3]);
            }
            Array.Sort(profits, (x, y) => y.CompareTo(x));
            Array.Reverse(profits);
            for (int x = 0; x < numOfRows; x++)
                for (int y = 0; y < numOfRows; y++)
                {
                    if (profits[x] == profit(hallOfFame[y, 0], hallOfFame[y, 1], hallOfFame[y, 2], hallOfFame[y, 3]))
                    {
                        for (int j = 0; j < 4; j++)
                            tempHall[x, j] = hallOfFame[y, j];
                    }
                }
            
            return tempHall;
        }
        //Update Hall Of Fame
        private static double[,] updateHallOfFame(double[] fittestArray, double[,] hallOfFame)
        {
            double[,] tempArr = new double[hallOfFame.GetLength(0), 4];
            for (int row = 0; row < (hallOfFame.GetLength(0) - 1); row++)
            {
                for(int col = 0; col < 3; col++)
                {
                    hallOfFame[row, col] = hallOfFame[row + 1, col];
                }
            }
            for (int x = 0; x < 4; x++)
                hallOfFame[hallOfFame.GetLength(0) - 1, x] = fittestArray[x];
            return hallOfFame;
        }
        //Mutate with Gaussian function
        private static double mutate(double stddev)
        {
            Random randomGaus = new Random();
            double val1 = 1.0 - randomGaus.NextDouble();
            double val2 = 1.0 - randomGaus.NextDouble();
            double output = Math.Sqrt(-2.0 * Math.Log(val1)) * Math.Cos(2.0 * Math.PI * val2);
            if (output < 0)
                output = 0;
            return output * stddev;
        }

    //Fitness function
        private static double profit(double adamantiumWeight, double unobtaniumWeight, double dilithiumWeight, double pandemoniumWeight)
        {
            double platWeight = (0.2 * adamantiumWeight) + (0.3*unobtaniumWeight) + (0.8*dilithiumWeight) + (0.1*pandemoniumWeight);
            double ironWeight = (0.7 * adamantiumWeight) + (0.2 * unobtaniumWeight) + (0.1 * dilithiumWeight) + (0.5 * pandemoniumWeight);
            double copperWeight = (0.1 * adamantiumWeight) + (0.5 * unobtaniumWeight) + (0.1 * dilithiumWeight) + (0.4 * pandemoniumWeight);
            double platWeightCost = (1200 + (10 * platWeight)) * platWeight;
            double ironWeightCost = (ironWeight - Math.Floor(platWeight)) * 300;    //Calculate the price of buying iron if you recieve a free kg of iron for every kg of platinum
            if (ironWeightCost < 0) //Ensures that the cost of iron purchases is never negative
                ironWeightCost = 0;
            double copperWeightCost = copperWeight * 800;
            if (copperWeight >= 8)  //Calculate the discount of 10% if buying more than 8kg of copper
                copperWeightCost = (copperWeight * 0.9) * 800;
            if (platWeight < 0)
                platWeightCost = 0;
            if (ironWeight < 0)
                ironWeightCost = 0;
            if (copperWeight < 0)
                copperWeightCost = 0;
            double electricityCost = Math.Exp(0.005 * ((adamantiumWeight*25)+(unobtaniumWeight*23)+(dilithiumWeight*35)+(pandemoniumWeight*20)));
            double totalCost = platWeightCost + ironWeightCost + copperWeightCost + electricityCost;
            double income = (adamantiumWeight * 3000) + (unobtaniumWeight * 3100) + (dilithiumWeight * 5200) + (pandemoniumWeight * 2500);
            return income-totalCost;
        }  
    //DISPLAY ARRAYS
        //Dsiplay 2D Arrays just individuals
        private static void disp2DArray(double[,] array)
        {
            for (int rows = 0; rows < array.GetLength(0); rows++)
            {
                Console.Write("{0}:  ", rows);
                for (int cols = 0; cols < array.GetLength(1); cols++)
                {
                    Console.Write("{0:f3}   ", array[rows, cols]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.ReadLine();
        }
        //Display 2D arrays with PROFIT
        private static void disp2DArray(double[,] array, double fitness)
        {
            for (int rows = 0; rows < array.GetLength(0); rows++)
            {
                Console.Write("{0}:  ", rows+1);
                for (int cols = 0; cols < array.GetLength(1); cols++)
                {
                    Console.Write("{0:f3}    ", array[rows, cols]);
                }
                Console.Write("fitness: {0:f3}", profit(array[rows, 0], array[rows, 1], array[rows, 2], array[rows, 3]));
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.ReadLine();
        }
        //Display 1D Array
        private static void dispArr(double[] array)
        {
            Console.WriteLine("Best Weights: ");
            for (int x = 0; x < array.Length; x++)
                Console.Write("{0:f3}   ", array[x]);
            Console.WriteLine();
            Console.ReadLine();
        }
        private static void dispArr(int[] array)
        {
            Console.WriteLine("Uniform array: ");
            for (int x = 0; x < array.Length; x++)
                Console.Write("{0}   ", array[x]);
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
