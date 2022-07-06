/*--------------------------------------------------------
  NewBridge02Proto.cs

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
    public class NewBridge02Proto : MonoBehaviour
    {
        [MenuItem("Tools/3D World In Seconds/New Stone Bridge (prototype)")]
        public static void Set()
        {
            TagCreatorProto.CreateBridgeTag();
            GameObject bridgeObject = new GameObject("New Stone Bridge");
            bridgeObject.tag = "bridge";
            bridgeObject.AddComponent<Bridge02Proto>();
            Selection.activeObject = bridgeObject;
        }
    }
}
