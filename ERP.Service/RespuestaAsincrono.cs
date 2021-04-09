using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service
{
    public class RespuestaAsincrono
    {
        public string NumeroTicket { get; set; }
        public bool Exito { get; set; }
        public string MensajeError { get; set; }
    }
}
