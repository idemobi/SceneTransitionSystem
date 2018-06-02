using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using UnityEngine;

//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSEffectNameAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string ClassName;
        //-------------------------------------------------------------------------------------------------------------
        public STSEffectNameAttribute(string sClassName)
        {
            this.ClassName = sClassName;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSNoTintPrimaryAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSNoTintSecondaryAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSNoTexturePrimaryAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSNoTextureSecondaryAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSNoParameterOneAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSNoParameterTwoAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSNoParameterThreeAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSNoOffsetAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSParameterOneEntitleAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Name;
        public int Min;
        public int Max;
        //-------------------------------------------------------------------------------------------------------------
        public STSParameterOneEntitleAttribute(string sName, int sMin, int sMax)
        {
            this.Name = sName;
            this.Min = sMin;
            this.Max = sMax;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSParameterTwoEntitleAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Name;
        public int Min;
        public int Max;
        //-------------------------------------------------------------------------------------------------------------
        public STSParameterTwoEntitleAttribute(string sName, int sMin, int sMax)
        {
            this.Name = sName;
            this.Min = sMin;
            this.Max = sMax;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSParameterThreeEntitleAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Name;
        public int Min;
        public int Max;
        //-------------------------------------------------------------------------------------------------------------
        public STSParameterThreeEntitleAttribute(string sName, int sMin, int sMax)
        {
            this.Name = sName;
            this.Min = sMin;
            this.Max = sMax;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class STSEffectBase
    {
        //[SerializeField]
        public string EffectName = "";
        public Color TintPrimary = Color.black;
        public Color TintSecondary = Color.black;
        public Texture2D TexturePrimary = null;
        public Texture2D TextureSecondary = null;
        public Vector2 Offset;

        public int ParameterOne;
        public int ParameterTwo;
        public int ParameterThree;

        public float Duration = 1.0F;
        public float Purcent = 0.0F;
        //-------------------------------------------------------------------------------------------------------------
        // Pseudo private... pblic but not in the inspector
        public int Direction = 0; // 0 is unknow; -1 go from 1.0F to 0.0F purcent; 1 go from 0.0F to 1.0F purcent
        public float AnimPurcent = 0.0F;
        public bool AnimIsPlaying = false;
        public bool AnimIsFinished = false;


        public Color OldColor;
        public float ColorDuration = 0.0F;
        public float ColorPurcent = 0.0F;
        public bool ColorIsPlaying = false;
        public bool ColorIsFinished = false;
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class STSEffectType : STSEffectBase
    {
        //-------------------------------------------------------------------------------------------------------------
        public static List<GUIContent> kEffectContentList = new List<GUIContent>();
        public static List<Type> kEffectTypeList = new List<Type>();
        public static List<string> kEffectNameList = new List<string>();
        //-------------------------------------------------------------------------------------------------------------
        public static STSEffectType Default = new STSEffectType("Fade", Color.black, 1.0F);
        public static STSEffectType QuickDefault = new STSEffectType("Fade", Color.black, 0.50F);
        public static STSEffectType Flash = new STSEffectType("Fade", Color.white, 0.50F);
        //-------------------------------------------------------------------------------------------------------------
        public STSEffectType()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public STSEffect GetEffect()
        {
            STSEffect rReturn = null;
            int tIndex = STSEffectType.kEffectNameList.IndexOf(EffectName);
            if (tIndex < 0 || tIndex >= STSEffectType.kEffectNameList.Count())
            {
                rReturn = new STSEffectFade();
            }
            else
            {
                Type tEffectType = STSEffectType.kEffectTypeList[tIndex];
                rReturn = (STSEffect)Activator.CreateInstance(tEffectType);
                // Copy Param
                rReturn.CopyFrom(this);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public STSEffectType(string sString, Color sColor, float sDuration)
        {
            EffectName = sString;
            TintPrimary = sColor;
            Duration = sDuration;
        }
        //-------------------------------------------------------------------------------------------------------------
        public STSEffectType(STSEffectType sObject)
        {
            CopyFrom(sObject);
        }
        //-------------------------------------------------------------------------------------------------------------
        public STSEffectType Dupplicate()
        {
            STSEffectType tCopy = new STSEffectType(this);
            return tCopy;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CopyFrom(STSEffectType sObject)
        {
            EffectName = sObject.EffectName;
            TintPrimary = sObject.TintPrimary;
            TintSecondary = sObject.TintSecondary;
            TexturePrimary = sObject.TexturePrimary;
            TextureSecondary = sObject.TextureSecondary;

            ParameterOne = sObject.ParameterOne;
            ParameterTwo = sObject.ParameterTwo;
            ParameterThree = sObject.ParameterThree;

            Duration = sObject.Duration;
            Purcent = sObject.Purcent;
        }
        //-------------------------------------------------------------------------------------------------------------
        static STSEffectType()
        {
            Type[] tAllTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            Type[] tAllNWDTypes = (from System.Type type in tAllTypes
                                   where type.IsSubclassOf(typeof(STSEffect))
                                   select type).ToArray();
            // reccord all the effect type
            foreach (Type tType in tAllNWDTypes)
            {
                string tClassName = tType.Name;
                if (tType.GetCustomAttributes(typeof(STSEffectNameAttribute), true).Length > 0)
                {
                    foreach (STSEffectNameAttribute tReference in tType.GetCustomAttributes(typeof(STSEffectNameAttribute), true))
                    {
                        tClassName = tReference.ClassName;
                    }
                }
                if (string.IsNullOrEmpty(tClassName))
                {
                    tClassName = tType.Name;
                }
                //MethodInfo tMethodInfo = tType.GetMethod("EffectName", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                //if (tMethodInfo != null)
                //{
                //    tClassName = tMethodInfo.Invoke(null, null) as string;
                //}
                kEffectContentList.Add(new GUIContent(tClassName));
                kEffectTypeList.Add(tType);
                kEffectNameList.Add(tClassName);
            }
            // Add Default
            kEffectNameList.Insert(0, "");
            kEffectTypeList.Insert(0, typeof(STSEffectFade));
            kEffectContentList.Insert(0, new GUIContent("Default"));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [STSEffectNameAttribute("Default effect")]
    // *** Remove some parameters in inspector
    // No remove
    // ***
    public class STSEffect : STSEffectType
    {
        //-------------------------------------------------------------------------------------------------------------
        protected void EstimatePurcent()
        {
            if (AnimPurcent < 1.0F || AnimPurcent >= 0.0F)
            {
                AnimPurcent += (Time.deltaTime) / Duration;
                if (AnimPurcent > 1.0F)
                {
                    AnimPurcent = 1.0F;
                    //AnimIsPlaying = false;
                    AnimIsFinished = true;
                }
                if (AnimPurcent < 0.0F)
                {
                    // IMPOSSIBLE
                    AnimPurcent = 0.0F;
                    //AnimIsPlaying = false;
                    AnimIsFinished = true;
                }
            }
            switch (Direction)
            {
                case 0:
                    // no direction do nothing ... on error
                    break;
                case 1:
                    Purcent = AnimPurcent;
                    break;
                case -1:
                    Purcent = 1 - AnimPurcent;
                    break;
            }
            //Debug.Log("STSEffect EstimatePurcent() => AnimPurcent = " + AnimPurcent.ToString("F3"));
            //Debug.Log("STSEffect EstimatePurcent() => Purcent = " + Purcent.ToString("F3"));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void StartEffectEnter(Rect sRect, Color sOldColor, float sInterEffectDelay)
        {
            PrepareEffectEnter(sRect);
            ColorIsPlaying = false;
            if (sInterEffectDelay > 0)
            {
                // I need do to a color transitionpublic float 
                ColorPurcent = 0.0F;
                ColorDuration = sInterEffectDelay;
                OldColor = sOldColor;
                ColorIsPlaying = true;
                ColorIsFinished = false;
            }
            else
            {
                ColorIsFinished = true;
            }
            AnimPurcent = 0.0F;
            Direction = -1;
            AnimIsPlaying = true;
            AnimIsFinished = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void StartEffectExit(Rect sRect)
        {
            PrepareEffectExit(sRect);
            AnimPurcent = 0.0F;
            Direction = 1;
            AnimIsPlaying = true;
            AnimIsFinished = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PauseEffect()
        {
            AnimIsPlaying = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void StopEffect()
        {
            AnimPurcent = 1.0F;
            AnimIsPlaying = false;
            AnimIsFinished = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetEffect()
        {
            AnimPurcent = 0.0F;
            Direction = 0;
            AnimIsPlaying = false;
            AnimIsFinished = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawMaster(Rect sRect)
        {
            // Do drawing
            if (ColorIsFinished == false)
            {
                ColorPurcent+= (Time.deltaTime) / ColorDuration;
                Color tColor = Color.Lerp(OldColor,TintPrimary,ColorPurcent);
                STSTransitionDrawing.DrawQuad(sRect,tColor);
                if (ColorPurcent >= 1)
                {
                    ColorIsPlaying = false;
                    ColorIsFinished = true;
                }
            }
            else
            {
                if (AnimIsPlaying == true) // play animation
                {
                    // estimate purcent
                    EstimatePurcent();
                    Draw(sRect);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void PrepareEffectEnter(Rect sRect)
        {
            // Prepare your datas to draw
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void PrepareEffectExit(Rect sRect)
        {
            // Prepare your datas to draw
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void Draw(Rect sRect)
        {
            // Do drawing with purcent
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================