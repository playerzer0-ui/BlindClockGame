using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NodeTesting.models;


namespace BlindClockGame
{
    public class Button
    {
        private SpriteFont _font;
        private CollisionRect hitbox;
        private TextBox box;

        public CollisionRect Hitbox { get => hitbox; set => hitbox = value; }

        public Button() 
        {
            _font = Globals.Content.Load<SpriteFont>("File");
            Hitbox = new CollisionRect(640, 360, 100, 50);
            box = new TextBox(_font);
        }

        public void SetText(string text)
        {
            box.Text = text;
        }

        public void Draw()
        {
            Hitbox.Draw(PicoPallete.white);
            box.Draw(640, 360, PicoPallete.black);
        }
    }
}
