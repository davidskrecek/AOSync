﻿namespace AOSync.MAUI.Model;

internal class syncGetCurrentTranidResponse
{
    public string tranid { get; set; }
    public bool iserror { get; set; }
    public string error { get; set; }
    public bool isrepeatable { get; set; }

    public string ToString()
    {
        return $"{tranid}, {iserror}, {error}, {isrepeatable}";
    }
}