namespace FolkerKinzel.DataUrls.Intls;

/// <summary>
/// Extension methods, which support the <see cref="DataUrlInfo"/> structure.
/// </summary>
internal static class DataUrlExtension
{
    internal static StringBuilder AppendMediaType(this StringBuilder builder, in MimeTypeInfo mimeType)
    {
        if (mimeType.IsTextPlain)
        {
            foreach (MimeTypeParameterInfo parameter in mimeType.Parameters())
            {
                _ = builder.Append(';');
                parameter.AppendTo(builder, urlFormat: true);
            }

            return builder;
        }

        mimeType.AppendTo(builder, MimeFormats.Url);
        return builder;
    }

}
