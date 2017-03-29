namespace AW.Entities.Domain.BaseEntites.Contracts
{
    public interface IApproverAudited : IHasApproveTime
    {
        bool? IsApproved { get; set; }
    }
}