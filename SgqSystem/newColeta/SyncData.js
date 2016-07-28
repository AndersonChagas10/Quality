
/*
    Metodo trabalha de forma estática, porem pode ser fácilmente modificado para trabalhar de forma dinâmica. Celso Géa 28 07 2016.
*/
function CreateCCAObject() {

    var Level03ConsolidationDTO = [];
    var DataCollectionResultDTO = [];
    var Level02ConsolidationDTO = [];
    var DataCollectionDTO = [];
    var Level01ConsolidationDTO;
      
    $('.level02List .level02Group[level01id="3"] .level02').each(function (a, b) {

        var totalNotEvaluated = $('.level02List .level02Group[level01id="3"] .level02[notavaliable="notavaliable"]').length;
        var evaluationNumber = $('.level02List .level02Group[level01id="3"] .level02').length - totalNotEvaluated;

        //Cabeçalho.
        DataCollectionDTO.push({
            //Id : 0 
            Level02ConsolidationId: 0,
            EvaluationNumber: 1,
            SampleNumber: 1,
            MonitorId: 1,
            //CollectionDate : $('.App').attr('CollectionDate'),
            //AddData :
            //AlterDate :
            ProductId: 0,
            Access: 1,
            Sets: null,
            Side: null,
            Shared: false,
            Plataform: 1,
            Processed: false,
            Shift: 1, //$('.App').attr('shift'),
            Periodo: 1, //$('.App').attr('period'),
            Reaudit: 0,
            TotalNotEvaluated: totalNotEvaluated,
            TotalEvaluated: evaluationNumber,
            Control: a
        });

        Level02ConsolidationDTO.push({
            Level02Id: b.id,
            TotalEvaluated: 1,
            TotalEvaluatedShared: 1,
            TotalEvaluatedSharedWeight: 1,
            TotalEvaluatedWeight: 1,
            TotalLevel03: 5,
            TotalLevel03Weight: 5,
            TotalNotConform: b.getAttribute("defects"),
            TotalNotConformShared: b.getAttribute("defects"),
            TotalNotConformSharedWeight: b.getAttribute("defects"),
            TotalNotConformWeight: b.getAttribute("defects"),
            //DateConsolidation : ,
            Shared: false,
            Control: a
        });

        var level2Total = $('.level02List .level02Group[level01id="3"] .level02').length;
        var totalNotConformLevel2 = $('.level02List .level02Group[level01id="3"] .level02[limitexceeded="limitExceeded"]').length;

        Level01ConsolidationDTO = {
            CenterResultId: 1,
            Level01Id: $('.level01').prop("id"),
            //DateConsolidation : DateTime.Now, 
            TotalLevel02: level2Total,
            TotalLevel02Weight: level2Total,
            TotalEvaluated: 1,
            TotalEvaluatedWeight: 1,
            TotalEvaluatedShared: 1,
            TotalEvaluatedSharedWeight: 1,
            TotalNotConform: totalNotConformLevel2 > 0 ? 1 : 0,
            TotalNotConformWeight: totalNotConformLevel2 > 0 ? 1 : 0,
            TotalNotConformShared: totalNotConformLevel2 > 0 ? 1 : 0,
            TotalNotConformShared_Weight: totalNotConformLevel2 > 0 ? 1 : 0,
            Shared: false,
        }

        var level3List = [];

        level3List.push({ id: 1, defects: b.hasAttribute("level031") ? b.getAttribute("level031") : 0, notAvaliated: b.hasAttribute("notavaliable"), conform: b.hasAttribute("completed"), exceded: b.hasAttribute("limitExceeded") });
        level3List.push({ id: 2, defects: b.hasAttribute("level032") ? b.getAttribute("level032") : 0, notAvaliated: b.hasAttribute("notavaliable"), conform: b.hasAttribute("completed"), exceded: b.hasAttribute("limitExceeded") });
        level3List.push({ id: 3, defects: b.hasAttribute("level033") ? b.getAttribute("level033") : 0, notAvaliated: b.hasAttribute("notavaliable"), conform: b.hasAttribute("completed"), exceded: b.hasAttribute("limitExceeded") });
        level3List.push({ id: 4, defects: b.hasAttribute("level034") ? b.getAttribute("level034") : 0, notAvaliated: b.hasAttribute("notavaliable"), conform: b.hasAttribute("completed"), exceded: b.hasAttribute("limitExceeded") });
        level3List.push({ id: 5, defects: b.hasAttribute("level035") ? b.getAttribute("level035") : 0, notAvaliated: b.hasAttribute("notavaliable"), conform: b.hasAttribute("completed"), exceded: b.hasAttribute("limitExceeded") });

        var totalnNotAvaliatedLevel3 = $.grep(level3List, function (a) { return a.notAvaliated == true }).length;
        var totalConformLevel3 = $.grep(level3List, function (a) { return a.conform == true }).length;
        var totalNotConformLevel3 = $.grep(level3List, function (a) { return a.exceded == true }).length;
        var totalEvaluatedLevel3 = totalnNotAvaliatedLevel3 + totalConformLevel3 + totalNotConformLevel3;

        $.each(level3List, function (x, z) {

            Level03ConsolidationDTO.push({
                Id: 0,
                Level02ConsolidationId: 0,
                Level03Id: z.id,
                //DateConsolidation : null,
                //AddDate :,
                //AlterDate :,
                TotalLevel03: totalEvaluatedLevel3,
                TotalLevel03Weight: totalEvaluatedLevel3,
                TotalEvaluated: totalEvaluatedLevel3,
                totalEvaluatedWeight: totalEvaluatedLevel3,
                TotalEvaluatedShared: totalEvaluatedLevel3,
                TotalEvaluatedSharedWeight: totalEvaluatedLevel3,
                TotalNotConform: b.getAttribute("defects"),
                TotalNotConformWeight: b.getAttribute("defects"),
                TotalNotConformShared: b.getAttribute("defects"),
                TotalNotConformSharedWeight: b.getAttribute("defects"),
                Shared: true,
                Control: a
            });

            DataCollectionResultDTO.push({
                //Id :
                DataCollectionId: 0, //Aqui ja tem que tratar com RN.... se for 0 tem que criar objeto e fazer dinamica para o resto.....
                //AddDate :
                //AlterDate :
                Level03Id: z.id,
                TotalEvaluated: totalEvaluatedLevel3,
                Conformed: totalConformLevel3,
                Repeated: false,
                DataCollectionValue: z.defects,
                DataCollectionValueText: z.defects.toString(),
                Weight: 1,
                Control: a
            });

        });

    });

    return  {
        syncConsolidado: {
            level01ConsolidationDTO: Level01ConsolidationDTO,
            level02ConsolidationDTO: Level02ConsolidationDTO,
            dataCollectionDTO: DataCollectionDTO,
            level03ConsolidationDTO: Level03ConsolidationDTO,
            dataCollectionResultDTO: DataCollectionResultDTO
        }
    }
}

function ConsildateCarcass() {
   
    var enviar = CreateCCAObject();

    $.post("http://192.168.25.200/SgqMaster/api/Sync/SetDataAuditConsolidated", enviar,
    function (r) {//Callback.
        console.log(r);
    });

}