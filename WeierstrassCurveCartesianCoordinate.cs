using System;

namespace EllipticCurveCompoents
{
    public class WeierstrassCurveCartesianCoordinate:IEllipticCurve
    {
        private static BigInteger fieldChar { get; set; }
        private static BigInteger a { get; set; }
        private static BigInteger b { get; set; }

        public WeierstrassCurveCartesianCoordinate(BigInteger _fieldChar, BigInteger _a, BigInteger _b)
        {
            fieldChar = _fieldChar;
            a = _a;
            b = _b;
        }

        private CartesianPoint AdditionPoints(CartesianPoint P, CartesianPoint Q)
        {
            if (P.x == 0 && P.y == 0)
                return Q;
            if (Q.x == 0 && Q.y == 0)
                return P;
            BigInteger x_diff = P.x - Q.x;
            BigInteger y_diff = P.y - Q.y;
            if (x_diff < 0)
                x_diff += fieldChar;
            if (y_diff < 0)
                y_diff += fieldChar;

            BigInteger m = y_diff * Operations.ExtendedEuclid(fieldChar, x_diff) % fieldChar;
            (BigInteger x, BigInteger y) = getCoordinates(fieldChar, P.x, P.y, Q.x, m);

            return new CartesianPoint(x, y);
        }

        private CartesianPoint DoublePoint(CartesianPoint P)
        {
            BigInteger y_reverse = Operations.ExtendedEuclid(fieldChar, 2 * P.y % fieldChar);
            BigInteger m = (3 * P.x * P.x % fieldChar + a) * y_reverse % fieldChar;
            (BigInteger x, BigInteger y) = getCoordinates(fieldChar, P.x, P.y, P.x, m);
            return new CartesianPoint(x, y);
        }

        private CartesianPoint MultiplyPoint(CartesianPoint P, BigInteger k)
        {
            CartesianPoint result = new CartesianPoint(0, 0);
            var tmp = P;
            while (k != 0)
            {
                if (k % 2 == 1)
                    result = AdditionPoints(tmp, result);
                tmp = DoublePoint(tmp);
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

        private (BigInteger, BigInteger) getCoordinates(BigInteger p, BigInteger x_p, BigInteger y_p, BigInteger x_q, BigInteger m)
        {
            BigInteger x = (m * m - x_p - x_q) % p;
            if (x < 0)
                x += p;

            BigInteger x_diff = (x_p - x + p) % p;
            BigInteger y = (m * x_diff - y_p + p) % p;
            return (x, y);
        }

        public string getCurveName()
        {
            return "В форме Вейерштрасса (декарт.)";
        }
    }
}
