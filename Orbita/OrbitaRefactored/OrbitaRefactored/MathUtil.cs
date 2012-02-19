using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OrbitaRefactored
{
    class MathUtil
    {
        public static Point rotate(Point vector, double angle)
        {
            double a = angle * Math.PI / 180;
            double s = Math.Sin(a);
            double c = Math.Cos(a);
            double ox = vector.X;
            double oy = vector.Y;
            //rotacionar o vetor direção
            vector.X = (int)(ox * c + oy * (-s));
            vector.Y = (int)(ox * s + oy * c);
            //normalizar o vetor direção
            //normalize (x, y);
            return vector;
        }

        public static Vector2 rotate(Vector2 vector, double angle)
        {
            double a = angle * Math.PI / 180;
            double s = Math.Sin(a);
            double c = Math.Cos(a);
            double ox = vector.X;
            double oy = vector.Y;
            //rotacionar o vetor direção
            vector.X = (int)(ox * c + oy * (-s));
            vector.Y = (int)(ox * s + oy * c);
            //normalizar o vetor direção
            //normalize (x, y);
            return vector;
        }

        public static Vector2 normalize(Vector2 vector)
        {
            float module = (float) Math.Sqrt (Math.Pow(vector.X,2) + Math.Pow(vector.Y, 2));
            Vector2 normalizedVector = new Vector2(vector.X/module, vector.Y/module);

            return normalizedVector;
        }

        public static float dotProduct(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static Vector2? minimumColisionVector (Vector2 colinearVector1, Vector2 colinearVector2, IList<Vector2> lists)
        {
            Vector2? result = null;
            Vector2 vectorReference = colinearVector1 - colinearVector2; //getting the axis to project to
            Vector2 axis = normalize(vectorReference); // normalize it so it will speed up projection

            float minScalar = float.MaxValue;
            float maxScalar = float.MinValue;

            foreach (Vector2 v in lists)
            {
                //Vector2 projectedVector = new Vector2();
                float cos = dotProduct(axis, v);
                if (cos > maxScalar) maxScalar = cos;
                if (cos < minScalar) minScalar = cos;
                //result.Add(projectedVector);
            }

            float vRefMin = dotProduct(axis,colinearVector1); //as the vector reference has the same origin than the axis vector
            float vRefMax = dotProduct(axis,colinearVector2); //making dotProduct with the same vector brings it Module
            if (vRefMax < vRefMin)
            {
                float temp = vRefMin;
                vRefMin = vRefMax;
                vRefMax = temp;
            }

            float difference1 = vRefMin - maxScalar;
            float difference2 = vRefMax - minScalar;

            //checks if difference1 and difference2 have the same direction (signal)
            //it means there where an intersection
            //by multiplying, this statement will only be true if the variables store values with the same signal (-and- or +and+)
            if (difference1 * difference2 < 0)
            {
                float scalar = difference1;
                if (Math.Abs(difference1) > Math.Abs(difference2)) scalar = difference2;
                result = new Vector2(axis.X*scalar, axis.Y*scalar); //multiply by the axis to get the colision vector
            }

            return result;
        }

        public static Vector2? min(Vector2? v1, Vector2? v2)
        {
            Vector2? result;
            if (v1 == null || v2 == null)
            {
                result = null;
            }
            else
            {
                result = v1;
                if (result == null)
                {
                    result = v2;
                }
                else
                {
                    if (v2 != null)
                    {
                        //both are not null the smaller is going to be the result
                        if (v2.Value.LengthSquared() < result.Value.LengthSquared())
                        {
                            result = v2;
                        }
                    }
                }
            }
            return result;
        }
    }
}
