using System;

namespace EllipticCurveCompoents
{
    public interface IEllipticCurve
    {
        IPoint MultiplyPoint(IPoint P, BigInteger k);
        IPoint AddPoint(IPoint P, IPoint Q);
        string getCurveName();
    }
}
