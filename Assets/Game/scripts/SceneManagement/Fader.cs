using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentActivefade;
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
            
        }

        public IEnumerator FadeOut(float time)
        {
            return Fade(1, time);
        }

        

        public IEnumerator FadeIn(float time)
        {
            return Fade(0, time);
        }

        public IEnumerator Fade(float target, float time)
        {
            if (currentActivefade != null)
            {
                StopCoroutine(currentActivefade);
            }
            currentActivefade = StartCoroutine(FadeRoutine(target, time));
            yield return currentActivefade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }

    }
}
