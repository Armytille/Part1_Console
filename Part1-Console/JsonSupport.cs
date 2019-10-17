using System;
using System.IO;

namespace Part1_Console
{
    public class JsonSupport : IDisposable
    {
        private readonly StreamWriter _sw;
        private readonly string _format;

        public JsonSupport(StreamWriter sw, string format)
        {
            _sw = sw;
            _format = format;
            try
            {
                if (_format.Equals("json"))
                    _sw.WriteLine("[");
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
                if (_format.Equals("json"))
                    _sw.WriteLine("]");
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }
    }
}
