using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace SecureDevApp
{
    class Program
    {
        public static string fileName;
        public static string[] lines;
        public static byte[][] keys = new byte[2][];


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
                        FileHandler("create");
                        break;
                    case '2':
                        FileHandler("open");
                        if (fileName != null)
                        Display();
                        Console.Write("Oh");
                        break;
                    case '3':
                        FileHandler("open");
                        if (fileName != null)
                        {
                            Update();
                            Display();
                        }
                        break;
                    case '4':
                        FileHandler("open");
                        if (fileName != null)
                            Delete();
                            chosen = true;
                        break;
                    case '5':
                        chosen = true;
                        break;
                    default:
                        break;
                }
            }

        }



        static void FileHandler(string mode)
        {
            AesManaged aesAlg = new AesManaged();

            aesAlg.Padding = PaddingMode.Zeros;

            switch (mode)
            {
                #region create
                case "create":
                    Console.Write("Enter File Name: ");
                    fileName = Console.ReadLine();
                    keys[0] = aesAlg.Key;
                    keys[1] = aesAlg.IV;
                    Console.WriteLine(aesAlg.IV.Length);
                    File.WriteAllBytes(@"../../" + "key" + fileName, aesAlg.Key);
                    File.WriteAllBytes(@"../../" + "IV" + fileName, aesAlg.IV);
                    fileName = fileName + ".txt";
                    Console.ReadKey();

                    int teamNumber = 0;
                    string team;
                    Console.Write("How many teams?: ");
                    int.TryParse(Console.ReadLine(), out teamNumber);
                    lines = new string[teamNumber];
                    for (int i = 0; i < teamNumber; i++)
                    {
                        int current = i;
                        Console.Write("Enter Team #{0}: ", current++);
                        team = Console.ReadLine();
                        Console.Write("Enter wins: ");
                        int.TryParse(Console.ReadLine(), out current);
                        team += "," + current.ToString();
                        Console.Write("Enter losses: ");
                        int.TryParse(Console.ReadLine(), out current);
                        team += "," + current.ToString();
                        lines[i] = team;
                    }
                       string encode = String.Join("[", lines);
                        EncryptTextToFile(encode, fileName, keys[0], keys[1]);
                    
                    break;
                #endregion
                #region openfile
                case "open":
                    lines = null;
                    bool exit = false;
                    fileName = null;
                    string test = "nah didn't work";
                    Console.Write("Enter Filename to open or press 2 to Exit.\nFile name: ");
                    if (Console.ReadKey(true).Key == ConsoleKey.D2)
                    {
                        exit = true;
                    }
                    if (!exit)
                    {
                        fileName = Console.ReadLine();
                    
                        Console.WriteLine(fileName);
                        keys[0] = File.ReadAllBytes(@"../../" + "key" + fileName);
                        keys[1] = File.ReadAllBytes(@"../../" + "IV" + fileName);
                        fileName = fileName + ".txt";
                        string decode = DecryptTextFromFile(fileName,keys[0], keys[1]);
                        lines = decode.Split('[');
                        foreach(string s in lines)
                            Console.WriteLine(lines);
                        try
                        {


                        }
                        catch (FileNotFoundException e)
                        {
                            Console.WriteLine("File Can't be found");
                            lines = null;
                            fileName = null;
                        }
                    }
                    Console.ReadKey();
                    break;
                    #endregion

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

            int current;
            Console.Write("Enter Team name: ");
            string team = Console.ReadLine();
            Console.Write("Enter wins: ");
            int.TryParse(Console.ReadLine(), out current);
            team += "," + current.ToString();
            Console.Write("Enter losses: ");
            int.TryParse(Console.ReadLine(), out current);
            team += "," + current.ToString();
            lines[lineNumber] = team;
            string encode = String.Join("[", lines);
            EncryptTextToFile(encode, fileName, keys[0], keys[1]);
            Console.ReadKey();

        }
        static void Display()
        {
            Console.Clear();
            string[] data;
            string formatted;
            string[] table = { "//----------------------------------------------------------------\\", "||                                        ||   Wins   ||  Losses  ||", "||----------------------------------------||----------||----------||", "\\----------------------------------------------------------------//" };
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + fileName.Length / 2) + "}", fileName));
            Console.WriteLine("{0," + ((Console.WindowWidth / 2) + table[0].Length / 2) + "}", table[0]);
            Console.WriteLine("{0," + ((Console.WindowWidth / 2) + table[1].Length / 2) + "}", table[1]);
            Console.WriteLine("{0," + ((Console.WindowWidth / 2) + table[2].Length / 2) + "}", table[2]);
            if (lines != null)
                foreach (string s in lines)
                {
                    data = s.Split(',');
                    formatted = String.Format("||{0,-40}||{1,10}||{2,10}||", data[0], data[1], data[2]);
                    Console.WriteLine("{0," + ((Console.WindowWidth / 2) + formatted.Length / 2) + "}", formatted);
                }
            Console.WriteLine("{0," + ((Console.WindowWidth / 2) + table[3].Length / 2) + "}", table[3]);
            Console.ReadKey();
        }





        public static void EncryptTextToFile(String Data, String FileName, byte[] Key, byte[] IV)
        {
            try
            {

                FileStream fStream = File.Open(FileName, FileMode.Create);//Create/Open File

                CryptoStream cStream = new CryptoStream(fStream, new AesManaged().CreateEncryptor(Key, IV), CryptoStreamMode.Write);

                StreamWriter sWriter = new StreamWriter(cStream);

                sWriter.WriteLine(Data);

                sWriter.Close();
                cStream.Close();
                fStream.Close();

            }

            catch (CryptographicException e)
            {

                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);

            }

            catch (UnauthorizedAccessException e)
            {

                Console.WriteLine("A file error occurred: {0}", e.Message);

            }

        }

        public static string DecryptTextFromFile(String FileName, byte[] Key, byte[] IV)
        {
            try
            {

                FileStream fStream = File.Open(FileName, FileMode.OpenOrCreate);

                CryptoStream cStream = new CryptoStream(fStream, new AesManaged().CreateDecryptor(Key, IV), CryptoStreamMode.Read);

                StreamReader sReader = new StreamReader(cStream);

                string val = sReader.ReadLine();

                sReader.Close();
                cStream.Close();
                fStream.Close();

                return val;

            }

            catch (CryptographicException e)
            {

                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;

            }

            catch (UnauthorizedAccessException e)
            {

                Console.WriteLine("A file error occurred: {0}", e.Message);
                return null;

            }

        }




    }
}
