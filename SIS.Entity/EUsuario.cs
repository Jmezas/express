using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Entity
{
    public class EUsuario
    {
        public EUsuario()
        {

            //   Ubigeo = new EUbigeo();
            //  Perfil = new EPerfil();
            TipoDocumento = new EGeneral();
            //Sucursal = new ESucursal();
        }
        public int Id { get; set; }
        public int Respuesta { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public EGeneral TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string Direccion { get; set; }
        public string FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public DateTime FechaUltimoAcceso { get; set; }
        public int TiempoVidaPassword { get; set; }
        public DateTime FechaCaducidad { get; set; }
        public bool CambiarPassword { get; set; }
        public bool PermitirCambio { get; set; }
        public bool TipoCliente { get; set; }
        public EEmpresa Empresa { get; set; }
        public EUbigeo Ubigeo { get; set; }
        public EPerfil Perfil { get; set; }
        //public ESucursal Sucursal { get; set; }
        public string Imagen { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<EMenu> Menu { get; set; }
        public string ultimafechaoperacion { get; set; }
        public string IndicadorDia { get; set; }
        public int Turno { get; set; }
        public string IndicadorTurno { get; set; }

        public string ultimafechaoperacionNC { get; set; }
        public string IndicadorDiaNC { get; set; }
        public int TurnoNC { get; set; }
        public string IndicadorTurnoNC { get; set; }
        public string TipoSistema { get; set; }
        public string AplicaPlaca { get; set; }
        public string MensajeTicket { get; set; }
        public string Estado { get; set; }
        public string RUC { get; set; }

        public string FechaNac { get; set; }
        public DateTime FechaNacNew { get; set; }
        public string NombreCompleto
        {
            get
            {
                return Nombre + (string.IsNullOrEmpty(ApellidoPaterno) ? "" : " " + ApellidoPaterno) + (string.IsNullOrEmpty(ApellidoMaterno) ? "" : " " + ApellidoMaterno);
            }
        }

        public string Nombres { get; set; }
    }
}
