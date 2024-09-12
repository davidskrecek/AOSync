namespace MAUI.Model
{
    internal class syncExistsSimpleComponentsResult
    {
        public List<apiComponentExists> data { get; set; } = new List<apiComponentExists>();
        public bool iserror { get; set; } = false;
        public string error { get; set; } = null!;
        public bool isrepeatable { get; set; } = false;
    }
}
