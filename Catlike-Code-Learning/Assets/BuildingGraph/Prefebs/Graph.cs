using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    public class Graph : MonoBehaviour
    {
        [SerializeField]
        Transform pointPrefab;

        void Awake()
        {
            Instantiate(pointPrefab);
        }
    }
}

