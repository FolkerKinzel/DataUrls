namespace FolkerKinzel.DataUrls;

/// <summary>
/// Encapsulates the data embedded in a "data" URL.
/// This can be either an array of <see cref="byte"/>s 
/// or a <see cref="string"/>.
/// </summary>
/// <example>
/// <note type="note">
/// For the sake of better readability, exception handling is ommitted in the example.
/// </note>
/// <para>
/// Creating and parsing a "data" URL:
/// </para>
/// <code language="c#" source="./../Examples/DataUrlExample.cs"/>
/// </example>
public readonly struct EmbeddedData
{
    private readonly object? _object;

    private EmbeddedData(object obj) { _object = obj; }

    internal static EmbeddedData FromBytes(byte[] bytes) => new(bytes);

    internal static EmbeddedData FromText(string str) => new(str);

    /// <summary>
    /// Gets the embedded <see cref="byte"/> array,
    /// or <c>null</c>, if the embedded data is a <see cref="string"/>.
    /// </summary>
    public byte[]? Bytes => _object is null ? [] : _object as byte[];

    /// <summary>
    /// Gets the embedded text,
    /// or <c>null</c>, if the embedded value can't be converted to
    /// a <see cref="string"/>.
    /// </summary>
    public string? Text => _object as string;

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// embedded data.
    /// </summary>
    /// <param name="bytesAction">The <see cref="Action{T}"/> to perform if the encapsulated value
    /// is an array of <see cref="byte"/>s, or <c>null</c>.</param>
    ///
    /// <param name="textAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>, or <c>null</c>.</param>
    /// 
    public void Switch(Action<byte[]>? bytesAction = null,
                       Action<string>? textAction = null)
    {
        if (Bytes is byte[] bytes)
        {
            bytesAction?.Invoke(bytes);
        }
        else
        {
            textAction?.Invoke((string)_object!);
        }
    }

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value and allows to pass an argument to the delegates.
    /// </summary>
    /// <typeparam name="TArg">Generic type parameter for the type of the argument to pass
    /// to the delegates.</typeparam>
    /// <param name="arg">The argument to pass to the delegates.</param>
    /// <param name="bytesAction">The <see cref="Action{T}"/> to perform if the encapsulated value
    /// is an array of <see cref="byte"/>s, or <c>null</c>.</param>
    /// <param name="textAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>, or <c>null</c>.</param>
    /// <example>
    /// <note type="note">
    /// For the sake of better readability, exception handling is ommitted in the example.
    /// </note>
    /// <para>
    /// Creating and parsing a "data" URL:
    /// </para>
    /// <code language="c#" source="./../Examples/DataUrlExample.cs"/>
    /// </example>
    public void Switch<TArg>(TArg arg,
                             Action<byte[], TArg>? bytesAction = null,
                             Action<string, TArg>? textAction = null)
    {
        if (Bytes is byte[] bytes)
        {
            bytesAction?.Invoke(bytes, arg);
        }
        else
        {
            textAction?.Invoke((string)_object!, arg);
        }
    }

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">Generic type parameter for the return type of the delegates.</typeparam>
    /// <param name="bytesFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated 
    /// value is an array of <see cref="byte"/>s.</param>
    /// <param name="textFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    public TResult Convert<TResult>(Func<byte[], TResult> bytesFunc,
                                    Func<string, TResult> textFunc)
        => Bytes switch 
        {
            byte[] bytes => bytesFunc is null ? throw new ArgumentNullException(nameof(bytesFunc)) : bytesFunc(bytes),
            _ => textFunc is null ? throw new ArgumentNullException(nameof(textFunc)) : textFunc((string)_object!)
        };

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/> and allows to specify an
    /// argument for the conversion.
    /// </summary>
    /// <typeparam name="TArg">Generic type parameter for the type of the argument to pass
    /// to the delegates.</typeparam>
    /// <typeparam name="TResult">Generic type parameter for the return type of the delegates.</typeparam>
    /// <param name="arg">The argument to pass to the delegates.</param>
    /// <param name="bytesFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated 
    /// value is an array of <see cref="byte"/>s.</param>
    /// <param name="textFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    public TResult Convert<TArg, TResult>(TArg arg,
                                          Func<byte[], TArg, TResult> bytesFunc,
                                          Func<string, TArg, TResult> textFunc)
        => Bytes switch {
            byte[] bytes => bytesFunc is null ? throw new ArgumentNullException(nameof(bytesFunc)) : bytesFunc(bytes, arg),
            _ => textFunc is null ? throw new ArgumentNullException(nameof(textFunc)) : textFunc((string)_object!, arg)
        };

    /// <summary>
    /// Returns a <see cref="string"/>-representation of the instance that
    /// helps in debugging.
    /// </summary>
    /// <returns>A <see cref="string"/>-representation of the instance.</returns>
    public override string ToString()
        => Bytes is byte[] bytes
              ? $"{bytes.GetType().FullName}: {bytes.Length} Bytes"
              : $"{typeof(string).FullName}: {Text}";
}

