class Result
{
    constructor(Id_Level3, Id_Level1, Id_Level2, Evaluate, NotConform, Id, numero1, numero2, Periodo, Reaudit, Auditor, UserID)
    {
        this.Id = Id;
        this.Id_Level3 = Id_Level3;
        this.Id_Level1 = Id_Level1;
        this.Id_Level2  = Id_Level2;
        this.Evaluate = Evaluate,
        this.NotConform = NotConform,
        this.AddDate = new Date().toISOString(),
        this.AlterDate = new Date().toISOString(),
        this.numero1 = numero1,
		this.numero2 = numero2,
        this.Period = Periodo,
        this.Reaudit = Reaudit,
        this.Auditor = Auditor
        this.UserID = UserID
    }

    setId_Level3(value) {
        this.Id_Level3 = parseInt(value);
    }


    setId_Level1(value){
        this.Id_Level1 = parseInt(value);
    }


    setId_Level2(value){
        this.Id_Level2 = parseInt(value);
    }

    setEvaluate(value){
        this.Evaluate = parseInt(value);
    }

    setPeriod(value) {
        this.Period = parseInt(value);
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

    setAuditor(value) {
        this.Auditor = value;
    }

    setReaudit(value) {
        this.Reaudit = value;
    }

    setUserID(value) {
        this.UserID = value;
    }

    getObject(){
        return { 'Id_Level3': this.Id_Level3,
            'Id_Level1': this.Id_Level1,
            'Id_Level2': this.Id_Level2,
            'Evaluate': this.Evaluate,
            'NotConform': this.NotConform,
            'numero1': this.numero1,
		    'numero2': this.numero2,
            'Id': this.Id,
            'AddDate': this.AddDate,
            'AlterDate': this.AlterDate,
            'Period': this.Period,
            'Reaudit': this.Reaudit,
            'Auditor': this.Auditor,
            'UserID' : this.UserID
        };
    }

};