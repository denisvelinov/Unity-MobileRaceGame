/*--------------------------------------------------------
  TerrainSaverEditor.cs

  Created by Alain Debelley on 2021-12-22.
  Copyright (c) 2021 ALAIN DEBELLEY. All rights reserved.
  http://www.3dworldinseconds.com/
--------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


namespace WorldInSeconds3DPremium
{
    [CustomEditor(typeof(TerrainSaver))]
    [InitializeOnLoad]
    public class TerrainSaverEditor : Editor
    {

        private Color32 textColor1 = new Color32(120, 220, 250, 255);
        private Color32 textColor2 = new Color32(210, 240, 250, 255);
        private Color32 bgColor1 = new Color32(190, 200, 230, 255);
        TerrainSaver terrainSaver;
        private int index = 0;
       



        void OnEnable()
        {
            terrainSaver = target as TerrainSaver;
           
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.Space();
            GUI.backgroundColor = bgColor1;
            GUI.contentColor = textColor2;
            var previousGUIState = GUI.enabled;
            // Disabling edit for property
            GUI.enabled = false;
            GUIContent folderLabel = new GUIContent("Backup Folder", "the folder where the backup folder is placed. Must begin with 'Assets'");
            terrainSaver.BackupFolder = EditorGUILayout.TextField(folderLabel, terrainSaver.BackupFolder);
            GUI.enabled = previousGUIState;
            GUIContent getFolderContent = new GUIContent("Specify Backup Folder", "Allows to specify the folder where the saved terrains will be stored.");

            bool buttonGetBackupFolder = GUILayout.Button(getFolderContent);
            if (buttonGetBackupFolder)
            {
                string absolutePath = EditorUtility.OpenFolderPanel("Prefab generation Folder", "Assets", "");


                if (absolutePath.StartsWith(Application.dataPath))
                {
                    terrainSaver.BackupFolder = "Assets" + absolutePath.Substring(Application.dataPath.Length);
                }
                else if (absolutePath != "")
                {
                    EditorUtility.DisplayDialog("Error", "The chosen folder must be inside the Assets folder.", "Ok");

                }


            }
            EditorGUILayout.Space();
            if (GUILayout.Button(new GUIContent("Save Terrain Data",
                    "Creates the TerrainData backup to the Resources folder.")))
            {
                terrainSaver.CopyTerrain();
            }
            GUIContent versionsToKeepLabel = new GUIContent("Vesions to keep", "the number of terrain data versions to keep");
            terrainSaver.VersionsToKeep = EditorGUILayout.IntSlider(versionsToKeepLabel, terrainSaver.VersionsToKeep, 1, 20);
            EditorGUILayout.Space();
            SortedList<DateTime, string> terrainDataSOrtedList = terrainSaver.GetBacupedTerrains();
            List<string> datesList = new List<string>();
            List<string> filesList = new List<string>();
            
            foreach (KeyValuePair<DateTime, string> kvp in terrainDataSOrtedList)
            {
               
                datesList.Add(kvp.Key.ToString(System.Globalization.CultureInfo.CurrentCulture).Replace("/", "-"));
                filesList.Add(kvp.Value);
            }

            if (datesList.Count > 0)
            {
                string[] datesArray = datesList.ToArray();
                Array.Reverse(datesArray);
                string[] filesArray = filesList.ToArray();
                Array.Reverse(filesArray);

                EditorGUILayout.LabelField("Select a backuped version to restore :");
                index = EditorGUILayout.Popup(index, datesArray);
                EditorGUILayout.Space();
                if (GUILayout.Button(new GUIContent("Restore Selected Terrain Data",
                        "Replace the current TerrainData with the cosenTerrainData backup in the Resources folder.")))
                {
                    terrainSaver.RestoreTerrain(filesArray[index]);
                    GUIUtility.ExitGUI();
                }
            }

            

        }

      
    }
}
