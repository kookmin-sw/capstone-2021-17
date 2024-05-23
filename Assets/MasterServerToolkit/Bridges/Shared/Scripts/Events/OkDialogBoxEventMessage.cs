﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MasterServerToolkit.Games
{
    public class OkDialogBoxEventMessage
    {
        public OkDialogBoxEventMessage() { }

        public OkDialogBoxEventMessage(string message)
        {
            Message = message;
            OkCallback = null;
        }

        public OkDialogBoxEventMessage(string message, UnityAction okCallback)
        {
            Message = message;
            OkCallback = okCallback;
        }

        public string Message { get; set; }
        public UnityAction OkCallback { get; set; }
    }
}
