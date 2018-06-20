namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pa_vinculo_participante_multipla_escolha
    {
        public int id { get; set; }

        public int id_participante { get; set; }

        public int id_multipla_escolha { get; set; }

        public virtual pa_multipla_escolha pa_multipla_escolha { get; set; }

        public virtual pa_participante pa_participante { get; set; }
    }
}
