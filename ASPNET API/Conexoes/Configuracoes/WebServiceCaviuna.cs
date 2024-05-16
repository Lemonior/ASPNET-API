using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ASPNET_API.Configuracoes
{
    class UrlWebservice
    {
        public string Url { get; set; }
        public bool Timeout { get; set; } = false;
    }
}
