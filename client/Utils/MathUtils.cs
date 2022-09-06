using System;
using System.Collections.Generic;
using System.Text;
using static RAGE.Ui.Cursor;

namespace car_dirty
{
    public class MathUtils
    {
        public static float GetMappedRangeValue(Vector2 inputRange, Vector2 outputRange, float value)
        {
            float divisor = inputRange.Y - inputRange.X;

            float koef = (value - inputRange.X) / divisor;

            return (outputRange.Y - outputRange.X) * koef + outputRange.X;
        }
    }
}
