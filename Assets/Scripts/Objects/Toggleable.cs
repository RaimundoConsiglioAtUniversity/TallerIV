using UnityEngine;

public class Toggleable : InteractiveObject
{
    public GameObject onState;
    public GameObject offState;

    public bool isActive;
    public void DefaultOn(bool isOn) => isActive = isOn;

    public void OnEnable() => InteractableToggle.ToggleLinkedObjects += Toggle;
    public void OnDisable() => InteractableToggle.ToggleLinkedObjects -= Toggle;

    public void Toggle(int ID)
    {
        if (ID == this.ID)
            InteractedWith();
    }

    public override void InteractedWith()
    {
        isActive = !isActive;
        SwitchStates();
    }

    public override void Awake() => SwitchStates();

    public void SwitchStates()
    {
        onState.SetActive(isActive);
        offState.SetActive(!isActive);
    }
}
