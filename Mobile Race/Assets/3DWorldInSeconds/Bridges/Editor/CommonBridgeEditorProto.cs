/*--------------------------------------------------------
  CommonBridgeEditorProto.cs

  Created by Alain Debelley on 2021-12-22.
  Copyright (c) 2021 ALAIN DEBELLEY. All rights reserved.
  http://www.3dworldinseconds.com/
--------------------------------------------------------*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using WorldInSeconds3DCommon;

namespace WorldInSeconds3DProto
{


    public class CommonBridgeEditorProto
    {
        /// <summary>
        /// Manages the Modify Terrain part of the bridge editor.
        /// </summary>
        /// <param name="target">The target bridge script component.</param>
        public static void ManageTerrainEditor(Object target)
        {
            ICommonBridgeProto iCommonBridge = (ICommonBridgeProto)target;
            int selectedTreeRemoval;
            switch (iCommonBridge.RemoveTreesProperty)
            {
                case RemoveTreesOption.No:
                    selectedTreeRemoval = 0;
                    break;
                case RemoveTreesOption.Smart:
                    selectedTreeRemoval = 1;
                    break;
                case RemoveTreesOption.ByDistance:
                    selectedTreeRemoval = 2;
                    break;
                default:
                    selectedTreeRemoval = 1;
                    break;
            }


            GUIContent removeTreesContent = new GUIContent("Remove Trees :", "Remove terrain trees that interfere in the bridge building.");
            GUIStyle lblStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
            EditorGUILayout.LabelField(removeTreesContent, lblStyle);
            GUILayout.BeginHorizontal();
            GUIContent noTreeRemovalContent = new GUIContent("No", "No tree removal.");
            GUIContent smartRemovalContent = new GUIContent("Smart", "Trees are removed by taking their width into account. The removal accuracy depends on the terrain resolution.");
            GUIContent distanceContent = new GUIContent("By distance", "Trees are removed when their trunk center is under the specified distance from each bridge side.");
            GUIContent[] options = new GUIContent[] { noTreeRemovalContent, smartRemovalContent, distanceContent };
            selectedTreeRemoval = GUILayout.SelectionGrid(selectedTreeRemoval, options, options.Length);

            GUILayout.EndHorizontal();
            switch (selectedTreeRemoval)
            {
                case 0:
                    iCommonBridge.RemoveTreesProperty = RemoveTreesOption.No;

                    break;
                case 1:
                    iCommonBridge.RemoveTreesProperty = RemoveTreesOption.Smart;
                    break;
                case 2:
                    iCommonBridge.RemoveTreesProperty = RemoveTreesOption.ByDistance;
                    GUIContent TreeDistanceContent = new GUIContent("Tree distance from bridge", "The minimum distance from tree origin point to the bridge side.");
                    iCommonBridge.TreesDistanceProperty = EditorGUILayout.FloatField(TreeDistanceContent, iCommonBridge.TreesDistanceProperty);
                    break;

            };


            GUIContent removeDetailsContent = new GUIContent("Remove Details (grass)", "Remove details that are close to the bridge.");
            iCommonBridge.RemoveDetailsProperty = EditorGUILayout.Toggle(removeDetailsContent, iCommonBridge.RemoveDetailsProperty);
            if (iCommonBridge.RemoveDetailsProperty)
            {
                GUIContent minDetailDistanceContent = new GUIContent("Details distance from bridge", "The minimum details distance from bridge side.");
                iCommonBridge.MinDetailDistanceProperty = EditorGUILayout.FloatField(minDetailDistanceContent, iCommonBridge.MinDetailDistanceProperty);
                float patchSize = iCommonBridge.GetDetailsPatchSize();
                EditorGUILayout.HelpBox("Due to the terrain ratio between the terrain size and details resolution, the grass will be removed by " + patchSize.ToString("n1") + " meters patches", MessageType.Info);
            }
            GUILayout.BeginVertical("box");
            GUIContent levelTerrainAtStartContent = new GUIContent("Level Terrain at start", "Level the terrain at the bridge start.");
            iCommonBridge.LevelTerrainAtStartProperty = EditorGUILayout.Toggle(levelTerrainAtStartContent, iCommonBridge.LevelTerrainAtStartProperty);
            if (iCommonBridge.LevelTerrainAtStartProperty)
            {
                GUIContent startAjustmentAutomaticContent = new GUIContent("Automatic adjustment", "Performs terrain adjustment automatically or manually by specifying a distance.");
                iCommonBridge.LevelTerrainAtStartAutomaticProperty = EditorGUILayout.Toggle(startAjustmentAutomaticContent, iCommonBridge.LevelTerrainAtStartAutomaticProperty);
                if (!iCommonBridge.LevelTerrainAtStartAutomaticProperty)
                {
                    GUIContent startAjustmentLengthContent = new GUIContent("Adjustment distance", "The distance to adjust leveled terrain height to existing terrain length at bridge start.");
                    iCommonBridge.StartAjustmentLengthProperty = EditorGUILayout.FloatField(startAjustmentLengthContent, iCommonBridge.StartAjustmentLengthProperty);
                    GUIContent startAjustmentRecomContent = new GUIContent("Get value recommendation", "Calculates a recommendation for the adjustment according to the terrrain profile.");
                    bool recommendation = GUILayout.Button(startAjustmentRecomContent);
                    if (recommendation)
                    {
                        if (LogManager.EnableLog) LogManager.LogMessage("CommonBridgeEditor " + "step01");
                        float recommendationValue = iCommonBridge.GetAdjustmentRecommendation(true);

                        bool pasteRecommendation = EditorUtility.DisplayDialog("Recommendation", "The recommendation for the Start adjustment distance is " + Mathf.Ceil(recommendationValue) + " m. Do you wante to paste it ?", "Ok", "No");
                        if (pasteRecommendation)
                        {
                            iCommonBridge.StartAjustmentLengthProperty = Mathf.Ceil(recommendationValue);
                        }
                    }
                }

            }
            GUILayout.EndVertical();
            EditorGUILayout.Space();
            GUILayout.BeginVertical("box");
            GUIContent levelTerrainAtEndContent = new GUIContent("Level Terrain at end", "Level the terrain at the bridge end.");
            iCommonBridge.LevelTerrainAtEndProperty = EditorGUILayout.Toggle(levelTerrainAtEndContent, iCommonBridge.LevelTerrainAtEndProperty);
            if (iCommonBridge.LevelTerrainAtEndProperty)
            {
                GUIContent endAjustmentAutomaticContent = new GUIContent("Automatic adjustment", "Performs terrain adjustment automatically or manually by specifying a distance.");
                iCommonBridge.LevelTerrainAtEndAutomaticProperty = EditorGUILayout.Toggle(endAjustmentAutomaticContent, iCommonBridge.LevelTerrainAtEndAutomaticProperty);
                if (!iCommonBridge.LevelTerrainAtEndAutomaticProperty)
                {
                    GUIContent endAjustmentLengthContent = new GUIContent("Adjustment distance", "The distance to adjust leveled terrain height to existing terrain length at bridge end.");
                    iCommonBridge.EndAjustmentLengthProperty = EditorGUILayout.FloatField(endAjustmentLengthContent, iCommonBridge.EndAjustmentLengthProperty);
                    GUIContent endAjustmentRecomContent = new GUIContent("Get value recommendation", "Calculates a recommendation for the adjustment according to the terrrain profile.");
                    bool recommendation = GUILayout.Button(endAjustmentRecomContent);
                    if (recommendation)
                    {
                        if (LogManager.EnableLog) LogManager.LogMessage("CommonBridgeEditor " + "step02");
                        float recommendationValue = iCommonBridge.GetAdjustmentRecommendation(false);
                        bool pasteRecommendation = EditorUtility.DisplayDialog("Recommendation", "The recommendation for the End adjustment distance is " + Mathf.Ceil(recommendationValue) + " m. Do you want to paste it ?", "Ok", "No");
                        if (pasteRecommendation)
                        {
                            iCommonBridge.EndAjustmentLengthProperty = Mathf.Ceil(recommendationValue);
                        }
                    }
                }

                GUIContent removeLipContent = new GUIContent("Remove Bank Lip", "Removes the bank lip that may occur when the Start terrain is LOWER than the End terrain and the bridge extends far over the End side bank.");
                iCommonBridge.RemoveLipProperty = EditorGUILayout.Toggle(removeLipContent, iCommonBridge.RemoveLipProperty);
                if (iCommonBridge.RemoveLipProperty)
                {
                    GUIContent lipRemovalDistanceContent = new GUIContent("Lip Removal Distance", "The expected lip thickness. It is approximately the length of the bridge part that extends over the End side bank.");
                    iCommonBridge.LipRemovalDistanceProperty = EditorGUILayout.FloatField(lipRemovalDistanceContent, iCommonBridge.LipRemovalDistanceProperty);
                    GUIContent lipRemovalRecomContent = new GUIContent("Get value recommendation", "Calculates a recommendation for the lip removal distance.");
                    bool recommendation = GUILayout.Button(lipRemovalRecomContent);
                    if (recommendation)
                    {
                        if (LogManager.EnableLog) LogManager.LogMessage("CommonBridgeEditor " + "step03");
                        float recommendationValue = iCommonBridge.GetRemovalDistanceRecommendation(); ;
                        if (recommendationValue <= 2)
                        {
                            EditorUtility.DisplayDialog("Recommendation", "There is no bank lip to remove", "Ok");
                        }
                        else
                        {
                            bool pasteRecommendation = EditorUtility.DisplayDialog("Recommendation", "The recommendation for the lip removal distance is " + Mathf.Ceil(recommendationValue) + " m. Do you want to paste it ?", "Ok", "No");
                            if (pasteRecommendation)
                            {
                                iCommonBridge.LipRemovalDistanceProperty = Mathf.Ceil(recommendationValue);
                            }
                        }

                    }
                }
            }
            GUILayout.EndVertical();
            EditorGUILayout.Space();
            GUIContent endSmoothEdgeContent = new GUIContent("Smooth edges", "Smooth the river or ravine edges to avoid steep slopes due to leveling.");
            iCommonBridge.SmoothEdgeProperty = EditorGUILayout.Toggle(endSmoothEdgeContent, iCommonBridge.SmoothEdgeProperty);
            if (iCommonBridge.SmoothEdgeProperty)
            {
                GUIContent endSmoothEdgeStrengthContent = new GUIContent("Smoothing Strength", "Smoothing strength for the river or ravine edges to avoid steep slopes due to leveling.");
                iCommonBridge.SmoothEdgeStrengthProperty = EditorGUILayout.IntSlider(endSmoothEdgeStrengthContent, iCommonBridge.SmoothEdgeStrengthProperty, 1, 4);
            }

            EditorGUILayout.Space();
            GUIContent noiseContent = new GUIContent("Natural look noise", "The noise amount to get either an even adjustment or a more natural one.");
            iCommonBridge.NoiseUnitProperty = EditorGUILayout.Slider(noiseContent, iCommonBridge.NoiseUnitProperty, 0, 2);

        }

        /// <summary>
        /// Manages the prefab generation part of the bridge editor.
        /// </summary>
        /// <param name="target">The target bridge script component.</param>
        public static void ManagePrefabGenerationEditor(Object target)
        {
            ICommonBridgeProto iCommonBridge = (ICommonBridgeProto)target;
            var previousGUIState = GUI.enabled;
            GUIContent generatePrefabContent = new GUIContent("Generate Bridge Prefab", "Generate a prefab associated with the bridge.");
            if (iCommonBridge.CanGeneratePrefab())
            {

                iCommonBridge.GeneratePrefabProperty = EditorGUILayout.Toggle(generatePrefabContent, iCommonBridge.GeneratePrefabProperty);
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.Toggle(generatePrefabContent, iCommonBridge.GeneratePrefabProperty);
                GUI.enabled = previousGUIState;
                GUIStyle remarkStyle = new GUIStyle();
                remarkStyle.fontStyle = FontStyle.Italic;
                remarkStyle.fontSize = 10;
                GUILayout.Label("Prefab Generation is only active for premium version.", remarkStyle);

            }


            if (iCommonBridge.GeneratePrefabProperty)
            {
                GUILayout.BeginVertical("box");
                // Saving previous GUI enabled value
                previousGUIState = GUI.enabled;
                // Disabling edit for property
                GUI.enabled = false;
                // Drawing Property
                GUIContent prefabFolderContent = new GUIContent("Generated Prefab Folder", "The folder where the generated prefab will be stored.");
                EditorGUILayout.TextField(prefabFolderContent, iCommonBridge.AssetGenerationFolderProperty);

                // Setting old GUI enabled value
                GUI.enabled = previousGUIState;
                GUIContent getFolderContent = new GUIContent("Specify Prefab Folder", "Allows to specify the folder where the generated prefab will be stored.");

                bool buttonGetPrefabFolder = GUILayout.Button(getFolderContent);
                if (buttonGetPrefabFolder)
                {
                    string absolutePath = EditorUtility.OpenFolderPanel("Prefab generation Folder", "Assets", "");


                    if (absolutePath.StartsWith(Application.dataPath))
                    {
                        iCommonBridge.AssetGenerationFolderProperty = "Assets" + absolutePath.Substring(Application.dataPath.Length);
                    }
                    else if (absolutePath != "")
                    {
                        EditorUtility.DisplayDialog("Error", "The chosen folder must be inside the Assets folder.", "Ok");

                    }


                }
                GUILayout.EndVertical();

            }


        }

        /// <summary>
        /// Manages the bridge generation part of the bridge editor.
        /// </summary>
        /// <param name="target">The target bridge script component.</param>
        /// <param name="quadMesh">The quad mesh that simulates the bridge location.</param>
        /// <param name="theTerrain">The terrain.</param>
        /// <param name="lineManager">The line manager script.</param>
        /// <param name="extractOneLod">The extract one lod script.</param>
        public static void ManageBridgeGeneration(Object target, QuadMeshProto quadMesh, Terrain theTerrain, LineManagerProto lineManager, ExtractOneLodProto extractOneLod)
        {
            ICommonBridgeProto iCommonBridge = (ICommonBridgeProto)target;
            if (iCommonBridge.PointsProperty.Count == 2 && !iCommonBridge.TargetTransformProperty.Find("Bridge_instance") && !iCommonBridge.TargetTransformProperty.Find("Bridge_instance_noLod"))
            {
                GUIContent generateMeshContent = new GUIContent("Generate Bridge", "Generate the specified bridge.");
                var oldColor = GUI.backgroundColor;
                GUI.backgroundColor = Color.green;
                bool buttonGenerateMesh = GUILayout.Button(generateMeshContent, GUILayout.MinHeight(40));
                GUI.backgroundColor = oldColor;
                EditorGUILayout.Space();
                if (buttonGenerateMesh)
                {
                    if (LogManager.EnableLog) LogManager.LogMessage("CommonBridgeEditor " + "step05");
                    Undo.SetCurrentGroupName("Generate Bridge");
                    int undoGroupIndex = Undo.GetCurrentGroup();
                    try
                    {
                        if (iCommonBridge.GenerateBridge())
                        {
                            BridgePoints bridgePoints;
                            if (iCommonBridge.TargetTransformProperty.TryGetComponent<BridgePoints>(out bridgePoints))
                            {
                                bridgePoints.points = new Vector3[2];
                                bridgePoints.points[0] = iCommonBridge.PointsProperty[0];
                                bridgePoints.points[1] = iCommonBridge.PointsProperty[1];
                                bridgePoints.width = iCommonBridge.BridgeWidthproperty;
                                TerrainManagerProto terrainManager = new TerrainManagerProto();
                                bridgePoints.points[0].y = terrainManager.GetElevationAt(bridgePoints.points[0]);
                                bridgePoints.points[1].y = bridgePoints.points[0].y;
                                bridgePoints.ElevationCorrection = iCommonBridge.ElevationCorrectionProperty;
                            }
                            else
                            {
                                bridgePoints = iCommonBridge.TargetTransformProperty.gameObject.AddComponent<BridgePoints>();
                                bridgePoints.points = new Vector3[2];
                                bridgePoints.points[0] = iCommonBridge.PointsProperty[0];
                                bridgePoints.points[1] = iCommonBridge.PointsProperty[1];
                                bridgePoints.width = iCommonBridge.BridgeWidthproperty;
                                TerrainManagerProto terrainManager = new TerrainManagerProto();
                                bridgePoints.points[0].y = terrainManager.GetElevationAt(bridgePoints.points[0]);
                                bridgePoints.points[1].y = bridgePoints.points[0].y;
                                bridgePoints.ElevationCorrection = iCommonBridge.ElevationCorrectionProperty;
                            }
                            if (bridgePoints != null && (iCommonBridge.LevelTerrainAtStartProperty || iCommonBridge.LevelTerrainAtEndProperty))
                            {
                                if (BridgesInterferencesFinder.FindInterferences(iCommonBridge.TargetTransformProperty.gameObject, iCommonBridge.BridgeWidthproperty, iCommonBridge.PointsProperty[0], iCommonBridge.PointsProperty[1], iCommonBridge.StartAjustmentLengthProperty, iCommonBridge.EndAjustmentLengthProperty))
                                {
                                    EditorUtility.DisplayDialog("Warning", "There may be interferences between this bridge terrain modifications and existing bridges. In this case, uncheck the 'Automatic Adjustment' checkboxes and specify the adjustment distances manually.\n See the video tutorial on this topic.", "Ok");
                                }
                            }
                            quadMesh.DisableQuadObject();

                        }
                        Undo.CollapseUndoOperations(undoGroupIndex);
                    }

                    catch (System.Exception generationException)
                    {
                        EditorUtility.DisplayDialog("Error", "There was an issue in generating the bridge. Please, see the Console Window", "Ok");
                        Debug.LogError("Exception: " + generationException.Message);
                        if (LogManager.EnableLog) LogManager.LogMessage("CommonBridgeEditor Exception: " + generationException.Message);
                        Undo.PerformUndo();
                    }



                    GUIUtility.ExitGUI();
                }
            }
            else if (iCommonBridge.TargetTransformProperty.Find("Bridge_instance") || iCommonBridge.TargetTransformProperty.Find("Bridge_instance_noLod"))
            {
                var oldColor = GUI.backgroundColor;
                GUI.backgroundColor = Color.red;
                GUIContent deleteMeshContent = new GUIContent("Delete Bridge*", "Delete the generated bridge.");
                bool buttonDeleteBridge = GUILayout.Button(deleteMeshContent, GUILayout.MinHeight(40));
                GUI.backgroundColor = oldColor;

                EditorGUILayout.HelpBox("* The bridge will be deleted, but the terrain will be left as is." + System.Environment.NewLine + "Use undo (CTRL-Z) instead to also restore the terrain in its previous state." + System.Environment.NewLine + "Successive bridge generations with the MODIFY TERRAIN option checked may lead to terrain disturbances.", MessageType.Info);
                EditorGUILayout.Space();
                if (buttonDeleteBridge)
                {
                    if (LogManager.EnableLog) LogManager.LogMessage("CommonBridgeEditor " + "step06");
                    bool confirmDelete = EditorUtility.DisplayDialog("Condirmation", "Do you confirm generated bridge deletion ? ", "Ok", "No");
                    if (confirmDelete)
                    {
                        Undo.SetCurrentGroupName("Delete Bridge");
                        int undoGroupIndex = Undo.GetCurrentGroup();
                        iCommonBridge.DeleteBridge();
                        if (iCommonBridge.PointsProperty.Count == 2) quadMesh.EnableQuadObject();
                        Undo.CollapseUndoOperations(undoGroupIndex);
                        if (iCommonBridge.PointsProperty.Count == 2 && theTerrain != null)
                        {
                            lineManager.CalculateElevationDifference();
                        }


                        GUIUtility.ExitGUI();
                    }


                }
                EditorGUILayout.BeginVertical("box");
                GUILayout.Label("Polygons count:");
                GUILayout.Label(iCommonBridge.PolygonsCountTextProperty, EditorStyles.wordWrappedLabel);
                EditorGUILayout.Space();

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();


                if (iCommonBridge.TargetTransformProperty.Find("Bridge_instance"))
                {
                    if (extractOneLod == null)
                    {
                        extractOneLod = new ExtractOneLodProto();
                    }
                    EditorGUILayout.BeginVertical("box");
                    GUILayout.Label("Lod Extraction");
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("This allows to keep only one Lod. For instance, for a LOW-POLY bridge, choose Lod 2", MessageType.Info);
                    string[] lodOptions = new string[] { "Lod 0", "Lod 1", "Lod 2" };


                    if (iCommonBridge.CanExtractLod())
                    {

                        iCommonBridge.LodIndexProperty = EditorGUILayout.Popup("Lod to keep :", iCommonBridge.LodIndexProperty, lodOptions);
                        GUIContent keepOneLodContent = new GUIContent("Apply choice", "Keeps only one LOD, for instance the lowest one to get a Low Poly bridge.");
                        bool buttonKeepOneLod = GUILayout.Button(keepOneLodContent, GUILayout.MinHeight(40));
                        if (buttonKeepOneLod)
                        {
                            if (LogManager.EnableLog) LogManager.LogMessage("CommonBridgeEditor " + "step07");
                            if (EditorUtility.DisplayDialog("Confirmation", "You will keep " + lodOptions[iCommonBridge.LodIndexProperty] + ". The other Lods will be deleted.", "Ok", "Cancel"))
                            {
                                Undo.IncrementCurrentGroup();
                                Undo.SetCurrentGroupName("Extract Lod");
                                int undoGroupIndex = Undo.GetCurrentGroup();


                                int triangles = extractOneLod.ExtractLod(target, iCommonBridge.LodIndexProperty, iCommonBridge.GeneratePrefabProperty);
                                if (triangles != 0)
                                {
                                    Undo.RecordObject(target, "Triangle count");
                                    iCommonBridge.PolygonsCountTextProperty = "Triangles count: " + triangles;
                                }
                                Undo.CollapseUndoOperations(undoGroupIndex);

                            }

                        }
                    }
                    else
                    {
                        var previousGUIState = GUI.enabled;
                        GUI.enabled = false;
                        iCommonBridge.LodIndexProperty = EditorGUILayout.Popup("Lod to keep :", iCommonBridge.LodIndexProperty, lodOptions);
                        GUIContent keepOneLodContent = new GUIContent("Apply choice", "Keeps only one LOD, for instance the lowest one to get a Low Poly bridge.");
                        GUILayout.Button(keepOneLodContent, GUILayout.MinHeight(40));
                        GUI.enabled = previousGUIState;
                        GUIStyle remarkStyle = new GUIStyle();
                        remarkStyle.fontStyle = FontStyle.Italic;
                        remarkStyle.fontSize = 10;
                        GUILayout.Label("Lod extraction is only active for premium version.", remarkStyle);

                    }


                    EditorGUILayout.EndVertical();
                }




            }

        }
    }
}