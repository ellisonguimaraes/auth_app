using System;

namespace AuthApp.Domain.Utils;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; }

    public int TotalPages { get; }

    public int PageSize { get; }

    public int TotalCount { get; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;

    public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        this.TotalCount = source.Count();
        this.PageSize = pageSize;
        this.CurrentPage = pageNumber;
        this.TotalPages = (int)Math.Ceiling(TotalCount / (double)this.PageSize);

        var items = source.Skip((this.CurrentPage - 1) * this.PageSize)
            .Take(this.PageSize)
            .ToList();

        this.AddRange(items);
    }

    public PagedList(IEnumerable<T> source, int pageNumber, int pageSize, int totalCount)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)this.PageSize);

        AddRange(source);
    }
}