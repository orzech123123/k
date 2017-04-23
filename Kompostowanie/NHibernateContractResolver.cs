using System;
using Newtonsoft.Json.Serialization;

namespace Kompostowanie
{
    public class NHibernateContractResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            return base.CreateContract(typeof(NHibernate.Proxy.INHibernateProxy).IsAssignableFrom(objectType) ? objectType.BaseType : objectType);
        }
    }
}