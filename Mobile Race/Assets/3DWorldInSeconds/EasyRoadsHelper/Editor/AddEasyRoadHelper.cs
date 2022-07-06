/*--------------------------------------------------------
  AddEasyRoadHelper.cs

  Created by Alain Debelley on 2021-12-22.
  Copyright (c) 2021 ALAIN DEBELLEY. All rights reserved.
  http://www.3dworldinseconds.com/
  
  EasyRoads3D v3 is a trade mark of AndaSoft
  3DWorldInseconds is not responsible of its operation
--------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WorldInSeconds3DCommon
{

    public class AddEasyRoadHelper : MonoBehaviour
    {
        [MenuItem("Tools/3D World In Seconds/Add EasyRoad Adjustment Component")]
        public static void Set()
        {
            foreach (var transform in Selection.transforms)
            {

                if (transform.GetComponent("ERModularRoad") != null)
                {
                    if (transform.GetComponent<EasyRoadAdjustment>() == null)
                    {
                        transform.gameObject.AddComponent<EasyRoadAdjustment>();
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", transform.name + " is not a Road object", "Ok");
                }


            }
        }
    }
}
