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
        private static readonly string OutPath = Path.Combine(Path.GetTempPath(), "result.txt");

        [TestMethod]
        public void P1Step1Test()
        {
            Part1_Console.Program.P1Step1(CsvPath);
            var result = File.ReadAllText(OutPath);
            Assert.IsTrue(string.CompareOrdinal(result,"OK\r\n") == 0);
        }
    }
}
