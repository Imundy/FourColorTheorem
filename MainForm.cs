using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FourColorTheorem
{
    public partial class MainForm : Form
    {

        PictureBox pictureBox;
        Button colorButton;
        Button resetButton;
        Point curPoint;
        ColorPoint curColorPoint;
        Dictionary<ColorPoint, List<ColorPoint>> adjacencyList = 
            new Dictionary<ColorPoint, List<ColorPoint>>(new ColorPointEqualityComparer());
        List<RoundButton> myButtons = new List<RoundButton>();
        List<ColorPoint> myPoints = new List<ColorPoint>();
        string[] colors = {"RED","BLUE","GREEN","YELLOW"};

        public MainForm()
        {
            InitializeComponent();

            //create pictureBox
            pictureBox = new PictureBox();
            pictureBox.Size = new Size(1000, 650);
            pictureBox.BorderStyle = BorderStyle.Fixed3D;
            pictureBox.TabStop = false;
            pictureBox.Location = new Point(0, 0);
            pictureBox.BackColor = SystemColors.ControlDarkDark;
            pictureBox.MouseClick += new MouseEventHandler(pictureBox_mouseClick);
            pictureBox.Paint += pictureBox_Paint;

            //create colorButton
            colorButton = new Button();
            colorButton.Size = new Size(65, 35);
            colorButton.Text = "Color";
            colorButton.Location = new Point(0, 0);
            colorButton.BackColor = SystemColors.ControlDark;
            colorButton.Font = new Font(new FontFamily("Arial"), 12);
            colorButton.Click+=colorButton_Click;
            
            //create resetButton
            resetButton = new Button();
            resetButton.Size = new Size(65, 35);
            resetButton.Text = "Reset";
            resetButton.Location = new Point(65, 0);
            resetButton.BackColor = SystemColors.ControlDark;
            resetButton.Font = new Font(new FontFamily("Arial"), 12);
            resetButton.Click += resetButton_Click;
            
            //add controls
            this.Controls.Add(pictureBox);
            this.Controls.Add(colorButton);
            this.Controls.Add(resetButton);
            colorButton.BringToFront();
            resetButton.BringToFront();
        }

        void resetButton_Click(object sender, EventArgs e)
        {
            //reset all data structures to their original values of empty
            clickPts = new List<Point>();
            Invalidate();
            pictureBox.Image = null;
            foreach(RoundButton button in myButtons)
            {
                Controls.Remove(button);
            }
            myButtons = new List<RoundButton>();
            myPoints = new List<ColorPoint>();
            adjacencyList =
                new Dictionary<ColorPoint, List<ColorPoint>>(new ColorPointEqualityComparer());
        }

        //compute the correct 4 coloring of the graph and then call paintColors to apply it
        private void colorButton_Click(object sender, EventArgs e)
        {
            if (adjacencyList.Count < 1) { return; };

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool colored = false;
            while(!colored)
            {
                int i = 0;
                while(i < myPoints.Count && myPoints[i].color != ""){ i++; };
                if(i < myPoints.Count)
                {
                    ColorPoint point = myPoints[i];
                    foreach (string color in colors)
                    {
                        bool sameColor = false;
                        List<ColorPoint> cpl = adjacencyList[point];
                        foreach (ColorPoint aPoint in adjacencyList[point])
                        {
                            if (aPoint.color.Equals(color))
                                sameColor = true;
                        }
                        if (!sameColor)
                        {
                            point.color = color;
                            foreach (ColorPoint cp in myPoints)
                            {
                                if (adjacencyList[cp].Contains(point))
                                {
                                    int index = adjacencyList[cp].IndexOf(point);
                                    adjacencyList[cp][index] = point;
                                }
                            }
                            break;
                        }else if(color.Equals("YELLOW"))
                        {
                            myPoints[i-1].color="";
                            foreach (ColorPoint cp in myPoints)
                            {
                                if (adjacencyList[cp].Contains(myPoints[i - 1]))
                                {
                                    int index = adjacencyList[cp].IndexOf(myPoints[i - 1]);
                                    adjacencyList[cp][index] = myPoints[i - 1];
                                }
                            }
                            

                            ColorPoint tmp = myPoints[i];
                            myPoints[i] = myPoints[0];
                            myPoints[0] = tmp;
                        }
                    }
                }else{ colored = true; };
                if (stopwatch.ElapsedMilliseconds > 10000)
                {
                    break;
                }
            }
            if (colored)
            {
                paintColors();
            }else
            {
                resetButton.PerformClick();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //do nothing
        }

        //paints the colored graph based on myPoints
        private void paintColors()
        {
            foreach(RoundButton button in myButtons)
            {
                foreach(ColorPoint point in myPoints)
                {
                    if(point == button.myPoint)
                    {
                        switch (point.color)
                        {
                            case "RED":
                                button.paintColor(Brushes.Red);
                                break;
                            case "BLUE":
                                button.paintColor(Brushes.Blue);
                                break;
                            case "GREEN":
                                button.paintColor(Brushes.Green);
                                break;
                            case "YELLOW":
                                button.paintColor(Brushes.Yellow);
                                break;
                        }
                    }
                }
            }
        }

        //adds a ColorPoint and RoundButton to the bitmap when drawn
        private void pictureBox_mouseClick(object sender, MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            clickPts.Add(point);
            RoundButton button = new RoundButton();
            button.Location = new Point(e.X-10, e.Y-10);
            button.myPoint = point;
            button.BackColor = SystemColors.ControlDarkDark;
            button.Click += button_Click;
            button.Size = new Size(21, 21);
            myButtons.Add(button);
            
            Controls.Add(button);
            button.BringToFront();

            ColorPoint colorPoint = new ColorPoint(point);
            if (clickPts.Count < 2)
            {
                curPoint = point;
                curColorPoint = new ColorPoint(curPoint);

            }
            else
            {
                Graphics g = pictureBox.CreateGraphics();
                Pen pen = new Pen(Brushes.Black);
                g.DrawLine(pen, curPoint, point);

                //add items to adjacency lists
                if (!adjacencyList.ContainsKey(curColorPoint)) 
                { 
                    adjacencyList.Add(curColorPoint, new List<ColorPoint>());
                }
                if (!adjacencyList.ContainsKey(colorPoint)) 
                { 
                    adjacencyList.Add(colorPoint, new List<ColorPoint>());
                }

                adjacencyList[curColorPoint].Add(colorPoint);
                adjacencyList[colorPoint].Add(curColorPoint);
            }

            if (!myPoints.Contains(colorPoint))
                myPoints.Add(colorPoint);

            if (!adjacencyList.ContainsKey(curColorPoint))
            {
                adjacencyList.Add(curColorPoint, new List<ColorPoint>());
            }
            if (!adjacencyList.ContainsKey(colorPoint))
            {
                adjacencyList.Add(colorPoint, new List<ColorPoint>());
            }
        }

        //if the shift key is pressed it connects the points, otherwise it shifts our focus to the current point
        void button_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                ColorPoint clickPoint = new ColorPoint(((RoundButton)sender).myPoint);
                if (!adjacencyList.ContainsKey(curColorPoint))
                {
                    adjacencyList.Add(curColorPoint, new List<ColorPoint>());
                }
                if (!adjacencyList.ContainsKey(clickPoint))
                {
                    adjacencyList.Add(clickPoint, new List<ColorPoint>());
                }
                adjacencyList[curColorPoint].Add(clickPoint);
                adjacencyList[clickPoint].Add(curColorPoint);
                Graphics g = pictureBox.CreateGraphics();
                Pen pen = new Pen(Brushes.Black);
                g.DrawLine(pen, curPoint, ((RoundButton)sender).myPoint);
            }
            else
            {
                curPoint = ((RoundButton)sender).myPoint;
                curColorPoint = new ColorPoint(curPoint);
            }
        }

        List<Point> clickPts = new List<Point>();
        //List<Button> buttons = new List<Button>();

        //unnecessary method - remove
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if(clickPts.Count > 1)
            {
                Pen pen = new Pen(Brushes.Black);
                e.Graphics.DrawLine(pen, curPoint, clickPts[clickPts.Count-1]);
            }
        }

        private void MainForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
