using System;
using System.ComponentModel.DataAnnotations;
using AW.Entities.Domain.BaseEntites.Contracts;

namespace AW.Entities.Domain.BaseEntites
{
    [Serializable]
    public abstract class TitleStartEndTimeEntity : TitleStartEndTimeEntity<DateTime>
    {

    }

    [Serializable]
    public abstract class TitleStartEndTimeEntity<TDatetime> : TitleStartEndTimeEntity<TDatetime, TDatetime>
    {

    }

    [Serializable]
    public abstract class TitleStartEndTimeEntity<TStart, TEnd> : IHasStartEndTime<TStart, TEnd>, IHasTitle
    {
        public TStart StarDate { get; set; }
        public TEnd EndDate { get; set; }

        [MaxLength(75)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}