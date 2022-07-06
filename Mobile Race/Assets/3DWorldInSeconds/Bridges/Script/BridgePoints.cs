/*--------------------------------------------------------
  BridgePoints.cs

  Created by Alain Debelley on 2021-12-22.
  Copyright (c) 2021 ALAIN DEBELLEY. All rights reserved.
  http://www.3dworldinseconds.com/
--------------------------------------------------------*/
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WorldInSeconds3DCommon
{
    public class BridgePoints : MonoBehaviour
    {
        [HideInInspector]
        public Vector3[] points;
        [HideInInspector]
        public float width;
        [HideInInspector]
        public float ElevationCorrection = 0;
    }
}
#endif
