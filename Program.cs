using System;
using System.Threading.Tasks;
using AcidChicken.Reversid.Components;
using AcidChicken.Reversid.Models;
using AcidChicken.Reversid.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AcidChicken.Reversid
{
    class Program
    {
        private IConfiguration _configuration;

        private IServiceProvider _services;

        private Program(string[] args)
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", true, true)
                .AddUserSecrets<Secret>(true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            _services = new ServiceCollection()
                .Configure<Secret>(_ => _configuration.GetSection(nameof(Secret)))
                .BuildServiceProvider();
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
