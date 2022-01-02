using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IReversiHub
    {
        Task StartGame(string name, string message);

        Task OnMove(string speler1Token, string spelToken, object move);
    }
}
