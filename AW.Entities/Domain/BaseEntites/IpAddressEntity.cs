using System.ComponentModel.DataAnnotations;
using AW.Common.SerializationToolkit;

namespace AW.Entities.Domain.BaseEntites
{
    [Serializable]
    public abstract class IpAddressEntity : IpAddressEntity<int>
    {
    }

    [Serializable]
    public abstract class IpAddressEntity<TPrimaryKey> : Entity<TPrimaryKey>
    {
        [MaxLength(30)]
        public string IpAddress { get; set; }
    }
}