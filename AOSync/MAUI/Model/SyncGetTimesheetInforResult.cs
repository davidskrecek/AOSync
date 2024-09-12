namespace MAUI.Model
{
    internal class syncGetTimesheetInfoResult
    {
        public int timesheetType { get; set; }
        public string task { get; set; } = null!;
        public string task_eid { get; set; } = null!;
        public string project { get; set; } = null!;
        public string project_eid { get; set; } = null!;
        public string team { get; set; } = null!;
        public string team_eid { get; set; } = null!;
        public string group { get; set; } = null!;
        public string group_eid { get; set; } = null!;
        public string workspace { get; set; } = null!;
        public string workspace_eid { get; set; } = null!;
        public bool iserror { get; set; }
        public string error { get; set; } = null!;
        public bool isrepeatable { get; set; }
    }

}
