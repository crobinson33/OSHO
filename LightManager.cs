using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class LightManager : Manager
    {
        public Level level;
        public EnemyManager enemyManager;
        bool phaseOneSpawned;
        bool phaseTwoSpawned;
        bool phaseThreeSpawned;
        bool phaseFourSpawned;
        bool baseLight;

        // Lights!
        public Spotlight sLight1;
        public Spotlight sLight2;
        public Spotlight sLight3;
        public Spotlight sLight4;
        public Spotlight sLight5;
        public Spotlight sLight6;

        public LightManager(string tag, Level level, EnemyManager enemyManager)
            : base(tag)
        {
            this.level = level;
            this.enemyManager = enemyManager;
            this.phaseOneSpawned = false;
            this.phaseTwoSpawned = false;
            this.phaseThreeSpawned = false;
            this.phaseFourSpawned = false;
            this.baseLight = false;

            // Lights
            sLight1 = new Spotlight(300, new Color(0.7f, 0.5f, 0.4f), new Vector2(1254, 1112), 100, 0.3f, true);
            sLight2 = new Spotlight(200, new Color(0.2f, 0.6f, 0.3f), new Vector2(1404, 1112), 100, 0.3f, true);
            sLight3 = new Spotlight(400, new Color(0.3f, 0.2f, 0.8f), new Vector2(1104, 1112), 100, 0.3f, true);
            sLight4 = new Spotlight(500, new Color(0.3f, 0.4f, 0.8f), new Vector2(1254, 1112), 100, 0.2f);
            sLight5 = new Spotlight(600, new Color(0.6f, 0.2f, 0.1f), new Vector2(1404, 1112), 100, 0.2f);
            sLight6 = new Spotlight(600, new Color(0.4f, 0.8f, 0.3f), new Vector2(1104, 1112), 100, 0.2f);
        }
        public override void Update(float deltaTime)
        {
            if (!baseLight)
            {
                level.ambientColor = new Color(1f, 1f, 1f, 1f);
                baseLight = true;
            }
            if (enemyManager.buttonOneDown && !phaseOneSpawned)
            {
                level.ambientColor = new Color(0.3f, 0.2f, 0.25f, 1f);
                level.AddLight(sLight1);
                level.AddLight(sLight2);
                level.AddLight(sLight3);
                level.AddLight(sLight4);
                level.AddLight(sLight5);
                level.AddLight(sLight6);
                phaseOneSpawned = true;
            }
            if (enemyManager.buttonTwoDown && !phaseTwoSpawned)
            {
                Spotlight sLight1 = new Spotlight(500, new Color(1f, 0f, 0f), new Vector2(300, 100), 100, 0.66f);
                Spotlight sLight2 = new Spotlight(500, new Color(0f, 1f, 0f), new Vector2(350, 100), 100, 0.66f);
                Spotlight sLight3 = new Spotlight(500, new Color(0f, 0f, 1f), new Vector2(325, 150), 100, 0.66f);
                level.AddLight(sLight1);
                level.AddLight(sLight2);
                level.AddLight(sLight3);
                phaseTwoSpawned = true;
            }
            if (enemyManager.buttonThreeDown && !phaseThreeSpawned)
            {
                Spotlight sLight1 = new Spotlight(500, new Color(1f, 0f, 0f), new Vector2(300, 100), 100, 0.66f);
                Spotlight sLight2 = new Spotlight(500, new Color(0f, 1f, 0f), new Vector2(350, 100), 100, 0.66f);
                Spotlight sLight3 = new Spotlight(500, new Color(0f, 0f, 1f), new Vector2(325, 150), 100, 0.66f);
                level.AddLight(sLight1);
                level.AddLight(sLight2);
                level.AddLight(sLight3);
                phaseThreeSpawned = true;
            }
            if (enemyManager.buttonFourDown && !phaseFourSpawned)
            {
                Spotlight sLight1 = new Spotlight(500, new Color(1f, 0f, 0f), new Vector2(300, 100), 100, 0.66f);
                Spotlight sLight2 = new Spotlight(500, new Color(0f, 1f, 0f), new Vector2(350, 100), 100, 0.66f);
                Spotlight sLight3 = new Spotlight(500, new Color(0f, 0f, 1f), new Vector2(325, 150), 100, 0.66f);
                level.AddLight(sLight1);
                level.AddLight(sLight2);
                level.AddLight(sLight3);
                phaseFourSpawned = true;
            }
            base.Update(deltaTime);
        }

        public void setupBaseLights()
        {

        }

        public void setupPhaseOneLights()
        {

        }

        public void setupPhaseTwoLights()
        {

        }

        public void setupPhaseThreeLights()
        {

        }

        public void setupPhaseFourLights()
        {

        }

        //Spotlight sLight31 = new Spotlight(500, new Color(1f, 0f, 0f), new Vector2(500, 100), 100, 0.33f);
        //Spotlight sLight23 = new Spotlight(500, new Color(0f, 1f, 0f), new Vector2(550, 100), 100, 0.33f);
        //Spotlight sLight33 = new Spotlight(500, new Color(0f, 0f, 1f), new Vector2(525, 150), 100, 0.33f);

        //Spotlight asLight = new Spotlight(500, new Color(1f, 0f, 0f), new Vector2(700, 500), 100, 0.7f, true);
        //Spotlight asLight2 = new Spotlight(500, new Color(0f, 1f, 0f), new Vector2(150, 300), 100, 0.6f, true);
        //Spotlight asLight3 = new Spotlight(500, new Color(0f, 0f, 1f), new Vector2(325, 850), 100, 0.3f, true);

        //Spotlight asLight21 = new Spotlight(500, new Color(1f, 0f, 0f), new Vector2(300, 300), 100, 0.66f, true);
        //Spotlight asLight22 = new Spotlight(500, new Color(0f, 1f, 0f), new Vector2(450, 100), 100, 0.66f, true);
        //Spotlight asLight32 = new Spotlight(500, new Color(0f, 0f, 1f), new Vector2(325, 350), 100, 0.66f, true);

        //Spotlight asLight31 = new Spotlight(500, new Color(1f, 0f, 0f), new Vector2(500, 300), 100, 0.33f, true);
        //Spotlight asLight23 = new Spotlight(500, new Color(0f, 1f, 0f), new Vector2(350, 760), 100, 0.33f, true);
        //Spotlight asLight33 = new Spotlight(500, new Color(0f, 0f, 1f), new Vector2(825, 450), 100, 0.33f, true);


        //level1.AddLight(sLight21);
        //level1.AddLight(sLight22);
        //level1.AddLight(sLight32);
        //level1.AddLight(sLight31);
        //level1.AddLight(sLight23);
        //level1.AddLight(sLight33);
        //level1.AddLight(asLight);
        //level1.AddLight(asLight2);
        //level1.AddLight(asLight3);
        //level1.AddLight(asLight21);
        //level1.AddLight(asLight22);
        //level1.AddLight(asLight32);
        //level1.AddLight(asLight31);
        //level1.AddLight(asLight23);
        //level1.AddLight(asLight33);
    }
}
