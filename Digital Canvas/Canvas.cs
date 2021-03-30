using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digital_Canvas
{
    


    public partial class Canvas : Form 
    {
        //Reference to graphics object.
        Graphics graphic;

        //This ensures that anything the mouse is doing doesn't effect the panel
        int cursorX = -1;
        int cursorY= -1;

        //When you press the mouse down this flag is set to true so that the move mouse button functions
        bool moving = false;
    
        Pen pencil; //Pen Object Reference
        Color colour = Color.Black; //Pen starting colour
        Color colourBkg = Color.White; //Background colour used in panel
        int size = 10; //Pen starting size

        ColorDialog diag;
        Bitmap bmap;


        int defaultHeight = 600; //Starting Canvas Dimensions
        int defaultWidth = 800;
        public Canvas()
        {
            diag = new ColorDialog();
            InitializeComponent();
            txtSizebox.Text = size.ToString();
            size = Int32.Parse(txtSizebox.Text);

            graphic = splitContainer1.Panel2.CreateGraphics();

            pencil = new Pen(colour, size);
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pencil.StartCap = System.Drawing.Drawing2D.LineCap.Round; // makes the start of line  rounded
            pencil.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            btnPencil.BackColor = Color.LightGreen;

            bmap = new Bitmap(splitContainer1.Panel2.Width, splitContainer1.Panel2.Height);
            // splitContainer1.Panel2.MouseMove += splitContainer1_Panel2_MouseMove;
            //splitContainer1.Panel2.Paint += splitContainer1_Panel2_Paint;
             
            typeof(SplitContainer).InvokeMember("DoubleBuffered",
                 BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, splitContainer1.Panel2,
                 new object[] { true });

        }
   

        private void Canvas_Load(object sender, EventArgs e)// occurs when the application is run/code is run
        {
            Size = new Size(defaultWidth, defaultHeight);
        }
        
        //Colour Box Code
        private void red_Click(object sender, EventArgs e)// this is the colour box method, deals with changing colour
        {
            //colour selector method
            PictureBox colours = (PictureBox)sender;
            colours.Invalidate();
           // pencil.Color = colours.BackColor; // sets the selected colour
           // colour = colours.BackColor;

                if (diag.ShowDialog() == DialogResult.OK)
                {
                    pencil.Color = diag.Color;
                    colours.BackColor = diag.Color;
                }
        }

        private void splitContainer1_Panel2_MouseDown(object sender, MouseEventArgs e) // this event runs when the mouse button(left) is pressed/held down
        {
            moving = true;
            cursorX = e.X; // gets mouse X location
            cursorY = e.Y; // gets mouse Y location
            splitContainer1.Panel2.Cursor = Cursors.Cross;
        }

        private void splitContainer1_Panel2_MouseMove(object sender, MouseEventArgs e) // this event runs when the mouse button(left) is moved but will only run if mouse button is down due to boolean flag
        {
            // graphic = splitContainer1.Panel2.CreateGraphics();// updates graphic size when window size changes

            // use this the line below when needed to do something with Graphics , This will use e.graphics which is much appropriate
            using (Graphics g = Graphics.FromImage(bmap)) // draws on the bitmap
            {
                if (moving && cursorX != -1 && cursorY != -1)
                {
               
                    g.DrawLine(pencil, new Point(cursorX, cursorY), e.Location);
                    //update the cursorX and cursorY to draw as intended
                   cursorX= e.X; 
                   cursorY= e.Y;
                }
                
                //splitContainer1.Panel2.do();
            }
            splitContainer1.Panel2.Invalidate();


        }

        private void splitContainer1_Panel2_MouseUp(object sender, MouseEventArgs e) // occurs when mouse button is not being pressed (when you let go of it)
        {
            moving = false;
            cursorX= -1;
            cursorX= -1;
            splitContainer1.Panel2.Cursor = Cursors.Default;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) // this is the new option form drop down
        {
            graphic.Clear(colourBkg);
        }

        private void fillButton_Click(object sender, EventArgs e)// customisation of the fill button
        {
            btnPencil.BackColor = System.Drawing.Color.Transparent;
            btnEraser.BackColor = System.Drawing.Color.Transparent;
            btnFill.BackColor = Color.LightGreen;
        }

        private void sizebox_TextChanged(object sender, EventArgs e)// select the size of brush
        {
            int value = 0;
            if(int.TryParse(txtSizebox.Text, out value)==false)
            {
            }
            else
            {
                size = Int32.Parse(txtSizebox.Text);
                pencil.Width = size;
            }
        }

        private void eraserButton_Click_1(object sender, EventArgs e) //code and customisation of erasser button
        {
            pencil.Color = colourBkg; // sets to erase
            btnPencil.BackColor = System.Drawing.Color.Transparent;
            btnEraser.BackColor = Color.LightGreen;
            btnFill.BackColor = System.Drawing.Color.Transparent;
        }

        private void pencilButton_Click_1(object sender, EventArgs e)// code for pencil button 
        {
            pencil.Color = diag.Color; // sets the selected colour
            btnPencil.BackColor = Color.LightGreen;
            btnEraser.BackColor = System.Drawing.Color.Transparent;
            btnFill.BackColor = System.Drawing.Color.Transparent;
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // need to figure out how to save it as a new file

          
            bmap.Save(@"### PATH FROM A DRIVE###\bmp3.bmp");
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)//  this is the larger part of the split container 
        {
            // Using this to create drawing unlike before to enable us to save drawings
            Graphics g = e.Graphics;
            g.DrawImage(bmap, Point.Empty);
        }

        private void resizeCanvasToolStripMenuItem_Click(object sender, EventArgs e)// option form the menustrip 
        {
            //splitContainer1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            // resize to default
            Size = new Size(defaultWidth, defaultHeight);
           // splitContainer1.pane = 300;//new System.Drawing.Size(300, 300);
            bmap = new Bitmap(splitContainer1.Panel2.Width, splitContainer1.Panel2.Height);




        }

        private void btnChangeSize_Click(object sender, EventArgs e) // changing size of the canvas
        {
            // width of canvas differnce from window size is 112 and height diffence is 62
            int formWidth;
            int formHeight;
            int widthDiffernce = 112;
            int heightDiffernce = 62;
            Canvas canvas = new Canvas();
            if ((txtWidth != null && txtHeight != null) && (Int32.TryParse(txtWidth.Text, out formWidth) && Int32.TryParse(txtHeight.Text, out formHeight)))
            {
                if(formHeight < canvas.MinimumSize.Height || formWidth < canvas.MinimumSize.Width)
                {
                    MessageBox.Show("Minimum Height and Width are: " + canvas.MinimumSize.Height + " & " + canvas.MinimumSize.Width + " respectively.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }

               else if (MessageBox.Show("Resizing Canvas will erase all work on canvas. Do you wish to continue?", "Alert", (MessageBoxButtons.YesNo),MessageBoxIcon.Question ) == DialogResult.Yes)
               {
                   splitContainer1.Anchor = (AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top);
                   Size = new Size(formWidth + widthDiffernce, formWidth+ heightDiffernce);//give exact canvas size as required
                   
                   bmap = new Bitmap(splitContainer1.Panel2.Width, splitContainer1.Panel2.Height);
                   splitContainer1.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                   // Console.WriteLine("###############" + splitContainer1.Panel2.Width + splitContainer1.Panel2.Height);
                }
            }
            else
            {
                MessageBox.Show("Invalid Canvas Dimensions", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }
    }
}
