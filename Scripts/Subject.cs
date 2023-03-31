using UnityEngine;
public class Subject : MonoBehaviour
{
    [Header("Object Information")]
    public string objectName;
    public ObjectTheme objectTheme;
    public ObjectCategory objectCategory;
    public ObjectRarity objectRating;
    public int points = 10;

    [Header("DEBUG ONLY")]
    public ObjectSpecial objectSpecial = ObjectSpecial.None;
    public ObjectFraming objectFraming = ObjectFraming.None;
    public int totalPoints = 0;
    public float distance = 0f;

    private void Start()
    {
        name = transform.gameObject.name;
    }
}
