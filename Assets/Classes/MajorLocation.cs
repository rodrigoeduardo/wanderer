[System.Serializable]
public class MajorLocationsObject {
    public MajorLocation[] majorLocations;
}

[System.Serializable]
public class MajorLocation {
    public string name;
    public string[] minorLocations;
}
