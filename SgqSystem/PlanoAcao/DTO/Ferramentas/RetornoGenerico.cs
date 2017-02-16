using PA.DTO.Ferramentas;
using System;
using System.Runtime.Serialization;

namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class RetornoGenerico<T>
    {
        /// <summary>
        /// Mensagem a exibir
        /// </summary>
        [DataMember]
        public string Mensagem { get; set; }

        /// <summary>
        /// Tipo Mensagem
        /// {1 = Success} {2 = Info} {3 = Warning} {4 = Danger}
        /// </summary>
        [DataMember]
        public int TipoMensagem
        {
            get
            {
                if (String.IsNullOrEmpty(IdMensagem))
                {
                    return 0;
                }
                else
                {
                    if (IdMensagem.Equals("success"))
                    {
                        return 1;
                    }
                    else if (IdMensagem.Equals("info"))
                    {
                        return 2;
                    }
                    else if (IdMensagem.Equals("warning"))
                    {
                        return 3;
                    }
                    else if (IdMensagem.Equals("danger"))
                    {
                        return 4;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            set
            {
                if (value.IsNull())
                {
                    IdMensagem = string.Empty;
                }
                else
                {

                    if (value == 1)
                    {
                        IdMensagem = "success";
                    }
                    else if (value == 2)
                    {
                        IdMensagem = "info";
                    }
                    else if (value == 3)
                    {
                        IdMensagem = "warning";
                    }
                    else if (value == 4)
                    {
                        IdMensagem = "danger";
                    }
                }
            }
        }

        /// <summary>
        /// Id da div mensagem
        /// </summary>
        [DataMember]
        public string IdMensagem { get; set; }

        /// <summary>
        /// Para comparar se ocorreu algum erro
        /// </summary>
        [DataMember]
        public bool IsSucesso { get; set; }

        /// <summary>
        /// Retorno do tipo Generico
        /// </summary>
        [DataMember]
        public T Retorno { get; set; }

        [DataMember]
        public Exception Exception { get; set; }

    }


    public class RequestJbs
    {

        public int IdProjeto { get; set; }

        public int IdBusca { get; set; }

        public int IdBusca2 { get; set; }

        public string Tabela { get; set; }

    }

}
