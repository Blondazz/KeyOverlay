using System;
using System.IO;

namespace KeyOverlay
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AppWindow window;
            try
            {
                window = new AppWindow();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                using var sw = new StreamWriter("errorMessage.txt");
                sw.WriteLine(e);
                throw;
            }
            window.Run();
        }
    }
}
