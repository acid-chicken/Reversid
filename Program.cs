using System;
using System.Threading.Tasks;
using AcidChicken.Reversid.Models;
using AcidChicken.Reversid.Views;

namespace AcidChicken.Reversid
{
    class Program
    {
        private Program(string[] args)
        {
        }

        static Task Main(string[] args) =>
            new Program(args).RunAsync();

        private async Task RunAsync()
        {
            Console.Clear();
            ConsoleView.Show(ReversiBoard.GetInitial());
            await Task.CompletedTask;
        }
    }
}
