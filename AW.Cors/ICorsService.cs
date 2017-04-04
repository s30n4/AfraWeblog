using System;

namespace AW.Cors
{
    public interface ICorsService
    {
        CorsResult EvaluatePolicy(string policyName);

        CorsResult EvaluatePolicy(CorsPolicy policy);

        CorsResult EvaluatePolicy(Action<CorsPolicyBuilder> configurePolicy);

        void AddCors(string name, CorsPolicy policy);

        void AddCors(string name, Action<CorsPolicyBuilder> configurePolicy);

        void ApplyCors(CorsResult result);
    }
}