using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Examples
{
    [RequireComponent(typeof(PathCreator))]
    public class StackPathUpdater : MonoBehaviour
    {
        public static bool deliveryTriggered;

        public bool closedLoop = true;

        public PathCreator pathCreator;
        public List<Transform> path;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) pathCreator.bezierPath = new BezierPath(path, closedLoop, PathSpace.xyz);

            if(deliveryTriggered)
            {
                pathCreator.bezierPath = new BezierPath(path, closedLoop, PathSpace.xyz);
            }
        }
    }
}
