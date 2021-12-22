using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Models.Session
{
    public class StateModel
    {
        public string Guid { get; set; }
        public int State { get; set; }

        public StateModel() { }

        public StateModel(string guid, int state)
        {
            Guid = guid;
            State = state;
        }
    }
}
