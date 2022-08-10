namespace ZoomOpera.CartersianPlane
{
    public static class IntersectionFinder
    {
        public static CartesianPoint IntesectionBetween(ImplicitFormStraightLine firstLine, ImplicitFormStraightLine secondLine)
        {
            //TODO: caso rette parallele e x=k oppure y=k

            //Sistema di euqazioni

            //trovo x da seconda equazione ax = -by -c
            //   x = (-b/a)y + (-c/a) 
            //sostituisco la x nella prima equazione
            // a(-secondLine.b / secondLine.a) + (-secondLine.c / secondLine.a)) + by + c = 0;
            var b1 = -secondLine.b * firstLine.a;
            var c1 = -secondLine.c * firstLine.a;

            //elimino il denominatore moltiplicando tutto per la 'a' della seconda equazione
            firstLine.b *= secondLine.a;
            firstLine.c *= secondLine.a;

            //trovo y dalla prima equazione
            var y = -(firstLine.c + c1) / (firstLine.b + b1);

            //sostituisco y nella seconda equazione e trovo x
            var x = ((-secondLine.b * y) / secondLine.a) + (-secondLine.c / secondLine.a);


            return new CartesianPoint(x, y);
        }

        public static LinkedList<CartesianPoint> IntesectionBetween(ImplicitFormStraightLine line, ImplicitFormcCircumference circumference)
        {
            LinkedList<CartesianPoint> intersectionPoints = new LinkedList<CartesianPoint>();

            //dopo aver ricavato la x dalla retta
            //la sostituisco nella circonferenza e ne ottengo
            //un equazione di secondo grado
            var equation = GetEquationFrom(line, circumference);

            var delta = equation.getDelta();

            //due intersezioni
            if (delta > 0)
            {
                deltaGreaterThanZero(line, equation, intersectionPoints);
                return intersectionPoints;
            }
            //una intersezione
            if (delta == 0)
            {
                deltaIsZero(line, equation, intersectionPoints);
                return intersectionPoints;
            }
            //lista vuota
            return intersectionPoints;
        }


        private static SecondGradeEquation GetEquationFrom(ImplicitFormStraightLine line,
                                                            ImplicitFormcCircumference circumference)
        {
            //trovo x dalla retta ax = -by -c
            //   x = (-b/a)y + (-c/a) 
            //e sostituisco nella formula della circonferenza

            var coefficientOfSquaredY_2 = Math.Pow(-line.b, 2);
            var denominatorCoefficientOfSquaredY_2 = Math.Pow(line.a, 2);
            var b2 = 2 * (-line.b * -line.c);
            var c2 = Math.Pow(-line.c, 2);

            var b3 = circumference.A * (-line.b);
            var Denominator_b3 = line.a;
            var c3 = circumference.A * (-line.c);

            //elimino denominatore moltiplicando tutto per il quadrato della 'a' della retta
            circumference.CoefficientOfSquaredY *= denominatorCoefficientOfSquaredY_2;
            b3 *= (denominatorCoefficientOfSquaredY_2 / Denominator_b3);
            c3 *= (denominatorCoefficientOfSquaredY_2 / Denominator_b3);
            circumference.B *= denominatorCoefficientOfSquaredY_2;
            circumference.C *= denominatorCoefficientOfSquaredY_2;

            //ricavo equazione secondo grado --> ay^2 + by + c = 0
            var a = circumference.CoefficientOfSquaredY + coefficientOfSquaredY_2;
            var b = circumference.B + b2 + b3;
            var c = circumference.C + c2 + c3;

            return new SecondGradeEquation(a, b, c);
        }

        private static void deltaGreaterThanZero(ImplicitFormStraightLine line, 
                                                    SecondGradeEquation equation, 
                                                    LinkedList<CartesianPoint> result)
        {
            var firstY = -equation.b + Math.Sqrt(equation.getDelta());
            var denominatorFirstY = 2 * equation.a;
            //sostituisco la y in x = (-b/a)y + (-c/a) 
            var substitution = -line.b * firstY;
            var denominatorSubstitution = line.a * denominatorFirstY;
            var leastCommonMultiple = LCM(denominatorSubstitution, line.a);
            var firstX = (substitution * (leastCommonMultiple / denominatorSubstitution) + (-line.c * (leastCommonMultiple / line.a))) / leastCommonMultiple;

            result.AddLast(new CartesianPoint(firstX, firstY/denominatorFirstY));
            
            //denominatori uguali
            var secondY = -equation.b - Math.Sqrt(equation.getDelta());
            substitution = -line.b * secondY;
            var secondX = (substitution * (leastCommonMultiple / denominatorSubstitution) + (-line.c * (leastCommonMultiple / line.a))) / leastCommonMultiple;

            result.AddLast(new CartesianPoint(secondX, secondY/denominatorFirstY));   
        }

        public static double LCM(double a, double b)
        {
            double num1, num2;
            if (a > b)
            {
                num1 = a; num2 = b;
            }
            else
            {
                num1 = b; num2 = a;
            }

            for (int i = 1; i < num2; i++)
            {
                double mult = num1 * i;
                if (mult % num2 == 0)
                {
                    return mult;
                }
            }
            return num1 * num2;
        }

        private static void deltaIsZero(ImplicitFormStraightLine line, 
                                        SecondGradeEquation equation, 
                                        LinkedList<CartesianPoint> result)
        {
            //y = 0
            double x = -line.c/line.a;
            result.AddLast(new CartesianPoint(x, 0));
        }



        public static LinkedList<CartesianPoint> IntesectionBetween(ImplicitFormcCircumference firstCircumference, 
                                                                    ImplicitFormcCircumference secondCircumference)
        {
            LinkedList<CartesianPoint> intersectionPoints = new LinkedList<CartesianPoint>();

            //equazione ottenuta sostiturendo la x trovata con risoluzione sistema
            var equation = GetEquationFrom(firstCircumference, secondCircumference);

            var delta = equation.getDelta();
            //due intersezioni due y ( da prima circonferenza )
            if (delta > 0)
            {
                deltaGreaterThanZero(firstCircumference,secondCircumference, equation, intersectionPoints);
                return intersectionPoints;
            }
            //una intersezione
            if (delta == 0)
            {
                deltaIsZero(firstCircumference, secondCircumference, equation, intersectionPoints);
                return intersectionPoints;
            }
            //lista vuota
            return intersectionPoints;
        }


        private static SecondGradeEquation GetEquationFrom(ImplicitFormcCircumference firstCircumference, 
                                                            ImplicitFormcCircumference secondCircumference)
        {
            //Ottegno la x:
            //risolvo sitemda tra ude circonferenze con medoto riduzione
            //  { x^2 + y^2 + ax + by + c = 0  - 
            // <
            //  { x^2 + y^2 + dx + ey + f = 0  =
            //    ________________________
            //            (a-d)x+(b-e)y+(c-f) 

            //  x = ((b-e)y + (c-f)) / (a-d)
            var bMinusE = firstCircumference.B - secondCircumference.B;
            var cMinusF = firstCircumference.C - secondCircumference.C;
            var denominator = firstCircumference.A - secondCircumference.A;

            //sostituisco nella prima circonferenza
            var squared_BminusE = Math.Pow(bMinusE, 2);
            var doubleProduct = 2 * (bMinusE * cMinusF);
            var squared_CminusF = Math.Pow(cMinusF, 2);
            var squaredDenominator = Math.Pow(denominator, 2);

            //denominatore di b2 e c2 uguale a 'denominator'
            var b2 = firstCircumference.A * bMinusE;
            var c2 = firstCircumference.A * cMinusF;

            //elimino denominatori moltiplicando tuttii membri per squaredDenominator
            firstCircumference.CoefficientOfSquaredY *= squaredDenominator;
            b2 *= (squaredDenominator / denominator);
            c2 *= (squaredDenominator / denominator);
            firstCircumference.B *= squaredDenominator;
            firstCircumference.C *= squaredDenominator;

            //equazione
            var a = firstCircumference.CoefficientOfSquaredY + squared_BminusE;
            var b = firstCircumference.B + doubleProduct + b2;
            var c = firstCircumference.C + squared_CminusF + c2;

            return new SecondGradeEquation(a, b, c);
        }

        private static void deltaGreaterThanZero(ImplicitFormcCircumference firstCircumference,
                                                    ImplicitFormcCircumference secondCircumference,
                                                    SecondGradeEquation equation,
                                                    LinkedList<CartesianPoint> result)
        {
            //  x = ((b-e)y + (c-f)) / (a-d)
            var bMinusE = firstCircumference.B - secondCircumference.B;
            var cMinusF = firstCircumference.C - secondCircumference.C;
            var denominator = firstCircumference.A - secondCircumference.A;

            var firstY = -equation.b + Math.Sqrt(equation.getDelta());
            var denominatorFirstY = 2 * equation.a;
            var substitution = bMinusE * firstY;
            var substitutionDenominator = denominatorFirstY * denominator;
            var leastCommonMultiple = LCM(substitution, denominator);
            var firstX = (substitution * (leastCommonMultiple / substitutionDenominator) + (cMinusF * (leastCommonMultiple / denominator))) / leastCommonMultiple;

            result.AddLast(new CartesianPoint(firstX, firstY / denominatorFirstY));

            //denominatori uguali
            var secondY = -equation.b + Math.Sqrt(equation.getDelta());
            substitution = bMinusE * secondY;
            var secondX = (substitution * (leastCommonMultiple / substitutionDenominator) + (cMinusF * (leastCommonMultiple / denominator))) / leastCommonMultiple;

            result.AddLast(new CartesianPoint(secondX, secondY/denominatorFirstY));
        }   

        private static void deltaIsZero(ImplicitFormcCircumference firstCircumference,
                                        ImplicitFormcCircumference secondCircumference,
                                        SecondGradeEquation equation,
                                        LinkedList<CartesianPoint> result)
        {
            //  x = ((b-e)y + (c-f)) / (a-d)
            var cMinusF = firstCircumference.C - secondCircumference.C;
            var denominator = firstCircumference.A - secondCircumference.A;

            // y = 0
            var x = cMinusF / denominator;
            result.AddLast(new CartesianPoint(x, 0));           
        }

        //equazione secondo grado --> ay^2 + by + c = 0
        private class SecondGradeEquation
        {
            public double a { get; set; }

            public double b { get; set; }
                
            public double c { get; set; }

            public double getDelta()
            {
                return Math.Pow(b, 2) - 4 * a * c;
            }

            public SecondGradeEquation(double a, double b, double c)
            {
                this.a = a;
                this.b = b;
                this.c = c;
            }
        }

    }

}
