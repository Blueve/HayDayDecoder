using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace HayDayDecoder
{
    class Program
    {
        static void Main(string[] args)
        {
            string path;
            path = args.Length == 0 ? Directory.GetCurrentDirectory() : args[0];

            Decoder decoder = new Decoder();
            try
            {
                decoder.unzipDirectory(path);
            }
            catch
            {
                System.Console.WriteLine("[Error]\tDirectory doesn't exist.");
            }
            
        }
    }
}
