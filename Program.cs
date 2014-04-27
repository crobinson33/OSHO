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
            Keyboard keyboard = new Keyboard();

            Level level1 = game.AddLevel(new Vector2(game.windowWidth / 2, game.windowHeight / 2), new Vector2(1048, 1048), new Color(0.05f, 0.13f, 0.18f, 1f));
            game.SetCurrentLevel(0);

            /*Spotlight sLight = new Spotlight(500, new Color(1f, 0f, 0f), new Vector2(100, 100), 100, 1f);
            Spotlight sLight2 = new Spotlight(500, new Color(0f, 1f, 0f), new Vector2(150, 100), 100, 1f);
            Spotlight sLight3 = new Spotlight(500, new Color(0f, 0f, 1f), new Vector2(125, 150), 100, 1f);

            Spotlight sLight21 = new Spotlight(500, new Color(1f, 0f, 0f), new Vector2(300, 100), 100, 0.66f);
            Spotlight sLight22 = new Spotlight(500, new Color(0f, 1f, 0f), new Vector2(350, 100), 100, 0.66f);
            Spotlight sLight32 = new Spotlight(500, new Color(0f, 0f, 1f), new Vector2(325, 150), 100, 0.66f);

            Spotlight sLight31 = new Spotlight(500, new Color(1f, 0f, 0f), new Vector2(500, 100), 100, 0.33f);
            Spotlight sLight23 = new Spotlight(500, new Color(0f, 1f, 0f), new Vector2(550, 100), 100, 0.33f);
            Spotlight sLight33 = new Spotlight(500, new Color(0f, 0f, 1f), new Vector2(525, 150), 100, 0.33f);

            Spotlight asLight = new Spotlight(500, new Color(1f, 0f, 0f), new Vector2(100, 300), 100, 1f, true);
            Spotlight asLight2 = new Spotlight(500, new Color(0f, 1f, 0f), new Vector2(150, 300), 100, 1f, true);
            Spotlight asLight3 = new Spotlight(500, new Color(0f, 0f, 1f), new Vector2(125, 350), 100, 1f, true);

            Spotlight asLight21 = new Spotlight(500, new Color(1f, 0f, 0f), new Vector2(300, 300), 100, 0.66f, true);
            Spotlight asLight22 = new Spotlight(500, new Color(0f, 1f, 0f), new Vector2(350, 300), 100, 0.66f, true);
            Spotlight asLight32 = new Spotlight(500, new Color(0f, 0f, 1f), new Vector2(325, 350), 100, 0.66f, true);

            Spotlight asLight31 = new Spotlight(500, new Color(1f, 0f, 0f), new Vector2(500, 300), 100, 0.33f, true);
            Spotlight asLight23 = new Spotlight(500, new Color(0f, 1f, 0f), new Vector2(550, 300), 100, 0.33f, true);
            Spotlight asLight33 = new Spotlight(500, new Color(0f, 0f, 1f), new Vector2(525, 350), 100, 0.33f, true);*/


            EnemyManager enemyManager = new EnemyManager("enemyManager", level1);

            Player player1 = new Player("one", new Vector2(524, 524), level1.world, level1.mouse, level1.camera, keyboard, enemyManager);

            //Item item1 = new Item("box1", "assets/HunchSprite.png", new Vector2(64, 64), new Vector2(64, 64), new Vector2(300, 300), level1.world, new Vector2(0, 0));
            //item1.CreateAnimation("box1", 10, 10);
            //item1.collider.isStatic = true;
            //item1.collider.mass = 100000000;


            //Enemy newEnemy = new Enemy("enemy", new Vector2(100, 100), level1.world, player1);
            //BigEyeEnemy eyeEnemy = new BigEyeEnemy("bigEye", new Vector2(100, 100), level1.world, player1, level1.camera, enemyManager);
            //SkellyEnemy skellyEnemy = new SkellyEnemy("skelly", new Vector2(200, 100), level1.world, player1, level1.camera, enemyManager);
            //GhostEnemy ghostEnemy = new GhostEnemy("ghost", new Vector2(300, 100), level1.world, player1, level1.camera, enemyManager);


            Item buttonOne = new Item("buttonOne", "assets/red_button.png", new Vector2(64, 64), new Vector2(64, 32), new Vector2(200, 200), level1.world, new Vector2(0, -32));
            buttonOne.CreateAnimation("button", 0, 1);
            buttonOne.collider.isStatic = true;
            buttonOne.collider.mass = 100000000;
            buttonOne.collider.AddTagToIgnore("one");
            buttonOne.collider.AddTagToIgnore("bullet");
            Item buttonTwo = new Item("buttonTwo", "assets/red_button.png", new Vector2(64, 64), new Vector2(64, 32), new Vector2(350, 200), level1.world, new Vector2(0, -32));
            buttonTwo.CreateAnimation("button", 0, 1);
            buttonTwo.collider.isStatic = true;
            buttonTwo.collider.mass = 100000000;
            buttonTwo.collider.AddTagToIgnore("one");
            buttonOne.collider.AddTagToIgnore("bullet");
            Item buttonThree = new Item("buttonThree", "assets/red_button.png", new Vector2(64, 64), new Vector2(64, 32), new Vector2(200, 350), level1.world, new Vector2(0, -32));
            buttonThree.CreateAnimation("button", 0, 1);
            buttonThree.collider.isStatic = true;
            buttonThree.collider.mass = 100000000;
            buttonThree.collider.AddTagToIgnore("one");
            buttonOne.collider.AddTagToIgnore("bullet");

            LoadObjects objects = new LoadObjects();
            objects.LoadTrees(level1);
            objects.LoadGrass(level1);

            /*Tree tree1 = new Tree("tree", new Vector2(310, 110), level1.world);
            Tree tree2 = new Tree("tree", new Vector2(305, 95), level1.world);
            Tree tree3 = new Tree("tree", new Vector2(350, 102), level1.world);
            Tree tree4 = new Tree("tree", new Vector2(310, 80), level1.world);
            Tree tree5 = new Tree("tree", new Vector2(280, 91), level1.world);
            Tree tree6 = new Tree("tree", new Vector2(265, 115), level1.world);*/

            level1.AddObject(player1);
            //level1.AddObject(item1);
            //level1.AddObject(eyeEnemy);
            //level1.AddObject(skellyEnemy);
            //level1.AddObject(ghostEnemy);
            level1.AddManagerObject(enemyManager);
            //level1.AddObject(newEnemy);
            /*level1.AddObject(tree1);
            level1.AddObject(tree2);
            level1.AddObject(tree3);
            level1.AddObject(tree4);
            level1.AddObject(tree5);
            level1.AddObject(tree6);*/

            level1.AddObject(buttonOne);
            level1.AddObject(buttonTwo);
            level1.AddObject(buttonThree);

            /*level1.AddLight(sLight);
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
            level1.AddLight(asLight33);*/

            game.Start();
        }
    }
}
