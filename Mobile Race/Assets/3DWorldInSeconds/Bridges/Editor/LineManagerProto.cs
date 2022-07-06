/*--------------------------------------------------------
  LineManagerProto.cs

  Created by Alain Debelley on 2021-12-22.
  Copyright (c) 2021 ALAIN DEBELLEY. All rights reserved.
  http://www.3dworldinseconds.com/
--------------------------------------------------------*/


using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

namespace WorldInSeconds3DProto
{
    public class LineManagerProto
    {
        private UnityEngine.Object _myTarget;
        private ICommonBridgeProto commonBridgeInterface;

        private QuadMeshProto quadMesh;
        private Terrain theTerrain;


        private string currentPointName;
        private enum CurrentAction
        {
            Navigate,
            AddAction,
            MoveAction,
            DeleteAction
        }
        private CurrentAction currentAction;
        private int selected;
        private bool displayElevationDifference;
        private float elevationDifference = 0;
        private int currentIndexForMove = -1;
        private Texture nodeTexture;
        private static GUIStyle handleStyle = new GUIStyle();

        public LineManagerProto(UnityEngine.Object target, QuadMeshProto aQuadMesh, Terrain terrain)
        {
            this._myTarget = target;
            commonBridgeInterface = (ICommonBridgeProto)target;
            this.quadMesh = aQuadMesh;
            this.theTerrain = terrain;
            this.nodeTexture = Resources.Load<Texture>("Handle");
            if (nodeTexture == null) nodeTexture = EditorGUIUtility.whiteTexture;
            handleStyle.alignment = TextAnchor.MiddleCenter;
            handleStyle.fixedWidth = 30;
            handleStyle.fixedHeight = 30;

            selected = 0;
            currentAction = CurrentAction.Navigate;
            if (displayElevationDifference && commonBridgeInterface.PointsProperty != null && commonBridgeInterface.PointsProperty.Count == 2)
            {
                float startElevation = theTerrain.SampleHeight(commonBridgeInterface.PointsProperty[0]);
                float endElevation = theTerrain.SampleHeight(commonBridgeInterface.PointsProperty[1]);
                elevationDifference = endElevation - startElevation;
            }
        }

        public void SceneManager()
        {
            Transform bridgeInstance = commonBridgeInterface.TargetTransformProperty.Find("Bridge_instance");
            Transform bridgeInstanceNoLod = commonBridgeInterface.TargetTransformProperty.Find("Bridge_instance_noLod");

            if (bridgeInstance == null & bridgeInstanceNoLod == null)
            {
                if (quadMesh.width != commonBridgeInterface.BridgeWidthproperty)
                {
                    quadMesh.width = commonBridgeInterface.BridgeWidthproperty;
                    if (commonBridgeInterface.PointsProperty.Count == 2)
                    {
                        quadMesh.startPoint = commonBridgeInterface.PointsProperty[0];
                        quadMesh.endPoint = commonBridgeInterface.PointsProperty[1];
                        quadMesh.PlaceQuadObject();
                    }

                }
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                // 2D operations
                Handles.BeginGUI();
                GUILayout.BeginArea(new Rect(80, 10, 380, 60));

                var rect = EditorGUILayout.BeginVertical();

                GUI.Box(rect, GUIContent.none);

                GUI.color = Color.white;

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUIStyle titleStyle = new GUIStyle();
                titleStyle.normal.textColor = Color.black;
                GUILayout.Label("Draw Bridge start and end Points", titleStyle);
                GUILayout.FlexibleSpace();
                GUIStyle currentPointStyle = new GUIStyle();
                currentPointStyle.normal.textColor = Color.red;
                currentPointStyle.fontStyle = FontStyle.Bold;
                GUILayout.Label(currentPointName, currentPointStyle);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();

                GUIContent navigateContent = new GUIContent("Navigate", "Navigate freely in the scene and stop any drawing.");
                GUIContent addContent = new GUIContent("Add Point", "Add the first or last point.");
                GUIContent moveContent = new GUIContent("Move Point", "Drag a point of the bridge.");
                GUIContent deleteContent = new GUIContent("Delete Point", "Delete a point by clicking on it.");
                List<GUIContent> contentsList = new List<GUIContent>();
                List<CurrentAction> actionsList = new List<CurrentAction>();
                contentsList.Add(navigateContent);
                actionsList.Add(CurrentAction.Navigate);

                if (commonBridgeInterface.PointsProperty.Count < 2)
                {
                    contentsList.Add(addContent);
                    actionsList.Add(CurrentAction.AddAction);
                }
                if (commonBridgeInterface.PointsProperty.Count > 0)
                {
                    contentsList.Add(moveContent);
                    contentsList.Add(deleteContent);
                    actionsList.Add(CurrentAction.MoveAction);
                    actionsList.Add(CurrentAction.DeleteAction);
                }
                GUIContent[] options = contentsList.ToArray();
                selected = actionsList.IndexOf(currentAction, 0);
                if (selected >= actionsList.Count) selected = 0;


                selected = GUILayout.SelectionGrid(selected, options, options.Length);
                currentAction = actionsList[selected];

                switch (currentAction)
                {
                    case CurrentAction.Navigate:
                        if (commonBridgeInterface.PointsProperty.Count < 2)
                        {
                            displayElevationDifference = false;


                        }
                        else if (commonBridgeInterface.PointsProperty.Count == 2)
                        {
                            displayElevationDifference = true;
                        }
                        currentPointName = "";
                        break;
                    case CurrentAction.AddAction:

                        break;
                    case CurrentAction.MoveAction:
                        if (commonBridgeInterface.PointsProperty.Count < 2)
                        {
                            displayElevationDifference = false;
                        }
                        else
                        {
                            displayElevationDifference = true;
                        }
                        break;
                    case CurrentAction.DeleteAction:
                        displayElevationDifference = false;
                        currentPointName = "";
                        break;

                }


                GUILayout.EndHorizontal();

                if (displayElevationDifference)
                {
                    GUILayout.BeginHorizontal();
                    GUIStyle textStyle = new GUIStyle();
                    textStyle.normal.textColor = Color.blue;
                    string textToDisplay = "";
                    string textElevation = Mathf.Abs(elevationDifference).ToString("F1") + " m";
                    if (elevationDifference <= 0.1f && elevationDifference >= -0.1f)
                    {
                        textToDisplay = "The start terrain has the same elevation as the end terrain";
                    }
                    else if (elevationDifference < -0.1f)
                    {
                        textToDisplay = "The start terrain is " + textElevation + " HIGHER than the end terrain";
                    }
                    else if (elevationDifference > 0.1f)
                    {
                        textToDisplay = "The start terrain is " + textElevation + " LOWER than the end terrain";
                    }
                    GUILayout.Label(textToDisplay, textStyle);

                    GUILayout.EndHorizontal();
                }


                EditorGUILayout.EndVertical();

                GUILayout.EndArea();




                Handles.EndGUI();

                // 3D operations
                DrawNodes(commonBridgeInterface.PointsProperty);
                DrawLine(commonBridgeInterface.PointsProperty);

                if (commonBridgeInterface.PointsProperty.Count == 0 && currentAction == CurrentAction.AddAction)
                {
                    if (Event.current.type == EventType.MouseDown && !Event.current.shift && !Event.current.control && Event.current.button != 2)
                    {
                        Vector3 mousePos;
                        Ray rayFromMouse = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                        if (theTerrain == null)
                        {
                            mousePos = rayFromMouse.origin;
                        }
                        else
                        {
                            TerrainCollider terrainCollider = theTerrain.GetComponent<TerrainCollider>();
                            RaycastHit hit;
                            if (terrainCollider.Raycast(rayFromMouse, out hit, Mathf.Infinity))
                            {
                                mousePos = hit.point;
                            }
                            else mousePos = rayFromMouse.origin;
                        }

                        Undo.RecordObject(_myTarget, "Add Point");
                        commonBridgeInterface.PointsProperty.Add(mousePos);
                        Event.current.Use();
                        displayElevationDifference = false;

                    }
                }




                if (!Event.current.shift && !Event.current.control && Event.current.button != 2)
                {
                    if (currentAction == CurrentAction.AddAction && commonBridgeInterface.PointsProperty.Count == 1)
                    {

                        if (Event.current.mousePosition.x > 0 && Event.current.mousePosition.y > 0 && Event.current.mousePosition.x < Screen.width && Event.current.mousePosition.y < Screen.height)
                        {

                            Vector3 mousePos;
                            Ray rayFromMouse = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                            if (theTerrain == null)
                            {
                                mousePos = rayFromMouse.origin;
                            }
                            else
                            {
                                TerrainCollider terrainCollider = theTerrain.GetComponent<TerrainCollider>();
                                RaycastHit hit;
                                if (terrainCollider.Raycast(rayFromMouse, out hit, Mathf.Infinity))
                                {
                                    mousePos = hit.point;
                                }
                                else mousePos = rayFromMouse.origin;


                            }


                            Vector3 lastPoint = commonBridgeInterface.PointsProperty[0];
                            float handleSize = HandleUtility.GetHandleSize(lastPoint);
                            Handles.DrawLine(lastPoint, mousePos);
                            if (commonBridgeInterface.PointsProperty.Count == 1)
                            {
                                displayElevationDifference = true;
                                float startElevation = theTerrain.SampleHeight(commonBridgeInterface.PointsProperty[0]);
                                float endElevation = theTerrain.SampleHeight(mousePos);
                                elevationDifference = endElevation - startElevation;

                                currentPointName = "End Point";
                            }


                            if (Handles.Button(mousePos, Quaternion.identity, handleSize * 0.1f, handleSize * 1.5f, HandleFunction))
                            {

                                Undo.RecordObject(_myTarget, "Add Node");
                                commonBridgeInterface.PointsProperty.Add(mousePos);
                                Event.current.Use();
                                currentAction = CurrentAction.Navigate;
                                quadMesh.startPoint = commonBridgeInterface.PointsProperty[0];
                                quadMesh.endPoint = commonBridgeInterface.PointsProperty[1];
                                quadMesh.PlaceQuadObject();
                                quadMesh.EnableQuadObject();

                            }
                        }


                    }
                    else if (currentAction == CurrentAction.DeleteAction && commonBridgeInterface.PointsProperty.Count > 0)
                    {
                        float handleSize = HandleUtility.GetHandleSize(commonBridgeInterface.PointsProperty[0]);
                        for (int i = 0; i < commonBridgeInterface.PointsProperty.Count; i++)
                        {

                            if (Handles.Button(commonBridgeInterface.PointsProperty[i], Quaternion.identity, handleSize * 0.1f, handleSize * 1.5f, DeleteHandleFunction))
                            {
                                Undo.RecordObject(_myTarget, "Remove Point");
                                commonBridgeInterface.PointsProperty.RemoveAt(i);
                                quadMesh.DisableQuadObject();
                                Event.current.Use();
                                if (commonBridgeInterface.PointsProperty.Count == 0)
                                {
                                    currentAction = CurrentAction.Navigate;

                                }
                            }
                        }
                    }


                }
            }
            else
            {
                if (bridgeInstance != null || bridgeInstanceNoLod != null)
                    quadMesh.DisableQuadObject();
            }


        }
        /// <summary>
        /// Actions to perform when the gameobject is selected instance is selected.
        /// </summary>
        public void IsSelected()
        {
            if (Selection.activeTransform == commonBridgeInterface.TargetTransformProperty)
            {

                if (commonBridgeInterface.PointsProperty.Count == 2 && theTerrain != null)
                {
                    float startElevation = theTerrain.SampleHeight(commonBridgeInterface.PointsProperty[0]);
                    float endElevation = theTerrain.SampleHeight(commonBridgeInterface.PointsProperty[1]);
                    elevationDifference = endElevation - startElevation;
                }
            }

        }
        /// <summary>
        /// Actions to perform when there is an Undo or a Redo
        /// </summary>
        public void UndoRedoCallBack()
        {
            if (LogManager.EnableLog) LogManager.LogMessage("LineManager Call UndoRedoCallBack");
            if (quadMesh != null)
            {
                if (commonBridgeInterface.PointsProperty.Count == 2)
                {

                    if (quadMesh.QuadObject != null)
                    {
                        quadMesh.EnableQuadObject();
                        quadMesh.PlaceQuadObject();
                        GameObject bridgeInstance = GameObject.Find("/Bridge_instance");
                        if (bridgeInstance != null)
                        {
                            if (LogManager.EnableLog) LogManager.LogMessage("LineManager Call UndoRedoCallBack Destroy bridge");
                            GameObject.DestroyImmediate(bridgeInstance);
                            Undo.ClearAll();
                        }
                    }
                    quadMesh.startPoint = commonBridgeInterface.PointsProperty[0];
                    quadMesh.endPoint = commonBridgeInterface.PointsProperty[1];
                    if (theTerrain != null)
                    {
                        float startElevation = theTerrain.SampleHeight(commonBridgeInterface.PointsProperty[0]);
                        float endElevation = theTerrain.SampleHeight(commonBridgeInterface.PointsProperty[1]);
                        elevationDifference = endElevation - startElevation;
                    }

                }
                else
                {
                    quadMesh.DisableQuadObject();
                }
            }


        }

        /// <summary>
        /// Calculates the elevation difference between the bridge start and the bridge end.
        /// </summary>
        public void CalculateElevationDifference()
        {
            float startElevation = theTerrain.SampleHeight(commonBridgeInterface.PointsProperty[0]);
            float endElevation = theTerrain.SampleHeight(commonBridgeInterface.PointsProperty[1]);
            elevationDifference = endElevation - startElevation;
        }

        /// <summary>
        /// Draws the line points.
        /// </summary>
        /// <param name="pointsList">The points list.</param>
        private void DrawNodes(List<Vector3> pointsList)
        {
            if (theTerrain != null)
            {
                for (int i = 0; i < pointsList.Count; i++)
                {

                    Vector3 pos = pointsList[i];
                    float handleSize = HandleUtility.GetHandleSize(pos);
                    if (currentAction == CurrentAction.MoveAction) currentIndexForMove = i;
                    Vector3 newPos = Handles.FreeMoveHandle(pos, Quaternion.identity, handleSize * 2, Vector3.one, HandleFunction);

                    if (newPos != pos && currentAction != CurrentAction.Navigate && currentAction != CurrentAction.DeleteAction)
                    {
                        Vector3 snapPos = newPos;
                        if (theTerrain != null) snapPos.y = theTerrain.transform.position.y + theTerrain.SampleHeight(snapPos);

                        Undo.RecordObject(_myTarget, "Move Point");

                        commonBridgeInterface.PointsProperty[i] = snapPos;
                        if (commonBridgeInterface.PointsProperty.Count == 2)
                        {
                            displayElevationDifference = true;
                            float startElevation = theTerrain.SampleHeight(commonBridgeInterface.PointsProperty[0]);
                            float endElevation = theTerrain.SampleHeight(commonBridgeInterface.PointsProperty[1]);
                            elevationDifference = endElevation - startElevation;
                            quadMesh.startPoint = commonBridgeInterface.PointsProperty[0];
                            quadMesh.endPoint = commonBridgeInterface.PointsProperty[1];
                            quadMesh.PlaceQuadObject();
                            quadMesh.EnableQuadObject();
                        }
                        else
                        {
                            displayElevationDifference = false;
                            quadMesh.DisableQuadObject();
                        }

                    }
                }
            }
            else
            {
                Debug.LogError("Error. It is not possible to draw bridge points if there is no active terrain");
            }


        }

        /// <summary>
        /// Draws the line between the bridge start and the bridge end.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        private void DrawLine(List<Vector3> nodes)
        {

            if (currentAction == CurrentAction.AddAction) Handles.color = Color.green;
            else if (currentAction == CurrentAction.DeleteAction) Handles.color = Color.red;

            else Handles.color = Color.white;
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                Handles.DrawLine(nodes[i], nodes[i + 1]);

            }
            Handles.color = Color.white;
        }

        /// <summary>
        /// Actions to perform when a handle is selected.
        /// </summary>
        /// <param name="controlID">The control identifier.</param>
        /// <param name="position">The handle position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="size">The size.</param>
        /// <param name="eventType">Type of the event.</param>
        private void HandleFunction(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
        {
            if (controlID == GUIUtility.hotControl && currentAction != CurrentAction.Navigate)
            {
                GUI.color = Color.red;
                if (currentAction == CurrentAction.MoveAction)
                {
                    if (currentIndexForMove == 0) currentPointName = "Start Point";
                    else if (currentIndexForMove == 1) currentPointName = "End Point";
                }
            }
            else
            {
                GUI.color = Color.green;
            }



            Handles.Label(position, new GUIContent(nodeTexture), handleStyle);
            GUI.color = Color.white;
            Handles.DotHandleCap(controlID, position, rotation, 0, eventType);
        }
        /// <summary>
        ///  Actions to perform when a handle is selected for deletion.
        /// </summary>
        /// <param name="controlID">The control identifier.</param>
        /// <param name="position">The handle position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="size">The handle size.</param>
        /// <param name="eventType">Type of the event.</param>
        private void DeleteHandleFunction(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
        {
            if (LogManager.EnableLog) LogManager.LogMessage("LineManager Call DeleteHandleFunction");
            GUI.color = Color.red;
            Handles.Label(position, new GUIContent(nodeTexture), handleStyle);
            GUI.color = Color.white;
            Handles.DotHandleCap(controlID, position, rotation, 0, eventType);
        }



    }
}