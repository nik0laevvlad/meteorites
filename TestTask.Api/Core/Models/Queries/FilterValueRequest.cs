namespace TestTask.Core.Models.Queries;

public class FilterValueRequest
{
    public DateTime? YearFrom { get; set; }
    public DateTime? YearTo { get; set; }
    public string? RecClass { get; set; }
    public string? Name { get; set; }
    public string? SortField { get; set; }
    public string? SortOrder { get; set; }
}
