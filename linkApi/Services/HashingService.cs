namespace linkApi.Services;
using System.Text;

public class HashingService
{
    //returns hashed url wich will be used as a pattern
    public string EncodeBase64(string url)
    {
        byte[] urlBytes = Encoding.UTF8.GetBytes(url);
        return Convert.ToBase64String(urlBytes);
    }
}
