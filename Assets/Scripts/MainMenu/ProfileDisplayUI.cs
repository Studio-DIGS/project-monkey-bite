using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class ProfileDisplayUI : MonoBehaviour
{
    [SerializeField] private SaveProfileDataEventChannelSO askEnterSaveProfile;
    [SerializeField] private Button enterProfileButton;
    
    private SaveProfileData saveProfileData;

    private void OnEnable()
    {
        enterProfileButton.onClick.AddListener(EnterProfile);
    }

    private void OnDisable()
    {
        enterProfileButton.onClick.RemoveListener(EnterProfile);
    }

    public void Setup(SaveProfileData saveProfileData)
    {
        this.saveProfileData = saveProfileData;
    }

    public void EnterProfile()
    {
        if(saveProfileData != null)
            askEnterSaveProfile.RaiseEvent(saveProfileData);
    }
}
