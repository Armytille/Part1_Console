using System;
using CsvHelper;
using System.IO;
using System.IO.Compression;
using CommandLine;

namespace Part1_Console
{
    public class Program
    {
        private static readonly string WorkingDirectory = Directory.GetCurrentDirectory();
        private static readonly string ProjectDirectory = Directory.GetParent(WorkingDirectory).Parent?.FullName;
        private static readonly string CsvPath = Path.Combine(ProjectDirectory, "bigfile.csv");

        private static void Main(string[] args)
        {
            string[] paths =
                {
                    Path.Combine(Path.GetTempPath(), "result.txt"),
                    Path.Combine(Path.GetTempPath(), "result.json"),
                    Path.Combine(Path.GetTempPath(), "result.zip")
                };

            try
            {
                foreach (var path in paths)
                    if (File.Exists(path))
                        File.Delete(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    if (o.Txt)
                        ParseCsv(CsvPath, paths[0], "txt", o.Zip);
                    if (o.Json)
                        ParseCsv(CsvPath, paths[1], "json", o.Zip);
                });
        }

        private static void WriteToFile(CsvReader csv, StreamWriter file, string format)
        {
            try
            {
                var sumCD = csv.GetField<int>("columnC") + csv.GetField<int>("columnD");

                if (sumCD <= 100) return;

                var line = format.Equals("json") ? $@"{{""lineNumber"":{csv.GetFieldIndex("columnA")},"
                                                   + @"""type"": ""ok"", ""concatAB"":"
                                                   + $@"""{csv.GetField<string>("columnA") + csv.GetField<string>("columnB")}"","
                                                   + $@"""sumCD"": {sumCD} }}," :
                            format.Equals("txt") ? $@"lineNumber : {csv.GetFieldIndex("columnA")},"
                                           + @"type : ok , concatAB :"
                                           + $@"{csv.GetField<string>("columnA") + csv.GetField<string>("columnB")},"
                                           + $@"sumCD: {sumCD} ,"
                            : "No format";
                file.WriteLine(line);
            }
            catch (CsvHelper.TypeConversion.TypeConverterException exception)
            {
                var line = format.Equals("json") ? $@"{{ ""lineNumber"": {csv.GetFieldIndex("columnA")}, ""type"": ""error"", ""errorMessage"": {exception} }},"
                    : format.Equals("txt") ? $@"lineNumber : {csv.GetFieldIndex("columnA")}, type : error , errorMessage : {exception},"
                    : "No format";
                file.WriteLine(line);
            }
        }


        public static void ParseCsv(string path, string outPath, string format, bool zipMode)
        {
            if (zipMode)
            {
                ZipSupport.ArchiveOutput(path, outPath, format, WriteToFile);
                return;
            }

            using (var file = new StreamWriter(outPath, true))
            using (new JsonSupport(file, format))
                ParseCsv(path, file, format, WriteToFile);
        }

        internal static void ParseCsv(string path, StreamWriter file, string format, Action<CsvReader, StreamWriter, string> formatMethod)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                    formatMethod(csv, file, format);
            }
        }
    }
}
