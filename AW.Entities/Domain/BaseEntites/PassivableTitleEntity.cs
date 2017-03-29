using AW.Entities.Domain.BaseEntites.Contracts;

namespace AW.Entities.Domain.BaseEntites
{
    public class PassivableTitleEntity : PassivableTitleEntity<int>
    {
    }

    public class PassivableTitleEntity<TPrimaryKey> : TitleEntity<TPrimaryKey>, IPassivable
    {
        public bool IsActive { get; set; }

        public PassivableTitleEntity()
        {
            IsActive = true;
        }
    }
}