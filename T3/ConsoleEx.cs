namespace BolsaValores; 

/// <summary>
/// Classe ajudante estática com funções para o console.
/// </summary>
public static class ConsoleEx {

    /// <summary>
    /// Escreve um texto ao terminal. Aceita cores e quebra de linha.
    /// </summary>
    /// <param name="msg">A mensagem a ser mostrada</param>
    /// <param name="color">A cor que o texto terá</param>
    /// <param name="appendLF">Se deve ser adiciona uma quebra de linha ao final se não houver</param>
    public static void Write(string msg, ConsoleColor? color = null, bool appendLF = true) {
        if (appendLF && !msg.EndsWith('\n')) {
            msg += '\n';
        }

        if (color is not null)
            Console.ForegroundColor = (ConsoleColor)color;
        Console.Write(msg);
        Console.ResetColor();
    }
    
    /// <summary>
    /// Avisa de algo que pode estar errado
    /// </summary>
    /// <param name="msg">A mensagem a ser mostrada</param>
    public static void Warn(string msg) {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
    
    /// <summary>
    /// Mostra um erro crítico na aplicação.
    /// </summary>
    /// <param name="msg">A mensagem a ser mostrada</param>
    public static void Error(string msg) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg);
        Console.ResetColor();
    }

    /// <summary>
    /// Indica que algo ocorreu com sucesso.
    /// </summary>
    /// <param name="msg">A mensagem a ser mostrada</param>
    public static void Success(string msg) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}