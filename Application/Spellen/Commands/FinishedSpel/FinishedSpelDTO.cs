using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Spellen.Commands.FinishedSpel
{
    public class FinishedSpelDTO
    {
        public string WinnerName { get; set; }

        public string LoserName { get; set; }

        public bool IsDraw { get; set; }

        public int WinnerPoints { get; set; }
        public int LoserPoints { get; set; }
    }
}
