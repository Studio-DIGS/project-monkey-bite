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

    struct ProfileDisplay
    {
        public ProfileSaveData data;
        public ProfileDisplayUI ui;
    }

    private List<ProfileDisplay> profileDisplays = new List<ProfileDisplay>();
    
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
        var profileSaveList = getAllProfileSaves.CallFunc().ToList();
        foreach (var profileSave in profileSaveList)
        {
            var display = CreateProfileUI(profileSave);
        }

        if (profileDisplays.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(profileDisplays[0].ui.GetComponentInChildren<Button>().gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(createNewProfileButton.gameObject);
        }
    }

    private ProfileDisplay CreateProfileUI(ProfileSaveData profileSave)
    {
        var profileUI = Instantiate(profileSaveUI, profileLayoutTransform);
        var component = profileUI.GetComponent<ProfileDisplayUI>();
        component.Setup(profileSave);
        profileUI.GetComponentInChildren<TextMeshProUGUI>().text = $"Profile {profileSave.metaData.profileID} | Deaths: {profileSave.statsData.deathCounter}";
        var display = new ProfileDisplay { data = profileSave, ui =  component};
        profileDisplays.Add(display);
        return display;
    }

    private void OnCreateNewProfile()
    {
        var newProfile = new ProfileSaveData();
        newProfile.metaData.profileID = "" + (profileDisplays.Count + 1);
        askSaveNewProfile.RaiseEvent(newProfile);
        
        CreateProfileUI(newProfile);
        EventSystem.current.SetSelectedGameObject(profileDisplays[profileDisplays.Count - 1].ui.GetComponentInChildren<Button>().gameObject);
    }
}
