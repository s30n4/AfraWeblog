namespace AW.Common
{
    public class BaseSearchRequest
    {
        public int PageSize { get; set; }

        public int SkipCount { get; set; }

        public int LanguageId { get; set; }

        public BaseSearchRequest()
        {

        }
    }
}
