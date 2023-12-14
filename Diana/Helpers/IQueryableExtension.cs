namespace Diana.Helpers
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> AddPagination<T>(this IQueryable<T> query, int page, int take)
             where T : class
            => query.Skip((page-1)*take).Take(take);
    }
}
