namespace cumulus {
    public class cumulusTag {
        private string _project;
        private int _startupOrder;

        public cumulusTag() {}

        public cumulusTag(string project, int startupOrder) {
            this.project = project;
            this.startupOrder = startupOrder;
        }

        public string project  {
            get {
                return _project;
            }
            set {
                if (value != "") {
                    _project = value;
                } else {
                    throw new System.ArgumentException("Project cannot be empty", "project");
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
    }

}