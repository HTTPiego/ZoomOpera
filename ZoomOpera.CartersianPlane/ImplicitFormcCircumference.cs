namespace ZoomOpera.CartersianPlane
{

    // x^2 + y^2 + ax + by + c = 0
    public class ImplicitFormcCircumference
    {
        public double CoefficientOfSquaredX { get; set; } 
        public double CoefficientOfSquaredY { get; set; }

        public double A { get; set; }

        public double B { get; set; }

        public double C { get; set; }

        public ImplicitFormcCircumference(double coefficientOfSquaredX, double coefficientOfSquaredY, double a, double b, double c)
        {
            CoefficientOfSquaredX = coefficientOfSquaredX;
            CoefficientOfSquaredY = coefficientOfSquaredY;
            A = a;
            B = b;
            C = c;
        }
    }
}
