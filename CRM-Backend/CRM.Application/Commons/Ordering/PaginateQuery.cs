using CRM.Application.Commons.Bases.Request;

namespace CRM.Application.Commons.Ordering
{
    public static class PaginateQuery
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, BasePaginationRequest request)
        {
            return queryable.Skip((request.NumPage - 1) * request.Records).Take(request.Records);
        }
    }
}
