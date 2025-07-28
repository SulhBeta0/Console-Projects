using System.Runtime.Versioning;
using System.Media;
using System.Reflection;
namespace Clicks.Tools;

[SupportedOSPlatform("windows")]
public static class Accesories
{
    public static readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "clicker_datos.bin");

    public static void MessageCreator(string text, ConsoleColor color, int heightPos, int? widthPos = null)
    {
        Console.ForegroundColor = color;
        Console.SetCursorPosition(widthPos ?? CalculatingWidthForMenus(text.Length), heightPos);
        Console.Write(text);
        Console.ResetColor();
    }

    public static void ChangingWindowsValues(int? changingBufferHeight = null, int? changingBufferWidth = null)
    {
        if (changingBufferWidth.HasValue && changingBufferWidth > Console.BufferWidth)
            Console.BufferWidth = changingBufferWidth.Value;

        if (changingBufferHeight.HasValue && changingBufferHeight > Console.BufferHeight)
            Console.BufferHeight = changingBufferHeight.Value;
    }

    public static Exception WarningsCreator(string text, int heightPos, int? widthPos = null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(widthPos ?? CalculatingWidthForMenus(text.Length), heightPos);
        return new Exception(text);
    }

    public static void PathForMusic(string assemblyPath, int time = 0)
    {
        try{
            var music = new SoundPlayer(EmbeddedAssemblyAssist.GettingResource(assemblyPath));

            music.Load();
            music.Play();
            Thread.Sleep(time);

            music.Dispose();
        }
        catch (Exception)
        {
            Console.Write(WarningsCreator("Music not loaded!", 25, 43));
            Thread.Sleep(800);
            return;
        }
    }

    public static void ClearKeyBuffer()
    {
        while (Console.KeyAvailable)
        {
            Console.ReadKey(true);
        }
    }

    public static void SetUpConsole(bool activated, string title = "Default")
    {
        Console.Title = title;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = activated;
    }

    private static int CalculatingWidthForMenus(int lenghtText) => (Console.BufferWidth - lenghtText) / 2;
}

[SupportedOSPlatform("windows")]
public static class EmbeddedAssemblyAssist
{
    private static readonly Assembly _assembly = typeof(EmbeddedAssemblyAssist).Assembly;
    
    public static Stream? GettingResource(string name)
    {
        try {
            Stream? stream = _assembly.GetManifestResourceStream(name);
            if (stream == null) return null;

            var keepingInMemoryStream = new MemoryStream();
            stream.CopyTo(keepingInMemoryStream);
            keepingInMemoryStream.Position = 0;

            return keepingInMemoryStream;
        }
        catch (Exception e)
        {
            Console.WriteLine(Accesories.WarningsCreator($"ERROR: {e.Message}",27));
            return null;
        }
    }
}

public class Sufixs
{
    private static readonly string[] _sufixs = ["", "K", "M", "B", "T", "Qa", "Qi", "Sx"];

    public static string GettingNewLook(ulong number)
    {
        if (number < 1000) return number.ToString();

        decimal myValue = number;
        byte timesDivided = 0;

        while (myValue >= 1000 && timesDivided < _sufixs.Length - 1)
        {
            myValue /= 1000;
            timesDivided++;
        }
        
        return $"{myValue:0.##}{_sufixs[timesDivided]}";
    }
}