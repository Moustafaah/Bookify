namespace Infrastructure.Effects.FileSystem;

public static class FileSystemHelpers
{
    public static (bool IsValid, Error? Error) IsValidPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return (false, Error.New($"Path cannot be empty."));
        char[] invalidChars = Path.GetInvalidPathChars();
        var idx = path.IndexOfAny(invalidChars);
        if (idx >= 0)
            return (false, Error.New($"Path contain invalid character {invalidChars[idx]}."));

        return (true, null);
    }
}