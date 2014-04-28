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

        // Lights
        public Spotlight sLight1;
        public Spotlight sLight2;
        public Spotlight sLight3;
        public Spotlight sLight4;
        public Spotlight sLight5;
        public Spotlight sLight6;

        public Spotlight sLight7;
        public Spotlight sLight8;
        public Spotlight sLight9;
        public Spotlight sLight10;
        public Spotlight sLight11;
        public Spotlight sLight12;

        public Spotlight sLight13;
        public Spotlight sLight14;
        public Spotlight sLight15;
        public Spotlight sLight16;
        public Spotlight sLight17;
        public Spotlight sLight18;

        public Spotlight sLight19;
        public Spotlight sLight20;
        public Spotlight sLight21;
        public Spotlight sLight22;
        public Spotlight sLight23;
        public Spotlight sLight24;

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
            sLight4 = new Spotlight(500, new Color(0.3f, 0.4f, 0.8f), new Vector2(1254, 1112), 100, 0.03f);
            sLight5 = new Spotlight(600, new Color(0.6f, 0.2f, 0.1f), new Vector2(1404, 1112), 100, 0.05f);
            sLight6 = new Spotlight(600, new Color(0.4f, 0.8f, 0.3f), new Vector2(1104, 1112), 100, 0.04f);

            sLight7 = new Spotlight(300, new Color(0.7f, 0.5f, 0.4f), new Vector2(1254, 1112), 100, 0.2f, true);
            sLight8 = new Spotlight(200, new Color(0.2f, 0.6f, 0.3f), new Vector2(1404, 1112), 100, 0.2f, true);
            sLight9 = new Spotlight(400, new Color(0.3f, 0.2f, 0.8f), new Vector2(1104, 1112), 100, 0.15f, true);
            sLight10 = new Spotlight(500, new Color(0.3f, 0.4f, 0.8f), new Vector2(1254, 1112), 100, 0.02f);
            sLight11 = new Spotlight(600, new Color(0.6f, 0.2f, 0.1f), new Vector2(1404, 1112), 100, 0.05f);
            sLight12 = new Spotlight(600, new Color(0.4f, 0.8f, 0.3f), new Vector2(1104, 1112), 100, 0.07f);

            sLight13 = new Spotlight(300, new Color(0.7f, 0.5f, 0.4f), new Vector2(1254, 1112), 100, 0.15f, true);
            sLight14 = new Spotlight(200, new Color(0.2f, 0.6f, 0.3f), new Vector2(1404, 1112), 100, 0.15f, true);
            sLight15 = new Spotlight(400, new Color(0.3f, 0.2f, 0.8f), new Vector2(1104, 1112), 100, 0.2f, true);
            sLight16 = new Spotlight(500, new Color(0.3f, 0.4f, 0.8f), new Vector2(1254, 1112), 100, 0.02f);
            sLight17 = new Spotlight(600, new Color(0.6f, 0.2f, 0.1f), new Vector2(1404, 1112), 100, 0.06f);
            sLight18 = new Spotlight(600, new Color(0.4f, 0.8f, 0.3f), new Vector2(1104, 1112), 100, 0.08f);

            sLight19 = new Spotlight(300, new Color(0.7f, 0.5f, 0.4f), new Vector2(1254, 1112), 100, 0.1f, true);
            sLight20 = new Spotlight(200, new Color(0.2f, 0.6f, 0.3f), new Vector2(1404, 1112), 100, 0.1f, true);
            sLight21 = new Spotlight(400, new Color(0.3f, 0.2f, 0.8f), new Vector2(1104, 1112), 100, 0.2f, true);
            sLight22 = new Spotlight(500, new Color(0.3f, 0.4f, 0.8f), new Vector2(1254, 1112), 100, 0.02f);
            sLight23 = new Spotlight(600, new Color(0.6f, 0.2f, 0.1f), new Vector2(1404, 1112), 100, 0.04f);
            sLight24 = new Spotlight(600, new Color(0.4f, 0.8f, 0.3f), new Vector2(1104, 1112), 100, 0.08f);
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
                level.ambientColor = new Color(0.1f, 0.07f, 0.08f, 1f);
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
                level.ambientColor = new Color(0.06f, 0.05f, 0.08f, 1f);
                level.RemoveLight(sLight1);
                level.RemoveLight(sLight2);
                level.RemoveLight(sLight3);
                level.RemoveLight(sLight4);
                level.RemoveLight(sLight5);
                level.RemoveLight(sLight6);
                level.AddLight(sLight7);
                level.AddLight(sLight8);
                level.AddLight(sLight9);
                level.AddLight(sLight10);
                level.AddLight(sLight11);
                level.AddLight(sLight12);
                phaseTwoSpawned = true;
            }
            if (enemyManager.buttonThreeDown && !phaseThreeSpawned)
            {
                level.ambientColor = new Color(0.015f, 0.06f, 0.07f, 1f);
                level.RemoveLight(sLight7);
                level.RemoveLight(sLight8);
                level.RemoveLight(sLight9);
                level.RemoveLight(sLight10);
                level.RemoveLight(sLight11);
                level.RemoveLight(sLight12);
                level.AddLight(sLight13);
                level.AddLight(sLight14);
                level.AddLight(sLight15);
                level.AddLight(sLight16);
                level.AddLight(sLight17);
                level.AddLight(sLight18);
                phaseThreeSpawned = true;
            }
            if (enemyManager.buttonFourDown && !phaseFourSpawned)
            {
                level.ambientColor = new Color(0.01f, 0.02f, 0.03f, 1f);
                level.RemoveLight(sLight13);
                level.RemoveLight(sLight14);
                level.RemoveLight(sLight15);
                level.RemoveLight(sLight16);
                level.RemoveLight(sLight17);
                level.RemoveLight(sLight18);
                level.AddLight(sLight19);
                level.AddLight(sLight20);
                level.AddLight(sLight21);
                level.AddLight(sLight22);
                level.AddLight(sLight23);
                level.AddLight(sLight24);
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
