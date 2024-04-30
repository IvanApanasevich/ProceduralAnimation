using UnityEngine;

public class SecondOrderDynamics
{
    private Vector3 _xp, _xd;
    private Vector3 _yd, _y;
    private readonly float _k1, _k2, _k3;

    public SecondOrderDynamics(float f, float z, float r, Vector3 x0)
    {
        _k1 = z / (Mathf.PI * f);
        _k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        _k3 = r * z / (2 * Mathf.PI * f);

        _xp = x0;
        _y = x0;
        _yd = Vector3.zero;
    }

    public Vector3 Update(float T, Vector3 x)
    {
        _xd = (x - _xp) / T;
        _xp = x;
        float k2Stable = Mathf.Max(_k2, T * T / 2 + T * _k1 / 2, T * _k1);
        _y = _y + T * _yd;
        _yd = _yd + T * (x + _k3 * _xd - _y - _k1 * _yd) / k2Stable;

        return _y;
    }
}
