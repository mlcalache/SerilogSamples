using Serilog;
using System;
using System.Configuration;

namespace SerilogConsoleSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Example_Log_To_Console();
            //Example_Log_To_File();
            //Example_Log_With_Enricher_AssemblyName(); //NOT WORKING AS I EXPECTED
            Example_Log_To_SQLServer();
        }

        private static void Example_Log_To_SQLServer()
        {
            var connectionString = @"Data Source=localhost;Initial Catalog=Serilog;Persist Security Info=True;User ID=serilog;Password=serilog";
            //var connectionString = @"Data Source=localhost;Initial Catalog=Serilog;Integrated Security=True";
            var tableName = "Logs";

            Log.Logger = new LoggerConfiguration()
                            .WriteTo.MSSqlServer(connectionString, tableName, autoCreateSqlTable: true)
                            .CreateLogger();

            Log.Information("Hello, world!");

            var a = 10;
            var b = 0;

            try
            {
                Log.Debug("Dividing {A} by {B}", a, b);
                Console.WriteLine(a / b);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong");
            }

            Log.CloseAndFlush();
        }

        private static void Example_Log_With_Enricher_AssemblyName()
        {
            Log.Logger = new LoggerConfiguration()
                            .Enrich.WithProperty("Application", "[My application name]")
                            .Enrich.WithProperty("Environment", ConfigurationManager.AppSettings["Environment"])
                            .Enrich.WithAssemblyName()
                            .Enrich.WithAssemblyVersion()
                            .MinimumLevel.Debug()
                            .WriteTo.Console()
                            .WriteTo.File("logs\\myapp.txt", rollingInterval: RollingInterval.Day)
                            .CreateLogger();

            Log.Information("Hello, world!");

            var a = 10;
            var b = 0;

            try
            {
                Log.Debug("Dividing {A} by {B}", a, b);
                Console.WriteLine(a / b);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong");
            }

            Log.CloseAndFlush();
            //Console.ReadKey();
        }

        private static void Example_Log_To_File()
        {
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.Console()
                            .WriteTo.File("logs\\myapp.txt", rollingInterval: RollingInterval.Day)
                            .CreateLogger();

            Log.Information("Hello, world!");

            var a = 10;
            var b = 0;

            try
            {
                Log.Debug("Dividing {A} by {B}", a, b);
                Console.WriteLine(a / b);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong");
            }

            Log.CloseAndFlush();
            //Console.ReadKey();
        }

        private static void Example_Log_To_Console()
        {
            var log = new LoggerConfiguration()
                            .WriteTo.Console()
                            .CreateLogger();

            log.Information("Hello, Serilog!");

            Log.Logger = log;

            Log.Information("The global logger has been configured");

            Console.WriteLine("Hello World!");
        }
    }
}