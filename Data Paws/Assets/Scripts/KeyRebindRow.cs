using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeyRebindRow : MonoBehaviour
{
    public InputActionAsset inputActions;
    public string actionName;
    public string bindingId;  // Optional: leave empty if action has 1 binding

    public TMP_InputField keyInputField;
    public Button rebindButton;

    private InputAction action;

    void Start()
    {
        action = inputActions.FindAction(actionName);
        if (action == null)
        {
            Debug.LogError($"Action '{actionName}' not found");
            return;
        }

        UpdateKeyDisplay();
        rebindButton.onClick.AddListener(StartRebind);
    }

    void StartRebind()
{
    keyInputField.text = "Press key...";

    int bindingIndex = 0;

    if (!string.IsNullOrEmpty(bindingId))
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (action.bindings[i].id.ToString() == bindingId)
            {
                bindingIndex = i;
                break;
            }
        }
    }

    // 🔴 DISABLE before rebinding
    action.Disable();

    action.PerformInteractiveRebinding(bindingIndex)
        .WithControlsExcluding("Mouse")
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(op =>
        {
            op.Dispose();
            UpdateKeyDisplay();

            // ✅ Save the binding
            FindAnyObjectByType<InputRebindManager>().SaveBindings();

            // ✅ RE-ENABLE after rebinding
            action.Enable();
        })
        .Start();
}


    void UpdateKeyDisplay()
    {
        int bindingIndex = 0;

        if (!string.IsNullOrEmpty(bindingId))
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action.bindings[i].id.ToString() == bindingId)
                {
                    bindingIndex = i;
                    break;
                }
            }
        }
        keyInputField.text = action.GetBindingDisplayString(bindingIndex);
    }
}
