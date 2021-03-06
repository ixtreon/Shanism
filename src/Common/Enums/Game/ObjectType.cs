﻿

namespace Shanism.Common
{
    /// <summary>
    /// The different object types as seen by the client. 
    /// </summary>
    public enum ObjectType : byte
    {
        /// <summary>
        /// An object, also entity, of type unit. 
        /// </summary>
        Unit,
        /// <summary>
        /// An object, also entity, of type effect. 
        /// </summary>
        Effect,
        /// <summary>
        /// An object, also entity, of type doodad. 
        /// </summary>
        Doodad,
        /// <summary>
        /// An object, also entity, of type hero. 
        /// </summary>
        Hero,

        /// <summary>
        /// An object of type buff. 
        /// </summary>
        Buff,
        /// <summary>
        /// An object of type buff instance. 
        /// </summary>
        BuffInstance,
        /// <summary>
        /// An object of type ability. 
        /// </summary>
        Ability,
        /// <summary>
        /// An object of type item. 
        /// </summary>
        Item,
        /// <summary>
        /// An object of type script. 
        /// </summary>
        Script,
    }
}