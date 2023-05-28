using SharedLibrary.Models;
using System.Collections;
using UnityEngine;

namespace Components
{
    public class ColorParser
    {
        public Color GetColor(ColorStatus colorStatus)
        {
            switch (colorStatus)
            {
                case ColorStatus.Red:
                    return Color.red;
                case ColorStatus.Blue:
                    return Color.blue;
                case ColorStatus.Yellow:
                    return Color.yellow;
                default:
                    return Color.red;
            }
        }

        public ColorStatus GetColorStatus(Color color)
        {
            if (color.Equals(Color.red))
            {
                return ColorStatus.Red;
            }
            if (color.Equals(Color.blue))
            {
                return ColorStatus.Blue;
            }
            if (color.Equals(Color.yellow))
            {
                return ColorStatus.Yellow;
            }
            return ColorStatus.Red;
        }
    }
}