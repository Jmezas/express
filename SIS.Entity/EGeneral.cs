using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Entity
{
    public class EGeneral
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public string Text { get; set; }
        public string Dir { get; set; }
        public float Num { get; set; }
        public int TotalPagina { get; set; }
        public int TotalReg { get; set; }
        public string sCodigo { get; set; }
        public int Linea { get; set; }

        public class EGeneralJson<T>
        {
            public long Total { get; set; }
            public int Visualizados { get; set; }
            public List<T> Datos { get; set; }
        }

    }
    public class ETipo
    {
        public ETipo()
        {
            Empresa = new EEmpresa();
        }
        public int IdTipo { get; set; }
        public string Nombre { get; set; }

        public EEmpresa Empresa { get; set; }
    }
    public class EBarra
    {
        public int Id { get; set; }
        public string imgen { get; set; }
        public string nombre { get; set; }
        public decimal precio { get; set; }
        public string codigo { get; set; }
        public int cantidad { get; set; }
    }
    public class ETipoSerie
    {
        public int Id { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string serie { get; set; }
        public int numero { get; set; }
    }

    public class EBancos
    {
        public EBancos()
        {
           
            Empresa = new EEmpresa();
        }
    
        public int Id { get; set; }
        public string nombreBanco { get; set; }
        public string numeroCuenta { get; set; }
   
        public EEmpresa Empresa { get; set; }

    }

    public class ETipoEnvio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public float Costo { get; set; }

    }
}
