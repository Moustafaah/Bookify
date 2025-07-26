using LanguageExt.Traits;

using static Infrastructure.Effects.FileSystem.FileSystemHelpers;

namespace Infrastructure.Effects;
public static class DirectoryIO<M> where M : MonadIO<M>, Fallible<M>
{


    public static K<M, bool> Exists(string path)
    {
        return M.LiftIO(IO.lift(() => Directory.Exists(path)));
    }

    public static K<M, DirectoryInfo> CreateDirectory(string path)
    {
        var (isValid, error) = IsValidPath(path);
        return isValid ? M.LiftIO(IO.lift(() => Directory.CreateDirectory(path))) : M.Fail<DirectoryInfo>(error);

    }

    public static K<M, Unit> Delete(string path)
    {
        return from b in Exists(path)
               from _ in b
                   ? M.LiftIO(IO.lift(() =>
                   {
                       Directory.Delete(path);
                       return unit;
                   }))
                   : M.Fail<Unit>(Error.New($"Specified {path} directory does not exist."))

               select unit;

    }
    public static K<M, Unit> Delete(string path, bool recursive)
    {
        return from b in Exists(path)
               from _ in b
                   ? M.LiftIO(IO.lift(() =>
                   {
                       Directory.Delete(path, recursive);
                       return unit;
                   }))
                   : M.Fail<Unit>(Error.New($"Specified {path} directory does not exist."))

               select unit;

    }



    public static K<M, string[]> GetFiles(string path)
    {
        var (isValid, error) = IsValidPath(path);
        return from a in when(!isValid, M.Fail<Unit>(error!))
               from b in Exists(path)
               from result in b
                   ? M.LiftIO(IO.lift(() => Directory.GetFiles(path)))
                   : M.Fail<string[]>(Error.New($"Specified {path} directory does not exist."))

               select result;


    }

    public static K<M, string[]> GetFiles(string path, string searchPattern)
    {
        var (isValid, error) = IsValidPath(path);
        return from a in when(!isValid, M.Fail<Unit>(error!))
               from b in Exists(path)
               from result in b
                   ? M.LiftIO(IO.lift(() => Directory.GetFiles(path, searchPattern)))
                   : M.Fail<string[]>(Error.New($"Specified {path} directory does not exist."))

               select result;
    }

    public static K<M, string[]> GetDirectories(string path)
    {
        var (isValid, error) = IsValidPath(path);
        return from a in when(!isValid, M.Fail<Unit>(error!))
               from b in Exists(path)
               from result in b
                   ? M.LiftIO(IO.lift(() => Directory.GetDirectories(path)))
                   : M.Fail<string[]>(Error.New($"Specified {path} directory does not exist."))
               select result;
    }

    public static K<M, string[]> GetDirectories(string path, string searchPattern)
    {
        var (isValid, error) = IsValidPath(path);
        return from a in when(!isValid, M.Fail<Unit>(error!))
               from b in Exists(path)
               from result in b
                   ? M.LiftIO(IO.lift(() => Directory.GetDirectories(path, searchPattern)))
                   : M.Fail<string[]>(Error.New($"Specified {path} directory does not exist."))
               select result;
    }

    public static K<M, string[]> GetFileSystemEntries(string path)
    {
        var (isValid, error) = IsValidPath(path);
        return from a in when(!isValid, M.Fail<Unit>(error!))
               from b in Exists(path)
               from result in b
                   ? M.LiftIO(IO.lift(() => Directory.GetFileSystemEntries(path)))
                   : M.Fail<string[]>(Error.New($"Specified {path} directory does not exist."))
               select result;
    }

    public static K<M, string[]> GetFileSystemEntries(string path, string searchPattern)
    {
        var (isValid, error) = IsValidPath(path);
        return from a in when(!isValid, M.Fail<Unit>(error!))
               from b in Exists(path)
               from result in b
                   ? M.LiftIO(IO.lift(() => Directory.GetFileSystemEntries(path, searchPattern)))
                   : M.Fail<string[]>(Error.New($"Specified {path} directory does not exist."))
               select result;
    }

    public static K<M, Unit> Move(string sourceDirName, string destDirName)
    {
        var (isValid1, error1) = IsValidPath(sourceDirName);
        var (isValid2, error2) = IsValidPath(destDirName);
        return from _1 in when(!isValid1, M.Fail<Unit>(error1!))
               from _2 in when(!isValid2, M.Fail<Unit>(error2!))
               from _3 in when(!Directory.Exists(sourceDirName),
                   M.Fail<Unit>(Error.New($"Specified {sourceDirName} directory does not exist.")))
               from _4 in when(!Directory.Exists(destDirName),
                   M.Fail<Unit>(Error.New($"Specified {destDirName} directory does not exist.")))
               from _6 in M.LiftIO(IO.lift(() => Directory.Move(sourceDirName, destDirName)))
               select unit;


    }

    public static K<M, DateTime> GetCreationTime(string path)
    {
        var (isValid, error) = IsValidPath(path);
        return from _1 in when(!isValid, M.Fail<Unit>(error!))
               from _2 in when(!Directory.Exists(path),
                   M.Fail<Unit>(Error.New($"Specified {path} directory does not exist.")))
               from res in M.LiftIO(IO.lift(() => Directory.GetCreationTime(path)))
               select res;
    }

    public static K<M, Unit> SetCreationTime(string path, DateTime creationTime)
    {

        var (isValid, error) = IsValidPath(path);
        return from _1 in when(!isValid, M.Fail<Unit>(error!))
               from _2 in when(!Directory.Exists(path),
                   M.Fail<Unit>(Error.New($"Specified {path} directory does not exist.")))
               from _3 in M.LiftIO(IO.lift(() => Directory.SetCreationTime(path, creationTime)))
               select unit;
    }
}