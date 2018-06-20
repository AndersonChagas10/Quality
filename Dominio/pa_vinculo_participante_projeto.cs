namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pa_vinculo_participante_projeto
    {
        public int id { get; set; }

        public int id_projeto { get; set; }

        public int id_participante { get; set; }

        public virtual pa_participante pa_participante { get; set; }

        public virtual pa_projeto pa_projeto { get; set; }
    }
}
