namespace MalodyKey2DR3;

public class Util
{
    public static string FloatToDRBDecimal(float dec)
    {
        string qwq = dec.ToString("0.00");
        return qwq.EndsWith(".00") ? qwq.Substring(0, qwq.Length - 3) : qwq;
    }
}