using System;
using AW.Entities.Domain.BaseEntites.Contracts;

namespace AW.Entities.Domain.BaseEntites
{
    [Serializable]
    public abstract class StartEndTimeEntity : StartEndTimeEntity<DateTime?>
    {
    }

    [Serializable]
    public abstract class StartEndTimeEntity<TDatetime> : StartEndTimeEntity<TDatetime, TDatetime>
    {
    }

    [Serializable]
    public abstract class StartEndTimeEntity<TStart, TEnd> : Entity, IHasStartEndTime<TStart, TEnd>
    {
        public TStart StarDate { get; set; }
        public TEnd EndDate { get; set; }
    }
}