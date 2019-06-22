using Dominio.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dominio.Seed
{
    public class DicionarioEstaticoSeed
    {
        public void SetDicionarioEstatico()
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {

                //System.Diagnostics.Debugger.Break();

                var dicionariosKeys = db.DicionarioEstatico.Select(r => r.Key).ToList();

                var DicionariosInserir = new List<DicionarioEstatico>();


                //Acompanhamento Embarque
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "TipoVeiculo", Value = "198", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho tipo de veículo" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "Transportador", Value = "199", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Transportador" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "Placa", Value = "200", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Placa" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "NomeMotorista", Value = "201", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Nome do Motorista" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "LacreNumero", Value = "202", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Número do Lacre" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "Termografo_Id", Value = "203", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Termografo" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "SifNumber", Value = "204", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Numero do SIF" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "Pedido", Value = "205", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Numero do Pedido" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "DataCarregamento", Value = "206", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Data de Carregamento" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "Instrucao", Value = "207", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Numero da Instrução" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "NumeroNotaFiscal", Value = "208", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Numero da Nota Fiscal" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "TipoCarga", Value = "209", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Numero do Tipo de Carga" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "TipoProduto", Value = "211", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Numero do tipo de Produto" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "TipoEmbalagem", Value = "210", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Numero do tipo de embalagem" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "TemperaturaMin", Value = "212", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Temperatura Minima" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "TemperaturaMax", Value = "213", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Temperatura Maxima" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "PlanilhaRecebimentoCDs_Id", Value = "89", ControllerName = "AcompanhamentoEmbarque.cshtml", Descricao = "Id do Indicador Planilia de Recebimentos CDs" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "SIF", Value = "216", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho SIF" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "DataValidade", Value = "218", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Data de Validade" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "DataProducaoEmbarque", Value = "217", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Data de Produção" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "CB", Value = "215", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Data de CB" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "CodigoProduto", Value = "214", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Código Produto" });

                DicionariosInserir.Add(new DicionarioEstatico() { Key = "DataProducao", Value = "1197", ControllerName = "AvaliacaoSensorial.cshtml", Descricao = "Id do campo de cabeçalho Data de Produção" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "Produto", Value = "1196", ControllerName = "AvaliacaoSensorial.cshtml", Descricao = "Id do campo de cabeçalho Produto" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "IdIndicadorPesoHB", Value = "71", ControllerName = "AvaliacaoSensorial.cshtml", Descricao = "Id do campo de cabeçalho Produto" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "IdTarefaPesoHB", Value = "1378", ControllerName = "AvaliacaoSensorial.cshtml", Descricao = "Id do campo de cabeçalho Produto" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "IdCabecalhoTaraPesoHB", Value = "cb1198", ControllerName = "AvaliacaoSensorial.cshtml", Descricao = "Id do campo de cabeçalho Produto" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "IdCabecalhoQuantidadeAmostraPesoHB", Value = "cb1199", ControllerName = "AvaliacaoSensorial.cshtml", Descricao = "Id do campo de cabeçalho Produto" });

                DicionariosInserir.Add(new DicionarioEstatico() { Key = "controllerParametrizacao", Value = "ParamsV2", ControllerName = "Dashboard", Descricao = "Parametrização" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "actionParametrizacao", Value = "Index", ControllerName = "Dashboard", Descricao = "Parametrização" });

                DicionariosInserir.Add(new DicionarioEstatico() { Key = "controllerAppColeta", Value = "http://192.168.25.200/AppColeta_2_0/", ControllerName = "Dashboard", Descricao = "App Coleta" });

                DicionariosInserir.Add(new DicionarioEstatico() { Key = "controllerGestao", Value = "Gestao", ControllerName = "Dashboard", Descricao = "Gestão" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "actionGestao", Value = "Index", ControllerName = "Dashboard", Descricao = "Gestão" });

                DicionariosInserir.Add(new DicionarioEstatico() { Key = "BuildEm", Value = "DesenvolvimentoDeployServidorGrtParaTeste", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "aspnet:MaxJsonDeserializerMembers", Value = "2147483644", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "Producao", Value = "", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "URL_PA", Value = "http://192.168.25.200/PlanoAcao;http://192.168.25.200/PlanoAcao/Pa_Acao/NewFTA?", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "EnderecoEmailAlertaBR", Value = "http://localhost:57506/api/hf/SendMail", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "UsuariosComEmailBloqueado", Value = "543,546,511,545", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "UpdateTelaTabletRoot", Value = "~", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "SendMailJob", Value = "off", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "ResendProcessJsonJob", Value = "off", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "IntegrationJob", Value = "off", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "CollectionDataJob", Value = "off", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "PreencherMandala", Value = "off", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "appVersion", Value = "2.0.47", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "StorageRoot", Value = "C:\\uploadFiles", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "AppFiles", Value = "RH", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "credentialUserServerPhoto", Value = "GRT Soluções", ControllerName = "WebConfig", Descricao = "" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "credentialPassServerPhoto", Value = "1qazmko0", ControllerName = "WebConfig", Descricao = "" });

                var add = DicionariosInserir.Select(r => r.Key).Except(dicionariosKeys);

                if (add != null)
                {
                    db.DicionarioEstatico.AddRange(DicionariosInserir.Where(r => add.Contains(r.Key)));
                    db.SaveChanges();
                }

            }
        }
    }
}