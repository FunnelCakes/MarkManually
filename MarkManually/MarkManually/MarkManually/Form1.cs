using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MarkManually
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            string exePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string imgPath = exePath + "..\\..\\..\\..\\..\\Source\\";
            Debug.WriteLine(imgPath);
            Pictures=Directory.GetFiles(imgPath);
            string[] name = new string[Pictures.Length];
            int[] val = new int[Pictures.Length];
            #region 图片排序
            for (int i = 0; i < Pictures.Length; i++)
            {
                name[i] = (Pictures[i].Substring(Pictures[i].Length-9));//截取文件名
                val[i] = Convert.ToInt32(name[i].Substring(0, 5));
            }
            for(int i = 0; i < val.Length-1; i++)
            {
                for(int j = 0; j < val.Length - 1 - i; j++)
                {
                    if (val[j] > val[j + 1])
                    {
                        int buf = val[j];
                        val[j] = val[j + 1];
                        val[j + 1] = buf;
                    }
                }
            }
            for(int i = 0; i < val.Length; i++)
            {
                if (val[i] < 10000)
                {
                    Pictures[i] = imgPath+"0" + Convert.ToString(val[i]) + ".png";
                }
                else Pictures[i] =imgPath+ Convert.ToString(val[i]) + ".png";
            }
            #endregion
            Image InitPic = Image.FromFile(Pictures[0]);

            pictureBox1.Image = InitPic;
            g = pictureBox1.CreateGraphics();


        }
        string[] Pictures;
        private bool drawable = false;
        private int x, y, x1, y1;           //声明鼠标坐标值
        private Graphics g ;       

        private int[,] features = new int[25,5];//声明y张量
        private int[,] tmp = new int[20,5]; //缓存方框信息([bx,bw,bh,bw])
        private int q = 0;                  //Pictures[]的指针
        private int p = 0;                //缓存张量tmp上的指针

        private void button1_Click(object sender, EventArgs e)
        {
            if (p != 0) p--;
        }//往左

        private void button2_Click(object sender, EventArgs e)
        {

        }//往右
        private void button4_Click(object sender, EventArgs e)
        {

        }//上一张图
        private void button3_Click(object sender, EventArgs e)
        {

        }//下一张图


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            drawable = true;
            

            x = e.X;
            y = e.Y;

        }//按下左键
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawable)
            {
                if (e.Button != MouseButtons.Left) return;
                x1 = e.X;
                y1 = e.Y;
                pictureBox1.Image.Dispose();//防止Image类实例多次刷新导致的内存溢出
                pictureBox1.Image = Image.FromFile(Pictures[q]);

                for (int i = 0; i < p; i++)
                {
                    int bx = tmp[i, 1];int by = tmp[i, 2];
                    int bh = tmp[i, 3];int bw = tmp[i, 4];
                    int X, Y, X1, Y1;
                    X = (2 * bx - bw) / 2;Y = (2 * by - bh) / 2;
                    X1 = (2 * bx + bw) / 2;Y1 = (2 * by + bh) / 2;
                    g.DrawLine(new Pen(Color.Red), X, Y, X, Y1);
                    g.DrawLine(new Pen(Color.Red), X, Y1, X1, Y1);
                    g.DrawLine(new Pen(Color.Red), X1, Y1, X1, Y);
                    g.DrawLine(new Pen(Color.Red), X1, Y, X, Y);
                }//draw方框in记忆
                tmp[p,0] = 1;
                tmp[p,1] = (int)((x + x1) / 2.0);
                tmp[p,2] = (int)((y + y1) / 2.0);
                tmp[p,3] = y1 - y;
                tmp[p,4] = x1 - x;

                g.DrawLine(new Pen(Color.Red), x, y, x, y1); //left
                g.DrawLine(new Pen(Color.Red), x, y1, x1, y1); //bottom
                g.DrawLine(new Pen(Color.Red), x1, y1, x1, y);//right
                g.DrawLine(new Pen(Color.Red), x1, y, x, y);//top

                
                


            }
        }//鼠标划过&框选


        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
    {
        drawable = false;

            for (int i = 0; i < p; i++)
            {
                int bx = tmp[i, 1]; int by = tmp[i, 2];
                int bh = tmp[i, 3]; int bw = tmp[i, 4];
                int X, Y, X1, Y1;
                X = (2 * bx - bw) / 2; Y = (2 * by - bh) / 2;
                X1 = (2 * bx + bw) / 2; Y1 = (2 * by + bh) / 2;
                g.DrawLine(new Pen(Color.Red), X, Y, X, Y1);
                g.DrawLine(new Pen(Color.Red), X, Y1, X1, Y1);
                g.DrawLine(new Pen(Color.Red), X1, Y1, X1, Y);
                g.DrawLine(new Pen(Color.Red), X1, Y, X, Y);
            }//draw方框in记忆
            tmp[p, 0] = 1;
            tmp[p, 1] = (int)((x + x1) / 2.0);
            tmp[p, 2] = (int)((y + y1) / 2.0);
            tmp[p, 3] = y1 - y;
            tmp[p, 4] = x1 - x;

            g.DrawLine(new Pen(Color.Red), x, y, x, y1); //left
            g.DrawLine(new Pen(Color.Red), x, y1, x1, y1); //bottom
            g.DrawLine(new Pen(Color.Red), x1, y1, x1, y);//right
            g.DrawLine(new Pen(Color.Red), x1, y, x, y);//top
            p++;
        }//鼠标抬起
        private void writeCSVFile(int data)
        {

        }//文件写入
   

    }
}
