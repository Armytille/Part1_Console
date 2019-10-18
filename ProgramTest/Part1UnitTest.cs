using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

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
        public void ParseCsvTest()
        {
            Part1_Console.Program.ParseCsv(CsvPath, OutPath, "json", false);
            Assert.IsTrue(File.Exists(OutPath));
            var result = File.ReadAllText(OutPath);
            /*Pas vraiment satisfait de cette solution*/
            Assert.IsTrue(string.CompareOrdinal(result, "[\r\n{\"LineNumber\":1,\"Type\":\"ok\",\"ConcatAb\":\"OK\",\"SumCd\":103}," +
                                          "\r\n{\"LineNumber\":0,\"Type\":\"error\",\"ErrorMessage\":\"The conversion cannot be performed.\\r\\n" +
                                          "    Text: '#number'\\r\\n    MemberType: \\r\\n    TypeConverter: 'CsvHelper.TypeConversion.Int32Converter'\"}\r\n]\r\n") == 0);
        }
    }
}
