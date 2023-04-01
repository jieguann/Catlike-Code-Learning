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

        void Awake()
        {   float step = 2f / resolution;
            Vector3 position = Vector3.zero;
            var scale = Vector3.one * step;
            for (int i = 0; i < resolution; i++)
            {
                Transform point = Instantiate(pointPrefab);
                point.SetParent(transform, false);

                position.x = (i + 0.5f)  * step - 1f;
                position.y = position.x * position.x;
                point.localPosition = position;
                point.localScale = scale;
            }
            
        }
    }
}

