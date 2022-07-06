/*--------------------------------------------------------
  NewBridge01Proto.cs

  Created by Alain Debelley on 2021-12-22.
  Copyright (c) 2021 ALAIN DEBELLEY. All rights reserved.
  http://www.3dworldinseconds.com/
--------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WorldInSeconds3DProto
{
    public class NewBridge01Proto : MonoBehaviour
    {
        [MenuItem("Tools/3D World In Seconds/New Columns Bridge (prototype)")]
        public static void Set()
        {
            TagCreatorProto.CreateBridgeTag();
            GameObject bridgeObject = new GameObject("New Bridge with columns");
            bridgeObject.tag = "bridge";
            bridgeObject.AddComponent<Bridge01Proto>();
            Selection.activeObject = bridgeObject;
        }
    }
}
