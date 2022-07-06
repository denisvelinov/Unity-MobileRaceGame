/*--------------------------------------------------------
  WoodenBridgeMaterialEditorProto.cs

  Created by Alain Debelley on 22021-12-22.
  Copyright (c) 2021 ALAIN DEBELLEY. All rights reserved.
  http://www.3dworldinseconds.com/
--------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WorldInSeconds3DProto
{
    public class WoodenBridgeMaterialEditorProto
    {
        public class MaterialsParameters
        {
            public WoodenPresets oldPresets;
            public Material oldMainMaterial;
            public Material oldRopeMaterial;
            public Material oldFastenerMaterial;
            public Texture mainTexture;

            public MaterialsParameters(WoodenPresets oldPresets, Material oldMainMaterial, Material oldRopeMaterial, Material oldFastenerMaterial, Texture mainTexture)
            {
                this.oldPresets = oldPresets;
                this.oldMainMaterial = oldMainMaterial;
                this.oldRopeMaterial = oldRopeMaterial;
                this.oldFastenerMaterial = oldFastenerMaterial;
                this.mainTexture = mainTexture;
            }
        }
        /// <summary>
        /// Load default materials
        /// </summary>
        /// <param name="target">The bridge script</param>
        public static void InitMaterialInspector(Object target, string materialPathPrefix)
        {
            IWoodenBridgeProto iWoodenBridgeMaterialInterface = (IWoodenBridgeProto)target;
            if (iWoodenBridgeMaterialInterface.MainMaterialProperty == null)
            {
                iWoodenBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Wood_grainy.mat");
            }
            if (iWoodenBridgeMaterialInterface.RopeMaterialProperty == null)
            {
                iWoodenBridgeMaterialInterface.RopeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Rope.mat");

            }
            if (iWoodenBridgeMaterialInterface.FastenerMaterialProperty == null)
            {
                iWoodenBridgeMaterialInterface.FastenerMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Bronze.mat");

            }

        }
        /// <summary>
        /// Manages the materials display in the inspector
        /// </summary>
        /// <param name="target">Th bridge script</param>
        /// <param name="materialsParameters">Previous materials</param>
        public static void ManageMaterialInspector(Object target, MaterialsParameters materialsParameters, string materialPathPrefix)
        {


            IWoodenBridgeProto iWoodenBridgeMaterialInterface = (IWoodenBridgeProto)target;
            iWoodenBridgeMaterialInterface.MaterialPresetsProperty = (WoodenPresets)EditorGUILayout.EnumPopup("Materials Preset:", iWoodenBridgeMaterialInterface.MaterialPresetsProperty);

            if (materialsParameters.oldPresets != iWoodenBridgeMaterialInterface.MaterialPresetsProperty)
            {
                //Debug.Log("Materials Preset " + _myTarget.MaterialPresetsProperty);
                switch (iWoodenBridgeMaterialInterface.MaterialPresetsProperty)
                {
                    case WoodenPresets.GrainyWood:
                        iWoodenBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Wood_grainy.mat");
                        iWoodenBridgeMaterialInterface.RopeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Rope.mat");
                        iWoodenBridgeMaterialInterface.FastenerMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Bronze.mat");

                        materialsParameters.oldMainMaterial = iWoodenBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRopeMaterial = iWoodenBridgeMaterialInterface.RopeMaterialProperty;
                        materialsParameters.oldFastenerMaterial = iWoodenBridgeMaterialInterface.FastenerMaterialProperty;
                        break;
                    case WoodenPresets.WeatheredWood:
                        iWoodenBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Wood_weathered.mat");
                        iWoodenBridgeMaterialInterface.RopeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Rope.mat");
                        iWoodenBridgeMaterialInterface.FastenerMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Bronze.mat");

                        materialsParameters.oldMainMaterial = iWoodenBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRopeMaterial = iWoodenBridgeMaterialInterface.RopeMaterialProperty;
                        materialsParameters.oldFastenerMaterial = iWoodenBridgeMaterialInterface.FastenerMaterialProperty;
                        break;

                    case WoodenPresets.WoodenPannel:
                        iWoodenBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Wood_pannel.mat");
                        iWoodenBridgeMaterialInterface.RopeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Rope.mat");
                        iWoodenBridgeMaterialInterface.FastenerMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Bronze.mat");

                        materialsParameters.oldMainMaterial = iWoodenBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRopeMaterial = iWoodenBridgeMaterialInterface.RopeMaterialProperty;
                        materialsParameters.oldFastenerMaterial = iWoodenBridgeMaterialInterface.FastenerMaterialProperty;

                        break;


                    case WoodenPresets.BrownWood:
                        iWoodenBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Wood_brown.mat");
                        iWoodenBridgeMaterialInterface.RopeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Rope.mat");
                        iWoodenBridgeMaterialInterface.FastenerMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Bronze.mat");

                        materialsParameters.oldMainMaterial = iWoodenBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRopeMaterial = iWoodenBridgeMaterialInterface.RopeMaterialProperty;
                        materialsParameters.oldFastenerMaterial = iWoodenBridgeMaterialInterface.FastenerMaterialProperty;

                        break;

                    case WoodenPresets.MagicWood:
                        iWoodenBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Wood_magic.mat");
                        iWoodenBridgeMaterialInterface.RopeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Rope.mat");
                        iWoodenBridgeMaterialInterface.FastenerMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Bronze.mat");

                        materialsParameters.oldMainMaterial = iWoodenBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRopeMaterial = iWoodenBridgeMaterialInterface.RopeMaterialProperty;
                        materialsParameters.oldFastenerMaterial = iWoodenBridgeMaterialInterface.FastenerMaterialProperty;

                        break;
                    case WoodenPresets.PaintedWood:
                        iWoodenBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Wood_painted.mat");
                        iWoodenBridgeMaterialInterface.RopeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Rope.mat");
                        iWoodenBridgeMaterialInterface.FastenerMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Bronze.mat");

                        materialsParameters.oldMainMaterial = iWoodenBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRopeMaterial = iWoodenBridgeMaterialInterface.RopeMaterialProperty;
                        materialsParameters.oldFastenerMaterial = iWoodenBridgeMaterialInterface.FastenerMaterialProperty;

                        break;
                    case WoodenPresets.RottenWood:
                        iWoodenBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Wood_rotten.mat");
                        iWoodenBridgeMaterialInterface.RopeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Rope.mat");
                        iWoodenBridgeMaterialInterface.FastenerMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Bronze.mat");

                        materialsParameters.oldMainMaterial = iWoodenBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRopeMaterial = iWoodenBridgeMaterialInterface.RopeMaterialProperty;
                        materialsParameters.oldFastenerMaterial = iWoodenBridgeMaterialInterface.FastenerMaterialProperty;

                        break;
                    case WoodenPresets.RoughWood:
                        iWoodenBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Wood_rough.mat");
                        iWoodenBridgeMaterialInterface.RopeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Rope.mat");
                        iWoodenBridgeMaterialInterface.FastenerMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Bronze.mat");

                        materialsParameters.oldMainMaterial = iWoodenBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRopeMaterial = iWoodenBridgeMaterialInterface.RopeMaterialProperty;
                        materialsParameters.oldFastenerMaterial = iWoodenBridgeMaterialInterface.FastenerMaterialProperty;

                        break;
                    case WoodenPresets.CartonWood1:
                        iWoodenBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Wood_cartoon01.mat");
                        iWoodenBridgeMaterialInterface.RopeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Rope.mat");
                        iWoodenBridgeMaterialInterface.FastenerMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Bronze.mat");

                        materialsParameters.oldMainMaterial = iWoodenBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRopeMaterial = iWoodenBridgeMaterialInterface.RopeMaterialProperty;
                        materialsParameters.oldFastenerMaterial = iWoodenBridgeMaterialInterface.FastenerMaterialProperty;

                        break;
                    case WoodenPresets.CartonWood2:
                        iWoodenBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Wood_cartoon02.mat");
                        iWoodenBridgeMaterialInterface.RopeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Rope.mat");
                        iWoodenBridgeMaterialInterface.FastenerMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Bronze.mat");

                        materialsParameters.oldMainMaterial = iWoodenBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRopeMaterial = iWoodenBridgeMaterialInterface.RopeMaterialProperty;
                        materialsParameters.oldFastenerMaterial = iWoodenBridgeMaterialInterface.FastenerMaterialProperty;

                        break;
                    case WoodenPresets.CartonWood3:
                        iWoodenBridgeMaterialInterface.MainMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Wood_cartoon03.mat");
                        iWoodenBridgeMaterialInterface.RopeMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Rope.mat");
                        iWoodenBridgeMaterialInterface.FastenerMaterialProperty = AssetDatabase.LoadAssetAtPath<Material>(materialPathPrefix + "/Bronze.mat");

                        materialsParameters.oldMainMaterial = iWoodenBridgeMaterialInterface.MainMaterialProperty;
                        materialsParameters.oldRopeMaterial = iWoodenBridgeMaterialInterface.RopeMaterialProperty;
                        materialsParameters.oldFastenerMaterial = iWoodenBridgeMaterialInterface.FastenerMaterialProperty;

                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (materialsParameters.oldMainMaterial != iWoodenBridgeMaterialInterface.MainMaterialProperty || materialsParameters.oldRopeMaterial != iWoodenBridgeMaterialInterface.RopeMaterialProperty)
                {
                    iWoodenBridgeMaterialInterface.MaterialPresetsProperty = WoodenPresets.Custom;
                    materialsParameters.oldPresets = iWoodenBridgeMaterialInterface.MaterialPresetsProperty;
                }
            }
            if (iWoodenBridgeMaterialInterface.MainMaterialProperty != null)
            {
                GUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Main Material Preview : ");
                materialsParameters.mainTexture = iWoodenBridgeMaterialInterface.MainMaterialProperty.mainTexture;
                GUIStyle textureStyle = new GUIStyle();
                textureStyle.fixedHeight = 120;
                textureStyle.fixedWidth = 120;
                GUILayout.Label(materialsParameters.mainTexture, textureStyle);
                EditorGUILayout.EndHorizontal();
            }

            iWoodenBridgeMaterialInterface.MainMaterialProperty = (Material)EditorGUILayout.ObjectField("Main Material", iWoodenBridgeMaterialInterface.MainMaterialProperty, typeof(Material), false);
            iWoodenBridgeMaterialInterface.RopeMaterialProperty = (Material)EditorGUILayout.ObjectField("Rope Material", iWoodenBridgeMaterialInterface.RopeMaterialProperty, typeof(Material), false);
            iWoodenBridgeMaterialInterface.FastenerMaterialProperty = (Material)EditorGUILayout.ObjectField("Rope Fastener Material", iWoodenBridgeMaterialInterface.FastenerMaterialProperty, typeof(Material), false);




        }
    }
}