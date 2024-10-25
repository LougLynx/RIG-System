using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace Manage_Receive_Issues_Goods.Fomat
{
    public class PascalCaseNamingStrategy : NamingStrategy
    {
        protected override string ResolvePropertyName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            return char.ToUpper(name[0]) + name.Substring(1);
        }
    }
}
