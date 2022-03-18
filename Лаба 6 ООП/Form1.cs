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

        class Frame : Object
        {
            public int a = 50, b = 30;
            public Frame(int x, int y, int a, int b)
            {
                this.x = x;
                this.y = y;
                this.a = a;
                this.b = b;
            }

            public override void draw(Graphics gr)
            {
                gr.DrawRectangle(redPen, x - a, y - b, 2 * a, 2 * b);
            }

            public override bool prov_(int X, int Y)
            {
                if (x - a <= X && X <= x - a + 2 * a && y - b <= Y && Y <= y - b + 2 * b)
                {
                    selected = 1;
                    return true;
                }
                else return false;
            }
        }
        class Object
        {
            public int x, y, selected = 0;

            public Pen blackPen;
            public Pen redPen;
            public Pen bluePen;
            public Brush br, brw;
            public Frame fr1, fr2;
            public Object()
            {
                blackPen = new Pen(Color.Black);
                blackPen.Width = 3;
                redPen = new Pen(Color.Red);
                redPen.Width = 3;
                bluePen = new Pen(Color.BlueViolet);
                bluePen.Width = 3;
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

        class Group : Object
        {
            public Storage<Object> objects;
            
            public int per, _x = 0, _y = 0, max = 0, min = 0;
            public Group()
            {
                objects = new Storage<Object>();
            }


            public void addshape(Object per)
            {
                objects.push_back(per);
            }
            public override void draw(Graphics gr)
            {
                Object perObj;
                if (x != 0 || y != 0)
                {
                    for (int i = 0; i < objects.get_size(); i++)
                    {
                        perObj = objects.getObject(i);
                        perObj.x += x;
                        perObj.y += y;
                    }
                    x = 0; y = 0;
                }
                if (selected == 1)
                {
                    for (int i = 0; i < objects.get_size(); i++)
                    {
                        perObj = objects.getObject(i);
                        perObj.selected = 1;
                        perObj.draw(gr);
                    }
                }
                else
                    for (int i = 0; i < objects.get_size(); i++)
                    {
                        perObj = objects.getObject(i);
                        perObj.selected = 0;
                        perObj.draw(gr);
                    }
            }

            public override bool prov_(int X, int Y)
            {
                Object perObj;
                for (int i = 0; i < objects.get_size(); i++)
                {
                    perObj = objects.getObject(i);
                    if (perObj.prov_(X, Y))
                    {
                        selected = 1;
                        return true;
                    }
                }
                return false;
            }

            public override bool prov_exit(int X, int Y)
            {
                Object perObj;
                for (int i = 0; i < objects.get_size(); i++)
                {
                    perObj = objects.getObject(i);
                    if (!perObj.prov_exit(X, Y))
                    {
                        return false;
                    }
                }
                return true;
            }


        }

        class Vertex : Object
        {
            public int w = 30, h = 30, R = 30;

            public Vertex(int x, int y)
            {
                this.x = x;
                this.y = y;
                fr1 = new Frame(x, y, w + 10, h + 10);
                fr2 = new Frame(x, y, w - 10, h - 10);
            }

            public override void draw(Graphics gr)
            {
                gr.FillEllipse(brw, (x - w), (y - h), w * 2, h * 2);
                
                if (selected == 1)
                    gr.DrawEllipse(redPen, (x - w), (y - h), w * 2, h * 2);
                else
                    gr.DrawEllipse(blackPen, (x - w), (y - h), w * 2, h * 2);

            }

            public override void draw_frame(Graphics gr)
            {
                fr1.draw(gr);
                fr2.draw(gr);
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
                if (x+X < 800 - w && x+X > w && y+Y > h && y+Y < 424 - h)
                    return true;
                else
                    return false;
            }

            public override bool prov_frame(int X, int Y)
            {
                if (fr1.prov_(X, Y) && !fr2.prov_(X, Y))
                    return true;
                else
                    return false;
            }

            public override void set_coords(int X, int Y, int Xd, int Yd)
            {

            }
        }

        class Rectangle : Object
        {
            public int a = 50, b = 30;
            public Rectangle(int x, int y)
            {
                this.x = x;
                this.y = y;
                fr1 = new Frame(x, y, a + 10, b + 10);
                fr2 = new Frame(x, y, a - 10, b - 10);
            }

            public override void draw(Graphics gr)
            {
                gr.FillRectangle(brw, x - a, y - b, 2 * a, 2 * b);
                gr.DrawRectangle(blackPen, x - a, y - b, 2 * a, 2 * b);
                if (selected == 1)
                    gr.DrawRectangle(redPen, x - a, y - b, 2 * a, 2 * b);
                else
                    gr.DrawRectangle(blackPen, x - a, y - b, 2 * a, 2 * b);

            }

            public override void draw_frame(Graphics gr)
            {
                fr1.draw(gr);
                fr2.draw(gr);
            }
            public override bool prov_(int X, int Y)
            {
                if (x - a <= X && X <= x - a + 2 * a && y - b <= Y && Y <= y - b + 2 * b)
                {
                    selected = 1;
                    return true;
                }
                else return false;
            }
            public override bool prov_exit(int X, int Y)
            {
                if (x+X < 800 - a && x+X > a && y+Y > b && y+Y < 424 - b)
                    return true;
                else
                    return false;
            }

            public override bool prov_frame(int X, int Y)
            {
                if (fr1.prov_(X, Y) && !fr2.prov_(X, Y))
                    return true;
                else
                    return false;
            }

            public override void set_coords(int X, int Y, int Xd, int Yd)
            {

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
        int per, but = 1;
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
                    selectObj(per);
                }
                else
                {
                    if (but == 1)
                        V.push_back(new Vertex(e.X, e.Y));
                    else if (but == 3)
                        V.push_back(new Rectangle(e.X, e.Y));

                }
                clearSheet(gr);
                DrawALL(gr);
                sheet.Image = bitmap;

            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            myKeysPressed = false;
        }
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
                int per = provALL_ver2();
                if (per != -1)
                {
                    V.remove(per);
                    clearSheet(gr);
                    DrawALL(gr);
                    sheet.Image = bitmap;
                }
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                bool flag = false;
                Group per = new Group();
                for (int i = 0; i < V.get_size(); i++)
                {
                    perobj0 = V.getObject(i);
                    if (perobj0.selected == 1)
                    {
                        flag = true;
                        per.addshape(perobj0);
                        V.remove(i);
                        i--;
                    }
                }
                if (flag == true)
                    V.push_back(per);

            }
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
            myKeysPressed = e.Shift;
            int per = provALL_ver2();
            if (per != -1)
            {
                Object perObj = V.getObject(per);
                if (e.KeyCode == Keys.Up)
                {
                    if (perObj.prov_exit(0, -10))
                        perObj.y -= 10;
                    else
                        MessageBox.Show("Объект выходит за границы");
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (perObj.prov_exit(0,10))
                        perObj.y += 10;
                    else
                        MessageBox.Show("Объект выходит за границы");
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (perObj.prov_exit(10,0))
                        perObj.x += 10;
                    else
                        MessageBox.Show("Объект выходит за границы");
                }
                else if (e.KeyCode == Keys.Left)
                {
                    if (perObj.prov_exit(-10,0))
                        perObj.x -= 10;
                    else
                        MessageBox.Show("Объект выходит за границы");
                }
            }
            clearSheet(gr);
            DrawALL(gr);
            sheet.Image = bitmap;
        }

        private void красныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int per = provALL_ver2();
            if (per != -1)
            {
                Object perObj = V.getObject(per);
                perObj.brw = Brushes.Red;
                clearSheet(gr);
                DrawALL(gr);
                sheet.Image = bitmap;
            }
        }
        private void синийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int per = provALL_ver2();
            if (per != -1)
            {
                Object perObj = V.getObject(per);
                perObj.brw = Brushes.Blue;
                clearSheet(gr);
                DrawALL(gr);
                sheet.Image = bitmap;
            }
        }

        private void зелёныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int per = provALL_ver2();
            if (per != -1)
            {
                Object perObj = V.getObject(per);
                perObj.brw = Brushes.Green;
                clearSheet(gr);
                DrawALL(gr);
                sheet.Image = bitmap;
            }
        }

        private void белыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int per = provALL_ver2();
            if (per != -1)
            {
                Object perObj = V.getObject(per);
                perObj.brw = Brushes.White;
                clearSheet(gr);
                DrawALL(gr);
                sheet.Image = bitmap;
            }
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
            int per = provALL_ver2();
            if (per != -1)
            {
                V.remove(per);
                clearSheet(gr);
                DrawALL(gr);
                sheet.Image = bitmap;
            }
        }

        private void кругToolStripMenuItem_Click(object sender, EventArgs e)
        {
            but = 1;
        }
    }
}
