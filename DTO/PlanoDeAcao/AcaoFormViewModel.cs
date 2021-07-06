using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using static Dominio.Enums.Enums;

namespace DTO.PlanoDeAcao
{
    public class AcaoFormViewModel
    {
        public int Id { get; set; }
        public int ParLevel1_Id { get; set; }
        public string ParLevel1_Name { get; set; }
        public int ParLevel2_Id { get; set; }
        public string ParLevel2_Name { get; set; }
        public int ParLevel3_Id { get; set; }
        public string ParLevel3_Name { get; set; }
        public int ParCompany_Id { get; set; }
        public string ParCompany_Name { get; set; }
        public int ParDepartment_Id { get; set; }
        public string ParDepartment_Name { get; set; }
        public int ParDepartmentParent_Id { get; set; }
        public string ParDepartmentParent_Name { get; set; }
        public int ParCargo_Id { get; set; }
        public string ParCargo_Name { get; set; }
        public string Acao_Naoconformidade { get; set; }
        public string AcaoText { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string _DataEmissao { get { return DataEmissao?.ToShortDateString(); } }
        public DateTime? DataConclusao { get; set; }
        public TimeSpan HoraEmissao { get; set; }
        public TimeSpan HoraConclusao { get; set; }
        public string Referencia { get; set; }
        public int Responsavel { get; set; }
        public string Notificar { get; set; }
        public int Emissor { get; set; }
        public string EmissorNome { get; set; }
        public int UsuarioLogado { get; set; }

        public List<string> EvidenciaNaoConformidade { get; set; }

        public List<FileStream> listaDeFotosEvidencia { get; set; }

        public List<string> EvidenciaAcaoConcluida { get; set; }
        public string Prioridade { get; set; }
        public int Status { get; set; }
        public string StatusName { get { return Enum.GetName(typeof(Dominio.Enums.Enums.EAcaoStatus), Status); } }
        public bool IsActive { get; set; }
        public string Responsavel_Name { get; set; }

        public List<EvidenciaViewModel> ListaEvidencia { get; set; }

        public List<EvidenciaViewModel> ListaEvidenciaConcluida { get; set; }


        public string Observacao { get; set; }
        public string Acao { get; set; }


        public List<NotificarViewModel> ListaResponsavel { get; set; } = new List<NotificarViewModel>();

        public List<NotificarViewModel> ListaPrioridade { get; set; } = new List<NotificarViewModel>()
            {
                new NotificarViewModel{ Id = 1, Nome = "Baixa"},
                new NotificarViewModel{ Id = 2, Nome = "Média"},
                new NotificarViewModel{ Id = 3, Nome = "Alta"},
            };
        public List<NotificarViewModel> ListaNotificar { get; set; } = new List<NotificarViewModel>();
        public List<NotificarViewModel> ListaNotificarAcao { get; set; } = new List<NotificarViewModel>();
        public List<AcompanhamentoAcaoViewModel> ListaAcompanhamento { get; set; } = new List<AcompanhamentoAcaoViewModel>();

        public bool PermiteEditar
        { 
            get => EhEmissor && (Status == (int)EAcaoStatus.Pendente || Status == (int)EAcaoStatus.Em_Andamento);
        }
        public bool PermiteAlterarStatus 
        { 
            get => (EhEmissor && Status == (int)EAcaoStatus.Em_Andamento) && EhResponsavel; 
        }

        public bool PermiteInserirAcompanhamento 
        {
            get 
            {
                if (!EhEmissor && (Status == (int)EAcaoStatus.Em_Andamento || Status == (int)EAcaoStatus.Atrasada)) return false;

                else
                {
                    return EhVinculadoEmNotificacao() || EhResponsavel;
                }
            }   
        }

        public bool EhResponsavel => Responsavel == UsuarioLogado || Responsavel == 0;
        public bool EhEmissor => Emissor == UsuarioLogado;

        private bool EhVinculadoEmNotificacao()
        {
            if (ListaNotificarAcao != null)
            {
                return ListaNotificarAcao.Exists(l => l.Id == UsuarioLogado);
            }
            return false;
        }
    }
}
