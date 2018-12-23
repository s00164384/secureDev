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
            byte[][] keys = new byte[2][];
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
                    try
                    {
                        EncryptTextToFile("New file", fileName,keys[0], keys[1]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
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
                        test = DecryptTextFromFile(fileName,keys[0], keys[1]);
                        Console.WriteLine(test);
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
            
            Console.Write("\nCurrent Value : {0}\nEnter new value: ",lines[lineNumber]);
            lines[lineNumber] = Console.ReadLine();
            foreach(string s in lines)
            {
                //EncryptTextToFile(s, fileName, aesAlg.Key, aesAlg.IV);
            }
            Console.ReadKey();

        }
        static void Display()
        {
            Console.Clear();
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + fileName.Length / 2) + "}", fileName));
            if (lines != null)
                foreach (string s in lines)
                {
                    Console.WriteLine(s);
                }
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
