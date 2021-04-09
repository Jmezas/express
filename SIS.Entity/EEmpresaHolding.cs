using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Entity
{
    public class EEmpresaHolding
    {
        public EEmpresaHolding()
        {
            Holding = new EHolding();
            Empresa = new EEmpresa();
        }
        public int IdEmpresa { get; set; }
        public EHolding Holding { get; set; }
        public EEmpresa Empresa { get; set; }
        public bool AgenteRetencion { get; set; }
    }
}
