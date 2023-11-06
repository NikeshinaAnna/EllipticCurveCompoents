
namespace EllipticCurveCompoents
{
    public class EdwardsCurveCartesianCoordinate:IEllipticCurve
    {
        private static BigInteger fieldChar { get; set; }
        private static BigInteger e { get; set; }
        private static BigInteger d { get; set; }

        public EdwardsCurveCartesianCoordinate(BigInteger _fieldChar, BigInteger _e, BigInteger _d)
        {
            fieldChar = _fieldChar;
            e = _e;
            d = _d;
        }

        private CartesianPoint AdditionPoints(CartesianPoint p, CartesianPoint q)
        {
            if (p.x == 0 && p.y == 1)
                return q;
            if (q.x == 0 && q.y == 1)
                return p;
            BigInteger multiCoord = (d * p.x * p.y * q.y * q.x) % fieldChar;
            while (multiCoord < 0)
                multiCoord += fieldChar;
            BigInteger x_numerator = (p.x * q.y + p.y * q.x) % fieldChar;
            BigInteger x_denominator = (1 + multiCoord) % fieldChar;
            BigInteger x = x_numerator * Operations.ExtendedEuclid(fieldChar, x_denominator) % fieldChar;

            BigInteger y_numerator = (p.y * q.y - p.x * q.x) % fieldChar;
            while (y_numerator < 0)
                y_numerator += fieldChar;
            BigInteger y_denominator = (1 - multiCoord);
            while (y_denominator < 0)
                y_denominator += fieldChar;
            BigInteger y = y_numerator * Operations.ExtendedEuclid(fieldChar, y_denominator) % fieldChar;

            return new CartesianPoint(x, y);
        }

        private CartesianPoint MultiplyPoint(CartesianPoint point, BigInteger k)
        {
            CartesianPoint result = new CartesianPoint(0, 1);// у кривой Эдвардса нейтраьная точка (0,1) ??
            var tmp = point;
            while (k != 0)
            {
                if (k % 2 == 1)
                {
                    result = AdditionPoints(tmp, result);
                }
                tmp = AdditionPoints(tmp, tmp);
                k = k / 2;
            }
            return result;
        }

        public IPoint MultiplyPoint(IPoint P, BigInteger k)
        {
            return (IPoint)this.MultiplyPoint((CartesianPoint)P, k);
        }
        public IPoint AddPoint(IPoint P, IPoint Q)
        {
            return (IPoint)this.AdditionPoints((CartesianPoint)P, (CartesianPoint)Q);
        }
        
        public string getCurveName()
        {
            return "Кривая Эдвардса (декарт.)";
        }
    }
}
