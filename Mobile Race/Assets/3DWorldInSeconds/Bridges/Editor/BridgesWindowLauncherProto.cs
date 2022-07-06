/*--------------------------------------------------------
  BridgesWindowLauncherProto.cs

  Created by Alain Debelley on 2021-12-22.
  Copyright (c) 2021 ALAIN DEBELLEY. All rights reserved.
  http://www.3dworldinseconds.com/
--------------------------------------------------------*/
using UnityEngine;
using UnityEditor;
using System.IO;

namespace WorldInSeconds3DProto
{
    public class BridgesWindowLauncherProto
    {
        private static readonly string version = "V01.1";
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void OnProjectLoadedInEditor()
        {
            // is it the prototyping version ?
            string worldInSecondsFolder = "Assets/3DWorldInSeconds";
            string premiumFolder = worldInSecondsFolder + "/PremiumBridges";
            bool premiumFolderExists = Directory.Exists(premiumFolder);
           
            string pathPrefix = PathProviderProto.GetLogPathPrefix();
            string path = pathPrefix + "/startupWindowProtoShown.txt";

            bool fileExists = File.Exists(path);
            if (!fileExists)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    BridgesStartupWindowProto.Open(); 
                    StreamWriter writer = new StreamWriter(path, false);
                    writer.WriteLine(version);
                    writer.Close();
                };
              
            }
            else
            {
                StreamReader reader = new StreamReader(path);
                string recordedVersion = reader.ReadLine();
                reader.Close();
                if (recordedVersion != version)
                {
                    BridgesStartupWindowProto.Open();
                    StreamWriter writer = new StreamWriter(path, false);
                    writer.WriteLine(version);
                    writer.Close();
                }
            }
        }
#endif
    }
}

