namespace Shanism.Client.Sprites
{
    public class DynamicSprite : Sprite
    {

        public int CurrentFrame { get; set; }
        public int CurrentFrameElapsed { get; set; }
        public ShanoAnimation Animation { get; set; }

        /// <summary>
        /// Gets or sets whether the animation should loop
        /// after it's finished playing once.
        /// </summary>
        public bool LoopAnimation { get; set; }

        public bool IsPlaying => Animation != null && CurrentFrame < Animation.Frames.Count;


        public void SetFrame(int frameID)
        {
            CurrentFrame = frameID;
            CurrentFrameElapsed = 0;
            SourceRectangle = Animation.Frames[CurrentFrame];
        }

        public void Stop()
        {
            SetFrame(Animation.Frames.Count);
        }

        public void Restart() => SetFrame(0);

        public void SetAnimation(ShanoAnimation anim)
        {
            if (Animation?.Name == anim.Name)
                return;

            Animation = anim;
            Texture = anim.Texture.Texture;
            SetFrame(0);
        }


        public override string ToString() => Animation.Name;
    }
}
