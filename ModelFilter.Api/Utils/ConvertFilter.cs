using ModelFilter.Domain.Utils.Filters;
using Newtonsoft.Json;

namespace ModelFilter.Api.Utils
{
    public static class ConvertFilter
    {
        public static FilterBase ConvertFilterDefault(this string valueFilter)
        {
            if (valueFilter is null) return new FilterBase()
            {
                CurrentPage = 1,
                Filters = new List<Filter>(),
                MaxPerPage = 10,
            };
            return JsonConvert.DeserializeObject<FilterBase>(valueFilter);
        }
    }
}
