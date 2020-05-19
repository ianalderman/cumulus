namespace cumulus {
    public class cumulusItem {
        public enum itemType {
            VM,
            WebApp,
            AzureSQLServer
        }

        public enum  outputFormat {
            Detailed,
            Default
        }

        #region "Globals"
        private const outputFormat defaultOutputFormatting = outputFormat.Default;
        private string _id;
        private string _name;
        private itemType _resourceType;

        private string _resourceGroup;

        private int _startupOrder;

        private string _project;

        private static string _detailedFormatting="{0,-15} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15}"; //all cols
        private static string _defaultFormatting="{0,-15} {1,-15} {2,-15} {3,-15}"; //project name type startup
        #endregion

        #region "Constructors"
        public cumulusItem() {

        }
        public cumulusItem(string project, string id, string name, itemType resourceType, string resourceGroup, int startupOrder) {
            this.project = project;
            this.id = id;
            this.name = name;
            this.resourceType = resourceType;
            this.resourceGroup = resourceGroup;
            this.startupOrder = startupOrder;
        }
        #endregion

        #region "Methods"
        public static itemType convertItemType(string value ) {
            switch (value) {
                case "microsoft.web/serverfarms":
                    return itemType.WebApp;
                case "microsoft.compute/virtualmachines":
                    return itemType.VM;
                default:
                    throw new System.ArgumentException("Invalid type supplied.  Supported types: microsoft.web/serverfarms");
            }
        }

        public static string outputHeading(outputFormat formatter = defaultOutputFormatting) {
            switch (formatter){
                case outputFormat.Detailed:
                    return string.Format(_detailedFormatting, new string[] {"project", "id", "name", "resourceType", "resourceGroup", "startupOrder"});
                default:
                    return string.Format(_defaultFormatting, new string[] { "project", "name", "resourceType", "startupOrder" });
            }
        }

        public override string ToString() {
            return string.Format(_detailedFormatting, new string[] { project, id, name, resourceType.ToString(), resourceGroup, startupOrder.ToString("N") });
        }

        public string ToString(outputFormat formatter) {
            switch (formatter) {
                case outputFormat.Detailed:
                    return string.Format(_detailedFormatting, new string[] { project, id, name, resourceType.ToString(), resourceGroup, startupOrder.ToString("N") });
                    
                default:
                    return string.Format(_defaultFormatting, new string[] { project, name, resourceType.ToString(), startupOrder.ToString("N") });
            }
            
        }
        #endregion

        #region "Properties"
        public string project {
            get {
                return _project;
            }
            set {
                if (value != null || value != "") {
                    _project = value;
                } else {
                    throw new System.ArgumentException("project name should not be blank", "project");
                }
            }
        }
        public string id {
            get {
                return _id;
            }
            set {
                if (value != null || value != "") {
                    if (value.Substring(0,1) == "/") {
                        _id = value;
                    } else {
                        throw new System.ArgumentException("Invalid id supplied these should be in the format /subscriptions/...", "id");
                    }
                } else {
                   throw new System.ArgumentNullException("id");
                }
            }
        }

        public string name {
            get {
                return _name;
            }
            set {
                if (value != null || value != "") {
                    _name = value;
                } else {
                    throw new System.ArgumentNullException("name");
                }
            }
        }

        public itemType resourceType {
            get {
                return _resourceType;
            }
            set {
                _resourceType = value;
            }
        }

        public string resourceGroup {
            get {
                return _resourceGroup;
            }
            set {
                 if (value != null || value != "") {
                    _resourceGroup = value;
                } else {
                    throw new System.ArgumentNullException("resourceGroup");
                }
            }
        }

        public int startupOrder {
            get {
                return _startupOrder;
            }
            set {
                    if (value >= 0 && value < 101) {
                        _startupOrder = value;
                    } else {
                        throw new System.ArgumentOutOfRangeException("startupOrder","startupOrder should be between 0 and 100");
                    }
            }
        }


        #endregion

    }
}