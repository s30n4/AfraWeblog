using System;
using System.Collections.Generic;

namespace AW.Cors
{
    internal class CorsOptions
    {
        public IDictionary<string, CorsPolicy> PolicyMap;
        private static volatile CorsOptions _instance;
        private static readonly object SyncRoot = new object();

        private CorsOptions()
        {
            PolicyMap = new Dictionary<string, CorsPolicy>();
        }

        internal static CorsOptions Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance != null) return _instance;

                    _instance = new CorsOptions();
                }

                return _instance;
            }
        }

        public static void AddPolicy(string name, CorsPolicy policy)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            Instance.PolicyMap[name] = policy;
        }

        public static void AddPolicy(string name, Action<CorsPolicyBuilder> configurePolicy)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (configurePolicy == null)
            {
                throw new ArgumentNullException(nameof(configurePolicy));
            }

            var policyBuilder = new CorsPolicyBuilder();
            configurePolicy(policyBuilder);

            Instance.PolicyMap[name] = policyBuilder.Build();
        }

        public static CorsPolicy GetPolicy(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return Instance.PolicyMap.ContainsKey(name) ? Instance.PolicyMap[name] : null;
        }
    }
}