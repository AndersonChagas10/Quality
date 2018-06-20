namespace Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Acoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Chave = c.String(),
                        Corretiva = c.String(),
                        Preventiva = c.String(),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AcoesCorretivas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Operacao = c.Int(nullable: false),
                        Tarefa = c.Int(nullable: false),
                        AcaoCorretiva = c.String(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Operacoes", t => t.Operacao)
                .ForeignKey("dbo.Tarefas", t => t.Tarefa)
                .Index(t => t.Operacao)
                .Index(t => t.Tarefa);
            
            CreateTable(
                "dbo.Operacoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                        Nivel = c.String(maxLength: 20),
                        Frequencia = c.String(maxLength: 20),
                        Vigencia = c.DateTime(storeType: "date"),
                        ControleVP = c.Boolean(),
                        ADCAMPOVAZIO = c.Boolean(),
                        AvaliarEquipamentos = c.Boolean(),
                        AvaliarCamaras = c.Boolean(),
                        Especifico = c.Boolean(),
                        ExibirPColeta = c.Boolean(),
                        PadraoPerc = c.Boolean(),
                        ControleFP = c.Boolean(),
                        AvaliarSequencial = c.Boolean(),
                        Criterio = c.String(maxLength: 100),
                        FrequenciaAlerta = c.String(maxLength: 20),
                        ExibirData = c.Boolean(),
                        EmitirAlerta = c.Boolean(),
                        ControlarAvaliacoes = c.Boolean(),
                        AlterarAmostra = c.Boolean(),
                        IncluirTarefa = c.Boolean(),
                        IncluirAvaliacao = c.Boolean(),
                        AlertaAgruparAvaliacoes = c.Boolean(),
                        QteFamiliaUnidade = c.Int(),
                        QteFamiliaCorporativa = c.Int(),
                        ExibirTarefasAcumuladas = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Alertas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Unidade = c.Int(),
                        Operacao = c.Int(),
                        Tarefa = c.Int(),
                        Mensagem = c.String(nullable: false),
                        Incidencia = c.Int(),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tarefas", t => t.Tarefa)
                .ForeignKey("dbo.Unidades", t => t.Unidade)
                .ForeignKey("dbo.Operacoes", t => t.Operacao)
                .Index(t => t.Unidade)
                .Index(t => t.Operacao)
                .Index(t => t.Tarefa);
            
            CreateTable(
                "dbo.Tarefas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Operacao = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Amostragem = c.String(nullable: false, maxLength: 20),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                        Departamento = c.Int(),
                        Frequencia = c.String(maxLength: 20),
                        Vigencia = c.DateTime(storeType: "date"),
                        Produto = c.Int(),
                        EditarAcesso = c.Boolean(),
                        ExibirAcesso = c.Boolean(),
                        FormaAmostragem = c.String(maxLength: 100),
                        AvaliarProdutos = c.Boolean(),
                        Sequencial = c.Int(),
                        InformarPesagem = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Produtos", t => t.Produto)
                .ForeignKey("dbo.Departamentos", t => t.Departamento)
                .ForeignKey("dbo.Operacoes", t => t.Operacao)
                .Index(t => t.Operacao)
                .Index(t => t.Departamento)
                .Index(t => t.Produto);
            
            CreateTable(
                "dbo.Departamentos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identificador = c.Int(),
                        Nome = c.String(nullable: false, maxLength: 100),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DepartamentoOperacoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Departamento = c.Int(nullable: false),
                        Operacao = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departamentos", t => t.Departamento)
                .ForeignKey("dbo.Operacoes", t => t.Operacao)
                .Index(t => t.Departamento)
                .Index(t => t.Operacao);
            
            CreateTable(
                "dbo.DepartamentoProdutos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Departamento = c.Int(nullable: false),
                        Produto = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Produtos", t => t.Produto)
                .ForeignKey("dbo.Departamentos", t => t.Departamento)
                .Index(t => t.Departamento)
                .Index(t => t.Produto);
            
            CreateTable(
                "dbo.Produtos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoriaProdutos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Categoria = c.Int(nullable: false),
                        Produto = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categorias", t => t.Categoria)
                .ForeignKey("dbo.Produtos", t => t.Produto)
                .Index(t => t.Categoria)
                .Index(t => t.Produto);
            
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 50),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TarefaCategorias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tarefa = c.Int(nullable: false),
                        Categoria = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categorias", t => t.Categoria)
                .ForeignKey("dbo.Tarefas", t => t.Tarefa)
                .Index(t => t.Tarefa)
                .Index(t => t.Categoria);
            
            CreateTable(
                "dbo.FamiliaProdutos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Operacao = c.Int(),
                        Ano = c.Int(nullable: false),
                        Mes = c.Int(nullable: false),
                        Unidade = c.Int(),
                        Posicao = c.Int(nullable: false),
                        Produto = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Unidades", t => t.Unidade)
                .ForeignKey("dbo.Produtos", t => t.Produto)
                .ForeignKey("dbo.Operacoes", t => t.Operacao)
                .Index(t => t.Operacao)
                .Index(t => t.Unidade)
                .Index(t => t.Produto);
            
            CreateTable(
                "dbo.Unidades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identificador = c.Int(),
                        Sigla = c.String(maxLength: 5),
                        Nome = c.String(nullable: false, maxLength: 100),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                        SIF = c.Int(),
                        Estado = c.Int(),
                        Regional = c.Int(),
                        Cluster = c.Int(),
                        Codigo = c.Int(),
                        EnderecoIP = c.String(maxLength: 50),
                        NomeDatabase = c.String(maxLength: 50),
                        Ativa = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Equipamentos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Unidade = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 100),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                        Tipo = c.String(maxLength: 100),
                        Subtipo = c.String(maxLength: 100),
                        ParCompany_Id = c.Int(),
                        ParCompanyName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Unidades", t => t.Unidade)
                .Index(t => t.Unidade);
            
            CreateTable(
                "dbo.GrupoProjeto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Sequencia = c.Int(),
                        IdEmpresa = c.Int(),
                        DataCriacao = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DataAlteracao = c.DateTime(precision: 7, storeType: "datetime2"),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Unidades", t => t.IdEmpresa)
                .Index(t => t.IdEmpresa);
            
            CreateTable(
                "dbo.Projeto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        IdGrupoProjeto = c.Int(),
                        DataCriacao = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DataAlteracao = c.DateTime(precision: 7, storeType: "datetime2"),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GrupoProjeto", t => t.IdGrupoProjeto)
                .Index(t => t.IdGrupoProjeto);
            
            CreateTable(
                "dbo.Cabecalho",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProjeto = c.Int(nullable: false),
                        IdParticipanteCriador = c.Int(),
                        DataCriacao = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DataAlteracao = c.DateTime(precision: 7, storeType: "datetime2"),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.IdParticipanteCriador)
                .ForeignKey("dbo.Projeto", t => t.IdProjeto)
                .Index(t => t.IdProjeto)
                .Index(t => t.IdParticipanteCriador);
            
            CreateTable(
                "dbo.TarefaPA",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProjeto = c.Int(nullable: false),
                        IdParticipanteCriador = c.Int(),
                        DataCriacao = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DataAlteracao = c.DateTime(precision: 7, storeType: "datetime2"),
                        Ativo = c.Boolean(nullable: false),
                        IdCabecalho = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.IdParticipanteCriador)
                .ForeignKey("dbo.Cabecalho", t => t.IdCabecalho)
                .ForeignKey("dbo.Projeto", t => t.IdProjeto)
                .Index(t => t.IdProjeto)
                .Index(t => t.IdParticipanteCriador)
                .Index(t => t.IdCabecalho);
            
            CreateTable(
                "dbo.AcompanhamentoTarefa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdTarefa = c.Int(nullable: false),
                        DataEnvio = c.DateTime(nullable: false),
                        Comentario = c.String(),
                        Enviado = c.String(),
                        Status = c.String(),
                        NomeParticipanteEnvio = c.String(),
                        IdParticipanteEnvio = c.Int(nullable: false),
                        DataCriacao = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.IdParticipanteEnvio)
                .ForeignKey("dbo.TarefaPA", t => t.IdTarefa)
                .Index(t => t.IdTarefa)
                .Index(t => t.IdParticipanteEnvio);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identificador = c.Int(),
                        Unidade = c.Int(nullable: false),
                        Usuario = c.String(nullable: false, maxLength: 50),
                        Senha = c.String(nullable: false, maxLength: 50),
                        Nome = c.String(nullable: false, maxLength: 100),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                        Funcao = c.String(maxLength: 30),
                        Email = c.String(maxLength: 100),
                        DefineFamilia = c.Boolean(),
                        DefineParametros = c.Boolean(),
                        EditaUsuarios = c.Boolean(),
                        ReceberAlerta = c.Boolean(),
                        Regional = c.Int(),
                        DataAlteracaoSenha = c.DateTime(nullable: false),
                        ConfiguraSistema = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regionais", t => t.Regional)
                .ForeignKey("dbo.Unidades", t => t.Unidade)
                .Index(t => t.Unidade)
                .Index(t => t.Regional);
            
            CreateTable(
                "dbo.NiveisUsuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Usuario = c.Int(nullable: false),
                        Nivel = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Niveis", t => t.Nivel)
                .ForeignKey("dbo.Usuarios", t => t.Usuario)
                .Index(t => t.Usuario)
                .Index(t => t.Nivel);
            
            CreateTable(
                "dbo.Niveis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identificador = c.Int(),
                        Nome = c.String(nullable: false, maxLength: 100),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlanoDeAcaoQuemQuandoComo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quem = c.String(maxLength: 100, fixedLength: true),
                        Quando = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Como = c.String(nullable: false),
                        IdUsuario = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.fa_PlanoDeAcaoQuemQuandoComo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdFormularioTratamentoAnomalia = c.Int(nullable: false),
                        IdPlanoDeAcaoQuemQuandoComo = c.Int(nullable: false),
                        IdContramedidaEspecifica = c.Int(nullable: false),
                        Versao = c.Binary(nullable: false, maxLength: 8, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlanoDeAcaoQuemQuandoComo", t => t.IdPlanoDeAcaoQuemQuandoComo)
                .Index(t => t.IdPlanoDeAcaoQuemQuandoComo);
            
            CreateTable(
                "dbo.Regionais",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UsuarioUnidades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Usuario = c.Int(nullable: false),
                        Unidade = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.Usuario)
                .ForeignKey("dbo.Unidades", t => t.Unidade)
                .Index(t => t.Usuario)
                .Index(t => t.Unidade);
            
            CreateTable(
                "dbo.VinculoCampoCabecalho",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdMultiplaEscolha = c.Int(),
                        IdCampo = c.Int(nullable: false),
                        IdCabecalho = c.Int(nullable: false),
                        Valor = c.String(unicode: false),
                        IdParticipante = c.Int(),
                        IdGrupoCabecalho = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GrupoCabecalho", t => t.IdGrupoCabecalho)
                .ForeignKey("dbo.MultiplaEscolha", t => t.IdMultiplaEscolha)
                .ForeignKey("dbo.Campo", t => t.IdCampo)
                .ForeignKey("dbo.Usuarios", t => t.IdParticipante)
                .ForeignKey("dbo.Cabecalho", t => t.IdCabecalho)
                .Index(t => t.IdMultiplaEscolha)
                .Index(t => t.IdCampo)
                .Index(t => t.IdCabecalho)
                .Index(t => t.IdParticipante)
                .Index(t => t.IdGrupoCabecalho);
            
            CreateTable(
                "dbo.Campo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, unicode: false),
                        Tipo = c.String(nullable: false, unicode: false),
                        Agrupador = c.Boolean(nullable: false),
                        Sequencia = c.Int(),
                        Obrigatorio = c.Boolean(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        IdProjeto = c.Int(nullable: false),
                        Predefinido = c.Boolean(nullable: false),
                        Modificavel = c.Boolean(nullable: false),
                        DataCriacao = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DataAlteracao = c.DateTime(precision: 7, storeType: "datetime2"),
                        IdCampoPai = c.Int(),
                        FixadoEsquerda = c.Boolean(),
                        ExibirTabela = c.Boolean(),
                        Cabecalho = c.Boolean(),
                        IdGrupoCabecalho = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GrupoCabecalho", t => t.IdGrupoCabecalho)
                .ForeignKey("dbo.Projeto", t => t.IdProjeto)
                .Index(t => t.IdProjeto)
                .Index(t => t.IdGrupoCabecalho);
            
            CreateTable(
                "dbo.GrupoCabecalho",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        IdGrupoCabecalhoPai = c.Int(),
                        IdProjeto = c.Int(),
                        DataCriacao = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DataAlteracao = c.DateTime(precision: 7, storeType: "datetime2"),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projeto", t => t.IdProjeto)
                .Index(t => t.IdProjeto);
            
            CreateTable(
                "dbo.MultiplaEscolha",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdCampo = c.Int(nullable: false),
                        Nome = c.String(),
                        IdTabelaExterna = c.Int(),
                        Cor = c.String(),
                        NomeTabelaExterna = c.String(),
                        DataCriacao = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DataAlteracao = c.DateTime(precision: 7, storeType: "datetime2"),
                        Ativo = c.Boolean(nullable: false),
                        IdMultiplaEscolhaPai = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campo", t => t.IdCampo)
                .Index(t => t.IdCampo);
            
            CreateTable(
                "dbo.VinculoCampoTarefa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdMultiplaEscolha = c.Int(),
                        IdCampo = c.Int(nullable: false),
                        IdTarefa = c.Int(nullable: false),
                        Valor = c.String(unicode: false),
                        IdParticipante = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MultiplaEscolha", t => t.IdMultiplaEscolha)
                .ForeignKey("dbo.Campo", t => t.IdCampo)
                .ForeignKey("dbo.Usuarios", t => t.IdParticipante)
                .ForeignKey("dbo.TarefaPA", t => t.IdTarefa)
                .Index(t => t.IdMultiplaEscolha)
                .Index(t => t.IdCampo)
                .Index(t => t.IdTarefa)
                .Index(t => t.IdParticipante);
            
            CreateTable(
                "dbo.VinculoParticipanteMultiplaEscolha",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdParticipante = c.Int(nullable: false),
                        IdMultiplaEscolha = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MultiplaEscolha", t => t.IdMultiplaEscolha)
                .ForeignKey("dbo.Usuarios", t => t.IdParticipante)
                .Index(t => t.IdParticipante)
                .Index(t => t.IdMultiplaEscolha);
            
            CreateTable(
                "dbo.VinculoParticipanteProjeto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProjeto = c.Int(nullable: false),
                        IdParticipante = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.IdParticipante)
                .ForeignKey("dbo.Projeto", t => t.IdProjeto)
                .Index(t => t.IdProjeto)
                .Index(t => t.IdParticipante);
            
            CreateTable(
                "dbo.Horarios",
                c => new
                    {
                        HorarioId = c.Int(nullable: false, identity: true),
                        UnidadeId = c.Int(),
                        OperacaoId = c.Int(nullable: false),
                        Hora = c.Time(nullable: false, precision: 7),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.HorarioId)
                .ForeignKey("dbo.Unidades", t => t.UnidadeId)
                .ForeignKey("dbo.Operacoes", t => t.OperacaoId)
                .Index(t => t.UnidadeId)
                .Index(t => t.OperacaoId);
            
            CreateTable(
                "dbo.MonitoramentosConcorrentes",
                c => new
                    {
                        MonitoramentoConcorrenteId = c.Int(nullable: false, identity: true),
                        UnidadeId = c.Int(),
                        OperacaoId = c.Int(nullable: false),
                        TarefaId = c.Int(nullable: false),
                        MonitoramentoId = c.Int(nullable: false),
                        ConcorrenteId = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.MonitoramentoConcorrenteId)
                .ForeignKey("dbo.Monitoramentos", t => t.ConcorrenteId)
                .ForeignKey("dbo.Monitoramentos", t => t.MonitoramentoId)
                .ForeignKey("dbo.Unidades", t => t.UnidadeId)
                .ForeignKey("dbo.Tarefas", t => t.TarefaId)
                .ForeignKey("dbo.Operacoes", t => t.OperacaoId)
                .Index(t => t.UnidadeId)
                .Index(t => t.OperacaoId)
                .Index(t => t.TarefaId)
                .Index(t => t.MonitoramentoId)
                .Index(t => t.ConcorrenteId);
            
            CreateTable(
                "dbo.Monitoramentos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 300),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                        Frequencia = c.String(maxLength: 20),
                        Vigencia = c.DateTime(storeType: "date"),
                        SiglaContusao = c.String(maxLength: 5),
                        SiglaFalhaOperacional = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GrupoTipoAvaliacaoMonitoramentos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GrupoTipoAvaliacao = c.Int(nullable: false),
                        Monitoramento = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GrupoTipoAvaliacoes", t => t.GrupoTipoAvaliacao)
                .ForeignKey("dbo.Monitoramentos", t => t.Monitoramento)
                .Index(t => t.GrupoTipoAvaliacao)
                .Index(t => t.Monitoramento);
            
            CreateTable(
                "dbo.GrupoTipoAvaliacoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Positivo = c.Int(nullable: false),
                        Negativo = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipoAvaliacoes", t => t.Negativo)
                .ForeignKey("dbo.TipoAvaliacoes", t => t.Positivo)
                .Index(t => t.Positivo)
                .Index(t => t.Negativo);
            
            CreateTable(
                "dbo.TipoAvaliacoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 100),
                        Valor = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MonitoramentoEquipamentos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Monitoramento = c.Int(nullable: false),
                        Subtipo = c.String(nullable: false, maxLength: 100),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Monitoramentos", t => t.Monitoramento)
                .Index(t => t.Monitoramento);
            
            CreateTable(
                "dbo.PadraoMonitoramentos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Padrao = c.Int(nullable: false),
                        Monitoramento = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                        Unidade = c.Int(),
                        UnidadeMedida = c.Int(),
                        UnidadeMedidaLegenda = c.Int(),
                        Tipo = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Padroes", t => t.Padrao)
                .ForeignKey("dbo.UnidadesMedidas", t => t.UnidadeMedidaLegenda)
                .ForeignKey("dbo.UnidadesMedidas", t => t.UnidadeMedida)
                .ForeignKey("dbo.Monitoramentos", t => t.Monitoramento)
                .ForeignKey("dbo.Unidades", t => t.Unidade)
                .Index(t => t.Padrao)
                .Index(t => t.Monitoramento)
                .Index(t => t.Unidade)
                .Index(t => t.UnidadeMedida)
                .Index(t => t.UnidadeMedidaLegenda);
            
            CreateTable(
                "dbo.Padroes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Minimo = c.String(maxLength: 20),
                        Maximo = c.String(maxLength: 20),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PadraoTolerancias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Monitoramento = c.Int(nullable: false),
                        PadraoNivel1 = c.Int(),
                        PadraoNivel3 = c.Int(),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Padroes", t => t.PadraoNivel1)
                .ForeignKey("dbo.Padroes", t => t.PadraoNivel3)
                .ForeignKey("dbo.Monitoramentos", t => t.Monitoramento)
                .Index(t => t.Monitoramento)
                .Index(t => t.PadraoNivel1)
                .Index(t => t.PadraoNivel3);
            
            CreateTable(
                "dbo.UnidadesMedidas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sigla = c.String(nullable: false, maxLength: 20),
                        Descricao = c.String(nullable: false, maxLength: 100),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TarefaMonitoramentos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tarefa = c.Int(nullable: false),
                        Monitoramento = c.Int(nullable: false),
                        Sequencia = c.Int(nullable: false),
                        Peso = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                        Unidade = c.Int(),
                        NaoAvaliado = c.Boolean(),
                        ExibirReincidencia = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Monitoramentos", t => t.Monitoramento)
                .ForeignKey("dbo.Unidades", t => t.Unidade)
                .ForeignKey("dbo.Tarefas", t => t.Tarefa)
                .Index(t => t.Tarefa)
                .Index(t => t.Monitoramento)
                .Index(t => t.Unidade);
            
            CreateTable(
                "dbo.VerificacaoTipificacaoTarefaIntegracao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TarefaId = c.Int(nullable: false),
                        CaracteristicaTipificacaoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Monitoramentos", t => t.TarefaId)
                .Index(t => t.TarefaId);
            
            CreateTable(
                "dbo.TarefaAmostras",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tarefa = c.Int(nullable: false),
                        Amostras = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                        Unidade = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Unidades", t => t.Unidade)
                .ForeignKey("dbo.Tarefas", t => t.Tarefa)
                .Index(t => t.Tarefa)
                .Index(t => t.Unidade);
            
            CreateTable(
                "dbo.TarefaAvaliacoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Departamento = c.Int(nullable: false),
                        Operacao = c.Int(nullable: false),
                        Tarefa = c.Int(nullable: false),
                        Avaliacao = c.Int(nullable: false),
                        Acesso = c.String(nullable: false, maxLength: 10),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                        Unidade = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Unidades", t => t.Unidade)
                .ForeignKey("dbo.Departamentos", t => t.Departamento)
                .ForeignKey("dbo.Tarefas", t => t.Tarefa)
                .ForeignKey("dbo.Operacoes", t => t.Operacao)
                .Index(t => t.Departamento)
                .Index(t => t.Operacao)
                .Index(t => t.Tarefa)
                .Index(t => t.Unidade);
            
            CreateTable(
                "dbo.TipificacaoReal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Operacao = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        Unidade = c.Int(nullable: false),
                        PercReal = c.Decimal(nullable: false, precision: 5, scale: 2),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Unidades", t => t.Unidade)
                .ForeignKey("dbo.Operacoes", t => t.Operacao)
                .Index(t => t.Operacao)
                .Index(t => t.Unidade);
            
            CreateTable(
                "dbo.VerificacaoTipificacao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sequencial = c.Int(nullable: false),
                        Banda = c.Byte(nullable: false),
                        DataHora = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UnidadeId = c.Int(nullable: false),
                        Chave = c.String(unicode: false),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Unidades", t => t.UnidadeId)
                .Index(t => t.UnidadeId);
            
            CreateTable(
                "dbo.VolumeProducao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Operacao = c.Int(nullable: false),
                        Unidade = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        Departamento = c.Int(nullable: false),
                        Volume = c.Int(),
                        Quartos = c.Int(),
                        Meta = c.Decimal(precision: 5, scale: 2),
                        ToleranciaDia = c.Decimal(precision: 11, scale: 8),
                        Nivel1 = c.Decimal(precision: 11, scale: 8),
                        Nivel2 = c.Decimal(precision: 11, scale: 8),
                        Nivel3 = c.Decimal(precision: 11, scale: 8),
                        Quantidade = c.Int(),
                        HorasTrabalho = c.Int(),
                        QuantidadeMediaHora = c.Int(),
                        NivelGeralInspecao = c.Int(),
                        AvaliacaoHora = c.Int(),
                        QtdColabOuEsteiras = c.Int(),
                        QuantidadeAvaliacao = c.Int(),
                        TamanhoAmostra = c.Int(),
                        AmostraAvaliacao = c.Int(),
                        AmostraDia = c.Int(),
                        QtdFamiliaProdutos = c.Int(),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Unidades", t => t.Unidade)
                .ForeignKey("dbo.Departamentos", t => t.Departamento)
                .ForeignKey("dbo.Operacoes", t => t.Operacao)
                .Index(t => t.Operacao)
                .Index(t => t.Unidade)
                .Index(t => t.Departamento);
            
            CreateTable(
                "dbo.ObservacoesPadroes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Departamento = c.Int(nullable: false),
                        Operacao = c.Int(nullable: false),
                        Tarefa = c.Int(nullable: false),
                        Observacao = c.String(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departamentos", t => t.Departamento)
                .ForeignKey("dbo.Tarefas", t => t.Tarefa)
                .ForeignKey("dbo.Operacoes", t => t.Operacao)
                .Index(t => t.Departamento)
                .Index(t => t.Operacao)
                .Index(t => t.Tarefa);
            
            CreateTable(
                "dbo.Metas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Operacao = c.Int(nullable: false),
                        Meta = c.Decimal(nullable: false, precision: 5, scale: 2),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Operacoes", t => t.Operacao)
                .Index(t => t.Operacao);
            
            CreateTable(
                "dbo.PacotesOperacoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pacote = c.Int(),
                        Operacao = c.Int(),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pacotes", t => t.Pacote)
                .ForeignKey("dbo.Operacoes", t => t.Operacao)
                .Index(t => t.Pacote)
                .Index(t => t.Operacao);
            
            CreateTable(
                "dbo.Pacotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 100),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pontos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cluster = c.Int(nullable: false),
                        Operacao = c.Int(nullable: false),
                        Pontos = c.Decimal(nullable: false, precision: 5, scale: 2),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                        Nivel = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clusters", t => t.Cluster)
                .ForeignKey("dbo.Operacoes", t => t.Operacao)
                .Index(t => t.Cluster)
                .Index(t => t.Operacao);
            
            CreateTable(
                "dbo.Clusters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sigla = c.Int(nullable: false),
                        Legenda = c.String(nullable: false, maxLength: 100),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AcoesPreventivas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        Departamento = c.Int(nullable: false),
                        Operacao = c.Int(nullable: false),
                        Tarefa = c.Int(nullable: false),
                        Avaliacao = c.Int(nullable: false),
                        Amostra = c.Int(nullable: false),
                        Motivo = c.String(nullable: false, maxLength: 20),
                        AcaoPreventiva = c.String(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AggregatedCounter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 100),
                        Value = c.Long(nullable: false),
                        ExpireAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "HangFire.AggregatedCounter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 100),
                        Value = c.Long(nullable: false),
                        ExpireAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AreasParticipantes",
                c => new
                    {
                        nCdCaracteristica = c.Decimal(nullable: false, precision: 12, scale: 0),
                        cNmCaracteristica = c.String(nullable: false, maxLength: 50, unicode: false),
                        cNrCaracteristica = c.String(nullable: false, maxLength: 10, unicode: false),
                        cSgCaracteristica = c.String(nullable: false, maxLength: 3, unicode: false),
                        cIdentificador = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.nCdCaracteristica);
            
            CreateTable(
                "dbo.BkpCollection",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Shift = c.Int(nullable: false),
                        Period = c.Int(nullable: false),
                        Html = c.String(nullable: false),
                        Error = c.String(),
                        Json = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CACHORRO",
                c => new
                    {
                        name = c.String(nullable: false, maxLength: 128),
                        database_id = c.Int(nullable: false),
                        create_date = c.DateTime(nullable: false),
                        compatibility_level = c.Byte(nullable: false),
                        is_auto_close_on = c.Boolean(nullable: false),
                        is_master_key_encrypted_by_server = c.Boolean(nullable: false),
                        is_published = c.Boolean(nullable: false),
                        is_subscribed = c.Boolean(nullable: false),
                        is_merge_published = c.Boolean(nullable: false),
                        is_distributor = c.Boolean(nullable: false),
                        is_sync_with_backup = c.Boolean(nullable: false),
                        service_broker_guid = c.Guid(nullable: false),
                        is_broker_enabled = c.Boolean(nullable: false),
                        is_date_correlation_on = c.Boolean(nullable: false),
                        is_cdc_enabled = c.Boolean(nullable: false),
                        source_database_id = c.Int(),
                        owner_sid = c.Binary(maxLength: 85),
                        collation_name = c.String(maxLength: 128),
                        user_access = c.Byte(),
                        user_access_desc = c.String(maxLength: 60),
                        is_read_only = c.Boolean(),
                        is_auto_shrink_on = c.Boolean(),
                        state = c.Byte(),
                        state_desc = c.String(maxLength: 60),
                        is_in_standby = c.Boolean(),
                        is_cleanly_shutdown = c.Boolean(),
                        is_supplemental_logging_enabled = c.Boolean(),
                        snapshot_isolation_state = c.Byte(),
                        snapshot_isolation_state_desc = c.String(maxLength: 60),
                        is_read_committed_snapshot_on = c.Boolean(),
                        recovery_model = c.Byte(),
                        recovery_model_desc = c.String(maxLength: 60),
                        page_verify_option = c.Byte(),
                        page_verify_option_desc = c.String(maxLength: 60),
                        is_auto_create_stats_on = c.Boolean(),
                        is_auto_create_stats_incremental_on = c.Boolean(),
                        is_auto_update_stats_on = c.Boolean(),
                        is_auto_update_stats_async_on = c.Boolean(),
                        is_ansi_null_default_on = c.Boolean(),
                        is_ansi_nulls_on = c.Boolean(),
                        is_ansi_padding_on = c.Boolean(),
                        is_ansi_warnings_on = c.Boolean(),
                        is_arithabort_on = c.Boolean(),
                        is_concat_null_yields_null_on = c.Boolean(),
                        is_numeric_roundabort_on = c.Boolean(),
                        is_quoted_identifier_on = c.Boolean(),
                        is_recursive_triggers_on = c.Boolean(),
                        is_cursor_close_on_commit_on = c.Boolean(),
                        is_local_cursor_default = c.Boolean(),
                        is_fulltext_enabled = c.Boolean(),
                        is_trustworthy_on = c.Boolean(),
                        is_db_chaining_on = c.Boolean(),
                        is_parameterization_forced = c.Boolean(),
                        is_query_store_on = c.Boolean(),
                        log_reuse_wait = c.Byte(),
                        log_reuse_wait_desc = c.String(maxLength: 60),
                        is_encrypted = c.Boolean(),
                        is_honor_broker_priority_on = c.Boolean(),
                        replica_id = c.Guid(),
                        group_database_id = c.Guid(),
                        resource_pool_id = c.Int(),
                        default_language_lcid = c.Short(),
                        default_language_name = c.String(maxLength: 128),
                        default_fulltext_language_lcid = c.Int(),
                        default_fulltext_language_name = c.String(maxLength: 128),
                        is_nested_triggers_on = c.Boolean(),
                        is_transform_noise_words_on = c.Boolean(),
                        two_digit_year_cutoff = c.Short(),
                        containment = c.Byte(),
                        containment_desc = c.String(maxLength: 60),
                        target_recovery_time_in_seconds = c.Int(),
                        delayed_durability = c.Int(),
                        delayed_durability_desc = c.String(maxLength: 60),
                        is_memory_optimized_elevate_to_snapshot_on = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.name, t.database_id, t.create_date, t.compatibility_level, t.is_auto_close_on, t.is_master_key_encrypted_by_server, t.is_published, t.is_subscribed, t.is_merge_published, t.is_distributor, t.is_sync_with_backup, t.service_broker_guid, t.is_broker_enabled, t.is_date_correlation_on, t.is_cdc_enabled });
            
            CreateTable(
                "dbo.CaracteristicaTipificacao",
                c => new
                    {
                        nCdCaracteristica = c.Decimal(nullable: false, precision: 10, scale: 0),
                        cNmCaracteristica = c.String(nullable: false, maxLength: 50, unicode: false),
                        cNrCaracteristica = c.String(nullable: false, maxLength: 10, unicode: false),
                        cSgCaracteristica = c.String(nullable: false, maxLength: 3, unicode: false),
                        cIdentificador = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.nCdCaracteristica);
            
            CreateTable(
                "dbo.CaracteristicaTipificacaoSequencial",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Sequencial = c.Int(nullable: false),
                        nCdCaracteristica_Id = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CaracteristicaTipificacao", t => t.nCdCaracteristica_Id)
                .Index(t => t.nCdCaracteristica_Id);
            
            CreateTable(
                "dbo.CausaEspecifica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 500, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.fa_CausaEspecifica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdFormularioTratamentoAnomalia = c.Int(nullable: false),
                        IdCausaEspecifica = c.Int(nullable: false),
                        Versao = c.Binary(nullable: false, maxLength: 8, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CausaEspecifica", t => t.IdCausaEspecifica)
                .Index(t => t.IdCausaEspecifica);
            
            CreateTable(
                "dbo.CausaGenerica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CausaGenerica = c.String(maxLength: 100),
                        GrupoCausa = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.fa_CausaGenerica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdFormularioTratamentoAnomalia = c.Int(nullable: false),
                        IdCausaGenerica = c.Int(nullable: false),
                        Versao = c.Binary(nullable: false, maxLength: 8, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CausaGenerica", t => t.IdCausaGenerica)
                .Index(t => t.IdCausaGenerica);
            
            CreateTable(
                "dbo.Classificacao",
                c => new
                    {
                        nCdClassificacao = c.Decimal(nullable: false, precision: 10, scale: 0),
                        cNmClassificacao = c.String(nullable: false, maxLength: 50, unicode: false),
                        cNrClassificacao = c.String(nullable: false, maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.nCdClassificacao);
            
            CreateTable(
                "dbo.ClassificacaoProduto",
                c => new
                    {
                        nCdProduto = c.Decimal(nullable: false, precision: 10, scale: 0),
                        nCdClassificacao = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.nCdProduto, t.nCdClassificacao });
            
            CreateTable(
                "dbo.ClusterDepartamentos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cluster = c.String(maxLength: 100),
                        Departamento = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CollectionHtml",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Html = c.String(nullable: false),
                        Period = c.Int(nullable: false),
                        Shift = c.Int(nullable: false),
                        CollectionDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UnitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CollectionJson",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Unit_Id = c.Int(),
                        Shift = c.Int(),
                        Period = c.Int(),
                        level01_Id = c.Int(),
                        Level01CollectionDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        level02_Id = c.Int(),
                        Evaluate = c.Int(),
                        Sample = c.Int(),
                        AuditorId = c.Int(),
                        Level02CollectionDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Level02HeaderJson = c.String(unicode: false, storeType: "text"),
                        Level03ResultJSon = c.String(nullable: false, unicode: false, storeType: "text"),
                        CorrectiveActionJson = c.String(unicode: false, storeType: "text"),
                        Reaudit = c.Boolean(nullable: false),
                        ReauditNumber = c.Int(),
                        haveReaudit = c.Boolean(),
                        haveCorrectiveAction = c.Boolean(),
                        Device_Id = c.String(),
                        AppVersion = c.String(),
                        Ambient = c.String(maxLength: 50),
                        IsProcessed = c.Boolean(nullable: false),
                        Device_Mac = c.String(),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Key = c.String(nullable: false),
                        TTP = c.String(),
                        ReauditLevel = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CollectionLevel2XCollectionJson",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CollectionLevel2_Id = c.Int(nullable: false),
                        CollectionJson_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CollectionLevel2", t => t.CollectionLevel2_Id)
                .ForeignKey("dbo.CollectionJson", t => t.CollectionJson_Id)
                .Index(t => t.CollectionLevel2_Id)
                .Index(t => t.CollectionJson_Id);
            
            CreateTable(
                "dbo.CollectionLevel2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConsolidationLevel2_Id = c.Int(nullable: false),
                        ParLevel1_Id = c.Int(nullable: false),
                        ParLevel2_Id = c.Int(nullable: false),
                        UnitId = c.Int(nullable: false),
                        AuditorId = c.Int(nullable: false),
                        Shift = c.Int(nullable: false),
                        Period = c.Int(nullable: false),
                        Phase = c.Int(nullable: false),
                        ReauditIs = c.Boolean(nullable: false),
                        ReauditNumber = c.Int(nullable: false),
                        CollectionDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        StartPhaseDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EvaluationNumber = c.Int(),
                        Sample = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ConsecutiveFailureIs = c.Boolean(nullable: false),
                        ConsecutiveFailureTotal = c.Int(nullable: false),
                        NotEvaluatedIs = c.Boolean(nullable: false),
                        Duplicated = c.Boolean(nullable: false),
                        HaveCorrectiveAction = c.Boolean(),
                        HaveReaudit = c.Boolean(),
                        HavePhase = c.Boolean(),
                        Completed = c.Boolean(),
                        ParFrequency_Id = c.Int(),
                        AlertLevel = c.Int(),
                        Sequential = c.Int(),
                        Side = c.Int(),
                        WeiEvaluation = c.Decimal(precision: 38, scale: 10),
                        Defects = c.Decimal(precision: 38, scale: 10),
                        WeiDefects = c.Decimal(precision: 15, scale: 5),
                        TotalLevel3WithDefects = c.Int(),
                        TotalLevel3Evaluation = c.Int(),
                        LastEvaluationAlert = c.Int(),
                        EvaluatedResult = c.Int(),
                        DefectsResult = c.Int(),
                        IsEmptyLevel3 = c.Boolean(),
                        Key = c.String(maxLength: 50),
                        LastLevel2Alert = c.Int(),
                        ReauditLevel = c.Int(),
                        StartPhaseEvaluation = c.Int(),
                        CounterDonePhase = c.Int(),
                        EndPhaseEvaluation = c.Int(),
                        CollectionLevel22_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CollectionLevel2", t => t.CollectionLevel22_Id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .ForeignKey("dbo.ConsolidationLevel2", t => t.ConsolidationLevel2_Id)
                .ForeignKey("dbo.ParLevel2", t => t.ParLevel2_Id)
                .ForeignKey("dbo.UserSgq", t => t.AuditorId)
                .Index(t => t.ConsolidationLevel2_Id)
                .Index(t => t.ParLevel1_Id)
                .Index(t => t.ParLevel2_Id)
                .Index(t => t.AuditorId)
                .Index(t => t.CollectionLevel22_Id);
            
            CreateTable(
                "dbo.CollectionLevel2XParHeaderField",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CollectionLevel2_Id = c.Int(nullable: false),
                        ParHeaderField_Id = c.Int(nullable: false),
                        ParHeaderField_Name = c.String(nullable: false),
                        ParFieldType_Id = c.Int(nullable: false),
                        Value = c.String(nullable: false),
                        Evaluation = c.Int(),
                        Sample = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParFieldType", t => t.ParFieldType_Id)
                .ForeignKey("dbo.ParHeaderField", t => t.ParHeaderField_Id)
                .ForeignKey("dbo.CollectionLevel2", t => t.CollectionLevel2_Id)
                .Index(t => t.CollectionLevel2_Id)
                .Index(t => t.ParHeaderField_Id)
                .Index(t => t.ParFieldType_Id);
            
            CreateTable(
                "dbo.ParFieldType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParHeaderField",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParFieldType_Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        ParLevelDefinition_Id = c.Int(nullable: false),
                        LinkNumberEvaluetion = c.Boolean(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(),
                        duplicate = c.Boolean(nullable: false),
                        CheckBox = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParLevelDefiniton", t => t.ParLevelDefinition_Id)
                .ForeignKey("dbo.ParFieldType", t => t.ParFieldType_Id)
                .Index(t => t.ParFieldType_Id)
                .Index(t => t.ParLevelDefinition_Id);
            
            CreateTable(
                "dbo.ParLevel1XHeaderField",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParLevel1_Id = c.Int(nullable: false),
                        ParHeaderField_Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(),
                        DefaultSelected = c.Boolean(),
                        HeaderFieldGroup = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .ForeignKey("dbo.ParHeaderField", t => t.ParHeaderField_Id)
                .Index(t => t.ParLevel1_Id)
                .Index(t => t.ParHeaderField_Id);
            
            CreateTable(
                "dbo.ParLevel1",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParConsolidationType_Id = c.Int(nullable: false),
                        ParFrequency_Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        HasSaveLevel2 = c.Boolean(nullable: false),
                        HasNoApplicableLevel2 = c.Boolean(nullable: false),
                        HasGroupLevel2 = c.Boolean(nullable: false),
                        HasAlert = c.Boolean(nullable: false),
                        IsSpecific = c.Boolean(),
                        IsSpecificHeaderField = c.Boolean(nullable: false),
                        IsSpecificNumberEvaluetion = c.Boolean(nullable: false),
                        IsSpecificNumberSample = c.Boolean(nullable: false),
                        IsSpecificLevel3 = c.Boolean(nullable: false),
                        IsSpecificGoal = c.Boolean(),
                        IsRuleConformity = c.Boolean(nullable: false),
                        IsFixedEvaluetionNumber = c.Boolean(nullable: false),
                        IsLimitedEvaluetionNumber = c.Boolean(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        Level2Number = c.Int(),
                        hashKey = c.Int(),
                        haveRealTimeConsolidation = c.Boolean(),
                        RealTimeConsolitationUpdate = c.Int(),
                        IsPartialSave = c.Boolean(),
                        HasCompleteEvaluation = c.Boolean(),
                        ParScoreType_Id = c.Int(),
                        IsChildren = c.Boolean(),
                        ParLevel1Origin_Id = c.Int(),
                        PointsDestiny = c.Boolean(),
                        ParLevel1Destiny_Id = c.Int(),
                        EditLevel2 = c.Boolean(nullable: false),
                        HasTakePhoto = c.Boolean(nullable: false),
                        AllowAddLevel3 = c.Boolean(),
                        AllowEditPatternLevel3Task = c.Boolean(),
                        AllowEditWeightOnLevel3 = c.Boolean(),
                        IsRecravacao = c.Boolean(),
                        ParGroupLevel1_Id = c.Int(),
                        ShowInTablet = c.Boolean(),
                        ShowScorecard = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParFrequency", t => t.ParFrequency_Id)
                .ForeignKey("dbo.ParConsolidationType", t => t.ParConsolidationType_Id)
                .ForeignKey("dbo.ParScoreType", t => t.ParScoreType_Id)
                .Index(t => t.ParConsolidationType_Id)
                .Index(t => t.ParFrequency_Id)
                .Index(t => t.ParScoreType_Id);
            
            CreateTable(
                "dbo.ConsolidationLevel1",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnitId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        ParLevel1_Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ConsolidationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Evaluation = c.Decimal(precision: 32, scale: 8),
                        AtualAlert = c.Int(),
                        WeiEvaluation = c.Decimal(precision: 32, scale: 8),
                        EvaluateTotal = c.Decimal(precision: 32, scale: 8),
                        DefectsTotal = c.Decimal(precision: 10, scale: 5),
                        WeiDefects = c.Decimal(precision: 30, scale: 8),
                        TotalLevel3Evaluation = c.Int(),
                        TotalLevel3WithDefects = c.Int(),
                        LastEvaluationAlert = c.Int(),
                        EvaluatedResult = c.Int(),
                        DefectsResult = c.Int(),
                        LastLevel2Alert = c.Int(),
                        ReauditIs = c.Int(),
                        ReauditNumber = c.Int(),
                        Shift = c.Int(),
                        Period = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParCompany", t => t.UnitId)
                .ForeignKey("dbo.Department", t => t.DepartmentId)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .Index(t => t.UnitId)
                .Index(t => t.DepartmentId)
                .Index(t => t.ParLevel1_Id);
            
            CreateTable(
                "dbo.ConsolidationLevel2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConsolidationLevel1_Id = c.Int(nullable: false),
                        ParLevel2_Id = c.Int(nullable: false),
                        UnitId = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlertLevel = c.Int(),
                        ConsolidationDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        WeiEvaluation = c.Decimal(precision: 10, scale: 5),
                        EvaluateTotal = c.Decimal(precision: 10, scale: 5),
                        DefectsTotal = c.Decimal(precision: 10, scale: 5),
                        WeiDefects = c.Decimal(precision: 30, scale: 8),
                        TotalLevel3Evaluation = c.Int(),
                        TotalLevel3WithDefects = c.Int(),
                        LastEvaluationAlert = c.Int(),
                        EvaluatedResult = c.Int(),
                        DefectsResult = c.Int(),
                        LastLevel2Alert = c.Int(),
                        ReauditIs = c.Int(),
                        ReauditNumber = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParLevel2", t => t.ParLevel2_Id)
                .ForeignKey("dbo.ConsolidationLevel1", t => t.ConsolidationLevel1_Id)
                .Index(t => t.ConsolidationLevel1_Id)
                .Index(t => t.ParLevel2_Id);
            
            CreateTable(
                "dbo.ParLevel2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParFrequency_Id = c.Int(nullable: false),
                        ParDepartment_Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        IsEmptyLevel3 = c.Boolean(nullable: false),
                        HasShowLevel03 = c.Boolean(nullable: false),
                        HasGroupLevel3 = c.Boolean(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        HasSampleTotal = c.Boolean(),
                        HasTakePhoto = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParFrequency", t => t.ParFrequency_Id)
                .ForeignKey("dbo.ParDepartment", t => t.ParDepartment_Id)
                .Index(t => t.ParFrequency_Id)
                .Index(t => t.ParDepartment_Id);
            
            CreateTable(
                "dbo.ParCounterXLocal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParLocal_Id = c.Int(nullable: false),
                        ParCounter_Id = c.Int(nullable: false),
                        ParLevel1_Id = c.Int(),
                        ParLevel2_Id = c.Int(),
                        ParLevel3_Id = c.Int(),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParCounter", t => t.ParCounter_Id)
                .ForeignKey("dbo.ParLevel3", t => t.ParLevel3_Id)
                .ForeignKey("dbo.ParLocal", t => t.ParLocal_Id)
                .ForeignKey("dbo.ParLevel2", t => t.ParLevel2_Id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .Index(t => t.ParLocal_Id)
                .Index(t => t.ParCounter_Id)
                .Index(t => t.ParLevel1_Id)
                .Index(t => t.ParLevel2_Id)
                .Index(t => t.ParLevel3_Id);
            
            CreateTable(
                "dbo.ParCounter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Level = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 155),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(),
                        Hashkey = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParLevel3",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 1000),
                        Description = c.String(nullable: false, maxLength: 1000),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        HasTakePhoto = c.Boolean(nullable: false),
                        IsPointLess = c.Boolean(),
                        AllowNA = c.Boolean(),
                        OrderColumn = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParLevel3EvaluationSample",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParCompany_Id = c.Int(),
                        ParLevel1_Id = c.Int(),
                        ParLevel2_Id = c.Int(),
                        ParLevel3_Id = c.Int(nullable: false),
                        SampleNumber = c.Decimal(precision: 18, scale: 0),
                        EvaluationNumber = c.Decimal(precision: 18, scale: 0),
                        EvaluationInterval = c.String(maxLength: 30, unicode: false),
                        AddDate = c.DateTime(nullable: false),
                        AlterDate = c.DateTime(),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_Id)
                .ForeignKey("dbo.ParLevel3", t => t.ParLevel3_Id)
                .ForeignKey("dbo.ParLevel2", t => t.ParLevel2_Id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .Index(t => t.ParCompany_Id)
                .Index(t => t.ParLevel1_Id)
                .Index(t => t.ParLevel2_Id)
                .Index(t => t.ParLevel3_Id);
            
            CreateTable(
                "dbo.ParCompany",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        Initials = c.String(maxLength: 50),
                        SIF = c.String(maxLength: 155),
                        CompanyNumber = c.Int(),
                        IPServer = c.String(maxLength: 155),
                        DBServer = c.String(maxLength: 155),
                        IntegrationId = c.Decimal(precision: 18, scale: 0),
                        ParCompany_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Defect",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParCompany_Id = c.Int(nullable: false),
                        ParLevel1_Id = c.Int(nullable: false),
                        Defects = c.Decimal(nullable: false, precision: 30, scale: 8),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Active = c.Boolean(nullable: false),
                        Evaluations = c.Int(),
                        CurrentEvaluation = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_Id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .Index(t => t.ParCompany_Id)
                .Index(t => t.ParLevel1_Id);
            
            CreateTable(
                "dbo.ParCompanyCluster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParCompany_Id = c.Int(nullable: false),
                        ParCluster_Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParCluster", t => t.ParCluster_Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_Id)
                .Index(t => t.ParCompany_Id)
                .Index(t => t.ParCluster_Id);
            
            CreateTable(
                "dbo.ParCluster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParClusterGroup_Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        ParClusterParent_Id = c.Int(),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParClusterGroup", t => t.ParClusterGroup_Id)
                .Index(t => t.ParClusterGroup_Id);
            
            CreateTable(
                "dbo.ParClusterGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        ParClusterGroupParent_Id = c.Int(),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParClusterXModule",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParCluster_Id = c.Int(nullable: false),
                        ParModule_Id = c.Int(nullable: false),
                        Points = c.Decimal(nullable: false, precision: 10, scale: 5),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        EffectiveDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParModule", t => t.ParModule_Id)
                .ForeignKey("dbo.ParCluster", t => t.ParCluster_Id)
                .Index(t => t.ParCluster_Id)
                .Index(t => t.ParModule_Id);
            
            CreateTable(
                "dbo.ParModule",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParModuleXModule",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParModuleParent_Id = c.Int(nullable: false),
                        ParModuleChild_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParModule", t => t.ParModuleChild_Id)
                .ForeignKey("dbo.ParModule", t => t.ParModuleParent_Id)
                .Index(t => t.ParModuleParent_Id)
                .Index(t => t.ParModuleChild_Id);
            
            CreateTable(
                "dbo.ParLevel1XCluster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParLevel1_Id = c.Int(nullable: false),
                        ParCluster_Id = c.Int(nullable: false),
                        Points = c.Decimal(nullable: false, precision: 10, scale: 5),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        ParCriticalLevel_Id = c.Int(),
                        ValidoApartirDe = c.DateTime(storeType: "date"),
                        EffectiveDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParCriticalLevel", t => t.ParCriticalLevel_Id)
                .ForeignKey("dbo.ParCluster", t => t.ParCluster_Id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .Index(t => t.ParLevel1_Id)
                .Index(t => t.ParCluster_Id)
                .Index(t => t.ParCriticalLevel_Id);
            
            CreateTable(
                "dbo.ParCriticalLevel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 155),
                        AddDate = c.DateTime(nullable: false),
                        AlterDate = c.DateTime(),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParCompanyXStructure",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParStructure_Id = c.Int(nullable: false),
                        ParCompany_Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParStructure", t => t.ParStructure_Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_Id)
                .Index(t => t.ParStructure_Id)
                .Index(t => t.ParCompany_Id);
            
            CreateTable(
                "dbo.ParStructure",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParStructureGroup_Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        ParStructureParent_Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParStructureGroup", t => t.ParStructureGroup_Id)
                .Index(t => t.ParStructureGroup_Id);
            
            CreateTable(
                "dbo.ParStructureGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        ParStructureGroupParent_Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Active = c.Boolean(nullable: false),
                        IsParentCompany = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParCompanyXUserSgq",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserSgq_Id = c.Int(nullable: false),
                        ParCompany_Id = c.Int(nullable: false),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserSgq", t => t.UserSgq_Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_Id)
                .Index(t => t.UserSgq_Id)
                .Index(t => t.ParCompany_Id);
            
            CreateTable(
                "dbo.UserSgq",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        AcessDate = c.DateTime(),
                        AddDate = c.DateTime(nullable: false),
                        AlterDate = c.DateTime(),
                        Role = c.String(),
                        FullName = c.String(nullable: false),
                        Email = c.String(),
                        Phone = c.String(),
                        ParCompany_Id = c.Int(),
                        PasswordDate = c.DateTime(),
                        UseActiveDirectory = c.Boolean(nullable: false),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CollectionLevel02",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConsolidationLevel02Id = c.Int(nullable: false),
                        Level01Id = c.Int(nullable: false),
                        Level02Id = c.Int(nullable: false),
                        UnitId = c.Int(nullable: false),
                        AuditorId = c.Int(nullable: false),
                        Shift = c.Int(nullable: false),
                        Period = c.Int(nullable: false),
                        Phase = c.Int(nullable: false),
                        ReauditIs = c.Boolean(nullable: false),
                        ReauditNumber = c.Int(nullable: false),
                        CollectionDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        StartPhaseDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EvaluationNumber = c.Int(),
                        Sample = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        CattleTypeId = c.Int(nullable: false),
                        Chainspeed = c.Decimal(nullable: false, precision: 20, scale: 5),
                        ConsecutiveFailureIs = c.Boolean(nullable: false),
                        ConsecutiveFailureTotal = c.Int(nullable: false),
                        LotNumber = c.Decimal(nullable: false, precision: 20, scale: 5),
                        Mudscore = c.Decimal(nullable: false, precision: 20, scale: 5),
                        NotEvaluatedIs = c.Boolean(nullable: false),
                        Duplicated = c.Boolean(nullable: false),
                        HaveCorrectiveAction = c.Boolean(),
                        HaveReaudit = c.Boolean(),
                        HavePhase = c.Boolean(),
                        Completed = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ConsolidationLevel02", t => t.ConsolidationLevel02Id)
                .ForeignKey("dbo.Level01", t => t.Level01Id)
                .ForeignKey("dbo.Level02", t => t.Level02Id)
                .ForeignKey("dbo.UserSgq", t => t.AuditorId)
                .Index(t => t.ConsolidationLevel02Id)
                .Index(t => t.Level01Id)
                .Index(t => t.Level02Id)
                .Index(t => t.AuditorId);
            
            CreateTable(
                "dbo.CollectionLevel03",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CollectionLevel02Id = c.Int(nullable: false),
                        Level03Id = c.Int(),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ConformedIs = c.Boolean(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 10, scale: 5),
                        ValueText = c.String(nullable: false, unicode: false),
                        Duplicated = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Level03", t => t.Level03Id)
                .ForeignKey("dbo.CollectionLevel02", t => t.CollectionLevel02Id)
                .Index(t => t.CollectionLevel02Id)
                .Index(t => t.Level03Id);
            
            CreateTable(
                "dbo.Level03",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, unicode: false),
                        Alias = c.String(maxLength: 25, unicode: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConsolidationLevel02",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Level01ConsolidationId = c.Int(nullable: false),
                        Level02Id = c.Int(nullable: false),
                        UnitId = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ConsolidationDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ConsolidationLevel01", t => t.Level01ConsolidationId)
                .ForeignKey("dbo.Level02", t => t.Level02Id)
                .Index(t => t.Level01ConsolidationId)
                .Index(t => t.Level02Id);
            
            CreateTable(
                "dbo.ConsolidationLevel01",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnitId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        Level01Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ConsolidationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Department", t => t.DepartmentId)
                .ForeignKey("dbo.Level01", t => t.Level01Id)
                .ForeignKey("dbo.Unit", t => t.UnitId)
                .Index(t => t.UnitId)
                .Index(t => t.DepartmentId)
                .Index(t => t.Level01Id);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(nullable: false),
                        AlterDate = c.DateTime(),
                        Name = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Level01",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, unicode: false),
                        Alias = c.String(maxLength: 25, unicode: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Unit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        AlterDate = c.DateTime(),
                        Number = c.Int(),
                        Code = c.String(),
                        Ip = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UnitUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserSgqId = c.Int(nullable: false),
                        UnitId = c.Int(nullable: false),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Unit", t => t.UnitId)
                .ForeignKey("dbo.UserSgq", t => t.UserSgqId)
                .Index(t => t.UserSgqId)
                .Index(t => t.UnitId);
            
            CreateTable(
                "dbo.Level02",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, unicode: false),
                        Alias = c.String(maxLength: 25, unicode: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CorrectiveAction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuditorId = c.Int(nullable: false),
                        CollectionLevel02Id = c.Int(nullable: false),
                        SlaughterId = c.Int(),
                        TechinicalId = c.Int(),
                        DateTimeSlaughter = c.DateTime(precision: 7, storeType: "datetime2"),
                        DateTimeTechinical = c.DateTime(precision: 7, storeType: "datetime2"),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        DateCorrectiveAction = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DescriptionFailure = c.String(),
                        ImmediateCorrectiveAction = c.String(),
                        ProductDisposition = c.String(),
                        PreventativeMeasure = c.String(),
                        CollectionLevel2_Id = c.Int(),
                        MailProcessed = c.Boolean(),
                        EmailContent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmailContent", t => t.EmailContent_Id)
                .ForeignKey("dbo.UserSgq", t => t.AuditorId)
                .ForeignKey("dbo.UserSgq", t => t.TechinicalId)
                .ForeignKey("dbo.UserSgq", t => t.SlaughterId)
                .ForeignKey("dbo.CollectionLevel2", t => t.CollectionLevel02Id)
                .Index(t => t.AuditorId)
                .Index(t => t.CollectionLevel02Id)
                .Index(t => t.SlaughterId)
                .Index(t => t.TechinicalId)
                .Index(t => t.EmailContent_Id);
            
            CreateTable(
                "dbo.EmailContent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        To = c.String(nullable: false, maxLength: 900),
                        Body = c.String(),
                        SendStatus = c.String(maxLength: 900),
                        SendDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Project = c.String(maxLength: 15),
                        IsBodyHtml = c.Boolean(),
                        From = c.String(maxLength: 350),
                        Subject = c.String(maxLength: 2900),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Deviation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParCompany_Id = c.Int(nullable: false),
                        ParLevel1_Id = c.Int(nullable: false),
                        ParLevel2_Id = c.Int(nullable: false),
                        Evaluation = c.Decimal(nullable: false, precision: 18, scale: 0),
                        Sample = c.Decimal(nullable: false, precision: 18, scale: 0),
                        AlertNumber = c.Int(nullable: false),
                        Defects = c.Decimal(nullable: false, precision: 10, scale: 5),
                        DeviationDate = c.DateTime(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        sendMail = c.Boolean(),
                        DeviationMessage = c.String(),
                        Status = c.String(),
                        SendDate = c.String(),
                        EmailContent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmailContent", t => t.EmailContent_Id)
                .Index(t => t.EmailContent_Id);
            
            CreateTable(
                "dbo.UserXRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(nullable: false),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RoleUserSgq", t => t.Role_Id)
                .ForeignKey("dbo.UserSgq", t => t.User_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.RoleUserSgq",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        FazColeta = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuXRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Menu_Id = c.Int(nullable: false),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Menu", t => t.Menu_Id)
                .ForeignKey("dbo.RoleUserSgq", t => t.Role_Id)
                .Index(t => t.Menu_Id)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.Menu",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Class = c.String(nullable: false, maxLength: 100),
                        Url = c.String(nullable: false, maxLength: 255),
                        Title = c.String(nullable: false, maxLength: 100),
                        GroupMenu_Id = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupMenu", t => t.GroupMenu_Id)
                .Index(t => t.GroupMenu_Id);
            
            CreateTable(
                "dbo.GroupMenu",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Class = c.String(nullable: false, maxLength: 100),
                        Url = c.String(nullable: false, maxLength: 255),
                        Title = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParEvaluation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParCompany_Id = c.Int(),
                        ParLevel2_Id = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        ParLevel1_Id = c.Int(),
                        ParCluster_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_Id)
                .ForeignKey("dbo.ParLevel2", t => t.ParLevel2_Id)
                .Index(t => t.ParCompany_Id)
                .Index(t => t.ParLevel2_Id);
            
            CreateTable(
                "dbo.ParGoal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParLevel1_Id = c.Int(nullable: false),
                        ParCompany_Id = c.Int(),
                        PercentValue = c.Decimal(nullable: false, precision: 10, scale: 5),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        EffectiveDate = c.DateTime(),
                        ValidoApartirDe = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_Id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .Index(t => t.ParLevel1_Id)
                .Index(t => t.ParCompany_Id);
            
            CreateTable(
                "dbo.ParLevel2ControlCompany",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParCompany_Id = c.Int(),
                        ParLevel1_Id = c.Int(),
                        ParLevel2_Id = c.Int(),
                        InitDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_Id)
                .ForeignKey("dbo.ParLevel2", t => t.ParLevel2_Id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .Index(t => t.ParCompany_Id)
                .Index(t => t.ParLevel1_Id)
                .Index(t => t.ParLevel2_Id);
            
            CreateTable(
                "dbo.ParLevel3Level2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParLevel2_Id = c.Int(nullable: false),
                        ParLevel3_Id = c.Int(nullable: false),
                        ParLevel3Group_Id = c.Int(),
                        Weight = c.Decimal(nullable: false, precision: 10, scale: 5),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        ParCompany_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParLevel3Group", t => t.ParLevel3Group_Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_Id)
                .ForeignKey("dbo.ParLevel3", t => t.ParLevel3_Id)
                .ForeignKey("dbo.ParLevel2", t => t.ParLevel2_Id)
                .Index(t => t.ParLevel2_Id)
                .Index(t => t.ParLevel3_Id)
                .Index(t => t.ParLevel3Group_Id)
                .Index(t => t.ParCompany_Id);
            
            CreateTable(
                "dbo.ParLevel3Group",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParLevel2_Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParLevel2", t => t.ParLevel2_Id)
                .Index(t => t.ParLevel2_Id);
            
            CreateTable(
                "dbo.ParLevel3Level2Level1",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParLevel3Level2_Id = c.Int(nullable: false),
                        ParLevel1_Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Active = c.Boolean(nullable: false),
                        ParCompany_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParLevel3Level2", t => t.ParLevel3Level2_Id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .Index(t => t.ParLevel3Level2_Id)
                .Index(t => t.ParLevel1_Id);
            
            CreateTable(
                "dbo.ParLevel3Value",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParLevel3_Id = c.Int(nullable: false),
                        ParLevel3InputType_Id = c.Int(nullable: false),
                        ParLevel3BoolFalse_Id = c.Int(),
                        ParLevel3BoolTrue_Id = c.Int(),
                        ParCompany_Id = c.Int(),
                        ParMeasurementUnit_Id = c.Int(),
                        AcceptableValueBetween = c.Boolean(),
                        IntervalMin = c.Decimal(precision: 38, scale: 10),
                        IntervalMax = c.Decimal(precision: 38, scale: 10),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        ParLevel1_Id = c.Int(),
                        ParLevel2_Id = c.Int(),
                        DynamicValue = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParLevel3BoolFalse", t => t.ParLevel3BoolFalse_Id)
                .ForeignKey("dbo.ParLevel3BoolTrue", t => t.ParLevel3BoolTrue_Id)
                .ForeignKey("dbo.ParLevel3InputType", t => t.ParLevel3InputType_Id)
                .ForeignKey("dbo.ParMeasurementUnit", t => t.ParMeasurementUnit_Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_Id)
                .ForeignKey("dbo.ParLevel3", t => t.ParLevel3_Id)
                .ForeignKey("dbo.ParLevel2", t => t.ParLevel2_Id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .Index(t => t.ParLevel3_Id)
                .Index(t => t.ParLevel3InputType_Id)
                .Index(t => t.ParLevel3BoolFalse_Id)
                .Index(t => t.ParLevel3BoolTrue_Id)
                .Index(t => t.ParCompany_Id)
                .Index(t => t.ParMeasurementUnit_Id)
                .Index(t => t.ParLevel1_Id)
                .Index(t => t.ParLevel2_Id);
            
            CreateTable(
                "dbo.ParLevel3BoolFalse",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 155),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParLevel3BoolTrue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 155),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParLevel3InputType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        Sampling = c.Decimal(precision: 10, scale: 5),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParMeasurementUnit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParMultipleValuesXParCompany",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParMultipleValues_Id = c.Int(),
                        Parent_ParMultipleValues_Id = c.Int(),
                        ParCompany_Id = c.Int(nullable: false),
                        HashKey = c.String(maxLength: 255, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        ParLevel1_Id = c.Int(nullable: false),
                        ParHeaderField_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParMultipleValues", t => t.Parent_ParMultipleValues_Id)
                .ForeignKey("dbo.ParMultipleValues", t => t.ParMultipleValues_Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_Id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .ForeignKey("dbo.ParHeaderField", t => t.ParHeaderField_Id)
                .Index(t => t.ParMultipleValues_Id)
                .Index(t => t.Parent_ParMultipleValues_Id)
                .Index(t => t.ParCompany_Id)
                .Index(t => t.ParLevel1_Id)
                .Index(t => t.ParHeaderField_Id);
            
            CreateTable(
                "dbo.ParMultipleValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParHeaderField_Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        PunishmentValue = c.Decimal(nullable: false, precision: 38, scale: 10),
                        Conformity = c.Boolean(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        IsDefaultOption = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParHeaderField", t => t.ParHeaderField_Id)
                .Index(t => t.ParHeaderField_Id);
            
            CreateTable(
                "dbo.ParNotConformityRuleXLevel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParNotConformityRule_Id = c.Int(nullable: false),
                        ParCompany_Id = c.Int(),
                        Value = c.Decimal(precision: 38, scale: 10),
                        Level = c.Int(nullable: false),
                        ParLevel1_Id = c.Int(),
                        ParLevel2_Id = c.Int(),
                        ParLevel3_Id = c.Int(),
                        IsReaudit = c.Boolean(),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParNotConformityRule", t => t.ParNotConformityRule_Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_Id)
                .ForeignKey("dbo.ParLevel3", t => t.ParLevel3_Id)
                .ForeignKey("dbo.ParLevel2", t => t.ParLevel2_Id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .Index(t => t.ParNotConformityRule_Id)
                .Index(t => t.ParCompany_Id)
                .Index(t => t.ParLevel1_Id)
                .Index(t => t.ParLevel2_Id)
                .Index(t => t.ParLevel3_Id);
            
            CreateTable(
                "dbo.ParNotConformityRule",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 155),
                        Sufix = c.String(maxLength: 50),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParSample",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParCompany_Id = c.Int(),
                        ParLevel2_Id = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        ParLevel1_Id = c.Int(),
                        ParCluster_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_Id)
                .ForeignKey("dbo.ParLevel2", t => t.ParLevel2_Id)
                .Index(t => t.ParCompany_Id)
                .Index(t => t.ParLevel2_Id);
            
            CreateTable(
                "dbo.VolumeCepDesossa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Indicador = c.Int(),
                        Unidade = c.Int(),
                        Data = c.DateTime(precision: 7, storeType: "datetime2"),
                        Departamento = c.String(maxLength: 255, unicode: false),
                        HorasTrabalhadasPorDia = c.Int(),
                        AmostraPorDia = c.Int(),
                        QtdadeFamiliaProduto = c.Int(),
                        Avaliacoes = c.Int(),
                        Amostras = c.Int(),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ParCompany_id = c.Int(),
                        ParLevel1_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_id)
                .Index(t => t.ParCompany_id)
                .Index(t => t.ParLevel1_id);
            
            CreateTable(
                "dbo.VolumeCepRecortes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Indicador = c.Int(),
                        Unidade = c.Int(),
                        Data = c.DateTime(precision: 7, storeType: "datetime2"),
                        Departamento = c.String(maxLength: 255, unicode: false),
                        HorasTrabalhadasPorDia = c.Int(),
                        QtdadeMediaKgRecProdDia = c.Int(),
                        QtdadeMediaKgRecProdHora = c.Int(),
                        NBR = c.Int(),
                        TotalKgAvaliaHoraProd = c.Int(),
                        QtadeTrabEsteiraRecortes = c.Int(),
                        TotalAvaliaColaborEsteirHoraProd = c.Int(),
                        TamanhoAmostra = c.Int(),
                        TotalAmostraAvaliaColabEsteiraHoraProd = c.Int(),
                        Avaliacoes = c.Int(),
                        Amostras = c.Int(),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ParCompany_id = c.Int(),
                        ParLevel1_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_id)
                .Index(t => t.ParCompany_id)
                .Index(t => t.ParLevel1_id);
            
            CreateTable(
                "dbo.VolumePcc1b",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Indicador = c.Int(),
                        Unidade = c.Int(),
                        Data = c.DateTime(precision: 7, storeType: "datetime2"),
                        Departamento = c.String(maxLength: 255, unicode: false),
                        VolumeAnimais = c.Int(),
                        Quartos = c.Int(),
                        Meta = c.Decimal(precision: 10, scale: 5),
                        ToleranciaDia = c.Single(),
                        Nivel11 = c.Single(),
                        Nivel12 = c.Single(),
                        Nivel13 = c.Single(),
                        Avaliacoes = c.Int(),
                        Amostras = c.Int(),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ParCompany_id = c.Int(),
                        ParLevel1_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_id)
                .Index(t => t.ParCompany_id)
                .Index(t => t.ParLevel1_id);
            
            CreateTable(
                "dbo.VolumeVacuoGRD",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Indicador = c.Int(),
                        Unidade = c.Int(),
                        Data = c.DateTime(precision: 7, storeType: "datetime2"),
                        Departamento = c.String(maxLength: 255, unicode: false),
                        HorasTrabalhadasPorDia = c.Int(),
                        AmostraPorDia = c.Int(),
                        QtdadeFamiliaProduto = c.Int(),
                        Avaliacoes = c.Int(),
                        Amostras = c.Int(),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ParCompany_id = c.Int(),
                        ParLevel1_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParCompany", t => t.ParCompany_id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_id)
                .Index(t => t.ParCompany_id)
                .Index(t => t.ParLevel1_id);
            
            CreateTable(
                "dbo.ParRelapse",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParLevel1_Id = c.Int(),
                        ParLevel2_Id = c.Int(),
                        ParLevel3_Id = c.Int(),
                        ParFrequency_Id = c.Int(nullable: false),
                        NcNumber = c.Int(nullable: false),
                        EffectiveLength = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParFrequency", t => t.ParFrequency_Id)
                .ForeignKey("dbo.ParLevel3", t => t.ParLevel3_Id)
                .ForeignKey("dbo.ParLevel2", t => t.ParLevel2_Id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .Index(t => t.ParLevel1_Id)
                .Index(t => t.ParLevel2_Id)
                .Index(t => t.ParLevel3_Id)
                .Index(t => t.ParFrequency_Id);
            
            CreateTable(
                "dbo.ParFrequency",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Result_Level3",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CollectionLevel2_Id = c.Int(nullable: false),
                        ParLevel3_Id = c.Int(nullable: false),
                        ParLevel3_Name = c.String(),
                        Weight = c.Decimal(precision: 10, scale: 5),
                        IntervalMin = c.String(),
                        IntervalMax = c.String(),
                        Value = c.String(),
                        ValueText = c.String(),
                        IsConform = c.Boolean(),
                        IsNotEvaluate = c.Boolean(),
                        Defects = c.Decimal(precision: 10, scale: 5),
                        PunishmentValue = c.Decimal(precision: 10, scale: 5),
                        WeiEvaluation = c.Decimal(precision: 10, scale: 5),
                        Evaluation = c.Decimal(precision: 10, scale: 5),
                        WeiDefects = c.Decimal(precision: 30, scale: 8),
                        CT4Eva3 = c.Decimal(precision: 10, scale: 5),
                        Sampling = c.Decimal(precision: 10, scale: 5),
                        HasPhoto = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParLevel3", t => t.ParLevel3_Id)
                .ForeignKey("dbo.CollectionLevel2", t => t.CollectionLevel2_Id)
                .Index(t => t.CollectionLevel2_Id)
                .Index(t => t.ParLevel3_Id);
            
            CreateTable(
                "dbo.ParLocal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Level = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 155),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParDepartment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParLevel2Level1",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParLevel1_Id = c.Int(nullable: false),
                        ParLevel2_Id = c.Int(nullable: false),
                        ParCompany_Id = c.Int(),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParLevel2", t => t.ParLevel2_Id)
                .ForeignKey("dbo.ParLevel1", t => t.ParLevel1_Id)
                .Index(t => t.ParLevel1_Id)
                .Index(t => t.ParLevel2_Id);
            
            CreateTable(
                "dbo.ParConsolidationType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 155),
                        Description = c.String(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParScoreType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParLevelDefiniton",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 150),
                        Description = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CollectionLevel2XCluster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CollectionLevel2_Id = c.Int(),
                        ParCluster_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConfiguracaoEmail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Assunto = c.String(nullable: false),
                        Corpo1 = c.String(nullable: false),
                        Corpo2 = c.String(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConfiguracaoEmailPA",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Senha = c.String(nullable: false),
                        Host = c.String(nullable: false),
                        Port = c.Int(nullable: false),
                        DataCriacao = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DataAlteracao = c.DateTime(precision: 7, storeType: "datetime2"),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConsolidationLevel1XCluster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConsolidationLevel1_Id = c.Int(),
                        ParCluster_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConsolidationLevel2XCluster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConsolidationLevel2_Id = c.Int(),
                        ParCluster_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContramedidaEspecifica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 500, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.fa_ContramedidaEspecifica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdFormularioTratamentoAnomalia = c.Int(nullable: false),
                        IdContramedidaEspecifica = c.Int(nullable: false),
                        Versao = c.Binary(nullable: false, maxLength: 8, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContramedidaEspecifica", t => t.IdContramedidaEspecifica)
                .Index(t => t.IdContramedidaEspecifica);
            
            CreateTable(
                "dbo.ContramedidaGenerica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContramedidaGenerica = c.String(maxLength: 100),
                        CausaGenerica = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.fa_ContramedidaGenerica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdFormularioTratamentoAnomalia = c.Int(nullable: false),
                        IdContramedidaGenerica = c.Int(nullable: false),
                        Prioridade = c.Int(nullable: false),
                        Versao = c.Binary(nullable: false, maxLength: 8, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContramedidaGenerica", t => t.IdContramedidaGenerica)
                .Index(t => t.IdContramedidaGenerica);
            
            CreateTable(
                "dbo.ControleMetaTolerancia",
                c => new
                    {
                        UnidadeId = c.Int(nullable: false),
                        DepartamentoId = c.Int(nullable: false),
                        OperacaoId = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        ProximaMetaTolerancia = c.Decimal(precision: 11, scale: 8),
                        UltimoNumeroNC = c.Decimal(precision: 11, scale: 8),
                        TotalNC = c.Decimal(precision: 11, scale: 8),
                    })
                .PrimaryKey(t => new { t.UnidadeId, t.DepartamentoId, t.OperacaoId, t.Data });
            
            CreateTable(
                "dbo.Counter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 100),
                        Value = c.Short(nullable: false),
                        ExpireAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "HangFire.Counter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 100),
                        Value = c.Short(nullable: false),
                        ExpireAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DelDados",
                c => new
                    {
                        TESTE = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TESTE);
            
            CreateTable(
                "dbo.DesvioNiveis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Desvio = c.Int(nullable: false),
                        Nivel = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Desvios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataHora = c.DateTime(nullable: false),
                        UnidadeId = c.Int(nullable: false),
                        Unidade = c.String(nullable: false, maxLength: 100),
                        DepartamentoId = c.Int(nullable: false),
                        Departamento = c.String(nullable: false, maxLength: 100),
                        OperacaoId = c.Int(nullable: false),
                        Operacao = c.String(nullable: false, maxLength: 100),
                        NumeroAvaliacao = c.Int(nullable: false),
                        Desvio = c.Int(nullable: false),
                        MailItemId = c.Int(),
                        Meta = c.Decimal(precision: 5, scale: 2),
                        Real = c.Decimal(precision: 5, scale: 2),
                        TarefaId = c.Int(),
                        NumeroAmostra = c.Int(),
                        AlertaEmitido = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Email_ConfiguracaoEmailSgq",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Port = c.Int(nullable: false),
                        UseDefaultCredentials = c.Boolean(nullable: false),
                        EnableSsl = c.Boolean(nullable: false),
                        login = c.String(nullable: false),
                        Host = c.String(nullable: false),
                        pass = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Empresa",
                c => new
                    {
                        nCdEmpresa = c.Decimal(nullable: false, precision: 10, scale: 0),
                        cNmEmpresa = c.String(nullable: false, maxLength: 50, unicode: false),
                        cSgEmpresa = c.String(nullable: false, maxLength: 3, unicode: false),
                        cCdOrgaoRegulador = c.String(maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.nCdEmpresa);
            
            CreateTable(
                "dbo.Empresas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 100),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EquipamentosAvaliados",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Chave = c.String(),
                        EquipamentoId = c.Int(nullable: false),
                        Equipamento = c.String(nullable: false, maxLength: 100),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Estados",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Example",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        SelectedElement = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.fa_GrupoCausa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdFormularioTratamentoAnomalia = c.Int(nullable: false),
                        IdGrupoCausa = c.Int(nullable: false),
                        Versao = c.Binary(nullable: false, maxLength: 8, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GrupoCausa", t => t.IdGrupoCausa)
                .Index(t => t.IdGrupoCausa);
            
            CreateTable(
                "dbo.GrupoCausa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GrupoCausa = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FormularioTratamentoAnomalia",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Unidade = c.Int(nullable: false),
                        Operacao = c.Int(nullable: false),
                        Monitoramento = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DataInicio = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DataFim = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Tarefa = c.Int(nullable: false),
                        Versao = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
                        Supervisor = c.Int(nullable: false),
                        Gerente = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Hash",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 100),
                        Field = c.String(nullable: false, maxLength: 100),
                        Value = c.String(),
                        ExpireAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "HangFire.Hash",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 100),
                        Field = c.String(nullable: false, maxLength: 100),
                        Value = c.String(),
                        ExpireAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ItemMenu",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemMenu_Id = c.Int(),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        Icon = c.String(maxLength: 255, unicode: false),
                        Url = c.String(maxLength: 255, unicode: false),
                        Resource = c.String(maxLength: 255, unicode: false),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Job",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StateId = c.Int(),
                        StateName = c.String(maxLength: 20),
                        InvocationData = c.String(nullable: false),
                        Arguments = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ExpireAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobParameter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 40),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Job", t => t.JobId, cascadeDelete: true)
                .Index(t => t.JobId);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Reason = c.String(maxLength: 100),
                        CreatedAt = c.DateTime(nullable: false),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Job", t => t.JobId, cascadeDelete: true)
                .Index(t => t.JobId);
            
            CreateTable(
                "HangFire.Job",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StateId = c.Int(),
                        StateName = c.String(maxLength: 20),
                        InvocationData = c.String(nullable: false),
                        Arguments = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ExpireAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "HangFire.JobParameter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 40),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("HangFire.Job", t => t.JobId, cascadeDelete: true)
                .Index(t => t.JobId);
            
            CreateTable(
                "HangFire.State",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Reason = c.String(maxLength: 100),
                        CreatedAt = c.DateTime(nullable: false),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("HangFire.Job", t => t.JobId, cascadeDelete: true)
                .Index(t => t.JobId);
            
            CreateTable(
                "dbo.JobQueue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobId = c.Int(nullable: false),
                        Queue = c.String(nullable: false, maxLength: 50),
                        FetchedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "HangFire.JobQueue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobId = c.Int(nullable: false),
                        Queue = c.String(nullable: false, maxLength: 50),
                        FetchedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LeftControlRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Controller = c.String(nullable: false, maxLength: 50),
                        Action = c.String(nullable: false, maxLength: 50),
                        Role = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.List",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 100),
                        Value = c.String(),
                        ExpireAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "HangFire.List",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 100),
                        Value = c.String(),
                        ExpireAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LogAlteracoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tabela = c.String(nullable: false, maxLength: 100),
                        Registro = c.Int(nullable: false),
                        Campo = c.String(nullable: false, maxLength: 100),
                        Valor = c.String(),
                        UsuarioAlteracao = c.Int(nullable: false),
                        DataAlteracao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LogJson",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        result = c.String(nullable: false, unicode: false, storeType: "text"),
                        log = c.String(nullable: false, unicode: false, storeType: "text"),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Device_Id = c.String(),
                        AppVersion = c.String(),
                        callback = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LogOperacaoPA",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MensagemOperacao = c.String(nullable: false, unicode: false),
                        Linha = c.Int(),
                        NomeUsuario = c.String(unicode: false),
                        DataOcorrencia = c.DateTime(precision: 7, storeType: "datetime2"),
                        TipoRegistro = c.Int(),
                        TextoExcecao = c.String(unicode: false),
                        TextoPilhaExcecao = c.String(),
                        DescricaoLanIp = c.String(unicode: false),
                        DescricaoInternetIp = c.String(unicode: false),
                        NomeMetodo = c.String(unicode: false),
                        UrlTela = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LogSgq",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        addDate = c.DateTime(nullable: false),
                        Level = c.String(unicode: false),
                        Call_Site = c.String(unicode: false),
                        Exception_Type = c.String(unicode: false),
                        Exception_Message = c.String(unicode: false),
                        Stack_Trace = c.String(unicode: false),
                        Additional_Info = c.String(unicode: false),
                        Email = c.String(unicode: false),
                        Second_Log_Path = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LogSgqGlobal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        addDate = c.DateTime(),
                        Level = c.String(unicode: false),
                        Call_Site = c.String(unicode: false),
                        Exception_Type = c.String(unicode: false),
                        Exception_Message = c.String(unicode: false),
                        Stack_Trace = c.String(unicode: false),
                        Additional_Info = c.String(unicode: false),
                        Object = c.String(unicode: false),
                        email = c.String(unicode: false),
                        Second_Log_Path = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.manDataCollectIT",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        instantDatetime = c.DateTime(),
                        referenceDatetime = c.DateTime(),
                        userSGQ_id = c.Int(),
                        parCompany_id = c.Int(),
                        parFrequency_id = c.Int(),
                        shift = c.Int(),
                        dataType = c.String(maxLength: 110),
                        amountData = c.Decimal(precision: 32, scale: 10),
                        parMeasurementUnit_Id = c.Int(),
                        comments = c.String(maxLength: 800, unicode: false),
                        addDate = c.DateTime(nullable: false),
                        alterDate = c.DateTime(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MigrationHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 155),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NQA",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NivelGeralInspecao = c.Int(nullable: false),
                        TamanhoLoteMin = c.Int(nullable: false),
                        TamanhoLoteMax = c.Int(nullable: false),
                        Amostra = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Observacoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Chave = c.String(),
                        Foto = c.Binary(),
                        Texto = c.String(),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Acao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Unidade_Id = c.Int(),
                        Departamento_Id = c.Int(),
                        QuandoInicio = c.DateTime(precision: 7, storeType: "datetime2"),
                        DuracaoDias = c.Int(),
                        QuandoFim = c.DateTime(precision: 7, storeType: "datetime2"),
                        ComoPontosimportantes = c.String(),
                        Predecessora_Id = c.Int(),
                        PraQue = c.String(),
                        QuantoCusta = c.Decimal(precision: 35, scale: 10),
                        Status = c.Int(),
                        Panejamento_Id = c.Int(),
                        Pa_IndicadorSgqAcao_Id = c.Int(),
                        Pa_Problema_Desvio_Id = c.Int(),
                        Level1Id = c.Int(),
                        Level2Id = c.Int(),
                        Level3Id = c.Int(),
                        Quem_Id = c.Int(),
                        CausaGenerica_Id = c.Int(),
                        ContramedidaGenerica_Id = c.Int(),
                        GrupoCausa_Id = c.Int(),
                        CausaEspecifica = c.String(),
                        ContramedidaEspecifica = c.String(),
                        TipoIndicador = c.Int(),
                        Fta_Id = c.Int(),
                        Observacao = c.String(maxLength: 255, unicode: false),
                        Level1Name = c.String(maxLength: 500, unicode: false),
                        Level2Name = c.String(maxLength: 500, unicode: false),
                        Level3Name = c.String(maxLength: 500, unicode: false),
                        Regional = c.String(maxLength: 500, unicode: false),
                        UnidadeName = c.String(maxLength: 500, unicode: false),
                        UnidadeDeMedida_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_AcaoXQuem",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Acao_Id = c.Int(nullable: false),
                        Quem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.Acao_Id, t.Quem_Id });
            
            CreateTable(
                "dbo.Pa_Acompanhamento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Acao_Id = c.Int(nullable: false),
                        Order = c.Int(),
                        Status_Id = c.Int(nullable: false),
                        Author_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.pa_acompanhamento_tarefa",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        id_tarefa = c.Int(nullable: false),
                        data_envio = c.DateTime(nullable: false),
                        comentario = c.String(maxLength: 200),
                        enviado = c.String(maxLength: 50),
                        status = c.String(maxLength: 50),
                        nome_participante_envio = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pa_tarefa", t => t.id_tarefa, cascadeDelete: true)
                .Index(t => t.id_tarefa);
            
            CreateTable(
                "dbo.pa_tarefa",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        id_projeto = c.Int(nullable: false),
                        id_participante_criador = c.Int(),
                        data_criacao = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pa_projeto", t => t.id_projeto, cascadeDelete: true)
                .ForeignKey("dbo.pa_participante", t => t.id_participante_criador)
                .Index(t => t.id_projeto)
                .Index(t => t.id_participante_criador);
            
            CreateTable(
                "dbo.pa_participante",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 150, unicode: false),
                        senha = c.String(nullable: false, maxLength: 50),
                        ativo = c.Boolean(nullable: false),
                        codigo = c.Int(nullable: false),
                        email = c.String(nullable: false, maxLength: 150, unicode: false),
                        telefone = c.String(maxLength: 50, unicode: false),
                        id_empresa = c.Int(nullable: false),
                        perfil_acesso = c.Byte(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pa_empresa", t => t.id_empresa, cascadeDelete: true)
                .Index(t => t.id_empresa);
            
            CreateTable(
                "dbo.pa_empresa",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.pa_grupo_projeto",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 150),
                        sequencia = c.Int(),
                        id_empresa = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pa_empresa", t => t.id_empresa)
                .Index(t => t.id_empresa);
            
            CreateTable(
                "dbo.pa_projeto",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 150),
                        id_empresa = c.Int(nullable: false),
                        id_grupo_projeto = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pa_grupo_projeto", t => t.id_grupo_projeto)
                .ForeignKey("dbo.pa_empresa", t => t.id_empresa)
                .Index(t => t.id_empresa)
                .Index(t => t.id_grupo_projeto);
            
            CreateTable(
                "dbo.pa_campo",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 150, unicode: false),
                        tipo = c.String(nullable: false, maxLength: 20, unicode: false),
                        agrupador = c.Boolean(nullable: false),
                        sequencia = c.Int(),
                        obrigatorio = c.Boolean(nullable: false),
                        ativo = c.Boolean(nullable: false),
                        id_projeto = c.Int(nullable: false),
                        predefinido = c.Boolean(nullable: false),
                        modificavel = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pa_projeto", t => t.id_projeto, cascadeDelete: true)
                .Index(t => t.id_projeto);
            
            CreateTable(
                "dbo.pa_multipla_escolha",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        id_campo = c.Int(nullable: false),
                        nome = c.String(),
                        id_externo = c.Int(),
                        cor = c.String(maxLength: 50),
                        id_pai_externo = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pa_campo", t => t.id_campo, cascadeDelete: true)
                .Index(t => t.id_campo);
            
            CreateTable(
                "dbo.pa_vinculo_campo_tarefa",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        id_multipla_escolha = c.Int(),
                        id_campo = c.Int(nullable: false),
                        id_tarefa = c.Int(nullable: false),
                        valor = c.String(maxLength: 150, unicode: false),
                        id_participante = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pa_multipla_escolha", t => t.id_multipla_escolha)
                .ForeignKey("dbo.pa_campo", t => t.id_campo)
                .ForeignKey("dbo.pa_participante", t => t.id_participante)
                .ForeignKey("dbo.pa_tarefa", t => t.id_tarefa)
                .Index(t => t.id_multipla_escolha)
                .Index(t => t.id_campo)
                .Index(t => t.id_tarefa)
                .Index(t => t.id_participante);
            
            CreateTable(
                "dbo.pa_vinculo_participante_multipla_escolha",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        id_participante = c.Int(nullable: false),
                        id_multipla_escolha = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pa_multipla_escolha", t => t.id_multipla_escolha, cascadeDelete: true)
                .ForeignKey("dbo.pa_participante", t => t.id_participante, cascadeDelete: true)
                .Index(t => t.id_participante)
                .Index(t => t.id_multipla_escolha);
            
            CreateTable(
                "dbo.pa_vinculo_participante_projeto",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        id_projeto = c.Int(nullable: false),
                        id_participante = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pa_projeto", t => t.id_projeto, cascadeDelete: true)
                .ForeignKey("dbo.pa_participante", t => t.id_participante, cascadeDelete: true)
                .Index(t => t.id_projeto)
                .Index(t => t.id_participante);
            
            CreateTable(
                "dbo.Pa_CausaEspecifica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 500, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_CausaGenerica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CausaGenerica = c.String(maxLength: 100),
                        GrupoCausa = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_CausaMedidaXAcao",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Acao_Id = c.Int(nullable: false),
                        CausaGenerica_Id = c.Int(),
                        CausaEspecifica_Id = c.Int(),
                        ContramedidaGenerica_Id = c.Int(),
                        ContramedidaEspecifica_Id = c.Int(),
                        GrupoCausa_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.Id, t.Acao_Id });
            
            CreateTable(
                "dbo.Pa_Colvis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ColVisShow = c.String(maxLength: 255, unicode: false),
                        ColVisHide = c.String(maxLength: 255, unicode: false),
                        Pa_Quem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.pa_configuracao_email",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        email = c.String(nullable: false, maxLength: 200),
                        senha = c.String(nullable: false, maxLength: 200),
                        host = c.String(nullable: false, maxLength: 50),
                        port = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Pa_ContramedidaEspecifica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 500, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_ContramedidaGenerica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContramedidaGenerica = c.String(maxLength: 100),
                        CausaGenerica = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Coordenacao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                        GERENCIA_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Departamento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Dimensao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Diretoria",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Gerencia",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_GrupoCausa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GrupoCausa = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Indicadores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_IndicadoresDeProjeto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                        Pa_Iniciativa_Id = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_IndicadoresDiretriz",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                        Pa_Objetivo_Id = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_IndicadorSgqAcao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                        Pa_Dimensao_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Iniciativa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.pa_log_operacao",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        mensagem_operacao = c.String(nullable: false, unicode: false),
                        linha = c.Int(),
                        nm_usuario = c.String(maxLength: 150, unicode: false),
                        dt_ocorrencia = c.DateTime(storeType: "date"),
                        hr_ocorrencia = c.Time(precision: 7),
                        tp_registro = c.Int(),
                        tx_excecao = c.String(unicode: false),
                        dc_lan_ip = c.String(maxLength: 50, unicode: false),
                        tx_pilha_excecao = c.String(),
                        dc_internet_ip = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Pa_Missao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Objetivo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                        Pa_Dimensao_Id = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_ObjetivoGeral",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                        Pa_IndicadoresDeProjeto_Id = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Planejamento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Diretoria_Id = c.Int(),
                        Gerencia_Id = c.Int(),
                        Coordenacao_Id = c.Int(),
                        Missao_Id = c.Int(),
                        Visao_Id = c.Int(),
                        TemaAssunto_Id = c.Int(),
                        Indicadores_Id = c.Int(),
                        Iniciativa_Id = c.Int(),
                        ObjetivoGerencial_Id = c.Int(),
                        Dimensao = c.String(),
                        Objetivo = c.String(),
                        ValorDe = c.Decimal(precision: 38, scale: 10),
                        ValorPara = c.Decimal(precision: 38, scale: 10),
                        DataInicio = c.DateTime(precision: 7, storeType: "datetime2"),
                        DataFim = c.DateTime(precision: 7, storeType: "datetime2"),
                        Order = c.Int(),
                        Dimensao_Id = c.Int(),
                        Objetivo_Id = c.Int(),
                        IndicadoresDiretriz_Id = c.Int(),
                        IndicadoresDeProjeto_Id = c.Int(),
                        Estrategico_Id = c.Int(),
                        Responsavel_Diretriz = c.Int(),
                        Responsavel_Projeto = c.Int(),
                        UnidadeDeMedida_Id = c.Int(),
                        TemaProjeto_Id = c.Int(),
                        TipoProjeto_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Problema_Desvio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                        Pa_Dimensao_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Quem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Query",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Query = c.String(nullable: false),
                        Desc = c.String(nullable: false),
                        FastKey = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_TemaAssunto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_TemaProjeto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_TipoProjeto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Unidade",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_UnidadeMedida",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pa_Visao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParCalendar",
                c => new
                    {
                        id = c.Int(nullable: false),
                        Data = c.DateTime(storeType: "date"),
                        Feriado = c.Boolean(),
                        DiaUtil = c.Boolean(),
                        NrDiaSemana = c.Int(),
                        NrSemanaMes = c.Int(),
                        NrSemanaAno = c.Int(),
                        Sabado = c.Int(),
                        Domingo = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ParConfSGQ",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HaveUnitLogin = c.Boolean(),
                        HaveShitLogin = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParGoalScorecard",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        PercentValueMid = c.Decimal(nullable: false, precision: 25, scale: 7),
                        PercentValueHigh = c.Decimal(nullable: false, precision: 25, scale: 7),
                        InitDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParGroupParLevel1",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128, unicode: false),
                        ParGroupParLevel1Type_Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        AlterDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.Id, t.Name, t.ParGroupParLevel1Type_Id, t.AddDate, t.IsActive });
            
            CreateTable(
                "dbo.ParGroupParLevel1Type",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128, unicode: false),
                        AddDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        AlterDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.Id, t.Name, t.AddDate, t.IsActive });
            
            CreateTable(
                "dbo.ParGroupParLevel1XParLevel1",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ParLevel1_Id = c.Int(),
                        ParGroupParLevel1_Id = c.Int(),
                        AlterDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.Id, t.AddDate, t.IsActive });
            
            CreateTable(
                "dbo.ParLataImagens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Imagem = c.Binary(nullable: false),
                        ParRecravacao_TipoLata_Id = c.Int(nullable: false),
                        PathFile = c.String(nullable: false, maxLength: 500, unicode: false),
                        FileName = c.String(nullable: false, maxLength: 100, unicode: false),
                        PontoIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParLevel1VariableProduction",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        AddDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        AlterDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.Id, t.Name, t.AddDate });
            
            CreateTable(
                "dbo.ParLevel1VariableProductionXLevel1",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ParLevel1VariableProduction_Id = c.Int(nullable: false),
                        ParLevel1_Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        AlterDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.Id, t.ParLevel1VariableProduction_Id, t.ParLevel1_Id, t.AddDate });
            
            CreateTable(
                "dbo.ParLevel2XHeaderField",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParLevel2_Id = c.Int(nullable: false),
                        ParLevel1_Id = c.Int(nullable: false),
                        ParHeaderField_Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        AlterDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParLevel3Value_Outer",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        ParLevel3_Id = c.Int(nullable: false),
                        ParLevel3_Name = c.String(nullable: false, maxLength: 200),
                        ParLevel3InputType_Id = c.Int(nullable: false),
                        ParLevel3InputType_Name = c.String(nullable: false, maxLength: 200),
                        ParCompany_Id = c.Int(),
                        ParCompany_Name = c.String(maxLength: 200),
                        ParMeasurementUnit_Id = c.Int(),
                        ParMeasurementUnit_Name = c.String(maxLength: 200),
                        OuterEmpresa_Id = c.Int(),
                        OuterEmpresa_Text = c.String(maxLength: 200),
                        OuterLevel3_Id = c.Int(),
                        OuterLevel3_Text = c.String(maxLength: 200),
                        OuterLevel3Value_Id = c.Int(),
                        OuterLevel3Value_Text = c.String(maxLength: 200),
                        OuterLevel3ValueIntervalMaxValue = c.Int(),
                        OuterLevel3ValueIntervalMinValue = c.Int(),
                        Operator = c.String(maxLength: 200),
                        Order = c.Int(),
                        UnidadeMedida_Id = c.Int(),
                        UnidadeMedidaText = c.String(maxLength: 200),
                        AceitavelEntreText = c.String(maxLength: 200),
                        AceitavelEntre_Id = c.Int(),
                        LimInferior = c.Decimal(precision: 18, scale: 5),
                        LimSuperior = c.Decimal(precision: 18, scale: 5),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParRecravacao_Linhas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ParCompany_Id = c.Int(),
                        ParRecravacao_TypeLata_Id = c.Int(),
                        NumberOfHeads = c.Int(),
                        Description = c.String(maxLength: 500),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParRecravacao_TipoLata",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                        NumberOfPoints = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParReports",
                c => new
                    {
                        id = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 128, unicode: false),
                        query = c.String(nullable: false, maxLength: 128, unicode: false),
                        AddDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        groupReport = c.String(unicode: false),
                        AlterDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.id, t.name, t.query, t.AddDate, t.IsActive });
            
            CreateTable(
                "dbo.Pcc1b",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Indicador = c.Int(),
                        Unidade = c.Int(),
                        Data = c.DateTime(precision: 7, storeType: "datetime2"),
                        Departamento = c.String(maxLength: 255, unicode: false),
                        VolumeAnimais = c.Int(),
                        Quartos = c.Int(),
                        Meta = c.Decimal(precision: 2, scale: 0),
                        ToleranciaDia = c.Single(),
                        Nivel11 = c.Single(),
                        Nivel12 = c.Single(),
                        Nivel13 = c.Single(),
                        Avaliacoes = c.Int(),
                        Amostras = c.Int(),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ParCompany_id = c.Int(),
                        ParLevel1_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PenalidadeReincidencia",
                c => new
                    {
                        PenalidadeReincidenciaId = c.Int(nullable: false, identity: true),
                        UnidadeId = c.Int(nullable: false),
                        DepartamentoId = c.Int(nullable: false),
                        OperacaoId = c.Int(nullable: false),
                        TarefaId = c.Int(nullable: false),
                        MonitoramentoId = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false),
                        Indice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PenalidadeReincidenciaId);
            
            CreateTable(
                "dbo.Perfil",
                c => new
                    {
                        nCdPerfil = c.Decimal(nullable: false, precision: 10, scale: 0),
                        cNmPerfil = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.nCdPerfil);
            
            CreateTable(
                "dbo.Period",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Produto",
                c => new
                    {
                        nCdProduto = c.Decimal(nullable: false, precision: 10, scale: 0),
                        cNmProduto = c.String(nullable: false, maxLength: 60, unicode: false),
                        cDescricaoDetalhada = c.String(maxLength: 400, unicode: false),
                    })
                .PrimaryKey(t => t.nCdProduto);
            
            CreateTable(
                "dbo.ProdutoInNatura",
                c => new
                    {
                        nCdProduto = c.Decimal(nullable: false, precision: 10, scale: 0),
                        cNmProduto = c.String(nullable: false, maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.nCdProduto);
            
            CreateTable(
                "dbo.ProdutosAvaliados",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Chave = c.String(),
                        Produto = c.String(nullable: false, maxLength: 100),
                        HorarioPesagem = c.Time(precision: 7),
                        DataPesagem = c.DateTime(storeType: "date"),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RecravacaoJson",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserSgqId = c.Int(nullable: false),
                        ParCompany_Id = c.Int(nullable: false),
                        Linha_Id = c.Int(nullable: false),
                        ObjectRecravacaoJson = c.String(nullable: false),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(),
                        isValidated = c.Boolean(),
                        ValidateLockDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ParLevel1_Id = c.Int(),
                        SalvoParaInserirNovaColeta = c.Int(),
                        UserFinished_Id = c.Int(),
                        UserValidated_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reports_CCA_Audit",
                c => new
                    {
                        AuditPeriod = c.Int(name: "Audit Period", nullable: false),
                        Shift = c.Int(nullable: false),
                        AuditReaudit = c.String(name: "Audit/Reaudit", nullable: false, maxLength: 8, unicode: false),
                        NoofDef = c.String(name: "No of Def", nullable: false, maxLength: 3, unicode: false),
                        CattleType = c.String(name: "Cattle Type", nullable: false, maxLength: 5, unicode: false),
                        ChainSpeed = c.Decimal(name: "Chain Speed", nullable: false, precision: 20, scale: 5),
                        Lot = c.Decimal(name: "Lot #", nullable: false, precision: 20, scale: 5),
                        MudScore = c.Decimal(name: "Mud Score", nullable: false, precision: 20, scale: 5),
                        PlantNumber = c.Int(name: "Plant Number"),
                        AuditArea = c.String(name: "Audit Area", unicode: false),
                        SetStartDate = c.DateTime(name: "Set Start Date", storeType: "date"),
                        SetStartTime = c.String(name: "Set Start Time", maxLength: 5, unicode: false),
                        Specks = c.Decimal(precision: 10, scale: 5),
                        Dressing = c.Decimal(precision: 10, scale: 5),
                        SingleHairs = c.Decimal(name: "Single Hairs", precision: 10, scale: 5),
                        Clusters = c.Decimal(precision: 10, scale: 5),
                        Hide = c.Decimal(precision: 10, scale: 5),
                        Defects = c.Decimal(precision: 38, scale: 5),
                        AuditorNameGlobal = c.String(name: "Auditor Name Global"),
                        SetEndDate = c.DateTime(name: "Set End Date", storeType: "date"),
                        SetEndTime = c.String(name: "Set End Time", maxLength: 5, unicode: false),
                    })
                .PrimaryKey(t => new { t.AuditPeriod, t.Shift, t.AuditReaudit, t.NoofDef, t.CattleType, t.ChainSpeed, t.Lot, t.MudScore });
            
            CreateTable(
                "dbo.Reports_CFF_Audit",
                c => new
                    {
                        Shift = c.Int(nullable: false),
                        Period = c.Int(nullable: false),
                        Side = c.Int(nullable: false),
                        Reaudit = c.String(nullable: false, maxLength: 8, unicode: false),
                        DefectCounter = c.Int(name: "Defect Counter", nullable: false),
                        PlantNumber = c.Int(name: "Plant Number"),
                        Set = c.Int(),
                        StartDate = c.DateTime(name: "Start Date", precision: 7, storeType: "datetime2"),
                        AuditArea = c.String(name: "Audit Area", unicode: false),
                        SetStartDate = c.DateTime(name: "Set Start Date", storeType: "date"),
                        SetStartTime = c.String(name: "Set Start Time", maxLength: 5, unicode: false),
                        SetEndDate = c.DateTime(name: "Set End Date", storeType: "date"),
                        SetEndTime = c.String(name: "Set End Time", maxLength: 5, unicode: false),
                        Auditor = c.String(),
                        Defects = c.Decimal(precision: 38, scale: 5),
                        Cut = c.Decimal(precision: 38, scale: 5),
                        FoldFlap = c.Decimal(name: "Fold/Flap", precision: 38, scale: 5),
                        Puncture = c.Decimal(precision: 38, scale: 5),
                    })
                .PrimaryKey(t => new { t.Shift, t.Period, t.Side, t.Reaudit, t.DefectCounter });
            
            CreateTable(
                "dbo.Reports_HTP_Audit",
                c => new
                    {
                        Shift = c.Int(nullable: false),
                        AuditReAudit = c.String(name: "Audit/ReAudit", nullable: false, maxLength: 8, unicode: false),
                        Period = c.Int(nullable: false),
                        BaisedUnbiased = c.String(name: "Baised/Unbiased", nullable: false, maxLength: 8, unicode: false),
                        StartingPhase = c.Int(name: "Starting Phase", nullable: false),
                        Reaudit = c.String(nullable: false, maxLength: 8, unicode: false),
                        ID = c.Int(nullable: false),
                        PlantNumber = c.Int(name: "Plant Number"),
                        StartDate = c.DateTime(name: "Start Date", precision: 7, storeType: "datetime2"),
                        StartTime = c.String(name: "Start Time", maxLength: 10, unicode: false),
                        JobId = c.String(name: "Job Id", unicode: false),
                        Auditor = c.String(),
                        JobDate = c.DateTime(name: "Job Date", storeType: "date"),
                        JobTime = c.String(name: "Job Time", maxLength: 10, unicode: false),
                        CorePracticeHTPDeviation = c.String(name: "Core Practice HTP Deviation", maxLength: 3, unicode: false),
                        OtherDeviaton = c.String(name: "Other Deviaton", maxLength: 3, unicode: false),
                        EndingPhase = c.Int(name: "Ending Phase"),
                        EndDate = c.DateTime(name: "End Date", storeType: "date"),
                        EndTime = c.String(name: "End Time", maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => new { t.Shift, t.AuditReAudit, t.Period, t.BaisedUnbiased, t.StartingPhase, t.Reaudit, t.ID });
            
            CreateTable(
                "dbo.Result_Level3_Photos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Result_Level3_Id = c.Int(),
                        Photo_Thumbnaills = c.String(unicode: false, storeType: "text"),
                        Photo = c.String(unicode: false, storeType: "text"),
                        Latitude = c.Double(),
                        Longitude = c.Double(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Resultados",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Chave = c.String(),
                        EmpresaId = c.Int(nullable: false),
                        Empresa = c.String(nullable: false, maxLength: 100),
                        UnidadeId = c.Int(nullable: false),
                        Unidade = c.String(nullable: false, maxLength: 100),
                        DepartamentoId = c.Int(nullable: false),
                        Departamento = c.String(nullable: false, maxLength: 100),
                        OperacaoId = c.Int(nullable: false),
                        Operacao = c.String(nullable: false, maxLength: 100),
                        TarefaId = c.Int(nullable: false),
                        Tarefa = c.String(nullable: false, maxLength: 100),
                        NumeroAvaliacao = c.Int(nullable: false),
                        NumeroAmostra = c.Int(nullable: false),
                        MonitoramentoId = c.Int(nullable: false),
                        Monitoramento = c.String(maxLength: 300),
                        ProdutoId = c.Int(nullable: false),
                        Produto = c.String(nullable: false, maxLength: 100),
                        DataHora = c.DateTime(nullable: false),
                        Monitor = c.Int(nullable: false),
                        Peso = c.Int(),
                        Lote = c.String(maxLength: 50),
                        PecasAvaliadas = c.Int(),
                        Minimo = c.String(nullable: false, maxLength: 20),
                        Maximo = c.String(nullable: false, maxLength: 20),
                        Acesso = c.String(nullable: false, maxLength: 20),
                        Avaliacao_1 = c.String(nullable: false, maxLength: 20),
                        Avaliacao_2 = c.Int(nullable: false),
                        Meta = c.Decimal(precision: 5, scale: 2),
                        Sequencial = c.Int(),
                        Banda = c.Int(),
                        DataHoraMonitor = c.DateTime(),
                        ToleranciaDia = c.Int(),
                        Nivel1 = c.Int(),
                        Nivel2 = c.Int(),
                        Nivel3 = c.Int(),
                        Status = c.String(maxLength: 100),
                        AvaliacaoAvulsa = c.Boolean(),
                        Amostragem = c.String(maxLength: 20),
                        FormaAmostragem = c.String(maxLength: 100),
                        Sexo = c.String(maxLength: 50),
                        Idade = c.Int(),
                        SiglaContusao = c.String(maxLength: 5),
                        SiglaFalhaOperacional = c.String(maxLength: 5),
                        DataTipificacao = c.DateTime(),
                        Mobile = c.Boolean(),
                        Data = c.DateTime(storeType: "date"),
                        Identificador = c.String(nullable: false, maxLength: 450),
                        VersaoAPP = c.String(maxLength: 100),
                        Reincidente = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResultadosData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Chave = c.String(unicode: false),
                        Campo = c.String(unicode: false),
                        Valor = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResultadosPCC",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Chave = c.String(),
                        EmpresaId = c.Int(nullable: false),
                        Empresa = c.String(nullable: false, maxLength: 100),
                        UnidadeId = c.Int(nullable: false),
                        Unidade = c.String(nullable: false, maxLength: 100),
                        DepartamentoId = c.Int(nullable: false),
                        Departamento = c.String(nullable: false, maxLength: 100),
                        OperacaoId = c.Int(nullable: false),
                        Operacao = c.String(nullable: false, maxLength: 100),
                        TarefaId = c.Int(nullable: false),
                        Tarefa = c.String(nullable: false, maxLength: 100),
                        NumeroAvaliacao = c.Int(nullable: false),
                        NumeroAmostra = c.Int(nullable: false),
                        MonitoramentoId = c.Int(nullable: false),
                        Monitoramento = c.String(maxLength: 300),
                        ProdutoId = c.Int(nullable: false),
                        Produto = c.String(nullable: false, maxLength: 100),
                        DataHora = c.DateTime(nullable: false),
                        Monitor = c.Int(nullable: false),
                        Peso = c.Int(),
                        Lote = c.String(maxLength: 50),
                        PecasAvaliadas = c.Int(),
                        Minimo = c.String(nullable: false, maxLength: 20),
                        Maximo = c.String(nullable: false, maxLength: 20),
                        Acesso = c.String(nullable: false, maxLength: 20),
                        Avaliacao_1 = c.String(nullable: false, maxLength: 20),
                        Avaliacao_2 = c.Int(nullable: false),
                        Meta = c.Decimal(precision: 5, scale: 2),
                        Sequencial = c.Int(),
                        Banda = c.Int(),
                        DataHoraMonitor = c.DateTime(),
                        ToleranciaDia = c.Int(),
                        Nivel1 = c.Int(),
                        Nivel2 = c.Int(),
                        Nivel3 = c.Int(),
                        Status = c.String(maxLength: 100),
                        AvaliacaoAvulsa = c.Boolean(),
                        Amostragem = c.String(maxLength: 20),
                        FormaAmostragem = c.String(maxLength: 100),
                        Sexo = c.String(maxLength: 50),
                        Idade = c.Int(),
                        SiglaContusao = c.String(maxLength: 5),
                        SiglaFalhaOperacional = c.String(maxLength: 5),
                        DataTipificacao = c.DateTime(),
                        Mobile = c.Boolean(),
                        Data = c.DateTime(storeType: "date"),
                        Identificador = c.String(nullable: false, maxLength: 450),
                        VersaoAPP = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResultLevel2HeaderField",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CollectionLevel2_Id = c.Int(nullable: false),
                        ParHeaderField_Id = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        name = c.String(),
                        value = c.String(),
                        PunishmentValue = c.Decimal(precision: 10, scale: 5),
                        AlterDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.Id, t.CollectionLevel2_Id, t.ParHeaderField_Id, t.AddDate, t.IsActive });
            
            CreateTable(
                "dbo.RetornoParaTablet",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UsuariosDaUnidade = c.String(nullable: false, maxLength: 128),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ParteDaTela = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.UsuariosDaUnidade });
            
            CreateTable(
                "dbo.RoleJBS",
                c => new
                    {
                        ScreenComponent_Id = c.Int(nullable: false),
                        Role = c.String(maxLength: 10, fixedLength: true),
                        Id = c.Int(),
                    })
                .PrimaryKey(t => t.ScreenComponent_Id);
            
            CreateTable(
                "dbo.RoleSGQ",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScreenComponent_Id = c.Int(nullable: false),
                        Role = c.String(maxLength: 10, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Type = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Id, t.Type });
            
            CreateTable(
                "dbo.RoleUserSgqXItemMenu",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(nullable: false, storeType: "date"),
                        AlterDate = c.DateTime(storeType: "date"),
                        Name = c.String(maxLength: 255, unicode: false),
                        ItemMenu_Id = c.Int(nullable: false),
                        RoleUserSgq_Id = c.Int(nullable: false),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Schema",
                c => new
                    {
                        Version = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Version);
            
            CreateTable(
                "HangFire.Schema",
                c => new
                    {
                        Version = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Version);
            
            CreateTable(
                "dbo.ScorecardConsolidadoDia",
                c => new
                    {
                        Unidade = c.Int(nullable: false),
                        Operacao = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        TotalAvaliado = c.Decimal(precision: 10, scale: 2),
                        TotalForaPadrao = c.Decimal(precision: 10, scale: 2),
                        Meta = c.Decimal(precision: 5, scale: 2),
                        Pontos = c.Decimal(precision: 5, scale: 2),
                    })
                .PrimaryKey(t => new { t.Unidade, t.Operacao, t.Data });
            
            CreateTable(
                "dbo.ScorecardJBS_V",
                c => new
                    {
                        TipoIndicador = c.Int(nullable: false),
                        TipoIndicadorName = c.String(nullable: false, maxLength: 5, unicode: false),
                        Level1Id = c.Int(nullable: false),
                        Level1Name = c.String(nullable: false, maxLength: 155),
                        Cluster = c.Int(),
                        ClusterName = c.String(maxLength: 155),
                        Regional = c.Int(),
                        RegionalName = c.String(maxLength: 155),
                        ParCompanyId = c.Int(),
                        ParCompanyName = c.String(maxLength: 155),
                        Criterio = c.Int(),
                        CriterioName = c.String(maxLength: 155),
                        AV = c.Decimal(precision: 32, scale: 8),
                        NC = c.Decimal(precision: 30, scale: 8),
                        Pontos = c.Decimal(precision: 10, scale: 5),
                        Meta = c.Decimal(precision: 10, scale: 5),
                        Real = c.Decimal(precision: 38, scale: 6),
                        PontosAtingidos = c.Decimal(precision: 38, scale: 6),
                        Scorecard = c.Decimal(precision: 38, scale: 6),
                    })
                .PrimaryKey(t => new { t.TipoIndicador, t.TipoIndicadorName, t.Level1Id, t.Level1Name });
            
            CreateTable(
                "dbo.ScreenComponent",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        HashKey = c.String(nullable: false, maxLength: 128),
                        Component = c.String(nullable: false, maxLength: 128),
                        Type = c.Int(),
                    })
                .PrimaryKey(t => new { t.Id, t.HashKey, t.Component });
            
            CreateTable(
                "dbo.Server",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100),
                        Data = c.String(),
                        LastHeartbeat = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "HangFire.Server",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100),
                        Data = c.String(),
                        LastHeartbeat = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Set",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 100),
                        Score = c.Double(nullable: false),
                        Value = c.String(nullable: false, maxLength: 256),
                        ExpireAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "HangFire.Set",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 100),
                        Score = c.Double(nullable: false),
                        Value = c.String(nullable: false, maxLength: 256),
                        ExpireAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SgqConfig",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ActiveIn = c.Int(nullable: false),
                        recoveryPassAvaliable = c.Boolean(nullable: false),
                        mockLoginEUA = c.Boolean(nullable: false),
                        MailSSL = c.Boolean(nullable: false),
                        MailPort = c.Int(nullable: false),
                        MockEmail = c.Boolean(nullable: false),
                        AddDate = c.DateTime(),
                        AlterDate = c.DateTime(),
                        urlPreffixAppColleta = c.String(),
                        urlAppColleta = c.String(),
                        MailFrom = c.String(),
                        MailPass = c.String(),
                        MailSmtp = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.ActiveIn, t.recoveryPassAvaliable, t.mockLoginEUA, t.MailSSL, t.MailPort, t.MockEmail });
            
            CreateTable(
                "dbo.SgqConfig2",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Config = c.String(nullable: false, maxLength: 128),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => new { t.Id, t.Config });
            
            CreateTable(
                "dbo.Shift",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sugestoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Chave = c.String(),
                        Acao = c.String(),
                        Porque = c.String(),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Terceiro",
                c => new
                    {
                        nCdTerceiro = c.Decimal(nullable: false, precision: 10, scale: 0),
                        cNmTerceiro = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.nCdTerceiro);
            
            CreateTable(
                "dbo.Tipificacao",
                c => new
                    {
                        Data = c.DateTime(nullable: false),
                        Unidade = c.String(nullable: false, maxLength: 100),
                        Sequencial = c.Int(nullable: false),
                        Banda = c.Int(nullable: false),
                        Sexo = c.String(nullable: false, maxLength: 100),
                        Idade = c.Int(nullable: false),
                        Tarefa = c.String(nullable: false, maxLength: 100),
                        Padrao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Data, t.Unidade, t.Sequencial, t.Banda, t.Sexo, t.Idade, t.Tarefa, t.Padrao });
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        nCdUsuario = c.Decimal(nullable: false, precision: 10, scale: 0),
                        cNmUsuario = c.String(nullable: false, maxLength: 50, unicode: false),
                        cSigla = c.String(nullable: false, maxLength: 20, unicode: false),
                        cEMail = c.String(maxLength: 50, unicode: false),
                        cTelefone = c.String(maxLength: 30, unicode: false),
                        cCelular = c.String(maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.nCdUsuario);
            
            CreateTable(
                "dbo.UsuarioPerfilEmpresa",
                c => new
                    {
                        nCdUsuario = c.Decimal(nullable: false, precision: 10, scale: 0),
                        nCdPerfil = c.Decimal(nullable: false, precision: 10, scale: 0),
                        nCdEmpresa = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.nCdUsuario, t.nCdPerfil, t.nCdEmpresa });
            
            CreateTable(
                "dbo.VacuoGRD",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Indicador = c.Int(),
                        Unidade = c.Int(),
                        Data = c.DateTime(precision: 7, storeType: "datetime2"),
                        Departamento = c.String(maxLength: 255, unicode: false),
                        HorasTrabalhadasPorDia = c.Int(),
                        AmostraPorDia = c.Int(),
                        QtdadeFamiliaProduto = c.Int(),
                        Avaliacoes = c.Int(),
                        Amostras = c.Int(),
                        AddDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ParCompany_id = c.Int(),
                        ParLevel1_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VerificacaoContagem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Status = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VerificacaoTipificacaoComparacao",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Sequencial = c.Int(nullable: false),
                        Banda = c.Int(nullable: false),
                        Identificador = c.String(nullable: false, maxLength: 30, unicode: false),
                        NumCaracteristica = c.Int(nullable: false),
                        DataHora = c.DateTime(nullable: false),
                        Conforme = c.Boolean(),
                        JBS = c.Boolean(nullable: false),
                        valorSGQ = c.Int(),
                        valorJBS = c.Int(),
                        nCdEmpresa = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.VerificacaoTipificacaoJBS",
                c => new
                    {
                        nCdEmpresa = c.Int(nullable: false),
                        iSequencial = c.Int(nullable: false),
                        iSequencialTipificacao = c.Int(nullable: false),
                        iBanda = c.Int(nullable: false),
                        cIdentificadorTipificacao = c.String(nullable: false, maxLength: 30, unicode: false),
                        nCdCaracteristicaTipificacao = c.Int(nullable: false),
                        dMovimento = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.nCdEmpresa, t.iSequencial, t.iSequencialTipificacao, t.iBanda, t.cIdentificadorTipificacao, t.nCdCaracteristicaTipificacao });
            
            CreateTable(
                "dbo.VerificacaoTipificacaoResultados",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TarefaId = c.Int(nullable: false),
                        CaracteristicaTipificacaoId = c.Int(),
                        Chave = c.String(unicode: false),
                        AreasParticipantesId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VerificacaoTipificacaoV2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AlterDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        CollectionDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Sequencial = c.Int(),
                        Banda = c.Int(),
                        ParCompany_Id = c.Int(),
                        UserSgq_Id = c.Int(),
                        cSgCaracteristica = c.String(maxLength: 250),
                        GRT_nCdCaracteristicaTipificacao = c.Int(),
                        JBS_nCdCaracteristicaTipificacao = c.Int(),
                        ResultadoComparacaoGRT_JBS = c.Boolean(),
                        cIdentificadorTipificacao = c.String(maxLength: 250),
                        cNmCaracteristica = c.String(maxLength: 250),
                        Key = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VerificacaoTipificacaoValidacao",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nCdEmpresa = c.Int(nullable: false),
                        dMovimento = c.DateTime(),
                        iSequencial = c.Int(nullable: false),
                        iSequencialTipificacao = c.Int(nullable: false),
                        iBanda = c.Int(nullable: false),
                        cIdentificadorTipificacao = c.String(nullable: false, maxLength: 30, unicode: false),
                        nCdCaracteristicaTipificacao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.VolumeAbate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Volume = c.Int(nullable: false),
                        UnidadeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VTVerificacaoContagem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Status = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VTVerificacaoTipificacao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sequencial = c.Int(nullable: false),
                        Banda = c.Byte(nullable: false),
                        DataHora = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UnidadeId = c.Int(nullable: false),
                        Chave = c.String(),
                        Status = c.Boolean(),
                        EvaluationNumber = c.Int(),
                        Sample = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VTVerificacaoTipificacaoComparacao",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Sequencial = c.Int(nullable: false),
                        Banda = c.Int(nullable: false),
                        Identificador = c.String(nullable: false, maxLength: 30, unicode: false),
                        NumCaracteristica = c.Int(nullable: false),
                        DataHora = c.DateTime(nullable: false),
                        valorSGQ = c.Int(),
                        valorJBS = c.Int(),
                        nCdEmpresa = c.Int(),
                        Conforme = c.Boolean(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.VTVerificacaoTipificacaoJBS",
                c => new
                    {
                        nCdEmpresa = c.Int(nullable: false),
                        iSequencial = c.Int(nullable: false),
                        iSequencialTipificacao = c.Int(nullable: false),
                        iBanda = c.Int(nullable: false),
                        cIdentificadorTipificacao = c.String(nullable: false, maxLength: 30, unicode: false),
                        nCdCaracteristicaTipificacao = c.Int(nullable: false),
                        dMovimento = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.nCdEmpresa, t.iSequencial, t.iSequencialTipificacao, t.iBanda, t.cIdentificadorTipificacao, t.nCdCaracteristicaTipificacao });
            
            CreateTable(
                "dbo.VTVerificacaoTipificacaoResultados",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TarefaId = c.Int(nullable: false),
                        CaracteristicaTipificacaoId = c.Int(),
                        Chave = c.String(),
                        AreasParticipantesId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VTVerificacaoTipificacaoTarefaIntegracao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TarefaId = c.Int(nullable: false),
                        CaracteristicaTipificacaoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VTVerificacaoTipificacaoValidacao",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nCdEmpresa = c.Int(nullable: false),
                        dMovimento = c.DateTime(),
                        iSequencial = c.Int(nullable: false),
                        iSequencialTipificacao = c.Int(nullable: false),
                        iBanda = c.Int(nullable: false),
                        cIdentificadorTipificacao = c.String(nullable: false, maxLength: 30, unicode: false),
                        nCdCaracteristicaTipificacao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.VWCFFResults",
                c => new
                    {
                        Shift = c.Int(nullable: false),
                        Period = c.Int(nullable: false),
                        Side = c.Int(nullable: false),
                        Reaudit = c.String(nullable: false, maxLength: 8, unicode: false),
                        DefectCounter = c.Int(name: "Defect Counter", nullable: false),
                        PlantNumber = c.Int(name: "Plant Number"),
                        Set = c.Int(),
                        StartDate = c.DateTime(name: "Start Date", precision: 7, storeType: "datetime2"),
                        AuditArea = c.String(name: "Audit Area", unicode: false),
                        SetStartDate = c.DateTime(name: "Set Start Date", storeType: "date"),
                        SetStartTime = c.String(name: "Set Start Time", maxLength: 5, unicode: false),
                        SetEndDate = c.DateTime(name: "Set End Date", storeType: "date"),
                        SetEndTime = c.String(name: "Set End Time", maxLength: 5, unicode: false),
                        Auditor = c.String(),
                        Defects = c.Decimal(precision: 38, scale: 5),
                        Cut = c.Decimal(precision: 38, scale: 5),
                        FoldFlap = c.Decimal(name: "Fold/Flap", precision: 38, scale: 5),
                        Puncture = c.Decimal(precision: 38, scale: 5),
                    })
                .PrimaryKey(t => new { t.Shift, t.Period, t.Side, t.Reaudit, t.DefectCounter });
            
            CreateTable(
                "dbo.Z_Sistema",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IntervaloAtualizacao = c.Int(nullable: false),
                        UsuarioInsercao = c.Int(nullable: false),
                        DataInsercao = c.DateTime(nullable: false),
                        UsuarioAlteracao = c.Int(),
                        DataAlteracao = c.DateTime(),
                        VersaoDB = c.String(nullable: false, maxLength: 20),
                        ArquivoAtualizacao = c.String(maxLength: 100),
                        VersaoAPP = c.String(maxLength: 100, unicode: false),
                        Atualizacao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.pa_vinculo_campo_tarefa", "id_tarefa", "dbo.pa_tarefa");
            DropForeignKey("dbo.pa_vinculo_participante_projeto", "id_participante", "dbo.pa_participante");
            DropForeignKey("dbo.pa_vinculo_participante_multipla_escolha", "id_participante", "dbo.pa_participante");
            DropForeignKey("dbo.pa_vinculo_campo_tarefa", "id_participante", "dbo.pa_participante");
            DropForeignKey("dbo.pa_tarefa", "id_participante_criador", "dbo.pa_participante");
            DropForeignKey("dbo.pa_projeto", "id_empresa", "dbo.pa_empresa");
            DropForeignKey("dbo.pa_participante", "id_empresa", "dbo.pa_empresa");
            DropForeignKey("dbo.pa_grupo_projeto", "id_empresa", "dbo.pa_empresa");
            DropForeignKey("dbo.pa_projeto", "id_grupo_projeto", "dbo.pa_grupo_projeto");
            DropForeignKey("dbo.pa_vinculo_participante_projeto", "id_projeto", "dbo.pa_projeto");
            DropForeignKey("dbo.pa_tarefa", "id_projeto", "dbo.pa_projeto");
            DropForeignKey("dbo.pa_campo", "id_projeto", "dbo.pa_projeto");
            DropForeignKey("dbo.pa_vinculo_campo_tarefa", "id_campo", "dbo.pa_campo");
            DropForeignKey("dbo.pa_multipla_escolha", "id_campo", "dbo.pa_campo");
            DropForeignKey("dbo.pa_vinculo_participante_multipla_escolha", "id_multipla_escolha", "dbo.pa_multipla_escolha");
            DropForeignKey("dbo.pa_vinculo_campo_tarefa", "id_multipla_escolha", "dbo.pa_multipla_escolha");
            DropForeignKey("dbo.pa_acompanhamento_tarefa", "id_tarefa", "dbo.pa_tarefa");
            DropForeignKey("HangFire.State", "JobId", "HangFire.Job");
            DropForeignKey("HangFire.JobParameter", "JobId", "HangFire.Job");
            DropForeignKey("dbo.State", "JobId", "dbo.Job");
            DropForeignKey("dbo.JobParameter", "JobId", "dbo.Job");
            DropForeignKey("dbo.fa_GrupoCausa", "IdGrupoCausa", "dbo.GrupoCausa");
            DropForeignKey("dbo.fa_ContramedidaGenerica", "IdContramedidaGenerica", "dbo.ContramedidaGenerica");
            DropForeignKey("dbo.fa_ContramedidaEspecifica", "IdContramedidaEspecifica", "dbo.ContramedidaEspecifica");
            DropForeignKey("dbo.CollectionLevel2XCollectionJson", "CollectionJson_Id", "dbo.CollectionJson");
            DropForeignKey("dbo.Result_Level3", "CollectionLevel2_Id", "dbo.CollectionLevel2");
            DropForeignKey("dbo.CorrectiveAction", "CollectionLevel02Id", "dbo.CollectionLevel2");
            DropForeignKey("dbo.CollectionLevel2XParHeaderField", "CollectionLevel2_Id", "dbo.CollectionLevel2");
            DropForeignKey("dbo.ParHeaderField", "ParFieldType_Id", "dbo.ParFieldType");
            DropForeignKey("dbo.ParMultipleValuesXParCompany", "ParHeaderField_Id", "dbo.ParHeaderField");
            DropForeignKey("dbo.ParMultipleValues", "ParHeaderField_Id", "dbo.ParHeaderField");
            DropForeignKey("dbo.ParHeaderField", "ParLevelDefinition_Id", "dbo.ParLevelDefiniton");
            DropForeignKey("dbo.ParLevel1XHeaderField", "ParHeaderField_Id", "dbo.ParHeaderField");
            DropForeignKey("dbo.VolumeVacuoGRD", "ParLevel1_id", "dbo.ParLevel1");
            DropForeignKey("dbo.VolumePcc1b", "ParLevel1_id", "dbo.ParLevel1");
            DropForeignKey("dbo.VolumeCepRecortes", "ParLevel1_id", "dbo.ParLevel1");
            DropForeignKey("dbo.VolumeCepDesossa", "ParLevel1_id", "dbo.ParLevel1");
            DropForeignKey("dbo.ParLevel1", "ParScoreType_Id", "dbo.ParScoreType");
            DropForeignKey("dbo.ParRelapse", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.ParNotConformityRuleXLevel", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.ParMultipleValuesXParCompany", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.ParLevel3Value", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.ParLevel3Level2Level1", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.ParLevel3EvaluationSample", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.ParLevel2Level1", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.ParLevel2ControlCompany", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.ParLevel1XHeaderField", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.ParLevel1XCluster", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.ParGoal", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.ParCounterXLocal", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.ParLevel1", "ParConsolidationType_Id", "dbo.ParConsolidationType");
            DropForeignKey("dbo.Defect", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.ConsolidationLevel1", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.ConsolidationLevel2", "ConsolidationLevel1_Id", "dbo.ConsolidationLevel1");
            DropForeignKey("dbo.ParSample", "ParLevel2_Id", "dbo.ParLevel2");
            DropForeignKey("dbo.ParRelapse", "ParLevel2_Id", "dbo.ParLevel2");
            DropForeignKey("dbo.ParNotConformityRuleXLevel", "ParLevel2_Id", "dbo.ParLevel2");
            DropForeignKey("dbo.ParLevel3Value", "ParLevel2_Id", "dbo.ParLevel2");
            DropForeignKey("dbo.ParLevel3Level2", "ParLevel2_Id", "dbo.ParLevel2");
            DropForeignKey("dbo.ParLevel3Group", "ParLevel2_Id", "dbo.ParLevel2");
            DropForeignKey("dbo.ParLevel3EvaluationSample", "ParLevel2_Id", "dbo.ParLevel2");
            DropForeignKey("dbo.ParLevel2Level1", "ParLevel2_Id", "dbo.ParLevel2");
            DropForeignKey("dbo.ParLevel2ControlCompany", "ParLevel2_Id", "dbo.ParLevel2");
            DropForeignKey("dbo.ParEvaluation", "ParLevel2_Id", "dbo.ParLevel2");
            DropForeignKey("dbo.ParLevel2", "ParDepartment_Id", "dbo.ParDepartment");
            DropForeignKey("dbo.ParCounterXLocal", "ParLevel2_Id", "dbo.ParLevel2");
            DropForeignKey("dbo.ParCounterXLocal", "ParLocal_Id", "dbo.ParLocal");
            DropForeignKey("dbo.Result_Level3", "ParLevel3_Id", "dbo.ParLevel3");
            DropForeignKey("dbo.ParRelapse", "ParLevel3_Id", "dbo.ParLevel3");
            DropForeignKey("dbo.ParRelapse", "ParFrequency_Id", "dbo.ParFrequency");
            DropForeignKey("dbo.ParLevel2", "ParFrequency_Id", "dbo.ParFrequency");
            DropForeignKey("dbo.ParLevel1", "ParFrequency_Id", "dbo.ParFrequency");
            DropForeignKey("dbo.ParNotConformityRuleXLevel", "ParLevel3_Id", "dbo.ParLevel3");
            DropForeignKey("dbo.ParLevel3Value", "ParLevel3_Id", "dbo.ParLevel3");
            DropForeignKey("dbo.ParLevel3Level2", "ParLevel3_Id", "dbo.ParLevel3");
            DropForeignKey("dbo.ParLevel3EvaluationSample", "ParLevel3_Id", "dbo.ParLevel3");
            DropForeignKey("dbo.VolumeVacuoGRD", "ParCompany_id", "dbo.ParCompany");
            DropForeignKey("dbo.VolumePcc1b", "ParCompany_id", "dbo.ParCompany");
            DropForeignKey("dbo.VolumeCepRecortes", "ParCompany_id", "dbo.ParCompany");
            DropForeignKey("dbo.VolumeCepDesossa", "ParCompany_id", "dbo.ParCompany");
            DropForeignKey("dbo.ParSample", "ParCompany_Id", "dbo.ParCompany");
            DropForeignKey("dbo.ParNotConformityRuleXLevel", "ParCompany_Id", "dbo.ParCompany");
            DropForeignKey("dbo.ParNotConformityRuleXLevel", "ParNotConformityRule_Id", "dbo.ParNotConformityRule");
            DropForeignKey("dbo.ParMultipleValuesXParCompany", "ParCompany_Id", "dbo.ParCompany");
            DropForeignKey("dbo.ParMultipleValuesXParCompany", "ParMultipleValues_Id", "dbo.ParMultipleValues");
            DropForeignKey("dbo.ParMultipleValuesXParCompany", "Parent_ParMultipleValues_Id", "dbo.ParMultipleValues");
            DropForeignKey("dbo.ParLevel3Value", "ParCompany_Id", "dbo.ParCompany");
            DropForeignKey("dbo.ParLevel3Value", "ParMeasurementUnit_Id", "dbo.ParMeasurementUnit");
            DropForeignKey("dbo.ParLevel3Value", "ParLevel3InputType_Id", "dbo.ParLevel3InputType");
            DropForeignKey("dbo.ParLevel3Value", "ParLevel3BoolTrue_Id", "dbo.ParLevel3BoolTrue");
            DropForeignKey("dbo.ParLevel3Value", "ParLevel3BoolFalse_Id", "dbo.ParLevel3BoolFalse");
            DropForeignKey("dbo.ParLevel3Level2", "ParCompany_Id", "dbo.ParCompany");
            DropForeignKey("dbo.ParLevel3Level2Level1", "ParLevel3Level2_Id", "dbo.ParLevel3Level2");
            DropForeignKey("dbo.ParLevel3Level2", "ParLevel3Group_Id", "dbo.ParLevel3Group");
            DropForeignKey("dbo.ParLevel3EvaluationSample", "ParCompany_Id", "dbo.ParCompany");
            DropForeignKey("dbo.ParLevel2ControlCompany", "ParCompany_Id", "dbo.ParCompany");
            DropForeignKey("dbo.ParGoal", "ParCompany_Id", "dbo.ParCompany");
            DropForeignKey("dbo.ParEvaluation", "ParCompany_Id", "dbo.ParCompany");
            DropForeignKey("dbo.ParCompanyXUserSgq", "ParCompany_Id", "dbo.ParCompany");
            DropForeignKey("dbo.UserXRoles", "User_Id", "dbo.UserSgq");
            DropForeignKey("dbo.UserXRoles", "Role_Id", "dbo.RoleUserSgq");
            DropForeignKey("dbo.MenuXRoles", "Role_Id", "dbo.RoleUserSgq");
            DropForeignKey("dbo.MenuXRoles", "Menu_Id", "dbo.Menu");
            DropForeignKey("dbo.Menu", "GroupMenu_Id", "dbo.GroupMenu");
            DropForeignKey("dbo.UnitUser", "UserSgqId", "dbo.UserSgq");
            DropForeignKey("dbo.ParCompanyXUserSgq", "UserSgq_Id", "dbo.UserSgq");
            DropForeignKey("dbo.CorrectiveAction", "SlaughterId", "dbo.UserSgq");
            DropForeignKey("dbo.CorrectiveAction", "TechinicalId", "dbo.UserSgq");
            DropForeignKey("dbo.CorrectiveAction", "AuditorId", "dbo.UserSgq");
            DropForeignKey("dbo.Deviation", "EmailContent_Id", "dbo.EmailContent");
            DropForeignKey("dbo.CorrectiveAction", "EmailContent_Id", "dbo.EmailContent");
            DropForeignKey("dbo.CollectionLevel2", "AuditorId", "dbo.UserSgq");
            DropForeignKey("dbo.CollectionLevel02", "AuditorId", "dbo.UserSgq");
            DropForeignKey("dbo.ConsolidationLevel02", "Level02Id", "dbo.Level02");
            DropForeignKey("dbo.CollectionLevel02", "Level02Id", "dbo.Level02");
            DropForeignKey("dbo.UnitUser", "UnitId", "dbo.Unit");
            DropForeignKey("dbo.ConsolidationLevel01", "UnitId", "dbo.Unit");
            DropForeignKey("dbo.ConsolidationLevel01", "Level01Id", "dbo.Level01");
            DropForeignKey("dbo.CollectionLevel02", "Level01Id", "dbo.Level01");
            DropForeignKey("dbo.ConsolidationLevel1", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.ConsolidationLevel01", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.ConsolidationLevel02", "Level01ConsolidationId", "dbo.ConsolidationLevel01");
            DropForeignKey("dbo.CollectionLevel02", "ConsolidationLevel02Id", "dbo.ConsolidationLevel02");
            DropForeignKey("dbo.CollectionLevel03", "CollectionLevel02Id", "dbo.CollectionLevel02");
            DropForeignKey("dbo.CollectionLevel03", "Level03Id", "dbo.Level03");
            DropForeignKey("dbo.ParCompanyXStructure", "ParCompany_Id", "dbo.ParCompany");
            DropForeignKey("dbo.ParStructure", "ParStructureGroup_Id", "dbo.ParStructureGroup");
            DropForeignKey("dbo.ParCompanyXStructure", "ParStructure_Id", "dbo.ParStructure");
            DropForeignKey("dbo.ParCompanyCluster", "ParCompany_Id", "dbo.ParCompany");
            DropForeignKey("dbo.ParLevel1XCluster", "ParCluster_Id", "dbo.ParCluster");
            DropForeignKey("dbo.ParLevel1XCluster", "ParCriticalLevel_Id", "dbo.ParCriticalLevel");
            DropForeignKey("dbo.ParCompanyCluster", "ParCluster_Id", "dbo.ParCluster");
            DropForeignKey("dbo.ParClusterXModule", "ParCluster_Id", "dbo.ParCluster");
            DropForeignKey("dbo.ParModuleXModule", "ParModuleParent_Id", "dbo.ParModule");
            DropForeignKey("dbo.ParModuleXModule", "ParModuleChild_Id", "dbo.ParModule");
            DropForeignKey("dbo.ParClusterXModule", "ParModule_Id", "dbo.ParModule");
            DropForeignKey("dbo.ParCluster", "ParClusterGroup_Id", "dbo.ParClusterGroup");
            DropForeignKey("dbo.Defect", "ParCompany_Id", "dbo.ParCompany");
            DropForeignKey("dbo.ConsolidationLevel1", "UnitId", "dbo.ParCompany");
            DropForeignKey("dbo.ParCounterXLocal", "ParLevel3_Id", "dbo.ParLevel3");
            DropForeignKey("dbo.ParCounterXLocal", "ParCounter_Id", "dbo.ParCounter");
            DropForeignKey("dbo.ConsolidationLevel2", "ParLevel2_Id", "dbo.ParLevel2");
            DropForeignKey("dbo.CollectionLevel2", "ParLevel2_Id", "dbo.ParLevel2");
            DropForeignKey("dbo.CollectionLevel2", "ConsolidationLevel2_Id", "dbo.ConsolidationLevel2");
            DropForeignKey("dbo.CollectionLevel2", "ParLevel1_Id", "dbo.ParLevel1");
            DropForeignKey("dbo.CollectionLevel2XParHeaderField", "ParHeaderField_Id", "dbo.ParHeaderField");
            DropForeignKey("dbo.CollectionLevel2XParHeaderField", "ParFieldType_Id", "dbo.ParFieldType");
            DropForeignKey("dbo.CollectionLevel2XCollectionJson", "CollectionLevel2_Id", "dbo.CollectionLevel2");
            DropForeignKey("dbo.CollectionLevel2", "CollectionLevel22_Id", "dbo.CollectionLevel2");
            DropForeignKey("dbo.fa_CausaGenerica", "IdCausaGenerica", "dbo.CausaGenerica");
            DropForeignKey("dbo.fa_CausaEspecifica", "IdCausaEspecifica", "dbo.CausaEspecifica");
            DropForeignKey("dbo.CaracteristicaTipificacaoSequencial", "nCdCaracteristica_Id", "dbo.CaracteristicaTipificacao");
            DropForeignKey("dbo.VolumeProducao", "Operacao", "dbo.Operacoes");
            DropForeignKey("dbo.TipificacaoReal", "Operacao", "dbo.Operacoes");
            DropForeignKey("dbo.Tarefas", "Operacao", "dbo.Operacoes");
            DropForeignKey("dbo.TarefaAvaliacoes", "Operacao", "dbo.Operacoes");
            DropForeignKey("dbo.Pontos", "Operacao", "dbo.Operacoes");
            DropForeignKey("dbo.Pontos", "Cluster", "dbo.Clusters");
            DropForeignKey("dbo.PacotesOperacoes", "Operacao", "dbo.Operacoes");
            DropForeignKey("dbo.PacotesOperacoes", "Pacote", "dbo.Pacotes");
            DropForeignKey("dbo.ObservacoesPadroes", "Operacao", "dbo.Operacoes");
            DropForeignKey("dbo.MonitoramentosConcorrentes", "OperacaoId", "dbo.Operacoes");
            DropForeignKey("dbo.Metas", "Operacao", "dbo.Operacoes");
            DropForeignKey("dbo.Horarios", "OperacaoId", "dbo.Operacoes");
            DropForeignKey("dbo.FamiliaProdutos", "Operacao", "dbo.Operacoes");
            DropForeignKey("dbo.DepartamentoOperacoes", "Operacao", "dbo.Operacoes");
            DropForeignKey("dbo.Alertas", "Operacao", "dbo.Operacoes");
            DropForeignKey("dbo.TarefaMonitoramentos", "Tarefa", "dbo.Tarefas");
            DropForeignKey("dbo.TarefaCategorias", "Tarefa", "dbo.Tarefas");
            DropForeignKey("dbo.TarefaAvaliacoes", "Tarefa", "dbo.Tarefas");
            DropForeignKey("dbo.TarefaAmostras", "Tarefa", "dbo.Tarefas");
            DropForeignKey("dbo.ObservacoesPadroes", "Tarefa", "dbo.Tarefas");
            DropForeignKey("dbo.MonitoramentosConcorrentes", "TarefaId", "dbo.Tarefas");
            DropForeignKey("dbo.VolumeProducao", "Departamento", "dbo.Departamentos");
            DropForeignKey("dbo.Tarefas", "Departamento", "dbo.Departamentos");
            DropForeignKey("dbo.TarefaAvaliacoes", "Departamento", "dbo.Departamentos");
            DropForeignKey("dbo.ObservacoesPadroes", "Departamento", "dbo.Departamentos");
            DropForeignKey("dbo.DepartamentoProdutos", "Departamento", "dbo.Departamentos");
            DropForeignKey("dbo.Tarefas", "Produto", "dbo.Produtos");
            DropForeignKey("dbo.FamiliaProdutos", "Produto", "dbo.Produtos");
            DropForeignKey("dbo.VolumeProducao", "Unidade", "dbo.Unidades");
            DropForeignKey("dbo.VerificacaoTipificacao", "UnidadeId", "dbo.Unidades");
            DropForeignKey("dbo.UsuarioUnidades", "Unidade", "dbo.Unidades");
            DropForeignKey("dbo.Usuarios", "Unidade", "dbo.Unidades");
            DropForeignKey("dbo.TipificacaoReal", "Unidade", "dbo.Unidades");
            DropForeignKey("dbo.TarefaMonitoramentos", "Unidade", "dbo.Unidades");
            DropForeignKey("dbo.TarefaAvaliacoes", "Unidade", "dbo.Unidades");
            DropForeignKey("dbo.TarefaAmostras", "Unidade", "dbo.Unidades");
            DropForeignKey("dbo.PadraoMonitoramentos", "Unidade", "dbo.Unidades");
            DropForeignKey("dbo.MonitoramentosConcorrentes", "UnidadeId", "dbo.Unidades");
            DropForeignKey("dbo.VerificacaoTipificacaoTarefaIntegracao", "TarefaId", "dbo.Monitoramentos");
            DropForeignKey("dbo.TarefaMonitoramentos", "Monitoramento", "dbo.Monitoramentos");
            DropForeignKey("dbo.PadraoTolerancias", "Monitoramento", "dbo.Monitoramentos");
            DropForeignKey("dbo.PadraoMonitoramentos", "Monitoramento", "dbo.Monitoramentos");
            DropForeignKey("dbo.PadraoMonitoramentos", "UnidadeMedida", "dbo.UnidadesMedidas");
            DropForeignKey("dbo.PadraoMonitoramentos", "UnidadeMedidaLegenda", "dbo.UnidadesMedidas");
            DropForeignKey("dbo.PadraoTolerancias", "PadraoNivel3", "dbo.Padroes");
            DropForeignKey("dbo.PadraoTolerancias", "PadraoNivel1", "dbo.Padroes");
            DropForeignKey("dbo.PadraoMonitoramentos", "Padrao", "dbo.Padroes");
            DropForeignKey("dbo.MonitoramentosConcorrentes", "MonitoramentoId", "dbo.Monitoramentos");
            DropForeignKey("dbo.MonitoramentosConcorrentes", "ConcorrenteId", "dbo.Monitoramentos");
            DropForeignKey("dbo.MonitoramentoEquipamentos", "Monitoramento", "dbo.Monitoramentos");
            DropForeignKey("dbo.GrupoTipoAvaliacaoMonitoramentos", "Monitoramento", "dbo.Monitoramentos");
            DropForeignKey("dbo.GrupoTipoAvaliacoes", "Positivo", "dbo.TipoAvaliacoes");
            DropForeignKey("dbo.GrupoTipoAvaliacoes", "Negativo", "dbo.TipoAvaliacoes");
            DropForeignKey("dbo.GrupoTipoAvaliacaoMonitoramentos", "GrupoTipoAvaliacao", "dbo.GrupoTipoAvaliacoes");
            DropForeignKey("dbo.Horarios", "UnidadeId", "dbo.Unidades");
            DropForeignKey("dbo.GrupoProjeto", "IdEmpresa", "dbo.Unidades");
            DropForeignKey("dbo.Projeto", "IdGrupoProjeto", "dbo.GrupoProjeto");
            DropForeignKey("dbo.VinculoParticipanteProjeto", "IdProjeto", "dbo.Projeto");
            DropForeignKey("dbo.TarefaPA", "IdProjeto", "dbo.Projeto");
            DropForeignKey("dbo.GrupoCabecalho", "IdProjeto", "dbo.Projeto");
            DropForeignKey("dbo.Campo", "IdProjeto", "dbo.Projeto");
            DropForeignKey("dbo.Cabecalho", "IdProjeto", "dbo.Projeto");
            DropForeignKey("dbo.VinculoCampoCabecalho", "IdCabecalho", "dbo.Cabecalho");
            DropForeignKey("dbo.TarefaPA", "IdCabecalho", "dbo.Cabecalho");
            DropForeignKey("dbo.VinculoCampoTarefa", "IdTarefa", "dbo.TarefaPA");
            DropForeignKey("dbo.AcompanhamentoTarefa", "IdTarefa", "dbo.TarefaPA");
            DropForeignKey("dbo.VinculoParticipanteProjeto", "IdParticipante", "dbo.Usuarios");
            DropForeignKey("dbo.VinculoParticipanteMultiplaEscolha", "IdParticipante", "dbo.Usuarios");
            DropForeignKey("dbo.VinculoCampoTarefa", "IdParticipante", "dbo.Usuarios");
            DropForeignKey("dbo.VinculoCampoCabecalho", "IdParticipante", "dbo.Usuarios");
            DropForeignKey("dbo.VinculoCampoTarefa", "IdCampo", "dbo.Campo");
            DropForeignKey("dbo.VinculoCampoCabecalho", "IdCampo", "dbo.Campo");
            DropForeignKey("dbo.MultiplaEscolha", "IdCampo", "dbo.Campo");
            DropForeignKey("dbo.VinculoParticipanteMultiplaEscolha", "IdMultiplaEscolha", "dbo.MultiplaEscolha");
            DropForeignKey("dbo.VinculoCampoTarefa", "IdMultiplaEscolha", "dbo.MultiplaEscolha");
            DropForeignKey("dbo.VinculoCampoCabecalho", "IdMultiplaEscolha", "dbo.MultiplaEscolha");
            DropForeignKey("dbo.VinculoCampoCabecalho", "IdGrupoCabecalho", "dbo.GrupoCabecalho");
            DropForeignKey("dbo.Campo", "IdGrupoCabecalho", "dbo.GrupoCabecalho");
            DropForeignKey("dbo.UsuarioUnidades", "Usuario", "dbo.Usuarios");
            DropForeignKey("dbo.TarefaPA", "IdParticipanteCriador", "dbo.Usuarios");
            DropForeignKey("dbo.Usuarios", "Regional", "dbo.Regionais");
            DropForeignKey("dbo.PlanoDeAcaoQuemQuandoComo", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.fa_PlanoDeAcaoQuemQuandoComo", "IdPlanoDeAcaoQuemQuandoComo", "dbo.PlanoDeAcaoQuemQuandoComo");
            DropForeignKey("dbo.NiveisUsuarios", "Usuario", "dbo.Usuarios");
            DropForeignKey("dbo.NiveisUsuarios", "Nivel", "dbo.Niveis");
            DropForeignKey("dbo.Cabecalho", "IdParticipanteCriador", "dbo.Usuarios");
            DropForeignKey("dbo.AcompanhamentoTarefa", "IdParticipanteEnvio", "dbo.Usuarios");
            DropForeignKey("dbo.FamiliaProdutos", "Unidade", "dbo.Unidades");
            DropForeignKey("dbo.Equipamentos", "Unidade", "dbo.Unidades");
            DropForeignKey("dbo.Alertas", "Unidade", "dbo.Unidades");
            DropForeignKey("dbo.DepartamentoProdutos", "Produto", "dbo.Produtos");
            DropForeignKey("dbo.CategoriaProdutos", "Produto", "dbo.Produtos");
            DropForeignKey("dbo.TarefaCategorias", "Categoria", "dbo.Categorias");
            DropForeignKey("dbo.CategoriaProdutos", "Categoria", "dbo.Categorias");
            DropForeignKey("dbo.DepartamentoOperacoes", "Departamento", "dbo.Departamentos");
            DropForeignKey("dbo.Alertas", "Tarefa", "dbo.Tarefas");
            DropForeignKey("dbo.AcoesCorretivas", "Tarefa", "dbo.Tarefas");
            DropForeignKey("dbo.AcoesCorretivas", "Operacao", "dbo.Operacoes");
            DropIndex("dbo.pa_vinculo_participante_projeto", new[] { "id_participante" });
            DropIndex("dbo.pa_vinculo_participante_projeto", new[] { "id_projeto" });
            DropIndex("dbo.pa_vinculo_participante_multipla_escolha", new[] { "id_multipla_escolha" });
            DropIndex("dbo.pa_vinculo_participante_multipla_escolha", new[] { "id_participante" });
            DropIndex("dbo.pa_vinculo_campo_tarefa", new[] { "id_participante" });
            DropIndex("dbo.pa_vinculo_campo_tarefa", new[] { "id_tarefa" });
            DropIndex("dbo.pa_vinculo_campo_tarefa", new[] { "id_campo" });
            DropIndex("dbo.pa_vinculo_campo_tarefa", new[] { "id_multipla_escolha" });
            DropIndex("dbo.pa_multipla_escolha", new[] { "id_campo" });
            DropIndex("dbo.pa_campo", new[] { "id_projeto" });
            DropIndex("dbo.pa_projeto", new[] { "id_grupo_projeto" });
            DropIndex("dbo.pa_projeto", new[] { "id_empresa" });
            DropIndex("dbo.pa_grupo_projeto", new[] { "id_empresa" });
            DropIndex("dbo.pa_participante", new[] { "id_empresa" });
            DropIndex("dbo.pa_tarefa", new[] { "id_participante_criador" });
            DropIndex("dbo.pa_tarefa", new[] { "id_projeto" });
            DropIndex("dbo.pa_acompanhamento_tarefa", new[] { "id_tarefa" });
            DropIndex("HangFire.State", new[] { "JobId" });
            DropIndex("HangFire.JobParameter", new[] { "JobId" });
            DropIndex("dbo.State", new[] { "JobId" });
            DropIndex("dbo.JobParameter", new[] { "JobId" });
            DropIndex("dbo.fa_GrupoCausa", new[] { "IdGrupoCausa" });
            DropIndex("dbo.fa_ContramedidaGenerica", new[] { "IdContramedidaGenerica" });
            DropIndex("dbo.fa_ContramedidaEspecifica", new[] { "IdContramedidaEspecifica" });
            DropIndex("dbo.ParLevel2Level1", new[] { "ParLevel2_Id" });
            DropIndex("dbo.ParLevel2Level1", new[] { "ParLevel1_Id" });
            DropIndex("dbo.Result_Level3", new[] { "ParLevel3_Id" });
            DropIndex("dbo.Result_Level3", new[] { "CollectionLevel2_Id" });
            DropIndex("dbo.ParRelapse", new[] { "ParFrequency_Id" });
            DropIndex("dbo.ParRelapse", new[] { "ParLevel3_Id" });
            DropIndex("dbo.ParRelapse", new[] { "ParLevel2_Id" });
            DropIndex("dbo.ParRelapse", new[] { "ParLevel1_Id" });
            DropIndex("dbo.VolumeVacuoGRD", new[] { "ParLevel1_id" });
            DropIndex("dbo.VolumeVacuoGRD", new[] { "ParCompany_id" });
            DropIndex("dbo.VolumePcc1b", new[] { "ParLevel1_id" });
            DropIndex("dbo.VolumePcc1b", new[] { "ParCompany_id" });
            DropIndex("dbo.VolumeCepRecortes", new[] { "ParLevel1_id" });
            DropIndex("dbo.VolumeCepRecortes", new[] { "ParCompany_id" });
            DropIndex("dbo.VolumeCepDesossa", new[] { "ParLevel1_id" });
            DropIndex("dbo.VolumeCepDesossa", new[] { "ParCompany_id" });
            DropIndex("dbo.ParSample", new[] { "ParLevel2_Id" });
            DropIndex("dbo.ParSample", new[] { "ParCompany_Id" });
            DropIndex("dbo.ParNotConformityRuleXLevel", new[] { "ParLevel3_Id" });
            DropIndex("dbo.ParNotConformityRuleXLevel", new[] { "ParLevel2_Id" });
            DropIndex("dbo.ParNotConformityRuleXLevel", new[] { "ParLevel1_Id" });
            DropIndex("dbo.ParNotConformityRuleXLevel", new[] { "ParCompany_Id" });
            DropIndex("dbo.ParNotConformityRuleXLevel", new[] { "ParNotConformityRule_Id" });
            DropIndex("dbo.ParMultipleValues", new[] { "ParHeaderField_Id" });
            DropIndex("dbo.ParMultipleValuesXParCompany", new[] { "ParHeaderField_Id" });
            DropIndex("dbo.ParMultipleValuesXParCompany", new[] { "ParLevel1_Id" });
            DropIndex("dbo.ParMultipleValuesXParCompany", new[] { "ParCompany_Id" });
            DropIndex("dbo.ParMultipleValuesXParCompany", new[] { "Parent_ParMultipleValues_Id" });
            DropIndex("dbo.ParMultipleValuesXParCompany", new[] { "ParMultipleValues_Id" });
            DropIndex("dbo.ParLevel3Value", new[] { "ParLevel2_Id" });
            DropIndex("dbo.ParLevel3Value", new[] { "ParLevel1_Id" });
            DropIndex("dbo.ParLevel3Value", new[] { "ParMeasurementUnit_Id" });
            DropIndex("dbo.ParLevel3Value", new[] { "ParCompany_Id" });
            DropIndex("dbo.ParLevel3Value", new[] { "ParLevel3BoolTrue_Id" });
            DropIndex("dbo.ParLevel3Value", new[] { "ParLevel3BoolFalse_Id" });
            DropIndex("dbo.ParLevel3Value", new[] { "ParLevel3InputType_Id" });
            DropIndex("dbo.ParLevel3Value", new[] { "ParLevel3_Id" });
            DropIndex("dbo.ParLevel3Level2Level1", new[] { "ParLevel1_Id" });
            DropIndex("dbo.ParLevel3Level2Level1", new[] { "ParLevel3Level2_Id" });
            DropIndex("dbo.ParLevel3Group", new[] { "ParLevel2_Id" });
            DropIndex("dbo.ParLevel3Level2", new[] { "ParCompany_Id" });
            DropIndex("dbo.ParLevel3Level2", new[] { "ParLevel3Group_Id" });
            DropIndex("dbo.ParLevel3Level2", new[] { "ParLevel3_Id" });
            DropIndex("dbo.ParLevel3Level2", new[] { "ParLevel2_Id" });
            DropIndex("dbo.ParLevel2ControlCompany", new[] { "ParLevel2_Id" });
            DropIndex("dbo.ParLevel2ControlCompany", new[] { "ParLevel1_Id" });
            DropIndex("dbo.ParLevel2ControlCompany", new[] { "ParCompany_Id" });
            DropIndex("dbo.ParGoal", new[] { "ParCompany_Id" });
            DropIndex("dbo.ParGoal", new[] { "ParLevel1_Id" });
            DropIndex("dbo.ParEvaluation", new[] { "ParLevel2_Id" });
            DropIndex("dbo.ParEvaluation", new[] { "ParCompany_Id" });
            DropIndex("dbo.Menu", new[] { "GroupMenu_Id" });
            DropIndex("dbo.MenuXRoles", new[] { "Role_Id" });
            DropIndex("dbo.MenuXRoles", new[] { "Menu_Id" });
            DropIndex("dbo.UserXRoles", new[] { "Role_Id" });
            DropIndex("dbo.UserXRoles", new[] { "User_Id" });
            DropIndex("dbo.Deviation", new[] { "EmailContent_Id" });
            DropIndex("dbo.CorrectiveAction", new[] { "EmailContent_Id" });
            DropIndex("dbo.CorrectiveAction", new[] { "TechinicalId" });
            DropIndex("dbo.CorrectiveAction", new[] { "SlaughterId" });
            DropIndex("dbo.CorrectiveAction", new[] { "CollectionLevel02Id" });
            DropIndex("dbo.CorrectiveAction", new[] { "AuditorId" });
            DropIndex("dbo.UnitUser", new[] { "UnitId" });
            DropIndex("dbo.UnitUser", new[] { "UserSgqId" });
            DropIndex("dbo.ConsolidationLevel01", new[] { "Level01Id" });
            DropIndex("dbo.ConsolidationLevel01", new[] { "DepartmentId" });
            DropIndex("dbo.ConsolidationLevel01", new[] { "UnitId" });
            DropIndex("dbo.ConsolidationLevel02", new[] { "Level02Id" });
            DropIndex("dbo.ConsolidationLevel02", new[] { "Level01ConsolidationId" });
            DropIndex("dbo.CollectionLevel03", new[] { "Level03Id" });
            DropIndex("dbo.CollectionLevel03", new[] { "CollectionLevel02Id" });
            DropIndex("dbo.CollectionLevel02", new[] { "AuditorId" });
            DropIndex("dbo.CollectionLevel02", new[] { "Level02Id" });
            DropIndex("dbo.CollectionLevel02", new[] { "Level01Id" });
            DropIndex("dbo.CollectionLevel02", new[] { "ConsolidationLevel02Id" });
            DropIndex("dbo.ParCompanyXUserSgq", new[] { "ParCompany_Id" });
            DropIndex("dbo.ParCompanyXUserSgq", new[] { "UserSgq_Id" });
            DropIndex("dbo.ParStructure", new[] { "ParStructureGroup_Id" });
            DropIndex("dbo.ParCompanyXStructure", new[] { "ParCompany_Id" });
            DropIndex("dbo.ParCompanyXStructure", new[] { "ParStructure_Id" });
            DropIndex("dbo.ParLevel1XCluster", new[] { "ParCriticalLevel_Id" });
            DropIndex("dbo.ParLevel1XCluster", new[] { "ParCluster_Id" });
            DropIndex("dbo.ParLevel1XCluster", new[] { "ParLevel1_Id" });
            DropIndex("dbo.ParModuleXModule", new[] { "ParModuleChild_Id" });
            DropIndex("dbo.ParModuleXModule", new[] { "ParModuleParent_Id" });
            DropIndex("dbo.ParClusterXModule", new[] { "ParModule_Id" });
            DropIndex("dbo.ParClusterXModule", new[] { "ParCluster_Id" });
            DropIndex("dbo.ParCluster", new[] { "ParClusterGroup_Id" });
            DropIndex("dbo.ParCompanyCluster", new[] { "ParCluster_Id" });
            DropIndex("dbo.ParCompanyCluster", new[] { "ParCompany_Id" });
            DropIndex("dbo.Defect", new[] { "ParLevel1_Id" });
            DropIndex("dbo.Defect", new[] { "ParCompany_Id" });
            DropIndex("dbo.ParLevel3EvaluationSample", new[] { "ParLevel3_Id" });
            DropIndex("dbo.ParLevel3EvaluationSample", new[] { "ParLevel2_Id" });
            DropIndex("dbo.ParLevel3EvaluationSample", new[] { "ParLevel1_Id" });
            DropIndex("dbo.ParLevel3EvaluationSample", new[] { "ParCompany_Id" });
            DropIndex("dbo.ParCounterXLocal", new[] { "ParLevel3_Id" });
            DropIndex("dbo.ParCounterXLocal", new[] { "ParLevel2_Id" });
            DropIndex("dbo.ParCounterXLocal", new[] { "ParLevel1_Id" });
            DropIndex("dbo.ParCounterXLocal", new[] { "ParCounter_Id" });
            DropIndex("dbo.ParCounterXLocal", new[] { "ParLocal_Id" });
            DropIndex("dbo.ParLevel2", new[] { "ParDepartment_Id" });
            DropIndex("dbo.ParLevel2", new[] { "ParFrequency_Id" });
            DropIndex("dbo.ConsolidationLevel2", new[] { "ParLevel2_Id" });
            DropIndex("dbo.ConsolidationLevel2", new[] { "ConsolidationLevel1_Id" });
            DropIndex("dbo.ConsolidationLevel1", new[] { "ParLevel1_Id" });
            DropIndex("dbo.ConsolidationLevel1", new[] { "DepartmentId" });
            DropIndex("dbo.ConsolidationLevel1", new[] { "UnitId" });
            DropIndex("dbo.ParLevel1", new[] { "ParScoreType_Id" });
            DropIndex("dbo.ParLevel1", new[] { "ParFrequency_Id" });
            DropIndex("dbo.ParLevel1", new[] { "ParConsolidationType_Id" });
            DropIndex("dbo.ParLevel1XHeaderField", new[] { "ParHeaderField_Id" });
            DropIndex("dbo.ParLevel1XHeaderField", new[] { "ParLevel1_Id" });
            DropIndex("dbo.ParHeaderField", new[] { "ParLevelDefinition_Id" });
            DropIndex("dbo.ParHeaderField", new[] { "ParFieldType_Id" });
            DropIndex("dbo.CollectionLevel2XParHeaderField", new[] { "ParFieldType_Id" });
            DropIndex("dbo.CollectionLevel2XParHeaderField", new[] { "ParHeaderField_Id" });
            DropIndex("dbo.CollectionLevel2XParHeaderField", new[] { "CollectionLevel2_Id" });
            DropIndex("dbo.CollectionLevel2", new[] { "CollectionLevel22_Id" });
            DropIndex("dbo.CollectionLevel2", new[] { "AuditorId" });
            DropIndex("dbo.CollectionLevel2", new[] { "ParLevel2_Id" });
            DropIndex("dbo.CollectionLevel2", new[] { "ParLevel1_Id" });
            DropIndex("dbo.CollectionLevel2", new[] { "ConsolidationLevel2_Id" });
            DropIndex("dbo.CollectionLevel2XCollectionJson", new[] { "CollectionJson_Id" });
            DropIndex("dbo.CollectionLevel2XCollectionJson", new[] { "CollectionLevel2_Id" });
            DropIndex("dbo.fa_CausaGenerica", new[] { "IdCausaGenerica" });
            DropIndex("dbo.fa_CausaEspecifica", new[] { "IdCausaEspecifica" });
            DropIndex("dbo.CaracteristicaTipificacaoSequencial", new[] { "nCdCaracteristica_Id" });
            DropIndex("dbo.Pontos", new[] { "Operacao" });
            DropIndex("dbo.Pontos", new[] { "Cluster" });
            DropIndex("dbo.PacotesOperacoes", new[] { "Operacao" });
            DropIndex("dbo.PacotesOperacoes", new[] { "Pacote" });
            DropIndex("dbo.Metas", new[] { "Operacao" });
            DropIndex("dbo.ObservacoesPadroes", new[] { "Tarefa" });
            DropIndex("dbo.ObservacoesPadroes", new[] { "Operacao" });
            DropIndex("dbo.ObservacoesPadroes", new[] { "Departamento" });
            DropIndex("dbo.VolumeProducao", new[] { "Departamento" });
            DropIndex("dbo.VolumeProducao", new[] { "Unidade" });
            DropIndex("dbo.VolumeProducao", new[] { "Operacao" });
            DropIndex("dbo.VerificacaoTipificacao", new[] { "UnidadeId" });
            DropIndex("dbo.TipificacaoReal", new[] { "Unidade" });
            DropIndex("dbo.TipificacaoReal", new[] { "Operacao" });
            DropIndex("dbo.TarefaAvaliacoes", new[] { "Unidade" });
            DropIndex("dbo.TarefaAvaliacoes", new[] { "Tarefa" });
            DropIndex("dbo.TarefaAvaliacoes", new[] { "Operacao" });
            DropIndex("dbo.TarefaAvaliacoes", new[] { "Departamento" });
            DropIndex("dbo.TarefaAmostras", new[] { "Unidade" });
            DropIndex("dbo.TarefaAmostras", new[] { "Tarefa" });
            DropIndex("dbo.VerificacaoTipificacaoTarefaIntegracao", new[] { "TarefaId" });
            DropIndex("dbo.TarefaMonitoramentos", new[] { "Unidade" });
            DropIndex("dbo.TarefaMonitoramentos", new[] { "Monitoramento" });
            DropIndex("dbo.TarefaMonitoramentos", new[] { "Tarefa" });
            DropIndex("dbo.PadraoTolerancias", new[] { "PadraoNivel3" });
            DropIndex("dbo.PadraoTolerancias", new[] { "PadraoNivel1" });
            DropIndex("dbo.PadraoTolerancias", new[] { "Monitoramento" });
            DropIndex("dbo.PadraoMonitoramentos", new[] { "UnidadeMedidaLegenda" });
            DropIndex("dbo.PadraoMonitoramentos", new[] { "UnidadeMedida" });
            DropIndex("dbo.PadraoMonitoramentos", new[] { "Unidade" });
            DropIndex("dbo.PadraoMonitoramentos", new[] { "Monitoramento" });
            DropIndex("dbo.PadraoMonitoramentos", new[] { "Padrao" });
            DropIndex("dbo.MonitoramentoEquipamentos", new[] { "Monitoramento" });
            DropIndex("dbo.GrupoTipoAvaliacoes", new[] { "Negativo" });
            DropIndex("dbo.GrupoTipoAvaliacoes", new[] { "Positivo" });
            DropIndex("dbo.GrupoTipoAvaliacaoMonitoramentos", new[] { "Monitoramento" });
            DropIndex("dbo.GrupoTipoAvaliacaoMonitoramentos", new[] { "GrupoTipoAvaliacao" });
            DropIndex("dbo.MonitoramentosConcorrentes", new[] { "ConcorrenteId" });
            DropIndex("dbo.MonitoramentosConcorrentes", new[] { "MonitoramentoId" });
            DropIndex("dbo.MonitoramentosConcorrentes", new[] { "TarefaId" });
            DropIndex("dbo.MonitoramentosConcorrentes", new[] { "OperacaoId" });
            DropIndex("dbo.MonitoramentosConcorrentes", new[] { "UnidadeId" });
            DropIndex("dbo.Horarios", new[] { "OperacaoId" });
            DropIndex("dbo.Horarios", new[] { "UnidadeId" });
            DropIndex("dbo.VinculoParticipanteProjeto", new[] { "IdParticipante" });
            DropIndex("dbo.VinculoParticipanteProjeto", new[] { "IdProjeto" });
            DropIndex("dbo.VinculoParticipanteMultiplaEscolha", new[] { "IdMultiplaEscolha" });
            DropIndex("dbo.VinculoParticipanteMultiplaEscolha", new[] { "IdParticipante" });
            DropIndex("dbo.VinculoCampoTarefa", new[] { "IdParticipante" });
            DropIndex("dbo.VinculoCampoTarefa", new[] { "IdTarefa" });
            DropIndex("dbo.VinculoCampoTarefa", new[] { "IdCampo" });
            DropIndex("dbo.VinculoCampoTarefa", new[] { "IdMultiplaEscolha" });
            DropIndex("dbo.MultiplaEscolha", new[] { "IdCampo" });
            DropIndex("dbo.GrupoCabecalho", new[] { "IdProjeto" });
            DropIndex("dbo.Campo", new[] { "IdGrupoCabecalho" });
            DropIndex("dbo.Campo", new[] { "IdProjeto" });
            DropIndex("dbo.VinculoCampoCabecalho", new[] { "IdGrupoCabecalho" });
            DropIndex("dbo.VinculoCampoCabecalho", new[] { "IdParticipante" });
            DropIndex("dbo.VinculoCampoCabecalho", new[] { "IdCabecalho" });
            DropIndex("dbo.VinculoCampoCabecalho", new[] { "IdCampo" });
            DropIndex("dbo.VinculoCampoCabecalho", new[] { "IdMultiplaEscolha" });
            DropIndex("dbo.UsuarioUnidades", new[] { "Unidade" });
            DropIndex("dbo.UsuarioUnidades", new[] { "Usuario" });
            DropIndex("dbo.fa_PlanoDeAcaoQuemQuandoComo", new[] { "IdPlanoDeAcaoQuemQuandoComo" });
            DropIndex("dbo.PlanoDeAcaoQuemQuandoComo", new[] { "IdUsuario" });
            DropIndex("dbo.NiveisUsuarios", new[] { "Nivel" });
            DropIndex("dbo.NiveisUsuarios", new[] { "Usuario" });
            DropIndex("dbo.Usuarios", new[] { "Regional" });
            DropIndex("dbo.Usuarios", new[] { "Unidade" });
            DropIndex("dbo.AcompanhamentoTarefa", new[] { "IdParticipanteEnvio" });
            DropIndex("dbo.AcompanhamentoTarefa", new[] { "IdTarefa" });
            DropIndex("dbo.TarefaPA", new[] { "IdCabecalho" });
            DropIndex("dbo.TarefaPA", new[] { "IdParticipanteCriador" });
            DropIndex("dbo.TarefaPA", new[] { "IdProjeto" });
            DropIndex("dbo.Cabecalho", new[] { "IdParticipanteCriador" });
            DropIndex("dbo.Cabecalho", new[] { "IdProjeto" });
            DropIndex("dbo.Projeto", new[] { "IdGrupoProjeto" });
            DropIndex("dbo.GrupoProjeto", new[] { "IdEmpresa" });
            DropIndex("dbo.Equipamentos", new[] { "Unidade" });
            DropIndex("dbo.FamiliaProdutos", new[] { "Produto" });
            DropIndex("dbo.FamiliaProdutos", new[] { "Unidade" });
            DropIndex("dbo.FamiliaProdutos", new[] { "Operacao" });
            DropIndex("dbo.TarefaCategorias", new[] { "Categoria" });
            DropIndex("dbo.TarefaCategorias", new[] { "Tarefa" });
            DropIndex("dbo.CategoriaProdutos", new[] { "Produto" });
            DropIndex("dbo.CategoriaProdutos", new[] { "Categoria" });
            DropIndex("dbo.DepartamentoProdutos", new[] { "Produto" });
            DropIndex("dbo.DepartamentoProdutos", new[] { "Departamento" });
            DropIndex("dbo.DepartamentoOperacoes", new[] { "Operacao" });
            DropIndex("dbo.DepartamentoOperacoes", new[] { "Departamento" });
            DropIndex("dbo.Tarefas", new[] { "Produto" });
            DropIndex("dbo.Tarefas", new[] { "Departamento" });
            DropIndex("dbo.Tarefas", new[] { "Operacao" });
            DropIndex("dbo.Alertas", new[] { "Tarefa" });
            DropIndex("dbo.Alertas", new[] { "Operacao" });
            DropIndex("dbo.Alertas", new[] { "Unidade" });
            DropIndex("dbo.AcoesCorretivas", new[] { "Tarefa" });
            DropIndex("dbo.AcoesCorretivas", new[] { "Operacao" });
            DropTable("dbo.Z_Sistema");
            DropTable("dbo.VWCFFResults");
            DropTable("dbo.VTVerificacaoTipificacaoValidacao");
            DropTable("dbo.VTVerificacaoTipificacaoTarefaIntegracao");
            DropTable("dbo.VTVerificacaoTipificacaoResultados");
            DropTable("dbo.VTVerificacaoTipificacaoJBS");
            DropTable("dbo.VTVerificacaoTipificacaoComparacao");
            DropTable("dbo.VTVerificacaoTipificacao");
            DropTable("dbo.VTVerificacaoContagem");
            DropTable("dbo.VolumeAbate");
            DropTable("dbo.VerificacaoTipificacaoValidacao");
            DropTable("dbo.VerificacaoTipificacaoV2");
            DropTable("dbo.VerificacaoTipificacaoResultados");
            DropTable("dbo.VerificacaoTipificacaoJBS");
            DropTable("dbo.VerificacaoTipificacaoComparacao");
            DropTable("dbo.VerificacaoContagem");
            DropTable("dbo.VacuoGRD");
            DropTable("dbo.UsuarioPerfilEmpresa");
            DropTable("dbo.Usuario");
            DropTable("dbo.Tipificacao");
            DropTable("dbo.Terceiro");
            DropTable("dbo.Sugestoes");
            DropTable("dbo.Shift");
            DropTable("dbo.SgqConfig2");
            DropTable("dbo.SgqConfig");
            DropTable("HangFire.Set");
            DropTable("dbo.Set");
            DropTable("HangFire.Server");
            DropTable("dbo.Server");
            DropTable("dbo.ScreenComponent");
            DropTable("dbo.ScorecardJBS_V");
            DropTable("dbo.ScorecardConsolidadoDia");
            DropTable("HangFire.Schema");
            DropTable("dbo.Schema");
            DropTable("dbo.RoleUserSgqXItemMenu");
            DropTable("dbo.RoleType");
            DropTable("dbo.RoleSGQ");
            DropTable("dbo.RoleJBS");
            DropTable("dbo.RetornoParaTablet");
            DropTable("dbo.ResultLevel2HeaderField");
            DropTable("dbo.ResultadosPCC");
            DropTable("dbo.ResultadosData");
            DropTable("dbo.Resultados");
            DropTable("dbo.Result_Level3_Photos");
            DropTable("dbo.Reports_HTP_Audit");
            DropTable("dbo.Reports_CFF_Audit");
            DropTable("dbo.Reports_CCA_Audit");
            DropTable("dbo.RecravacaoJson");
            DropTable("dbo.ProdutosAvaliados");
            DropTable("dbo.ProdutoInNatura");
            DropTable("dbo.Produto");
            DropTable("dbo.Period");
            DropTable("dbo.Perfil");
            DropTable("dbo.PenalidadeReincidencia");
            DropTable("dbo.Pcc1b");
            DropTable("dbo.ParReports");
            DropTable("dbo.ParRecravacao_TipoLata");
            DropTable("dbo.ParRecravacao_Linhas");
            DropTable("dbo.ParLevel3Value_Outer");
            DropTable("dbo.ParLevel2XHeaderField");
            DropTable("dbo.ParLevel1VariableProductionXLevel1");
            DropTable("dbo.ParLevel1VariableProduction");
            DropTable("dbo.ParLataImagens");
            DropTable("dbo.ParGroupParLevel1XParLevel1");
            DropTable("dbo.ParGroupParLevel1Type");
            DropTable("dbo.ParGroupParLevel1");
            DropTable("dbo.ParGoalScorecard");
            DropTable("dbo.ParConfSGQ");
            DropTable("dbo.ParCalendar");
            DropTable("dbo.Pa_Visao");
            DropTable("dbo.Pa_UnidadeMedida");
            DropTable("dbo.Pa_Unidade");
            DropTable("dbo.Pa_TipoProjeto");
            DropTable("dbo.Pa_TemaProjeto");
            DropTable("dbo.Pa_TemaAssunto");
            DropTable("dbo.Pa_Status");
            DropTable("dbo.Pa_Query");
            DropTable("dbo.Pa_Quem");
            DropTable("dbo.Pa_Problema_Desvio");
            DropTable("dbo.Pa_Planejamento");
            DropTable("dbo.Pa_ObjetivoGeral");
            DropTable("dbo.Pa_Objetivo");
            DropTable("dbo.Pa_Missao");
            DropTable("dbo.pa_log_operacao");
            DropTable("dbo.Pa_Iniciativa");
            DropTable("dbo.Pa_IndicadorSgqAcao");
            DropTable("dbo.Pa_IndicadoresDiretriz");
            DropTable("dbo.Pa_IndicadoresDeProjeto");
            DropTable("dbo.Pa_Indicadores");
            DropTable("dbo.Pa_GrupoCausa");
            DropTable("dbo.Pa_Gerencia");
            DropTable("dbo.Pa_Diretoria");
            DropTable("dbo.Pa_Dimensao");
            DropTable("dbo.Pa_Departamento");
            DropTable("dbo.Pa_Coordenacao");
            DropTable("dbo.Pa_ContramedidaGenerica");
            DropTable("dbo.Pa_ContramedidaEspecifica");
            DropTable("dbo.pa_configuracao_email");
            DropTable("dbo.Pa_Colvis");
            DropTable("dbo.Pa_CausaMedidaXAcao");
            DropTable("dbo.Pa_CausaGenerica");
            DropTable("dbo.Pa_CausaEspecifica");
            DropTable("dbo.pa_vinculo_participante_projeto");
            DropTable("dbo.pa_vinculo_participante_multipla_escolha");
            DropTable("dbo.pa_vinculo_campo_tarefa");
            DropTable("dbo.pa_multipla_escolha");
            DropTable("dbo.pa_campo");
            DropTable("dbo.pa_projeto");
            DropTable("dbo.pa_grupo_projeto");
            DropTable("dbo.pa_empresa");
            DropTable("dbo.pa_participante");
            DropTable("dbo.pa_tarefa");
            DropTable("dbo.pa_acompanhamento_tarefa");
            DropTable("dbo.Pa_Acompanhamento");
            DropTable("dbo.Pa_AcaoXQuem");
            DropTable("dbo.Pa_Acao");
            DropTable("dbo.Observacoes");
            DropTable("dbo.NQA");
            DropTable("dbo.MigrationHistory");
            DropTable("dbo.manDataCollectIT");
            DropTable("dbo.LogSgqGlobal");
            DropTable("dbo.LogSgq");
            DropTable("dbo.LogOperacaoPA");
            DropTable("dbo.LogJson");
            DropTable("dbo.LogAlteracoes");
            DropTable("HangFire.List");
            DropTable("dbo.List");
            DropTable("dbo.LeftControlRole");
            DropTable("HangFire.JobQueue");
            DropTable("dbo.JobQueue");
            DropTable("HangFire.State");
            DropTable("HangFire.JobParameter");
            DropTable("HangFire.Job");
            DropTable("dbo.State");
            DropTable("dbo.JobParameter");
            DropTable("dbo.Job");
            DropTable("dbo.ItemMenu");
            DropTable("HangFire.Hash");
            DropTable("dbo.Hash");
            DropTable("dbo.FormularioTratamentoAnomalia");
            DropTable("dbo.GrupoCausa");
            DropTable("dbo.fa_GrupoCausa");
            DropTable("dbo.Example");
            DropTable("dbo.Estados");
            DropTable("dbo.EquipamentosAvaliados");
            DropTable("dbo.Empresas");
            DropTable("dbo.Empresa");
            DropTable("dbo.Email_ConfiguracaoEmailSgq");
            DropTable("dbo.Desvios");
            DropTable("dbo.DesvioNiveis");
            DropTable("dbo.DelDados");
            DropTable("HangFire.Counter");
            DropTable("dbo.Counter");
            DropTable("dbo.ControleMetaTolerancia");
            DropTable("dbo.fa_ContramedidaGenerica");
            DropTable("dbo.ContramedidaGenerica");
            DropTable("dbo.fa_ContramedidaEspecifica");
            DropTable("dbo.ContramedidaEspecifica");
            DropTable("dbo.ConsolidationLevel2XCluster");
            DropTable("dbo.ConsolidationLevel1XCluster");
            DropTable("dbo.ConfiguracaoEmailPA");
            DropTable("dbo.ConfiguracaoEmail");
            DropTable("dbo.CollectionLevel2XCluster");
            DropTable("dbo.ParLevelDefiniton");
            DropTable("dbo.ParScoreType");
            DropTable("dbo.ParConsolidationType");
            DropTable("dbo.ParLevel2Level1");
            DropTable("dbo.ParDepartment");
            DropTable("dbo.ParLocal");
            DropTable("dbo.Result_Level3");
            DropTable("dbo.ParFrequency");
            DropTable("dbo.ParRelapse");
            DropTable("dbo.VolumeVacuoGRD");
            DropTable("dbo.VolumePcc1b");
            DropTable("dbo.VolumeCepRecortes");
            DropTable("dbo.VolumeCepDesossa");
            DropTable("dbo.ParSample");
            DropTable("dbo.ParNotConformityRule");
            DropTable("dbo.ParNotConformityRuleXLevel");
            DropTable("dbo.ParMultipleValues");
            DropTable("dbo.ParMultipleValuesXParCompany");
            DropTable("dbo.ParMeasurementUnit");
            DropTable("dbo.ParLevel3InputType");
            DropTable("dbo.ParLevel3BoolTrue");
            DropTable("dbo.ParLevel3BoolFalse");
            DropTable("dbo.ParLevel3Value");
            DropTable("dbo.ParLevel3Level2Level1");
            DropTable("dbo.ParLevel3Group");
            DropTable("dbo.ParLevel3Level2");
            DropTable("dbo.ParLevel2ControlCompany");
            DropTable("dbo.ParGoal");
            DropTable("dbo.ParEvaluation");
            DropTable("dbo.GroupMenu");
            DropTable("dbo.Menu");
            DropTable("dbo.MenuXRoles");
            DropTable("dbo.RoleUserSgq");
            DropTable("dbo.UserXRoles");
            DropTable("dbo.Deviation");
            DropTable("dbo.EmailContent");
            DropTable("dbo.CorrectiveAction");
            DropTable("dbo.Level02");
            DropTable("dbo.UnitUser");
            DropTable("dbo.Unit");
            DropTable("dbo.Level01");
            DropTable("dbo.Department");
            DropTable("dbo.ConsolidationLevel01");
            DropTable("dbo.ConsolidationLevel02");
            DropTable("dbo.Level03");
            DropTable("dbo.CollectionLevel03");
            DropTable("dbo.CollectionLevel02");
            DropTable("dbo.UserSgq");
            DropTable("dbo.ParCompanyXUserSgq");
            DropTable("dbo.ParStructureGroup");
            DropTable("dbo.ParStructure");
            DropTable("dbo.ParCompanyXStructure");
            DropTable("dbo.ParCriticalLevel");
            DropTable("dbo.ParLevel1XCluster");
            DropTable("dbo.ParModuleXModule");
            DropTable("dbo.ParModule");
            DropTable("dbo.ParClusterXModule");
            DropTable("dbo.ParClusterGroup");
            DropTable("dbo.ParCluster");
            DropTable("dbo.ParCompanyCluster");
            DropTable("dbo.Defect");
            DropTable("dbo.ParCompany");
            DropTable("dbo.ParLevel3EvaluationSample");
            DropTable("dbo.ParLevel3");
            DropTable("dbo.ParCounter");
            DropTable("dbo.ParCounterXLocal");
            DropTable("dbo.ParLevel2");
            DropTable("dbo.ConsolidationLevel2");
            DropTable("dbo.ConsolidationLevel1");
            DropTable("dbo.ParLevel1");
            DropTable("dbo.ParLevel1XHeaderField");
            DropTable("dbo.ParHeaderField");
            DropTable("dbo.ParFieldType");
            DropTable("dbo.CollectionLevel2XParHeaderField");
            DropTable("dbo.CollectionLevel2");
            DropTable("dbo.CollectionLevel2XCollectionJson");
            DropTable("dbo.CollectionJson");
            DropTable("dbo.CollectionHtml");
            DropTable("dbo.ClusterDepartamentos");
            DropTable("dbo.ClassificacaoProduto");
            DropTable("dbo.Classificacao");
            DropTable("dbo.fa_CausaGenerica");
            DropTable("dbo.CausaGenerica");
            DropTable("dbo.fa_CausaEspecifica");
            DropTable("dbo.CausaEspecifica");
            DropTable("dbo.CaracteristicaTipificacaoSequencial");
            DropTable("dbo.CaracteristicaTipificacao");
            DropTable("dbo.CACHORRO");
            DropTable("dbo.BkpCollection");
            DropTable("dbo.AreasParticipantes");
            DropTable("HangFire.AggregatedCounter");
            DropTable("dbo.AggregatedCounter");
            DropTable("dbo.AcoesPreventivas");
            DropTable("dbo.Clusters");
            DropTable("dbo.Pontos");
            DropTable("dbo.Pacotes");
            DropTable("dbo.PacotesOperacoes");
            DropTable("dbo.Metas");
            DropTable("dbo.ObservacoesPadroes");
            DropTable("dbo.VolumeProducao");
            DropTable("dbo.VerificacaoTipificacao");
            DropTable("dbo.TipificacaoReal");
            DropTable("dbo.TarefaAvaliacoes");
            DropTable("dbo.TarefaAmostras");
            DropTable("dbo.VerificacaoTipificacaoTarefaIntegracao");
            DropTable("dbo.TarefaMonitoramentos");
            DropTable("dbo.UnidadesMedidas");
            DropTable("dbo.PadraoTolerancias");
            DropTable("dbo.Padroes");
            DropTable("dbo.PadraoMonitoramentos");
            DropTable("dbo.MonitoramentoEquipamentos");
            DropTable("dbo.TipoAvaliacoes");
            DropTable("dbo.GrupoTipoAvaliacoes");
            DropTable("dbo.GrupoTipoAvaliacaoMonitoramentos");
            DropTable("dbo.Monitoramentos");
            DropTable("dbo.MonitoramentosConcorrentes");
            DropTable("dbo.Horarios");
            DropTable("dbo.VinculoParticipanteProjeto");
            DropTable("dbo.VinculoParticipanteMultiplaEscolha");
            DropTable("dbo.VinculoCampoTarefa");
            DropTable("dbo.MultiplaEscolha");
            DropTable("dbo.GrupoCabecalho");
            DropTable("dbo.Campo");
            DropTable("dbo.VinculoCampoCabecalho");
            DropTable("dbo.UsuarioUnidades");
            DropTable("dbo.Regionais");
            DropTable("dbo.fa_PlanoDeAcaoQuemQuandoComo");
            DropTable("dbo.PlanoDeAcaoQuemQuandoComo");
            DropTable("dbo.Niveis");
            DropTable("dbo.NiveisUsuarios");
            DropTable("dbo.Usuarios");
            DropTable("dbo.AcompanhamentoTarefa");
            DropTable("dbo.TarefaPA");
            DropTable("dbo.Cabecalho");
            DropTable("dbo.Projeto");
            DropTable("dbo.GrupoProjeto");
            DropTable("dbo.Equipamentos");
            DropTable("dbo.Unidades");
            DropTable("dbo.FamiliaProdutos");
            DropTable("dbo.TarefaCategorias");
            DropTable("dbo.Categorias");
            DropTable("dbo.CategoriaProdutos");
            DropTable("dbo.Produtos");
            DropTable("dbo.DepartamentoProdutos");
            DropTable("dbo.DepartamentoOperacoes");
            DropTable("dbo.Departamentos");
            DropTable("dbo.Tarefas");
            DropTable("dbo.Alertas");
            DropTable("dbo.Operacoes");
            DropTable("dbo.AcoesCorretivas");
            DropTable("dbo.Acoes");
        }
    }
}
