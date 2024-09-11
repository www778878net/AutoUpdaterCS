using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // if (args.Length < 3)
        // {
        //     Console.WriteLine("Usage: dotnet run initialize_directory <local_directory> <remote_base_url>");
        //     return;
        // }

        string command ="update_files";// args[0];
        string localDirectory ="C:\\trade24\\update";// args[1];
        string remoteBaseUrl ="http://steamoss78.778878.net/download/trade248";// args[2];

        var updater = new AutoUpdater(localDirectory, remoteBaseUrl);

        if (command == "initialize_directory")
        {
            string result = updater.InitializeLocalDirectory();
            Console.WriteLine(result);
        }
        else if (command == "update_files")
        {
            string result = await updater.UpdateFilesAsync();
            Console.WriteLine(result);
        }
        else
        {
            Console.WriteLine($"Unknown command: {command}");
        }
    }
}