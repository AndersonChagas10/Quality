using System.Collections.Generic;

namespace DTO.DTO
{
    public class SyncDTO
    {
        //public List<ColetaDTO> Coleta { get; set; }
        //public List<Level1DTO> Level1 { get; set; }
        //public List<Level2DTO> Level2 { get; set; }
        //public List<Level3DTO> Level3 { get; set; }
        //public List<UserDTO> UserSgq { get; set; }
        //public List<CorrectiveActionDTO> CorrectiveAction { get; set; }
        //public ObjectConsildationDTO syncConsolidado { get; set; }

        

        public List<RootObject> Root { get; set; }

    }

    public class NextnextRoot
    {
        public string auditorid { get; set; } //Remover HTML
        public string @class { get; set; }
        public string conform { get; set; }
        public string date { get; set; }
        public string level03id { get; set; }
        public string totalerror { get; set; }
        public string value { get; set; }
        public CollectionLevel03DTO collectionLevel03DTO { get; set; }

    }

    public class NextRoot //UnidadeId Dept Id
    {
        public string auditorid { get; set; }
        public string cattletype { get; set; } //[CattleType_Id]
        public string chainspeed { get; set; }
        public string @class { get; set; }
        public string consecutivefailurelevel { get; set; } //[ConsecutiveFailureIs]
        public string consecutivefailuretotal { get; set; } //[ConsecutiveFailureTotal]
        public string date { get; set; } // Db ignore
        public string datetime { get; set; } //AddDate data que adicionou no DB
        public string defects { get; set; } //TotalDefectsLevel03
        public string evaluate { get; set; }
        public string level01id { get; set; } // [Level01_Id]
        public string level02id { get; set; } // [Level02_Id]
        public string lotnumber { get; set; }
        public string mudscore { get; set; }
        public string notavaliable { get; set; } // [NotEvaluatedIs]
        public string period { get; set; } //[Period]
        public string phase { get; set; }
        public string reaudit { get; set; }
        public string reauditnumber { get; set; }
        public string sample { get; set; }
        public string shift { get; set; }
        public string startphasedate { get; set; }
        public string unidadeid { get; set; }
        public List<NextnextRoot> nextnextRoot { get; set; }
    }

    public class RootObject
    {
        public string biasedunbiased { get; set; }
        public string @class { get; set; }
        public string completed { get; set; }
        public string completereaudit { get; set; }
        public string correctiveactioncomplete { get; set; }
        public string date { get; set; }
        public string datetime { get; set; }
        public string lastevaluate { get; set; }
        public string lastsample { get; set; }
        public string level01id { get; set; }
        public string more3defects { get; set; }
        public string period { get; set; }
        public string reaudit { get; set; }
        public string reauditnumber { get; set; }
        public string shift { get; set; }
        public string sidewitherros { get; set; }
        public string totalevaluate { get; set; }
        public string totalreaudits { get; set; }
        public string unidadeid { get; set; }
        public List<NextRoot> nextRoot { get; set; }

        public ConsolidationLevel01DTO ValidateAndCreateDtoConsolidationLevel01DTO()
        {
            return new ConsolidationLevel01DTO(this);
        }
    }

}
