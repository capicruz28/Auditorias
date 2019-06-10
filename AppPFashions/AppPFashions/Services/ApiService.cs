using AppPFashions.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppPFashions.Services
{
    public class ApiService
    {
        public async Task<Response> Login(string user, string password)
        {
            Usuario usuario = new Usuario();
            try
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.BaseAddress = new Uri("http://192.168.1.3:7030");
                var url = "/api/ousuar00?cusuar=" + user + "&cclave=" + password;
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Usuario o contraseña incorrectos",
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                usuario = JsonConvert.DeserializeObject<Usuario>(result);
            
                return new Response
                {
                    IsSuccess = true,                    
                    Result = usuario,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Usuario o contraseña incorrectos",
                    //    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Operarios<mtraba00>()
        {
            List<mtraba00> operarios = new List<mtraba00>();
            try
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.BaseAddress = new Uri("http://192.168.1.3:7030");
                var url = "/api/mtraba00";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Hubo problemas con la sincronización",
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                operarios = JsonConvert.DeserializeObject<List<mtraba00>>(result);

                return new Response
                {
                    IsSuccess = true,                    
                    Result = operarios,
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

        public async Task<Response> Operaciones<topera01>()
        {
            List<topera01> operaciones = new List<topera01>();
            try
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.BaseAddress = new Uri("http://192.168.1.3:7030");
                var url = "/api/topera01";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Hubo problemas con la sincronización",
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                operaciones = JsonConvert.DeserializeObject<List<topera01>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = operaciones,
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

        public async Task<Response> Defectos<mdefec00>()
        {
            List<mdefec00> defectos = new List<mdefec00>();
            try
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.BaseAddress = new Uri("http://192.168.1.3:7030");
                var url = "/api/mdefec00";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Hubo problemas con la sincronización",
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                defectos = JsonConvert.DeserializeObject<List<mdefec00>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = defectos,
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

        public async Task<Response> GetOP<ordprod>(string nordpr)
        {
            List<ordprod> fichas = new List<ordprod>();
            try
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.BaseAddress = new Uri("http://192.168.1.3:7030");
                var url = "/api/ordprod?nordpr=" + nordpr;
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al intentar traer datos de la OP",
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                fichas = JsonConvert.DeserializeObject<List<ordprod>>(result);

                if (fichas.Count == 0)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "La OP " + nordpr + " no existe",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Result = fichas,
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No se encuentra dentro de la red de Perú Fashions",
                };
            }
        }

        public async Task<Response> GetAuditoriaResumen<paudit10>(string careas,DateTime faudit)
        {
            List<paudit10> fichas = new List<paudit10>();
            try
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.BaseAddress = new Uri("http://192.168.1.3:7030");
                var url = "/api/paudit10?careas=" + careas+ "&faudit=2018-11-10";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al intentar traer datos de la OP",
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                fichas = JsonConvert.DeserializeObject<List<paudit10>>(result);

                if (fichas.Count == 0)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "La OP " + careas + " no existe",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Result = fichas,
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No se encuentra dentro de la red de Perú Fashions",
                };
            }
        }

        public async Task<Response> Aql<ttcmue00>()
        {
            List<ttcmue00> muestra = new List<ttcmue00>();
            try
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.BaseAddress = new Uri("http://192.168.1.3:7030");
                var url = "/api/ttcmue00";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Hubo problemas con la sincronización",
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                muestra = JsonConvert.DeserializeObject<List<ttcmue00>>(result);

                return new Response
                {
                    IsSuccess = true,
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

        public async Task<Response> GetPiezas<ppzxes00>(string nordpr,string ccarub)
        {
            List<ppzxes00> piezas = new List<ppzxes00>();
            try
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.BaseAddress = new Uri("http://192.168.1.3:7030");
                var url = "/api/ppzxes00?nordpr=" + nordpr+"&ccarub="+ccarub;
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al intentar traer datos de la OP",
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                piezas = JsonConvert.DeserializeObject<List<ppzxes00>>(result);

                if (piezas.Count == 0)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "La OP " + nordpr + " no existe",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Result = piezas,
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No se encuentra dentro de la red de Perú Fashions",
                };
            }
        }

        public async Task<Response> GetCorte<pcorte00>(string nordpr, string nordct,string npieza)
        {
            List<pcorte00> corte = new List<pcorte00>();
            try
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.BaseAddress = new Uri("http://192.168.1.3:7030");
                var url = "/api/pcorte00?nordpr=" + nordpr + "&nordct=" + nordct + "&npieza=" + npieza;
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al intentar traer datos de la OP",
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                corte = JsonConvert.DeserializeObject<List<pcorte00>>(result);

                if (corte.Count == 0)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "La OP " + nordpr + " no existe",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Result = corte,
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No se encuentra dentro de la red de Perú Fashions",
                };
            }
        }

        public async Task<Response> GetCorteColor<padcor00>(string nordpr, string nordct)
        {
            List<padcor00> corte = new List<padcor00>();
            try
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.BaseAddress = new Uri("http://192.168.1.3:7030");
                var url = "/api/padcor00?nordpr=" + nordpr + "&nordct=" + nordct ;
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al intentar traer datos de la OP",
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                corte = JsonConvert.DeserializeObject<List<padcor00>>(result);

                if (corte.Count == 0)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "La OP " + nordpr + " no existe",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Result = corte,
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No se encuentra dentro de la red de Perú Fashions",
                };
            }
        }

        public async Task<Response> GetUltRegistro(string careas, string faudit, string clinea)
        {
            paudit02 ultregistro = new paudit02();
            try
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.BaseAddress = new Uri("http://192.168.1.3:7030");
                var url = "/api/paudit02?careas=" + careas + "&faudit=" + faudit + "&clinea=" + clinea;
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al intentar traer el ultimo registro de auditoria",
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                ultregistro = JsonConvert.DeserializeObject<paudit02>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = ultregistro,
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No se encuentra dentro de la red de Perú Fashions",
                };
            }
        }

        public async Task<Response> GetFichasPdf<OrdenProduccion>(string nordpr)
        {
            List<OrdenProduccion> corte = new List<OrdenProduccion>();
            try
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.BaseAddress = new Uri("http://192.168.1.3:7030");
                var url = "/OrdenP?nordpr=" + nordpr;
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al intentar traer datos de la OP",
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                corte = JsonConvert.DeserializeObject<List<OrdenProduccion>>(result);

                if (corte.Count == 0)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "La OP " + nordpr + " no existe",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Result = corte,
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No se encuentra dentro de la red de Perú Fashions",
                };
            }
        }

    }
}
