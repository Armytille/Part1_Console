using CsvHelper;
using System;
using System.IO;
using System.IO.Compression;

namespace Part1_Console
{
    public class ZipSupport
    {
        public static void ArchiveOutput(string csvPath, string outPath, string format, Action<CsvReader, StreamWriter, string> formatMethod)
        {
            using (var zipToOpen = new FileStream(Path.ChangeExtension(outPath, "zip"), FileMode.OpenOrCreate))
            {
                using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    var resultEntry = archive.CreateEntry(Path.GetFileName(outPath));

                    using (var file = new StreamWriter(resultEntry.Open()))
                    using (new JsonSupport(file, format))
                        Program.ParseCsv(csvPath, file, format, formatMethod);
                }
            }
        }
    }
}
