using System.Collections.Generic;

namespace AW.Application.DtoDefault.Contracts
{
    public interface IOutputList<TOutputDto> : ISearchRequest
    {
        IEnumerable<TOutputDto> Data { get; set; }
    }
}