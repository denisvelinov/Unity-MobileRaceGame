/*--------------------------------------------------------
  CharacterPositionnerEditor.cs

  Created by Alain Debelley on 2021-12-22.
  Copyright (c) 2021 ALAIN DEBELLEY. All rights reserved.
  http://www.3dworldinseconds.com/
--------------------------------------------------------*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityStandardAssets.Characters.FirstPerson;

namespace WorldInSeconds3DCommon
{
    [CustomEditor(typeof(CharacterPosition))]
    [InitializeOnLoad]
    public class CharacterPositionEditor : Editor
    {
        CharacterPosition _myTarget;
        int selectedIndex;
        GameObject[] bridgesArray;

        void OnEnable()
        {
            _myTarget = target as CharacterPosition;
            selectedIndex = 0;

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            // get bridge list
            bridgesArray = GameObject.FindGameObjectsWithTag("bridge");
            string[] bridgesName = new string[bridgesArray.Length];
            for (int i = 0; i < bridgesArray.Length; i++)
            {
                bridgesName[i] = bridgesArray[i].name;
            }
            EditorGUILayout.LabelField("Select a bridge to place FPS :");

            selectedIndex = EditorGUILayout.Popup(selectedIndex, bridgesName);
            if (GUILayout.Button(new GUIContent("Go to the selected bridge",
                           "Place the FPS at the middle of the selected bridge.")))
            {
                GameObject selectedBridge = bridgesArray[selectedIndex];

                BridgePoints bridgePoints;
                if (selectedBridge.TryGetComponent<BridgePoints>(out bridgePoints))
                {
                    Transform bridgeInstance = selectedBridge.transform.Find("Bridge_instance");
                    if (bridgeInstance != null)
                    {

                        if (_myTarget.transform.TryGetComponent<CharacterController>(out CharacterController characterController))
                        {
                            float characterHeight = characterController.height;
                            Vector2 unitVector2D = new Vector2(bridgePoints.points[1].x - bridgePoints.points[0].x, bridgePoints.points[1].z - bridgePoints.points[0].z).normalized;
                            Vector3 unitVector = new Vector3(unitVector2D.x, 0, unitVector2D.y);
                            Vector3 position = bridgePoints.points[0] + unitVector;
                            position.y += characterHeight * 0.5f;
                            _myTarget.transform.position = position;
                            _myTarget.transform.LookAt(bridgeInstance, Vector3.up);
                            Vector3 rotationVector = _myTarget.transform.eulerAngles;
                            rotationVector.x = 0;
                            rotationVector.z = 0;
                            _myTarget.transform.rotation = Quaternion.Euler(rotationVector);
                        }

                    }
                }



            }
        }

    }
}