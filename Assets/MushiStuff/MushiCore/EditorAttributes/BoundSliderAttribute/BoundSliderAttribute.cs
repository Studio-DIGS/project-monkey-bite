using System;
using UnityEngine;

namespace MushiCore.EditorAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class BoundSliderAttribute : PropertyAttribute
    {
        public readonly float min;
        public readonly float max;

        /// <summary>
        ///  Attribute for drawing Vector2/Vector2Int as a range bound slider
        /// </summary>
        /// <param name="min">minimum allowed value</param>
        /// <param name="max">maximum allowed value</param>
        public BoundSliderAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}