using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASPNET_API.Conexoes.Utils
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ColunaBanco : System.Attribute
    {
        public string name { get; private set; }
        public ColunaBanco(string name)
        {
            this.name = name;
        }
    }
}
