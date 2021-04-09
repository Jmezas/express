using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Entity
{
  public  class EUbigeo: EGeneral
    {
        public string CodigoDepartamento { get; set; }
        public string CodigoInei { get; set; }
        public string CodigoProvincia { get; set; }
        public string CodigoDistrito { get; set; }
        public string UbicacionGeografica { get; set; }

        public int IdUbigeo { get; set; }
    }
}
