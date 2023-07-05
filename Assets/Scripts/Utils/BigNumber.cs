using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;

[System.Serializable]
public class BigNumber
{
    [SerializeField] private string _value;

    public BigInteger Value { get => BigInteger.Parse(string.IsNullOrEmpty(_value) ? "0" : _value); set => _value = value == null ? "0" : value.ToString(); }

    public override string ToString()
    {
        BigInteger divisor = new BigInteger(1000);
        BigInteger coeff = Value;
        BigInteger rem;
        int power = 0;

        while (coeff >= divisor)
        {
            BigInteger divResult = BigInteger.DivRem(coeff, divisor, out BigInteger remainder);

            coeff = divResult;
            rem = remainder;
            power++;
        }

        string suffix = CalculateSuffix(power);
        string decimalPart = Mathf.RoundToInt((float)rem / 10f).ToString("D2");
        string result = $"{coeff}.{decimalPart}{suffix}";

        return result;
    }

    public string CalculateSuffix(int value)
    {
        string suffix = "";
        while (value > 0)
        {
            var remainder = (value - 1) % 26;
            value = (value - 1) / 26;
            suffix = (char)(remainder + 97) + suffix;
        }
        return suffix;
    }
}
