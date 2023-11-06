using System;

namespace EllipticCurveCompoents
{
    public class ProectivPoint : IPoint
    {
        public BigInteger X;
        public BigInteger Y;
        public BigInteger Z;

        public ProectivPoint(BigInteger x, BigInteger y, BigInteger z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public CartesianPoint GetCartesianPointFromProject(BigInteger fieldCharacteristic)
        {
            var z_inverse = Operations.ExtendedEuclid(fieldCharacteristic, Z);
            var x = X * z_inverse % fieldCharacteristic;
            var y = Y * z_inverse % fieldCharacteristic;

            return new CartesianPoint(x, y);
        }
    }
}
