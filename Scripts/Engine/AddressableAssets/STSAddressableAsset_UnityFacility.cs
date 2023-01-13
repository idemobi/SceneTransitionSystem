//=====================================================================================================================
//
//  ideMobi 2023
//
//  Author		Dolwen (Jérôme DEMYTTENAERE) 
//  Email		jerome.demyttenaere@gmail.com
//  Project 	SceneTransitionSystem for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Events;

//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class STSSceneManagerAddressableAssets : STSSingletonUnity<STSSceneManagerAddressableAssets>, STSTransitionInterface, STSIntermissionInterface
    {
        //-------------------------------------------------------------------------------------------------------------
        public static Scene GetActiveScene()
        {
            return SceneManager.GetActiveScene();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Scene GetSceneAt(int sIndex)
        {
            return SceneManager.GetSceneAt(sIndex);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Scene GetSceneByName(string sName)
        {
            return SceneManager.GetSceneByName(sName);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Scene GetSceneByPath(string sPath)
        {
            return SceneManager.GetSceneByPath(sPath);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadScene(string sSceneName, LoadSceneMode sLoadSceneMode = LoadSceneMode.Single, string sSceneIntermission = null, STSTransitionData sDatas = null, bool sAllowCyclic = false)
        {
            INTERNAL_LoadScene(sSceneName, sLoadSceneMode, sSceneIntermission, sDatas, sAllowCyclic);
        }
        //-------------------------------------------------------------------------------------------------------------
        /*public static AsyncOperation LoadSceneAsync(string sSceneName, LoadSceneMode sLoadSceneMode = LoadSceneMode.Single, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            INTERNAL_LoadScene(sSceneName, sLoadSceneMode, sSceneIntermission, sDatas);
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static AsyncOperation LoadSceneAsync(string sSceneName, LoadSceneParameters sParameters, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            LoadSceneMode tLoadSceneMode = sParameters.loadSceneMode;
            INTERNAL_LoadScene(sSceneName, tLoadSceneMode, sSceneIntermission, sDatas);
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static AsyncOperation LoadSceneAsync(string sSceneName, bool sAllowCyclic, LoadSceneMode sLoadSceneMode = LoadSceneMode.Single, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            INTERNAL_LoadScene(sSceneName, sLoadSceneMode, sSceneIntermission, sDatas, sAllowCyclic);
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static AsyncOperation LoadSceneAsync(string sSceneName, bool sAllowCyclic, LoadSceneParameters sParameters, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            LoadSceneMode tLoadSceneMode = sParameters.loadSceneMode;
            INTERNAL_LoadScene(sSceneName, tLoadSceneMode, sSceneIntermission, sDatas, sAllowCyclic);
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static AsyncOperation UnloadSceneAsync(string sSceneName, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            RemoveScene(SceneManager.GetActiveScene().name, sSceneName, sSceneIntermission, sDatas);
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static AsyncOperation UnloadSceneAsync(Scene sScene, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            string tSceneName = sScene.name;
            RemoveScene(SceneManager.GetActiveScene().name, tSceneName, sSceneIntermission, sDatas);
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static AsyncOperation UnloadSceneAsync(string sSceneName, UnloadSceneOptions sOptions, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            RemoveScene(SceneManager.GetActiveScene().name, sSceneName, sSceneIntermission, sDatas);
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static AsyncOperation UnloadSceneAsync(Scene sScene, UnloadSceneOptions sOptions, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            string tSceneName = sScene.name;
            return null;
        }*/
        //-------------------------------------------------------------------------------------------------------------
        public static void SetActiveScene(Scene sScene)
        {
            SceneManager.SetActiveScene(sScene);
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void INTERNAL_LoadScene(string sSceneName, LoadSceneMode sLoadSceneMode, string sSceneIntermission = null, STSTransitionData sDatas = null, bool sAllowCyclic = false)
        {
            switch (sLoadSceneMode)
            {
                case LoadSceneMode.Single:
                    {
                        ReplaceAllByScene(SceneManager.GetActiveScene().name, sSceneName, sSceneIntermission, sDatas, sAllowCyclic);
                    }
                    break;
                case LoadSceneMode.Additive:
                    {
                        AddScene(SceneManager.GetActiveScene().name, sSceneName, sSceneIntermission, sDatas, sAllowCyclic);
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void INTERNAL_UnloadScene(string sSceneName, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            RemoveScene(SceneManager.GetActiveScene().name, sSceneName, null, sSceneIntermission, sDatas);
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Scene> GetAllLoadedScenes()
        {
            List<Scene> tSceneList = new List<Scene>();
            for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
            {
                Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                if (tScene.isLoaded == true)
                {
                    tSceneList.Add(tScene);
                }
            }
            return tSceneList;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================