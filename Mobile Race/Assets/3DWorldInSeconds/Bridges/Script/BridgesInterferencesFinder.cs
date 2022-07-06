/*--------------------------------------------------------
  BridgesInterferencesFinder.cs

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
    public class BridgesInterferencesFinder
    {
        /// <summary>
        /// Detects if there are interfernces in terrain modification between the current bridge and existing ones
        /// </summary>
        /// <param name="currentBridge">the current bridge game object</param>
        /// <param name="startPoint">the current stratpoint</param>
        /// <param name="endPoint">the current end point</param>
        /// <param name="startAdjustmentDistance">the start adjustment distance</param>
        /// <param name="endAdjustmentDistance">the end adjustment distance</param>
        /// <returns>true if there is a possible issue</returns>
        public static bool FindInterferences(GameObject currentBridge,float currentWidth, Vector3 startPoint, Vector3 endPoint, float startAdjustmentDistance, float endAdjustmentDistance)
        {
            bool result = false;
            Terrain theTerrain = Terrain.activeTerrain;
            float terrainResolution = 0;
            if (theTerrain != null)
            {
                TerrainData terrainData = theTerrain.terrainData;
                if (terrainData != null)
                {
                    terrainResolution = terrainData.heightmapScale.x;
                }
            }
            
            Vector2 start2D = new Vector2(startPoint.x, startPoint.z);
            Vector2 end2D = new Vector2(endPoint.x, endPoint.z);
            GameObject[] bridgesArray = GameObject.FindGameObjectsWithTag("bridge");
            foreach(GameObject bridge in bridgesArray)
            {
                if (bridge != currentBridge)
                {
                    BridgePoints bridgePoints;
                    if (bridge.TryGetComponent<BridgePoints>(out bridgePoints))
                    {
                        float otherWidth = bridgePoints.width;
                        Vector2 otherStart = new Vector2(bridgePoints.points[0].x, bridgePoints.points[0].z);
                        Vector2 otherEnd = new Vector2(bridgePoints.points[1].x, bridgePoints.points[1].z);
                        float startStartDistance = Vector2.Distance(start2D, otherStart);
                        float startEndDistance = Vector2.Distance(start2D, otherEnd);
                        float endStartDistance = Vector2.Distance(end2D, otherStart);
                        float endEndDistance = Vector2.Distance(end2D, otherEnd);
                        if (startStartDistance - currentWidth * 0.5f - otherWidth * 0.5f - terrainResolution * 0.5f < startAdjustmentDistance || startEndDistance - currentWidth * 0.5f - otherWidth * 0.5f - terrainResolution * 0.5f < endAdjustmentDistance)
                        {
                            result = true;
                            break;
                        }
                        
                        if (endStartDistance - currentWidth*0.5f - otherWidth*0.5f - terrainResolution*0.5f < endAdjustmentDistance || endEndDistance - currentWidth * 0.5f - otherWidth * 0.5f - terrainResolution * 0.5f < endAdjustmentDistance)
                        {
                            result = true;
                            break;
                        }

                    }
                }
            }

            return result;
        }
    }
}

#endif