using static ASPNET_API.Conexoes.Utils.Enums;

namespace ASPNET_API.Configuracoes
{
    public class Conf
    {
        static public void setStrConnection(string str,TypeDataBase dataBase)
        {
            
            Config.Default.strConnection = str;
            Config.Default.G_IDBanco = dataBase.GetHashCode();
        }
    }
    
}
