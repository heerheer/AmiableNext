namespace AmiableNext.Utils;

public class ConsoleLogUtil<T>
{
    public ConsoleLogUtil()
    {
    }

    public void Info(string content) => Log(content, 0);

    public void Debug(string content)
        => Log(content, 1);

    public void Warn(string content)
        => Log(content, 2);

    public void Error(string content)
        => Log(content, 3);


    public void Log(string content, int level)
    {
        Console.ForegroundColor = level switch
        {
            1 => ConsoleColor.White,
            2 => ConsoleColor.Yellow,
            3 => ConsoleColor.Red,
            _ => ConsoleColor.DarkGray
        };
        Console.WriteLine(content);
        Console.ResetColor();
    }
}