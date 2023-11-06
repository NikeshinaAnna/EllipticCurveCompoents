using System;

namespace EllipticCurveCompoents
{
    public class CartesianPoint : IPoint
    {
        public BigInteger x;
        public BigInteger y;

        public CartesianPoint() {}
        public CartesianPoint(BigInteger x, BigInteger y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
