﻿using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System;

namespace cumulus
{
    class Program
    {
        private static IConfigurationRoot _configuration;
        private static string _accessToken;
        private static string _subscriptionId;

        private static string _tenantId;
        private static string _clientId;

        static async Task<int> Main(string[] args)
        {
            if (args.Length < 2) {
                System.Console.WriteLine("Please enter a selector and a command.");
                return 1;
            }

            if (File.Exists("appsettings.dev.json")) {
                _configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                    .AddJsonFile("appsettings.dev.json", false)
                    .Build();
            } else {
                _configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                    .AddJsonFile("appsettings.json", false)
                    .Build();
            }
            
            try {
                _subscriptionId = _configuration.GetValue<string>("AzureConfiguration:SubscriptionId");
                _tenantId =_configuration.GetValue<string>("AzureConfiguration:TenantId");
                _clientId = _configuration.GetValue<string>("AzureConfiguration:ClientId");
            } catch (Exception e) {
                throw new InvalidDataException("Error loading configuration: {0}", e);
            }



            string accessToken = await authenticationHelper.GetAzureToken(_tenantId, _clientId);
            _accessToken = accessToken;

            switch (args[0]) {
                case "project":
                    processProject(args, _accessToken,_tenantId, _subscriptionId);
                    break;
                default:
                    System.Console.WriteLine("Available Commands: project");
                    return 1;
            }         
            return 0;
        }

        private static int processProject(string[] args, string accessToken, string tenantId, string subscriptionId) {
            //float project list, float project start tiger, float project stop tiger
            var project = new project(accessToken, tenantId, subscriptionId);

            if (args.Length < 3 && args[1] != "list") {
                System.Console.WriteLine("Available Commands: list, start <project name>, stop <project name>");
                return -1;
            }

            switch (args[1]) {
                case "list":
                    project.listProjects();
                    break;
                case "start":
                    project.startProject(args[2]);
                    break;
                case "stop":
                    project.stopProject(args[2]);
                    break;
                default:
                    System.Console.WriteLine("Available Commands: list, start <project name>, stop <project name>");
                    return -1;
            }
            return 0;
        }
    }
}