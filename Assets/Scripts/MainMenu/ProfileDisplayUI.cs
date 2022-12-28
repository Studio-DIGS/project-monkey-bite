using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class ProfileDisplayUI : MonoBehaviour
{
    [SerializeField] private ProfileSaveDataEventChannelSO askEnterProfile;
    [SerializeField] private Button enterProfileButton;
    
    private ProfileSaveData profileData;

    private void OnEnable()
    {
        enterProfileButton.onClick.AddListener(EnterProfile);
    }

    private void OnDisable()
    {
        enterProfileButton.onClick.RemoveListener(EnterProfile);
    }

    public void Setup(ProfileSaveData profileSaveData)
    {
        this.profileData = profileSaveData;
    }

    public void EnterProfile()
    {
        if(profileData != null)
            askEnterProfile.RaiseEvent(profileData);
    }
}
