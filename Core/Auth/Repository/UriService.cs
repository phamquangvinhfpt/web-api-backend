using Core.Auth.Services;
using Core.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace Core.Auth.Repository
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var endpointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());

            if (!string.IsNullOrEmpty(filter.SortBy))
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "sortBy", filter.SortBy);

            if (!string.IsNullOrEmpty(filter.SortOrder))
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "sortOrder", filter.SortOrder);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "searchTerm", filter.SearchTerm);

            if (!string.IsNullOrEmpty(filter.FilterBy))
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "filterBy", filter.FilterBy);

            if (!string.IsNullOrEmpty(filter.FilterValue))
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "filterValue", filter.FilterValue);

            return new Uri(modifiedUri);
        }
    }
}