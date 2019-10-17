using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProgramTest
{
    [TestClass]
    public class Part1UnitTest
    {
        private static readonly string WorkingDirectory = Directory.GetCurrentDirectory();
        private static readonly string ProjectDirectory = Directory.GetParent(WorkingDirectory).Parent?.FullName;
        private static readonly string CsvPath = Path.Combine(ProjectDirectory, "smallfile.csv");
        private static readonly string OutPath = Path.Combine(Path.GetTempPath(), "result.json");

        [TestMethod]
        public void P1Step1Test()
        {
            Part1_Console.Program.ParseCsv(CsvPath, OutPath, "json", false);
            Assert.IsTrue(File.Exists(OutPath));
            var result = File.ReadAllText(OutPath);
            /*Pas vraiment satisfait de cette solution*/
            Assert.IsTrue(result.Contains("[\r\n{\"lineNumber\":1,\"type\": \"ok\"," +
                                                        " \"concatAB\":\"OK\",\"sumCD\": 103 },\r\n{ \"lineNumber\": 1," +
                                                        " \"type\": \"error\", \"errorMessage\": CsvHelper.TypeConversion.TypeConverterException:"));
        }
    }
}
