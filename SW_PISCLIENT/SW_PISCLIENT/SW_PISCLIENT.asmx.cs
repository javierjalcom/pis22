using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;


using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using System.Data;

namespace SW_PISCLIENT
{
    /// <summary>
    /// Descripción breve de SW_PISCLIENT
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class SW_PISCLIENT : System.Web.Services.WebService
    {
       //public PISSecurityResponse iobj_response;

        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }

        [WebMethod]
        //public string GetToken()

        public PISSecurityResponse GetToken()  
        {
            string _url = "";
            string _user = "";
            string _password = "";
            string _guid = "";
            int _idAPI = 0;

            PISSecurityRequest lobj_Request = new PISSecurityRequest();
               
            _url = ConfigurationManager.AppSettings["url_pislogin"];
            _user = ConfigurationManager.AppSettings["usuario"];
            _password = ConfigurationManager.AppSettings["password"];
            _guid= ConfigurationManager.AppSettings["guid"];
            if (  int.TryParse(ConfigurationManager.AppSettings["guid"].ToString(), out _idAPI)== false)
            {
                _idAPI = 0;
            }

            lobj_Request.usuario = _user;
            lobj_Request.password = _password;
            lobj_Request.guid = _guid;

            var client = new RestClient(_url);
           
            var request = new RestRequest(_url, Method.Post);

            request.AddHeader("content-type", "application/json");
            //request.AddParameter("application/json", "{ \"grant_type\":\"client_credentials\" }", ParameterType.RequestBody);
            
            string jsonString;
            jsonString = System.Text.Json.JsonSerializer.Serialize(lobj_Request);
            //request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
            request.AddParameter("application/json", jsonString, ParameterType.RequestBody);

            var responseJson = client.Execute(request).Content;

            var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson)["mensaje"].ToString();
            token = token;

            PISSecurityResponse iobj_response = new PISSecurityResponse();
            iobj_response = new PISSecurityResponse();
            iobj_response = JsonConvert.DeserializeObject<PISSecurityResponse>(responseJson);

            return iobj_response;
         //   return responseJson.ToString();
        }

        
        //////
        ///

        [WebMethod]
        public string GetContainersTypes()
        {
            string _url = "";
            string lstr_temp = "";

            PISSecurityResponse iobj_response = new PISSecurityResponse();
            //lstr_temp = GetToken();
            iobj_response = GetToken();

            //PISSecurityRequest lobj_Request = new PISSecurityRequest();


            _url = ConfigurationManager.AppSettings["url_pistipoContenedor"];

            var client = new RestClient(_url);
    
            //client.Authenticator = new HttpBasicAuthenticator(_user, _password);
            //var request = new RestRequest("api/users/login", Method.POST);
            var request = new RestRequest(_url, Method.Get);

            //request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + iobj_response.valor);
            //request.AddParameter("application/json", "{ \"grant_type\":\"client_credentials\" }", ParameterType.RequestBody);

            //  string jsonString;
            //  jsonString = System.Text.Json.JsonSerializer.Serialize(lobj_Request);

            //request.AddParameter("application/json", jsonString, ParameterType.RequestBody);

            //request.AddParameter("application/json", jsonString, ParameterType.RequestBody);

            var responseJson = client.Execute(request).Content;

            //var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson)["mensaje"].ToString();

            //List<PISTipoContendorResponse> listresult = JsonConvert.DeserializeObject<List<PISTipoContendorResponse>>(responseJson);
            PISTipoContendorResponse objTiposContResponse = JsonConvert.DeserializeObject<PISTipoContendorResponse>(responseJson);
            

            //List<ShippingInfo> shippingInfo = JsonConvert.DeserializeObject<List<ShippingInfo>>(json);

            //token = token;

            //PISSecurityResponse iobj_response = new PISSecurityResponse();
            //iobj_response = new PISSecurityResponse();
            //iobj_response = JsonConvert.DeserializeObject<PISSecurityResponse>(responseJson);


            return responseJson.ToString();
        }

        [WebMethod]
        public string AssingVisitToPIS(long alng_VisitId)
        {
            string _url = "";
            string lstr_temp = "";
            int lint_obj = 0;


            SolicitarCitaRequest lobj_Visita= new SolicitarCitaRequest();
            PISSecurityResponse iobj_sec = new PISSecurityResponse();
            SW_PISSource.SW_PISSource lswref = new SW_PISSource.SW_PISSource();

            DataTable ldtb_Result = new DataTable();// ' la tabla que obtiene el resultado
            ldtb_Result.TableName = "result";
            iobj_sec = GetToken();

            _url = ConfigurationManager.AppSettings["url_pisSolicitaCita"];

            var client = new RestClient(_url);


            var request = new RestRequest(_url, Method.Post);

            //request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + iobj_sec.valor);
            //request.AddParameter("application/json", "{ \"grant_type\":\"client_credentials\" }", ParameterType.RequestBody);

            // obtener informacion
            try
            {
                ldtb_Result = lswref.GetPISInfo(alng_VisitId);
                lobj_Visita.codigoCita = ldtb_Result.Rows[0]["strVisitCodigo"].ToString();

                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["idTipoOperacion"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.idTipoOperacion = lint_obj;


                lobj_Visita.bl = ldtb_Result.Rows[0]["strBL"].ToString();
                lobj_Visita.noPedimento= ldtb_Result.Rows[0]["strPedimento"].ToString();
                //--
                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["idTipo"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.idTipo= lint_obj;
                //--
                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["intMotivo"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.idMotivo= lint_obj;

                //--
                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["idRecintoOrigen"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.idRecintoOrigen = lint_obj;

                //--
                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["idRecintoDestino"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.idRecintoDestino= lint_obj;

                //
                lobj_Visita.imo = ldtb_Result.Rows[0]["strimo"].ToString();
                lobj_Visita.nombreBuque = ldtb_Result.Rows[0]["strBuque"].ToString();

                //--
                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["idSolicitante"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.idSolicitante = lint_obj;

                //
                //--
                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["idAgenteAduanal"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.idAgenteAduanal = lint_obj;

                //
                //--
                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["idTipoProducto"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.idTipoProducto= lint_obj;

                //
                //--
                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["idManiobra"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.idManiobra = lint_obj;

                //
                //--
                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["idTipoDespacho"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.idTipoDespacho = lint_obj;

                //
                lobj_Visita.noContenedor = ldtb_Result.Rows[0]["noContenedor"].ToString();

                //
                lobj_Visita.idTipoContenedor = ldtb_Result.Rows[0]["idTipoContenedor"].ToString();

               
                //--
                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["idEstadoContenedor"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.idEstadoContenedor= lint_obj;

                //
                lobj_Visita.ium = ldtb_Result.Rows[0]["strium"].ToString();
                lobj_Visita.fechaInicio = ldtb_Result.Rows[0]["strfechaInicio"].ToString();
                //--
                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["idHoraInicia"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.idHoraInicia= lint_obj;

                // 
                lobj_Visita.fechaFin= ldtb_Result.Rows[0]["strfechaFin"].ToString();
                //--
                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["idHoraFinaliza"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.idHoraFinaliza= lint_obj;

                // 
                //--
                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["inttotalTractos"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.totalTractos= lint_obj;

                // 

                //--
                lint_obj = 0;
                if (int.TryParse(ldtb_Result.Rows[0]["inttotalManiobras"].ToString(), out lint_obj) == false)
                    lint_obj = 0;

                lobj_Visita.totalManiobras= lint_obj;

                // 
                clsdistribucion liobjitem = new clsdistribucion();

                liobjitem.fecha = lobj_Visita.fechaInicio;
                liobjitem.idHora = lobj_Visita.idHoraInicia;
                liobjitem.noManiobras = 1;

                lobj_Visita.distribucion = new List<clsdistribucion>();

                lobj_Visita.distribucion.Add(liobjitem);

            }
            catch (Exception ext)
            {
                string lst = ext.Message;
                lst = lst;
                return "";
            }

            ///


            string jsonString;
            jsonString = System.Text.Json.JsonSerializer.Serialize(lobj_Visita);
            //request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
            request.AddParameter("application/json", jsonString, ParameterType.RequestBody);


            //  string jsonString;
            //  jsonString = System.Text.Json.JsonSerializer.Serialize(lobj_Request);

            //request.AddParameter("application/json", jsonString, ParameterType.RequestBody);

            //request.AddParameter("application/json", jsonString, ParameterType.RequestBody);

            var responseJson = client.Execute(request).Content;

            //var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson)["mensaje"].ToString();

            //List<PISTipoContendorResponse> listresult = JsonConvert.DeserializeObject<List<PISTipoContendorResponse>>(responseJson);


            //List<ShippingInfo> shippingInfo = JsonConvert.DeserializeObject<List<ShippingInfo>>(json);

            //token = token;

            //PISSecurityResponse iobj_response = new PISSecurityResponse();
            //iobj_response = new PISSecurityResponse();
            //iobj_response = JsonConvert.DeserializeObject<PISSecurityResponse>(responseJson);


            PISRegisterVResponseInfo objTiposContResponse = JsonConvert.DeserializeObject<PISRegisterVResponseInfo>(responseJson);
            try
            {
                string lstridpis = "";
                string lstrMessage = "";
                try
                {
                    objTiposContResponse = JsonConvert.DeserializeObject<PISRegisterVResponseInfo>(responseJson);

                    try
                    {
                        lstridpis = objTiposContResponse.valor.idpis.ToString();
                    }
                    catch (Exception ex)
                    { }
                    
                    lstrMessage = objTiposContResponse.mensaje;
                    lstrMessage = lstrMessage + lstridpis;
                       
                }
                catch (Exception ex)
                {
                    lstrMessage = ex.Message;
                }


                //lswref.LogPISInfo(alng_VisitId, objTiposContResponse.mensaje.ToString() + "--" + lstridpis);
                lswref.LogPISInfo(alng_VisitId, lstrMessage);
                return "";
            }
            catch(Exception ex )
            {

            }
            //  return responseJson.ToString();
            lswref.LogPISInfo(alng_VisitId, "intento llamada");
            return "";
        }


    }

    public class PISSecurityRequest
    {
        public string usuario { get; set; }
        public string password { get; set; }
        public string guid { get; set; }
        public int idAPI { get; set; }

    }

    public class PISSecurityResponse
    {
        public bool error { get; set; }
        public string mensaje { get; set; }  
        public string valor  { get; set; }
  
    }

    public class PISTipoContendorResponse
    {
        public string error { get; set; }
        public string mensaje { get; set; }
        public IList<PISTipoContenedorItem> valor { get; set; }        
    }

     public class PISTipoContenedorItem
    {
       public int id { get; set; }
       public string nombre { get; set; }
       public string nombreCorto { get; set; }
       public bool activo { get; set; }

     }

    public class clsdistribucion
    {
        public  string fecha { get; set; }
        public int idHora { get; set; }
        public int noManiobras { get; set; }
    }

    public class SolicitarCitaRequest
    {
        public string codigoCita { get; set; }
        public int   idTipoOperacion { get; set; }
        public string bl { get; set; }
        public string noPedimento { get; set; }
        public int idTipo { get; set; }
        public int idMotivo { get; set; }
        public int idRecintoOrigen { get; set; }
        public int idRecintoDestino { get; set; }
        public string imo { get; set; }
        public string nombreBuque { get; set; }
        public int idSolicitante { get; set; }
        public int idAgenteAduanal { get; set; }
        public int idTipoProducto { get; set; }
        public int idManiobra { get; set; }
        public int idTipoDespacho { get; set; }
        public string noContenedor { get; set; }
        public string idTipoContenedor { get; set; }
        public int idEstadoContenedor { get; set; }
        public string ium { get; set; }
        public string fechaInicio { get; set; }
        public int idHoraInicia { get; set; }
        public string fechaFin { get; set; }
        public int idHoraFinaliza { get; set; }
        public int totalTractos { get; set; }
        public int totalManiobras { get; set; }
        public IList<clsdistribucion> distribucion { get; set; }            

    }

    public class PISRegisterVResponse
    {
        public bool error { get; set; }
        public string mensaje { get; set; }
        public IList<ClsValor> valor { get; set; }

    }

    public class PISRegisterVResponseInfo
    {
        public bool error { get; set; }
        public string mensaje { get; set; }
        public ClsValor valor { get; set; }

    }

    public class ClsValor
    {
        public int idpis { get; set; }
    }
    //error\":false,\"mensaje\":\"\",\"valor\":{\"idpis\":\"48775\"}}"
}
