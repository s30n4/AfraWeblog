using System.Threading.Tasks;
using AW.Application.DtoDefault;

namespace AW.Application.DtoDefault.Contracts
{
    public interface IEntityDefinition
    {
        EntityDefinitionOutputDto Add(EntityDefinitionDto data);

        Task<EntityDefinitionOutputDto> GetAsync(string entityName);

        EntityDefinitionOutputDto Get(string entityName);
    }
}