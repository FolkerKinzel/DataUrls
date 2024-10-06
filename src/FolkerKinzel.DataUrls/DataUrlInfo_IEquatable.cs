namespace FolkerKinzel.DataUrls;

public readonly partial struct DataUrlInfo : IEquatable<DataUrlInfo>
{
    /// <summary>
    /// Determines whether <paramref name="obj"/> is a <see cref="DataUrlInfo"/> structure whose
    /// value is equal to that of this instance.
    /// </summary>
    /// <param name="obj">The <see cref="object"/> to compare with.</param>
    /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="DataUrlInfo"/> structure whose
    /// value is equal to that of this instance; <c>false</c>, otherwise.</returns>
    public override bool Equals(object? obj)
        => obj is DataUrlInfo other && Equals(in other);

    /// <summary>
    /// Determines whether the value of this instance is equal to the value of <paramref name="other"/>. 
    /// </summary>
    /// <param name="other">The <see cref="DataUrlInfo"/> instance to compare with.</param>
    /// <returns><c>true</c> if this the value of this instance is equal to that of <paramref name="other"/>; <c>false</c>, otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(DataUrlInfo other) => Equals(in other);

    /// <summary>
    /// Determines whether the value of this instance is equal to the value of <paramref name="other"/>. 
    /// </summary>
    /// <param name="other">The <see cref="DataUrlInfo"/> instance to compare with.</param>
    /// <returns><c>true</c> if this the value of this instance is equal to that of <paramref name="other"/>; <c>false</c>, otherwise.</returns>
    /// <remarks>This is the most performant overload of the Equals methods but unfortunately it's not CLS compliant.
    /// Use it if you can.</remarks>
    [CLSCompliant(false)]
    public bool Equals(in DataUrlInfo other)
        => this.IsEmpty || other.IsEmpty
            ? this.IsEmpty && other.IsEmpty
            : EqualsMimeType(in other) && EqualsData(in other);

    #region private

    private bool EqualsMimeType(in DataUrlInfo other)
    {
        bool thisParsed = MimeTypeInfo.TryParse(this.MimeType, out MimeTypeInfo thisMime);
        bool otherParsed = MimeTypeInfo.TryParse(other.MimeType, out MimeTypeInfo otherMime);

        return !(thisParsed || otherParsed) || thisMime.Equals(otherMime, true);
    }

    private bool EqualsData(in DataUrlInfo other)
        => this.ContainsEmbeddedText
            ? EqualsText(in other)
            : this.DataEncoding == DataEncoding.Base64 && other.DataEncoding == DataEncoding.Base64
                ? this.Data.Equals(other.Data, StringComparison.Ordinal)
                : EqualsBytes(in other);

    private bool EqualsText(in DataUrlInfo other)
    {
        if (other.TryGetEmbeddedText(out string? otherText))
        {
            if (TryGetEmbeddedText(out string? thisText))
            {
                return StringComparer.Ordinal.Equals(thisText, otherText);
            }
        }

        return false;
    }

    private bool EqualsBytes(in DataUrlInfo other)
    {
        if (other.TryGetEmbeddedBytes(out byte[]? otherBytes))
        {
            if (TryGetEmbeddedBytes(out byte[]? thisBytes))
            {
                return thisBytes.SequenceEqual(otherBytes);
            }
        }

        return false;
    }

    #endregion
}
