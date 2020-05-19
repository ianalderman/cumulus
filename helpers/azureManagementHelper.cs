using Microsoft.Rest;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using System;

namespace cumulus {
    public class azureManagementHelper {
        private IAzure _azure;
        public azureManagementHelper(string accessToken, string tenantId) {
            ServiceClientCredentials serviceClientCreds = new TokenCredentials(accessToken);

            try {
                var azure = Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(new AzureCredentials(
                    serviceClientCreds,
                    serviceClientCreds,
                    tenantId,
                    AzureEnvironment.AzureGlobalCloud))
                .WithDefaultSubscription();
            
                _azure = azure;
            } catch (Exception e) {
                throw new Exception(e.Message);
            }   
        }

        public int stopVirtualMachine(string vmId) {
            //https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.management.compute.fluent.ivirtualmachine.deallocateasync?view=azure-dotnet
            //vm.deallocateasync();
            var vm = _azure.VirtualMachines.GetById(vmId);
            vm.DeallocateAsync();
            Console.WriteLine ("Requested stop of VM: {0}", vmId);
            return 0;
        }

        public int startVirtualMachine(string vmId) {
            var vm = _azure.VirtualMachines.GetById(vmId);
            vm.StartAsync();
            Console.WriteLine ("Requested start of VM: {0}", vmId);
            return 0;
        }
    }
}