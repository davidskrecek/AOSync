using System.Diagnostics;
using AOSync.COMMON.Models;

namespace AOSync.BL.Converters;

public static class ApiProjectToEntityConverter
{
    public static SyncProject Parse(Components component)
    {
        return new SyncProject()
        {
            Id = component.Id,
            Eid = component.Eid,
            Def = ComponentsDef.Project.ToString(),
            Type = GetProjectType(component.Type),
            Simple = component.Simple,
            Parent = component.Parent,
            Creator = component.Creator,
            Created = component.Created,
            
            Name = component.AdditionalProperties["Name"].ToString(),
        };
    }

    private static SyncProjectType GetProjectType(ComponentsType? type)
    {
        if (type == null) return SyncProjectType.Create;
        return (SyncProjectType)type;
    }

}