using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [RequireComponent(typeof(Image))]
    [ExecuteInEditMode]
    public class STSScreenGauge : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        [Header("Connect images")]
        public Image ImageBackground;
        public Image ImageFill;
        public Image ImageOverLay;
        //-------------------------------------------------------------------------------------------------------------
        [Header("Expand zone")]
        public bool HorizontalExpand = true;
        public float HorizontalMin = 30.0f;
        public bool VerticalExpand = false;
        public float VerticalMin = 30.0f;
        //-------------------------------------------------------------------------------------------------------------
        [Header("Expand value")]
        [Range(0.0F, 1.0F)]
        public float HorizontalValue = 1.0F;
        [Range(0.0F, 1.0F)]
        public float VerticalValue = 1.0F;
        //-------------------------------------------------------------------------------------------------------------
        public void SetHidden(bool sValue)
        {
            gameObject.SetActive(!sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CheckHorizontalValue()
        {
            if (HorizontalValue > 1.0F)
            {
                HorizontalValue = 1.0F;
            }
            else if (HorizontalValue < 0)
            {
                HorizontalValue = 0.0F;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CheckVerticalValue()
        {
            if (VerticalValue > 1.0F)
            {
                VerticalValue = 1.0F;
            }
            else if (VerticalValue < 0)
            {
                VerticalValue = 0.0F;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            ReDraw();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Update()
        {
            ReDraw();
        }
        //-------------------------------------------------------------------------------------------------------------
        void ReDraw()
        {
            if (ImageBackground != null)
            {
                CheckHorizontalValue();
                CheckVerticalValue();
                Rect tRect = ImageBackground.rectTransform.rect;
                float tW = tRect.width;
                if (HorizontalExpand)
                {
                    tW = HorizontalMin + (tRect.width - HorizontalMin) * HorizontalValue;
                }
                float tH = tRect.height;
                if (VerticalExpand)
                {
                    tH = VerticalMin + (tRect.height - VerticalMin) * VerticalValue;
                }
                ImageFill.rectTransform.sizeDelta = new Vector2(tW, tH);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
