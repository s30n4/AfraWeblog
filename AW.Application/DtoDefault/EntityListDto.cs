using System.Collections.Generic;
using System.Reflection;
using AW.Application.DtoDefault.Contracts;
using AW.Common.SerializationToolkit;

namespace AW.Application.DtoDefault
{
    [Serializable]
    public class EntityListDto<TOutputDto, TPrimaryKey> : IOutputList<TOutputDto>, IEntityDto<TPrimaryKey>
    {
        public IEnumerable<TOutputDto> Data { get; set; }
        public BaseSearchRequest SearchRequest { get; set; }
        public TPrimaryKey Id { get; set; }

        public List<HeaderEntity> Headers { get; set; }

        public List<string> Actions { get; set; }

        public EntityListDto()
        {
            GetPropNames();
        }

        private void GetPropNames()
        {
            Actions = new List<string>();
            Headers = new List<HeaderEntity>();
            foreach (var prop in typeof(TOutputDto).GetProperties())
            {
                if (prop.Name == "Id")
                    Headers.Insert(0, new HeaderEntity(prop.Name));
                else
                    Headers.Add(new HeaderEntity(prop.Name));
            }
        }
    }

    [Serializable]
    public class EntityListDto<TOutputDto> : EntityListDto<TOutputDto, int>
    {
    }
}