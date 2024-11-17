namespace CleanArch.eCode.WebApp.Core.ViewModels;

public class SelectItem
{
    public SelectItem(string value, string name)
    {
        Value = value;
        Name = name;
    }

    public string Value { get; set; }
    public string Name { get; set; }
}