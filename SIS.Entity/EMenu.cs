using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Entity
{
   public class EMenu:EGeneral
    {
        public int Orden { get; set; }
        public string Controlador { get; set; }
        public string Vista { get; set; }
        public bool TieneAcceso { get; set; }
        public EMenu Padre { get; set; }
        public List<EMenu> Hijos { get; set; }
        public string Icono { get; set; }
        public int IdPadre { get; set; }
        public string sEstado { get; set; }
    }
}
