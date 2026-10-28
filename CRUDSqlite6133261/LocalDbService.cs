using CRUDSqlite6133261.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUDSqlite6133261
{
    public class LocalDbService
    {
        private const string DB_NAME = "demo_local_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<Cliente>().Wait();
        }

        public async Task<List<Cliente>> GetClientes()
        {
            return await _connection.Table<Cliente>().ToListAsync();
        }

        public async Task<Cliente> GetClienteById(int id)
        {
            return await _connection.Table<Cliente>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateCliente(Cliente cliente)
        {
            await _connection.InsertAsync(cliente);
        }

        public async Task UpdateCliente(Cliente cliente)
        {
            await _connection.UpdateAsync(cliente);
        }

        public async Task DeleteCliente(Cliente cliente)
        {
            await _connection.DeleteAsync(cliente);
        }
    }
}
