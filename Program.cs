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

            Level level1 = game.AddLevel(new Vector2(game.windowWidth / 2, game.windowHeight / 2), new Vector2(1000, 1000), new Color(0.145f, 0.345f, 0.709f, 0.9f));
            game.SetCurrentLevel(0);

            Player player1 = new Player("one", new Vector2(320, 200), level1.world, level1.mouse, level1.camera);
            Background bg = new Background("bg", new Vector2(0,0));
            Background bg2 = new Background("bg2", new Vector2(640, 0));
            Background bg3 = new Background("bg3", new Vector2(0,400));
            Background bg4 = new Background("bg4", new Vector2(640,400));

            Spotlight sLight = new Spotlight(100, new Color(1f, 1f, 1f), new Vector2(0, 0));
            Spotlight sLight2 = new Spotlight(50, new Color(0.5f, 0.2f, 0.7f), new Vector2(50, 50));
            Spotlight sLight3 = new Spotlight(75, new Color(0.1f, 0.1f, 0.6f), new Vector2(404, 302));
            Spotlight sLight4 = new Spotlight(137, new Color(0.5f, 0.9f, 0.2f), new Vector2(320, 20));
            Spotlight sLight5 = new Spotlight(232, new Color(0.3f, 0.3f, 0.7f), new Vector2(670, 80));
            Spotlight sLight6 = new Spotlight(10, new Color(0.6f, 0.4f, 0.1f), new Vector2(103, 123));

            Item item1 = new Item("box1", new Vector2(300, 300), level1.world);

            level1.AddObject(bg);
            level1.AddObject(bg2);
            level1.AddObject(bg3);
            level1.AddObject(bg4);
            level1.AddObject(player1);
            level1.AddObject(item1);
            level1.AddLight(sLight);
            level1.AddLight(sLight2);
            level1.AddLight(sLight3);
            level1.AddLight(sLight4);
            level1.AddLight(sLight5);
            level1.AddLight(sLight6);

            game.Start();
        }
    }
}
