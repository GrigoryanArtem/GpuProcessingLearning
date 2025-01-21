namespace GPL.OpenGLApp
{
    internal class Program
    {
        static void Main()
        {
            using var app = new App(1000, 1000, "App");
            app.Run();
        }
    }
}
