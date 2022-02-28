using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Лаба_6_ООП
{
    public partial class Form1 : Form
    {    
        public void selectObj(int l) 
        {
            for (int i = 0; i < V.get_size(); i++)
            {
                perobj0 = V.getObject(i);
                perobj0.selected = 0;
            }
            perobj0 = V.getObject(l);
            perobj0.selected = 1;
        }
        public int provALL(int X, int Y)
        {
            Object per;
            for (int i = V.get_size() - 1; i >= 0; i--)
            {
                per = V.getObject(i);
                if (per.prov_(X, Y))
                    return i;
            }
            return -1;
        }

        public int provALL_ver2()
        {
            Object per;
            for (int i = V.get_size() - 1; i >= 0; i--)
            {
                per = V.getObject(i);
                if (per.selected == 1)
                    return i;
            }
            return -1;
        }

        public void clearSheet(Graphics gr)
        {
            gr.Clear(Color.White);
        }

        public void DrawALL(Graphics gr)
        {
            Object per;
            for (int i = 0; i < V.get_size(); i++)
            {
                per = V.getObject(i);
                per.draw(gr);
            }
        }
    
        class Object
        {
            public int x, y, selected = 0;

            public Pen blackPen;
            public Pen redPen;
            public Pen bluePen;
            public Pen darkGoldPen;
            public Font fo;
            public Brush br, brw;
            public PointF point;

            public Object()
            {
                blackPen = new Pen(Color.Black);
                blackPen.Width = 3;
                redPen = new Pen(Color.Red);
                redPen.Width = 3;
                bluePen = new Pen(Color.BlueViolet);
                bluePen.Width = 3;
                darkGoldPen = new Pen(Color.DarkGoldenrod);
                darkGoldPen.Width = 2;
                fo = new Font("Arial", 15);
                br = Brushes.Black;
                brw = Brushes.White;
            }

            virtual public bool prov_(int X, int Y) 
            {
                return false;
            }

            virtual public bool prov_exit(int X, int Y)
            {
                return false;
            }

            virtual public void draw(Graphics gr)
            {
            }

            virtual public void draw_frame(Graphics gr)
            {
            }

            virtual public bool prov_frame(int X, int Y)
            {
                return false;
            }

            virtual public void set_coords(int X, int Y, int Xd, int Yd)
            {
            }
        }

        class Vertex : Object
        {
            public int w = 30, h = 30, R = 30;

            public Vertex(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public override void draw(Graphics gr)
            {
                gr.FillEllipse(brw, (x - w), (y - h), w*2, h*2);
                gr.DrawEllipse(blackPen, (x - w), (y - h), w*2, h*2);
                if (selected == 1)
                    draw_frame(gr);
            }

            public override void draw_frame(Graphics gr) 
            {
                gr.DrawRectangle(bluePen, (x - w), (y - h), w*2, h*2);

                gr.DrawRectangle(redPen, x-6, (y - h)-6, 12, 12);
                gr.DrawRectangle(redPen, (x - w)-6, y-6, 12, 12);
                gr.DrawRectangle(redPen, (x + w)-6, (y)-6, 12, 12);
                gr.DrawRectangle(redPen, (x)-6, (y + h)-6, 12, 12);
            }

            public override bool prov_(int X, int Y)
            {
                double per = (Math.Pow((x - X), 2) / Math.Pow((w), 2)) + (Math.Pow((y - Y), 2) / Math.Pow(h, 2));
                if (per <= 1)
                {
                    selected = 1;
                    return true;
                }
                else return false;
            }

            public override bool prov_exit(int X, int Y) 
            {
                if (X < 800 - w && X > w && Y > h && Y < 424 - h)
                    return true;
                else
                    return false;
            }

            public override bool prov_frame(int X, int Y)
            {
                if (x - 6 <= X && X <= x + 6 && (y - h) - 6 <= Y && Y <= (y - h) + 6)//1
                    return true;
                else if ((x - w) - 6 <= X && X <= (x - w) + 6 && y - 6 <= Y && Y <= y + 6)//2
                    return true;
                else if ((x + w) - 6 <= X && X <= (x + w) + 6 && (y) - 6 <= Y && Y <= (y+6))//3
                    return true;
                else if ((x) - 6 <= X && X <= (x+6) && (y + w) - 6 <= Y && Y <= (y + w)+6)//4
                    return true;
                else return false;
            }

            public override void set_coords(int X, int Y, int Xd, int Yd)
            {
                if (x - 6 <= X && X <= x + 6 && (y - h) - 6 <= Y && Y <= (y - h) + 6)//1
                    h += Y - Yd;
                else if ((x - w) - 6 <= X && X <= (x - w) + 6 && y - 6 <= Y && Y <= y + 6)//2
                    w += X - Xd;
                else if ((x + w) - 6 <= X && X <= (x + w) + 6 && (y) - 6 <= Y && Y <= (y + 6))//3
                    w += Xd - X;
                else if ((x) - 6 <= X && X <= (x + 6) && (y + w) - 6 <= Y && Y <= (y + w) + 6)//4
                    h += Yd - Y;
            }
        }

        class Triangle : Object
        {
            public int a = 40, b = 40, c = 40, d = 40;

            PointF[] points = new PointF[3];
            public Triangle(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public override void draw(Graphics gr)
            {
                points[0].X = x; points[0].Y = y - a;
                points[1].X = x - c; points[1].Y = y+b;
                points[2].X = x + d; points[2].Y = y+b;
                gr.DrawPolygon(blackPen, points);
                gr.FillPolygon(brw, points);
                if (selected == 1)
                    draw_frame(gr);  
            }

            public override void draw_frame(Graphics gr)
            {
                gr.DrawRectangle(bluePen, x-c, y-a, c+d, a+b);

                gr.DrawRectangle(redPen, x - 6, (y - a) - 6, 12, 12);
                gr.DrawRectangle(redPen, (x - c) - 6, y - 6, 12, 12);
                gr.DrawRectangle(redPen, (x + d) - 6, (y) - 6, 12, 12);
                gr.DrawRectangle(redPen, (x) - 6, (y + b) - 6, 12, 12);
            }

            public override bool prov_(int X, int Y)
            {
                if (Y > y - a && Y < y + b && X > x - c && X < x + d)
                {/*((Y < 2 * X && X < x) || (Y < 0.5 * X && X > x))*/
                    return true;
                }
                else return false;
            }

            public override bool prov_exit(int X, int Y)
            {
                if (X < 800 - 40 && X > 40 && Y > 40 && Y < 424 - 40)
                    return true;
                else
                    return false;
            }

            public override bool prov_frame(int X, int Y)
            {
                if (x - 6 <= X && X <= x + 6 && (y - a) - 6 <= Y && Y <= (y - a) + 6)//1
                    return true;
                else if ((x - c) - 6 <= X && X <= (x - c) + 6 && y - 6 <= Y && Y <= y + 6)//2
                    return true;
                else if ((x + d) - 6 <= X && X <= (x + d) + 6 && (y) - 6 <= Y && Y <= (y + 6))//3
                    return true;
                else if ((x) - 6 <= X && X <= (x + 6) && (y + b) - 6 <= Y && Y <= (y + b) + 6)//4
                    return true;
                else return false;
            }

            public override void set_coords(int X, int Y, int Xd, int Yd)
            {
                if (x - 6 <= X && X <= x + 6 && (y - a) - 6 <= Y && Y <= (y - a) + 6)//1
                    a += Y - Yd;
                else if ((x - c) - 6 <= X && X <= (x - c) + 6 && y - 6 <= Y && Y <= y + 6)//2
                    c += X - Xd;
                else if ((x + d) - 6 <= X && X <= (x + d) + 6 && (y) - 6 <= Y && Y <= (y + 6))//3
                    d += Xd - X;
                else if ((x) - 6 <= X && X <= (x + 6) && (y + b) - 6 <= Y && Y <= (y + b) + 6)//4
                    b += Yd - Y;
            }
        }


        class Rectangle : Object
        {
            public int a = 100, b = 60;
            public Rectangle(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public override void draw(Graphics gr)
            {
                gr.FillRectangle(brw, x-a/2, y-b/2, a, b);
                gr.DrawRectangle(blackPen, x - a / 2, y - b / 2, a, b);
                if (selected == 1)
                    draw_frame(gr);
            }

            public override void draw_frame(Graphics gr)
            {
                gr.DrawRectangle(bluePen, x - a / 2, y - b / 2, a, b);

                gr.DrawRectangle(redPen, x - 6, (y - b/2) - 6, 12, 12);
                gr.DrawRectangle(redPen, (x - a/2) - 6, y - 6, 12, 12);
                gr.DrawRectangle(redPen, (x + a/2) - 6, (y) - 6, 12, 12);
                gr.DrawRectangle(redPen, (x) - 6, (y + b/2) - 6, 12, 12);
            }
            public override bool prov_(int X, int Y)
            {
                if (x-a/2 <= X && X <= x-a/2 + a && y-b/2 <= Y && Y <= y-b/2 + b)
                {
                    selected = 1;
                    return true;
                }
                else return false;
            }
            public override bool prov_exit(int X, int Y)
            {
                if (X < 800 - a / 2 && X > a / 2 && Y > b / 2 && Y < 424 - b / 2)
                    return true;
                else
                    return false;
            }

            public override bool prov_frame(int X, int Y)
            {
                if (x - 6 <= X && X <= x + 6 && (y - b/2) - 6 <= Y && Y <= (y - b/2) + 6)//1
                    return true;
                else if ((x - a/2) - 6 <= X && X <= (x - a/2) + 6 && y - 6 <= Y && Y <= y + 6)//2
                    return true;
                else if ((x + a/2) - 6 <= X && X <= (x + a/2) + 6 && (y) - 6 <= Y && Y <= (y + 6))//3
                    return true;
                else if ((x) - 6 <= X && X <= (x + 6) && (y + b/2) - 6 <= Y && Y <= (y + b/2) + 6)//4
                    return true;
                else return false;
            }

            public override void set_coords(int X, int Y, int Xd, int Yd)
            {
                if (x - 6 <= X && X <= x + 6 && (y - b/2) - 6 <= Y && Y <= (y - b/2) + 6)//1
                    b += 2*(Y - Yd);
                else if ((x - a/2) - 6 <= X && X <= (x - a/2) + 6 && y - 6 <= Y && Y <= y + 6)//2
                    a += 2*(X - Xd);
                else if ((x + a/2) - 6 <= X && X <= (x + a/2) + 6 && (y) - 6 <= Y && Y <= (y + 6))//3
                    a += 2*(Xd - X);
                else if ((x) - 6 <= X && X <= (x + 6) && (y + b/2) - 6 <= Y && Y <= (y + b/2) + 6)//4
                    b += 2*(Yd - Y);
            }
        }

        class Storage<T>
        {
            private class Node
            {
                public Node pNext;
                public T data;
                public Node(T data_, Node pNext_ = null)
                {
                    data = data_;
                    pNext = pNext_;
                }
            };

            private Node arr;
            private int size;
            private Node iteratorElement;
            public Storage()
            {
                size = 0;
            }

            public int get_size()
            {
                return size;
            }
            public T getObject(int index)
            {
                if (index > size || index < 0) return default(T);
                Node tmp = arr;
                for (int i = 0; i < index; i++)
                    tmp = tmp.pNext;
                return tmp.data;
            }
            public void push_back(T data_)
            {
                if (arr == null)
                    arr = new Node(data_);
                else
                {
                    Node tmp = arr;

                    while (tmp.pNext != null)
                        tmp = tmp.pNext;

                    tmp.pNext = new Node(data_);
                }
                size++;
            }
            public void remove(int index)
            {
                if (index > size || index < 0) return;
                size--;
                if (index == 0)
                {
                    arr = arr.pNext;
                    //delete tmp;
                    return;
                }

                Node previous = arr;

                for (int i = 0; i < index - 1; i++)
                    previous = previous.pNext;

                Node toDelete = previous.pNext;
                previous.pNext = toDelete.pNext;
                //delete toDelete;
            }
        }

        


        Bitmap bitmap;
        Graphics gr;

        
        public Form1()
        {
            InitializeComponent();
            V = new Storage<Object>();
            bitmap = new Bitmap(sheet.Width, sheet.Height);
            gr = Graphics.FromImage(bitmap);
            sheet.Image = bitmap;
            clearSheet(gr);
        }

        Storage<Object> V;
        Object perobj0;

        int per, but=1;
        int Xd, Yd;
        bool myKeysPressed;

        private void sheet_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && myKeysPressed == true)
            {

                for (int i = V.get_size() - 1; i >= 0; i--)
                {
                    perobj0 = V.getObject(i);
                    if (perobj0.prov_(e.X, e.Y))
                        perobj0.selected = 1;
                }
                clearSheet(gr);
                DrawALL(gr);
                sheet.Image = bitmap;

            }
            else if (e.Button == MouseButtons.Left)
            {
                bool abc;
                Xd = e.X;
                Yd = e.Y;
                
                for (int i = 0; i < V.get_size(); i++)
                {
                    perobj0 = V.getObject(i);
                    abc = perobj0.prov_frame(Xd, Yd);
                    if (abc == true)
                        return;
                }

                per = provALL(e.X, e.Y);
                if (per != -1)
                {
                    clearSheet(gr);
                    selectObj(per);
                    DrawALL(gr);
                    sheet.Image = bitmap;
                }
                else
                {
                    if (but == 1)
                        V.push_back(new Vertex(e.X, e.Y));
                    else if (but == 3)
                        V.push_back(new Rectangle(e.X, e.Y));
                    else if (but == 2)
                        V.push_back(new Triangle(e.X, e.Y));

                    DrawALL(gr);
                    sheet.Image = bitmap;
                }
                
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
                удалениеToolStripMenuItem_Click(null, null);
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            myKeysPressed = false;
        }

        private void треугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            but = 2;
        }

        private void прямоугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            but = 3;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Object perObj;
            myKeysPressed = e.Shift;
            for (int i = 0; i < V.get_size(); i++) 
            {
                perObj = V.getObject(i);
                if (perObj.selected == 1)
                {
                    if (e.KeyCode == Keys.Up)
                    {
                        if (perObj.prov_exit(perObj.x, perObj.y - 10))
                            perObj.y -= 10;
                    }
                    else if (e.KeyCode == Keys.Down)
                    {
                        if (perObj.prov_exit(perObj.x, perObj.y + 10))
                            perObj.y += 10;
                    }
                    else if (e.KeyCode == Keys.Right)
                    {
                        if (perObj.prov_exit(perObj.x + 10, perObj.y))
                            perObj.x += 10;
                    }
                    else if (e.KeyCode == Keys.Left)
                    {
                        if (perObj.prov_exit(perObj.x - 10, perObj.y))
                            perObj.x -= 10;
                    }
                }
                clearSheet(gr);
                DrawALL(gr);
                sheet.Image = bitmap;
            }
        }

        private void красныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Object per;
            for (int i = V.get_size() - 1; i >= 0; i--)
            {
                per = V.getObject(i);
                if (per.selected == 1)
                    per.brw = Brushes.Red;
            }
            clearSheet(gr);
            DrawALL(gr);
            sheet.Image = bitmap;
        }
        private void синийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Object per;
            for (int i = V.get_size() - 1; i >= 0; i--)
            {
                per = V.getObject(i);
                if (per.selected == 1)
                    per.brw = Brushes.Blue;
            }
            clearSheet(gr);
            DrawALL(gr);
            sheet.Image = bitmap;
        }

        private void зелёныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Object per;
            for (int i = V.get_size() - 1; i >= 0; i--)
            {
                per = V.getObject(i);
                if (per.selected == 1)
                    per.brw = Brushes.Green;
            }
            clearSheet(gr);
            DrawALL(gr);
            sheet.Image = bitmap;
        }

        private void белыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Object per;
            for (int i = V.get_size() - 1; i >= 0; i--)
            {
                per = V.getObject(i);
                if (per.selected == 1)
                    per.brw = Brushes.White;
            }
            clearSheet(gr);
            DrawALL(gr);
            sheet.Image = bitmap;
        }

        private void sheet_MouseUp(object sender, MouseEventArgs e)
        {
            bool abc;
            for (int i = 0; i < V.get_size(); i++)
            {
                perobj0 = V.getObject(i);
                if (perobj0.prov_frame(Xd, Yd)) 
                {
                    perobj0.set_coords(Xd, Yd, e.X, e.Y);
                    clearSheet(gr);
                    DrawALL(gr);
                    sheet.Image = bitmap;
                    return;
                }
            }
        }

        private void удалениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Object per;
            for (int i = V.get_size() - 1; i >= 0; i--)
            {
                per = V.getObject(i);
                if (per.selected == 1)
                    V.remove(i);
            }
            clearSheet(gr);
            DrawALL(gr);
            sheet.Image = bitmap;
        }

        private void кругToolStripMenuItem_Click(object sender, EventArgs e)
        {
            but = 1;
        }
    }
}
