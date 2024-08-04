using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Utils {
    public static string[] keywords = {
        "inventory", "equip", "use", "goto"
    };

    public static string ConvertMinorLocationsToString(List<MinorLocation> minorLocations) {
        if (minorLocations == null || minorLocations.Count == 0)
        {
            return string.Empty;
        }

        if (minorLocations.Count == 1)
        {
            return $"a {minorLocations[0].name}";
        }

        StringBuilder result = new();
        for (int i = 0; i < minorLocations.Count; i++)
        {
            if (i == minorLocations.Count - 1)
            {
                result.Append($"and a {minorLocations[i].name}");
            }
            else
            {
                result.Append($"a {minorLocations[i].name}");
                if (i < minorLocations.Count - 2)
                {
                    result.Append(", ");
                }
                else
                {
                    result.Append(" ");
                }
            }
        }

        return result.ToString();
    }
}