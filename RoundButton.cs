using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

class RoundButton : UserControl
{
    public Point myPoint { get; set; }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics graphics = e.Graphics;
        Pen myPen = new Pen(Color.SlateGray);
        // Draw the button in the form of a circle
        graphics.DrawEllipse(myPen, 0,0, 20, 20);
        graphics.FillEllipse(Brushes.Black, 0, 0, 20, 20);
        myPen.Dispose();
    }

    public void paintColor(Brush brush)
    {
        Graphics g = this.CreateGraphics();
        g.FillEllipse(brush, 0, 0, 20, 20);
    }
}