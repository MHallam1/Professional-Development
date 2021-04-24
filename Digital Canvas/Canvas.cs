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
        SaveFileDialog saveDialog = new SaveFileDialog();
        //This ensures that anything the mouse is doing doesn't effect the panel
        int cursorX = -1;
        int cursorY = -1;
        int shapeX ;
        int shapeY ;
        Single x ;
        Single y ;

        //When you press the mouse down this flag is set to true so that the move mouse button functions
        bool moving = false;

        Color colour = Color.Black; //Pen starting colour
        Color colourBkg = Color.White; //Background colour used in panel
        int size = 10; //Pen starting size
        Pen pencil, brush, eraser, rect,ellipse,line;

        ColorDialog diag;
        Bitmap bmap;
        String fileTypeDefaultSave = "bmp";

        Color tempColour;

        int defaultHeight = 600; //Starting Canvas Dimensions
        int defaultWidth = 800;
        

        //Flag for bucket tool
        

        public Canvas()
        {
            diag = new ColorDialog(); //Initialised ColorDialog for diag
            InitializeComponent(); //Runs the form

            txtSizebox.Text = size.ToString(); //Takes the default text and converts it to string to display in textbox

            pencil = new Pen(colour, size);
            brush = new Pen(colour, size);
            eraser = new Pen(colour, size);
            rect = new Pen(colour, size);
            line = new Pen(colour, size);
            ellipse = new Pen(colour, size);
            SolidBrush brushFill = new SolidBrush(Color.Blue);

            pencil.StartCap = System.Drawing.Drawing2D.LineCap.Round; // makes the start of line rounded
            pencil.EndCap = System.Drawing.Drawing2D.LineCap.Round; //makes the end of the line rounded
            brush.StartCap = System.Drawing.Drawing2D.LineCap.Round; // makes the start of line rounded
            brush.EndCap = System.Drawing.Drawing2D.LineCap.Round; //makes the end of the line rounded
            eraser.StartCap = System.Drawing.Drawing2D.LineCap.Round; // makes the start of line rounded
            eraser.EndCap = System.Drawing.Drawing2D.LineCap.Round; //makes the end of the line rounded

            brush.Color = colour;
            eraser.Color = colourBkg;
            rect.Color = colour;

            bmap = new Bitmap(splitContainer1.Panel2.Width, splitContainer1.Panel2.Height);

            //Prevents flickering whilst drawing
            typeof(SplitContainer).InvokeMember("DoubleBuffered",
                 BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, splitContainer1.Panel2,
                 new object[] { true });

        }


        private void Canvas_Load(object sender, EventArgs e)//occurs when the application is run/code is run
        {
            refresh();
            btnPencil_Click(sender, e);
            btnPencil.BackColor= Color.LightGreen;
            Size = new Size(defaultWidth, defaultHeight); //Sets width and height of the application 

        }

        //Colour Box Code
        private void colourSelect_Click(object sender, EventArgs e)
        {
            //colour selector method
            PictureBox colours = (PictureBox)sender;
            colours.Invalidate();
            // pencil.Color = colours.BackColor; // sets the selected colour
            // colour = colours.BackColor;

            if (diag.ShowDialog() == DialogResult.OK)
            {
                colours.BackColor = diag.Color;

                colour = diag.Color;
                brush.Color = diag.Color;
            }
        }

        private void splitContainer1_Panel2_MouseDown(object sender, MouseEventArgs e) // this event runs when the mouse button(left) is pressed/held down
        {
            moving = true;
            cursorX = e.X; // gets mouse X location
            cursorY = e.Y; // gets mouse Y location
            shapeX = e.X;
            shapeY = e.Y;
             x = e.X;
             y = e.Y;
            splitContainer1.Panel2.Cursor = Cursors.Cross;
        }

        private void splitContainer1_Panel2_MouseMove(object sender, MouseEventArgs e) // this event runs when the mouse button(left) is moved but will only run if mouse button is down due to boolean flag
        {
            // use this the line below when needed to do something with Graphics , This will use e.graphics which is much appropriate
            using (Graphics g = Graphics.FromImage(bmap)) // draws on the bitmap
            {
                if (e.Button == MouseButtons.Left && moving && cursorX != -1 && cursorY != -1 && !fillSelected)
                {
                    if (pencil == rect)
                    {
                        SolidBrush sb = new SolidBrush(colourBkg);
                        //SolidBrush sb2 = new SolidBrush(Color.Blue);
                        //Pen pencil2 = new Pen(Color.Red);

                        g.DrawRectangle(pencil, new Rectangle(cursorX, cursorY, e.X- shapeX,e.Y-shapeY));
                        //g.FillRectangle(sb2, new Rectangle(cursorX-size, cursorY-size, e.X - shapeX+size, e.Y - shapeY+size));
                        g.FillRectangle(sb, new Rectangle(cursorX, cursorY, e.X - shapeX, e.Y - shapeY));
                       // g.DrawRectangle(pencil2, new Rectangle(cursorX - size, cursorY - size, e.X - shapeX + size, e.Y - shapeY + size));
                    }
                    else if(pencil == ellipse)
                    {
                        // SolidBrush sb = new SolidBrush(colourBkg);

                        //g.DrawEllipse(pencil, new RectangleF( x, y, e.X - shapeX, e.Y - shapeY));
                        // g.FillEllipse(sb, new Rectangle(cursorX, cursorY, e.X - shapeX, e.Y - shapeY));
                        // g.FillEllipse(sb, new Rectangle(cursorX, cursorY, e.X - shapeX, e.Y - shapeY));
                        Pen guidePen = new Pen(colour, 1);
                        guidePen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                        guidePen.StartCap = System.Drawing.Drawing2D.LineCap.Square;
                        g.DrawLine(guidePen, new Point(cursorX, cursorY), new Point(cursorX, e.Y));
                        g.DrawLine(guidePen, new Point(cursorX, cursorY), new Point(e.X, cursorY));

                    }
                    else if (pencil == line)
                    {
                        Pen guidePen = new Pen(colour, 1);
                        guidePen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                        guidePen.StartCap = System.Drawing.Drawing2D.LineCap.Square;
                        g.DrawLine(guidePen, new Point(cursorX, cursorY), new Point(cursorX, e.Y));
                        g.DrawLine(guidePen, new Point(cursorX, cursorY), new Point(e.X, cursorY));
                    }
                    else
                    {
                        g.DrawLine(pencil, new Point(cursorX, cursorY), e.Location);
                        //update the cursorX and cursorY to draw as intended
                        cursorX = e.X;
                        cursorY = e.Y;
                    }
                }

                //splitContainer1.Panel2.do();
            }
            splitContainer1.Panel2.Invalidate();


        }

        
        //This is called when the Canvas is clicked
        private void splitContainer1_Panel2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && fillSelected) //If the fill tool is selected and left click is pressed
            {
                    Color fillColour = bmap.GetPixel(e.X, e.Y); //Gets the colour of clicked pixel, to compare with neighbours, and their neighbours, and so on

                    //We don't need to fill a selection that is already the same colour
                    if(!fillColour.Equals(colour))
                    {
                        Point p = new Point(e.X, e.Y); //Selected pixel coordinate 
                        Stack<Point> s = new Stack<Point>(); //Stack of pixel coordinates to check

                        s.Push(p); //Push clicked pixel coordinate to stacks
                        
                        while (s.Count > 0) //While the stack contains elements
                        {
                            p = s.Pop(); //Pop the last
                            Color currentcolor = bmap.GetPixel(p.X, p.Y);
                            if (currentcolor == fillColour) //If the colour of the pixel at that position is the same as the colour we are filling
                            {
                                bmap.SetPixel(p.X, p.Y, colour); //Set it to the colour selected

                                //Add neighbours to the stack, if statements check if the new point is within the canvas bounds
                                if (p.X - 1 > 0) s.Push(new Point(p.X - 1, p.Y));
                                if (p.X + 1 < bmap.Width) s.Push(new Point(p.X + 1, p.Y));
                                if (p.Y - 1 > 0) s.Push(new Point(p.X, p.Y - 1));
                                if (p.Y + 1 < bmap.Height) s.Push(new Point(p.X, p.Y + 1));
                            }
                        }
                    }
            }
        }

        private void splitContainer1_Panel2_MouseUp(object sender, MouseEventArgs e) // occurs when mouse button is not being pressed (when you let go of it)
        {
            moving = false;
            using (Graphics g = Graphics.FromImage(bmap))
            {
                if (pencil == ellipse)
                {
                    //removing the trace lines that helped see how big ellipse is
                    Pen guidePen = new Pen(colourBkg, 4);
                    guidePen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    guidePen.StartCap = System.Drawing.Drawing2D.LineCap.Square;
                    g.DrawLine(guidePen, new Point(cursorX, cursorY), new Point(cursorX, e.Y));
                    g.DrawLine(guidePen, new Point(cursorX, cursorY), new Point(e.X, cursorY));

                    SolidBrush sb = new SolidBrush(colourBkg);
                    g.DrawEllipse(pencil, new Rectangle(cursorX, cursorY, e.X - shapeX, e.Y - shapeY));
                    //g.DrawEllipse(pencil, new RectangleF( x, y, e.X - shapeX, e.Y - shapeY));
                    g.FillEllipse(sb, new Rectangle(cursorX, cursorY, e.X - shapeX, e.Y - shapeY));
                }
                else if ( pencil == line)
                {
                    Pen guidePen = new Pen(colourBkg, 4);
                    guidePen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    guidePen.StartCap = System.Drawing.Drawing2D.LineCap.Square;
                    g.DrawLine(guidePen, new Point(cursorX, cursorY), new Point(cursorX, e.Y));
                    g.DrawLine(guidePen, new Point(cursorX, cursorY), new Point(e.X, cursorY));


                    g.DrawLine(pencil, new Point(cursorX, cursorY), new Point(e.X, e.Y));
                }
            }
            splitContainer1.Panel2.Invalidate();
            cursorX = -1;
            cursorX = -1;
            splitContainer1.Panel2.Cursor = Cursors.Default;
        }


        private void sizebox_TextChanged(object sender, EventArgs e)// select the size of brush
        {
            int value = 0;
            if (int.TryParse(txtSizebox.Text, out value) == false)
            {
            }
            else
            {
                size = Int32.Parse(txtSizebox.Text);
                pencil.Width = size;
            }
        }
        private void btnBrush_Click(object sender, EventArgs e)
        {
            pencil = brush;

            refresh();
            btnBrush.BackColor = SetHue(Color.LightGreen);
            tempColour = Color.LightGreen;

        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // need to figure out how to save it as a new file


            // bmap.Save(@"### PATH FROM A DRIVE###\bmp3.bmp");
            var saveDialogue = new SaveFileDialog();
            saveDialogue.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            saveDialogue.Filter = ".png|PNG|.jpg;.jpeg;.jpe|JPG|.gif|GIF|.bmp|BMP| "  + "All Graphics Types|.*bmp;.*jpg;.*png;.*gif";
            saveDialogue.Title = "Save your file";
            //Show the file saving dialogue
            DialogResult result = saveDialogue.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = saveDialogue.FileName;
                // fileTypeDefault is set when a file is saved using saved as 
                if (fileTypeDefaultSave == "bmp")
                {
                    bmap.Save(folderName + ".bmp");
                }
                else if (fileTypeDefaultSave == "jpg")
                {
                    bmap.Save(folderName + ".jpg");
                }
                else if (fileTypeDefaultSave == "png")
                {
                    bmap.Save(folderName + ".png");
                }
                else if (fileTypeDefaultSave == "gif")
                {
                    bmap.Save(folderName + ".gif");
                }

            }





        }

        private static bool ColorMatch(Color a, Color b)
        {
            return (a.ToArgb() & 0xffffff) == (b.ToArgb() & 0xffffff);
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
                if (formHeight < canvas.MinimumSize.Height || formWidth < canvas.MinimumSize.Width)
                {
                    MessageBox.Show("Minimum Height and Width are: " + canvas.MinimumSize.Height + " & " + canvas.MinimumSize.Width + " respectively.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                else if (MessageBox.Show("Resizing Canvas will erase all work on canvas. Do you wish to continue?", "Alert", (MessageBoxButtons.YesNo), MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    splitContainer1.Anchor = (AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top);
                    Size = new Size(formWidth + widthDiffernce, formWidth + heightDiffernce);//give exact canvas size as required

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

       

        private void pNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveDialogue = new SaveFileDialog();
            saveDialogue.Filter = ".png|PNG|";
            saveDialogue.Title = "Save your file as a png";
            //Show the file saving dialogue
            DialogResult result = saveDialogue.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = saveDialogue.FileName;
                bmap.Save(".png");
                fileTypeDefaultSave = "png";
            }
        }

        private void jPGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveDialogue = new SaveFileDialog();
            saveDialogue.Filter = ".jpg;.jpeg;.jpe|JPG|";
            saveDialogue.Title = "Save your file as a jpg";
            //Show the file saving dialogue
            DialogResult result = saveDialogue.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = saveDialogue.FileName;
                bmap.Save(folderName + ".jpg");
                fileTypeDefaultSave = "jpg";
            }
        }

        private void bMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveDialogue = new SaveFileDialog();
            saveDialogue.Filter = ".bmp|BMP|";
            saveDialogue.Title = "Save your file as a bitmap";
            //Show the file saving dialogue
            DialogResult result = saveDialogue.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = saveDialogue.FileName;
                bmap.Save(".bmp");
                fileTypeDefaultSave = "bmp";
            }
        }

        private void gIFToolStripMenuItem_Click(object sender, EventArgs e)
        {var saveDialogue = new SaveFileDialog();
            saveDialogue.Filter = ".gif|GIF|";
            saveDialogue.Title = "Save your file as a gif";
            DialogResult result = saveDialogue.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = saveDialogue.FileName;
                bmap.Save(".gif");
                fileTypeDefaultSave = "gif";
            }
         }

      

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int widthDiffernce = 112;
            int heightDiffernce = 62;
            var openFile = new OpenFileDialog();

            openFile.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFile.Filter = "Image files (.png, .jpg, .jpeg, .jpe, gif, bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.gif; *.bmp";

            openFile.Title = "Open a file";
            //Show the file saving dialogue
            DialogResult result = openFile.ShowDialog();
            // loads the bitmap and hen rsizes the canvas accordingly
            if (result == DialogResult.OK)
            {

                bmap = (Bitmap)Bitmap.FromFile(openFile.FileName);
                splitContainer1.Anchor = (AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top);
                Size = new Size(bmap.Width + widthDiffernce, bmap.Height + heightDiffernce);//give exact canvas size as required

                // bmap = new Bitmap(splitContainer1.Panel2.Width, splitContainer1.Panel2.Height);
                splitContainer1.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
            }
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            pencil = line;

            refresh();
            btnLine.BackColor = SetHue(Color.LightGreen);
            tempColour = Color.LightGreen;
        }

        private void btnEllipse_Click(object sender, EventArgs e)
        {
            pencil = ellipse;

            refresh();
            btnEllipse.BackColor = SetHue(Color.LightGreen);
            tempColour = Color.LightGreen;
        }

        private void btnRect_Click(object sender, EventArgs e)
        {
            pencil = rect;

            refresh();
            btnRect.BackColor = SetHue(Color.LightGreen);
            tempColour = Color.LightGreen;
        }

        private void btnEyedropper_Click(object sender, EventArgs e)
        {


            refresh();
            btnEyedropper.BackColor = SetHue(Color.LightGreen);
            tempColour = Color.LightGreen;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Canvas tab = new Canvas();
            tab.ShowDialog();
        }

        private void btnPencil_Click(object sender, EventArgs e)
        {
            pencil = brush;

            refresh();
            btnPencil.BackColor = SetHue(Color.LightGreen);
            tempColour = Color.LightGreen;
        }

        private void btnEraser_Click(object sender, EventArgs e)
        {
            pencil = eraser;

            refresh();
            btnEraser.BackColor = SetHue(Color.LightGreen);
            tempColour = Color.LightGreen;
        }


        bool fillSelected;
        private void btnFill_Click(object sender, EventArgs e)
        {
            refresh();
            btnFill.BackColor = SetHue(Color.LightGreen);
            tempColour = Color.LightGreen;
            fillSelected = true;
        }

        private void splitContainer1_Panel2_DragDrop(object sender, DragEventArgs e)
        {
            int widthDiffernce = 112;
            int heightDiffernce = 62;
            var data = e.Data.GetData(DataFormats.FileDrop);
            if(data!=null)
            {
                var fileName = data as string[];
                if(fileName.Length>0)
                {
                    bmap = (Bitmap)Bitmap.FromFile(fileName[0]);
                    splitContainer1.Anchor = (AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top);
                    Size = new Size(bmap.Width + widthDiffernce, bmap.Height + heightDiffernce);//give exact canvas size as required

                    // bmap = new Bitmap(splitContainer1.Panel2.Width, splitContainer1.Panel2.Height);
                    splitContainer1.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                }
            }
        }

        private void splitContainer1_Panel2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        //hover

        private void btnPencil_MouseEnter(object sender, EventArgs e)
        {
            tempColour = btnPencil.BackColor;
            Color colour;
            if (btnPencil.BackColor == System.Drawing.Color.Transparent)
            {
                colour = Color.LightGray;
            }
            else
            {
                colour = SetHue(btnPencil.BackColor);
            }

            btnPencil.BackColor = colour;
        }

        private void btnPencil_MouseLeave(object sender, EventArgs e)
        {
            btnPencil.BackColor = tempColour;
        }

        private void btnBrush_MouseEnter(object sender, EventArgs e)
        {
            tempColour = btnBrush.BackColor;
            Color colour;
            if (btnBrush.BackColor == System.Drawing.Color.Transparent)
            {
                colour = Color.LightGray;
            }
            else
            {
                colour = SetHue(btnBrush.BackColor);
            }

            btnBrush.BackColor = colour;
        }

        private void btnBrush_MouseLeave(object sender, EventArgs e)
        {
            btnBrush.BackColor = tempColour;
        }

        private void btnEraser_MouseEnter(object sender, EventArgs e)
        {
            tempColour = btnEraser.BackColor;
            Color colour;
            if (btnEraser.BackColor == System.Drawing.Color.Transparent)
            {
                colour = Color.LightGray;
            }
            else
            {
                colour = SetHue(btnEraser.BackColor);
            }

            btnEraser.BackColor = colour;
        }

        private void btnEraser_MouseLeave(object sender, EventArgs e)
        {
            btnEraser.BackColor = tempColour;
        }

        //file save

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(bmap)) // draws on the bitmap
            {
                g.Clear(colourBkg);
            }
        }

        //file import
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var imageDialogue = new OpenFileDialog();

            imageDialogue.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            imageDialogue.Filter = "Image files (*.png,*.jpg, *.jpeg, *.jpe, *gif, bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            imageDialogue.Title = "Import an image";

            //Show the file saving dialogue
            DialogResult result = imageDialogue.ShowDialog();
            if (result == DialogResult.OK)
            {
                Image image = Image.FromFile(imageDialogue.FileName);
                using (Graphics g = Graphics.FromImage(bmap)) // draws on the bitmap
                {
                    g.DrawImage(image, 1, 1);
                }
            }
        }

        private void bkgSelect_Click(object sender, EventArgs e)
        {
            //colour selector method
            PictureBox colours = (PictureBox)sender;
            colours.Invalidate();
            // pencil.Color = colours.BackColor; // sets the selected colour
            // colour = colours.BackColor;


            if (diag.ShowDialog() == DialogResult.OK)
            {
                bkgSelect.BackColor = diag.Color;

                colourBkg = diag.Color;
                eraser.Color = diag.Color;
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt == true && e.KeyCode == Keys.P)
            {
                btnPencil_Click(sender, e);
            }
            else if (e.Alt == true && e.KeyCode == Keys.B)
            {
                btnBrush_Click(sender, e);
            }
            else if (e.Alt == true && e.KeyCode == Keys.E)
            {
                btnEraser_Click(sender, e);
            }
            else if (e.Alt == true && e.KeyCode == Keys.F)
            {
                btnFill_Click(sender, e);
            }
            else if (e.Alt == true && e.KeyCode == Keys.I)
            {
                btnEyedropper_Click(sender, e);
            }
            else if (e.Alt == true && e.KeyCode == Keys.L)
            {
                btnLine_Click(sender, e);
            }
            else if (e.Alt == true && e.KeyCode == Keys.E)
            {
                btnEllipse_Click(sender, e);
            }
            else if (e.Alt == true && e.KeyCode == Keys.O)
            {
                btnRect_Click(sender, e);
            }
            else if (e.Control == true && e.KeyCode == Keys.N)
            {
                saveToolStripMenuItem_Click(sender, e);
            }
            else if (e.Control == true && e.KeyCode == Keys.O)
            {
                openToolStripMenuItem_Click(sender, e);
            }
            else if (e.Control == true && e.KeyCode == Keys.S)
            {
                saveToolStripMenuItem1_Click(sender, e);
            }
            else if (e.Control == true && e.KeyCode == Keys.I)
            {
                importToolStripMenuItem_Click(sender, e);
            }
        }





        //methods

        Color SetHue(Color oldColor)
        {
            var temp = new HSV();
            float hue = oldColor.GetHue();
            temp.h = hue - 15;
            temp.s = oldColor.GetSaturation();
            temp.v = getBrightness(oldColor);
            return ColorFromHSL(temp);
        }

        private void btnFill_MouseEnter(object sender, EventArgs e)
        {
            tempColour = btnFill.BackColor;
            Color colour;
            if (btnFill.BackColor == System.Drawing.Color.Transparent)
            {
                colour = Color.LightGray;
            }
            else
            {
                colour = SetHue(btnFill.BackColor);
            }

            btnFill.BackColor = colour;
        }

        private void btnFill_MouseLeave(object sender, EventArgs e)
        {
            btnFill.BackColor = tempColour;
        }

        private void btnEyedropper_MouseEnter(object sender, EventArgs e)
        {
            tempColour = btnEyedropper.BackColor;
            Color colour;
            if (btnEyedropper.BackColor == System.Drawing.Color.Transparent)
            {
                colour = Color.LightGray;
            }
            else
            {
                colour = SetHue(btnEyedropper.BackColor);
            }

            btnEyedropper.BackColor = colour;
        }

        private void btnEyedropper_MouseLeave(object sender, EventArgs e)
        {
            btnEyedropper.BackColor = tempColour;
        }

        private void btnLine_MouseEnter(object sender, EventArgs e)
        {
            tempColour = btnLine.BackColor;
            Color colour;
            if (btnLine.BackColor == System.Drawing.Color.Transparent)
            {
                colour = Color.LightGray;
            }
            else
            {
                colour = SetHue(btnLine.BackColor);
            }

            btnLine.BackColor = colour;
        }

        private void btnLine_MouseLeave(object sender, EventArgs e)
        {
            btnLine.BackColor = tempColour;
        }

        private void btnEllipse_MouseEnter(object sender, EventArgs e)
        {
            tempColour = btnEllipse.BackColor;
            Color colour;
            if (btnEllipse.BackColor == System.Drawing.Color.Transparent)
            {
                colour = Color.LightGray;
            }
            else
            {
                colour = SetHue(btnEllipse.BackColor);
            }

            btnEllipse.BackColor = colour;
        }

        private void btnEllipse_MouseLeave(object sender, EventArgs e)
        {
            btnEllipse.BackColor = tempColour;
        }

        private void btnRect_MouseEnter(object sender, EventArgs e)
        {
            tempColour = btnRect.BackColor;
            Color colour;
            if (btnRect.BackColor == System.Drawing.Color.Transparent)
            {
                colour = Color.LightGray;
            }
            else
            {
                colour = SetHue(btnRect.BackColor);
            }

            btnRect.BackColor = colour;
        }

        private void btnRect_MouseLeave(object sender, EventArgs e)
        {
            btnRect.BackColor = tempColour;
        }

        float getBrightness(Color c)
        { return (c.R * 0.299f + c.G * 0.587f + c.B * 0.114f) / 256f; }

        static public Color ColorFromHSL(HSV hsl)
        {
            if (hsl.s == 0)
            { int L = (int)hsl.v; return Color.FromArgb(255, L, L, L); }

            double min, max, h;
            h = hsl.h / 360d;

            max = hsl.v < 0.5d ? hsl.v * (1 + hsl.s) : (hsl.v + hsl.s) - (hsl.v * hsl.s);
            min = (hsl.v * 2d) - max;

            Color c = Color.FromArgb(255, (int)(255 * RGBChannelFromHue(min, max, h + 1 / 3d)),
                                          (int)(255 * RGBChannelFromHue(min, max, h)),
                                          (int)(255 * RGBChannelFromHue(min, max, h - 1 / 3d)));
            return c;
        }

        static double RGBChannelFromHue(double m1, double m2, double h)
        {
            h = (h + 1d) % 1d;
            if (h < 0) h += 1;
            if (h * 6 < 1) return m1 + (m2 - m1) * 6 * h;
            else if (h * 2 < 1) return m2;
            else if (h * 3 < 2) return m1 + (m2 - m1) * 6 * (2d / 3d - h);
            else return m1;

        }
        public struct HSV { public float h; public float s; public float v; }

        void refresh()
        {
            btnBrush.BackColor = System.Drawing.Color.Transparent;
            btnEraser.BackColor = System.Drawing.Color.Transparent;
            btnFill.BackColor = System.Drawing.Color.Transparent;
            btnPencil.BackColor = System.Drawing.Color.Transparent;
            btnEyedropper.BackColor = System.Drawing.Color.Transparent;
            btnEllipse.BackColor = System.Drawing.Color.Transparent;
            btnLine.BackColor = System.Drawing.Color.Transparent;
            btnRect.BackColor = System.Drawing.Color.Transparent;

            fillSelected = false;
        }
    }
}
