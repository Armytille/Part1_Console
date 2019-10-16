using System;
using System.IO;

namespace Part1_Console
{
    public class CreateJson : IDisposable
    {
        private readonly string _outputPath;

        public CreateJson(string outputPath)
        {
            _outputPath = outputPath;

            try
            {
                if (File.Exists(outputPath))
                    File.Delete(outputPath);

                using (var file = new StreamWriter(outputPath, true))
                    file.WriteLine("[");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Dispose()
        {
            try
            {
                using (var file = new StreamWriter(_outputPath, true))
                    file.WriteLine("]");
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }
    }
}
