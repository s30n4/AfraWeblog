using System;
using AW.Common;

namespace AW.Presentation.Cors
{
    public interface ICorsManager
    {
        ICorsManager WithOrigins(params string[] headers);

        ICorsManager WithHeaders(params string[] headers);

        ICorsManager WithExposedHeaders(params string[] headers);

        ICorsManager WithMethods(params string[] headers);

        ICorsManager AllowCredentials();

        ICorsManager DisallowCredentials();

        ICorsManager AllowAnyOrigin();

        ICorsManager AllowAnyMethod();

        ICorsManager AllowAnyHeader();

        ICorsManager SetPreflightMaxAge(TimeSpan preflightMaxAge);

        void AddCorsOrigin();

        void AddErrorException(SystemError errorContent);
    }
}