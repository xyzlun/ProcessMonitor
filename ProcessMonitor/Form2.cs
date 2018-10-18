using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ProcessMonitor
{
    public partial class Form2 : Form
    {
        int Total_Memory;
        DataSet ds = new DataSet();
        public Form2(int total_memory)
        {
            InitializeComponent();
            Total_Memory = total_memory;
        }

        public void Read()
        {
            StreamReader sr = new StreamReader("log.txt", Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                string process_name = line.Split(':')[0].ToString();
                if (ds.Tables[process_name] != null)
                {
                    DataRow dr;
                    dr = ds.Tables[process_name].NewRow();
                    dr["value"] = line.Split(';')[1].Split('K')[0];
                    dr["time"] = line.Split(';')[1].Split(' ')[1];
                }
                else 
                {
                    DataTable dt= new DataTable();
                    DataColumn dCol = new System.Data.DataColumn("value", Type.GetType("System.String"));
                    dCol.Caption = "value";
                    dt.Columns.Add(dCol);

                    dCol = new System.Data.DataColumn("time", Type.GetType("System.String"));
                    dCol.Caption = "time";
                    dt.Columns.Add(dCol);

                    dt.TableName = process_name;
                    ds.Tables.Add(dt);
                }
                if (line.Contains("结束监控"))
                {
                    string time_span = line.Split(' ')[1];

                }


            }
        }


        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            int height = 480, width = 700; Bitmap image = new Bitmap(width, height);
            Graphics g = e.Graphics;
            CreatCharts(g,Total_Memory,1);            
        }

        private void CreatCharts(Graphics g,int totalMemory,int timeSpan)
        {
            try
            {
                //清空图片背景色 g.Clear(Color.White); 
                Font font = new System.Drawing.Font("Arial", 9, FontStyle.Regular);
                Font font1 = new System.Drawing.Font("宋体", 20, FontStyle.Regular);
                Font font2 = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, this.Width, this.Height), Color.Blue, Color.Blue, 1.2f, true);
                g.FillRectangle(Brushes.AliceBlue, 0, 0, this.Width, this.Height); 
                Brush brush1 = new SolidBrush(Color.Blue);
                Brush brush2 = new SolidBrush(Color.SaddleBrown);
                g.DrawString(" Process Chart", font1, brush1, new PointF(85, 30)); //画图片的边框线 
                g.DrawRectangle(new Pen(Color.Blue), 0, 0, this.Width - 1, this.Height - 1); 
                Pen mypen = new Pen(brush, 1); 
                Pen mypen2 = new Pen(Color.Red, 2); //绘制线条 
                //绘制纵向线条 
                int x = 60;
                for (int i = 0; i < 8; i++)
                {
                    g.DrawLine(mypen, x, 80, x, 340);
                    x = x + 80;
                }
                Pen mypen1 = new Pen(Color.Blue, 3);
                x = 60;
                g.DrawLine(mypen1, x, 82, x, 340); //绘制横向线条 
                int y = totalMemory;
                for (int i = 0; i < 10; i++)
                { g.DrawLine(mypen, 60, y, 620, y); y = y + 26; } // 
                y = 106;
                g.DrawLine(mypen1, 60, y - 26, 620, y - 26); //x轴 
                String[] n = { "第一期", "第二期", "第三期", "第四期", "上半年", "下半年", "全年统计" };
                x = 45;
                for (int i = 0; i < 7; i++)
                {
                    g.DrawString(n[i].ToString(), font, Brushes.Red, x, 348); //设置文字内容及输出位置 
                    x = x + 77;
                } 
                //y轴 
                String[] m = { "220人", " 200人", " 175人", "150人", " 125人", " 100人", " 75人", " 50人", " 25人" };
                y = 100;
                for (int i = 0; i < 9; i++)
                {
                    g.DrawString(m[i].ToString(), font, Brushes.Red, 10, y); //设置文字内容及输出位置 
                    y = y + 26;
                }
                int[] Count1 = new int[7];
                int[] Count2 = new int[7];

                DataSet ds = new DataSet();

                Font font3 = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                SolidBrush mybrush = new SolidBrush(Color.Red);
                Point[] points1 = new Point[7];
                points1[0].X = 60; points1[0].Y = 340 - Count1[0]; //从106纵坐标开始, 到(0, 0)坐标时 
                points1[1].X = 140; points1[1].Y = 340 - Count1[1];
                points1[2].X = 220; points1[2].Y = 340 - Count1[2];
                points1[3].X = 300; points1[3].Y = 340 - Count1[3];
                points1[4].X = 380; points1[4].Y = 340 - Count1[4];
                points1[5].X = 460; points1[5].Y = 340 - Count1[5];
                points1[6].X = 540; points1[6].Y = 340 - Count1[6];
                g.DrawLines(mypen2, points1); //绘制折线 //绘制数字
                g.DrawString(Count1[0].ToString(), font3, Brushes.Red, 58, points1[0].Y - 20);
                g.DrawString(Count1[1].ToString(), font3, Brushes.Red, 138, points1[1].Y - 20);
                g.DrawString(Count1[2].ToString(), font3, Brushes.Red, 218, points1[2].Y - 20);
                g.DrawString(Count1[3].ToString(), font3, Brushes.Red, 298, points1[3].Y - 20);
                g.DrawString(Count1[4].ToString(), font3, Brushes.Red, 378, points1[4].Y - 20);
                g.DrawString(Count1[5].ToString(), font3, Brushes.Red, 458, points1[5].Y - 20);
                g.DrawString(Count1[6].ToString(), font3, Brushes.Red, 538, points1[6].Y - 20);
                Pen mypen3 = new Pen(Color.Green, 2);
                Point[] points2 = new Point[7];
                points2[0].X = 60; points2[0].Y = 340 - Count2[0];
                points2[1].X = 140; points2[1].Y = 340 - Count2[1];
                points2[2].X = 220; points2[2].Y = 340 - Count2[2];
                points2[3].X = 300; points2[3].Y = 340 - Count2[3];
                points2[4].X = 380; points2[4].Y = 340 - Count2[4];
                points2[5].X = 460; points2[5].Y = 340 - Count2[5];
                points2[6].X = 540; points2[6].Y = 340 - Count2[6];
                g.DrawLines(mypen3, points2); //绘制折线 //绘制通过人数
                g.DrawString(Count2[0].ToString(), font3, Brushes.Green, 61, points2[0].Y - 15);
                g.DrawString(Count2[1].ToString(), font3, Brushes.Green, 131, points2[1].Y - 15);
                g.DrawString(Count2[2].ToString(), font3, Brushes.Green, 221, points2[2].Y - 15);
                g.DrawString(Count2[3].ToString(), font3, Brushes.Green, 301, points2[3].Y - 15);
                g.DrawString(Count2[4].ToString(), font3, Brushes.Green, 381, points2[4].Y - 15);
                g.DrawString(Count2[5].ToString(), font3, Brushes.Green, 461, points2[5].Y - 15);
                g.DrawString(Count2[6].ToString(), font3, Brushes.Green, 541, points2[6].Y - 15); //绘制标识 
                g.DrawRectangle(new Pen(Brushes.Red), 180, 390, 250, 50); //绘制范围框
                g.FillRectangle(Brushes.Red, 270, 402, 20, 10); //绘制小矩形 
                g.DrawString("报名人数", font2, Brushes.Red, 292, 400);
                g.FillRectangle(Brushes.Green, 270, 422, 20, 10);
                g.DrawString("通过人数", font2, Brushes.Green, 292, 420);

            }
            finally
            {
                g.Dispose();

            }
        }



    }
}