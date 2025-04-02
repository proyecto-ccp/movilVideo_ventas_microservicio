using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videos.Aplicacion.Dto
{
    public class VideoIn
    {
        public string Nombre { get; set; }
        public string UrlVideo { get; set; }
        public string EstadoCarga { get; set; }
    }
}
