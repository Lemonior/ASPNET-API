using ASPNET_API.Models;
using System.Data;
using Dapper;
using System;
using System.Collections.Generic;
using NuGet.Protocol.Plugins;

namespace ASPNET_API.Data.Repositories
{
    public abstract class BaseRepository<T> where T : Entity
    {
        public readonly IDbConnection _conexao;
        private readonly string _table;

        protected BaseRepository()
        {
            _conexao = ApplicationDb.GetDefaultConnection();
            _table = typeof(T).Name;
        }

        public int Delete(T entity)
        {
            return _conexao.Execute($"DELETE FROM {_table} WHERE {nameof(Entity.Id)}={entity.Id}");
        }

        public IEnumerable<T> GetAll()
        {
            return _conexao.Query<T>($"SELECT ID, Usuario, Email, Senha, Status FROM {_table}");
        }

        public T GetDatabaseConn(int id)
        {
            return _conexao.QueryFirst<T>($"SELECT NomeDB, UsuarioDB, SenhaDB, HostDB, PortaDB FROM ASP_Usuario_Cliente WHERE ID_ASP_Usuario={id}");
        }

    }
}
