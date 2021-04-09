using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Entity
{
   public class EEmpresa: EGeneral
    {

        public EEmpresa()
        {
            UsuarioCreador = new EUsuario();
            //TipoDocumentoIdentidad = new ETipoDocumentoIdentidad();
            EUbigeo = new EUbigeo();
        }

        public string EstadoContribuyente { get; set; }
        public string CondicionDomcilio { get; set; }
        public string Ubigeo { get; set; }
        public string TipoVia { get; set; }
        public string NombreVia { get; set; }
        public string TipoZona { get; set; }
        public string NombreZona { get; set; }
        public string Numero { get; set; }
        public string Interior { get; set; }
        public string Lote { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Manzana { get; set; }
        public string Kilometro { get; set; }
        public string RUC { get; set; }
        public string RazonSocial { get; set; }
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }

        public string CelularSec { get; set; }
        public string Fecha { get; set; }
        public EUsuario UsuarioCreador { get; set; }
        //public ETipoDocumentoIdentidad TipoDocumentoIdentidad { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string DireccionTexto { get; set; }
        public string Direccion { get; set; }
        public string NroCuenta { get; set; }
        public string NombreCuenta { get; set; }
        public string Moneda { get; set; }
        public string Logo { get; set; }
        public EUbigeo EUbigeo { get; set; }
       public string PaginaWeb { get; set; }
        public string UsuarioSol { get; set; }

        public string ClaveSol { get; set; }
        public string Certificado { get; set; }
        public string ClaveCertificado { get; set; }
        public int Total { get; set; }
        public string EndPointUrl { get; set; }
    }
}
