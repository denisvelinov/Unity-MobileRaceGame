/*--------------------------------------------------------
  LogInspectorEditorProto.cs

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
    [CustomEditor(typeof(LogInspector))]
    public class LogInspectorEditorProto : Editor
    {
        private LogInspector _myTarget;


        void OnEnable()
        {
            _myTarget = (LogInspector)target;
        }

        public override void OnInspectorGUI()
        {
            GUIContent LogEnableContent = new GUIContent("Enable Log", "Check this checkbox if you want to trace debug messages into the Log folder to send to the support.");
            _myTarget.enableLog = GUILayout.Toggle(_myTarget.enableLog, LogEnableContent);
            if (_myTarget.enableLog)
            {
                LogManager.EnableLog = true;
            }
            else
            {
                LogManager.EnableLog = false;
            }
        }
    }
}

