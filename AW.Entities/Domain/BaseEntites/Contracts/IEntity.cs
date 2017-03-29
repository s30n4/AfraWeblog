namespace AW.Entities.Domain.BaseEntites.Contracts
{
    /// <summary>
    ///     A shortcut of <see cref="IEntity{TPrimaryKey}" /> for most used primary key type (<see cref="int" />).
    /// </summary>
    public interface IEntity : IEntity<int>
    {
    }
}