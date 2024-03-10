namespace FolkerKinzel.DataUrls;

public readonly partial struct DataUrlInfo
{
    #region Operators

    /// <summary>
    /// Returns a value that indicates whether the values of two specified <see cref="DataUrlInfo"/> instances are equal.
    /// </summary>
    /// <param name="value1">The first <see cref="DataUrlInfo"/> to compare.</param>
    /// <param name="value2">The second <see cref="DataUrlInfo"/> to compare.</param>
    /// <returns><c>true</c> if the values of <paramref name="value1"/> and <paramref name="value2"/> are equal;
    /// otherwise, <c>false</c>.</returns>
    public static bool operator ==(DataUrlInfo value1, DataUrlInfo value2) => value1.Equals(in value2);

    /// <summary>
    /// Returns a value that indicates whether the values of two specified <see cref="DataUrlInfo"/> instances are not equal.
    /// </summary>
    /// <param name="value1">The first <see cref="DataUrlInfo"/> to compare.</param>
    /// <param name="value2">The second <see cref="DataUrlInfo"/> to compare.</param>
    /// <returns><c>true</c> if the values of <paramref name="value1"/> and <paramref name="value2"/> are not equal;
    /// otherwise, <c>false</c>.</returns>
    public static bool operator !=(DataUrlInfo value1, DataUrlInfo value2) => !value1.Equals(in value2);

    #endregion


}
