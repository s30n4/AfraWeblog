using AW.Application.DtoDefault;

namespace AW.Application.DtoDefault.Contracts
{
    public interface ISearchRequest
    {
        BaseSearchRequest SearchRequest { get; set; }
    }
}