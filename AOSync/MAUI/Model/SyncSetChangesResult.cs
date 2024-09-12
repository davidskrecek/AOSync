namespace MAUI.Model
{
    internal class syncSetChangesResult
    {
        public int errorchangeindex { get; set; }
        public string detailerror { get; set; } = null!;
        public List<syncResultChange> results { get; set; } = new List<syncResultChange>();
        public bool iserror { get; set; } = false;
        public string error { get; set; } = null!;
        public bool isrepeatable { get; set; } = false;
    }
}
