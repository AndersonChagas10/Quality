using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Migrations
{
    public class IndicadoresOperacoesMonitoramentoIntilizer : DropCreateDatabaseAlways<DbContextSgq>
    {
        //protected override void Seed(DbContextSgq context)
        //{
        //    var operacoesSeed = new List<Operacao>();
        //    operacoesSeed.Add(new Operacao { Name = "HTP" , Id = 0});
        //    operacoesSeed.Add(new Operacao { Name = "Carcass Contamination Audit" , Id = 0});
        //    operacoesSeed.Add(new Operacao { Name = "CFF" , Id = 0});

        //    //foreach (Operacao r in operacoesSeed)
        //    //    context.indicadores.Add(r);

        //    var monitoramentosSeed = new List<Monitoramento>();
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Open Hide Sticker')
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Stick and Bleed' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Mark Pattern' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Cap Bung' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Remove Udder' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), '1st Legger' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), '2nd Legger' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Rumper' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Inside Butter' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Outside Butter' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Belly Ripper' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Flanker' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Rimover' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Low Backer' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Open Necks' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Gullet Raiser' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Weasand Rodder' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Bung Dropper' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Pre - gutter' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Gutter' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Clip Weasand' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 1' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 2' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 3' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 4' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 5' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 6' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 7' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 8' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 9' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 10' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 11' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 12' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 13' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 14' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 15' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 16' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 17' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Area 18' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'External Round' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Internal Round' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'External loin' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Internal loin' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'External Rib' )
        //    Insert into Monitoramento (AddDate,Name) values (getdate(), 'Internal Rib' )
        //    Insert into Monitoramento(AddDate,Name) values (getdate(), 'External chunk' )

        //    //foreach (Monitoramento r in monitoramentosSeed)
        //    //    context.Monitoramentos.Add(r);

        //    var terefasSeed = new List<Tarefa>();
        //    Insert into Tarefa (addDate, name) values (getDate(),'Specks' )
        //    Insert into Tarefa (addDate, name) values (getDate(),'Dressing' )
        //    Insert into Tarefa (addDate, name) values (getDate(),'Single Hairs' )
        //    Insert into Tarefa (addDate, name) values (getDate(),'Clusters' )
        //    Insert into Tarefa (addDate, name) values (getDate(),'Hide' )
        //    Insert into Tarefa (addDate, name) values (getDate(),'cuts' )
        //    Insert into Tarefa (addDate, name) values (getDate(),'folds' )
        //    Insert into Tarefa (addDate, name) values (getDate(),'flaps' )
        //    Insert into Tarefa (addDate, name) values (getDate(),'General Provisions' )
        //    Insert into Tarefa (addDate, name) values (getDate(),'Hide Removal/ Opening Jobs' )
        //    Insert into Tarefa (addDate, name) values (getDate(),'Air Knives' )
        //    Insert into Tarefa (addDate, name) values (getDate(),'Evisceration Knives (bungers, gullet raisers, gutters)' )

        //    //foreach (Tarefa r in terefasSeed)
        //    //    context.Tarefas.Add(r);

        //    Seed(context);
        //}
    }
}
