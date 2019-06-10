using AppPFashions.Interfaces;
using AppPFashions.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;


namespace AppPFashions.Data
{
    public class DataAccess : IDisposable
    {
        private SQLiteConnection connection;
        public DataAccess()
        {
            var config = DependencyService.Get<IConfig>();
            connection = new SQLiteConnection(config.Platform,
                System.IO.Path.Combine(config.DirectoryDB, "DBPFashions.db3"));
            connection.CreateTable<Usuario>();
            connection.CreateTable<mtraba00>();
            connection.CreateTable<topera01>();
            connection.CreateTable<mdefec00>(); 
            connection.CreateTable<pdefec10>();
            connection.CreateTable<pdefec01>();            
            connection.CreateTable<paudit01>();
            connection.CreateTable<taudit00>();
            //connection.CreateTable<caudit00>();
            connection.CreateTable<ttcmue00>();
            connection.CreateTable<OrdenProduccion>();
        }

        public mtraba00 GetOperario(string ctraba)
        {
            return connection.Table<mtraba00>().FirstOrDefault(c => c.ctraba == ctraba);
        }

        public Usuario GetUsuario()
        {
            return connection.Table<Usuario>().FirstOrDefault();
        }

        public void Insert<T>(T model)
        {
            connection.Insert(model);
        }

        public void Update<T>(T model)
        {
            connection.Update(model);
        }

        public void Delete<T>(T model)
        {
            connection.Delete(model);
        }

        public void DeleteOperarios()
        {
            connection.DeleteAll<mtraba00>();
        }

        public void DeleteOperaciones()
        {
            connection.DeleteAll<topera01>();
        }

        public void DeleteDefectos()
        {
            connection.DeleteAll<mdefec00>();
        }

        public void DeleteAuditoriaDefectos()
        {
            connection.DeleteAll<pdefec01>();
            connection.DeleteAll<pdefec10>();
        }

        public void DeleteAllAuditoria()
        {
            connection.DeleteAll<pdefec01>();
            connection.DeleteAll<pdefec10>();
            //connection.DeleteAll<caudit00>();
            connection.DeleteAll<paudit01>();
            connection.DeleteAll<taudit00>();
        }

        public void DeleteAuditoria(DateTime faudit,string clinea)
        {
            connection.Table<pdefec01>().Delete(x => x.faudit == faudit && x.clinea==clinea);
            connection.Table<pdefec10>().Delete(x => x.faudit == faudit && x.clinea == clinea);            
            connection.Table<paudit01>().Delete(x => x.faudit == faudit && x.clinea == clinea);
            connection.Table<taudit00>().Delete(x => x.faudit == faudit && x.clinea == clinea);
        }

        public void DeleteAudiDefecTemp()
        {
            connection.DeleteAll<pdefec10>();
        }

        public List<OrdenProduccion> GetOP(string nordpr)
        {
            return connection.Table<OrdenProduccion>().Where(c => c.nordpr == nordpr).ToList();
        }

        public void DeleteFichaOP(string nordpr)
        {
            connection.Table<OrdenProduccion>().Delete(op => op.nordpr == nordpr);
        }

        public T First<T>(bool WithChildren) where T : class
        {
            if (WithChildren)
            {
                return connection.GetAllWithChildren<T>().FirstOrDefault();
            }
            else
            {
                return connection.Table<T>().FirstOrDefault();
            }
        }

        public List<T> GetList<T>(bool WithChildren) where T : class
        {
            if (WithChildren)
            {
                return connection.GetAllWithChildren<T>().ToList();
            }
            else
            {
                return connection.Table<T>().ToList();
            }
        }

        public T Find<T>(int pk, bool WithChildren) where T : class
        {
            if (WithChildren)
            {
                return connection.GetAllWithChildren<T>().FirstOrDefault(m => m.GetHashCode() == pk);
            }
            else
            {
                return connection.Table<T>().FirstOrDefault(m => m.GetHashCode() == pk);
            }
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }

}
