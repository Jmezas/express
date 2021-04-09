using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service
{
    public interface IEnviarDocumentoService
    {
        RespuestaSincrono EnviarDocumento(DocumentoSunat request);
        RespuestaAsincrono EnviarResumen(DocumentoSunat request);
        RespuestaAsincrono EnviarResumenBaja(DocumentoSunat request);
        RespuestaSincrono ConsultarTicket(string numeroTicket);
    }
    public class EnviarDocumentoService : IEnviarDocumentoService
    {
        private ServiceSunatSoap.billServiceClient _proxyDocumentos;

        public EnviarDocumentoService()
        {

        }
        Binding CreateBinding()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);
            var elements = binding.CreateBindingElements();
            elements.Find<SecurityBindingElement>().IncludeTimestamp = false;
            return new CustomBinding(elements);
        }

        public void Inicializar(ParametrosConexion parametros)
        {
            System.Net.ServicePointManager.UseNagleAlgorithm = true;
            System.Net.ServicePointManager.Expect100Continue = false;
            System.Net.ServicePointManager.CheckCertificateRevocationList = true;

            _proxyDocumentos = new ServiceSunatSoap.billServiceClient(CreateBinding(), new EndpointAddress(parametros.EndPointUrl))
            {
                ClientCredentials =
                {
                    UserName =
                    {
                        UserName = parametros.Ruc + parametros.UserName,
                        Password = parametros.Password
                    }
                }
            };
        }


        public RespuestaSincrono EnviarDocumento(DocumentoSunat request)
        {

            var dataOrigen = Convert.FromBase64String(request.TramaXml);
            var response = new RespuestaSincrono();

            try
            {
                _proxyDocumentos.Open();
                var resultado = _proxyDocumentos.sendBill(request.NombreArchivo, dataOrigen, null);

                _proxyDocumentos.Close();

                response.ConstanciaDeRecepcion = Convert.ToBase64String(resultado);
                response.Exito = true;
            }
            catch (FaultException ex)
            {
                response.MensajeError = string.Concat(ex.Code.Name, ex.Message);
            }
            catch (Exception ex)
            {
                var msg = string.Concat(ex.InnerException?.Message, ex.Message);
                //if (msg.Contains(Formatos.FaultCode))
                //{
                //    var posicion = msg.IndexOf(Formatos.FaultCode, StringComparison.Ordinal);
                //    var codigoError = msg.Substring(posicion + Formatos.FaultCode.Length, 4);
                //    msg = $"El Código de Error es {codigoError}";
                //}
                response.MensajeError = msg;
            }

            return response;
        }

        public RespuestaAsincrono EnviarResumen(DocumentoSunat request)
        {

            var dataOrigen = Convert.FromBase64String(request.TramaXml);
            var response = new RespuestaAsincrono();

            try
            {
                _proxyDocumentos.Open();
                var resultado = _proxyDocumentos.sendSummary(request.NombreArchivo, dataOrigen, null);

                _proxyDocumentos.Close();

                response.NumeroTicket = resultado;
                response.Exito = true;
            }
            catch (FaultException ex)
            {
                response.MensajeError = string.Concat(ex.Code.Name, ex.Message);
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException != null ? string.Concat(ex.InnerException.Message, ex.Message) : ex.Message;
                //if (msg.Contains(Formatos.FaultCode))
                //{
                //    var posicion = msg.IndexOf(Formatos.FaultCode, StringComparison.Ordinal);
                //    var codigoError = msg.Substring(posicion + Formatos.FaultCode.Length, 4);
                //    msg = $"El Código de Error es {codigoError}";
                //}
                response.MensajeError = msg;
            }

            return response;
        }

        public RespuestaSincrono ConsultarTicket(string numeroTicket)
        {
            var response = new RespuestaSincrono();

            try
            {
                _proxyDocumentos.Open();
                var resultado = _proxyDocumentos.getStatus(numeroTicket);

                _proxyDocumentos.Close();

                var estado = (resultado.statusCode != "98");

                response.ConstanciaDeRecepcion = estado ? Convert.ToBase64String(resultado.content) : "Aun en proceso";
                response.Exito = true;
                response.NroTicketCdr = numeroTicket;
            }
            catch (FaultException ex)
            {
                response.MensajeError = string.Concat(ex.Code.Name, ex.Message);
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException != null ? string.Concat(ex.InnerException.Message, ex.Message) : ex.Message;
                //if (msg.Contains(Formatos.FaultCode))
                //{
                //    var posicion = msg.IndexOf(Formatos.FaultCode, StringComparison.Ordinal);
                //    var codigoError = msg.Substring(posicion + Formatos.FaultCode.Length, 4);
                //    msg = $"El Código de Error es {codigoError}";
                //}
                response.MensajeError = msg;
            }

            return response;

        }

        public RespuestaAsincrono EnviarResumenBaja(DocumentoSunat request)
        {
            var dataOrigen = Convert.FromBase64String(request.TramaXml);
            var response = new RespuestaAsincrono();

            try
            {
                _proxyDocumentos.Open();
                var resultado = _proxyDocumentos.sendSummary(request.NombreArchivo, dataOrigen, null);

                _proxyDocumentos.Close();

                response.NumeroTicket = resultado;
                response.Exito = true;
            }
            catch (FaultException ex)
            {
                response.MensajeError = string.Concat(ex.Code.Name, ex.Message);
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException != null ? string.Concat(ex.InnerException.Message, ex.Message) : ex.Message;
                //if (msg.Contains(Formatos.FaultCode))
                //{
                //    var posicion = msg.IndexOf(Formatos.FaultCode, StringComparison.Ordinal);
                //    var codigoError = msg.Substring(posicion + Formatos.FaultCode.Length, 4);
                //    msg = $"El Código de Error es {codigoError}";
                //}
                response.MensajeError = msg;
            }

            return response;
        }
    }
}
