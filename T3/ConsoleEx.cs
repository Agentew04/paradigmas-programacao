namespace BolsaValores; 

public static class ConsoleEx {

    public static void Write(string msg, ConsoleColor? color = null, bool appendLF = true) {
        if (appendLF && !msg.EndsWith('\n')) {
            msg += '\n';
        }

        if (color is not null)
            Console.ForegroundColor = (ConsoleColor)color;
        Console.Write(msg);
        Console.ResetColor();
    }
    
    public static void Warn(string msg) {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
    
    public static void Error(string msg) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}