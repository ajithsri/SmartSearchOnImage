﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tessnet2;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace OCR_Test
{
    class Program
    {
        Tesseract ocr;
        

        public Program(){
            ocr = new Tesseract();
            ocr.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.,$-/#&=()\"':?"); // If digit only
            ocr.Init(@"..\..\..\tessdata1", "eng", false); ; // To use correct tessdata
        }
        static void Main(string[] args)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();
            //Task[] tasks = new Task[1];
            //tasks[0] = Task.Factory.StartNew(() => { new Test().Read("2"); });
            //tasks[1] = Task.Factory.StartNew(() => { new Program().Read("32"); });
            //tasks[2] = Task.Factory.StartNew(() =>
            //{ Read("33"); });
            //tasks[3] = Task.Factory.StartNew(() =>
            //{ Read("34"); });
            new Program().Read("5");
            ///Task.WaitAll(tasks);

            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);
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
