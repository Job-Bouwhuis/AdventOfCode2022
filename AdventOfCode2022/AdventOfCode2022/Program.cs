using TickSystem;
using TickSystem.Framework;

namespace AdventOfCode2022
{
    internal class Program
    {
        // application entry point
        static void Main(string[] args)
        {
            // setup the application
            Tickable app = new Application();
            Ticks ticks = new();

            //call the start function of the application
            app.Start(args);

            //update loop
            while (Ticks.IsRunning(app))
            {
                Tick time = ticks.NextTick();
                app.OnTick(time);
            }


            app.OnExit();
            app.Exit();
        }
    }
}