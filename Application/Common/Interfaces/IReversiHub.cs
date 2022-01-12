using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Application.Common.Interfaces
{
    public interface IReversiHub
    {
        Task StartGame();

        Task Redirect(string url);

        Task OnMove(object move);

        Task SendStartGameAsync(string speler1Token, string speler2Token);
        Task SendRedirectAsync(string speler1Token, string speler2Token, string url);
    }
}
