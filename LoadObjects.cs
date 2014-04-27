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

            float xIndex = 0;
            float yIndex = 50;

            for (int i = 0; i < 50; i++)
            {
                Tree newTree = new Tree("tree", new Vector2(xIndex, yIndex), level.world);
                level.AddObject(newTree);

                xIndex += 13;
            }

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

        }


    }
}
