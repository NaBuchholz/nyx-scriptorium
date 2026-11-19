using NyxScriptorium.Domain.Sessions;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: NyxScriptorium.Cli <audio-file-path>");
    return 2;
}

var audioFile = new AudioFile(Path.GetFullPath(args[0]));

Console.WriteLine($"Audio file received: {audioFile.FilePath}");
return 0;
