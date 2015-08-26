using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tessnet2;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using SearchEngine;

namespace OCR_Test
{
    class Program
    {
        Tesseract ocr;
        Dictionary<string, string> dic = new Dictionary<string, string>();

        public Program(){
            ocr = new Tesseract();
            ocr.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.,$-/#&=()\"':?"); // If digit only
            ocr.Init(@"..\..\..\tessdata1", "eng", false); ; // To use correct tessdata
        }
        static void Main(string[] args)
        {
            var program = new Program();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //Task[] tasks = new Task[1];
            //tasks[0] = Task.Factory.StartNew(() => { new Test().Read("2"); });
            //tasks[1] = Task.Factory.StartNew(() => { new Program().Read("32"); });
            //tasks[2] = Task.Factory.StartNew(() =>
            //{ Read("33"); });
            //tasks[3] = Task.Factory.StartNew(() =>
            //{ Read("34"); });
            
            ///Task.WaitAll(tasks);

            //program.ReadData("aa");

            Engine engin = Engine.Instance;
            engin.ReadData("aa");
            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);




            Console.WriteLine("Enter search text - enter xx to exit");
            string line;
            while ((line = Console.ReadLine()) != "xx")
            {
                var result = program.SearchData(line);
                foreach (var r in result)
                    Console.WriteLine(r);
            }



            Console.ReadLine();

           // var pp = new Program();
           // var fileName = Console.ReadLine();

            //pp.SingleRead("3");

            //pp.ReadMulti();
            //new Program().Read(fileName);
        }

        
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
                var image = new Bitmap(@"..\..\..\test\" + fileName + ".jpg");

                List<tessnet2.Word> result = ocr.DoOCR(image, System.Drawing.Rectangle.Empty);

                

                foreach (tessnet2.Word word in result)
                {
                    var key = word.Top + ", " + word.Bottom + ", " + word.Left + ", " + word.Right;
                    dic.Add(key, word.Text);
                    
                }                
            }
            catch (Exception exception)
            {

            }
        }

        public IEnumerable<string> SearchData(string text)
        {
            var values = dic.Where(pc => pc.Value.Contains(text)).Select(x=>x.Key);
            return values;
        }

        public void Read(string fileName)
        {
            try
            {                
                var image = new Bitmap(@"..\..\..\test\" + fileName + ".jpg");
                
                List<tessnet2.Word> result = ocr.DoOCR(image, System.Drawing.Rectangle.Empty);

                string Results = "";

                //x-left, y-top x2- right y2-bottom
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
