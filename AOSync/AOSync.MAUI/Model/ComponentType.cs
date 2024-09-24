using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AOSync.MAUI.Model;

public class ComponentBase
{
    public string type { get; set; } = "Create";
    public bool simple { get; set; } = false;
    public string def { get; set; } = "Attachment";
    public string? id { get; set; }
    public string? eid { get; set; }
    public syncParent? parent { get; set; }
    public bool? archived { get; set; }
    public string creator { get; set; } = null!;
    public string? created { get; set; }

    public override string ToString()
    {
        return $"{{Type: {type}, Simple: {simple}, Definition: {def}, ID: {id}, EID: {eid}, " +
               $"Parent: {parent}, Archived: {archived}, Creator: {creator}, Created: {created}}}";
    }
}

public class syncChangeSimple : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public List<ApiValue> fields { get; set; } = new();

    public override string ToString()
    {
        return $"{base.ToString()}, Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Fields: [{(fields != null ? string.Join(", ", fields) : "null")}]";
    }
}

public class syncAttachment : ComponentBase
{
    public readonly int? Uploaded = null;
    public string InternalType { get; set; } = "Public";
    public int? Renamed { get; set; } = null;
    public string? FileName { get; set; } = null;
    public string? ContentType { get; set; } = null;
    public string Content { get; set; } = null!;

    public override string ToString()
    {
        return $"{base.ToString()}, InternalType: {InternalType}, Renamed: {Renamed}, Uploaded: {Uploaded}, " +
               $"FileName: {FileName ?? "null"}, ContentType: {ContentType ?? "null"}, Content: {Content}";
    }
}

public class syncComment : ComponentBase
{
    public readonly int? OrderId = null;
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? Comment { get; set; } = null;
    public string? EmailOwner { get; set; } = null;
    public string? InternalType { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Comment: {Comment ?? "null"}, EmailOwner: {EmailOwner ?? "null"}, " +
               $"InternalType: {InternalType ?? "null"}, OrderId: {OrderId}";
    }
}

public class syncCommentPredefined : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? Name { get; set; } = null;
    public string? Text { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Name: {Name ?? "null"}, Text: {Text ?? "null"}";
    }
}

public class syncCompany : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();

    public override string ToString()
    {
        return $"{base.ToString()}, Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}]";
    }
}

public class syncField : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? ApiCode { get; set; } = null;
    public string? EnumView { get; set; } = null;
    public string? Label { get; set; } = null;

    [StringLength(14, MinimumLength = 14)] public string? SharedField { get; set; } = null;

    public string? SharedType { get; set; } = null;
    public string? Tag { get; set; } = null;
    public string? XType { get; set; } = null;

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(base.ToString());

        sb.Append($"{base.ToString()}, Api Code: {ApiCode}, Enum View: {EnumView}, Label: {Label}, " +
                  $"Shared Field: {SharedField}, Shared Type: {SharedType}, Tag: {Tag}, X Type: {XType}");

        return sb.ToString();
    }
}

public class syncFieldShared : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? ApiCode { get; set; } = null;
    public string? Label { get; set; } = null;
    public string? Tag { get; set; } = null;
    public string? XType { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"ApiCode: {ApiCode ?? "null"}, Label: {Label ?? "null"}, " +
               $"Tag: {Tag ?? "null"}, " +
               $"XType: {XType ?? "null"}";
    }
}

public class syncFieldVal : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? ApiCode { get; set; } = null;
    public string? Label { get; set; } = null;

    [StringLength(14, MinimumLength = 14)] public string? SharedFieldVal { get; set; } = null;

    public string? Tag { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"ApiCode: {ApiCode ?? "null"}, Label: {Label ?? "null"}, " +
               $"SharedFieldVal: {SharedFieldVal ?? "null"}, Tag: {Tag ?? "null"}";
    }
}

public class syncFieldValShared : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? ApiCode { get; set; } = null;
    public string? Label { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"ApiCode: {ApiCode ?? "null"}, Label: {Label ?? "null"}";
    }
}

public class syncPfLink : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? Http { get; set; } = null;
    public bool? InApplication { get; set; } = null;
    public string? Name { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Http: {Http ?? "null"}, InApplication: {InApplication?.ToString() ?? "null"}, Name: {Name ?? "null"}";
    }
}

public class syncProjectLink : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public bool? ExternalMember { get; set; } = null;
    public string? Http { get; set; } = null;
    public bool? InApplication { get; set; } = null;
    public bool? InternalMember { get; set; } = null;
    public string? Name { get; set; } = null;
    public bool? ViewInTask { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"ExternalMember: {ExternalMember?.ToString() ?? "null"}, " +
               $"Http: {Http ?? "null"}, " +
               $"InApplication: {InApplication?.ToString() ?? "null"}, " +
               $"InternalMember: {InternalMember?.ToString() ?? "null"}, " +
               $"Name: {Name ?? "null"}, " +
               $"ViewInTask: {ViewInTask?.ToString() ?? "null"}";
    }
}

public class syncLink : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();

    [StringLength(14, MinimumLength = 14)] public string? Link { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Link: {Link ?? "null"}";
    }
}

public class syncMemberProject : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public bool? Admin { get; set; } = null;
    public bool? CanAddKoop { get; set; } = null;
    public string? MemberType { get; set; } = "Internal";
    public string? Supervisor { get; set; } = "No";
    public string? SupervisorM { get; set; } = "No";
    public bool? Tools { get; set; } = false;

    [StringLength(14, MinimumLength = 14)] public string? UserCompany { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Admin: {Admin?.ToString() ?? "null"}, " +
               $"CanAddKoop: {CanAddKoop?.ToString() ?? "null"}, " +
               $"MemberType: {MemberType ?? "null"}, " +
               $"Supervisor: {Supervisor ?? "null"}, " +
               $"SupervisorM: {SupervisorM ?? "null"}, " +
               $"Tools: {Tools?.ToString() ?? "null"}, " +
               $"UserCompany: {UserCompany ?? "null"}";
    }
}

public class syncMemberTask : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();

    [StringLength(14, MinimumLength = 14)] public string? Link { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Link: {Link ?? "null"}";
    }
}

public class syncMemberTeam : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public bool? Admin { get; set; } = null;
    public bool? CanAddKoop { get; set; } = null;
    public string? MemberType { get; set; } = "Internal";
    public string? SupervisorM { get; set; } = "No";
    public bool? Tools { get; set; } = false;

    [StringLength(14, MinimumLength = 14)] public string? UserCompany { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Admin: {Admin}, " +
               $"CanAddKoop: {CanAddKoop}, " +
               $"MemberType: {MemberType}, " +
               $"SupervisorM: {SupervisorM}, " +
               $"Tools: {Tools}, " +
               $"UserCompany: {UserCompany ?? "null"}";
    }
}

public class syncMerged : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();

    [StringLength(14, MinimumLength = 14)] public string? MergedTask { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"MergedTask: {MergedTask ?? "null"}";
    }
}

public class syncPf : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? Active { get; set; } = "Active";
    public string? Addr1 { get; set; } = null;
    public string? Addr2 { get; set; } = null;
    public string? City { get; set; } = null;
    public string? Country { get; set; } = null;
    public string? Description { get; set; } = null;
    public string? Email { get; set; } = null;
    public string? End { get; set; } = null;
    public string? FaId { get; set; } = null;
    public string? FaName { get; set; } = null;
    public string? FaVat { get; set; } = null;
    public string? Name { get; set; } = null;

    [StringLength(14, MinimumLength = 14)] public string? Owner { get; set; } = null;

    [StringLength(14, MinimumLength = 14)] public string? PfType { get; set; } = null;

    public string? Phone { get; set; } = null;
    public string? Start { get; set; } = null;
    public string? Visibility { get; set; } = "PfMembers";
    public string? Www { get; set; } = null;
    public string? Zip { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Active: {Active}, Addr1: {Addr1}, Addr2: {Addr2}, City: {City}, Country: {Country}, " +
               $"Description: {Description}, Email: {Email}, End: {End}, FaId: {FaId}, FaName: {FaName}, " +
               $"FaVat: {FaVat}, Name: {Name}, Owner: {Owner}, PfType: {PfType}, Phone: {Phone}, " +
               $"Start: {Start}, Visibility: {Visibility}, Www: {Www}, Zip: {Zip}";
    }
}

public class syncPfContact : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? Email { get; set; } = null;
    public string? Name { get; set; } = null;
    public string? Note { get; set; } = null;
    public string? Phone { get; set; } = null;
    public string? Phone2 { get; set; } = null;
    public string? Position { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Email: {Email}, Name: {Name}, Note: {Note}, Phone: {Phone}, Phone2: {Phone2}, Position: {Position}";
    }
}

public class syncPfMember : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public bool? Admin { get; set; } = null;

    [StringLength(14, MinimumLength = 14)] public string? UserCompany { get; set; } = null;

    public override string ToString()
    {
        var attrsString = attrs != null ? string.Join(", ", attrs) : "null";
        return $"{base.ToString()}, Attributes: [{attrsString}], User Company: {UserCompany}";
    }
}

public class syncPfRel : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();

    [StringLength(14, MinimumLength = 14)] public string? Project { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Project: {Project}";
    }
}

public class syncPfType : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? Name { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Name: {Name}";
    }
}

public class syncPhase : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? Name { get; set; } = null;
    public string? Name2 { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Name: {Name}, " +
               $"Name2: {Name2}";
    }
}

public class syncProject : ComponentBase
{
    public readonly int? NumberId = null;
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();

    [StringLength(14, MinimumLength = 14)] public string? Anonymous { get; set; } = null;

    public bool? CaAddTasks { get; set; } = true;
    public bool? CommentViaEmail { get; set; } = true;
    public string? Description { get; set; } = null;
    public string? Done { get; set; } = "Plan";
    public string? DueDate { get; set; } = null;
    public bool? FreeRegistration { get; set; } = true;
    public string? FreeRegistrationGuid { get; set; } = null;
    public bool? MembersCanAddKoop { get; set; } = null;
    public bool? MembersTools { get; set; } = null;
    public string? Name { get; set; } = null;

    [StringLength(14, MinimumLength = 14)] public string? Owner { get; set; } = null;

    public bool? PfContactMust { get; set; } = null;

    [StringLength(14, MinimumLength = 14)] public string? PfTypeContact { get; set; } = null;

    public bool? RiskQuality { get; set; } = null;
    public bool? RiskTerm { get; set; } = null;
    public string? StartDate { get; set; } = null;
    public bool? TaskViaEmail { get; set; } = null;
    public bool? UseSections { get; set; } = null;
    public string? ViaEmailRestrictions { get; set; } = null;
    public string? View { get; set; } = null;
    public string? Visibility { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Anonymous: {Anonymous}, " +
               $"CaAddTasks: {CaAddTasks}, " +
               $"CommentViaEmail: {CommentViaEmail}, " +
               $"Description: {Description}, " +
               $"Done: {Done}, " +
               $"DueDate: {DueDate}, " +
               $"FreeRegistration: {FreeRegistration}, " +
               $"FreeRegistrationFuid: {FreeRegistrationGuid}, " +
               $"MembersCanAddKoop: {MembersCanAddKoop}, " +
               $"MembersTools: {MembersTools}, " +
               $"Name: {Name}, " +
               $"NumberId: {NumberId}, " +
               $"Owner: {Owner}, " +
               $"PfContactMust: {PfContactMust}, " +
               $"PfTypeContact: {PfTypeContact}, " +
               $"RiskQuality: {RiskQuality}, " +
               $"RiskTerm: {RiskTerm}, " +
               $"StartDate: {StartDate}, " +
               $"TaskViaEmail: {TaskViaEmail}, " +
               $"UseSections: {UseSections}, " +
               $"ViaEmailRestrictions: {ViaEmailRestrictions}, " +
               $"View: {View}, " +
               $"Visibility: {Visibility}";
    }
}

public class syncSection : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();

    [StringLength(14, MinimumLength = 14)] public string? UserCompany { get; set; } = null;

    public bool CanAddTask { get; set; } = false;
    public bool CanAddTaskExternal { get; set; } = false;
    public bool CanPlaceTask { get; set; } = false;
    public bool? IsDefaultForAddingTask { get; set; } = null;

    public string? Background { get; set; } = null;
    public string? Name { get; set; } = null;
    public string? StartDate { get; set; } = null;
    public string? DueDate { get; set; } = null;
    public string? DoneDate { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"UserCompany: {UserCompany}, " +
               $"CanAddTask: {CanAddTask}, " +
               $"CanAddTaskExternal: {CanAddTaskExternal}, " +
               $"CanPlaceTask: {CanPlaceTask}, " +
               $"IsDefaultForAddingTask: {IsDefaultForAddingTask}, " +
               $"Background: {Background}, " +
               $"Name: {Name}, " +
               $"StartDate: {StartDate}, " +
               $"DueDate: {DueDate}, " +
               $"DoneDate: {DoneDate}";
    }
}

public class syncSolver : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();

    [StringLength(14, MinimumLength = 14)] public string? UserCompany { get; set; } = null;

    public override string ToString()
    {
        var attrsString = attrs != null ? string.Join(", ", attrs) : "null";
        return $"{base.ToString()}, Attributes: [{attrsString}], User Company: {UserCompany}";
    }
}

public class syncSubtask : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();

    [StringLength(14, MinimumLength = 14)] public string? Subtask { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Subtask: {Subtask}";
    }
}

public class syncMentoredUser : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();

    [StringLength(14, MinimumLength = 14)] public string? Subtask { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, " +
               $"Attributes: [{(attrs != null ? string.Join(", ", attrs) : "null")}], " +
               $"Subtask: {Subtask}";
    }
}

public class syncTask : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? CancelDate { get; set; } = null;
    public string? CancelRequestDate { get; set; } = null;
    public bool? CancelRequestTrigger { get; set; } = true;
    public string? CancelRequestUser { get; set; } = null;
    public bool? CancelTrigger { get; set; } = true;
    public string? CancelUser { get; set; } = null;
    public string? CloseDate { get; set; } = null;
    public string? CloseRequestDate { get; set; } = null;
    public bool? CloseRequestTrigger { get; set; } = true;
    public string? CloseRequestUser { get; set; } = null;
    public bool? CloseTrigger { get; set; } = true;
    public string? CloseUser { get; set; } = null;
    public string? CopiedFrom { get; set; } = null;
    public string? Description { get; set; } = null;
    public string? Done { get; set; } = "Solving";
    public string? DueDate { get; set; } = null;
    public string? EmailOwner { get; set; } = null;
    public string? InternalType { get; set; } = "Public";
    public int? MinutesPlan { get; set; } = null;
    public int? MinutesTaken { get; set; } = null;
    public string? MovedFrom { get; set; } = null;
    public string? MovedTo { get; set; } = null;
    public string? Name { get; set; } = null;
    public string? NumberId { get; set; } = null;
    public string? Owner { get; set; } = null;
    public string? ParentSubTask { get; set; } = null;
    public string? Pf { get; set; } = null;
    public string? Phase { get; set; } = null;
    public string? Priority { get; set; } = "Normal";
    public string? ProDueDate { get; set; } = null;
    public string? ProDueDateUser { get; set; } = null;
    public string? ReactionComponent { get; set; } = null;
    public string? RepairFinished { get; set; } = null;
    public string? RepairStatus { get; set; } = "NoSla";
    public string? RepairTime { get; set; } = null;
    public string? ResponseFinished { get; set; } = null;
    public string? ResponseStatus { get; set; } = "NoSla";
    public string? ResponseTime { get; set; } = null;
    public string? StartDate { get; set; } = null;
    public int? StoryPoints { get; set; } = null;
    public int? StoryPointsTran { get; set; } = null;
    public string? SurveyQuality { get; set; } = "Unknown";
    public string? SurveyTerm { get; set; } = "Unknown";
    public string? SurveyText { get; set; } = null;
    public string? SurveyUser { get; set; } = null;
    public string? SuspendedDate { get; set; } = null;
    public string? TemplatedFrom { get; set; } = null;

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(base.ToString());

        sb.Append(
            $", CancelDate: {CancelDate}, CancelRequestDate: {CancelRequestDate}, CancelRequestTrigger: {CancelRequestTrigger}, " +
            $"CancelRequestUser: {CancelRequestUser}, CancelTrigger: {CancelTrigger}, CancelUser: {CancelUser}, " +
            $"CloseDate: {CloseDate}, CloseRequestDate: {CloseRequestDate}, CloseRequestTrigger: {CloseRequestTrigger}, " +
            $"CloseRequestUser: {CloseRequestUser}, CloseTrigger: {CloseTrigger}, CloseUser: {CloseUser}, " +
            $"CopiedFrom: {CopiedFrom}, Description: {Description}, Done: {Done}, DueDate: {DueDate}, " +
            $"EmailOwner: {EmailOwner}, InternalType: {InternalType}, MinutesPlan: {MinutesPlan}, MinutesTaken: {MinutesTaken}, " +
            $"MovedFrom: {MovedFrom}, MovedTo: {MovedTo}, Name: {Name}, NumberId: {NumberId}, Owner: {Owner}, " +
            $"ParentSubTask: {ParentSubTask}, Pf: {Pf}, Phase: {Phase}, Priority: {Priority}, ProDueDate: {ProDueDate}, " +
            $"ProDueDateUser: {ProDueDateUser}, ReactionComponent: {ReactionComponent}, RepairFinished: {RepairFinished}, " +
            $"RepairStatus: {RepairStatus}, RepairTime: {RepairTime}, ResponseFinished: {ResponseFinished}, " +
            $"ResponseStatus: {ResponseStatus}, ResponseTime: {ResponseTime}, StartDate: {StartDate}, " +
            $"StoryPoints: {StoryPoints}, StoryPointsTran: {StoryPointsTran}, SurveyQuality: {SurveyQuality}, " +
            $"SurveyTerm: {SurveyTerm}, SurveyText: {SurveyText}, SurveyUser: {SurveyUser}, SuspendedDate: {SuspendedDate}, " +
            $"TemplatedFrom: {TemplatedFrom}");

        return sb.ToString();
    }
}

public class syncTeam : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? Description { get; set; }
    public bool? MembersCanAddKoop { get; set; }
    public bool? MembersTools { get; set; }
    public string? Name { get; set; }
    public int? NumberId { get; set; }

    [StringLength(14, MinimumLength = 14)] public string? Owner { get; set; } = null;

    public string? View { get; set; } = null;
    public string? Visibility { get; set; } = null;

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(base.ToString());

        sb.Append(
            $", Description: {Description}, MembersCanAddKoop: {MembersCanAddKoop}, MembersTools: {MembersTools}, " +
            $"Name: {Name}, NumberId: {NumberId}, Owner: {Owner}, View: {View}, Visibility: {Visibility}");

        return sb.ToString();
    }
}

public class syncWorkingGroup : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? Name { get; set; } = null;

    [StringLength(14, MinimumLength = 14)] public string? Owner { get; set; } = null;

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(base.ToString());

        sb.Append($", Name: {Name}, Owner: {Owner}");

        return sb.ToString();
    }
}

public class syncWorkingGroupItem : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();

    [StringLength(14, MinimumLength = 14)] public string? UserCompany { get; set; } = null;

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(base.ToString());

        sb.Append($", UserCompany: {UserCompany}");

        return sb.ToString();
    }
}

public class syncTimeSheet : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? Date { get; set; }
    public string? Description { get; set; }
    public string? From { get; set; }
    public int? Minutes { get; set; }
    public string? Name { get; set; }
    public int? NumberId { get; set; }
    public string? SourceComponent { get; set; }
    public string? SourceType { get; set; }
    public string? To { get; set; }
    public string? Verificated { get; set; }
    public string? Verificator { get; set; }
    public string? Verified { get; set; } = "NotVerified";

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(base.ToString());

        sb.Append($", Date: {Date}, Description: {Description}, From: {From}, Minutes: {Minutes}, " +
                  $"Name: {Name}, NumberId: {NumberId}, SourceComponent: {SourceComponent}, " +
                  $"SourceType: {SourceType}, To: {To}, Verificated: {Verificated}, " +
                  $"Verificator: {Verificator}, Verified: {Verified}");

        return sb.ToString();
    }
}

public class syncToDo : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public bool? Checked { get; set; }
    public string? InternalType { get; set; } = "Public";
    public string? Name { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(base.ToString());

        sb.Append($", Checked: {Checked}, InternalType: {InternalType}, Name: {Name}");

        return sb.ToString();
    }
}

public class syncUserCompany : ComponentBase
{
    public readonly string? Accepted = "Invitation";
    public readonly bool? IsMentor;
    public readonly string? MyStatus = "ByUser";
    public readonly string? MyStatusText;
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? Active { get; set; } = "Inactive";
    public bool? AddPfs { get; set; }
    public bool? AddProjects { get; set; }
    public bool? AddTeams { get; set; }
    public bool? Admin { get; set; }
    public string? Background { get; set; }
    public string? Company { get; set; }
    public bool? CompanyHide { get; set; }
    public string? Email { get; set; }
    public bool? EmailHide { get; set; }
    public string? EmailToInvite { get; set; }
    public bool? FreeRegistration { get; set; }
    public string? InternalInfo { get; set; }
    public int? InvitationsSent { get; set; }
    public bool? LicencedInternal { get; set; }
    public bool? MakeTimeSheet { get; set; }
    public string? MemberType { get; set; } = "Internal";
    public string? Mobile { get; set; }
    public bool? MobileHide { get; set; }
    public string? Name { get; set; }
    public string? Position { get; set; }
    public bool? PositionHide { get; set; }
    public string? Shortcut { get; set; }
    public string? Supervisor { get; set; } = "No";
    public string? SupervisorM { get; set; } = "No";
    public bool? ViewPriority { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(base.ToString());

        sb.Append(
            $", Accepted: {Accepted}, Active: {Active}, AddPfs: {AddPfs}, AddProjects: {AddProjects}, AddTeams: {AddTeams}, " +
            $"Admin: {Admin}, Background: {Background}, Company: {Company}, CompanyHide: {CompanyHide}, " +
            $"Email: {Email}, EmailHide: {EmailHide}, EmailToInvite: {EmailToInvite}, FreeRegistration: {FreeRegistration}, " +
            $"InternalInfo: {InternalInfo}, InvitationsSent: {InvitationsSent}, IsMentor: {IsMentor}, " +
            $"LicencedInternal: {LicencedInternal}, MakeTimeSheet: {MakeTimeSheet}, MemberType: {MemberType}, " +
            $"Mobile: {Mobile}, MobileHide: {MobileHide}, MyStatus: {MyStatus}, MyStatusText: {MyStatusText}, " +
            $"Name: {Name}, Position: {Position}, PositionHide: {PositionHide}, Shortcut: {Shortcut}, " +
            $"Supervisor: {Supervisor}, SupervisorM: {SupervisorM}, ViewPriority: {ViewPriority}");

        return sb.ToString();
    }
}

public class syncUserGroup : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public string? Name { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(base.ToString());

        sb.Append($", Name: {Name}");

        return sb.ToString();
    }
}

public class syncUserGroupItem : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();

    [StringLength(14, MinimumLength = 14)] public string? UserCompany { get; set; } = null;

    public override string ToString()
    {
        var attrsString = attrs != null ? string.Join(", ", attrs) : "null";
        return $"{base.ToString()}, Attributes: [{attrsString}], User Company: {UserCompany}";
    }
}

public class syncWorkspace : ComponentBase
{
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = new();
    public bool? CanAddProjects { get; set; }
    public bool? CanAddTasks { get; set; }
    public string? Description { get; set; }
    public bool? MembersCanAddKoop { get; set; }
    public bool? MembersTools { get; set; }
    public string? Name { get; set; }
    public string? SurveyEmoji { get; set; } = "Stars";
    public bool? SurveyQualityOnCancel { get; set; }
    public bool? SurveyQualityOnClose { get; set; }
    public bool? SurveyTermOnCancel { get; set; }
    public bool? SurveyTermOnClose { get; set; }
    public bool? SurveyText { get; set; }
    public bool? TimeSheetOnClosed { get; set; }
    public string? View { get; set; } = "ProjectTasksList";

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(base.ToString());

        sb.Append($", Can Add Projects: {CanAddProjects}, Can Add Tasks: {CanAddTasks}, Description: {Description}, " +
                  $"Members Can Add Koop: {MembersCanAddKoop}, Members Tools: {MembersTools}, Name: {Name}, " +
                  $"Project Name: {SurveyEmoji}, Survey Quality On Cancel: {SurveyQualityOnCancel}, " +
                  $"Survey Quality On Close: {SurveyQualityOnClose}, Survey Term On Cancel: {SurveyTermOnCancel}, " +
                  $"Survey Term On Close: {SurveyTermOnClose}, Survey Text: {SurveyText}, " +
                  $"Time Sheet On Closed: {TimeSheetOnClosed}, View: {View}");

        return sb.ToString();
    }
}