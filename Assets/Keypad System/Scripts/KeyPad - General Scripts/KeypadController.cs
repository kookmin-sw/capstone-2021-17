﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace keypadSystem
{
    public class KeypadController : MonoBehaviour
    {
        [Header("Keypad Parameters")]
        [SerializeField] public string validCode;
        public int characterLim;
        [HideInInspector] public bool firstClick;
        
        [Header("UI Elements")]
        public InputField codeText;
        [SerializeField] private GameObject keyPadCanvas;

        [Header("GameObjects")]
        [SerializeField] private GameObject keypadModel;

        [Header("Unlock Event")]
        [SerializeField] private UnityEvent unlock;

        public Text quizInfo;

        public void CheckCode()
        {
            if (codeText.text == validCode)
            {
                keypadModel.tag = "Untagged";
                keypadModel.layer = 0;
                ValidCode();
            }

            else
            {
                KPAudioManager.instance.Play("KeypadDenied"); //0.2f
            }
        }

        void ValidCode() 
        {
            //IF YOUR CODE IS CORRECT!
            unlock.Invoke();
        }

        public void ShowKeypad()
        {
            KPDisableManager.instance.DisablePlayer(true);
            keyPadCanvas.SetActive(true);
        }

        public void CloseKeypad()
        {
            KPDisableManager.instance.DisablePlayer(false);
            keyPadCanvas.SetActive(false);
        }

        public void SingleBeep()
        {
            KPAudioManager.instance.Play("KeypadBeep");
        }

        public void SetPassword()
        {
            validCode = GameMgr.GeneratePassword(characterLim);
            //Debug.Log(validCode);
        }

        public int GetPassword()
        {
            string strValue = validCode;
            int intValue = 0;
            int.TryParse(strValue, out intValue);
            return intValue;
        }

        public void GenerateQuiz()
        {
            string quizText = null;
            int quizInt = GetPassword();
            int plusNum = 0;
            int minusNum = 0;
            int resultNum = 0;
            int randomCase=0;
            randomCase = Random.Range(0, 2);
            plusNum = Random.Range(0, quizInt);
            minusNum = Random.Range(0, 9999 - quizInt);
            
            switch (randomCase)
            {
                case 0:
                    resultNum = quizInt - plusNum;
                    quizText = resultNum.ToString() + " + " + plusNum.ToString() + "= 0000";
                    break;
                case 1:
                    resultNum = quizInt + minusNum;
                    quizText = resultNum.ToString() + " - " + minusNum.ToString() + "= 0000";
                    break;
            }

            quizInfo.text = quizText;
            Debug.Log(quizInfo.text);
        }
    }
}