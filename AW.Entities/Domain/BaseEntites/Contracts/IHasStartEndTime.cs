using System;

namespace AW.Entities.Domain.BaseEntites.Contracts
{
    public interface IHasStartEndTime : IHasStartEndTime<DateTime?, DateTime?>
    {
    }

    public interface IHasStartEndTime<TDatetime> : IHasStartEndTime<TDatetime, TDatetime>
    {
    }

    public interface IHasStartEndTime<TStart, TEnd>
    {
        TStart StarDate { get; set; }
        TEnd EndDate { get; set; }
    }
}