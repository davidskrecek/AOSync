using AOSync.COMMON.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AOSync.COMMON.Converters;

public class ChangesToDefClassConverter : JsonConverter<List<Changes>>
{
    private static readonly Dictionary<string, Type> TypeMap = new()
    {
        { "Attachment", typeof(SyncAttachment) },
        { "Comment", typeof(SyncComment) },
        { "CommentPredefined", typeof(SyncCommentPredefined) },
        { "Company", typeof(SyncCompany) },
        { "Field", typeof(SyncField) },
        { "FieldShared", typeof(SyncFieldShared) },
        { "FieldVal", typeof(SyncFieldVal) },
        { "FieldValShared", typeof(SyncFieldValShared) },
        { "PfLink", typeof(SyncPfLink) },
        { "ProjectLink", typeof(SyncProjectLink) },
        { "Link", typeof(SyncLink) },
        { "MemberProject", typeof(SyncMemberProject) },
        { "MemberTask", typeof(SyncMemberTask) },
        { "MemberTeam", typeof(SyncMemberTeam) },
        { "Merged", typeof(SyncMerged) },
        { "Pf", typeof(SyncPf) },
        { "PfContact", typeof(SyncPfContact) },
        { "PfMember", typeof(SyncPfMember) },
        { "PfRel", typeof(SyncPfRel) },
        { "PfType", typeof(SyncPfType) },
        { "Phase", typeof(SyncPhase) },
        { "Project", typeof(SyncProject) },
        { "Section", typeof(SyncSection) },
        { "Solver", typeof(SyncSolver) },
        { "Subtask", typeof(SyncSubtask) },
        { "MentoredUser", typeof(SyncMentoredUser) },
        { "Task", typeof(SyncTask) },
        { "Team", typeof(SyncTeam) },
        { "UserGroup", typeof(SyncUserGroup) },
        { "UserGroupItem", typeof(SyncUserGroupItem) },
        { "TimeSheet", typeof(SyncTimeSheet) },
        { "ToDo", typeof(SyncToDo) },
        { "UserCompany", typeof(SyncUserCompany) },
        { "UserGroup", typeof(SyncUserGroup) },
        { "UserGroupItem", typeof(SyncUserGroupItem) },
        { "Workspace", typeof(SyncWorkspace) }
    };

    public override bool CanWrite => true;

    public override List<Changes> ReadJson(JsonReader reader, Type objectType, List<Changes>? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonArray = JArray.Load(reader);
        var components = new List<Changes>();

        foreach (var item in jsonArray)
        {
            var jObject = (JObject)item;
            var def = jObject["def"]?.ToString();
            if (string.IsNullOrEmpty(def) || !TypeMap.TryGetValue(def, out var type))
                throw new JsonSerializationException($"Unknown component type: {def}");

            var instance = (Changes)Activator.CreateInstance(type)!;
            serializer.Populate(jObject.CreateReader(), instance);

            if (jObject["archived"] == null) instance.Archived = false;

            components.Add(instance);
        }

        return components;
    }

    public override void WriteJson(JsonWriter writer, List<Changes>? value, JsonSerializer serializer)
    {
        if (value == null) return;
        writer.WriteStartArray();
        foreach (var component in value)
        {
            var typeName = GetTypeName(component);
            if (!TypeMap.ContainsKey(typeName))
                throw new JsonSerializationException($"Unknown component type: {typeName}");

            var jObject = JObject.FromObject(component, serializer);
            jObject.AddFirst(new JProperty("def", typeName));
            jObject.WriteTo(writer);
        }

        writer.WriteEndArray();
    }

    private string GetTypeName(Changes component)
    {
        // This method ensures the correct type name is returned
        return TypeMap.FirstOrDefault(x => x.Value == component.GetType()).Key ?? component.GetType().Name;
    }
}