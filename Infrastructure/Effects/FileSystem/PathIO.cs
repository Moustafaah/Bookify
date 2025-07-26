using LanguageExt.Traits;

namespace Infrastructure.Effects.FileSystem;
public sealed class PathIO<M> where M : MonadIO<M>, Fallible<M>
{


    public static K<M, string> Combine(string path1, string path2)
    {
        return PathHelpers.IsValidPath(path1) && PathHelpers.IsValidPath(path2)
            ? M.LiftIO(IO.lift(() => Path.Combine(path1, path2)))
            : M.Fail<string>(Error.New($"Path values specified are invalid"));
    }




    public static K<M, string> Combine(params string[] paths)
    {
        return paths.All(PathHelpers.IsValidPath)
            ? M.LiftIO(IO.lift(() => Path.Combine(paths)))
            : M.Fail<string>(Error.New($"Path values specified are invalid"));
    }

    public static K<M, string> GetFileName(string path)
    {
        return PathHelpers.IsValidPath(path)
            ? M.LiftIO(IO.lift(() => Path.GetFileName(path)))
            : M.Fail<string>(Error.New($"Path values specified are invalid"));
    }


    public static K<M, Option<string>> GetExtension(string path)
    {
        return PathHelpers.IsValidPath(path)
            ? M.LiftIO(IO.lift(() => Optional(Path.GetExtension(path))))
            : M.Fail<Option<string>>(Error.New($"Path values specified are invalid"));
    }



    public static K<M, Option<string>> GetDirectoryName(string path)
    {
        return PathHelpers.IsValidPath(path)
            ? M.LiftIO(IO.lift(() => Optional(Path.GetDirectoryName(path))))
            : M.Fail<Option<string>>(Error.New($"Path values specified are invalid"));
    }

    public static K<M, string> GetFullPath(string path)
    {
        return PathHelpers.IsValidPath(path)
            ? M.LiftIO(IO.lift(() => Path.GetFullPath(path)))
            : M.Fail<string>(Error.New($"Path values specified are invalid\""));
    }


    public static K<M, bool> HasExtension(string path)
    {
        return PathHelpers.IsValidPath(path)
            ? M.LiftIO(IO.lift(() => Path.HasExtension(path)))
            : M.Fail<bool>(Error.New($"Path values specified are invalid"));
    }

    public static K<M, string> ChangeExtension(string path, string extension)
    {
        return PathHelpers.IsValidPath(path)
            ? M.LiftIO(IO.lift(() => Path.ChangeExtension(path, extension)))
            : M.Fail<string>(Error.New($"Path values specified are invalid"));
    }

}


public static class PathHelpers
{
    public static bool IsValidPath(string path)
    {

        return Path.GetInvalidPathChars().Any(path.Contains);
    }

    public static (bool IsValid, Error? Error) HasCorrectFileInfo(string path)
    {
        return Try.lift(() => new FileInfo(path)).Run().Match(_ => (true, null)!,
             e => (false, Error.New($"Could not parse file infos for {path}")));
    }
}
