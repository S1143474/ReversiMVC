using System;

namespace WebUI.Filters
{
    public static class FilterHelpers
    {
        /// <summary>
        /// Check if Speler 1 is waiting on a Speler 1
        /// </summary>
        /// <param name="speler2Token"></param>
        /// <returns></returns>
        public static bool IsSpeler1Waiting(Guid? speler2Token) => speler2Token is null;

        public static bool IsInWaitingView(object controller, object action) => controller.Equals("Spel") && action.Equals("Waiting");

        /// <summary>
        /// Check if the Speler is already playing the game
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool IsInReversiView(object controller, object action) => controller.Equals("Spel") && action.Equals("Reversi");

        public static bool IsInResultView(object controller, object action) => controller.Equals("Spel") && action.Equals("Result");

    }
}
