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

        private static void ArchiveOutput(string csvPath, string outPath, Action<CsvReader, StreamWriter> formatMethod)
        {
            using (var zipToOpen = new FileStream(Path.ChangeExtension(outPath, "zip"), FileMode.OpenOrCreate))
            {
                using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    var resultEntry = archive.CreateEntry(Path.GetFileName(outPath));

                    using (var file = new StreamWriter(resultEntry.Open()))
                        ParseCsv(csvPath, file, formatMethod);
                }
            }
        }

        private static void WritePlainTextToFile(CsvReader csv, StreamWriter file)
        {
            try
            {
                var sumCD = csv.GetField<int>("columnC") + csv.GetField<int>("columnD");

                if (sumCD <= 100) return;
                file.WriteLine($@"lineNumber : {csv.GetFieldIndex("columnA")},"
                                   + @"type : ok , concatAB :"
                                   + $@"{csv.GetField<string>("columnA") + csv.GetField<string>("columnB")},"
                                   + $@"sumCD: {sumCD} ,");
            }
            catch (CsvHelper.TypeConversion.TypeConverterException exception)
            {
                file.WriteLine($@"lineNumber : {csv.GetFieldIndex("columnA")}, type : error , errorMessage : {exception},");
            }
        }

        private static void WriteJsonArrayToFile(CsvReader csv, StreamWriter file)
        {
            try
            {
                var sumCD = csv.GetField<int>("columnC") + csv.GetField<int>("columnD");

                if (sumCD <= 100) return;
                file.WriteLine($@"{{""lineNumber"":{csv.GetFieldIndex("columnA")},"
                                   + @"""type"": ""ok"", ""concatAB"":"
                                   + $@"""{csv.GetField<string>("columnA") + csv.GetField<string>("columnB")}"","
                                   + $@"""sumCD"": {sumCD} }},");
            }
            catch (CsvHelper.TypeConversion.TypeConverterException exception)
            {
                file.WriteLine($@"{{ ""lineNumber"": {csv.GetFieldIndex("columnA")}, ""type"": ""error"", ""errorMessage"": {exception} }},");
            }
        }

        public static void ParseCsv(string path, string outPath, string format, bool zipMode)
        {
            switch (format)
            {
                case "json":

                    if (zipMode)
                        ArchiveOutput(path, outPath, WriteJsonArrayToFile);
                    else
                        using (new CreateJson(outPath))
                        using (var file = new StreamWriter(outPath, true))
                            ParseCsv(path, file, WriteJsonArrayToFile);
                    break;
                case "txt":
                    if (zipMode)
                        ArchiveOutput(path, outPath, WritePlainTextToFile);
                    else
                        using (var file = new StreamWriter(outPath, true))
                            ParseCsv(path, file, WritePlainTextToFile);
                    break;
                default:
                    Console.WriteLine("No output format. Will return.");
                    return;
            }
        }

        private static void ParseCsv(string path, StreamWriter file, Action<CsvReader, StreamWriter> formatMethod)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                    formatMethod(csv, file);
            }
        }
    }
}
