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
        int cursorX = -1;
        int cursorY= -1;
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
            cursorX = e.X; // gets mouse X location
            cursorY = e.Y; // gets mouse Y location
            splitContainer1.Panel2.Cursor = Cursors.Cross;
        }

        private void splitContainer1_Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (moving && cursorX != -1 && cursorY != -1)
            {
                graphic.DrawLine(pencil, new Point(cursorX, cursorY), e.Location);
                //update the cursorX and cursorY to draw as intended
               cursorX= e.X; 
               cursorY= e.Y;
            }
        }

        private void splitContainer1_Panel2_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
           cursorX= -1;
           cursorX= -1;
            splitContainer1.Panel2.Cursor = Cursors.Default;
        }
    }
}
