﻿
using SearchEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        Engine engine;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Upload(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);

                FileUpload1.PostedFile.SaveAs(@"D:\Ajith\GitHub\SmartSearchOnImage\WebApplication1\images\" + fileName);
                this.imgDemo.ImageUrl = "images/" + fileName;
                engine = Engine.Instance;
                engine.ReadData(fileName);
            }            
        }

        protected void SearchText(object sender, EventArgs e)
        {
            var text = txtSearch.Text;
            engine = Engine.Instance;
            var result = engine.SearchData(text);


            string fileName = imgDemo.ImageUrl;
            Bitmap bmp = new Bitmap(@"D:\Ajith\GitHub\SmartSearchOnImage\WebApplication1\" + fileName);
            Graphics g = Graphics.FromImage(bmp);

            foreach (var r in result)
            {//x-left, y-top x2- right y2-bottom
                var cor = r.Split(',').Select(n=>Convert.ToInt32(n)).ToArray();
                g.DrawRectangle(Pens.Black, cor[0], cor[1], cor[2] - cor[0], cor[3] - cor[1]);
            }


            bmp.Save(@"D:\Ajith\GitHub\SmartSearchOnImage\WebApplication1\images\" + text + ".jpg");
            this.imgDemo.ImageUrl = "images/" + text + ".jpg";
            g.Dispose();
            bmp.Dispose();


        }        
    }
}