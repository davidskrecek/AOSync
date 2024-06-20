using System;
using System.CommandLine;
using Newtonsoft.Json;
using AOSync.Model;
using System.Net.Http;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Specialized;

namespace AOSync
{
    class Program
    {
        private static Uri ApiUrl { get; } = new Uri("https://aliteoalfaapi.azurewebsites.net/");
        private static string CompanyId { get; } = "a0619bgp8g17cr";
        public string lasttransId { get; set; } = null!;

        private static async Task<int> syncGetChanges()
        {
            try
            {
                SyncGetChanges_Request request = new ();
                Console.Write("CompanyId: ");
                request.company = Console.ReadLine()!;
                Console.Write("Last transaction ID: ");
                request.lasttranId = Console.ReadLine() ?? null!;
                Console.Write("Limit (INTEGER): ");
                request.limit = Int32.Parse(Console.ReadLine()!);
                Console.Write("Simple result (Y/N):");
                string simpleresult = Console.ReadLine()!;
                switch(simpleresult)
                {
                    case "Y":
                    case "y":
                    case "yes":
                    case "YES":
                        request.simpleResult = true;
                        break;
                    default:
                        request.simpleResult = false;
                        break;
                }

                Console.WriteLine($"SyncGetChanges: {request.ToString()}");
                SyncGetChanges_Response response = await HttpClientExtension.SendRequestAsync<SyncGetChanges_Request, SyncGetChanges_Response>(ApiUrl + "/SyncGetChanges", request);
                Console.WriteLine($"Sync GetChanges: {response.ToString()}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
                return 1;
            }

            return 0;
        }

        private async Task<string> SyncGetCurrentTranid()
        {
            SyncGetCurrentTranid_Request request = new SyncGetCurrentTranid_Request()
            {
                company = CompanyId
            };
            SyncGetCurrentTranid_Response response = await HttpClientExtension.SendRequestAsync<SyncGetCurrentTranid_Request, SyncGetCurrentTranid_Response>(ApiUrl + "/SyncGetCurrentTranid", request);
            Console.WriteLine($"SyncGetCurrentTranid response: {response}");
            return response.tranid;
        }

        static async Task Main(string[] args)
        {
            Program program = new Program();

            Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e)
            {
                Console.WriteLine("SIGINT detected. The program will close.");
                // TODO - store changes
            };

            while (true)
            {
                program.lasttransId = await program.SyncGetCurrentTranid();
                Thread.Sleep(5000); // dotaz každých 5 sekund
            }
        }
    }
}