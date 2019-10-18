using System;
using CsvHelper;
using System.IO;
using CommandLine;
using Newtonsoft.Json;

namespace Part1_Console
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    var guid = Guid.NewGuid();
                    Directory.CreateDirectory(o.OutputPath);

                    if (o.Txt)
                        ParseCsv(o.CsvPath, Path.Combine(o.OutputPath, $"{guid}.txt"), "txt", o.Zip);
                    if (o.Json)
                        ParseCsv(o.CsvPath, Path.Combine(o.OutputPath, $"{guid}.json"), "json", o.Zip);
                    if (o.Xml)
                        ParseCsv(o.CsvPath, Path.Combine(o.OutputPath, $"{guid}.xml"), "xml", o.Zip);
                });
        }

        private static void WriteToFile(CsvReader csv, StreamWriter file, string format)
        {
            try
            {
                var sumCD = csv.GetField<int>("columnC") + csv.GetField<int>("columnD");
                if (sumCD <= 100) return;

                var data = new Data(csv.GetFieldIndex("columnA"), "ok",
                    csv.GetField<string>("columnA") + csv.GetField<string>("columnB"), sumCD);
                var line = format.Equals("json") ? JsonConvert.SerializeObject(data)
                    : format.Equals("txt") ? data.ToString() 
                    : format.Equals("xml") ? XmlConverter.Serialize(data)
                    : "No format";
                file.WriteLine(line + ",");
            }
            catch (CsvHelper.TypeConversion.TypeConverterException exception)
            {
                var onErrorData = new OnErrorData(csv.GetFieldIndex("columnA"), "error", exception.Message);
                var line = format.Equals("json") ? JsonConvert.SerializeObject(onErrorData) 
                    : format.Equals("txt") ? onErrorData.ToString()
                    : format.Equals("xml") ? XmlConverter.Serialize(onErrorData)
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

            using (var file = new StreamWriter(outPath, false))
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
