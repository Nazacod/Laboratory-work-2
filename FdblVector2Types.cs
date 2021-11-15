﻿using System;
using System.Numerics;

namespace Lab1
{
    static class FdblVector2Types
    {
        public static Vector2 f1(double x, double y) 
        {
            return new Vector2(1.0f, 1.0f);
        }
        public static Vector2 f2(double x, double y) 
        {
            return new Vector2(Convert.ToSingle(x), Convert.ToSingle(y));
        }
        public static Vector2 f3(double x, double y) 
        {
            return new Vector2(Convert.ToSingle(Math.Cos(x)), Convert.ToSingle(Math.Sin(y)));
        }
    }
}
