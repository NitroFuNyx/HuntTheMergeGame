using System;
using Random = UnityEngine.Random;

[Serializable]
public struct Secureint
{
    public int valueOffset;
    public int valueAmount;

    public Secureint(int meatValue)
    {
        valueOffset = Random.Range(-1000, 1000);
        valueAmount = meatValue + valueOffset;
    }

    public int GetValue()
    {
        return valueAmount - valueOffset;
    }

    public override string ToString()
    {
        return GetValue().ToString();
    }

    public static Secureint operator +(Secureint i1, Secureint i2)
    {
        return new Secureint(i1.GetValue() + i2.GetValue());
    }

    public static Secureint operator -(Secureint i1, Secureint i2)
    {
        return new Secureint(i1.GetValue() - i2.GetValue());
    }

    public static Secureint operator *(Secureint i1, Secureint i2)
    {
        return new Secureint(i1.GetValue() * i2.GetValue());
    }

    public static Secureint operator /(Secureint i1, Secureint i2)
    {
        return new Secureint(i1.GetValue() / i2.GetValue());
    }
}