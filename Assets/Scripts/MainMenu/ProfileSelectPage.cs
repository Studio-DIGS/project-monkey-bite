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
    [ColorHeader("Invoking - Ask Save Profile To File Channel", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private SaveProfileDataEventChannelSO askSaveNewProfile;
    
    [ColorHeader("Invoking - Get All Save Profiles Channel", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private SaveProfileDataArrFuncChannelSO getAllSaveProfile;
    
    [ColorHeader("Profile Select Page UI Dependencies", ColorHeaderColor.Dependencies)]
    [SerializeField] private GameObject profileSaveUI;
    [SerializeField] private MenuPage onCancelPage;
    [SerializeField] private Button createNewProfileButton;
    [SerializeField] private Transform profileLayoutTransform;

    struct ProfileDisplay
    {
        public SaveProfileData data;
        public ProfileDisplayUI ui;
    }
    
    private List<ProfileDisplay> profileDisplays = new List<ProfileDisplay>();
    private bool profilesSetup = false;
    
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
        var profileSaveList = getAllSaveProfile.CallFunc().ToList();
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

    private ProfileDisplay CreateProfileUI(SaveProfileData saveProfile)
    {
        var profileUI = Instantiate(profileSaveUI, profileLayoutTransform);
        var component = profileUI.GetComponent<ProfileDisplayUI>();
        component.Setup(saveProfile);
        profileUI.GetComponentInChildren<TextMeshProUGUI>().text = $"Profile {saveProfile.metaData.profileID} | Deaths: {saveProfile.statsData.deathCounter}";
        var display = new ProfileDisplay { data = saveProfile, ui =  component};
        profileDisplays.Add(display);
        return display;
    }

    private void OnCreateNewProfile()
    {
        var newProfile = new SaveProfileData();
        newProfile.metaData.profileID = "" + (profileDisplays.Count + 1);
        askSaveNewProfile.RaiseEvent(newProfile);
        
        CreateProfileUI(newProfile);
        EventSystem.current.SetSelectedGameObject(profileDisplays[profileDisplays.Count - 1].ui.GetComponentInChildren<Button>().gameObject);
    }
}
