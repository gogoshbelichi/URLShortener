using System.Text;

namespace URLShortener.Services;

public class Base62Converter : IBase62Converter
{
    private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    
    public string ToBase62String(ulong id)
    {
        if (id == 0) return Alphabet[0].ToString();

        var sb = new StringBuilder();

        while (id > 0)
        {
            sb.Insert(0, Alphabet[(int)(id % 62)]);
            id /= 62;
        }
        
        return sb.ToString();
    }
    
    public ulong ToULong(string value)
    {
        ulong result = 0;
        
        foreach (var c in value)
            result = checked(result * 62 + (ulong)Alphabet.IndexOf(c));
        
        return result;
    }
}