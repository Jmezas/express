using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Entity
{
    public class ComunicacionBaja
    {
        public List<Baja> Bajas { get; set; }
        public string IdDocumento { get; set; }
        public string FechaEmision { get; set; }
        public string FechaReferencia { get; set; }
        public string TipoDocumento { get; set; }
        public Emisor Emisor { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Baja
    {
        public string Correlativo { get; set; }
        public string MotivoBaja { get; set; }
        public int Id { get; set; }
        public string TipoDocumento { get; set; }
        public string Serie { get; set; }
    }

    public class Emisor
    {
        public string NroDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string NombreLegal { get; set; }
        public string NombreComercial { get; set; }
        public string Ubigeo { get; set; }
        public string Direccion { get; set; }
        public object Urbanizacion { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Telefono { get; set; }
    }

    public class Ecomunicacion
    {
        public Ecomunicacion()
        {
            Empresa = new EEmpresa();
        }
        public EEmpresa Empresa { get; set; }
        public string Documento { get; set; }
        public string fecha { get; set; }
        public string numero { get; set; }
        public string Serie { get; set; }
        public string codigo { get; set; }
        public string Motivo { get; set; }
    }


}
