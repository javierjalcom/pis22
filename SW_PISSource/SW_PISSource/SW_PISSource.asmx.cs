using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SW_PISSource
{
    /// <summary>
    /// Descripción breve de SW_PISSource
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class SW_PISSource : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }

        [WebMethod]
        public DataTable GetPISInfo(long alng_visitId)
        {
            DataTable ldtb_Result = new DataTable();// ' la tabla que obtiene el resultado
            OleDbDataAdapter iAdapt_comand = new OleDbDataAdapter();
            OleDbCommand iolecmd_comand = new OleDbCommand();
            OleDbConnection ioleconx_conexion = new OleDbConnection();// '' objeto de conexion que se usara para conectar
                                                                      //ADODB.conection obj = new ADODB.conection();


            string istr_conx = "";// ' cadena de conexion
            string strSQL = "";

            istr_conx = System.Configuration.ConfigurationManager.ConnectionStrings["dbCalathus"].ToString();
            ioleconx_conexion.ConnectionString = istr_conx;
            iolecmd_comand = ioleconx_conexion.CreateCommand();

            ldtb_Result = new DataTable("User");
            strSQL = "spGetPISVisitInfo";

            iolecmd_comand.Parameters.Add("intVisitId", OleDbType.Numeric);
        


            iolecmd_comand.Parameters["intVisitId"].Value = alng_visitId;
      

            iolecmd_comand.CommandText = strSQL;
            iolecmd_comand.CommandType = CommandType.StoredProcedure;
            iolecmd_comand.CommandTimeout = 99999;

            try
            {
                iAdapt_comand.SelectCommand = iolecmd_comand;
                iAdapt_comand.Fill(ldtb_Result);
            }
            catch (Exception ex)
            {
                string strError = ObtenerError(ex.Message, 99999);
                strError = ex.Message;
                return Dt_RetrieveErrorTable(strError);
            }
            finally
            {
                ioleconx_conexion.Close();
            }
            return ldtb_Result;

        }

        [WebMethod]
        public string LogPISInfo(long alng_visitId, string astr_message)
        {
            DataTable ldtb_Result = new DataTable();// ' la tabla que obtiene el resultado
            OleDbDataAdapter iAdapt_comand = new OleDbDataAdapter();
            OleDbCommand iolecmd_comand = new OleDbCommand();
            OleDbConnection ioleconx_conexion = new OleDbConnection();// '' objeto de conexion que se usara para conectar
                                                                      //ADODB.conection obj = new ADODB.conection();


            string istr_conx = "";// ' cadena de conexion
            string strSQL = "";

            istr_conx = System.Configuration.ConfigurationManager.ConnectionStrings["dbCalathus"].ToString();
            ioleconx_conexion.ConnectionString = istr_conx;
            iolecmd_comand = ioleconx_conexion.CreateCommand();

            ldtb_Result = new DataTable("User");
            strSQL = "spCRUDPISLog";

            iolecmd_comand.Parameters.Add("intMode", OleDbType.Numeric);
            iolecmd_comand.Parameters.Add("intVisitId", OleDbType.Numeric);
            iolecmd_comand.Parameters.Add("strMessage", OleDbType.VarChar);
            iolecmd_comand.Parameters.Add("strUser", OleDbType.VarChar);



            iolecmd_comand.Parameters["intMode"].Value = 1;
            iolecmd_comand.Parameters["intVisitId"].Value = alng_visitId;
            iolecmd_comand.Parameters["strMessage"].Value = astr_message;
            iolecmd_comand.Parameters["strUser"].Value = "useriis";



            iolecmd_comand.CommandText = strSQL;
            iolecmd_comand.CommandType = CommandType.StoredProcedure;
            iolecmd_comand.CommandTimeout = 99999;

            try
            {
                iAdapt_comand.SelectCommand = iolecmd_comand;
                iAdapt_comand.Fill(ldtb_Result);
            }
            catch (Exception ex)
            {
                string strError = ObtenerError(ex.Message, 99999);
                strError = ex.Message;
                //return Dt_RetrieveErrorTable(strError);
            }
            finally
            {
                ioleconx_conexion.Close();
            }
            return "";

        }



        public string ObtenerError(String cad, int ex)
        {

            if ((cad.Contains(ex.ToString()) == true) && (cad.Contains("Sybase Provider]") == true))
            {

                int idx = cad.LastIndexOf("]");
                idx = idx + 1;

                if ((idx > 0) && (idx <= cad.Length))
                    return cad.Substring(idx);
                else
                    return "";

            }
            else
            {
                if (cad.Contains("SSybase Provider]") == true)
                {
                    int idx;
                    idx = cad.LastIndexOf("]");
                    idx = idx + 1;

                    if (idx > 0 && idx <= cad.Length)
                        return cad.Substring(idx);
                    else
                        return "";
                }

            } // else if((cad.Contains(ex.ToString()) == True) &&(cad.Contains("Sybase Provider]") == True))

            return "";

        }

        public DataTable Dt_RetrieveErrorTable(string astr_Message)
        {
            DataTable ldt_ErrorTable;
            DataRow lrw_Error;

            ldt_ErrorTable = new DataTable("ErrorTable");
            ldt_ErrorTable.Columns.Add("Error", typeof(string));
            lrw_Error = ldt_ErrorTable.NewRow();

            lrw_Error["Error"] = astr_Message;
            ldt_ErrorTable.Rows.Add(lrw_Error);

            return ldt_ErrorTable;

        } //public DataTable dt_RetrieveErrorTable(string astr_Message)


    }
}
