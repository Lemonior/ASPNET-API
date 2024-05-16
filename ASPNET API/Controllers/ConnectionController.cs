using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASPNET_API.Data;
using ASPNET_API.Models;
using Microsoft.AspNetCore.Mvc;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ASPNET_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectionController : ControllerBase
    {
        [HttpGet(Name = "GetConnectionString")]
        public IEnumerable<_ConnectionString> GetString(int Cod_Cliente)
        {
            //Get ConnectionString from access database
            ASP_UsuarioRepository AccessSQL = new ASP_UsuarioRepository();
            _ConnectionString Parameter = new _ConnectionString();
            AccessSQL.GetConnString(Cod_Cliente);
            Parameter.Database = AccessSQL.NomeBD;
            Parameter.Host = AccessSQL.HostBD;
            Parameter.Porta = AccessSQL.PortaBD;
            Parameter.Usuario = AccessSQL.UsuarioBD;
            Parameter.Senha = AccessSQL.SenhaBD;
            yield return Parameter;
        }
    }
}
