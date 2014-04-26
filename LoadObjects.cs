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
