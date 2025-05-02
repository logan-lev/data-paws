using UnityEngine;
using UnityEngine.InputSystem;

public class InputRebindManager : MonoBehaviour
{
    public InputActionAsset inputActions;

    const string rebindsKey = "rebinds";

    void Awake()
    {
        LoadBindings();
    }

    public void LoadBindings()
    {
        if (PlayerPrefs.HasKey(rebindsKey))
        {
            string savedRebinds = PlayerPrefs.GetString(rebindsKey);
            inputActions.LoadBindingOverridesFromJson(savedRebinds);
        }
    }

    public void SaveBindings()
    {
        string rebinds = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(rebindsKey, rebinds);
    }

    public void ResetBindings()
    {
        inputActions.RemoveAllBindingOverrides();
        PlayerPrefs.DeleteKey(rebindsKey);
    }
}
