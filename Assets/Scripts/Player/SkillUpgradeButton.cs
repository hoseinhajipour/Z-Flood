using UnityEngine;

public class SkillUpgradeButton : MonoBehaviour
{
    public enum UpgradeType { Speed, Range, Health }
    public UpgradeType upgradeType;

    public void OnButtonClick()
    {
        SkillUpgradeUI ui = FindObjectOfType<SkillUpgradeUI>();
        if (ui != null)
        {
            switch (upgradeType)
            {
                case UpgradeType.Speed:
                    ui.OnSpeedUpgrade();
                    break;
                case UpgradeType.Range:
                    ui.OnRangeUpgrade();
                    break;
                case UpgradeType.Health:
                    ui.OnHealthUpgrade();
                    break;
            }
        }
    }
}
