using Microsoft.EntityFrameworkCore;

namespace Tech2023.Web.Shared;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginatedList<T> : List<T>
{
    /// <summary>
    /// The current page index of this custom list instance
    /// </summary>
    public int PageIndex { get; private set; }

    /// <summary>
    /// The total number of pages in the <see cref="PaginatedList{T}"/>
    /// </summary>
    public int PageCount { get; private set; }


    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedList{T}"/> class
    /// </summary>
    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)  
    {
        PageIndex = pageIndex;
        PageCount = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    /// <summary>
    /// Whether the list has a previous page or not
    /// </summary>
    public bool HasPreviousPage => PageIndex > 1;

    /// <summary>
    /// Whether the list has another page or not
    /// </summary>
    public bool HasNextPage => PageIndex < PageCount;

    /// <summary>
    /// Creates a paginated list using the supplied data source
    /// </summary>
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int index, int size)
    {
        int count = await source.CountAsync();

        var items = await source.Skip((index - 1) * size).Take(size).ToListAsync();

        return new PaginatedList<T>(items, count, index, size);
    }

    /// <summary>
    /// Creates a paginated list using an already existing list
    /// </summary>
    public static PaginatedList<T> Create(List<T> source, int index, int size)
    {
        int count = source.Count;

        var items = source.Skip((index - 1) * size).Take(size).ToList();

        return new PaginatedList<T>(items, count, index, size);
    }
}
