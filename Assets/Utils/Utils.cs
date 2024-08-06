using System.Collections.Generic;
using System.Text;

public class Utils {
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

    public static string ConvertMajorLocationsToString(List<MajorLocation> majorLocations) {
        if (majorLocations == null || majorLocations.Count == 0)
        {
            return string.Empty;
        }

        if (majorLocations.Count == 1)
        {
            return $"a {majorLocations[0].name}";
        }

        StringBuilder result = new();
        for (int i = 0; i < majorLocations.Count; i++)
        {
            if (i == majorLocations.Count - 1)
            {
                result.Append($"and a {majorLocations[i].name}");
            }
            else
            {
                result.Append($"a {majorLocations[i].name}");
                if (i < majorLocations.Count - 2)
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