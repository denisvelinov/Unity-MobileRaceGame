/*--------------------------------------------------------
  TerrainSaver.cs

  Created by Alain Debelley on 2021-12-22.
  Copyright (c) 2021 ALAIN DEBELLEY. All rights reserved.
  http://www.3dworldinseconds.com/
--------------------------------------------------------*/
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace WorldInSeconds3DPremium
{
    public class TerrainSaver : MonoBehaviour
    {
#if UNITY_EDITOR
        public string BackupFolder = "Assets/3DWorldInSeconds/TerrainSaver/TerrainBackup";
        public int VersionsToKeep = 5;
        /// <summary>
        /// Restores the terrain specified by the asset name
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        public void RestoreTerrain(string assetName)
        {
            TerrainData td1;
            TerrainData td2;

            Terrain terrain = GetComponent<Terrain>();
            if (terrain == null)
            {
                Debug.LogError("No terrain on GameObject: " + gameObject);
                return;
            }

            td1 = terrain.terrainData;
            // This is the backup name/path of the cloned TerrainData.


            td2 = (TerrainData)AssetDatabase.LoadAssetAtPath(BackupFolder + "/" + assetName + ".asset", typeof(TerrainData));
            if (td2 == null)
            {
                Debug.LogError("No TerrainData backup in a Resources folder, mussing folder is: Assets/Resources/TerrainData/");
                return;
            }
            td2.name = td1.name;
            if (EditorUtility.DisplayDialog("Confirmation", "Do you want to replace the current terrain with the saved terrain ?", "Ok", "Cancel"))
            {
                // Terrain collider
                td1.SetHeights(0, 0, td2.GetHeights(0, 0, td1.heightmapResolution, td1.heightmapResolution));
                // Textures
                td1.SetAlphamaps(0, 0, td2.GetAlphamaps(0, 0, td1.alphamapWidth, td1.alphamapHeight));
                // Trees
                td1.treeInstances = td2.treeInstances;
                // Grasses
                td1.SetDetailLayer(0, 0, 0, td2.GetDetailLayer(0, 0, td1.detailWidth, td1.detailHeight, 0));

            }

        }
        /// <summary>
        /// Gets the list  bacuped terrains.
        /// </summary>
        /// <returns>the list of backuped terrains with the backup date</returns>
        public SortedList<DateTime, string> GetBacupedTerrains()
        {

            string backupPath = BackupFolder;
            SortedList<DateTime, string> versionsList = new SortedList<DateTime, string>();

            Terrain tOriginal = GetComponent<Terrain>();
            TerrainData td1 = tOriginal.terrainData;

            string[] guids1 = AssetDatabase.FindAssets(td1.name, new[] { backupPath });
            string[] assets = new string[guids1.Length];


            for (int i = 0; i < guids1.Length; i++)
            {
                string currentAssetPath = AssetDatabase.GUIDToAssetPath(guids1[i]);
                string currentAssetMetaPath = currentAssetPath + ".meta";
                FileInfo backupedTerrainFileInfo = new FileInfo(currentAssetPath);
                FileInfo backupedTerrainMetaFileInfo = new FileInfo(currentAssetMetaPath);
                DateTime creationTime = backupedTerrainMetaFileInfo.LastWriteTime;

                versionsList.Add(creationTime, Path.GetFileNameWithoutExtension(backupedTerrainFileInfo.Name));
            }
            return versionsList;
        }

        /// <summary>
        /// Purge backuped terrain in order to only keep the specified versions number
        /// </summary>
        /// <param name="versionsNumber">The versions number.</param>
        public void OnlyKeepNversions(int versionsNumber)
        {
            string backupPath = BackupFolder;
            SortedList<DateTime, string> versionsList = new SortedList<DateTime, string>();

            Terrain tOriginal = GetComponent<Terrain>();
            TerrainData td1 = tOriginal.terrainData;

            string[] guids1 = AssetDatabase.FindAssets(td1.name, new[] { backupPath });
            string[] assets = new string[guids1.Length];


            for (int i = 0; i < guids1.Length; i++)
            {
                string currentAssetPath = AssetDatabase.GUIDToAssetPath(guids1[i]);
                string currentAssetMetaPath = currentAssetPath + ".meta";
                FileInfo backupedTerrainFileInfo = new FileInfo(currentAssetPath);
                FileInfo backupedTerrainMetaFileInfo = new FileInfo(currentAssetMetaPath);
                DateTime creationTime = backupedTerrainMetaFileInfo.LastWriteTime;

                versionsList.Add(creationTime, currentAssetPath);
            }
            if (versionsList.Count < versionsNumber) return;
            else
            {
                int numberToDelete = versionsList.Count - versionsNumber + 1;
                int deletedNumber = 0;
                foreach (KeyValuePair<DateTime, string> kvp in versionsList)
                {
                    Debug.Log("Delete " + kvp.Value);
                    AssetDatabase.DeleteAsset(kvp.Value);
                    deletedNumber++;
                    if (deletedNumber >= numberToDelete) break;

                }

            }
        }

        /// <summary>
        /// Backup the current terrain.
        /// </summary>
        public void CopyTerrain()
        {
            //if (GenerateBackupFoldersIfNecessary(BackupFolder))
            if (AssetDatabase.IsValidFolder(BackupFolder))
            {
                OnlyKeepNversions(VersionsToKeep);

                Terrain tOriginal = GetComponent<Terrain>();
                TerrainData td1 = tOriginal.terrainData;
                string tdBackupPath = BackupFolder + "/" + td1.name + ".asset";
                tdBackupPath = AssetDatabase.GenerateUniqueAssetPath(tdBackupPath);
                string currentTerrainPath = AssetDatabase.GetAssetPath(td1);
                Debug.Log("Backup Terrain Data");
                AssetDatabase.CopyAsset(currentTerrainPath, tdBackupPath);
            }
            else Debug.LogError("The backup folder does not exist");



        }

        //private bool GenerateBackupFoldersIfNecessary(string rootFolder) // must be "Asset/..... Folder_name"
        //{
        //    bool success = false;
        //    if (AssetDatabase.IsValidFolder(rootFolder))
        //    {
        //        bool continueCheck = false;
        //        if (AssetDatabase.IsValidFolder(rootFolder))
        //        {
        //            continueCheck = true;
        //        }
        //        else if (AssetDatabase.CreateFolder(rootFolder, "TerrainBackup") != "")
        //        {
        //            continueCheck = true;
        //        }

        //        if (continueCheck)
        //        {
        //            if (AssetDatabase.IsValidFolder(rootFolder + "/TerrainBackup/Manual")) success = true;
        //            else if (AssetDatabase.CreateFolder(rootFolder + "/TerrainBackup", "Manual") != "") success = true;
        //        }


        //    }

        //    return success;
        //}
#endif
    }
}
#endif