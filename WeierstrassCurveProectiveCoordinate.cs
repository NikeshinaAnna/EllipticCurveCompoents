using System;

namespace EllipticCurveCompoents
{
    public class WeierstrassCurveProectiveCoordinate:IEllipticCurve
    {
        private static BigInteger fieldChar { get; set; }
        private static BigInteger a { get; set; }
        private static BigInteger b { get; set; }

        public WeierstrassCurveProectiveCoordinate(BigInteger _fieldChar, BigInteger _a, BigInteger _b)
        {
            fieldChar = _fieldChar;
            a = _a;
            b = _b;
        }

        private ProectivPoint AdditionPoints(ProectivPoint P, ProectivPoint Q)
        {
            //var result = new ProectiveECPoint(0, 1, 0);//результат
            if (P.Z == 0)
                return Q;
            if (Q.Z == 0)
                return P;

            var T1 = P.Y * Q.Z % fieldChar;
            var T2 = P.Z * Q.Y % fieldChar;
            var U1 = P.X * Q.Z % fieldChar;
            var U2 = P.Z * Q.X % fieldChar;
            var U = (U1 - U2) % fieldChar;
            var T = (T1 - T2) % fieldChar;
            var V = Q.Z * P.Z % fieldChar;
            var W = (T * T * V - (U1 + U2) * U * U) % fieldChar;

            BigInteger x_new = (W * U);
            x_new = x_new % fieldChar;
            while (x_new < 0)
                x_new += fieldChar;

            BigInteger y_new = (T * (U1 * U * U - W) - T1 * U * U * U);
            y_new = y_new % fieldChar;
            while (y_new < 0)
                y_new += fieldChar;

            BigInteger z_new = (V * U * U * U);
            z_new = z_new % fieldChar;
            while (z_new < 0)
                z_new += fieldChar;

            return new ProectivPoint(x_new, y_new, z_new);
        }

        private ProectivPoint DoublePoint(ProectivPoint P)
        {
            var U = 2 * P.Y * P.Z % fieldChar;
            var T = (3 * P.X * P.X + a * P.Z * P.Z) % fieldChar;
            var V = 2 * U * P.X * P.Y % fieldChar;
            var W = (T * T - 2 * V) % fieldChar;
            BigInteger x_new = (W * U);
            x_new = x_new % fieldChar;
            while (x_new < 0)
                x_new += fieldChar;

            BigInteger y_new = (T * (V - W) - 2 * U * U * P.Y * P.Y);
            y_new = y_new % fieldChar;
            while (y_new < 0)
                y_new += fieldChar;

            BigInteger z_new = (U * U * U);
            z_new = z_new % fieldChar;
            while (z_new < 0)
                z_new += fieldChar;

            return new ProectivPoint(x_new, y_new, z_new);
        }

        public ProectivPoint MultiplyPoint(ProectivPoint P, BigInteger k)
        {
            ProectivPoint result = new ProectivPoint(0, 1, 0); // здесь такая точка нейтральная?????
            var tmp = P;
            while (k != 0)
            {
                if (k % 2 == 1)
                {
                    result = AdditionPoints(tmp, result);
                }
                tmp = DoublePoint(tmp);
                k = k / 2;
            }
            return result;
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
            return "В форме Вейерштрасса (проект.)";
        }
    }
}
