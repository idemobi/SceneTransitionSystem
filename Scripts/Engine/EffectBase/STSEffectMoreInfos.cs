using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Represents additional information for scene transition effects.
    /// This class holds the start point and finish point vectors that are used to provide
    /// more detailed parameters for transition effects.
    /// </summary>
    public partial class STSEffectMoreInfos
    {
        /// <summary>
        /// The initial position in a 2D coordinate system where an effect begins.
        /// Represented as a vector with X and Y components.
        /// </summary>
        public Vector2 StartPoint;

        /// <summary>
        /// Defines the ending point for a visual effect transition within the SceneTransitionSystem.
        /// </summary>
        public Vector2 FinishPoint;
    }
}