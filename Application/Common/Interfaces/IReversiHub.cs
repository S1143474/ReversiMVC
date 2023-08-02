using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Spellen.Commands.FinishedSpel;
using Application.Spellen.Commands.PlaceFiche;
using Microsoft.AspNetCore.Mvc;

namespace Application.Common.Interfaces
{
    public interface IReversiHub
    {
        // Probably not necessary anymore.
        Task StartGame();

        Task Redirect(string url);

        Task OnMove(List<FicheCoordDTO> fichesToTurnAround, int aanDeBeurt);

        Task OnWrongMove(string notExecutedMessage);

        Task OnDisableMove(List<FicheCoordDTO> fichesToTurnAround, int aanDeBeurt);

        Task OnFinish(FinishedSpelDTO finishedSpelDto);

        Task OnError(string message);

        Task OnPlayerOnline(int amount);
        Task OnCreateGame();
    }
}
