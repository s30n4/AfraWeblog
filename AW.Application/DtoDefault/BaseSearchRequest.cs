using AW.Application.DtoDefault.Contracts;
using System.Threading.Tasks;

namespace AW.Application.DtoDefault
{
    public class BaseSearchRequest : IBaseSearchRequest
    {
        public BaseSearchRequest()
        {
            PageSize = 10;
            PageIndex = 1;
            SkipCount = PageSize * (PageIndex - 1);
            SortTypeId = -1;
            LanguageId = 1;
            SortDirection = "Asc";
            SortColumn = "";
        }

        public BaseSearchRequest(int pageSize)
        {
            PageSize = pageSize;
            PageIndex = 1;
            SkipCount = PageSize * (PageIndex - 1);
            SortTypeId = -1;
            LanguageId = 1;
            SortDirection = "Asc";
            SortColumn = "";
        }
        


        public BaseSearchRequest(int pageSize, int pageIndex)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;

            SkipCount = PageSize * (PageIndex - 1);
            SortTypeId = -1;
            LanguageId = 1;
            SortDirection = "Asc";
            SortColumn = "";
        }

        public BaseSearchRequest(int pageSize, int pageIndex, Task<int> totalCount)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;

            SkipCount = PageSize * (PageIndex - 1);
            SortTypeId = -1;
            Total = totalCount;
            LanguageId = 1;
            SortDirection = "Asc";
            SortColumn = "";
        }

        public BaseSearchRequest(int pageSize, int pageIndex, Task<int> totalCount, int languageId)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;

            SkipCount = PageSize * (PageIndex - 1);
            SortTypeId = -1;
            Total = totalCount;
            LanguageId = languageId;
            SortDirection = "Asc";
            SortColumn = "";
        }

        public Task<int> Total { get; set; }

        public int MaxResultCount { get; set; }
        public int SkipCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int SortTypeId { get; set; }

        public int LanguageId { get; set; }

        public string SortColumn { get; set; }

        public string SortDirection { get; set; }
    }
}