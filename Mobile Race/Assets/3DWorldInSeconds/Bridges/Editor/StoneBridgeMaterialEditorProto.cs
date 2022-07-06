/*--------------------------------------------------------
  StoneBridgeMaterialEditorProto.cs

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
    public class StoneBridgeMaterialEditorProto
    {
 
        public class MaterialsParameters
        {
            public StonePresets oldPresets;
            public Material oldMainMaterial;
            public Material oldRoadMaterial;
            public Material oldSideWalkMaterial;
            public Material oldEdgeMaterial;
            public Texture mainTexture;
            
            public MaterialsParameters(StonePresets oldPresets, Material oldMainMaterial, Material oldRoadMaterial, Material oldSideWalkMaterial, Material oldEdgeMaterial, Texture mainTexture)
            {
                this.oldPresets = oldPresets;
                this.oldMainMaterial = oldMainMaterial;
                this.oldRoadMaterial = oldRoadMaterial;
                this.oldSideWalkMaterial = oldSideWalkMaterial;
                this.oldEdgeMaterial = oldEdgeMaterial;
                this.mainTexture = mainTexture;
                
            }
        }
        /// <summary>
        /// Load default materials
        /// </summary>
        /// <param name="target">The bridge script</param>
        /// <param name="sidewalks">Are sidewalks possible for the target bridge ?/param>
        public static void InitMaterialInspector(Object target, bool sidewalks, string materialPathPrefix)
        {
            IStoneBridgeProto iStoneBridgeMaterialInterface = (IStoneBridgeProto)target;
            if (iStoneBridgeMaterialInterface.MainMaterialProperty == null)
            {
                iStoneBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Brick_026.mat");
            }
            if (iStoneBridgeMaterialInterface.RoadMaterialProperty == null)
            {
                iStoneBridgeMaterialInterface.RoadMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix +"/roadPavingStone.mat");

            }
            if (sidewalks)
            {
                if (iStoneBridgeMaterialInterface.SideWalkMaterialProperty == null)
                {
                    iStoneBridgeMaterialInterface.SideWalkMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Sidewalk.mat");

                }
                if (iStoneBridgeMaterialInterface.EdgeMaterialProperty == null)
                {
                    iStoneBridgeMaterialInterface.EdgeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkEdge.mat");

                }
            }

        }
        /// <summary>
        /// Manages the materials display in the inspector
        /// </summary>
        /// <param name="target">Th bridge script</param>
        /// <param name="materialsParameters">Previous materials</param>
        /// /// <param name="sidewalks">Are sidewalks possible for the target bridge ?/param>
        public static void ManageMaterialInspector(Object target, MaterialsParameters materialsParameters, bool sidewalks, string materialPathPrefix)
        {

            IStoneBridgeProto iStoneBridgeMaterialInterface = (IStoneBridgeProto)target;
            iStoneBridgeMaterialInterface.MaterialPresetsProperty = (StonePresets)EditorGUILayout.EnumPopup("Materials Preset:", iStoneBridgeMaterialInterface.MaterialPresetsProperty);

            if (materialsParameters.oldPresets != iStoneBridgeMaterialInterface.MaterialPresetsProperty)
            {
                switch (iStoneBridgeMaterialInterface.MaterialPresetsProperty)
                {
                    case StonePresets.Brick:
                        iStoneBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Brick_026.mat");
                        iStoneBridgeMaterialInterface.RoadMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/roadPavingStone.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.SideWalkMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Sidewalk.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.EdgeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkEdge.mat");
                        materialsParameters.oldMainMaterial = iStoneBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRoadMaterial = iStoneBridgeMaterialInterface.RoadMaterialProperty;
                        if (sidewalks) materialsParameters.oldSideWalkMaterial = iStoneBridgeMaterialInterface.SideWalkMaterialProperty;
                        if (sidewalks) materialsParameters.oldEdgeMaterial = iStoneBridgeMaterialInterface.EdgeMaterialProperty;
                        break;
                    case StonePresets.OldBrick:
                        iStoneBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Brick_037.mat");
                        iStoneBridgeMaterialInterface.RoadMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/roadPavingStone.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.SideWalkMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Sidewalk.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.EdgeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkEdge.mat");
                        materialsParameters.oldMainMaterial = iStoneBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRoadMaterial = iStoneBridgeMaterialInterface.RoadMaterialProperty;
                        if (sidewalks) materialsParameters.oldSideWalkMaterial = iStoneBridgeMaterialInterface.SideWalkMaterialProperty;
                        if (sidewalks) materialsParameters.oldEdgeMaterial = iStoneBridgeMaterialInterface.EdgeMaterialProperty;
                        break;
                    case StonePresets.WhiteStone:
                        iStoneBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Brick_008.mat");
                        iStoneBridgeMaterialInterface.RoadMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/roadPavingStone.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.SideWalkMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Sidewalk.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.EdgeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkEdge.mat");
                        materialsParameters.oldMainMaterial = iStoneBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRoadMaterial = iStoneBridgeMaterialInterface.RoadMaterialProperty;
                        if (sidewalks) materialsParameters.oldSideWalkMaterial = iStoneBridgeMaterialInterface.SideWalkMaterialProperty;
                        if (sidewalks) materialsParameters.oldEdgeMaterial = iStoneBridgeMaterialInterface.EdgeMaterialProperty;
                        break;
                    case StonePresets.DarkStone:
                        iStoneBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Tiles_087.mat");
                        iStoneBridgeMaterialInterface.RoadMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/roadPavingStone.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.SideWalkMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Sidewalk.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.EdgeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkEdge.mat");
                        materialsParameters.oldMainMaterial = iStoneBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRoadMaterial = iStoneBridgeMaterialInterface.RoadMaterialProperty;
                        if (sidewalks) materialsParameters.oldSideWalkMaterial = iStoneBridgeMaterialInterface.SideWalkMaterialProperty;
                        if (sidewalks) materialsParameters.oldEdgeMaterial = iStoneBridgeMaterialInterface.EdgeMaterialProperty;
                        break;
                    case StonePresets.Grunge:
                        iStoneBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Grunge_Wall.mat");
                        iStoneBridgeMaterialInterface.RoadMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/roadFlagStone.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.SideWalkMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Sidewalk_Old.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.EdgeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkEdgeDammaged.mat");
                        materialsParameters.oldMainMaterial = iStoneBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRoadMaterial = iStoneBridgeMaterialInterface.RoadMaterialProperty;
                        if (sidewalks) materialsParameters.oldSideWalkMaterial = iStoneBridgeMaterialInterface.SideWalkMaterialProperty;
                        if (sidewalks) materialsParameters.oldEdgeMaterial = iStoneBridgeMaterialInterface.EdgeMaterialProperty;
                        break;
                    case StonePresets.WornOutStoneWithVariations:
                        iStoneBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/WornOutStone.mat");
                        iStoneBridgeMaterialInterface.RoadMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/roadFlagStone.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.SideWalkMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Sidewalk_Old.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.EdgeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkEdgeDammaged.mat");
                        materialsParameters.oldMainMaterial = iStoneBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRoadMaterial = iStoneBridgeMaterialInterface.RoadMaterialProperty;
                        if (sidewalks) materialsParameters.oldSideWalkMaterial = iStoneBridgeMaterialInterface.SideWalkMaterialProperty;
                        if (sidewalks) materialsParameters.oldEdgeMaterial = iStoneBridgeMaterialInterface.EdgeMaterialProperty;
                        break;
                    case StonePresets.WornOutStone:
                        iStoneBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/WornOutStone2.mat");
                        iStoneBridgeMaterialInterface.RoadMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/roadFlagStone.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.SideWalkMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Sidewalk_Old.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.EdgeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkEdgeDammaged.mat");
                        materialsParameters.oldMainMaterial = iStoneBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRoadMaterial = iStoneBridgeMaterialInterface.RoadMaterialProperty;
                        if (sidewalks) materialsParameters.oldSideWalkMaterial = iStoneBridgeMaterialInterface.SideWalkMaterialProperty;
                        if (sidewalks) materialsParameters.oldEdgeMaterial = iStoneBridgeMaterialInterface.EdgeMaterialProperty;
                        break;
                    case StonePresets.MossyStone:
                        iStoneBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Mossy_stone.mat");
                        iStoneBridgeMaterialInterface.RoadMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/roadFlagStone.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.SideWalkMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Sidewalk_Old.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.EdgeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkEdgeDammaged.mat");
                        materialsParameters.oldMainMaterial = iStoneBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRoadMaterial = iStoneBridgeMaterialInterface.RoadMaterialProperty;
                        if (sidewalks) materialsParameters.oldSideWalkMaterial = iStoneBridgeMaterialInterface.SideWalkMaterialProperty;
                        if (sidewalks) materialsParameters.oldEdgeMaterial = iStoneBridgeMaterialInterface.EdgeMaterialProperty;
                        break;
                    case StonePresets.WarmToon:
                        iStoneBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/BrickWarmToon.mat");
                        iStoneBridgeMaterialInterface.RoadMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/roadPavingStoneToon.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.SideWalkMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkToon.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.EdgeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkEdgeToon.mat");
                        materialsParameters.oldMainMaterial = iStoneBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRoadMaterial = iStoneBridgeMaterialInterface.RoadMaterialProperty;
                        if (sidewalks) materialsParameters.oldSideWalkMaterial = iStoneBridgeMaterialInterface.SideWalkMaterialProperty;
                        if (sidewalks) materialsParameters.oldEdgeMaterial = iStoneBridgeMaterialInterface.EdgeMaterialProperty;

                        break;
                    case StonePresets.ColdToon:
                        iStoneBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/BrickColdToon.mat");
                        iStoneBridgeMaterialInterface.RoadMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/roadPavingStoneToon.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.SideWalkMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkToon.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.EdgeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkEdgeToon.mat");
                        materialsParameters.oldMainMaterial = iStoneBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRoadMaterial = iStoneBridgeMaterialInterface.RoadMaterialProperty;
                        if (sidewalks) materialsParameters.oldSideWalkMaterial = iStoneBridgeMaterialInterface.SideWalkMaterialProperty;
                        if (sidewalks) materialsParameters.oldEdgeMaterial = iStoneBridgeMaterialInterface.EdgeMaterialProperty;

                        break;
                    case StonePresets.RedToon:
                        iStoneBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/BrickRedToon.mat");
                        iStoneBridgeMaterialInterface.RoadMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/roadPavingStoneToon.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.SideWalkMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkToon.mat");
                        if (sidewalks) iStoneBridgeMaterialInterface.EdgeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/SidewalkEdgeToon.mat");
                        materialsParameters.oldMainMaterial = iStoneBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRoadMaterial = iStoneBridgeMaterialInterface.RoadMaterialProperty;
                        if (sidewalks) materialsParameters.oldSideWalkMaterial = iStoneBridgeMaterialInterface.SideWalkMaterialProperty;
                        if (sidewalks) materialsParameters.oldEdgeMaterial = iStoneBridgeMaterialInterface.EdgeMaterialProperty;
                        break;

                    default:
                        break;
                }
            }
            else
            {
                if (materialsParameters.oldMainMaterial != iStoneBridgeMaterialInterface.MainMaterialProperty || materialsParameters.oldRoadMaterial != iStoneBridgeMaterialInterface.RoadMaterialProperty || materialsParameters.oldSideWalkMaterial != iStoneBridgeMaterialInterface.SideWalkMaterialProperty || materialsParameters.oldEdgeMaterial != iStoneBridgeMaterialInterface.EdgeMaterialProperty)
                {
                    iStoneBridgeMaterialInterface.MaterialPresetsProperty = StonePresets.Custom;
                    materialsParameters.oldPresets = iStoneBridgeMaterialInterface.MaterialPresetsProperty;
                }
            }
            if (iStoneBridgeMaterialInterface.MainMaterialProperty != null)
            {
                GUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Main Material Preview : ");
                materialsParameters.mainTexture = iStoneBridgeMaterialInterface.MainMaterialProperty.mainTexture;
                GUIStyle textureStyle = new GUIStyle();
                textureStyle.fixedHeight = 120;
                textureStyle.fixedWidth = 120;
                GUILayout.Label(materialsParameters.mainTexture, textureStyle);
                EditorGUILayout.EndHorizontal();
            }

            iStoneBridgeMaterialInterface.MainMaterialProperty = (Material)EditorGUILayout.ObjectField("Main Material", iStoneBridgeMaterialInterface.MainMaterialProperty, typeof(Material), false);
            iStoneBridgeMaterialInterface.RoadMaterialProperty = (Material)EditorGUILayout.ObjectField("Road Material", iStoneBridgeMaterialInterface.RoadMaterialProperty, typeof(Material), false);
            if (sidewalks) iStoneBridgeMaterialInterface.SideWalkMaterialProperty = (Material)EditorGUILayout.ObjectField("Sidewalk Material", iStoneBridgeMaterialInterface.SideWalkMaterialProperty, typeof(Material), false);
            if (sidewalks) iStoneBridgeMaterialInterface.EdgeMaterialProperty = (Material)EditorGUILayout.ObjectField("Sidewalk edge Material", iStoneBridgeMaterialInterface.EdgeMaterialProperty, typeof(Material), false);



        }
    }
}