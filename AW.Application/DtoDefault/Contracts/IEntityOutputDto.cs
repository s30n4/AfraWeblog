using System.Collections.Generic;

namespace AW.Application.DtoDefault.Contracts
{
    public interface IEntityOutputDto<TDestination>
    {
        List<string> Headers { get; set; }

        TDestination Information { get; set; }
    }
}