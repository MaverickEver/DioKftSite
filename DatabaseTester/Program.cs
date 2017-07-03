using MS.WebSolutions.DioKft.ProductImporter;
using System;

namespace MS.WebSolutions.DioKft.DatabaseTester
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 && !string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("Expected argument is the path of a CSV file.");
                return;
            }

            var importer = new Importer();

            importer.ClearDatabase();
            var errors = importer.ExecuteImportLogic(args[0]);

            if (errors.Trim().Length == 0)
            {
                Console.WriteLine("Import have been executed without any issue.");
            }
            else
            {
                Console.WriteLine($"Missed items: {errors.ToString()}");
            }
            

            Console.ReadLine();
        }
        
    }
}
