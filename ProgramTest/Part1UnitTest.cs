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
            /*Pas vraiment satisfait de cette solution*/
            Assert.IsTrue(string.CompareOrdinal(result, "[\r\n{\"lineNumber\":1,\"type\": \"ok\"," +
                                                        " \"concatAB\":\"OK\",\"sumCD\": 103 },\r\n{ \"lineNumber\": 1," +
                                                        " \"type\": \"error\", \"errorMessage\": CsvHelper.TypeConversion.TypeConverterException:" +
                                                        " The conversion cannot be performed.\r\n    Text: '#number'\r\n    MemberType: \r\n" +
                                                        "    TypeConverter: 'CsvHelper.TypeConversion.Int32Converter'\r\n   " +
                                                        "à CsvHelper.TypeConversion.DefaultTypeConverter.ConvertFromString" +
                                                        "(String text, IReaderRow row, MemberMapData memberMapData)\r\n   à " +
                                                        "CsvHelper.TypeConversion.Int32Converter.ConvertFromString(String text, IReaderRow row," +
                                                        " MemberMapData memberMapData)\r\n   à CsvHelper.CsvReader.GetField(Type type, Int32 index," +
                                                        " ITypeConverter converter)\r\n   à CsvHelper.CsvReader.GetField[T](Int32 index, " +
                                                        "ITypeConverter converter)\r\n   à CsvHelper.CsvReader.GetField[T](String name, " +
                                                        "ITypeConverter converter)\r\n   à CsvHelper.CsvReader.GetField[T](String name)\r\n   " +
                                                        "à Part1_Console.Program.P1Step1(String path) dans " +
                                                        $"{Directory.GetParent(ProjectDirectory).Parent?.FullName}\\Part1-Console\\Part1-Console\\Program.cs:ligne 32 }}" +
                                                        ",\r\n]\r\n") == 0);
        }
    }
}
