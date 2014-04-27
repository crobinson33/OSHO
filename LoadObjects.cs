using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class LoadObjects
    {

        public LoadObjects()
        {

        }

        public void LoadGrass(Level level)
        {
            float xIndex = 0;
            float yIndex = 0;

            /*for (int i = 0; i < 10; i++)
            {
                BackgroundItem newGrass = new BackgroundItem("grass", "assets/HunchGrass.png", new Vector2(128, 128), new Vector2(xIndex, yIndex));
                level.AddObject(newGrass);

                xIndex += 128;
            }*/

            while (xIndex < 2000 && yIndex < 2000)
            {
                BackgroundItem newGrass = new BackgroundItem("grass", "assets/HunchGrass.png", new Vector2(128, 128), new Vector2(xIndex, yIndex));
                level.AddBackgroundObject(newGrass);

                if (xIndex + 128 >= 2000)
                {
                    xIndex = 0;
                    yIndex += 128;
                }
                else
                {
                    xIndex += 128;
                }

            }
        }



        public void LoadTrees(Level level)
        {

            /*float xIndex = 0;
            float yIndex = 50;

            for (int i = 0; i < 50; i++)
            {
                Tree newTree = new Tree("tree", new Vector2(xIndex, yIndex), level.world);
                level.AddObject(newTree);

                xIndex += 13;
            }*/

            //xIndex = 0;
            //yIndex = 120;

            //for (int i = 0; i < 50; i++)
            //{
            //    Tree newTree = new Tree("tree", new Vector2(xIndex, yIndex), level.world);
            //    level.AddObject(newTree);

            //    xIndex += 13;
            //}

            //xIndex = 0;
            //yIndex = 190;

            //for (int i = 0; i < 50; i++)
            //{
            //    Tree newTree = new Tree("tree", new Vector2(xIndex, yIndex), level.world);
            //    level.AddObject(newTree);

            //    xIndex += 13;
            //}

			// bout to get real ugly up in here.

			int numberOfRings = 15;
			int pointIncrementor = 10;
			int radiusIncrementor = 40;

			int points = 1;
			//int points2 = 20;
			//int points3 = 30;
			int radius = 400;
			//int radius2 = 600;
			//int radius3 = 620;

			//double slice2 = 2 * Math.PI / points2;
			//double slice3 = 2 * Math.PI / points3;


			for (int j = 0; j < numberOfRings; j++)
			{
				double slice = 2 * Math.PI / points;
				for (int i = 0; i < points; i++)
				{
					double angle = slice * i;
					//Console.WriteLine("angle: " + angle);
					float x = (float)(1000 + radius * Math.Cos(angle));
					float y = (float)(1000 + radius * Math.Sin(angle));
					Vector2 target = new Vector2(x, y);

					/*Console.WriteLine ("creating tree at: " + target);
					Tree newTree = new Tree("tree", target, level.world);
					level.AddObject(newTree);*/


					BackgroundItem newTree = new BackgroundItem("tree", "assets/tree.png", new Vector2(32, 128), target);
					level.AddBackgroundObject(newTree);
				}

				points += pointIncrementor;
				radius += radiusIncrementor;

				/*for (int i = 0; i < points2; i++)
				{
					double angle2 = slice2 * i;
					//Console.WriteLine("angle: " + angle);
					float x = (float)(1000 + radius2 * Math.Cos(angle2));
					float y = (float)(1000 + radius2 * Math.Sin(angle2));
					Vector2 target = new Vector2(x, y);
					
					Console.WriteLine ("creating tree at: " + target);
					Tree newTree = new Tree("tree", target, level.world);
					level.AddObject(newTree);
				}

				for (int i = 0; i < points3; i++)
				{
					double angle3 = slice3 * i;
					//Console.WriteLine("angle: " + angle);
					float x = (float)(1000 + radius3 * Math.Cos(angle3));
					float y = (float)(1000 + radius3 * Math.Sin(angle3));
					Vector2 target = new Vector2(x, y);
					
					Console.WriteLine ("creating tree at: " + target);
					Tree newTree = new Tree("tree", target, level.world);
					level.AddObject(newTree);
				}*/
			}

        }


    }
}
