namespace ZoomOpera.CartersianPlane
{
    public static class CircumferenceFinder
    {
        public static ImplicitFormcCircumference FindWith(CartesianPoint center, CartesianPoint circumferencePoint)
        {
            //Avendo centro(c1; c2) e raggio 'r' => Formula (x - c1)^2 + (y - c2)^2 = r^2
            double radius = FindRadius(center, circumferencePoint);

            return 
                new ImplicitFormcCircumference(1, 1, (2 * (-center.X)), (2 * (-center.Y)), (Math.Pow(center.X, 2) + Math.Pow(center.Y, 2) - Math.Pow(radius, 2)) );
        }

        private static double FindRadius(CartesianPoint center, CartesianPoint circumferencePoint)
        {
            double radius;
            if (center.X == circumferencePoint.X)
            {
                radius = center.Y - circumferencePoint.Y;
                if (radius < 0)
                    radius = -radius;
            }
            else if (center.Y == circumferencePoint.Y)
            {
                radius = center.X - circumferencePoint.X;
                if (radius < 0)
                    radius = -radius;
            }
            else
                radius = PythagoreanTheorem(center, circumferencePoint);

            return radius;
        }

        private static double PythagoreanTheorem(CartesianPoint center, CartesianPoint circumferencePoint)
        {
            // Where C^2 = A^2 + B^2

            double A = center.X - circumferencePoint.X;

            double B = center.Y - circumferencePoint.Y;

            double C = Math.Sqrt(Math.Pow(A, 2) + Math.Pow(B, 2));

            return C;

        }
    }
}
