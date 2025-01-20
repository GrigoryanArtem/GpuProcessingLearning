namespace GPL.OpenGLApp
{
    internal class Program
    {
        static void Main()
        {
            using var app = new App(900, 900, "App");
            app.Run();
        }
    }
}
