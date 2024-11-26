namespace FolkerKinzel.DataUrls.Intls;

internal static class FileService
{

    [ExcludeFromCodeCoverage]
    internal static byte[] LoadFile(string path)
    {
        try
        {
            return File.ReadAllBytes(path);
        }
        catch (ArgumentNullException)
        {
            throw new ArgumentNullException(nameof(path));
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException(e.Message, nameof(path), e);
        }
        catch (UnauthorizedAccessException e)
        {
            throw new IOException(e.Message, e);
        }
        catch (NotSupportedException e)
        {
            throw new ArgumentException(e.Message, nameof(path), e);
        }
        catch (System.Security.SecurityException e)
        {
            throw new IOException(e.Message, e);
        }
        catch (PathTooLongException e)
        {
            throw new ArgumentException(e.Message, nameof(path), e);
        }
        catch (Exception e)
        {
            throw new IOException(e.Message, e);
        }
    }

    //[ExcludeFromCodeCoverage]
    //internal static void SaveFile(string path, byte[] bytes)
    //{
    //    try
    //    {
    //        File.WriteAllBytes(path, bytes);
    //    }
    //    catch (ArgumentNullException)
    //    {
    //        throw new ArgumentNullException(nameof(path));
    //    }
    //    catch (ArgumentException e)
    //    {
    //        throw new ArgumentException(e.Message, nameof(path), e);
    //    }
    //    catch (UnauthorizedAccessException e)
    //    {
    //        throw new IOException(e.Message, e);
    //    }
    //    catch (NotSupportedException e)
    //    {
    //        throw new ArgumentException(e.Message, nameof(path), e);
    //    }
    //    catch (System.Security.SecurityException e)
    //    {
    //        throw new IOException(e.Message, e);
    //    }
    //    catch (PathTooLongException e)
    //    {
    //        throw new ArgumentException(e.Message, nameof(path), e);
    //    }
    //    catch (Exception e)
    //    {
    //        throw new IOException(e.Message, e);
    //    }
    //}
}