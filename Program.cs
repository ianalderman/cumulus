using System.Threading.Tasks;

namespace cumulus
{
    class Program
    {
        static private string _accessToken;
        static private string _subscriptionId;

        static private string _tenantId;
        static private string _clientId;

        static async Task<int> Main(string[] args)
        {
            if (args.Length < 2) {
                System.Console.WriteLine("Please enter a selector and a command.");
                return 1;
            }

            string subscriptionId = "";

            
            _subscriptionId = subscriptionId;
            _tenantId = "";
            _clientId = "";

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