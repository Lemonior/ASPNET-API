using ASPNET_API.Configuracoes;
using ASPNET_API.Conexoes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ASPNET_API.Conexoes.Utils.Enums;

namespace ASPNET_API.Inicializar
{
    public class ClsInicializar
    {
        public static void Login(int CodEmpresa)
        {
            string usuario = "Lemon";
            string senha="HSP";
            //////

            //fazendo login e trazendo o nome completo do usuario
            DataSet ds = LemonLogin.logar(usuario, senha);
            //se a quantidade de registros for diferente de 0 (usuario valido)
            if (ds.Tables[0].Rows.Count != 0)
            {
                //gravando dados globais
                Config.Default.G_Usuario_Nome = ds.Tables[0].Rows[0]["NomeCompleto"].ToString();
                Config.Default.G_Usuario_ChUn_Codigo = Convert.ToInt32(ds.Tables[0].Rows[0]["ChUnUsuario"]);
                Config.Default.G_Usuario_GrupoUsuario = ds.Tables[0].Rows[0]["GrupoUsuarios"].ToString();
                Config.Default.G_Usuario_Administrador = ds.Tables[0].Rows[0]["AdmSN"].ToString();
                Config.Default.G_Usuario = usuario;
                Config.Default.G_Senha = senha;
                Config.Default.G_Empresa_Cod = CodEmpresa;
                Config.Default.G_SolicitarImpressora = false;
                Config.Default.G_Sistema = Menu.SetoresNomeSistema(CodEmpresa);
                Config.Default.G_SeparaConciliadoSN = Menu.SeparaConciliadoSN(CodEmpresa);


                //empresa nome
                string Empresa_D = LemonLogin.NomeEmpresa(CodEmpresa);
                Config.Default.G_Empresa_Nome = Empresa_D;


                //inicializa algumas configuração do sistema
                Inicializar.Iniciar();
            }
        }
    }

    public class Inicializar
    {
        //string diretorio_dll = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\Lemon\\";
        //int ponteiroLemonNew = 0;//utilizado para saber qual dll está baixando.

        //vai fazer o preenchimento das variaveis de sistema.

        static public void Iniciar()
        {
            //gravando informações de view

            //pergunta se deseja fechar a aplicação
            //Negocio.Configuracao.Config.Default.G_PerguntaFecharAplicacao = true;
            Config.Default.G_Sistema_pasta = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
            Config.Default.G_SistemaCpuCod = "BFEBFBFF000306C3";
            Config.Default.G_Ma_IdUsuario_codCliente = LemonLogin.Cliente_Codigo();
            Config.Default.G_SistemaPLacaMaeSerial = "NBM7411001425041BE7200";
            Config.Default.G_Certificadora_webservice_teste = webService_teste();
            Config.Default.G_SistemaNomeMaquina = NomeMaquina();
            Config.Default.G_SistemaServidorClienteName = MaquinaServidorClienteName();
            Config.Default.G_CaminhoArquivosDLL = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\Lemon\\";

            Config.Default.G_LastDigits = new Random().Next(0, 999);

            Config.Default.G_BoletoFolder = "";
            Config.Default.G_RemessaFolder = "";

            //dados adicionais para maquina no webservice
            Config.Default.G_CultureName = "pt-BR";
            Config.Default.G_CultureDisplayName = "Português (Brasil)";
            Config.Default.G_OSFullName = "Microsoft Windows 10 Home";
            Config.Default.G_OSPlatForm = "Win32NT";
            Config.Default.G_OSVersion = "6.2.9200.0";
            Config.Default.G_FrameworkVersion = "528372";
            Config.Default.G_is64Bit = Environment.Is64BitOperatingSystem;
            Config.Default.G_PastaTemporaria = CreateTempFolder();


            Config.Default.isLemon = (Config.Default.G_Usuario.ToLower() == "Lemon");
            Config.Default.runAsAdministrator = true;
        }






        private static string CreateTempFolder()
        {
            var folder = @"\TEMP\";
            try
            {

                if (Directory.Exists(folder))
                    Directory.Delete(folder, true);
                Directory.CreateDirectory(folder);
            }
            catch (Exception)
            {
            }
            return folder;
        }

        static public bool webService_teste()
        {
            try
            {
                return (Configuracoes.Config.Default.G_Usuario.ToLower() == "Lemon" || Configuracoes.Config.Default.G_Usuario.ToLower() == "wsteste");
            }
            catch (Exception)
            {
                return true;
            }
        }

        static public string NomeMaquina()
        {
            string nome = Environment.UserName + "_" + Environment.UserDomainName;

            return nome;

        }
        static public string MaquinaServidorClienteName()
        {
            Configuracoes.Config.Default.G_SistemaServidor = false;

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CLIENTNAME")))
            {
                Configuracoes.Config.Default.G_SistemaServidor = true;
                return Environment.GetEnvironmentVariable("CLIENTNAME");
            }
            return "";
        }

    }
}