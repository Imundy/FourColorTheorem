
using System;
using System.Collections.Generic;
using System.Drawing;

class ColorPoint : IEquatable<ColorPoint>
{

    Point point { get; set; }
    public string color { get; set; }

    public ColorPoint(Point point_)
    {
        
        point = point_;
        color = "";
    }

    public bool Equals(ColorPoint point_)
    {
        return (this.point.X == point_.point.X && this.point.Y == point_.point.Y);
    }

    public bool Equals(object obj)
    {
        ColorPoint cp = (ColorPoint)obj;
        return (cp.Equals(point));
    }

    public bool Equals(Point point_)
    {
        return (this.point.X == point_.X && this.point.Y == point_.Y);
    }

    public static bool operator == (ColorPoint point1, ColorPoint point2){
        return point1.Equals(point2);
    }

    public static bool operator !=(ColorPoint point1, ColorPoint point2)
    {
        return !point1.Equals(point2);
    }

    public static bool operator ==(Point point1, ColorPoint point2)
    {
        return point2.Equals(point1);
    }

    public static bool operator !=(Point point1, ColorPoint point2)
    {
        return !point2.Equals(point1);
    }

    public static bool operator ==(ColorPoint point1, Point point2)
    {
        return point1.Equals(point2);
    }

    public static bool operator !=(ColorPoint point1, Point point2)
    {
        return !point1.Equals(point2);
    }

    public override int GetHashCode()
    {
        string str = "" + point.X + ";" + point.Y;
        return str.GetHashCode();
    }


}

class ColorPointEqualityComparer : IEqualityComparer<ColorPoint>
{
    public bool Equals(ColorPoint cp1, ColorPoint cp2)
    {
        return cp1 == cp2;
    }

    public int GetHashCode(ColorPoint obj)
    {
        return obj.GetHashCode();
    }
}