using System.Collections.Generic;

[System.Serializable]
public class MajorLocationsObject {
    public List<MajorLocation> majorLocations;
}

[System.Serializable]
public class MajorLocation {
    public string name;
    public string description;
    public double probability;
    public List<MinorLocation> minorLocations;
}

[System.Serializable]
public class MinorLocation {
    public string name;
    public string description;
    public double probability;
    public List<Event> events;
}

[System.Serializable]
public class Event {
    public string name;
    public string description;
    public double probability;
}
