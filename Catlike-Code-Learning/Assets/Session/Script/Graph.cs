using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    public class Graph : MonoBehaviour
    {
        [SerializeField]
        Transform pointPrefab;

        [SerializeField, Range(10,100)]
        int resolution = 10;

        [SerializeField]
        FunctionLibrary.FunctionName function;

        Transform[] points;
        
        void Awake()
        {   float step = 2f / resolution;
            Vector3 position = Vector3.zero;
            var scale = Vector3.one * step;

            points = new Transform[resolution * resolution];
            for (int i = 0; i < points.Length; i++)
            {
                Transform point = points[i] = Instantiate(pointPrefab);
                point.SetParent(transform, false);

                position.x = (i + 0.5f)  * step - 1f;
                //position.y = position.x * position.x;
                point.localPosition = position;
                point.localScale = scale;
            }
            
        }

        private void Update()
        {
            FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);

            float time = Time.time;
            for (int i = 0; i < points.Length; i++)
            {
                Transform point = points[i];
                Vector3 position = point.localPosition;
                position.y = f(position.x, position.z, time);
                point.localPosition = position;
            }
        }
    }
}

