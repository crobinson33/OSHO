using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            Level level1 = game.AddLevel();
            game.SetCurrentLevel(0);

            Player player1 = new Player("one", new Vector2(0, 0));

            level1.AddObject(player1);

            game.Start();
        }
    }
}
