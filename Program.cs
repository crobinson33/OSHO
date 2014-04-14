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

            Level level1 = game.AddLevel(new Vector2(game.windowWidth/2, game.windowHeight/2));
            game.SetCurrentLevel(0);

            Player player1 = new Player("one", new Vector2(320, 200), level1.world, level1.mouse, level1.camera);
            Background bg = new Background("bg");

            Item item1 = new Item("box1", new Vector2(300, 300), level1.world);

            level1.AddObject(bg);
            level1.AddObject(player1);
            level1.AddObject(item1);

            game.Start();
        }
    }
}
