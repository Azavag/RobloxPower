using UnityEngine;

public class PowerValueText : MonoBehaviour
{
    public static string ConvertPowerValueText(int powerValue)
    {
        if (powerValue >= 1000000000)
        {
            return $"{powerValue / 1000000000}.{powerValue % 1000000000 / 10000000}B";
        }
        if (powerValue >= 1000000)
        {
            return $"{powerValue / 1000000}.{powerValue % 1000000 / 100000}M";
        }
        if (powerValue >= 1000)
        {
            return  $"{powerValue / 1000}.{powerValue % 1000 / 100}K";
        }
        return powerValue.ToString();
    }
}
