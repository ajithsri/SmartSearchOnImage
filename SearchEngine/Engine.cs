using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tessnet2;

namespace SearchEngine
{
    public class Engine
    {
        Tesseract ocr;
        Dictionary<string, tessnet2.Word> dic = new Dictionary<string, tessnet2.Word>();


        private static Engine instance;

        public static Engine Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Engine();
                }
                return instance;
            }
        }


        private Engine()
        {
            ocr = new Tesseract();
            ocr.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.,$-/#&=()\"':?"); // If digit only
            ocr.Init(@"D:\Ajith\GitHub\SmartSearchOnImage\tessdata1", "eng", false); ; // To use correct tessdata
        }
        //static void Main1()
        //{
        //    var program = new Engine();
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();

        //    program.ReadData("aa");

        //    sw.Stop();
        //}

        public void ReadMulti()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Task[] tasks = new Task[1];
            tasks[0] = Task.Factory.StartNew(() =>{ Read("aa"); });
            //tasks[1] = Task.Factory.StartNew(() =>{ Read("32"); });
            //tasks[2] = Task.Factory.StartNew(() =>
            //{ Read("33"); });
            //tasks[3] = Task.Factory.StartNew(() =>
            //{ Read("34"); });

            Task.WaitAll(tasks);

            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);
            Console.ReadLine();
        }

        public void SingleRead(string fileName)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Read("3");


            sw.Stop();
            Console.WriteLine("Elapsed{0}", sw.Elapsed);
            Console.ReadLine();
        }


        public void ReadData(string fileName)
        {
            try
            {
                string startupPath = Environment.CurrentDirectory;
                var image = new Bitmap(@"D:\Ajith\GitHub\SmartSearchOnImage\WebApplication1\images\" + fileName);

                List<tessnet2.Word> result = ocr.DoOCR(image, System.Drawing.Rectangle.Empty);

                

                foreach (tessnet2.Word word in result)
                {
                    //x-left, y-top x2- right y2-bottom
                    var key = word.Left + ", " + word.Top + ", " + word.Right + ", " + word.Bottom;
                    dic.Add(key, word);
                    
                }                
            }
            catch (Exception exception)
            {

            }
        }

        public IEnumerable<string> SearchData(string text)
        {
            List<string> coordinates = new List<string>();
            //var charList = dic.Where(pc => pc.Value.Text.Contains(text)).Select(x => x.Value.CharList);
            //if (charList.Count() > 0)
            //{
            //    //int sIndex, eIndex;

            //    foreach (List<Character> list in charList)
            //    {
            //        int sIndex, eIndex, count = 0;
            //        foreach (Character chart in list)
            //        {
            //            if (chart.Equals(text[count]))
            //            {
            //                sIndex = count;
            //            }
            //        }
            //        //Results += chart.Value + ", " + chart.Top + ", " + chart.Bottom + ", " + chart.Left + ", " + chart.Right + "\n";
            //    }

            //}
            //else
            //{
            //    return null;
            //}

            var wordList = dic.Where(pc => pc.Value.Text.Contains(text)).Select(x => x.Value);
            if (wordList.Count() > 0)
            {
                //int sIndex, eIndex;

                foreach (Word word in wordList)
                {
                    int sIndex = word.Text.IndexOf(text);
                    int eIndex = sIndex + text.Length - 1;

                    string coordinate = word.CharList[sIndex].Left + "," + word.CharList[sIndex].Top + "," + word.CharList[eIndex].Right + "," + word.CharList[eIndex].Bottom;

                    coordinates.Add(coordinate);
                }
                return coordinates;
            }
            else
            {
                return null;
            }
        }

        public void Read(string fileName)
        {
            try
            {                
                var image = new Bitmap(@"..\..\..\test\" + fileName + ".jpg");
                
                List<tessnet2.Word> result = ocr.DoOCR(image, System.Drawing.Rectangle.Empty);

                string Results = "";


                foreach (tessnet2.Word word in result)
                {
                    Results += word.Confidence + ", " + word.Text + ", " + word.Top + ", " + word.Bottom + ", " + word.Left + ", " + word.Right + "\n";
                    foreach (tessnet2.Character chart in word.CharList)
                    {
                        Results += chart.Value + ", " + chart.Top + ", " + chart.Bottom + ", " + chart.Left + ", " + chart.Right + "\n";
                    }
                    Results += "\n";
                }
                using (StreamWriter writer = new StreamWriter(@"..\..\..\test\" + fileName + ".txt", false))
                {
                    writer.WriteLine(Results);
                    writer.Close();
                }
                //Console.ReadLine();
            }
            catch (Exception exception)
            {

            }
        }
    }
}
