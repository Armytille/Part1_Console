using System;
using CsvHelper;
using System.IO;

namespace Part1_Console
{
    public class Program
    {
        private static readonly string WorkingDirectory = Directory.GetCurrentDirectory();
        private static readonly string ProjectDirectory = Directory.GetParent(WorkingDirectory).Parent?.FullName;
        private static readonly string CsvPath = Path.Combine(ProjectDirectory, "bigfile.csv");
        private static readonly string OutPath = Path.Combine(Path.GetTempPath(), "result.txt");

        private static void Main(string[] args)
        {
            P1Step1(CsvPath);
        }

        public static void P1Step1(string path)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader))
            {
                csv.Read();
                csv.ReadHeader();
                
                try
                {
                    if (File.Exists(OutPath))
                        File.Delete(OutPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                while (csv.Read())
                {
                    try
                    {
                        if ((csv.GetField<int>("columnC") + csv.GetField<int>("columnD")) <= 100) continue;
                        using (var file = new StreamWriter(OutPath, true))
                            file.WriteLine(csv.GetField<string>("columnA") + csv.GetField<string>("columnB"));
                    }
                    catch (CsvHelper.TypeConversion.TypeConverterException) {/*Ignored*/}
                }
            }
        }
    }
}
