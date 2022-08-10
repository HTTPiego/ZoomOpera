namespace ZoomOpera.CartersianPlane
{
    public static class StraightLineInTwoPointFinder    
    {
        public static ImplicitFormStraightLine FindStraightLine(CartesianPoint firstPoint, CartesianPoint secondPoint)
        {
            //retta verticale all'asse delle ascisse
            if (TheStraihgtLineIsVertical(firstPoint, secondPoint))
                return new ImplicitFormStraightLine(1, 0, firstPoint.X);

            //retta orizzonale all'asse delle ascisse
            if (TheStraihgtLineIsHorizontal(firstPoint, secondPoint))
                return new ImplicitFormStraightLine(0, 1, firstPoint.Y);

            //Seguendo formula (x-x1/x2-x1) = (y-y1/y2-y1)
            //membri di sinistra ---^              ^--- membri di destra
            double leftMembersDenominator = secondPoint.X - firstPoint.X;
            double rightMembersDenominator = secondPoint.Y - firstPoint.Y;
            double x = rightMembersDenominator;
            double x1 = -firstPoint.X * rightMembersDenominator;
            double y = leftMembersDenominator;
            double y1 = -firstPoint.Y * leftMembersDenominator;
            if (x < 0)
                return new ImplicitFormStraightLine(-x, y, -(x1 + (-y1)) );
            return new ImplicitFormStraightLine(x, -y, (x1 + (-y1)) );
        }

        private static bool TheStraihgtLineIsVertical(CartesianPoint firstPoint, CartesianPoint secondPoint)
        {
            if (firstPoint.X == secondPoint.X)
                return true;
            return false;
        }

        private static bool TheStraihgtLineIsHorizontal(CartesianPoint firstPoint, CartesianPoint secondPoint)
        {
            if (firstPoint.Y == secondPoint.Y)
                return true;
            return false;
        }

    }
}
