/*--------------------------------------------------------
  BridgesStartupWindowProto.cs

  Created by Alain Debelley on 2021-12-22.
  Copyright (c) 2021 ALAIN DEBELLEY. All rights reserved.
  http://www.3dworldinseconds.com/
--------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;

namespace WorldInSeconds3DProto
{
    public class BridgesStartupWindowProto : EditorWindow
    {
        static string pathPrefix = "Assets/3DWorldInSeconds/Bridges";
        private Texture2D headerPicture;
        private Vector2 scrollPosition = Vector2.zero;
        private GUIStyle buttonStyle;
        private GUIStyle centeredLabelStyle;
        private GUIStyle warningLabelStyle;
        private GUIStyle leftLabelStyle;
        private GUIStyle linkStyle;
        private bool prototype;
        private PathProviderProto pathProvider;
        private int scrollHeight;

        [MenuItem("Window/3D World In Seconds/About Bridges (Prototype)", false, 0)]
        public static void MenuStartupInit()
        {
            BridgesStartupWindowProto.Open();
        }
        [MenuItem("Help/3D World In Seconds/About Bridges (Prototype)", false, 0)]
        public static void MenuStartupHelpInit()
        {
            BridgesStartupWindowProto.Open();
        }
        

        public static void Open()
        {



            BridgesStartupWindowProto window = ScriptableObject.CreateInstance(typeof(BridgesStartupWindowProto)) as BridgesStartupWindowProto;
            window = EditorWindow.GetWindow<BridgesStartupWindowProto>(true, "About 3D World In Seconds", true);
            Vector2 size = new Vector2(530, 670);
            window.minSize = size;
            window.maxSize = size;
            window.ShowUtility();


        }

        void OnEnable()
        {
            string headerImage = pathPrefix + "/ui/Bridge-with-columns.png";
            headerPicture = AssetDatabase.LoadAssetAtPath<Texture2D>(headerImage);
            // is it the prototyping version ?
            string worldInSecondsFolder = "Assets/3DWorldInSeconds";
            string premiumFolder = worldInSecondsFolder + "/PremiumBridges";
            prototype = !Directory.Exists(premiumFolder);
            pathProvider = new PathProviderProto();
        }

        private void OnGUI()
        {
            scrollHeight = 180;
            if (headerPicture == null)
            {
                string headerImage = pathPrefix + "/ui/Bridge-with-columns.png";
                headerPicture = AssetDatabase.LoadAssetAtPath<Texture2D>(headerImage);
            }
            buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.richText = true;
            centeredLabelStyle = new GUIStyle(GUI.skin.label);
            centeredLabelStyle.fixedWidth = 520;
            centeredLabelStyle.wordWrap = true;
            centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
            centeredLabelStyle.richText = true;
            centeredLabelStyle.fontSize = 14;
            leftLabelStyle = new GUIStyle(GUI.skin.label);
            leftLabelStyle.fixedWidth = 500;
            leftLabelStyle.wordWrap = true;
            leftLabelStyle.alignment = TextAnchor.MiddleLeft;
            leftLabelStyle.richText = true;
            warningLabelStyle = new GUIStyle(GUI.skin.label);
            warningLabelStyle.fixedWidth = 500;
            warningLabelStyle.wordWrap = true;
            warningLabelStyle.fontSize = 18;
            warningLabelStyle.richText = true;
            linkStyle = new GUIStyle(GUI.skin.label);
            linkStyle.alignment = TextAnchor.MiddleLeft;
            linkStyle.normal.textColor = Color.blue;
            linkStyle.hover.textColor = new Color(0.3f,0.3f,1);


            GUILayout.BeginVertical();
            {
                
                Rect headerRectangle = new Rect(0, 0, 530, 235);
                if (headerPicture != null) EditorGUI.DrawTextureTransparent(headerRectangle, headerPicture, ScaleMode.StretchToFill);
                GUILayout.Space(240);
                GUILayout.Label(" <color=blue><b>Display this window when needed with the Unity menu :</b></color>", centeredLabelStyle);
                GUILayout.Label("<i>Help>3D World In Seconds>About Bridges</i>", centeredLabelStyle);

                // URP Warning
                if (UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset != null && UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset.GetType().Name == "UniversalRenderPipelineAsset")
                {
                    // URP Warning

                    string pathPrefix = PathProviderProto.GetLogPathPrefix();
                    string path = pathPrefix + "/URPimported.txt";
                    bool fileExists = File.Exists(path);
                    bool displayUrpMsg = false;
                    if (!fileExists)
                    {
                        displayUrpMsg = true;
                    }
                    else
                    {
                        StreamReader reader = new StreamReader(path);
                        string urpLoaded = reader.ReadLine();
                        reader.Close();
                        if (urpLoaded != "true")
                        {
                            displayUrpMsg = true;
                        }
                    }

                    if (displayUrpMsg)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.BeginVertical("box");
                        GUILayout.Label("<color=red><b>ATTENTION</b> You are using the Universal Render Pipeline (URP). To render 'Bridges In Seconds' materials properly, you should install the URP package.</color>", warningLabelStyle);
                        if (GUILayout.Button("<b>Install Now</b>", buttonStyle))
                        {
                            string urpPathPrefix = PathProviderProto.GetUrpPathPrefix();
                            string urpPath = urpPathPrefix + "/URP_Specific_Pack.unitypackage";
                            bool urpPackageExists = File.Exists(urpPath);
                            if (urpPackageExists)
                            {
                                AssetDatabase.ImportPackage(urpPath, true);
                            }
                            else EditorUtility.DisplayDialog("Error", "The URP package has not been found", "OK");

                        }
                        GUILayout.EndVertical();
                        EditorGUILayout.Space();
                        scrollHeight = 70;
                    }
                }


                GUILayout.BeginHorizontal();
                if (GUILayout.Button("<b>Quick start</b>\n<size=10>A step by step document to build my first bridge</size>", buttonStyle, GUILayout.MaxWidth(265), GUILayout.Height(40)))
                {
                    string file = "file:///" + Application.dataPath + "/3DWorldInSeconds/Bridges/Documentation/Quick Start.pdf";
                    Application.OpenURL(file);
                }


                if (GUILayout.Button("<b>Tutorial Videos </b>\n<size=10>Watch how to begin and to go on</size>", buttonStyle, GUILayout.MaxWidth(265), GUILayout.Height(40)))
                    Application.OpenURL("http://3dworldinseconds.com/Tutorials");



                GUILayout.EndHorizontal();
                if (GUILayout.Button("<b>Open the Tutorial Scene</b>\n<size=12>To follow either the Quick Start document or the Tutorial Videos</size>", buttonStyle, GUILayout.MaxWidth(530), GUILayout.Height(40)))
                {
                    string ScenePathPrefix = pathProvider.GetScenePathPrefix();
                    string scenePath = ScenePathPrefix + "/Tutorial.unity";
                    //Open the Scene in the Editor (do not enter Play Mode)
                    if (File.Exists(scenePath))
                    {
                        bool confirm = EditorUtility.DisplayDialog("Warning", "Any unregistered change in the current scene will be lost. Confirm ?", "Ok", "Cancel");
                        if (confirm)
                        {
                            EditorSceneManager.OpenScene(scenePath);
                        }
                        
                    }
                    else EditorUtility.DisplayDialog("Error", "The Tutorial scene has been removed", "OK");

                }
                GUILayout.Space(10);
                GUILayout.Label("<b><size=16>Credits:</size></b>", leftLabelStyle);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.BeginHorizontal();
                
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition,false, true, GUILayout.Width(530), GUILayout.Height(scrollHeight));
                GUILayout.Label("- Some <b>Standard Assets</b> are referenced in the Demo and Tutorial scenes but are not necessary for running the bridges tools.", leftLabelStyle);
                GUILayout.Space(10);

                GUILayout.Label("- The tutorial and demo scenes trees were generated with the free software <b>Tree it</b> of EVOLVED-Software. See:", leftLabelStyle);
                DisplayHyperlink("www.evolved-software.com", "https://www.evolved-software.com/treeit/treeit");
                GUILayout.Space(10);

                GUILayout.Label("- The tutorial and demo scenes trees and rocks imposter billboards were generated with the public domain imposter <b>IMP</b>. See:", leftLabelStyle);
                DisplayHyperlink("www.github.com/xraxra/IMP", "https://github.com/xraxra/IMP");
                GUILayout.Space(10);

                GUILayout.Label("- The tutorial and demo scenes grass is a derivative work of the public domain grass pack <b>Grass Pack #01</b> provided by Nobiax. See:", leftLabelStyle);
                DisplayHyperlink("www.opengameart.org/content/grass-pack-01", "https://opengameart.org/content/grass-pack-01");
                GUILayout.Label("Public Domain, see:", leftLabelStyle);
                DisplayHyperlink("CC0 ", "https://creativecommons.org/publicdomain/zero/1.0/");
                GUILayout.Space(10);

                GUILayout.Label("- The bridges base models were built with the <b>ARCHIMATIX Asset</b> available on the Asset Store. See:", leftLabelStyle);
                DisplayHyperlink("www.archimatix.com", "https://archimatix.com/");
                GUILayout.Space(10);

                GUILayout.Label("- Bridges archs were extruded with a script based on the <b>Unity - PolyExtruder</b> script that uses <b>Triangle.dll</b> provided by nicoversity. See:", leftLabelStyle);
                DisplayHyperlink("www.github.com/nicoversity/unity_polyextruder", "https://github.com/nicoversity/unity_polyextruder");
                GUILayout.Label("MIT License, see:", leftLabelStyle);
                DisplayHyperlink("License.md", "https://github.com/nicoversity/unity_polyextruder/blob/master/LICENSE.md");
                GUILayout.Space(10);

                GUILayout.Label("- Original <b>Triangle.dll</b> is a part of the <b>Triangle.net</b> project. See:", leftLabelStyle);
                DisplayHyperlink("www;github.com/garykac/triangle.net", "https://github.com/garykac/triangle.net");
                GUILayout.Label("MIT License, see:", leftLabelStyle);
                DisplayHyperlink("License.md", "https://github.com/garykac/triangle.net/blob/master/LICENSE");
                GUILayout.Space(10);

                GUILayout.Label("- The medieval street lamp is a derivative from Dan <b>medieval street lamp</b> work. See:", leftLabelStyle);
                DisplayHyperlink("www.opengameart.org/content/medieval-street-lamp", "https://opengameart.org/content/medieval-street-lamp");
                GUILayout.Label("Public Domain, see:", leftLabelStyle);
                DisplayHyperlink("CC0 ", "https://creativecommons.org/publicdomain/zero/1.0/");
                GUILayout.Space(10);

                GUILayout.Label("- The monk statue is a derivative from Čestmír Dammer <b>Monk</b> work. See:", leftLabelStyle);
                DisplayHyperlink("www.opengameart.org/content/monk", "https://opengameart.org/content/monk");
                GUILayout.Label("Public Domain, see:", leftLabelStyle);
                DisplayHyperlink("CC0 ", "https://creativecommons.org/publicdomain/zero/1.0/");
                GUILayout.Space(10);

                GUILayout.Label("- The soldier statue is a derivative from Daniel Andersson <b>Medieval Statue</b> work. See:", leftLabelStyle);
                DisplayHyperlink("opengameart.org/content/medieval-statue", "https://opengameart.org/content/medieval-statue");
                GUILayout.Label("Public Domain, see:", leftLabelStyle);
                DisplayHyperlink("CC0 ", "https://creativecommons.org/publicdomain/zero/1.0/");
                GUILayout.Space(10);

                GUILayout.Label("- The Victorian Lamp is a derivative from <b>Street Light</b> work. See:", leftLabelStyle);
                DisplayHyperlink("3dmodelscc0.com/model/street-light", "https://www.3dmodelscc0.com/model/street-light");
                GUILayout.Label("Public Domain, see:", leftLabelStyle);
                DisplayHyperlink("CC0 ", "https://creativecommons.org/publicdomain/zero/1.0/");
                GUILayout.Space(10);

                GUILayout.Label("- The Victorian Lamp Low Poly is a derivative from Catalin Pavel <b>Low Poly Lamp</b> work. See:", leftLabelStyle);
                DisplayHyperlink("opengameart.org/content/low-poly-lamp", "https://opengameart.org/content/low-poly-lamp");
                GUILayout.Label("Public Domain, see:", leftLabelStyle);
                DisplayHyperlink("CC0 ", "https://creativecommons.org/publicdomain/zero/1.0/");
                GUILayout.Space(10);

                GUILayout.Label("- The Dragon statue is a derivative from Cethiel <b>Dragon</b> work. See:", leftLabelStyle);
                DisplayHyperlink("www.opengameart.org/content/dragon-fully-animated", "https://opengameart.org/content/dragon-fully-animated");
                GUILayout.Label("Public Domain, see:", leftLabelStyle);
                DisplayHyperlink("CC0 ", "https://creativecommons.org/publicdomain/zero/1.0/");
                GUILayout.Space(10);

                GUILayout.Label("- The Skull statue is a derivative from CDmir <b>Human Skull</b> work. See:", leftLabelStyle);
                DisplayHyperlink("www.opengameart.org/content/human-skull-0", "https://opengameart.org/content/human-skull-0");
                GUILayout.Label("Public Domain, see:", leftLabelStyle);
                DisplayHyperlink("CC0 ", "https://creativecommons.org/publicdomain/zero/1.0/");
                GUILayout.Space(10);

                GUILayout.Label("- The statue with lamp is a derivative from Colorado Stark <b>Statue holding Lightpost</b> work. See:", leftLabelStyle);
                DisplayHyperlink("www.opengameart.org/content/statue-holding-lightpost", "https://opengameart.org/content/statue-holding-lightpost");
                GUILayout.Label("Public Domain, see:", leftLabelStyle);
                DisplayHyperlink("CC0 ", "https://creativecommons.org/publicdomain/zero/1.0/");
                GUILayout.Space(10);

                GUILayout.Label("- The Lion statue is a derivative from Thomas Flynn <b>Lion Statue - optimized</b> work. See:", leftLabelStyle);
                DisplayHyperlink("www.sketchfab.com/3d-models/lion-statue-optimized", "https://sketchfab.com/3d-models/lion-statue-optimized-62662d27c94d4994b2479b8de3a66ca7");
                GUILayout.Label("Public Domain, see:", leftLabelStyle);
                DisplayHyperlink("CC0 ", "https://creativecommons.org/publicdomain/zero/1.0/");
                GUILayout.Space(10);

                GUILayout.Label("- The Buddha statue is a derivative from Minneapolis Institute of Art <b>3rd C CE Gandhara Buddha</b> work. See:", leftLabelStyle);
                DisplayHyperlink("www.sketchfab.com/3d-models/3rd-c-ce-gandhara-buddha", "https://sketchfab.com/3d-models/3rd-c-ce-gandhara-buddha-1cd1470645334a76ae23b755b53fb736");
                GUILayout.Label("Public Domain, see:", leftLabelStyle);
                DisplayHyperlink("CC0 ", "https://creativecommons.org/publicdomain/zero/1.0/");
                GUILayout.Space(10);

                GUILayout.Label("- The House1, House2, House3, House4 are a derivative from <b>Daniel Andersson work </b> work. See:", leftLabelStyle);
                DisplayHyperlink("www.opengameart.org/content/medieval-house-pack", "https://opengameart.org/content/medieval-house-pack");
                GUILayout.Label("Public Domain, see:", leftLabelStyle);
                DisplayHyperlink("CC0 ", "https://creativecommons.org/publicdomain/zero/1.0/");
                GUILayout.Space(10);

                GUILayout.Label("- The Hut1 is a derivative from <b>Spiral Softworks</b> work. See:", leftLabelStyle);
                DisplayHyperlink("www.opengameart.org/content/old-medieval-house", "https://opengameart.org/content/old-medieval-house");
                GUILayout.Label("Public Domain, see:", leftLabelStyle);
                DisplayHyperlink("CC0 ", "https://creativecommons.org/publicdomain/zero/1.0/");
                GUILayout.Space(10);

                GUILayout.Label("- The Hut2 is a derivative from <b>OliverMH</b> work. See:", leftLabelStyle);
                DisplayHyperlink("www.blendswap.com/blend/10864", "https://blendswap.com/blend/10864");
                GUILayout.Label("Public Domain, see:", leftLabelStyle);
                DisplayHyperlink("CC0 ", "https://creativecommons.org/publicdomain/zero/1.0/");
                GUILayout.Space(10);

                GUILayout.Label("- Easyroads3D v3(TM) is a trademark of <b>AndaSoft</b>. See:", leftLabelStyle);
                DisplayHyperlink("www.easyroads3d.com/", "https://www.easyroads3d.com/");
                GUILayout.Space(10);



                GUILayout.EndScrollView();
                GUILayout.EndHorizontal();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("<b>Premium version</b>\n<size=10>Get the full version on the Asset Store</size>", buttonStyle, GUILayout.MaxWidth(530), GUILayout.Height(40)))
                    Application.OpenURL("http://u3d.as/2KnS");
                if (GUILayout.Button("<b>Web Site</b>\n<size=12>3dworldinseconds.com</size>", buttonStyle, GUILayout.MaxWidth(530), GUILayout.Height(40)))
                    Application.OpenURL("http://3dworldinseconds.com");
                if (GUILayout.Button("<b>E-mail</b>\n<size=12>3dworldinseconds@gmail.com</size>", buttonStyle, GUILayout.MaxWidth(530), GUILayout.Height(40)))
                    Application.OpenURL("mailto:3dworldinseconds@gmail.com");
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private void DisplayHyperlink(string dsplayedLink, string url)
        {
            if (GUILayout.Button(dsplayedLink, linkStyle, GUILayout.ExpandWidth(false)))
            {
                Application.OpenURL(url);
            }
            // Get the last rect to display the line
            Rect lastRect = GUILayoutUtility.GetLastRect();
            lastRect.y += lastRect.height - 2; // Vertical alignment of the underline
            lastRect.height = 2; // Thickness of the line
            GUIStyle undelineStyle = new GUIStyle(GUI.skin.box);
            undelineStyle.normal.background = MakeTex((int)Mathf.Ceil(lastRect.x), 2, Color.blue);
            GUI.Box(lastRect, "", undelineStyle);
        }
    }
}


