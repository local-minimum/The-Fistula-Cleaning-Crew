using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDLanding : MonoBehaviour
{
    [SerializeField] bool animateOnStart = false;

    private static HUDLanding _instance { get; set; }

    public static void Show()
    {
        _instance.AnimateShow();
    }

    public static void Hide()
    {
        _instance.canvas.enabled = false;
    }


    Canvas canvas;
    TextMeshProUGUI textUI;
    [SerializeField] AnimationCurve alphaAnimation;
    [SerializeField, Range(0, 3)] float animationDuration;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            canvas = GetComponentInChildren<Canvas>();
            canvas.enabled = false;
            textUI = canvas.GetComponentInChildren<TextMeshProUGUI>();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (animateOnStart) AnimateShow();
    }

    private void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }

    private void AnimateShow()
    {
        StartCoroutine(EaseIn());
    }

    private IEnumerator<WaitForSeconds> EaseIn()
    {
        var start = Time.timeSinceLevelLoad;
        var progress = 0f;
        textUI.alpha = alphaAnimation.Evaluate(progress);
        _instance.canvas.enabled = true;

        while (progress < 1f)
        {
            textUI.alpha = alphaAnimation.Evaluate(progress);
            yield return new WaitForSeconds(0.02f);
            progress = (Time.timeSinceLevelLoad - start) / animationDuration;
        }
        textUI.alpha = alphaAnimation.Evaluate(1f);
    }
}
