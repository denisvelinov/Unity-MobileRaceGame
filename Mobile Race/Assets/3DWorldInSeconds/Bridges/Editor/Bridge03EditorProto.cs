/*--------------------------------------------------------
  Bridge03EditorProto.cs

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
    [CustomEditor(typeof(Bridge03Proto))]
    public class Bridge03EditorProto : Editor
    {
        private Bridge03Proto _myTarget;
        private int selectedTreeRemoval;
        private Terrain theTerrain;
        private Tool LastTool = Tool.None;
        private QuadMeshProto quadMesh;
        private StonePresets oldPresets;
        private Material oldMainMaterial;
        private Material oldRoadMaterial;
        private ExtractOneLodProto extractOneLod;
        private LineManagerProto lineManager;

        private Texture mainTexture;

        private static readonly string[] _dontIncludeMe = new string[] { "m_Script" };
        void OnEnable()
        {
            _myTarget = (Bridge03Proto)target;
            extractOneLod = new ExtractOneLodProto();
            theTerrain = Terrain.activeTerrain;

            LastTool = Tools.current;
            Tools.current = Tool.None;
            PathProviderProto pathProvider = new PathProviderProto();
            string materialPath = pathProvider.GetMaterialPathPrefix();
            quadMesh = new QuadMeshProto(_myTarget.gameObject);
            StoneBridgeMaterialEditorProto.InitMaterialInspector(_myTarget, false, materialPath);
            oldMainMaterial = _myTarget.MainMaterialProperty;
            oldRoadMaterial = _myTarget.RoadMaterialProperty;

            mainTexture = _myTarget.MainMaterialProperty.mainTexture;

            lineManager = new LineManagerProto(_myTarget, quadMesh, theTerrain);

            Undo.undoRedoPerformed = lineManager.UndoRedoCallBack;
            Selection.selectionChanged += lineManager.IsSelected;

            LogInspector logInspector = _myTarget.GetComponent<LogInspector>();
            if (logInspector == null)
            {
                LogManager.EnableLog = false;
            }
        }
        void OnDisable()
        {
            Tools.current = LastTool;
            Selection.selectionChanged -= lineManager.IsSelected;

        }

        public override void OnInspectorGUI()
        {
            oldPresets = _myTarget.MaterialPresets;
            GUIStyle TitleStyle = new GUIStyle();
            TitleStyle.fontStyle = FontStyle.Bold;
            TitleStyle.fontSize = 13;
            EditorGUILayout.LabelField("TITLE: ", "Humpback Bridge", TitleStyle);
            GUILayout.Space(10);
            GUIStyle richButtonStyle = new GUIStyle(GUI.skin.button);
            richButtonStyle.richText = true;
            if (GUILayout.Button("<b>Help</b>\n<size=10>To know what each field means</size>", richButtonStyle))
            {
                PathProviderProto pathProvider = new PathProviderProto();
                string documentationPath = pathProvider.GetDocumentationPath();
                string helpFileUrl = documentationPath + "/Humpback Bridge Contents.html";
                Application.OpenURL(helpFileUrl);
            }
            GUILayout.Space(10);
            GUIContent tutorialContent = new GUIContent("Tutorial Bridge", "Check this checkbox in the prototyping version if it is a bridge to build for the tutorial.");
            if (_myTarget.IsPrototype())
            {

                _myTarget.Tutorial = GUILayout.Toggle(_myTarget.Tutorial, tutorialContent);
                if (_myTarget.Tutorial)
                {
                    EditorGUILayout.HelpBox("With the 'Tutorial' option checked, you get a fully textured bridge between specific C1 and C2 points in the Tutorial scene.", MessageType.Warning);
                }
            }
           
            if (!_myTarget.IsPrototype() || _myTarget.Tutorial)
            {
                PathProviderProto pathProvider = new PathProviderProto();
                string materialPath = pathProvider.GetMaterialPathPrefix();
                StoneBridgeMaterialEditorProto.MaterialsParameters materialsParameters = new StoneBridgeMaterialEditorProto.MaterialsParameters(oldPresets, oldMainMaterial, oldRoadMaterial, null, null, mainTexture);
                StoneBridgeMaterialEditorProto.ManageMaterialInspector(_myTarget, materialsParameters, false, materialPath);
                oldPresets = materialsParameters.oldPresets;
                oldMainMaterial = materialsParameters.oldMainMaterial;
                oldRoadMaterial = materialsParameters.oldRoadMaterial;

                mainTexture = materialsParameters.mainTexture;

            }
            GUILayout.Space(10);

            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, _dontIncludeMe);

            serializedObject.ApplyModifiedProperties();

            GUIContent automaticPillarsHeightContent = new GUIContent("Automatic Pillars Height", "Calculates automatically pillars heigth with the terrain elevation.");
            _myTarget.AutomaticPillarsHeight = EditorGUILayout.Toggle(automaticPillarsHeightContent, _myTarget.AutomaticPillarsHeight);
            if (!_myTarget.AutomaticPillarsHeight)
            {
                GUIContent manualPillarsHeightContent = new GUIContent("Manual Pillars height", "Specify the pillars height.");
                _myTarget.PillarsHeight = EditorGUILayout.FloatField(manualPillarsHeightContent, _myTarget.PillarsHeight);
            }
            EditorGUILayout.Space();

            GUILayout.Label("Terrain Settings", TitleStyle);
            EditorGUILayout.Space();
            Texture modifyTerrainImage = Resources.Load<Texture>("ModifyTerrain");
            GUIStyle textureStyle = new GUIStyle();
            textureStyle.fixedHeight = 40;
            textureStyle.fixedWidth = 40;
            RectOffset rectOffset = new RectOffset(0, 5, 0, 10);
            textureStyle.padding = rectOffset;
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(modifyTerrainImage, textureStyle);
            GUIContent modifyTerrainContent = new GUIContent("MODIFY TERRAIN", "Modifies terrain to get a ready to use bridge.");
            _myTarget.ModifyTerrain = EditorGUILayout.Toggle(modifyTerrainContent, _myTarget.ModifyTerrain);

            EditorGUILayout.EndHorizontal();


            if (_myTarget.ModifyTerrain)
            {
                CommonBridgeEditorProto.ManageTerrainEditor(_myTarget);

            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUILayout.Label("Generation", TitleStyle);
            CommonBridgeEditorProto.ManagePrefabGenerationEditor(_myTarget);

            EditorGUILayout.Space();
            CommonBridgeEditorProto.ManageBridgeGeneration(_myTarget, quadMesh, theTerrain, lineManager, extractOneLod);


        }

        void OnSceneGUI()
        {
            lineManager.SceneManager();

        }
    }
}