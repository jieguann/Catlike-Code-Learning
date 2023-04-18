using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    public class Graph : MonoBehaviour
    {
        [SerializeField]
        Transform pointPrefab;

        [SerializeField, Range(10,200)]
        int resolution = 10;

        [SerializeField]
        FunctionLibrary.FunctionName function;

        public enum TransitionMode { Cycle, Random }

        [SerializeField]
        TransitionMode transitionMode;

        [SerializeField, Min(0f)]
        float functionDuration = 1f, transitionDuration = 1f;

        Transform[] points;
        float duration;

        bool transitioning;

        FunctionLibrary.FunctionName transitionFunction;

        void Awake()
        {   float step = 2f / resolution;
            //Vector3 position = Vector3.zero;
            var scale = Vector3.one * step;

            points = new Transform[resolution * resolution];
            for (int i = 0; i < points.Length; i++)
            {
                
                Transform point = points[i] = Instantiate(pointPrefab);
                point.SetParent(transform, false);

                //position.x = (x + 0.5f)  * step - 1f;
                //position.z = (z + 0.5f) * step - 1f;
                //position.y = position.x * position.x;
                //point.localPosition = position;
                point.localScale = scale;
            }
            
        }

        private void Update()
        {
            duration += Time.deltaTime;
            if (transitioning) {
                if (duration >= transitionDuration)
                {
                    duration -= transitionDuration;
                    transitioning = false;
                }
            }
            else if(duration >= functionDuration) { 
                duration -=functionDuration;
                transitioning = true;
                transitionFunction = function;
                PickNextFunction();
            }
            if (transitioning)
            {
                UpdateFunctionTransition();
            }
            else
            {
                UpdateFunction();
            }
        }

        void PickNextFunction()
        {
            function = transitionMode == TransitionMode.Cycle ?
                FunctionLibrary.GetNextFunctionName(function) :
                FunctionLibrary.GetRandomFunctionNameOtherThan(function);
        }

        private void UpdateFunction()
        {
            FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);

            float time = Time.time;
            float step = 2f / resolution;

            float v = 0.5f * step - 1f;
            for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
            {
                if (x == resolution)
                {
                    x = 0;
                    z += 1;
                    v = (z + 0.5f) * step - 1f;
                }
                float u = (x + 0.5f) * step - 1f;
                
                points[i].localPosition = f(u, v, time);
            }
        }

        void UpdateFunctionTransition()
        {
            FunctionLibrary.Function
                from = FunctionLibrary.GetFunction(transitionFunction),
                to = FunctionLibrary.GetFunction(function);
            float progress = duration / transitionDuration;

            float time = Time.time;
            float step = 2f / resolution;

            float v = 0.5f * step - 1f;
            for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
            {
                if (x == resolution)
                {
                    x = 0;
                    z += 1;
                    v = (z + 0.5f) * step - 1f;
                }
                float u = (x + 0.5f) * step - 1f;

                points[i].localPosition = FunctionLibrary.Morph(
                u, v, time, from, to, progress
            );
            }
        }
    }
}

