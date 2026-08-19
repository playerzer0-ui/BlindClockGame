using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NodeTesting.models;
using System;
using System.Threading;
using System.Timers;

namespace BlindClockGame
{
    public class Timer
    {
        private float currentTime = 0f;
        private Random rand = new Random();
        private float targetTime = 0f;
        private TextBox targetText;
        private TextBox timeText;

        private SpriteFont timeFont;
        private SpriteFont font;

        public TextBox TimeText { get => timeText; set => timeText = value; }
        public float TargetTime { get => targetTime; set => targetTime = value; }

        public Timer() 
        {
            font = Globals.Content.Load<SpriteFont>("File");
            timeFont = Globals.Content.Load<SpriteFont>("TimeText");
            TimeText = new TextBox(timeFont);
            targetText = new TextBox(font);
        }

        public void Reset()
        {
            currentTime = 0f;
            TimeText.Text = "00:00:00";
        }

        public void Randomize()
        {
            Reset();
            TargetTime = rand.Next(20, 61);
            timeText.Text = TimeSpan.FromSeconds(TargetTime).ToString(@"mm\:ss\:ff");
            targetText.Text = "Target Time: " + TimeSpan.FromSeconds(TargetTime).ToString(@"mm\:ss\:ff");
        }

        public int CheckTime()
        {
            if (currentTime == TargetTime * 1000) //just right
            {
                return 1;
            }
            else if (currentTime > TargetTime * 1000) //too late
            {
                return -1;
            }
            else //too early
            {
                return 0;
            }
        }
        

        public void Update(GameTime gameTime) 
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            currentTime += deltaTime;
            TimeText.Text = TimeSpan.FromMilliseconds(currentTime).ToString(@"mm\:ss\:ff");
        }

        public void Draw()
        {
            if(Globals.state == 2)
            {
                targetText.Draw(640, 100, PicoPallete.red);
            }
            TimeText.Draw(640, 240, PicoPallete.black);
        }


    }
}
