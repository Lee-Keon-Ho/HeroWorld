using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
public class CSceneManager : MonoBehaviour
{
    public CanvasGroup Fade_img;
    public GameObject Loading;
    public TextMeshProUGUI Loading_text;
    public Image progresBar;
    public Image progresBarFrame;
    float fadeDuration = 0.5f;
    public static CSceneManager Instance
    {
        get
        {
            return instance;
        }
    }

    private static CSceneManager instance;
    void Start()
    {
        if(instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnChangeScene(string _sceneName)
    {
        Fade_img.DOFade(1, fadeDuration)
            .OnStart(() =>
            {
                Fade_img.blocksRaycasts = true;
            })
            .OnComplete(() =>
            {
                StartCoroutine("LoadScene", _sceneName);
            });
    }

    IEnumerator LoadScene(string sceneName)
    {
        Loading.SetActive(true); //로딩 화면을 띄움
        progresBar.gameObject.SetActive(true);
        progresBarFrame.gameObject.SetActive(true);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; //퍼센트 딜레이용

        float past_time = 0;
        float percentage = 0;

        while (!(async.isDone))
        {
            yield return null;

            past_time += Time.deltaTime;

            if (percentage >= 90)
            {
                percentage = Mathf.Lerp(percentage, 100, past_time);
                progresBar.fillAmount = percentage;
                if (percentage == 100)
                {
                    async.allowSceneActivation = true; //씬 전환 준비 완료
                }
            }
            else
            {
                percentage = Mathf.Lerp(percentage, async.progress * 100f, past_time);
                if (percentage >= 90) past_time = 0;
                progresBar.fillAmount = async.progress;
            }
            Loading_text.text = percentage.ToString("0") + "%"; //로딩 퍼센트 표기
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트에서 제거*
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Fade_img.DOFade(0, fadeDuration)
        .OnStart(() => {
            Loading.SetActive(false);
            progresBar.fillAmount = 0.0f;
            Loading_text.text = "0";
            progresBar.gameObject.SetActive(false);
            progresBarFrame.gameObject.SetActive(false);
        })
        .OnComplete(() => {
            Fade_img.blocksRaycasts = false;
        });
    }
}
