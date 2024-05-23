﻿using MasterServerToolkit.MasterServer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MasterServerToolkit.Games
{
    public class ClientAuthStatusComponent : MonoBehaviour
    {
        #region INSPECTOR
        [Header("Settings"), SerializeField]
        private bool changeStatusColor = true;
        
        [Header("Status Colors"), SerializeField]
        private Color unauthorizedStatusColor = Color.red;
        [SerializeField]
        private Color authorizedStatusColor = Color.green;
        [SerializeField]
        private Color guestStatusColor = Color.yellow;

        [Header("Components"), SerializeField]
        private Image statusImage;
        [SerializeField]
        private TextMeshProUGUI statusText;
        #endregion

        protected virtual void Update()
        {
            if (Mst.Client.Auth.IsSignedIn)
            {
                if (Mst.Client.Auth.AccountInfo != null)
                {
                    if (!Mst.Client.Auth.AccountInfo.IsGuest)
                    {
                        RepaintStatus(Mst.Client.Auth.AccountInfo.Username, authorizedStatusColor);
                    }
                    else
                    {
                        RepaintStatus(Mst.Client.Auth.AccountInfo.Username, guestStatusColor);
                    }
                }
                else
                {
                    RepaintStatus("Unauthorized", unauthorizedStatusColor);
                }
            }
            else
            {
                RepaintStatus("Unauthorized", unauthorizedStatusColor);
            }
        }

        private void RepaintStatus(string statusMsg, Color statusColor)
        {
            if (changeStatusColor && statusImage != null)
                statusImage.color = statusColor;

            if (changeStatusColor && statusText != null)
                statusText.color = statusColor;

            if (statusText != null)
                statusText.text = statusMsg;
        }
    }
}