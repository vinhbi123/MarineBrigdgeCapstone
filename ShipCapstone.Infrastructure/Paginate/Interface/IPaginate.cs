namespace ShipCapstone.Infrastructure.Paginate.Interface;

public interface IPaginate<TResult>
{
    int Size { get; }
    int Page { get; }
    int Total { get; }
    int TotalPages { get; }
    IList<TResult> Items { get; }
}