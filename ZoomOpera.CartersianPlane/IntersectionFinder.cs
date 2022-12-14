namespace ZoomOpera.CartersianPlane
{
    public static class IntersectionFinder
    {
        public static CartesianPoint? IntesectionBetween(ImplicitFormStraightLine firstLine, ImplicitFormStraightLine secondLine)
        {
            //TODO: caso rette parallele tramite coeficente angolare

            if ((firstLine.a == 0 && secondLine.a == 0)                 //Le rette sono parallele tra di loro 
                || (firstLine.b == 0 && secondLine.b == 0))             // e all'asse X oppure Y
                return null;                                

            if (firstLine.a == 0 && secondLine.b == 0)                  //La prima retta e' parallela all'asse X 
                return new CartesianPoint(-secondLine.c, -firstLine.c); //La seconda all'asse Y

            if (firstLine.b == 0 && secondLine.a == 0)                  //La prima retta e' parallela all'asse Y 
                return new CartesianPoint(-firstLine.c, -secondLine.c); //La seconda all'asse X
            
            //Solamente una delle due rette e' parallela ad uno dei due assi:
            if (firstLine.a == 0)                            //Prima retta parallela asse X - A
            {
                var Y = -firstLine.c;
                var X = -((secondLine.b * Y) + secondLine.c)/secondLine.a;
                Console.WriteLine("A");
                return new CartesianPoint(X, Y);
            } 
            if (firstLine.b == 0)                            //Prima retta parallela asse Y - B
            {
                var X = -firstLine.c;
                var Y = -((secondLine.a * X) + secondLine.c)/secondLine.b;
                Console.WriteLine("B");
                return new CartesianPoint(X, Y);
            }
            if (secondLine.a == 0)                           //Seconda retta parallela asse X - C
            {
                var Y = -secondLine.c;
                var X = -((firstLine.b * Y) + firstLine.c)/firstLine.a;
                Console.WriteLine("C");
                return new CartesianPoint(X, Y);
            }
            if (secondLine.b == 0)                          //Seconda retta parallela asse Y - D
            {
                var X = -secondLine.c;
                var Y = -((firstLine.a * X) + firstLine.c)/firstLine.b;
                Console.WriteLine("D");
                return new CartesianPoint(X, Y);
            }
                
            //Tutti altri casi
            //Sistema di euqazioni

            //trovo x da seconda equazione ax = -by -c
            //   x = (-b/a)y + (-c/a) 
            //sostituisco la x nella prima equazione
            // a((-secondLine.b / secondLine.a)y + (-secondLine.c / secondLine.a)) + by + c = 0;
            var b1 = -secondLine.b * firstLine.a;
            var c1 = -secondLine.c * firstLine.a;

            //elimino il denominatore moltiplicando tutto per la 'a' della seconda equazione
            //firstLine.b *= secondLine.a;
            var firstLine_BxDenominator = firstLine.b * secondLine.a;
            //firstLine.c *= secondLine.a;
            var firstLine_CxDenominator = firstLine.c * secondLine.a;

            //trovo y dalla prima equazione
            //var y = -(firstLine.c + c1) / (firstLine.b + b1);
            var y = -(firstLine_CxDenominator + c1) / (firstLine_BxDenominator + b1);

            //sostituisco y nella seconda equazione e trovo x
            var x = ((-secondLine.b * y) / secondLine.a) + (-secondLine.c / secondLine.a);


            return new CartesianPoint(x, y);
        }

        //public static LinkedList<CartesianPoint> IntesectionBetween(ImplicitFormStraightLine line,
        //                                                            ImplicitFormcCircumference circumference)
        //{
        //    LinkedList<CartesianPoint> intersectionPoints = new LinkedList<CartesianPoint>();

        //    //dopo aver ricavato la x dalla retta
        //    //la sostituisco nella circonferenza e ne ottengo
        //    //un equazione di secondo grado
        //    var equation = GetEquationFrom(line, circumference);

        //    var delta = equation.getDelta();

        //    //due intersezioni
        //    if (delta > 0)
        //    {
        //        deltaGreaterThanZero(line, equation, intersectionPoints);
        //        return intersectionPoints;
        //    }
        //    //una intersezione
        //    if (delta == 0)
        //    {
        //        deltaIsZero(line, equation, intersectionPoints);
        //        return intersectionPoints;
        //    }
        //    //lista vuota
        //    return intersectionPoints;
        //}

        public static LinkedList<CartesianPoint> IntesectionBetween(ImplicitFormStraightLine line,
                                                                    ImplicitFormcCircumference circumference)
        {
            LinkedList<CartesianPoint> intersectionPoints = new LinkedList<CartesianPoint>();

            if (line.a == 0)
            {
                HorizontalLineCase(line, circumference, intersectionPoints);
            }

            else if (line.b == 0)
            {
                VerticalLineCase(line, circumference, intersectionPoints);
            }

            else
            {
                AllOtherCases(line, circumference, intersectionPoints);
            }

            return intersectionPoints;
        }


        private static void HorizontalLineCase(ImplicitFormStraightLine line,
                                                ImplicitFormcCircumference circumference,
                                                LinkedList<CartesianPoint> intersectionPoints)
        {
            var equation = GetEquationHorizontalCase(line, circumference);

            var delta = equation.getDelta();


            if (delta > 0)
                TwoSolutionsHorizontal(line, equation, intersectionPoints);

            if (delta == 0)
                OneSolutionHorizontal(line, equation, intersectionPoints);

        }

        private static SecondGradeEquation GetEquationHorizontalCase(ImplicitFormStraightLine line, 
                                                                        ImplicitFormcCircumference circumference)
        {
            //ottengo y da retta
            var y = -line.c;

            //sostituisco nella equazione della circonferenza 

            var c1 = Math.Pow(y, 2); // --> y^2 --> (-line.c)^2
            var c2 = circumference.B * y; // --> considerando circonferenza: x^2 + y^2 + ax + by + c = 0 --> by --> b * (-line.c) 

            var equation_a = circumference.CoefficientOfSquaredX;
            var equation_b = circumference.A;
            var equation_c = circumference.C + c1 + c2;

            return new SecondGradeEquation(equation_a, equation_b, equation_c);
        }

        private static void TwoSolutionsHorizontal(ImplicitFormStraightLine line, 
                                                    SecondGradeEquation equation,
                                                    LinkedList<CartesianPoint> intersectionPoints)
        {
            var firstX = -equation.b - Math.Sqrt(equation.getDelta());
            var denominatorFirstX = 2 * equation.a;
            var fistSolution = new CartesianPoint((firstX / denominatorFirstX), -line.c);

            intersectionPoints.AddLast(fistSolution);

            var secondX = -equation.b + Math.Sqrt(equation.getDelta());
            // denominatore uguale a quello della prima x
            var secondSolution = new CartesianPoint((secondX / denominatorFirstX), -line.c);

            intersectionPoints.AddLast(secondSolution);

        }

        private static void OneSolutionHorizontal(ImplicitFormStraightLine line,
                                                    SecondGradeEquation equation,
                                                    LinkedList<CartesianPoint> intersectionPoints)
        {
            intersectionPoints.AddLast(new CartesianPoint((-equation.b / (2 * equation.a)), -line.c));
        }


        private static void VerticalLineCase(ImplicitFormStraightLine line, 
                                                ImplicitFormcCircumference circumference, 
                                                LinkedList<CartesianPoint> intersectionPoints)
        {
            var equation = GetEquationVerticalCase(line, circumference);

            var delta = equation.getDelta();


            if (delta > 0)
                TwoSolutionsVertical(line, equation, intersectionPoints);

            if (delta == 0)
                OneSolutionVertical(line, equation, intersectionPoints);
        }

        

        private static SecondGradeEquation GetEquationVerticalCase(ImplicitFormStraightLine line, 
                                                                    ImplicitFormcCircumference circumference)
        {
            //ottengo y da retta
            var x = -line.c;    

            //sostituisco nella equazione della circonferenza 

            var c1 = Math.Pow(x, 2); // --> x^2 --> (-line.c)^2
            var c2 = circumference.A * x; // --> considerando circonferenza: x^2 + y^2 + ax + by + c = 0 --> ax --> a * (-line.c) 

            var equation_a = circumference.CoefficientOfSquaredY;
            var equation_b = circumference.B;
            var equation_c = circumference.C + c1 + c2;

            return new SecondGradeEquation(equation_a, equation_b, equation_c);
        }

        private static void TwoSolutionsVertical(ImplicitFormStraightLine line, 
                                                    SecondGradeEquation equation, 
                                                    LinkedList<CartesianPoint> intersectionPoints)
        {
            var firstY = -equation.b - Math.Sqrt(equation.getDelta());
            var denominatorFirstY = 2 * equation.a;
            var firstSolution = new CartesianPoint(-line.c, (firstY / denominatorFirstY));

            intersectionPoints.AddLast(firstSolution);

            var secondY = -equation.b + Math.Sqrt(equation.getDelta());
            //denominatore uguale a quello della prima y
            var secondSolution = new CartesianPoint(-line.c, (secondY / denominatorFirstY));

            intersectionPoints.AddLast(secondSolution);
        }

        private static void OneSolutionVertical(ImplicitFormStraightLine line, 
                                                SecondGradeEquation equation, 
                                                LinkedList<CartesianPoint> intersectionPoints)
        {
            intersectionPoints.AddLast(new CartesianPoint(-line.c, (-equation.b / (2 * equation.a))));
        }

        private static void AllOtherCases(ImplicitFormStraightLine line, ImplicitFormcCircumference circumference, LinkedList<CartesianPoint> intersectionPoints)
        {
            //dopo aver ricavato la x dalla retta
            //la sostituisco nella circonferenza e ne ottengo
            //un equazione di secondo grado
            var equation = GetEquationFrom(line, circumference);

            var delta = equation.getDelta();

            //due intersezioni
            if (delta > 0)
            {
                deltaGreaterThanZero(line, equation, intersectionPoints);
            }

            //una intersezione
            if (delta == 0)
            {
                deltaIsZero(line, equation, intersectionPoints);
            }
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

            //circumference.CoefficientOfSquaredY *= denominatorCoefficientOfSquaredY_2;
            var squared_y = circumference.CoefficientOfSquaredY * denominatorCoefficientOfSquaredY_2;
            b3 *= (denominatorCoefficientOfSquaredY_2 / Denominator_b3);
            c3 *= (denominatorCoefficientOfSquaredY_2 / Denominator_b3);
            //circumference.B *= denominatorCoefficientOfSquaredY_2;
            var b = circumference.B * denominatorCoefficientOfSquaredY_2;
            //circumference.C *= denominatorCoefficientOfSquaredY_2;
            var c = circumference.C * denominatorCoefficientOfSquaredY_2;

            //ricavo equazione secondo grado --> ay^2 + by + c = 0
            var equation_a = squared_y + coefficientOfSquaredY_2;
            var equation_b = b + b2 + b3;
            var equation_c = c + c2 + c3; 

            return new SecondGradeEquation(equation_a, equation_b, equation_c);
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
            var firstX = (substitution * (leastCommonMultiple / denominatorSubstitution) + (-line.c * (leastCommonMultiple / line.a))) 
                                                                            / leastCommonMultiple;

            result.AddLast(new CartesianPoint(firstX, firstY/denominatorFirstY));
            
            //denominatori uguali
            var secondY = -equation.b - Math.Sqrt(equation.getDelta());
            substitution = -line.b * secondY;
            var secondX = (substitution * (leastCommonMultiple / denominatorSubstitution) + (-line.c * (leastCommonMultiple / line.a))) 
                                                                                / leastCommonMultiple;

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

            //  x = (-(b-e)y + -(c-f)) / (a-d)
            var bMinusE = -(firstCircumference.B - secondCircumference.B);
            var cMinusF = -(firstCircumference.C - secondCircumference.C);
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

            //firstCircumference.CoefficientOfSquaredY *= squaredDenominator;
            var squared_y = firstCircumference.CoefficientOfSquaredY * squaredDenominator;
            b2 *= (squaredDenominator / denominator);
            c2 *= (squaredDenominator / denominator);
            //firstCircumference.B *= squaredDenominator;
            var b = firstCircumference.B * squaredDenominator;
            //firstCircumference.C *= squaredDenominator;
            var c = firstCircumference.C * squaredDenominator;

            //equazione
            var equation_a = squared_y + squared_BminusE;
            var equation_b = b + doubleProduct + b2;
            var equation_c = c + squared_CminusF + c2;

            return new SecondGradeEquation(equation_a, equation_b, equation_c);
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
            var leastCommonMultiple = LCM(substitutionDenominator, denominator);
            var firstX = (substitution * (leastCommonMultiple / substitutionDenominator) + (cMinusF * (leastCommonMultiple / denominator))) 
                                                                                / leastCommonMultiple;

            result.AddLast(new CartesianPoint(firstX, firstY / denominatorFirstY));

            //denominatori uguali
            var secondY = -equation.b + Math.Sqrt(equation.getDelta());
            substitution = bMinusE * secondY;
            var secondX = (substitution * (leastCommonMultiple / substitutionDenominator) + (cMinusF * (leastCommonMultiple / denominator))) 
                                                                                / leastCommonMultiple;

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
