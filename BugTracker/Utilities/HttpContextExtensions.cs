using Microsoft.EntityFrameworkCore;

namespace BugTracker.Utilities
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParametersInHeader<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {
            if(httpContext == null) 
                throw new ArgumentNullException(nameof(httpContext));
            double quantity = await queryable.CountAsync();
            httpContext.Response.Headers.Add("TotalItemAmount", quantity.ToString());
        }
    }
}
