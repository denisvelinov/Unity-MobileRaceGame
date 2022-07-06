/*--------------------------------------------------------
  TagCreatorProto.cs

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
    public class TagCreatorProto
    {
        public static void CreateBridgeTag()
        {
            string tag = "bridge";
            var asset = AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset");
            if (asset != null)
            { // sanity checking
                var SerializedAsset = new SerializedObject(asset);
                var tags = SerializedAsset.FindProperty("tags");

                var numTags = tags.arraySize;
                // do not create duplicates
                for (int i = 0; i < numTags; i++)
                {
                    var existingTag = tags.GetArrayElementAtIndex(i);
                    if (existingTag.stringValue == tag) return;
                }

                tags.InsertArrayElementAtIndex(numTags);
                tags.GetArrayElementAtIndex(numTags).stringValue = tag;
                SerializedAsset.ApplyModifiedProperties();
                SerializedAsset.Update();

            }
        }
    }
}