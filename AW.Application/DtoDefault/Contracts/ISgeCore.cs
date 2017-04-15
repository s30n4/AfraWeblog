using System.Collections.Generic;
using System.Threading.Tasks;
using AW.Application.DtoDefault;

namespace AW.Application.DtoDefault.Contracts
{
    public interface ISgeCore
    {
        List<HeaderEntity> HeaderParse(SgeCoreDto entityDef, List<HeaderEntity> headers);

        List<HeaderEntity> RefineEntityHeader(List<HeaderEntity> header, string entityName, int languageId = 1);

        IBaseSearchRequest SetBaseSearchRequest(IBaseSearchRequest searchRequest, Task<int> totalCount);

        List<string> ActionParse(List<string> actions, string entityName);

       // Task AddRouteAsync(RouteContentAddDto data);

       // Task<List<int>> GetRouteAsync(RouteContentSearchDto searchRequest);

        //Task DeleteRouteAsync(RouteContentDeleteDto data);

        EntityDefinitionOutputDto GetEntityDefination(string entityDefName);
    }
}