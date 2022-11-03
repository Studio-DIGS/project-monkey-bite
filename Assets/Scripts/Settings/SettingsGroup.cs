using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SettingsGroup : MonoBehaviour
{
    public Action OnGroupMarkedDirty;
    public abstract void LoadSavedSettings();
    
    public abstract void BeginSession();
    
    public abstract void CloseSession();

    public abstract void SaveChanges();
}
