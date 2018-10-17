using Microsoft.Xna.Framework.Input;
using System;

namespace Shanism.Client
{
    /// <summary>
    /// The mouse cursors used throughout the game.
    /// </summary>
    public enum GameCursor
    {
        Default,    // a.k.a. Arrow
        ClickMe,    // e.g. Hand
        TextInput,
        SizeH,
        SizeV,
        SizeNWSE,
        SizeNESW,
    }

    public static class GameCursorExt
    {
        public static MouseCursor GetCursorObject(this GameCursor cursor)
        {
            switch(cursor)
            {
                case GameCursor.Default:
                    return MouseCursor.Arrow;

                case GameCursor.ClickMe:
                    return MouseCursor.Hand;

                case GameCursor.TextInput:
                    return MouseCursor.IBeam;

                case GameCursor.SizeH:
                    return MouseCursor.SizeWE;

                case GameCursor.SizeV:
                    return MouseCursor.SizeNS;

                case GameCursor.SizeNWSE:
                    return MouseCursor.SizeNWSE;

                case GameCursor.SizeNESW:
                    return MouseCursor.SizeNESW;

            }
            throw new ArgumentOutOfRangeException(nameof(cursor));
        }
    }
}
