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

            sLight7 = new Spotlight(300, new Color(0.7f, 0.5f, 0.4f), new Vector2(1254, 1112), 100, 0.12f, true);
            sLight8 = new Spotlight(200, new Color(0.2f, 0.6f, 0.3f), new Vector2(1404, 1112), 100, 0.18f, true);
            sLight9 = new Spotlight(400, new Color(0.3f, 0.2f, 0.8f), new Vector2(1104, 1112), 100, 0.2f, true);
            sLight10 = new Spotlight(500, new Color(0.3f, 0.4f, 0.8f), new Vector2(1254, 1112), 100, 0.01f);
            sLight11 = new Spotlight(600, new Color(0.6f, 0.2f, 0.1f), new Vector2(1404, 1112), 100, 0.04f);
            sLight12 = new Spotlight(600, new Color(0.4f, 0.8f, 0.3f), new Vector2(1104, 1112), 100, 0.02f);

            sLight13 = new Spotlight(300, new Color(0.7f, 0.5f, 0.4f), new Vector2(1254, 1112), 100, 0.1f, true);
            sLight14 = new Spotlight(200, new Color(0.2f, 0.6f, 0.3f), new Vector2(1404, 1112), 100, 0.09f, true);
            sLight15 = new Spotlight(400, new Color(0.3f, 0.2f, 0.8f), new Vector2(1104, 1112), 100, 0.07f, true);
            sLight16 = new Spotlight(500, new Color(0.3f, 0.4f, 0.8f), new Vector2(1254, 1112), 100, 0.01f);
            sLight17 = new Spotlight(600, new Color(0.6f, 0.2f, 0.1f), new Vector2(1404, 1112), 100, 0.005f);
            sLight18 = new Spotlight(600, new Color(0.4f, 0.8f, 0.3f), new Vector2(1104, 1112), 100, 0.003f);

            sLight19 = new Spotlight(300, new Color(0.7f, 0.5f, 0.4f), new Vector2(1254, 1112), 100, 0.06f, true);
            sLight20 = new Spotlight(200, new Color(0.5f, 0.6f, 0.3f), new Vector2(1404, 1112), 100, 0.08f, true);
            sLight21 = new Spotlight(400, new Color(0.8f, 0.2f, 0.8f), new Vector2(1104, 1112), 100, 0.04f, true);
            sLight22 = new Spotlight(500, new Color(0.3f, 0.4f, 0.8f), new Vector2(1254, 1112), 100, 0.001f);
            sLight23 = new Spotlight(600, new Color(0.6f, 0.2f, 0.1f), new Vector2(1404, 1112), 100, 0.0006f);
            sLight24 = new Spotlight(600, new Color(0.4f, 0.8f, 0.3f), new Vector2(1104, 1112), 100, 0.0008f);
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
                level.ambientColor = new Color(0.15f, 0.11f, 0.10f, 1f);
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
                level.ambientColor = new Color(0.1f, 0.08f, 0.2f, 1f);
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
                level.ambientColor = new Color(0.02f, 0.03f, 0.05f, 1f);
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
                level.ambientColor = new Color(0.04f, 0.07f, 0.01f, 1f);
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
    }
}
