﻿using AW.Application.DtoDefault.Contracts;
using AW.Common.SerializationToolkit;

namespace AW.Application.DtoDefault
{
    /// <summary>
    /// Implements common properties for entity based DTOs.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key</typeparam>
    [Serializable]
    public class EntityDto<TPrimaryKey> : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// Id of the entity.
        /// </summary>
        public TPrimaryKey Id { get; set; }

        /// <summary>
        /// Creates a new <see cref="EntityDto{TPrimaryKey}"/> object.
        /// </summary>
        public EntityDto()
        {
        }

        /// <summary>
        /// Creates a new <see cref="EntityDto{TPrimaryKey}"/> object.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public EntityDto(TPrimaryKey id)
        {
            Id = id;
        }
    }
}