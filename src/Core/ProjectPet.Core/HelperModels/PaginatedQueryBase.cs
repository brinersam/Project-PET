namespace ProjectPet.Core.HelperModels;

public abstract record PaginatedQueryBase()
{
    required public int Page { get; set; }
    required public int RecordAmount { get; set; }
    public int Skip => Math.Max(0, (Page - 1) * RecordAmount);
}
