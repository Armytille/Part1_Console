using CommandLine;

namespace Part1_Console
{
    public class Options
    {
        [Option("txt", Required = false, HelpText = "Output file is a plain text .txt")]
        public bool Txt { get; set; }

        [Option("json", Required = false, HelpText = "Output file is a .json")]
        public bool Json { get; set; }

        [Option("zip", Required = false, HelpText = "Output file is a .zip")]
        public bool Zip { get; set; }
    }
}
