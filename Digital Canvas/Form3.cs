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
        Color colour = Color.Black;
        Color colourBkg = Color.White;
        int size = 10;
        public Canvas()
        {
            InitializeComponent();
            sizebox.Text = size.ToString();
            size = Int32.Parse(sizebox.Text);
            graphic = splitContainer1.Panel2.CreateGraphics();
            pencil = new Pen(colour, size);
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pencil.StartCap = System.Drawing.Drawing2D.LineCap.Round; // makes the start of line  rounded
            pencil.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            pencilButton.BackColor = Color.LightGreen;

        }

        private void red_Click(object sender, EventArgs e)
        {
            PictureBox colours = (PictureBox)sender;
            pencil.Color = colours.BackColor; // sets the selected colour
            colour = colours.BackColor;
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
                //update thecursorXandcursorYto draw as intended
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphic.Clear(colourBkg);
        }

        private void fillButton_Click(object sender, EventArgs e)
        {
            pencilButton.BackColor = System.Drawing.Color.Transparent;
            eraserButton.BackColor = System.Drawing.Color.Transparent;
            fillButton.BackColor = Color.LightGreen;
        }

        private void sizebox_TextChanged(object sender, EventArgs e)
        {
            int value = 0;
            if(int.TryParse(sizebox.Text, out value)==false)
            {
            }
            else
            {
                size = Int32.Parse(sizebox.Text);
                pencil.Width = size;
            }
        }

        private void eraserButton_Click_1(object sender, EventArgs e)
        {
            pencil.Color = colourBkg; // sets to erase
            pencilButton.BackColor = System.Drawing.Color.Transparent;
            eraserButton.BackColor = Color.LightGreen;
            fillButton.BackColor = System.Drawing.Color.Transparent;
        }

        private void pencilButton_Click_1(object sender, EventArgs e)
        {
            pencil.Color = colour; // sets the selected colour
            pencilButton.BackColor = Color.LightGreen;
            eraserButton.BackColor = System.Drawing.Color.Transparent;
            fillButton.BackColor = System.Drawing.Color.Transparent;
        }
    }
}
