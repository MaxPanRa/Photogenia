using UnityEngine;
using UnityEngine.UI;

public class MinimapIcon : MonoBehaviour
{
    public MapWaypointIconsSO iconSO;

    private void Start()
    {
        GameObject prefab = Instantiate(iconSO.prefab);
        Transform icon = prefab.transform.Find("Icon");
        icon.GetComponent<Image>().sprite = iconSO.icon;
        prefab.transform.SetParent(this.transform);
        RectTransform rt = prefab.GetComponent<RectTransform>();
        rt.anchoredPosition.Set(0, 0);
        rt.anchorMin.Set(0, 1); 
        rt.anchorMax.Set(0, 1);
        rt.pivot.Set(0, 0);
        rt.localPosition = new Vector3(0, 1600, 0);
    }
}
