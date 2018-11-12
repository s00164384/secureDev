using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SecureDevApp
{
    class Program
    {
        public static string fileName;
        public static string[] lines;
        static void Main(string[] args)
        {
            string[] Title = { "Welcome to Standings Creator", "Created by Philip D'Arcy", "S00164384", "", "Press Any Key To Continue." };

            foreach (string s in Title)
            {
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + s.Length / 2) + "}", s));
            }
            Console.ReadKey();
            Console.Clear();
            Menu();
        }

        static void Menu()
        {
            bool chosen = false;
            string[] options = { "Please select an option: ", "1. Create a Table", "2. Read a Table", "3. Update an existing Table", "4. Remove a Table", "5. Quit" };
            while (!chosen)
            {
                Console.Clear();
                foreach (string s in options)
                {
                    Console.WriteLine(s);
                }

                char choice = Console.ReadKey(true).KeyChar;
                switch(choice)
                {
                    case '1':
                        Create();
                        break;
                    case '2':
                        ReadFile();
                        if(fileName != null)
                        Display();
                        Console.Write("Oh");
                        break;
                    case '3':
                        ReadFile();
                        if (fileName != null)
                        {
                            Update();
                            Display();
                        }
                        break;
                    case '4':
                        ReadFile();
                        if (fileName != null)
                            Delete();
                        break;
                    case '5':
                        chosen = true;
                        break;
                    default:
                        break;
                }
            }

        }
        static void Create()
        {
            int teamNumber = 0;
            string team;
            Console.Write("Enter File Name: ");
            fileName = Console.ReadLine();
            Console.Write("How many teams?: ");
            int.TryParse(Console.ReadLine(), out teamNumber);
            lines = new string[teamNumber];
            for(int i = 0; i < teamNumber;i++)
            {
                int current = i;
                Console.Write("Enter Team #{0}: ",current++);
                team = Console.ReadLine();
                Console.Write("Enter wins: ");
                int.TryParse(Console.ReadLine(), out current);
                team += "," + current.ToString();
                Console.Write("Enter losses: ");
                int.TryParse(Console.ReadLine(), out current);
                team += "," + current.ToString();
                lines[i] = team;
            }

            File.WriteAllLines(@"../../" + fileName + ".txt", lines);
        }
        static void ReadFile()
        {
            lines = null;
            bool exit = false;
            fileName = null;
            Console.Write("Enter Filename to open or press 2 to Exit.\nFile name: ");
            if(Console.ReadKey(true).Key == ConsoleKey.D2)
            {
                exit = true;
            }
            if (!exit)
            {
                fileName = Console.ReadLine();
                try
                {
                    lines = File.ReadAllLines(@"../../" + fileName + ".txt");
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine("File Can't be found");
                    lines = null;
                    fileName = null;
                    Console.ReadKey();
                }
            }
        }
        static void Delete()
        {
            File.Delete(@"../../" + fileName + ".txt");
            Console.Write("File Deleted!\nPress Any Key To Continue.");
            Console.ReadKey();
        }

        static void Update()
        {
            int lineNumber = 99;
            if (lineNumber > lines.Length - 1)
            {
                Console.Write("Edit which value: ");
                int.TryParse(Console.ReadLine(), out lineNumber);
                lineNumber--;
            }
            
            Console.Write("\nCurrent Value : {0}\nEnter new value: ",lines[lineNumber]);
            lines[lineNumber] = Console.ReadLine();
            File.WriteAllLines(@"../../" + fileName + ".txt",lines);
            Console.ReadKey();

        }
        static void Display()
        {
            Console.Clear();
            string[] data;
            string formatted;
            string[] table = { "//----------------------------------------------------------------\\", "||                                        ||   Wins   ||  Losses  ||", "||----------------------------------------||----------||----------||", "\\----------------------------------------------------------------//" };
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + fileName.Length / 2) + "}", fileName));
            Console.WriteLine("{0," + ((Console.WindowWidth / 2) + table[0].Length / 2) + "}",table[0]);
            Console.WriteLine("{0," + ((Console.WindowWidth / 2) + table[1].Length / 2) + "}", table[1]);
            Console.WriteLine("{0," + ((Console.WindowWidth / 2) + table[2].Length / 2) + "}", table[2]);
            if (lines != null)
                foreach (string s in lines)
                {
                    data = s.Split(',');
                    formatted = String.Format("||{0,-40}||{1,10}||{2,10}||", data[0], data[1], data[2]);
                    Console.WriteLine("{0," + ((Console.WindowWidth / 2) + formatted.Length / 2) + "}",formatted);
                }
            Console.WriteLine("{0," + ((Console.WindowWidth / 2) + table[3].Length / 2) + "}", table[3]);
            Console.ReadKey();
        }
    }
}
