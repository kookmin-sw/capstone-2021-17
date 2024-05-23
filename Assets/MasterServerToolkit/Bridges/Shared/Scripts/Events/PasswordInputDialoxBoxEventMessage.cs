﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MasterServerToolkit.Games
{
    public class PasswordInputDialoxBoxEventMessage : MonoBehaviour
    {
        public PasswordInputDialoxBoxEventMessage() { }

        public PasswordInputDialoxBoxEventMessage(string message)
        {
            Message = message;
            OkCallback = null;
        }

        public PasswordInputDialoxBoxEventMessage(string message, UnityAction submitCallback)
        {
            Message = message;
            OkCallback = submitCallback;
        }

        public string Message { get; set; }
        public UnityAction OkCallback { get; set; }
    }
}