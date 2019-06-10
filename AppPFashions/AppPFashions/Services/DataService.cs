using AppPFashions.Data;
using AppPFashions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppPFashions.Services
{
    public class DataService
    {
        public Response InsertUser(Usuario user)
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var oldUser = da.First<Usuario>(false);
                    if (oldUser != null)
                    {
                        da.Delete(oldUser);
                    }
                    da.Insert(user);               
                }
                return new Response
                {
                    IsSuccess = true,
                    Message = "Usuario Insertado OK",
                    Result = user,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public Usuario GetUser()
        {
            using (var da = new DataAccess())
            {
                return da.First<Usuario>(false);
            }
        }

        public Response UpdateUser(Usuario user)
        {
            try
            {
                using (var da = new DataAccess())
                {
                    da.Update(user);               
                }
                return new Response
                {
                    IsSuccess = true,
                    Message = "Usuario Actualizado OK",
                    Result = user,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public Response InsertOperario(mtraba00 opera)
        {
            try
            {
                using (var da = new DataAccess())
                {                    
                    da.Insert(opera);
                }
                return new Response
                {
                    IsSuccess = true,
                    Message = "Usuario Insertado OK",
                    Result = opera,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public Response InsertOperacion(topera01 opera)
        {
            try
            {
                using (var da = new DataAccess())
                {
                    da.Insert(opera);
                }
                return new Response
                {
                    IsSuccess = true,
                    Message = "Operación Insertado OK",
                    Result = opera,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public Response InsertDefecto(mdefec00 defecto)
        {
            try
            {
                using (var da = new DataAccess())
                {
                    da.Insert(defecto);
                }
                return new Response
                {
                    IsSuccess = true,
                    Message = "Operación Insertado OK",
                    Result = defecto,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public Response InsertAql(ttcmue00 muestra)
        {
            try
            {
                using (var da = new DataAccess())
                {
                    da.Insert(muestra);
                }
                return new Response
                {
                    IsSuccess = true,
                    Message = "Usuario Insertado OK",
                    Result = muestra,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public T InsertOrUpdate<T>(T model) where T : class
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var oldRecord = da.Find<T>(model.GetHashCode(), false);
                    if (oldRecord != null)
                    {
                        da.Update(model);
                    }
                    else
                    {
                        da.Insert(model);
                    }
                    return model;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return model;

            }
        }

        public void Save<T>(List<T> list) where T : class
        {
            using (var da = new DataAccess())
            {
                foreach (var record in list)
                {
                    InsertOrUpdate(record);
                }
            }
        }
    }
}
