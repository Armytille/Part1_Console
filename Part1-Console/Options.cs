using CommandLine;

namespace Part1_Console
{
    public class Options
    {
        [Option("csvPath", Required = true, HelpText = "Path to csv file")]
        public string CsvPath { get; set; }

        [Option("out", Required = false, Default = @"C:\Temp\", HelpText = @"Output path, C:\Temp\ by default ")]
        public string OutputPath { get; set; }

        [Option("txt", Required = false, HelpText = "Output file is a plain text .txt")]
        public bool Txt { get; set; }

        [Option("json", Required = false, HelpText = "Output file is a .json")]
        public bool Json { get; set; }

        /*Not a valid xml file*/
        [Option("xml", Required = false, HelpText = "Output file is a .xml")]
        public bool Xml { get; set; }

        [Option("zip", Required = false, HelpText = "Output file is a .zip")]
        public bool Zip { get; set; }
    }
}
