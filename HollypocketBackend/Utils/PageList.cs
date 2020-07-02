using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollypocketBackend.Utils
{
    public class PagedList<T>
    {

        public List<T> Items { get; private set; }

        public Pagination Pagination { get; private set; }

        public PagedList(List<T> items, int count, int pageSize, int pageNumber)
        {
            Pagination = new Pagination(count, pageNumber, pageSize);
            Items = items;
        }

        public async static Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber = 1, int pageSize = 10)
        {
            pageSize = Math.Min(pageSize, 100);
            var count = source.Count();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageSize, pageNumber);
        }

        public async static Task<PagedList<T>> ToPagedList(List<T> source, int pageSize = 10, int pageNumber = 1)
        {
            pageSize = Math.Min(pageSize, 100);
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageSize, pageNumber);
        }

    }

    public class Pagination
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public Pagination(int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
