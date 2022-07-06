/*--------------------------------------------------------
  EasyRoadAdjustment.cs

  Created by Alain Debelley on 2021-12-22.
  Copyright (c) 2021 ALAIN DEBELLEY. All rights reserved.
  http://www.3dworldinseconds.com/
  
  EasyRoads3D v3 is a trade mark of AndaSoft
  3DWorldInseconds is not responsible of its operation
  Please note that this helper may not work or even compile if the EasyRoads3D v3 interface is changed.
  UNCOMMENT "#define EasyRoadInstalled" only if this product is installed.
--------------------------------------------------------*/

#define EasyRoadInstalled
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if EasyRoadInstalled
using EasyRoads3Dv3;
#endif

namespace WorldInSeconds3DCommon
{
    public class EasyRoadAdjustment : MonoBehaviour
    {
        [Header("Please uncomment '#define EasyRoadInstalled' in EasyRoadAdjustmentEditor.cs")]
        [HideInInspector]
        public Vector3 startPosition = new Vector3();
        [HideInInspector]
        public Vector3 endPosition = new Vector3();
        [HideInInspector]
        public GameObject FoundBridge = null;
        [HideInInspector]
        public bool AtLeastAnIssue = false;
        [HideInInspector]
        public bool CrossingBridges = false;
        [HideInInspector]
        public float CrossingRoadRaise = 0.25f;
        [HideInInspector]
        public bool ForcePointsCloser = false;

#if EasyRoadInstalled
        [HideInInspector]
    public ERModularRoad RoadData = null;
    [HideInInspector]
    public ERModularBase RoadNetWork = null;
    [HideInInspector]
    public Transform RoadNetWorkTransform = null;
    [HideInInspector]
    public float RoadNetworkRaise = 0;
    [HideInInspector]
    public List<ERMarkerExt> Markers = null;
    [HideInInspector]
    public int AnalysedIndex = -1;
#endif
    }
}
#endif