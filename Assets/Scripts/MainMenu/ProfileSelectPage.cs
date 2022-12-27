using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProfileSelectPage : MenuPage
{
    [SerializeField] private MenuPage onCancelPage;
    [SerializeField] private Button createNewProfileButton;
    [SerializeField] private ProfileSaveDataArrFuncChannelSO getAllProfileSaves;
    [SerializeField] private ProfileSaveDataEventChannelSO askSaveNewProfile;
    [SerializeField] private GameObject profileSaveUI;
    [SerializeField] private Transform profileLayoutTransform;

    private bool profilesSetup = false;

    private List<ProfileSaveData> profileSaveList;
    
    public override void ShowPage()
    {
        base.ShowPage();
        if (!profilesSetup)
        {
            SetupProfiles();
        }
        createNewProfileButton.onClick.AddListener(OnCreateNewProfile);
    }

    public override void HidePage()
    {
        base.HidePage();
        createNewProfileButton.onClick.RemoveListener(OnCreateNewProfile);
    }

    public void OnCancel()
    {
        askChangeMenuPage.RaiseEvent(onCancelPage);
    }

    private void SetupProfiles()
    {
        profilesSetup = true;
        profileSaveList = getAllProfileSaves.CallFunc().ToList();
        foreach (var profileSave in profileSaveList)
        {
            var profileUI = Instantiate(profileSaveUI, profileLayoutTransform);
            profileUI.GetComponentInChildren<TextMeshProUGUI>().text = profileSave.profileID + " | Deaths: " + profileSave.statsData.deathCounter;
        }
    }

    private void OnCreateNewProfile()
    {
        //askSaveNewProfile.RaiseEvent(profileSaveList.Count - 1);
    }
}
