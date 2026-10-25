namespace URLShortener.Services;

public interface IBase62Converter
{ 
    string ToBase62String(ulong id);
    ulong ToULong(string value);
}