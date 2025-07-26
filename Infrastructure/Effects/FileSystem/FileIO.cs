using System.Text;

using LanguageExt.Traits;

using static Infrastructure.Effects.FileSystem.FileSystemHelpers;

namespace Infrastructure.Effects.FileSystem;
public static class FileIO<M> where M : MonadIO<M>, Fallible<M>
{
    public static K<M, bool> Exists(string path)
    {
        return M.LiftIO(IO.lift(() => File.Exists(path)));
    }

    public static K<M, Unit> Copy(string sourceFileName, string destFileName)
    {
        return from _1 in CheckFileExists(sourceFileName)
               from _2 in CheckFileExists(destFileName)
               from _3 in M.LiftIO(IO.lift(() => File.Copy(sourceFileName, destFileName)))
               select unit;


    }
    public static K<M, Unit> Copy(string sourceFileName, string destFileName, bool overwrite)
    {
        return from _1 in CheckFileExists(sourceFileName)
               from _2 in CheckFileExists(destFileName)
               from _3 in M.LiftIO(IO.lift(() => File.Copy(sourceFileName, destFileName, overwrite)))
               select unit;


    }

    public static K<M, Unit> Delete(string path)
    {
        return from b in Exists(path)
               from _ in b
                   ? M.LiftIO(IO.lift(() =>
                   {
                       File.Delete(path);
                       return unit;
                   }))
                   : M.Fail<Unit>(Error.New($"Specified file '{path}' does not exist."))

               select unit;

    }

    public static K<M, FileStream> Create(string path)
    {
        {
            return from _1 in CheckFileExists(path)
                   from _2 in when(!Path.HasExtension(path),
                       M.Fail<Unit>(Error.New($"Please Specify file extension for file.")))
                   from res in M.LiftIO(IO.lift(() => File.Create(path)))
                   select res;

        }
    }

    public static K<M, FileStream> Create(string path, int bufferSize)
    {
        {
            return from _1 in CheckFileExists(path)
                   from _2 in when(!Path.HasExtension(path),
                       M.Fail<Unit>(Error.New($"Please Specify file extension for file.")))
                   from res in M.LiftIO(IO.lift(() => File.Create(path, bufferSize)))
                   select res;

        }
    }

    public static K<M, FileStream> Create(string path, int bufferSize, FileOptions options)
    {
        return from _1 in CheckFileExists(path)
               from _2 in when(!Path.HasExtension(path),
                   M.Fail<Unit>(Error.New($"Please Specify file extension for file.")))
               from res in M.LiftIO(IO.lift(() => File.Create(path, bufferSize, options)))
               select res;

    }

    public static K<M, Unit> Move(string sourceFileName, string destFileName)
    {
        return from _1 in CheckFileExists(sourceFileName)
               from _2 in CheckFileExists(destFileName)
               from _3 in M.LiftIO(IO.lift(() => File.Move(sourceFileName, destFileName)))
               select unit;


    }

    public static K<M, Unit> Move(string sourceFileName, string destFileName, bool overwrite)
    {
        return from _1 in CheckFileExists(sourceFileName)
               from _2 in CheckFileExists(destFileName)
               from _3 in M.LiftIO(IO.lift(() => File.Move(sourceFileName, destFileName, overwrite)))
               select unit;


    }
    public static K<M, string> ReadAllText(string path)
    {
        return from _1 in CheckFileExists(path)
               from res in M.LiftIO(IO.lift(() => File.ReadAllText(path)))
               select res;


    }
    public static K<M, Unit> WriteAllText(string path, string contents)
    {
        return from _1 in CheckFileExists(path)
               from _2 in M.LiftIO(IO.lift(() => File.WriteAllText(path, contents)))
               select unit;


    }
    public static K<M, Unit> WriteAllText(string path, string contents, Encoding encoding)
    {
        return from _1 in CheckFileExists(path)
               from _2 in M.LiftIO(IO.lift(() => File.WriteAllText(path, contents, encoding)))
               select unit;


    }
    public static K<M, string[]> ReadAllLines(string path)
    {
        return from _1 in CheckFileExists(path)
               from res in M.LiftIO(IO.lift(() => File.ReadAllLines(path)))
               select res;


    }

    public static K<M, byte[]> ReadAllBytes(string path)
    {
        return from _1 in CheckFileExists(path)
               from res in M.LiftIO(IO.lift(() => File.ReadAllBytes(path)))
               select res;


    }

    public static K<M, Unit> WriteAllBytes(string path, byte[] bytes)
    {
        return from _1 in CheckFileExists(path)
               from _2 in M.LiftIO(IO.lift(() => File.WriteAllBytes(path, bytes)))
               select unit;


    }
    public static K<M, IEnumerable<string>> ReadLines(string path)
    {
        return from _1 in CheckFileExists(path)
               from res in M.LiftIO(IO.lift(() => File.ReadLines(path)))
               select res;


    }

    public static K<M, FileStream> Open(string path, FileMode mode)
    {
        return from _1 in CheckFileExists(path)
               from res in M.LiftIO(IO.lift(() => File.Open(path, mode)))
               select res;

    }

    public static K<M, FileStream> Open(string path, FileMode mode, FileAccess access)
    {
        return from _1 in CheckFileExists(path)
               from res in M.LiftIO(IO.lift(() => File.Open(path, mode, access)))
               select res;

    }

    public static K<M, FileStream> Open(string path, FileMode mode, FileAccess access, FileShare share)
    {
        return from _1 in CheckFileExists(path)
               from res in M.LiftIO(IO.lift(() => File.Open(path, mode, access, share)))
               select res;

    }
    public static K<M, FileStream> OpenRead(string path)
    {
        return from _1 in CheckFileExists(path)
               from res in M.LiftIO(IO.lift(() => File.OpenRead(path)))
               select res;

    }
    public static K<M, FileStream> OpenWrite(string path)
    {
        return from _1 in CheckFileExists(path)
               from res in M.LiftIO(IO.lift(() => File.OpenWrite(path)))
               select res;

    }

    public static K<M, Unit> SetAttributes(string path, FileAttributes attributes)
    {
        return from _1 in CheckFileExists(path)
               from res in M.LiftIO(IO.lift(() => File.SetAttributes(path, attributes)))
               select res;

    }
    public static K<M, FileAttributes> GetAttributes(string path)
    {
        return from _1 in CheckFileExists(path)
               from res in M.LiftIO(IO.lift(() => File.GetAttributes(path)))
               select res;

    }
    public static K<M, DateTime> GetCreationTime(string path)
    {
        return from _1 in CheckFileExists(path)
               from res in M.LiftIO(IO.lift(() => File.GetCreationTime(path)))
               select res;

    }

    public static K<M, Unit> SetCreationTime(string path, DateTime dt)
    {
        return from _1 in CheckFileExists(path)
               from _2 in M.LiftIO(IO.lift(() => File.SetCreationTime(path, dt)))
               select unit;

    }



    private static K<M, Unit> CheckFileExists(string path)
    {
        var (isValid, error) = IsValidPath(path);
        return from _1 in when(!isValid, M.Fail<Unit>(error!))
               from _2 in when(!File.Exists(path),
                   M.Fail<Unit>(Error.New($"Specified file '{path}' does not exist.")))
               select unit;
    }

}
