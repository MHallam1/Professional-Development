using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digital_Canvas
{
    public partial class Canvas : Form
    {
        //creating 
        Graphics graphic;
        int x = -1;
        int y = -1;
        bool moving = false;
        Pen pencil;
        public Canvas()
        {
            InitializeComponent();
            graphic = splitContainer1.Panel2.CreateGraphics();
            pencil = new Pen(Color.Black,2);
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pencil.StartCap = System.Drawing.Drawing2D.LineCap.Round; // makes the start of line  rounded
            pencil.EndCap = System.Drawing.Drawing2D.LineCap.Round;

        }

        private void red_Click(object sender, EventArgs e)
        {
            PictureBox colours = (PictureBox)sender;
            pencil.Color = colours.BackColor; // sets the selected colour
        }

        private void splitContainer1_Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            moving = true;
            x = e.X; // gets mouse x location
            y = e.Y; // gets mouse  y location
            splitContainer1.Panel2.Cursor = Cursors.Cross;
        }

        private void splitContainer1_Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (moving && x!=-1 && y != -1)
            {
                graphic.DrawLine(pencil, new Point(x, y), e.Location);
                //update the x and y to draw as intended
                x = e.X; 
                y = e.Y;
            }
        }

        private void splitContainer1_Panel2_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
            x = -1;
            x = -1;
            splitContainer1.Panel2.Cursor = Cursors.Default;
        }
    }
}
