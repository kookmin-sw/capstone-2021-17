using UnityEngine;
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


        //Add NetGame Player (Custom)
        [SerializeField]
        private ItemBoxNetBehaviour itemBoxNet;

        public void CheckCode()
        {
            if (codeText.text == validCode)
            {
                if(itemBoxNet != null)
                {
                    itemBoxNet.UnableKeypad();
                }
                codeText.text = "Ok";
                UnableKeypad();
                ValidCode();
                Invoke("CloseKeypad", 1.0f);
            }
            else
            {
                if (codeText.text.Length == characterLim)
                {
                    codeText.text = "Err";
                    KPAudioManager.instance.Play("KeypadDenied"); //0.2f
                    Invoke("ClearCode", 1.0f);
                }
            }
        }

        void ClearCode()
        {
            codeText.text = string.Empty;
            codeText.characterLimit = 0;
            firstClick = false;
        }

        public void UnableKeypad()
        {
            keypadModel.tag = "Untagged";
            keypadModel.layer = 0;
        }

        void ValidCode() 
        {
            //IF YOUR CODE IS CORRECT!
            unlock.Invoke();
        }

        public void ShowKeypad()
        {
            if (itemBoxNet != null )
            {
                if(itemBoxNet.IsUsing == false)
                {
                    itemBoxNet.SetUsing(true); // Set IsUsing true [Command] 
                }
                else
                {
                    Debug.Log("Other Player Using this ItemBox");
                    return;
                }
            }

            KPDisableManager.instance.DisablePlayer(true);
            keyPadCanvas.SetActive(true);
        }

        public void CloseKeypad()
        {
            GameMgr.lockKey = false;
            if (itemBoxNet != null)
            {
                itemBoxNet.SetUsing(false);
            }
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
            if (itemBoxNet != null)
            {
                itemBoxNet.validCode = validCode;
            }
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
            minusNum = Random.Range(0, 999 - quizInt);
            
            switch (randomCase)
            {
                case 0:
                    resultNum = quizInt - plusNum;
                    quizText = resultNum.ToString() + " + " + plusNum.ToString() + "= 000";
                    break;
                case 1:
                    resultNum = quizInt + minusNum;
                    quizText = resultNum.ToString() + " - " + minusNum.ToString() + "= 000";
                    break;
            }

            if (itemBoxNet != null)
            {
                itemBoxNet.quizText = quizText;
            }
            quizInfo.text = quizText;
            //Debug.Log(quizInfo.text);
        }
    }
}
