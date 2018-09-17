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
        public static string name;
        static void Main(string[] args)
        {
            string[] lines;
            Console.WriteLine("WHAT UP BITCHES IT'S YA BOY....");
            string name = Console.ReadLine();
            Console.WriteLine("{0}!! YEAH THAT'S WHO I WAS THINKING OF!!",name);
            Console.ReadKey();
            if (name != "DELETE")
            {
                try
                {
                    lines = File.ReadAllLines(@"..\..\test.txt");
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine("OOPS");
                    lines = null;
                }

                if (lines != null)
                    foreach (string s in lines)
                    {
                        Console.WriteLine(s);
                    }

                File.AppendAllText(@"..\..\test.txt", name + "\n");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Deleting File");
                if(File.Exists(@"..\..\test.txt"))
                File.Delete(@"..\..\test.txt");
                Console.ReadKey();
                
            }
            
           
           
           


        }
    }
}
