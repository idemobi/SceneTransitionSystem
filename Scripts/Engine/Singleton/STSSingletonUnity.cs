//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	SceneTransitionSystem for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSSingletonBasis : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public bool Initialized = false;
        //-------------------------------------------------------------------------------------------------------------
        public virtual void InitInstance()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSSingletonUnity<K> : STSSingletonBasis where K : STSSingletonBasis, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        private static K kSingleton = null;
        //-------------------------------------------------------------------------------------------------------------
        private void Awake()
        {
            Debug.Log("STSSingleton<K> Awake() for gameobject named '" + gameObject.name + "'");
            //Check if there is already an instance of K
            if (kSingleton == null)
            {
                Debug.Log("STSSingleton<K> Awake() case kSingleton == null for gameobject named '" + gameObject.name + "'");
                //if not, set it to this.
                kSingleton = this as K;
                if (Initialized == false)
                {
                    Debug.Log("STSSingleton<K> Awake() case kSingleton.Initialized == false for gameobject named '" + gameObject.name + "'");
                    // Init Instance
                    InitInstance();
                    // memorize the init instance
                    Initialized = true;
                }
                //Set K's gameobject to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
                DontDestroyOnLoad(gameObject);
            }
            //If instance already exists:
            if (kSingleton != this)
            {
                Debug.Log("STSSingleton<K> Awake() case kSingleton != this for gameobject named '" + gameObject.name + "'");
                //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
                Debug.Log("singleton prevent destruction gameobject named '" + gameObject.name + "'");
                Destroy(this);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K Singleton()
        {
            Debug.Log("STSSingleton<K> Singleton()");
            if (kSingleton == null)
            {
                Debug.Log("STSSingleton<K> Singleton() case kSingleton == null");
                // I need to create singleton
                GameObject tObjToSpawn;
                //spawn object
                tObjToSpawn = new GameObject(typeof(K).Name + " Singleton");
                //Add Components
                tObjToSpawn.AddComponent<K>();
                // keep k_Singleton
                kSingleton = tObjToSpawn.GetComponent<K>();
            }
            else
            {
                Debug.Log("STSSingleton<K> Singleton() case kSingleton != null (exist in gameobject named '" + kSingleton.gameObject.name + "')");
            }
            return kSingleton;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void InitInstance()
        {
            Debug.Log("STSSingleton<K> InitInstance() for gameobject named '" + gameObject.name + "'");
            // do something by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnDestroy()
        {
            Debug.Log("STSSingleton<K> OnDestroy() for gameobject named '" + gameObject.name + "'");
            if (kSingleton == this)
            {
                kSingleton = null;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================