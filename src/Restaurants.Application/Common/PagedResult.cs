namespace Restaurants.Application.Common;

public class PagedResult<T>
{
    public PagedResult(IEnumerable<T> items, int totalCount, int PageSize, int PageNumber)
    {
        Items = items;
        TotalItemsCount = totalCount;
        TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);
        ItemsFrom = PageSize * (PageNumber - 1) + 1;
        ItemsTo = ItemsFrom + PageSize - 1;
    }
    public IEnumerable<T> Items { get; set; }
    public int TotalItemsCount { get; set; }
    public int TotalPages { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
}
