using System;
using System.Collections.Generic;
using System.Linq;
using AppPFashions.Models;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLiteNetExtensions.Extensions;


namespace AppPFashions.Data
{
    public class BaseDatos
    {
        static object locker = new object();
        readonly ISQLitePlatform _plataforma;
        string _rutaBD;

        public SQLiteConnection Conexion { get; set; }

        public BaseDatos(ISQLitePlatform plataforma, string rutaBD)
        {
            _plataforma = plataforma;
            _rutaBD = rutaBD;
        }

        public void Conectar()
        {
            Conexion = new SQLiteConnection(_plataforma, _rutaBD, 
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create 
                | SQLiteOpenFlags.FullMutex, true);

            Conexion.CreateTable<Usuario>();
            Conexion.CreateTable<mtraba00>();
            Conexion.CreateTable<topera01>();
            Conexion.CreateTable<mdefec00>();
            Conexion.CreateTable<pdefec10>();
            Conexion.CreateTable<pdefec01>();
            Conexion.CreateTable<paudit01>();
            Conexion.CreateTable<taudit00>();
            Conexion.CreateTable<raudit00>();
            Conexion.CreateTable<ttcmue00>();
            Conexion.CreateTable<OrdenProduccion>();
        }

        public mtraba00 GetOperario(string ctraba)
        {
            lock (locker)
            {
                return Conexion.Table<mtraba00>().FirstOrDefault(c => c.ctraba == ctraba);
            }
        }

        public Usuario GetUsuario()
        {
            lock (locker)
            {
                return Conexion.Table<Usuario>().FirstOrDefault();
            }
        }

        public raudit00 GetResaudit(string careas, DateTime faudit, string clinea)
        {
            lock (locker)
            {
                return Conexion.Table<raudit00>().Where(x => x.careas == careas && x.faudit == faudit && x.clinea == clinea).FirstOrDefault();
            }
        }

        public void Insert<T>(T model)
        {
            lock (locker)
            {
                Conexion.Insert(model);
            }
        }

        public void Update<T>(T model)
        {
            lock (locker)
            {
                Conexion.Update(model);
            }
        }

        public void Deletes<T>(List<T> model)
        {
            lock (locker)
            {
                Conexion.Delete(model);
            }
        }

        public void Delete<T>(T model)
        {
            lock (locker)
            {
                Conexion.Delete(model);
            }
        }

        public void DeleteOperarios()
        {
            lock (locker)
            {
                Conexion.DeleteAll<mtraba00>();
            }
        }

        public void DeleteOperaciones()
        {
            lock (locker)
            {
                Conexion.DeleteAll<topera01>();
            }
        }

        public void DeleteDefectos()
        {
            lock (locker)
            {
                Conexion.DeleteAll<mdefec00>();
            }
        }

        public void DeleteAuditoriaDefectos()
        {
            lock (locker)
            {
                Conexion.DeleteAll<pdefec01>();
                Conexion.DeleteAll<pdefec10>();
            }
        }

        public void DeleteAllAuditoria()
        {
            lock (locker)
            {
                Conexion.DeleteAll<pdefec01>();
                Conexion.DeleteAll<pdefec10>();
                //Conexion.DeleteAll<caudit00>();
                Conexion.DeleteAll<paudit01>();
                Conexion.DeleteAll<taudit00>();
                Conexion.DeleteAll<raudit00>();
            }
        }

        public void DeleteAuditoria(DateTime xfaudit, string clinea)
        {
            lock (locker)
            {
                Conexion.Table<paudit01>().Delete(x => x.faudit == xfaudit && x.clinea == clinea);
                Conexion.Table<taudit00>().Delete(x => x.faudit == xfaudit && x.clinea == clinea);
                Conexion.Table<pdefec01>().Delete(x => x.faudit == xfaudit && x.clinea == clinea);
                Conexion.Table<pdefec10>().Delete(x => x.faudit == xfaudit && x.clinea == clinea);
                Conexion.Table<raudit00>().Delete(x => x.faudit == xfaudit && x.clinea == clinea);
            }
        }

        public void DeleteDefectoTemp()
        {
            lock (locker)
            {                
                Conexion.Table<pdefec10>().Delete(x => x.svigen == "N");                
            }
        }

        public void DeleteAudiDefecTemp()
        {
            lock (locker)
            {
                Conexion.DeleteAll<pdefec10>();
            }
        }

        public List<OrdenProduccion> GetOP(string nordpr)
        {
            lock (locker)
            {
                return Conexion.Table<OrdenProduccion>().Where(c => c.nordpr == nordpr).ToList();
            }
        }

        public void DeleteFichaOP(string nordpr)
        {
            lock (locker)
            {
                Conexion.Table<OrdenProduccion>().Delete(op => op.nordpr == nordpr);
            }
        }

        public T First<T>(bool WithChildren) where T : class
        {
            lock (locker)
            {
                if (WithChildren)
                {
                    return Conexion.GetAllWithChildren<T>().FirstOrDefault();
                }
                else
                {
                    return Conexion.Table<T>().FirstOrDefault();
                }
            }
        }

        public List<T> GetList<T>(bool WithChildren) where T : class
        {
            lock (locker)
            {
                if (WithChildren)
                {
                    return Conexion.GetAllWithChildren<T>().ToList();
                }
                else
                {
                    return Conexion.Table<T>().ToList();
                }
            }
        }

        public T Find<T>(int pk, bool WithChildren) where T : class
        {
            lock (locker)
            {
                if (WithChildren)
                {
                    return Conexion.GetAllWithChildren<T>().FirstOrDefault(m => m.GetHashCode() == pk);
                }
                else
                {
                    return Conexion.Table<T>().FirstOrDefault(m => m.GetHashCode() == pk);
                }
            }
        }

        public void Dispose()
        {
            Conexion.Dispose();
        }

    }
}
