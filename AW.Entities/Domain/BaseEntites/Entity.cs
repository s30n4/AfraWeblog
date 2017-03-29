using AW.Common.SerializationToolkit;
using AW.Entities.Domain.BaseEntites.Contracts;

namespace AW.Entities.Domain.BaseEntites
{
    /// <summary>
    ///     A shortcut of <see cref="Entity{TPrimaryKey}" /> for most used primary key type (<see cref="int" />).
    /// </summary>
    [Serializable]
    public abstract class Entity : Entity<int>, IEntity
    {
    }
}