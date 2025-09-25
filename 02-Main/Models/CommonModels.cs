namespace StairwayDesigns.Models;

public class FormatJSON<T> : IFormatJSON<T> where T : class
{
    public IEnumerable<T> JsonReader(string rows)
    {
        return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<T>>(rows);
    }

    public T JsonReaderSingle(string rows)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(rows);
    }
}

public interface IFormatJSON<T>
{
    IEnumerable<T> JsonReader(string rows);
    T JsonReaderSingle(string rows);
}

/// <summary>
/// Represents an image gallery item with URL, alt text, and caption
/// </summary>
public class ImageGalleryItem
{
    public string ImageUrl { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
}

public interface IAppConfig
{
    string OnrEmail { get; set; }
    string AppOwner { get; set; }
    string SourceID { get; set; }
}

public class AppConfig : IAppConfig
{
    public string OnrEmail { get; set; } = string.Empty;
    public string AppOwner { get; set; } = string.Empty;
    public string SourceID { get; set; } = string.Empty;
}