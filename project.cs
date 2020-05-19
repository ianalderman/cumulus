using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace cumulus {
    public class project {
        private string _accessToken;
        private string _subscriptionId;
        private azureManagementHelper _azure;

        public project (string accessToken, string tenantId, string subscriptionId) {
            _accessToken = accessToken;
            _azure = new azureManagementHelper(accessToken, tenantId);
            _subscriptionId = subscriptionId;
        }

        public int listProjects(string outputFormat = "Default") {
            List<cumulusItem> projects = getProjects();
            Enum.TryParse(outputFormat, out cumulusItem.outputFormat formatter);
            System.Console.WriteLine(cumulusItem.outputHeading(formatter));
            foreach(cumulusItem project in projects) {
                Console.WriteLine(project.ToString(formatter));
            }
            return 0;
        }

        private List<cumulusItem> getProjects(string project = "*") {

            List<cumulusItem> items = new List<cumulusItem>();
            string query;

            if (project == "*") {
                query = "Resources | where type == 'microsoft.compute/virtualmachines' or type == 'microsoft.resources/subscriptions/resourcegroups' | where tags contains 'Cumulus' | project id, name, type, tags.Cumulus, resourceGroup";
            } else {
                query = "Resources | where type == 'microsoft.compute/virtualmachines' or type == 'microsoft.resources/subscriptions/resourcegroups' | where tags.Cumulus contains 'Project:" + project + "' | project id, name, type, tags.Cumulus, resourceGroup";
            }
            
            dynamic tableSet = azureGraphHelper.readAzureGraph(_accessToken, _subscriptionId, query);
            
            JArray rows = tableSet["rows"];
            foreach(var row in rows) {
                string tag = (string)row[3];
                string[] tagValues = tag.Split(",");
                cumulusTag rowTag = new cumulusTag();
                try {
                    if (tagValues.Length > 0) {                    
                        rowTag.startupOrder = 0; //assume no startup order unless specified
                        foreach(string tagItem in tagValues) {
                            string[] tagItemArray = tagItem.Split(":");
                            if (tagItemArray.Length < 1) {
                                throw new System.Exception("Invalid Tag Item, expected format key:value");
                            }
                            switch (tagItemArray[0]) {
                                case "Project":
                                    rowTag.project = tagItemArray[1];
                                    break;
                                case "StartupOrder":
                                    int number;
                                    bool isParsable = Int32.TryParse(tagItemArray[1], out number);
                                    if (isParsable) {
                                        rowTag.startupOrder = number;
                                    }
                                    else {
                                        Console.WriteLine("Could not be parsed.");
                                        
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                        items.Add(new cumulusItem(rowTag.project, (string)row[0], (string)row[1], cumulusItem.convertItemType((string)row[2]), (string)row[4], rowTag.startupOrder));
                } catch (Exception ex) {
                    System.Console.WriteLine("Unable to add item due to non conformant tag.  Error details: {0}", ex);
                }
            }
            List<cumulusItem> sortedProjects = new List<cumulusItem>();
            sortedProjects.AddRange(items.OrderBy(i => i.project).ThenBy(s => s.startupOrder));
            return sortedProjects;
        }

        public int startProject(string projectName) {
            List<cumulusItem> projectItems = getProjects(projectName);

            foreach(cumulusItem item in projectItems) {
                if (item.resourceType == cumulusItem.itemType.VM) {
                    _azure.startVirtualMachine(item.id);
                }
            }
            return 0;
        }

        public int stopProject(string projectName) {
            List<cumulusItem> projectItems = getProjects(projectName);

            foreach(cumulusItem item in projectItems) {
                if (item.resourceType == cumulusItem.itemType.VM) {
                    _azure.stopVirtualMachine(item.id);
                }
            }
            return 0;
        }
    }
}