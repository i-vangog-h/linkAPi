namespace linkApi.Entities;
public partial class Url
{
    public string HashKey { get; set; } = null!;

    public string OriginalUrl { get; set; } = null!;

    public DateOnly? CreatedAt { get; set; }

    public int? AccessCount { get; set; }

    public Url()
    {
        CreatedAt = new DateOnly();
    }
}
