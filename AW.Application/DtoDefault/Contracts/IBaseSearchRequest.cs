namespace AW.Application.DtoDefault.Contracts
{
    public interface IBaseSearchRequest : IPagedResultRequest, ISortedResultRequest, IHasTotalCount, ILanguegeEntityDto
    {
    }
}