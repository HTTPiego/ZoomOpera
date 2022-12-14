namespace ZoomOpera.CartersianPlane
{
    public class ImplicitFormStraightLine
    {
        // ax + by + c = 0
        public double a { get; set; }
        public double b { get; set; }
        public double c { get; set; }

        public ImplicitFormStraightLine(double a, double b, double c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public override bool Equals(object? obj)
        {
            return obj is ImplicitFormStraightLine line &&
                   a == line.a &&
                   b == line.b &&
                   c == line.c;
        }
    }
}
