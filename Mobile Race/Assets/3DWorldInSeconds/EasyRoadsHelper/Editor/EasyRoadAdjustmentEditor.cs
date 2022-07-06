/*--------------------------------------------------------
  EasyRoadAdjustmentEditor.cs

  Created by Alain Debelley on 2021-12-22.
  Copyright (c) 2021 ALAIN DEBELLEY. All rights reserved.
  http://www.3dworldinseconds.com/

  EasyRoads3D v3 is a trade mark of AndaSoft
  3DWorldInseconds is not responsible of its operation
  Please note that this helper may not work or even compile if the EasyRoads3D v3 interface is changed.
  UNCOMMENT "#define EasyRoadInstalled" only if this product is installed.
--------------------------------------------------------*/
#define EasyRoadInstalled

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if EasyRoadInstalled
using EasyRoads3Dv3;
#endif
namespace WorldInSeconds3DCommon
{
    [CustomEditor(typeof(EasyRoadAdjustment))]
    public class EasyRoadAdjustmentEditor : Editor
    {
        private enum RoadBridgeRelation
        {
            RoadStart_BridgeStart,
            RoadStart_BridgeEnd,
            RoadEnd_BridgeStart,
            RoadEnd_BridgeEnd,
            RoadCrossesBridge
        }
        private class BridgeRecord
        {
            public GameObject bridge;
            public RoadBridgeRelation relation;
            public int crossMarkerIndex = -1;
            public int bridgeCrossStartIndex = -1;
            public BridgeRecord(GameObject bridge, RoadBridgeRelation relation, int crossMarkerIndex = -1, int bridgeCrossStartIndex = -1)
            {
                this.bridge = bridge;
                this.relation = relation;
                this.crossMarkerIndex = crossMarkerIndex;
                this.bridgeCrossStartIndex = bridgeCrossStartIndex;
            }
        }
        private EasyRoadAdjustment _myTarget;

        GUIStyle redLabel;
        GUIStyle blueLabel;
        int selectedIndex;
        GameObject[] bridgesArray;
        void OnEnable()
        {
            _myTarget = (EasyRoadAdjustment)target;
            RefreshRoadData();
            redLabel = new GUIStyle();
            redLabel.normal.textColor = Color.red;
            redLabel.wordWrap = true;
            
            blueLabel = new GUIStyle();
            blueLabel.normal.textColor = Color.blue;
            blueLabel.wordWrap = true;

            selectedIndex = 0;
        }

        private void RefreshRoadData()
        {

#if EasyRoadInstalled
            _myTarget.RoadNetWorkTransform = _myTarget.transform.root;
            if (_myTarget.transform.TryGetComponent<ERModularRoad>(out _myTarget.RoadData) && _myTarget.RoadNetWorkTransform.TryGetComponent<ERModularBase>(out _myTarget.RoadNetWork))
            {
                _myTarget.RoadNetworkRaise = _myTarget.RoadNetWork.raise;
                _myTarget.Markers = _myTarget.RoadData.markersExt;
                if (_myTarget.Markers.Count > 1)
                {
                    _myTarget.startPosition = _myTarget.Markers[0].position;
                    _myTarget.endPosition = _myTarget.Markers[_myTarget.Markers.Count - 1].position;
                }

            }

#endif
        }

        public override void OnInspectorGUI()
        {
#if EasyRoadInstalled
            RefreshRoadData();

            if (_myTarget.RoadData != null && _myTarget.RoadNetWork != null)
            {
                if (_myTarget.Markers != null && _myTarget.Markers.Count > 4)
                {
                    GUIContent lookForBridgesCrossingContent = new GUIContent("Look for crossed bridges", "Look for briges that are crossed by the road.");
                    _myTarget.CrossingBridges = EditorGUILayout.Toggle(lookForBridgesCrossingContent, _myTarget.CrossingBridges);
                    GUIContent crossingRoadRaiseContent = new GUIContent("Raise over crossed bridge", "At which height is the road over crossed bridges.");
                    if (_myTarget.CrossingBridges)
                    {
                        _myTarget.CrossingRoadRaise = EditorGUILayout.FloatField(crossingRoadRaiseContent, _myTarget.CrossingRoadRaise);
                    }
                    GameObject[] bridgesArray = GameObject.FindGameObjectsWithTag("bridge");
                    List<BridgeRecord> bridgesList = new List<BridgeRecord>();
                    foreach (GameObject bridge in bridgesArray)
                    {
                        BridgePoints bridgePoints;
                        if (bridge.TryGetComponent<BridgePoints>(out bridgePoints))
                        {
                            Vector2 bridgeStart2D = new Vector2(bridgePoints.points[0].x, bridgePoints.points[0].z);
                            Vector2 bridgeEnd2D = new Vector2(bridgePoints.points[1].x, bridgePoints.points[1].z);
                            Vector2 roadStart2D = new Vector2(_myTarget.startPosition.x, _myTarget.startPosition.z);
                            Vector2 roadEnd2D = new Vector2(_myTarget.endPosition.x, _myTarget.endPosition.z);
                            float distanceFromStartToBridgeStart = Vector2.Distance(roadStart2D, bridgeStart2D);
                            float distanceFromStartToBridgeEnd = Vector2.Distance(roadStart2D, bridgeEnd2D);
                            float distanceFromEndToBridgeStart = Vector2.Distance(roadEnd2D, bridgeStart2D);
                            float distanceFromEndToBridgeEnd = Vector2.Distance(roadEnd2D, bridgeEnd2D);
                            if (distanceFromStartToBridgeStart <= 1.5f)
                            {
                                BridgeRecord bridgeRelation = new BridgeRecord(bridge, RoadBridgeRelation.RoadStart_BridgeStart);
                                bridgesList.Add(bridgeRelation);

                            }
                            else if (distanceFromStartToBridgeEnd <= 1.5f)
                            {
                                BridgeRecord bridgeRelation = new BridgeRecord(bridge, RoadBridgeRelation.RoadStart_BridgeEnd);
                                bridgesList.Add(bridgeRelation);
                            }
                            else if (distanceFromEndToBridgeStart <= 1.5f)
                            {
                                BridgeRecord bridgeRelation = new BridgeRecord(bridge, RoadBridgeRelation.RoadEnd_BridgeStart);
                                bridgesList.Add(bridgeRelation);
                            }
                            else if (distanceFromEndToBridgeEnd <= 1.5f)
                            {
                                BridgeRecord bridgeRelation = new BridgeRecord(bridge, RoadBridgeRelation.RoadEnd_BridgeEnd);
                                bridgesList.Add(bridgeRelation);
                            }
                            else
                            {
                                if (_myTarget.CrossingBridges)
                                {
                                    for (int i = 0; i < _myTarget.Markers.Count - 1; i++)
                                    {
                                        Vector2 marker2D = new Vector2(_myTarget.Markers[i].position.x, _myTarget.Markers[i].position.z);
                                        Vector2 nextMarker2D = new Vector2(_myTarget.Markers[i + 1].position.x, _myTarget.Markers[i + 1].position.z);
                                        float distanceMarkerToBridgeStart = Vector2.Distance(marker2D, bridgeStart2D);
                                        float distanceNextMarkerToBridgeEnd = Vector2.Distance(nextMarker2D, bridgeEnd2D);
                                        float distanceMarkerToBridgeEnd = Vector2.Distance(marker2D, bridgeEnd2D);
                                        float distanceNextMarkerToBridgeStart = Vector2.Distance(nextMarker2D, bridgeStart2D);
                                        if (distanceMarkerToBridgeStart < 1.5f && distanceNextMarkerToBridgeEnd < 1.5f)
                                        {
                                            BridgeRecord bridgeRelation = new BridgeRecord(bridge, RoadBridgeRelation.RoadCrossesBridge, i, 0);
                                            bridgesList.Add(bridgeRelation);
                                            break;
                                        }
                                        else if (distanceMarkerToBridgeEnd < 1.5f && distanceNextMarkerToBridgeStart < 1.5f)
                                        {
                                            BridgeRecord bridgeRelation = new BridgeRecord(bridge, RoadBridgeRelation.RoadCrossesBridge, i, 1);
                                            bridgesList.Add(bridgeRelation);
                                            break;
                                        }

                                    }
                                }


                            }
                        }
                    }

                    string[] bridgesName = new string[bridgesList.Count];
                    for (int i = 0; i < bridgesList.Count; i++)
                    {
                        bridgesName[i] = bridgesList[i].bridge.name;
                    }
                    EditorGUILayout.LabelField("Select a bridge to analyze :");
                    selectedIndex = EditorGUILayout.Popup(selectedIndex, bridgesName);

                    if (GUILayout.Button(new GUIContent("Analyze the selected bridge",
                              "Analyze the relation between the road and of the selected bridge.")))
                    {
                        _myTarget.AnalysedIndex = selectedIndex;
                        _myTarget.AtLeastAnIssue = false;
                    }
                    if (bridgesList.Count > 0 && _myTarget.AnalysedIndex >= 0 && _myTarget.AnalysedIndex < bridgesList.Count)
                    {
                        EditorGUILayout.BeginVertical("box");
                        BridgeRecord selectedBridgeRecord = bridgesList[_myTarget.AnalysedIndex];

                        GUILayout.Label(selectedBridgeRecord.bridge.name);
                        int bridgePointAIndex = 0;
                        int bridgePointBIndex = 0;
                        int roadPointAIndex = 0;
                        int roadPointBIndex = 0;
                        switch (selectedBridgeRecord.relation)
                        {
                            case RoadBridgeRelation.RoadStart_BridgeEnd:
                                roadPointAIndex = 0;
                                roadPointBIndex = 1;
                                bridgePointAIndex = 1;
                                bridgePointBIndex = 0;
                                break;
                            case RoadBridgeRelation.RoadStart_BridgeStart:
                                roadPointAIndex = 0;
                                roadPointBIndex = 1;
                                bridgePointAIndex = 0;
                                bridgePointBIndex = 1;
                                break;
                            case RoadBridgeRelation.RoadEnd_BridgeStart:
                                roadPointAIndex = _myTarget.Markers.Count - 1;
                                roadPointBIndex = _myTarget.Markers.Count - 2;
                                bridgePointAIndex = 0;
                                bridgePointBIndex = 1;
                                break;
                            case RoadBridgeRelation.RoadEnd_BridgeEnd:
                                roadPointAIndex = _myTarget.Markers.Count - 1;
                                roadPointBIndex = _myTarget.Markers.Count - 2;
                                bridgePointAIndex = 1;
                                bridgePointBIndex = 0;
                                break;
                            case RoadBridgeRelation.RoadCrossesBridge:
                                roadPointAIndex = selectedBridgeRecord.crossMarkerIndex;
                                roadPointBIndex = selectedBridgeRecord.crossMarkerIndex + 1;
                                bridgePointAIndex = selectedBridgeRecord.bridgeCrossStartIndex;
                                if (bridgePointAIndex == 0) bridgePointBIndex = 1;
                                else bridgePointBIndex = 0;
                                break;
                        }

                        if (selectedBridgeRecord.relation != RoadBridgeRelation.RoadCrossesBridge) ManageBridge(selectedBridgeRecord.bridge, bridgePointAIndex, bridgePointBIndex, _myTarget.Markers, roadPointAIndex, roadPointBIndex);
                        else ManageBridgeCrossing(selectedBridgeRecord.bridge, bridgePointAIndex, bridgePointBIndex, _myTarget.Markers, roadPointAIndex, roadPointBIndex);

                        EditorGUILayout.EndVertical();
                    }

                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "For bridges adjustment, roads must have at least 5 points", "ok");
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No ERModularRoad component found on this Game Object", MessageType.Error);
            }

#else
            EditorGUILayout.HelpBox("If you have EasyRoads3D(TM) installed, the '#define EasyRoadInstalled' must be uncommented in EasyRoadAdjustmentEditor.cs and EasyRoadAdjustment.cs in 'Assets>3DWorldInSeconds>EasyRoadsHelper' folder.", MessageType.Error);
#endif
        }

#if EasyRoadInstalled
        private void ManageBridge(GameObject bridge, int bridgePointAIndex, int bridgePointBIndex, List<ERMarkerExt> markers, int roadPointAIndex, int roadPointBIndex)
        {
            BridgePoints bridgePoints;
            if (bridge.TryGetComponent<BridgePoints>(out bridgePoints))
            {
                Vector2 bridgePointA2D = new Vector2(bridgePoints.points[bridgePointAIndex].x, bridgePoints.points[bridgePointAIndex].z);
                Vector2 bridgePointB2D = new Vector2(bridgePoints.points[bridgePointBIndex].x, bridgePoints.points[bridgePointBIndex].z);
                Vector2 bridgeDirection2D = (bridgePointB2D - bridgePointA2D).normalized;
                Vector3 roadPointA = markers[roadPointAIndex].position;
                Vector3 roadPointB = markers[roadPointBIndex].position;
                Vector2 roadPointA2D = new Vector2(roadPointA.x, roadPointA.z);
                Vector2 roadPointB2D = new Vector2(roadPointB.x, roadPointB.z);
                Vector2 roadDirection2D = (roadPointB2D - roadPointA2D).normalized;
                float roadSegmentLength = Vector2.Distance(roadPointA, roadPointB);
                float distanceRoadPtA_BridgePtA = Vector2.Distance(roadPointA2D, bridgePointA2D);
                bool bridgeAndRoadAreAligned = false;
                if (roadDirection2D == bridgeDirection2D || roadDirection2D == -bridgeDirection2D) bridgeAndRoadAreAligned = true;
                bool atLeastAnIssue = false;
                if (distanceRoadPtA_BridgePtA > 0)
                {
                    GUILayout.Space(10);
                    GUILayout.Label("Issue: The road intersection segment start does not match the bridge first point", redLabel);
                    atLeastAnIssue = true;
                }
                if (_myTarget.Markers[roadPointAIndex].GetControlType() != ERMarkerControlType.StraightXZ)
                {
                    GUILayout.Space(10);
                    GUILayout.Label("Issue: The road first intersection point Control Type should be StraightXZ", redLabel);
                    atLeastAnIssue = true;
                }
                if (!PseudoEqual(roadPointA.y, bridgePoints.points[bridgePointAIndex].y - _myTarget.RoadNetworkRaise + bridgePoints.ElevationCorrection))

                {
                    GUILayout.Space(10);
                    GUILayout.Label("Issue: The road first intersection point is not at the right elevation", redLabel);
                    atLeastAnIssue = true;

                }
                if (roadSegmentLength < 1)
                {
                    GUILayout.Space(10);
                    GUILayout.Label("Issue: The road second point must be at least at 1m from the intersection point", redLabel);
                    atLeastAnIssue = true;
                }
                if (!bridgeAndRoadAreAligned)
                {
                    GUILayout.Space(10);
                    GUILayout.Label("Issue: The road first segment must be aligned with the bridge", redLabel);
                    atLeastAnIssue = true;
                }
                if (_myTarget.Markers[roadPointBIndex].splineStrength > 0.01f)
                {
                    GUILayout.Space(10);
                    GUILayout.Label("Issue: The road second point spline strength must be minimum", redLabel);
                    atLeastAnIssue = true;
                }
                if (atLeastAnIssue)
                {
                    _myTarget.AtLeastAnIssue = true;
                    GUIContent solveContent = new GUIContent("Solve Issues", "Solve the above issues.");
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(solveContent, GUILayout.Width(85)))
                    {
                        if (roadSegmentLength < 1)
                        {
                            Vector2 newSecondPoint2D = roadPointA2D + 1.1f * roadDirection2D;
                            Vector3 newPoint = new Vector3(newSecondPoint2D.x, roadPointB.y, newSecondPoint2D.y);
                            _myTarget.Markers[roadPointBIndex].position = newPoint;
                        }


                        if (!bridgeAndRoadAreAligned)
                        {
                            float angleBetweenRoadAndBridge = Vector2.Angle(roadDirection2D, bridgeDirection2D);
                            Vector2 newSecondPoint2D;
                            if (angleBetweenRoadAndBridge < 90) newSecondPoint2D = roadPointA2D + Vector2.Distance(roadPointA2D, roadPointB2D) * bridgeDirection2D;
                            else newSecondPoint2D = roadPointA2D - Vector2.Distance(roadPointA2D, roadPointB2D) * bridgeDirection2D;
                            Vector3 newSecondPoint = new Vector3(newSecondPoint2D.x, roadPointB.y, newSecondPoint2D.y);
                            _myTarget.Markers[roadPointBIndex].position = newSecondPoint;
                        }

                        if (_myTarget.Markers[roadPointAIndex].GetControlType() != ERMarkerControlType.StraightXZ)
                        {
                            _myTarget.Markers[roadPointAIndex].SetControlType(ERMarkerControlType.StraightXZ);

                        }
                        if (_myTarget.Markers[roadPointBIndex].splineStrength > 0.01f)
                        {
                            _myTarget.Markers[roadPointBIndex].splineStrength = 0.01f;
                        }
                        Vector3 roadExtremity = bridgePoints.points[bridgePointAIndex];
                        roadExtremity.y = bridgePoints.points[bridgePointAIndex].y - _myTarget.RoadNetworkRaise + bridgePoints.ElevationCorrection;
                        _myTarget.Markers[roadPointAIndex].position = roadExtremity;


                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
                else
                {
                    if (_myTarget.AtLeastAnIssue) DisplayRecommendation();
                    else
                    {
                        EditorGUILayout.HelpBox("The road seems ready for this bridge.", MessageType.Info);
                    }

                }
            }
        }

        private void ManageBridgeCrossing(GameObject bridge, int bridgePointAIndex, int bridgePointBIndex, List<ERMarkerExt> markers, int roadPointAIndex, int roadPointBIndex)
        {
            BridgePoints bridgePoints;
            if (bridge.TryGetComponent<BridgePoints>(out bridgePoints))
            {
                Vector2 bridgePointA2D = new Vector2(bridgePoints.points[bridgePointAIndex].x, bridgePoints.points[bridgePointAIndex].z);
                Vector2 bridgePointB2D = new Vector2(bridgePoints.points[bridgePointBIndex].x, bridgePoints.points[bridgePointBIndex].z);
                Vector2 bridgeDirection2D = (bridgePointB2D - bridgePointA2D).normalized;
                Vector3 roadPointA = markers[roadPointAIndex].position;
                Vector3 roadPointB = markers[roadPointBIndex].position;
                Vector2 roadPointA2D = new Vector2(roadPointA.x, roadPointA.z);// first bridge point
                Vector2 roadPointB2D = new Vector2(roadPointB.x, roadPointB.z);//last bridge point
                bool atLeastAnIssueAtPointA = false;
                float previousSegmentRoadLength = -1;
                Vector2 previousSegmentRoadDirection2D = new Vector2();
                bool bridgeAndPreviousRoadSegmentAreAligned = false;

                bool atLeastAnIssueAtPointB = false;
                bool displayForceCheckbox = false;
                float nextSegmentRoadLength = -1;
                Vector2 nextSegmentRoadDirection2D = new Vector2();
                bool bridgeAndNextRoadSegmentAreAligned = false;
                if (roadPointAIndex > 0)
                {
                    Vector3 previousRoadPoint = markers[roadPointAIndex - 1].position;
                    Vector2 previousRoadPoint2D = new Vector2(previousRoadPoint.x, previousRoadPoint.z);
                    previousSegmentRoadDirection2D = (roadPointA2D - previousRoadPoint2D).normalized;
                    previousSegmentRoadLength = Vector2.Distance(roadPointA2D, previousRoadPoint2D);
                    float distanceCrossingRoadPtA_BridgePtA = Vector2.Distance(roadPointA2D, bridgePointA2D);

                    if (previousSegmentRoadDirection2D == bridgeDirection2D || previousSegmentRoadDirection2D == -bridgeDirection2D) bridgeAndPreviousRoadSegmentAreAligned = true;
                    if (distanceCrossingRoadPtA_BridgePtA > 0)
                    {
                        GUILayout.Space(10);
                        GUILayout.Label("Issue: The road crossing segment start does not match the bridge first point", redLabel);
                        atLeastAnIssueAtPointA = true;
                    }

                    if (!PseudoEqual(roadPointA.y, bridgePoints.points[bridgePointAIndex].y - _myTarget.RoadNetworkRaise + bridgePoints.ElevationCorrection + _myTarget.CrossingRoadRaise))

                    {
                        GUILayout.Space(10);
                        GUILayout.Label("Issue: The road first intersection point is not at the right elevation", redLabel);
                        atLeastAnIssueAtPointA = true;

                    }
                    if (previousSegmentRoadLength > 15)
                    {
                        GUILayout.Space(10);
                        GUILayout.Label("Potential Issue: The road first point before the bridge should be at less than 15m from the bridge to avoid a rounded road across the bridge. THIS POTENTIAL ISSUE SHOULD BE SOLVED MANUALLY, BUT YOU CAN FORCE IT'S AUTOMATIC SOLVING BY CHECKING THE CHECKBOX HEREAFTER", blueLabel);
                        atLeastAnIssueAtPointA = true;
                        displayForceCheckbox = true;
                    }
                    if (previousSegmentRoadLength < 1)
                    {
                        GUILayout.Space(10);
                        GUILayout.Label("Issue: The road previous point must be at least at 1m from the intersection point", redLabel);
                        atLeastAnIssueAtPointA = true;
                    }
                    if (!bridgeAndPreviousRoadSegmentAreAligned)
                    {
                        GUILayout.Space(10);
                        GUILayout.Label("Issue: The road first segment must be aligned with the bridge", redLabel);
                        atLeastAnIssueAtPointA = true;
                    }
                }
                if (_myTarget.Markers[roadPointAIndex].GetControlType() != ERMarkerControlType.StraightXZ)
                {
                    GUILayout.Space(10);
                    GUILayout.Label("Issue: The road first intersection point Control Type should be StraightXZ", redLabel);
                    atLeastAnIssueAtPointA = true;
                }
                if (!_myTarget.Markers[roadPointAIndex].bridgeObject)
                {
                    GUILayout.Space(10);
                    GUILayout.Label("Issue: The road first intersection point should have no terrain deformation (bridge)", redLabel);
                    atLeastAnIssueAtPointA = true;
                }



                if (roadPointBIndex < markers.Count - 1)
                {
                    Vector3 nextRoadPoint = markers[roadPointBIndex + 1].position;
                    Vector2 nextRoadPoint2D = new Vector2(nextRoadPoint.x, nextRoadPoint.z);
                    nextSegmentRoadDirection2D = (nextRoadPoint2D - roadPointB2D).normalized;
                    nextSegmentRoadLength = Vector2.Distance(roadPointB2D, nextRoadPoint2D);
                    float distanceCrossingRoadPtB_BridgePtB = Vector2.Distance(roadPointB2D, bridgePointB2D);

                    if (nextSegmentRoadDirection2D == bridgeDirection2D || nextSegmentRoadDirection2D == -bridgeDirection2D) bridgeAndNextRoadSegmentAreAligned = true;
                    if (distanceCrossingRoadPtB_BridgePtB > 0)
                    {
                        GUILayout.Space(10);
                        GUILayout.Label("Issue: The road crossing segment end does not match the bridge last point", redLabel);
                        atLeastAnIssueAtPointB = true;
                    }

                    if (!PseudoEqual(roadPointB.y, bridgePoints.points[bridgePointBIndex].y - _myTarget.RoadNetworkRaise + bridgePoints.ElevationCorrection + _myTarget.CrossingRoadRaise))

                    {
                        GUILayout.Space(10);
                        GUILayout.Label("Issue: The road last intersection point is not at the right elevation", redLabel);
                        atLeastAnIssueAtPointB = true;

                    }
                    if (nextSegmentRoadLength < 1)
                    {
                        GUILayout.Space(10);
                        GUILayout.Label("Issue: The road next point must be at least at 1m from the intersection point", redLabel);
                        atLeastAnIssueAtPointB = true;
                    }
                    if (nextSegmentRoadLength > 15)
                    {
                        GUILayout.Space(10);
                        GUILayout.Label("Potential Issue: The road first point after the bridge should be at less than 15m from the bridge to avoid a rounded road across the bridge. THIS POTENTIAL ISSUE SHOULD BE SOLVED MANUALLY, BUT YOU CAN FORCE IT'S AUTOMATIC SOLVING BY CHECKING THE CHECKBOX HEREAFTER", blueLabel);
                        atLeastAnIssueAtPointB = true;
                        displayForceCheckbox = true;
                    }
                    if (!bridgeAndNextRoadSegmentAreAligned)
                    {
                        GUILayout.Space(10);
                        GUILayout.Label("Issue: The road last segment must be aligned with the bridge", redLabel);
                        atLeastAnIssueAtPointB = true;
                    }
                }
                if (_myTarget.Markers[roadPointBIndex].GetControlType() != ERMarkerControlType.StraightXZ)
                {
                    GUILayout.Space(10);
                    GUILayout.Label("Issue: The road last intersection point Control Type should be StraightXZ", redLabel);
                    atLeastAnIssueAtPointB = true;
                }










                if (atLeastAnIssueAtPointA || atLeastAnIssueAtPointB)
                {
                    if (displayForceCheckbox)
                    {
                        GUILayout.Space(10);
                        GUIContent forceContent = new GUIContent("Force points closer", "Check if you want too far first points to get closer to the bridge.");
                        _myTarget.ForcePointsCloser = EditorGUILayout.Toggle(forceContent, _myTarget.ForcePointsCloser);
                    }
                    _myTarget.AtLeastAnIssue = true;
                    GUIContent solveContent = new GUIContent("Solve Issues", "Solve the above issues.");
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(solveContent, GUILayout.Width(85)))
                    {
                        if (atLeastAnIssueAtPointA)
                        {
                            if (roadPointAIndex > 0 && previousSegmentRoadLength < 1)
                            {
                                Vector2 newSecondPoint2D = roadPointA2D - 1.1f * previousSegmentRoadDirection2D;
                                Vector3 newPoint = new Vector3(newSecondPoint2D.x, roadPointB.y, newSecondPoint2D.y);
                                _myTarget.Markers[roadPointAIndex - 1].position = newPoint;
                            }
                            else if (roadPointAIndex > 0 && previousSegmentRoadLength > 15 && _myTarget.ForcePointsCloser)
                            {
          
                                Vector2 newSecondPoint2D = roadPointA2D - 12f * previousSegmentRoadDirection2D;
                                Vector3 newPoint = new Vector3(newSecondPoint2D.x, roadPointB.y, newSecondPoint2D.y);
                               
                                _myTarget.Markers[roadPointAIndex - 1].position = newPoint;
                            }

                            if (roadPointAIndex > 0 && !bridgeAndPreviousRoadSegmentAreAligned)
                            {
                                Vector3 previousRoadPoint = markers[roadPointAIndex - 1].position;
                                Vector2 previousRoadPoint2D = new Vector2(previousRoadPoint.x, previousRoadPoint.z);
                                float angleBetweenRoadAndBridge = Vector2.Angle(previousSegmentRoadDirection2D, bridgeDirection2D);
                                Vector2 newSecondPoint2D;
                                if (angleBetweenRoadAndBridge < 90) newSecondPoint2D = roadPointA2D - Vector2.Distance(roadPointA2D, previousRoadPoint2D) * bridgeDirection2D;
                                else newSecondPoint2D = roadPointA2D + Vector2.Distance(roadPointA2D, previousRoadPoint2D) * bridgeDirection2D;
                                Vector3 newSecondPoint = new Vector3(newSecondPoint2D.x, previousRoadPoint.y, newSecondPoint2D.y);
                                _myTarget.Markers[roadPointAIndex - 1].position = newSecondPoint;
                            }

                            if (_myTarget.Markers[roadPointAIndex].GetControlType() != ERMarkerControlType.StraightXZ)
                            {
                                _myTarget.Markers[roadPointAIndex].SetControlType(ERMarkerControlType.StraightXZ);

                            }
                            if (!_myTarget.Markers[roadPointAIndex].bridgeObject)
                            {
                                _myTarget.Markers[roadPointAIndex].bridgeObject = true;

                            }
                            Vector3 roadExtremity = bridgePoints.points[bridgePointAIndex];
                            roadExtremity.y = bridgePoints.points[bridgePointAIndex].y - _myTarget.RoadNetworkRaise + bridgePoints.ElevationCorrection + _myTarget.CrossingRoadRaise;
                            _myTarget.Markers[roadPointAIndex].position = roadExtremity;
                        }

                        if (atLeastAnIssueAtPointB)
                        {
                            if (roadPointBIndex < markers.Count - 1 && nextSegmentRoadLength < 1)
                            {
                                Vector2 newSecondPoint2D = roadPointB2D + 1.1f * nextSegmentRoadDirection2D;
                                Vector3 newPoint = new Vector3(newSecondPoint2D.x, roadPointB.y, newSecondPoint2D.y);
                                _myTarget.Markers[roadPointBIndex + 1].position = newPoint;
                            }
                            if (roadPointBIndex < markers.Count - 1 && nextSegmentRoadLength > 15 && _myTarget.ForcePointsCloser)
                            {
                               
                                Vector2 newSecondPoint2D = roadPointB2D + 12 * nextSegmentRoadDirection2D;
                                Vector3 newPoint = new Vector3(newSecondPoint2D.x, roadPointB.y, newSecondPoint2D.y);
                                
                                _myTarget.Markers[roadPointBIndex + 1].position = newPoint;
                            }

                                if (roadPointBIndex < markers.Count - 1 && !bridgeAndNextRoadSegmentAreAligned)
                            {
                                Vector3 nextRoadPoint = markers[roadPointBIndex + 1].position;
                                Vector2 nextRoadPoint2D = new Vector2(nextRoadPoint.x, nextRoadPoint.z);
                                float angleBetweenRoadAndBridge = Vector2.Angle(nextSegmentRoadDirection2D, bridgeDirection2D);
                                Vector2 newSecondPoint2D;
                                if (angleBetweenRoadAndBridge < 90) newSecondPoint2D = roadPointB2D + Vector2.Distance(roadPointB2D, nextRoadPoint2D) * bridgeDirection2D;
                                else newSecondPoint2D = roadPointB2D - Vector2.Distance(roadPointB2D, nextRoadPoint2D) * bridgeDirection2D;
                                Vector3 newSecondPoint = new Vector3(newSecondPoint2D.x, nextRoadPoint.y, newSecondPoint2D.y);
                                _myTarget.Markers[roadPointBIndex + 1].position = newSecondPoint;
                            }

                            if (_myTarget.Markers[roadPointBIndex].GetControlType() != ERMarkerControlType.StraightXZ)
                            {
                                _myTarget.Markers[roadPointBIndex].SetControlType(ERMarkerControlType.StraightXZ);

                            }
                            Vector3 roadExtremity = bridgePoints.points[bridgePointBIndex];
                            roadExtremity.y = bridgePoints.points[bridgePointBIndex].y - _myTarget.RoadNetworkRaise + bridgePoints.ElevationCorrection + _myTarget.CrossingRoadRaise;
                            _myTarget.Markers[roadPointBIndex].position = roadExtremity;
                        }
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
                else
                {
                    if (_myTarget.AtLeastAnIssue) DisplayRecommendation();
                    else
                    {
                        _myTarget.ForcePointsCloser = false;
                        EditorGUILayout.HelpBox("The road seems ready for this bridge.", MessageType.Info);
                    }

                }
            }
        }
#endif



        private bool PseudoEqual(float value1, float value2, float tolerance = 0.01f)
        {
            bool result = false;
            if (value1 < value2 + tolerance && value1 > value2 - tolerance) result = true;

            return result;
        }
        private void DisplayRecommendation()
        {

            EditorGUILayout.HelpBox("It is not finished : \n - Select the Road Network gameobject, \n - If not already in Edit Mode, click on 'Back to Edit Mode' button, \n - Slightly move a road point, avoiding the two first or last points, \n - Build the Terrain.", MessageType.Info);
            if (GUILayout.Button("Done"))
            {
                _myTarget.AtLeastAnIssue = false;
            }
        }
    }
}


