using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASPNET_API.Conexoes.Utils
{
    public class Enums
    {
        public enum TypeDataBase
        {
            Access = 0,
            SQLServer = 1,
            MySQL = 2,
            PostgresSQL = 3,
            LocalDB = 4
        }

        public enum SQLDataFormat
        {
            Nenhum = 0,
            DiaMesAno = 1,
            DiaMesAnoHoraMin = 2,
            DiaMesAnoHoraMinSeg = 3,
        }

    }
}
