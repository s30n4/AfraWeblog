namespace AW.Entities.Domain.BaseEntites.Contracts
{
    public interface ICreateionNetwork :  IHasCreationTime
    {
        string CreatorUserIp { get; set; }
    }
}