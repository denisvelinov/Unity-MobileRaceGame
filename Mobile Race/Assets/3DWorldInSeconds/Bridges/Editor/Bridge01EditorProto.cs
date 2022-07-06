/*--------------------------------------------------------
  Bridge01EditorProto.cs

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

    [CustomEditor(typeof(Bridge01Proto))]
    public class Bridge01EditorProto : Editor
    {
#if UNITY_EDITOR
        private Bridge01Proto _myTarget;

        private Terrain theTerrain;
        private Tool LastTool = Tool.None;
        private QuadMeshProto quadMesh;
        private StonePresets oldPresets;
        private Material oldMainMaterial;
        private Material oldRoadMaterial;
        private Material oldSideWalkMaterial;
        private Material oldEdgeMaterial;
        private ExtractOneLodProto extractOneLod;
        public LineManagerProto lineManager;
        private Texture mainTexture;



        private static readonly string[] _dontIncludeMe = new string[] { "m_Script" };

        void OnEnable()
        {
            _myTarget = (Bridge01Proto)target;
            extractOneLod = new ExtractOneLodProto();

            theTerrain = Terrain.activeTerrain;


            LastTool = Tools.current;
            Tools.current = Tool.None;
            PathProviderProto pathProvider = new PathProviderProto();
            string materialPath = pathProvider.GetMaterialPathPrefix();
            quadMesh = new QuadMeshProto(_myTarget.gameObject);
            StoneBridgeMaterialEditorProto.InitMaterialInspector(_myTarget, true, materialPath);


            oldMainMaterial = _myTarget.MainMaterialProperty;
            oldRoadMaterial = _myTarget.RoadMaterialProperty;
            oldSideWalkMaterial = _myTarget.SideWalkMaterialProperty;
            oldEdgeMaterial = _myTarget.EdgeMaterialProperty;
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
            //Selection.selectionChanged -= isSelected;
            Selection.selectionChanged -= lineManager.IsSelected;

        }

        public override void OnInspectorGUI()
        {
            oldPresets = _myTarget.MaterialPresets;
            GUIStyle TitleStyle = new GUIStyle();
            TitleStyle.fontStyle = FontStyle.Bold;
            TitleStyle.fontSize = 13;
            EditorGUILayout.LabelField("TITLE: ", "Bridge with columns", TitleStyle);
            GUILayout.Space(10);
            GUIStyle richButtonStyle = new GUIStyle(GUI.skin.button);
            richButtonStyle.richText = true;
            if (GUILayout.Button("<b>Help</b>\n<size=10>To know what each field means</size>", richButtonStyle))
            {
                PathProviderProto pathProvider = new PathProviderProto();
                string documentationPath = pathProvider.GetDocumentationPath();
                string helpFileUrl = documentationPath + "/Bridge With Columns Contents.html";
                Application.OpenURL(helpFileUrl);
            }
            GUILayout.Space(10);
            GUIContent tutorialContent = new GUIContent("Tutorial Bridge", "Check this checkbox in the prototyping version if it is a bridge to build for the tutorial.");
            if (_myTarget.IsPrototype())
            {
               
                _myTarget.Tutorial = GUILayout.Toggle(_myTarget.Tutorial, tutorialContent);
                if (_myTarget.Tutorial)
                {
                    EditorGUILayout.HelpBox("With the 'Tutorial' option checked, you get a fully textured bridge between specific A1 and A2 points in the Tutorial scene.", MessageType.Warning);
                }
            }
           
            if (!_myTarget.IsPrototype() || _myTarget.Tutorial)
            {
                PathProviderProto pathProvider = new PathProviderProto();
                string materialPath = pathProvider.GetMaterialPathPrefix();
                StoneBridgeMaterialEditorProto.MaterialsParameters materialsParameters = new StoneBridgeMaterialEditorProto.MaterialsParameters(oldPresets, oldMainMaterial, oldRoadMaterial, oldSideWalkMaterial, oldEdgeMaterial, mainTexture);
                StoneBridgeMaterialEditorProto.ManageMaterialInspector(_myTarget, materialsParameters,true, materialPath);
                oldPresets = materialsParameters.oldPresets;
                oldMainMaterial = materialsParameters.oldMainMaterial;
                oldRoadMaterial = materialsParameters.oldRoadMaterial;
                oldSideWalkMaterial = materialsParameters.oldSideWalkMaterial;
                oldEdgeMaterial = materialsParameters.oldEdgeMaterial;
                mainTexture = materialsParameters.mainTexture;

            }
            GUIContent bridgeWidthContent = new GUIContent("Bridge Width", "The bridge width");
            _myTarget.BridgeWidth = EditorGUILayout.FloatField(bridgeWidthContent, _myTarget.BridgeWidth);
            
            GUIContent archWidthContent = new GUIContent("Target Arch Width", "The target bridge arch width. The actuel arch width will be as close as possible, depending on the bridge length");
            _myTarget.TargetArchWidth = EditorGUILayout.FloatField(archWidthContent, _myTarget.TargetArchWidth);
            var previousGUIState = GUI.enabled;
            if (!_myTarget.Tutorial)
            {
                // Disabling edit for property
                GUI.enabled = false;
            }
            GUIContent sideDecorationContent = new GUIContent("Side Decoration", "Are there statues or lamps on columns ?");
            string[] decorationLabels = System.Enum.GetNames(typeof(Bridge01Proto.Decoration));
            for (int i = 0; i < decorationLabels.Length; i++)
            {
                decorationLabels[i] = AddSpacesToSentence(decorationLabels[i], false);
            }
            int index = EditorGUILayout.Popup(sideDecorationContent, (int)_myTarget.SideDecoration, decorationLabels);
            _myTarget.SideDecoration = (Bridge01Proto.Decoration)index;
            if (!_myTarget.Tutorial)
            {
                // Disabling edit for property
                GUI.enabled = previousGUIState;
                GUIStyle remarkStyle = new GUIStyle();
                remarkStyle.fontStyle = FontStyle.Italic;
                remarkStyle.fontSize = 10;
                GUILayout.Label("Side decorationis only active for tutorial bridges.", remarkStyle);
            }



            GUILayout.Space(10);
            // Show default inspector property editor
            serializedObject.Update();


            DrawPropertiesExcluding(serializedObject, _dontIncludeMe);

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
           

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
        string AddSpacesToSentence(string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            System.Text.StringBuilder newText = new System.Text.StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }


#endif

    }
}