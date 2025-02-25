namespace FolkerKinzel.DataUrls;

/// <summary>
/// Named constants to specify the type of data embedded in a "data" URL.
/// </summary>
public enum DataType
{
    /// <summary>
    /// The embedded data represents a <see cref="string"/>.
    /// </summary>
    Text,

    /// <summary>
    /// The embedded data represents a <see cref="byte"/> array that can't be 
    /// converted to a <see cref="string"/>.
    /// </summary>
    Binary = 4
}