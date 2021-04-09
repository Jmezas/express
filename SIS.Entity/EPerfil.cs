using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Entity
{
    public class EPerfil: EGeneral
    {
        public EPerfil()
        { 
            UsuarioCreador = new EUsuario();
            Empresa = new EEmpresa();
        }

        public EEmpresaHolding EmpresaHolding { get; set; }
        public EUsuario UsuarioCreador { get; set; }
        public DateTime FechaCreacion { get; set; }
        public EEmpresa Empresa { get; set; }
        public int IdPerfil { get; set; }
        public int IdEmpresaHolding { get; set; }
        public string NombrePerfil { get; set; }
        public int IdUsuarioReg { get; set; }
        public string Usuario { get; set; }
        public string FechaHoraReg { get; set; }
        public string EstadoPerfil { get; set; }
        public int Menu { get; set; }
        public int MenuPadre { get; set; }
        public string DescripcionMenu { get; set; }
        public bool TieneAcceso { get; set; }
        public int Item { get; set; }
        public string EstadoB { get; set; }
        public int Respuesta { get; set; }
    }
}
