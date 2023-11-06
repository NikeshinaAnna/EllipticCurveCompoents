
namespace EllipticCurveCompoents
{
    public class EdwardsCurveProectiveCoordinate : IEllipticCurve
    {
        private static BigInteger fieldChar { get; set; }
        private static BigInteger e { get; set; }
        private static BigInteger d { get; set; }

        public EdwardsCurveProectiveCoordinate(BigInteger _fieldChar, BigInteger _e, BigInteger _d)
        {
            fieldChar = _fieldChar;
            e = _e;
            d = _d;
        }

        private ProectivPoint AdditionPoints(ProectivPoint P, ProectivPoint Q)
        {
            var A = P.Z * Q.Z % fieldChar;
            var B = A * A % fieldChar;
            var C = P.X * Q.X % fieldChar;
            var D = P.Y * Q.Y % fieldChar;
            var E = d * C * D % fieldChar;
            var F = (B - E + fieldChar) % fieldChar;
            var G = (B + E) % fieldChar;
            var H = (P.X + P.Y) * (Q.X + Q.Y) % fieldChar;

            var X3 = A * F * (H + 2 * fieldChar - C - D) % fieldChar;
            var Y3 = A * G * (D - C + fieldChar) % fieldChar;
            var Z3 = F * G % fieldChar;

            return new ProectivPoint(X3, Y3, Z3);
        }

        private ProectivPoint MultiplyPoint(ProectivPoint A, BigInteger k)
        {
            var res = new ProectivPoint(0, 1, 1);
            var tmp = A;
            while (k != 0)
            {
                if (k % 2 == 1) //В этом месте основное отличие кривых в форме Эдвардса от кривых Вейерштраса: 
                                //здесь алгоритм сложения одинаковых точек ничем не отличается от алгоритма сложения различных точек.
                {
                    res = AdditionPoints(tmp, res);
                }
                tmp = AdditionPoints(tmp, tmp);
                k = k / 2;
            }
            return res;
        }

        public IPoint MultiplyPoint(IPoint P, BigInteger k)
        {
            return (IPoint)this.MultiplyPoint((ProectivPoint)P, k);
        }
        public IPoint AddPoint(IPoint P, IPoint Q)
        {
            return (IPoint)this.AdditionPoints((ProectivPoint)P, (ProectivPoint)Q);
        }

        public string getCurveName()
        {
            return "Кривая Эдвардса (проект.)";
        }
    }
}
