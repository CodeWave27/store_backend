using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace store_backend.Helpers
{
    public class IgnorePassword: DefaultContractResolver
    {
        private readonly bool _ignorePassword;

        public IgnorePassword(bool ignore)
        {
            _ignorePassword= ignore;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property= base.CreateProperty(member, memberSerialization);

            if(property.PropertyName== "password")
            {
                property.ShouldSerialize = instance => !_ignorePassword;
            }
            return property;
        }

    }
}
