﻿using System;
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

            Level level1 = game.AddLevel(new Vector2(game.windowWidth / 2, game.windowHeight / 2), new Vector2(1000, 1000), new Color(1f, 1f, 1f, 1f));
            game.SetCurrentLevel(0);

            Player player1 = new Player("one", new Vector2(320, 200), level1.world, level1.mouse, level1.camera);
            Background bg = new Background("bg", new Vector2(0,0));
            Background bg2 = new Background("bg2", new Vector2(640, 0));
            Background bg3 = new Background("bg3", new Vector2(0,400));
            Background bg4 = new Background("bg4", new Vector2(640,400));

            Spotlight sLight = new Spotlight(50, new Color(1f, 0f, 0f), new Vector2(100, 100), 100, 1f);
            Spotlight sLight2 = new Spotlight(50, new Color(0f, 1f, 0f), new Vector2(150, 100), 100, 1f);
            Spotlight sLight3 = new Spotlight(50, new Color(0f, 0f, 1f), new Vector2(125, 150), 100, 1f);

            Spotlight sLight21 = new Spotlight(50, new Color(1f, 0f, 0f), new Vector2(300, 100), 100, 0.66f);
            Spotlight sLight22 = new Spotlight(50, new Color(0f, 1f, 0f), new Vector2(350, 100), 100, 0.66f);
            Spotlight sLight32 = new Spotlight(50, new Color(0f, 0f, 1f), new Vector2(325, 150), 100, 0.66f);

            Spotlight sLight31 = new Spotlight(50, new Color(1f, 0f, 0f), new Vector2(500, 100), 100, 0.33f);
            Spotlight sLight23 = new Spotlight(50, new Color(0f, 1f, 0f), new Vector2(550, 100), 100, 0.33f);
            Spotlight sLight33 = new Spotlight(50, new Color(0f, 0f, 1f), new Vector2(525, 150), 100, 0.33f);

            Spotlight asLight = new Spotlight(50, new Color(1f, 0f, 0f), new Vector2(100, 300), 100, 1f, true);
            Spotlight asLight2 = new Spotlight(50, new Color(0f, 1f, 0f), new Vector2(150, 300), 100, 1f, true);
            Spotlight asLight3 = new Spotlight(50, new Color(0f, 0f, 1f), new Vector2(125, 350), 100, 1f, true);

            Spotlight asLight21 = new Spotlight(50, new Color(1f, 0f, 0f), new Vector2(300, 300), 100, 0.66f, true);
            Spotlight asLight22 = new Spotlight(50, new Color(0f, 1f, 0f), new Vector2(350, 300), 100, 0.66f, true);
            Spotlight asLight32 = new Spotlight(50, new Color(0f, 0f, 1f), new Vector2(325, 350), 100, 0.66f, true);

            Spotlight asLight31 = new Spotlight(50, new Color(1f, 0f, 0f), new Vector2(500, 300), 100, 0.33f, true);
            Spotlight asLight23 = new Spotlight(50, new Color(0f, 1f, 0f), new Vector2(550, 300), 100, 0.33f, true);
            Spotlight asLight33 = new Spotlight(50, new Color(0f, 0f, 1f), new Vector2(525, 350), 100, 0.33f, true);

            Item item1 = new Item("box1", new Vector2(300, 300), level1.world);
            Enemy newEnemy = new Enemy("enemy", new Vector2(100, 100), level1.world, player1);

            level1.AddObject(bg);
            level1.AddObject(bg2);
            level1.AddObject(bg3);
            level1.AddObject(bg4);
            level1.AddObject(player1);
            level1.AddObject(item1);
            level1.AddObject(newEnemy);

            level1.AddLight(sLight);
            level1.AddLight(sLight2);
            level1.AddLight(sLight3);
            level1.AddLight(sLight21);
            level1.AddLight(sLight22);
            level1.AddLight(sLight32);
            level1.AddLight(sLight31);
            level1.AddLight(sLight23);
            level1.AddLight(sLight33);
            level1.AddLight(asLight);
            level1.AddLight(asLight2);
            level1.AddLight(asLight3);
            level1.AddLight(asLight21);
            level1.AddLight(asLight22);
            level1.AddLight(asLight32);
            level1.AddLight(asLight31);
            level1.AddLight(asLight23);
            level1.AddLight(asLight33);

            game.Start();
        }
    }
}
