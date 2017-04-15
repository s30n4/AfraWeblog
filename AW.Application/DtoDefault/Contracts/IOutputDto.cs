namespace AW.Application.DtoDefault.Contracts
{
    public interface IOutputDto<TOutputDto> : ISearchRequest
    {
        TOutputDto Data { get; set; }
    }
}