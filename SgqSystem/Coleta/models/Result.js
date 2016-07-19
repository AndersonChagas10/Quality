class Result
{
    constructor(Id_Tarefa, Id_Operacao, Id_Monitoramento, Evaluate, NotConform, Id, numero1, numero2) 
    {
        this.Id = Id;
        this.Id_Tarefa = Id_Tarefa;
        this.Id_Operacao = Id_Operacao;
        this.Id_Monitoramento  = Id_Monitoramento;
        this.Evaluate = Evaluate,
        this.NotConform = NotConform,
        this.AddDate = new Date().toISOString(),
        this.AlterDate = new Date().toISOString(),
        this.numero1 = numero1,
		this.numero2 = numero2
    }

    setId_Tarefa(value) {
        this.Id_Tarefa = parseInt(value);
    }


    setId_Operacao(value){
        this.Id_Operacao = parseInt(value);
    }


    setId_Monitoramento(value){
        this.Id_Monitoramento = parseInt(value);
    }

    setEvaluate(value){
        this.Evaluate = parseInt(value);
    }

    setNotConform(value){
        this.NotConform = parseInt(value);
    }

    setId(value){
        this.Id = parseInt(value);
    }

    setNumero1(value){
        this.numero1 = value;
    }

    setNumero2(value){
        this.numero2 = value;
    }

    getObject(){
        return { 'Id_Tarefa': this.Id_Tarefa,
            'Id_Operacao': this.Id_Operacao,
            'Id_Monitoramento': this.Id_Monitoramento,
            'Evaluate': this.Evaluate,
            'NotConform': this.NotConform,
            'numero1': this.numero1,
		    'numero2': this.numero2,
            'Id': this.Id,
            'AddDate': this.AddDate,
            'AlterDate': this.AlterDate
        };
    }

};