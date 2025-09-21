using System;
using DG.Tweening;
using UnityEngine;

namespace ArcherGame.Game3.Helper
{
    public static class GameUtilities
    {
        public static void Hide(this GameObject obj) => obj.SetActive(false);

        public static void Hide(this Component component) => component.gameObject.SetActive(false);

        public static void Show(this GameObject obj, bool show = true) => obj.SetActive(show);

        public static void Show(this Component component, bool show = true) => component.gameObject.SetActive(show);

        public static bool IsActive(this Component component) => component.gameObject.activeSelf;
        
        public static void Minimize(this GameObject obj) => obj.transform.localScale = Vector3.zero;

        public static void Minimize(this Component component) => component.gameObject.transform.localScale = Vector3.zero;

        public static void SetInvisible(this GameObject obj, float duration = 0, float delayTime = 0, Action completeHandle = null)
        {
            var canvasGroup = obj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>();
            }

            canvasGroup.DOFade(0, duration).IgnoreTimescale().SetDelay(delayTime).OnComplete(() =>
            {
                completeHandle?.Invoke();
            });
        }
    
        public static void SetInvisible(this Component component, float duration = 0, float delayTime = 0, Action completeHandle = null)
        {
            var canvasGroup = component.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = component.gameObject.AddComponent<CanvasGroup>();
            }

            canvasGroup.DOFade(0, duration).IgnoreTimescale().SetDelay(delayTime).OnComplete(() =>
            {
                completeHandle?.Invoke();
            });
        }
    
        public static void SetVisible(this GameObject obj, float duration = 0, float delayTime = 0, Action completeHandle = null)
        {
            var canvasGroup = obj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>();
            }

            canvasGroup.DOFade(1, duration).IgnoreTimescale().SetDelay(delayTime).OnComplete(() =>
            {
                completeHandle?.Invoke();
            });
        }
    
        public static void SetVisible(this Component component, float duration = 0, float delayTime = 0, Action completeHandle = null)
        {
            var canvasGroup = component.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = component.gameObject.AddComponent<CanvasGroup>();
            }

            canvasGroup.DOFade(1, duration).IgnoreTimescale().SetDelay(delayTime).OnComplete(() =>
            {
                completeHandle?.Invoke();
            });
        }
        
        public static Tweener IgnoreTimescale(this Tweener tweener) => tweener.SetUpdate(true);

        public static Tweener IgnoreTimescale(this Tweener tweener, Ease ease) => tweener.SetEase(ease).SetUpdate(true);
    }
}