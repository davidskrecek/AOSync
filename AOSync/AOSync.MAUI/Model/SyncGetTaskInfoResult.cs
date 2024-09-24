namespace AOSync.MAUI.Model;

internal class syncGetTaskInfoResult
{
    public string taskType { get; set; } = null!;
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

    public override string ToString()
    {
        return
            $"Task Type: {taskType}, Project: {project}, Project EID: {project_eid}, Team: {team}, Team EID: {team_eid}, Group: {group}, Group EID: {group_eid}, Workspace: {workspace}, Workspace EID: {workspace_eid}, Is Error: {iserror}, Error: {error}, Is Repeatable: {isrepeatable}";
    }
}