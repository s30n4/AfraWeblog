using System;

namespace AW.Entities.Domain.BaseEntites.Contracts
{
    public interface IHasShowTime
    {
        DateTime? ShowStartDate { get; set; }

        DateTime? ShowEndDate { get; set; }
    }
}