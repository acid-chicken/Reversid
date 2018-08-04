using System;
using System.Threading.Tasks;

namespace AcidChicken.Reversid
{
    class Program
    {
        private Program(string[] args)
        {
        }

        static async Task Main(string[] args) =>
            new Program(args).RunAsync();

        private async Task RunAsync()
        {
            await Task.Delay(-1);
        }
    }
}
