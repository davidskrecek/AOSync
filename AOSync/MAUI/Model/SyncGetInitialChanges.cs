using System.ComponentModel.DataAnnotations;

namespace MAUI.Model
{
    internal class syncGetInitialChanges
    {
        [Required]
        public string company { get; set; } = null!;
        public string lastcomponentid { get; set; } = null!;
        public string maxtranid { get; set; } = null!;
        public int limit { get; set; } = 0;
        public bool withexternalid { get; set; } = false;
        public bool addrelations { get; set; } = false;
        public bool simpleResult { get; set; } = false;

        public override string ToString()
        {
            return $"Company: {company}, Last Component ID: {lastcomponentid}, Max Tran ID: {maxtranid}, " +
                   $"Limit: {limit}, With External ID: {withexternalid}, Add Relations: {addrelations}, " +
                   $"Simple Result: {simpleResult}";
        }
    }
}
