﻿namespace MAUI.Model
{
    internal class syncGetChangesResponse
    {
        public string lasttranid { get; set; } = null!;
        public bool moredata { get; set; }
        public List<syncGetTransaction> trans { get; set; } = new List<syncGetTransaction>();

        public bool iserror { get; set; }

        public string error { get; set; } = null!;

        public bool isrepeatable { get; set; }

        public override string ToString()
        {
            var transString = string.Join(", ", trans.Select(t => t.ToString()));
            return $"LastTranId: {lasttranid}, MoreData: {moredata}, Trans: [{transString}], IsError: {iserror}, Error: {error}, IsRepeatable: {isrepeatable}";
        }
    }
}
