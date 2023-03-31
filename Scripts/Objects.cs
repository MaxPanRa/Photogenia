using SimpleJSON;

#region CAMERA
[System.Serializable]
public class CameraObject
{
    public string name;
    public CameraType type;
    public MemoryCard memory;
    public bool hasFlash;
    public bool autofocus;
    public float maxDistance = 50f;
}

public enum CameraType
{
    Handheld, Drone, Stationary, Telescope, Microscope
}
[System.Serializable]
public class MemoryCard
{
    public string name;
    public int size;
    public int used;
    public Photo[] photos;
}
[System.Serializable]
public class Photo
{
    public string name;
    public int score;
    public string genre;
    public string special;
    public JSONArray objects;
}
#endregion

#region OBJECTS
[System.Serializable]
public enum ObjectTheme
{
    None, Forest, City, Town, Village, Valley, Canyon, Desert, Mountain, Snow, Ocean, Water, Spring, Autumn, Summer, Winter, Sky, Underground, Animal, Flower, Portrait, Landscape
}
[System.Serializable]
public enum ObjectCategory
{
    Nature, Animal, Person, Building, Car, Object, Food
}
[System.Serializable]
public enum ObjectSpecial
{
    None, Quest, Taunt, Happy, Sad, Angry, InTrouble, OnFire, Frozen, Falling, Jumping, Flying, Surfing, Swimming
}
[System.Serializable]
public enum ObjectRarity
{
    None, Tutorial, Common, Uncommon, Rare, Epic, Legendary
}
[System.Serializable]
public enum ObjectFraming
{
    None, Center, Middle, Left, Right, LeftGeneral, RightGeneral, General
}

#endregion