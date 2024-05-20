using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using RPG.Saving;
using RPG.SceneManagement;
using System;
using System.IO;

namespace RPG.UI
{
    public class UiManager : MonoBehaviour
    {
        [Header("Menu Screen")]
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private GameObject menuScreen;

        [Header("Slider")]
        [SerializeField] private Slider slider;

        private const string ProgressKey = "gameProgress";
        public Button newGameButton;
        public Button continueButton;
        //SavingWrapper savingWrapper;

        //float experiencePoints = 0;
        //int sceneToLoad = 1;

        private void Start()
        {
            //SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            // Check if there is saved progress
            if (PlayerPrefs.HasKey(ProgressKey))
            {
                ShowContinueOption();
            }
            else
            {
                ShowNewGameOption();
            }
        }

        public void Play(int sceneToLoad)
        {
            loadingScreen.SetActive(true);
            menuScreen.SetActive(false);

            StartCoroutine(LoadLevelAsync(sceneToLoad));

        }

        public void Exit()
        {
            Application.Quit();
        }

        IEnumerator LoadLevelAsync(int sceneToLoad)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);

            while (!loadOperation.isDone)
            {
                float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
                slider.value = progressValue;
                yield return null;
            }

        }


        void ShowNewGameOption()
        {
            // Show only New Game button
            newGameButton.gameObject.SetActive(true);
            continueButton.gameObject.SetActive(false);
        }

        void ShowContinueOption()
        {
            // Show both New Game and Continue buttons
            newGameButton.gameObject.SetActive(true);
            continueButton.gameObject.SetActive(true);
        }

        public void OnNewGameButton()
        {
            // Start a new game and reset progress
            PlayerPrefs.DeleteKey(ProgressKey);
            StartNewGame();
        }

        public void OnContinueButton()
        {
            // Continue the game from saved progress
            ContinueGame();
        }

        void StartNewGame()
        {
            // Load the first level or reset the game state
            //SceneManager.LoadScene(1); // Replace with your first level scene name

            //savingWrapper.Delete();
            Play(1);
            File.Delete(Path.Combine(Application.persistentDataPath, "save" + ".sav"));  //this code is taken from SavingSystem.cs
            PlayerPrefs.SetInt(ProgressKey, 1);
            PlayerPrefs.Save();
        }

        void ContinueGame()
        {
            // Load the saved game state and continue
            //int savedLevel = PlayerPrefs.GetInt(ProgressKey);
            //SceneManager.LoadScene(savedLevel); // Replace with your level loading logic
            Play(1);
        }


    }
}
