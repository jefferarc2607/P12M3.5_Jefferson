using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SumaCRUD6133261.Models
{
    public class LocalDbService
    {
        private const string DB_NAME = "suma_local.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<Resultado>().Wait();
        }

        public async Task<List<Resultado>> GetResultados()
        {
            return await _connection.Table<Resultado>().ToListAsync();
        }

        public async Task<Resultado> GetResultadoById(int id)
        {
            return await _connection.Table<Resultado>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateResultado(Resultado resultado)
        {
            await _connection.InsertAsync(resultado);
        }

        public async Task UpdateResultado(Resultado resultado)
        {
            await _connection.UpdateAsync(resultado);
        }

        public async Task DeleteResultado(Resultado resultado)
        {
            await _connection.DeleteAsync(resultado);
        }
    }
}
