namespace linkApi.Services;
using System.Text;

public class HashingService
{
    // Url can only consist of one of these 62 characters
    private readonly char[] baseLiterals = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    //returns hashed url wich will be used as a pattern

    public string EncodeBase10To62(int base10Id)
    {
        StringBuilder sb = new StringBuilder();
        int n = base10Id;
        while(n > 0)
        {
            sb.Append(baseLiterals[n % 62]);
            n = n / 62;
        }
        return Reverse(sb.ToString());
    }

    private string Reverse(string hash)
    {
        char[] chars = hash.ToCharArray();
        int r = hash.Length - 1;
        for (int l = 0; l < r; l++, r--)
        {
            char temp = chars[l];
            chars[l] = chars[r];
            chars[r] = temp;
        }
        return new string(chars);
    }

    public int DecodeBase62To10(string base62Hash)
    {
        return 0;
    }


}
