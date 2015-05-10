using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using SevenZip;

namespace HayDayDecoder
{
    class Decoder
    {
        public Decoder() {}

        public bool unzipDirectory(string dirPath)
        {
            string[] fileList = Directory.GetFiles(dirPath);
            foreach(string file in fileList)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Extension != ".csv") continue;

                // Unzip file
                if(unzipFile(file))
                {
                    // Delete original file
                    File.Delete(file);

                    System.Console.WriteLine("[Delete file]\t" + fileInfo.Name);
                }
            }
            return true;
        }

        public bool unzipFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            // Step 1. Fix file
            fixFile(filePath, fileInfo);

            // Step 2. Unzip file
            try
            {
                string outputPath = fileInfo.FullName.Replace(".csv", "_r.csv");
                using (var input = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    var decoder = new LzmaDecodeStream(input);
                    using (var output = new FileStream(outputPath, FileMode.Create))
                    {
                        int bufSize = 24576, count;
                        byte[] buf = new byte[bufSize];
                        while ((count = decoder.Read(buf, 0, bufSize)) > 0)
                        {
                            output.Write(buf, 0, count);
                        }
                    }
                    decoder.Close();
                }
            }
            catch(Exception)
            {
                System.Console.WriteLine("[Broken file]\t" + fileInfo.Name);
                return false;
            }

            System.Console.WriteLine("[Unzip file]\t" + fileInfo.Name);
            return true;
        }

        /// <summary>
        /// Fix HayDay file to stander LMZA file.
        /// </summary>
        /// <param name="filePath"></param>
        private void fixFile(string filePath, FileInfo fileInfo)
        {

            if (fileInfo.Exists)
            {
                byte[] buffer = new byte[fileInfo.Length];

                // Read file to buffer
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {

                    using (BinaryReader bR = new BinaryReader(file))
                    {
                        buffer = bR.ReadBytes((int)fileInfo.Length);
                    }
                }

                // FixFile
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Write))
                {
                    using (BinaryWriter bW = new BinaryWriter(file))
                    {
                        bW.Write(buffer, 0, 8);
                        for (int i = 0; i < 4; i++)
                            bW.Write((byte)0x00);
                        bW.Write(buffer, 8, (int)fileInfo.Length - 8);
                    }

                }
            }
        }
    }
}
