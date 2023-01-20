using AudioRecorder.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AudioRecorder.Services
{
    public class BancoService : IDataStore<Audio>
    {
        private String _banco = "audio.db";
        public string DatabasePath
        {
            get
            {
                var connectionStringBuilder = new SqliteConnectionStringBuilder();
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                String caminho = Path.Combine(basePath, _banco);
                connectionStringBuilder.DataSource = caminho;
                return connectionStringBuilder.ConnectionString;
            }
        }
        private async Task<bool> CriarBanco()
        {
            using (var conexao = new SqliteConnection(DatabasePath))
            {
                conexao.Open();
                try
                {
                    var command = conexao.CreateCommand();
                    command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS audio (
                        id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        nome varchar(100) NOT NULL,
                        arquivo blob
                    );
                ";
                    command.ExecuteNonQuery();
                    return await Task.FromResult(true);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }
        public async Task<bool> AddItemAsync(Audio item)
        {
            try
            {
                await CriarBanco();
            }catch(Exception ex)
            {
                throw ex;
            }
            using (var conexao = new SqliteConnection(DatabasePath))
            {
                conexao.Open();
                try
                {
                    using (var transaction = conexao.BeginTransaction())
                    {
                        try
                        {

                            var command = conexao.CreateCommand();
                            command.CommandText =
                            String.Format(@"
                            insert into audio (nome, arquivo ) values('{0}', @pic)
                            ", item.Nome);
                            command.Parameters.Add("@pic", SqliteType.Blob);
                            command.Parameters[0].Value = item.Arquivo;
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            return await Task.FromResult(true);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            try
            {
                await this.CriarBanco();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            try
            {
                using (var conexao = new SqliteConnection(DatabasePath))
                {
                    conexao.Open();
                    try
                    {
                        using (var transaction = conexao.BeginTransaction())
                        {
                            try
                            {

                                var command = conexao.CreateCommand();
                                command.CommandText =
                                String.Format(@"
                            delete from audio where id = '{0}'
                            ", id);
                                command.ExecuteNonQuery();
                                transaction.Commit();
                                return await Task.FromResult(true);
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conexao.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }

        public Task<Audio> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Audio>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                await this.CriarBanco();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            List<Audio> lista = new List<Audio>();
            using (var conexao = new SqliteConnection(DatabasePath))
            {
                conexao.Open();
                try
                {
                    var command = conexao.CreateCommand();
                    command.CommandText =
                    String.Format(@"
                        select id, nome, arquivo from audio;
                    ");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Audio item = new Audio();
                            item.Id = reader.GetInt32(0);
                            item.Nome = reader.GetString(1);
                            MemoryStream outputStream = new MemoryStream();
                            using (var readStream = reader.GetStream(2))
                            {
                                await readStream.CopyToAsync(outputStream);
                            }
                            item.Arquivo = outputStream.ToArray();
                            lista.Add(item);
                        }
                    }
                    return await Task.FromResult(lista);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conexao.Close();
                }

            }
        }
        public async Task<bool> UpdateItemAsync(Audio item)
        {
            try
            {
                await this.CriarBanco();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            try
            {
                using (var conexao = new SqliteConnection(DatabasePath))
                {
                    conexao.Open();
                    try
                    {
                        using (var transaction = conexao.BeginTransaction())
                        {
                            try
                            {
                                var command = conexao.CreateCommand();
                                command.CommandText =
                                String.Format(@"
                                update audio set arquivo = @pic  where id = '{0}'
                                ", item.Id);
                                command.Parameters.Add("@pic", SqliteType.Blob);
                                command.Parameters[0].Value = item.Arquivo;
                                command.ExecuteNonQuery();
                                transaction.Commit();
                                return await Task.FromResult(true);
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conexao.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
