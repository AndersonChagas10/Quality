using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.Services
{
    public class Level02Result
    {
        public string unidadeId { get; set; }
        public int period { get; set; }
        public int shift { get; set; }
        public int evaluate { get; set; }
        public int sample { get; set; }
        public string result { get; set; }
        public bool haveCorrectiveAction { get; set; }
        public bool haveReaudit { get; set; }
        public bool havePhases { get; set; }
        public bool reaudit { get; set; }
        public int reauditNumber { get; set; }
        public bool completed { get; set; }
        public int defects { get; set; }
        public int baisedUnbaised { get; set; }
        public Level02Result()
        {

        }

        public Level02Result(string unidadeId, int period, int shift, int evaluate, int sample, string result, bool haveCorrectiveAction, bool haveReaudit, 
                             bool havePhases, bool reaudit, int reauditNumber, bool completed, int defects, int baisedUnbaised)
        {
            this.unidadeId = unidadeId;
            this.period = period;
            this.shift = shift;
            this.evaluate = evaluate;
            this.sample = sample; 
            this.result = result;
            this.haveCorrectiveAction = haveCorrectiveAction;
            this.haveReaudit = haveReaudit;
            this.havePhases = havePhases;
            this.reaudit = reaudit;
            this.reauditNumber = reauditNumber;
            this.completed = completed;
            this.defects = defects;
            this.baisedUnbaised = baisedUnbaised;
        }
    }
}