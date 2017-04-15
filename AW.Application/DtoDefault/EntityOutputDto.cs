using System.Collections.Generic;
using System.Reflection;
using AW.Application.DtoDefault.Contracts;

namespace AW.Application.DtoDefault
{
    public class EntityOutputDto<TOutputDto> : EntityOutputDto<TOutputDto, int>
    { }

    public class EntityOutputDto<TOutputDto, TPrimaryKey> : IOutputDto<TOutputDto>, IEntityDto<TPrimaryKey>
    {
        public TOutputDto Data { get; set; }
        public BaseSearchRequest SearchRequest { get; set; }
        public TPrimaryKey Id { get; set; }

        public List<HeaderEntity> Headers { get; set; }
        public List<string> Actions { get; set; }

        public EntityOutputDto()
        {
            GetPropNames();
        }

        private void GetPropNames()
        {
            Headers = new List<HeaderEntity>();
            Actions = new List<string>();
            foreach (var prop in typeof(TOutputDto).GetProperties())
            {
                if (prop.Name == "Id")
                    Headers.Insert(0, new HeaderEntity(prop.Name));
                else
                    Headers.Add(new HeaderEntity(prop.Name));
            }
        }
    }
}