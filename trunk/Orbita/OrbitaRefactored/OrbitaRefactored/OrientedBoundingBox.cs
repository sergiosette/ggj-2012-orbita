using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OrbitaRefactored
{
    public class OrientedBoundingBox
    {
        Vector2 center;
        int width;
        int height;
        double angle;

        public double Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
            }
        }

        public Vector2 Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
            }
        }

        public OrientedBoundingBox(Vector2 center, int width, int height)
        {
            this.center = center;
            this.width = width;
            this.height = height;
        }

        public OrientedBoundingBox(Vector2 center, int width, int height, double angle):this(center, width, height)
        {
            this.angle = angle;
        }

        public Vector2? Intersection (OrientedBoundingBox box)
        {
            Vector2 halfVector = new Vector2(width / 2, height / 2);
            Vector2 newOrigin = center + halfVector;

            Vector2? v1 = testEdges(GetEdges(), box.GetEdges());
            Vector2? v2 = testEdges(box.GetEdges(), GetEdges());

            Vector2? result = MathUtil.min(v1, v2);

            return result;
        }

        public IList<Vector2> GetEdges ()
        {
            IList<Vector2> edges = new List<Vector2>();

            Vector2 halfVector = new Vector2(width / 2, height / 2);
            

            edges.Add(halfVector);
            edges.Add(new Vector2(-halfVector.X, halfVector.Y));
            edges.Add(new Vector2(halfVector.X, -halfVector.Y));
            edges.Add(new Vector2(-halfVector.X, -halfVector.Y));

            for (int i = 0; i < edges.Count; i++)
            {
                edges[i] = MathUtil.rotate(edges[i], angle);
                edges[i] = edges[i] + center;
            }// return edges with screen coordinates

            return edges;
        }

        private Vector2? testEdges(IList<Vector2> sourceEdges, IList<Vector2> edgesToCompare)
        {
            Vector2? colisionResult = null;

            colisionResult = MathUtil.minimumColisionVector(sourceEdges[1], sourceEdges[0], edgesToCompare);
            Vector2? temp = MathUtil.minimumColisionVector(sourceEdges[2], sourceEdges[0], edgesToCompare);

            colisionResult = MathUtil.min(colisionResult, temp);

            return colisionResult;
        }

    }
}
