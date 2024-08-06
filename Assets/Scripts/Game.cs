using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public TextAsset locationsJson;
    public List<MajorLocation> majorLocations;

    public Typewriter typewriter;
    public TMP_InputField inputField;
    public HealthScript health;
    public LevelScript level;
    public UserInputSignalScript userInputSignal;

    bool hasStarted = false;

    public Player player = new();

    public List<MajorLocation> possibleMajorLocations = new();
    public List<MinorLocation> possibleMinorLocations = new();
    MajorLocation currentMajorLocation;
    MinorLocation currentMinorLocation;

    // Start is called before the first frame update
    void Start()
    {
        majorLocations = JsonUtility.FromJson<MajorLocationsObject>(locationsJson.text).majorLocations;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !hasStarted && Time.time > 2f) {
            // FOR TESTING PURPOSES ONLY
            player.special.strength = 1;
            player.special.perception = 1;
            player.special.endurance = 1;
            player.special.charisma = 1;
            player.special.intelligence = 1;
            player.special.agility = 1;
            player.special.luck = 1;
            hasStarted = true;
            StartAdventure();
            return;

            hasStarted = true;
            StartGame();
        }
    }

    void StartGame() {
        typewriter.SkipLine();
        typewriter.AddNewLine("Choose your SPECIAL:");
        typewriter.AddNewLine("You have 40 points to distribute as you want. The maximum for each stat is 10.");
        typewriter.SkipLine();

        AskUserStatValue(1);
    }

    void GetUserInput(Action<string> response) {
        inputField.ActivateInputField();
        userInputSignal.Activate();
        inputField.onEndEdit.AddListener(val => {
            if (Input.GetKeyDown(KeyCode.Return) && val != "") {
                response(val);
                inputField.text = "";
            }
        });
    }
    
    void EndUserInput() {
        inputField.onEndEdit.RemoveAllListeners();
        userInputSignal.Deactivate();
    }

    void RespondUserCommand(string command) {
        void PromptMajorLocation() {
            typewriter.AddNewLine(currentMajorLocation.description);

            if (possibleMinorLocations.Count == 0) {
                int randomMinorLocationsQuantity = (int)UnityEngine.Random.Range(1f, currentMajorLocation.minorLocations.Count);
                possibleMinorLocations = GetRandomMinorLocations(randomMinorLocationsQuantity);
            }

            typewriter.AddLine("You look around and see");
            typewriter.AddLine(Utils.ConvertMinorLocationsToString(possibleMinorLocations) + ".");
        }

        string normalizedInput = command.ToLower();
        string[] words = normalizedInput.Split(' ');

        foreach (string word in words) {
            switch (word)
            {
                case "inventory":
                    player.inventory.Prompt(typewriter);
                    return;
                case "leave": {
                    if (currentMinorLocation != null) {
                        typewriter.AddNewLine($"You step outside from {currentMinorLocation.name}.");
                        currentMinorLocation = null;

                        PromptMajorLocation();
                        return;
                    } else if (currentMajorLocation != null) {
                        typewriter.AddNewLine($"You leave {currentMajorLocation.name} now a more experienced wanderer. Ready for new challenges.");
                        currentMajorLocation = null;

                        int randomMajorLocationsQuantity = (int)UnityEngine.Random.Range(1f, majorLocations.Count);
                        possibleMajorLocations = GetRandomMajorLocations(randomMajorLocationsQuantity);

                        typewriter.AddNewLine("You see on the horizon:");
                        typewriter.AddLine(Utils.ConvertMajorLocationsToString(possibleMajorLocations) + ".");
                        return;
                    }

                    typewriter.AddNewLine("You wander amidst the desolation and ruins, but you find yourself in no particular place yet.");
                    return;
                }
                default:
                    break;
            }
        }

        void CommandNotRecognized() {
            typewriter.AddNewLine("[ERROR] Command not recognized.");
        }

        if (currentMajorLocation == null) {
            MajorLocation location = possibleMajorLocations.FirstOrDefault(location => normalizedInput.Contains(location.name.ToLower()));
            if (location == null) {
                CommandNotRecognized();
                return;
            }
            currentMajorLocation = location;

            PromptMajorLocation();

            possibleMajorLocations.Clear();
            return;
        } else if (currentMinorLocation == null) {
            MinorLocation location = currentMajorLocation.minorLocations.FirstOrDefault(location => normalizedInput.Contains(location.name.ToLower()));
            if (location == null) {
                CommandNotRecognized();
                return;
            }
            currentMinorLocation = location;
            typewriter.AddNewLine(currentMinorLocation.description);
            return;
        }

        CommandNotRecognized();
    }

    void AskUserStatValue(int statIndex) {
        if (statIndex == 8) {
            StartAdventure();
            return;
        }

        string stat = "";

        switch (statIndex) {
            case 1:
                stat = "STRENGTH";
                break;
            case 2:
                stat = "PERCEPTION";
                break;
            case 3:
                stat = "ENDURANCE";
                break;
            case 4:
                stat = "CHARISMA";
                break;
            case 5:
                stat = "INTELLIGENCE";
                break;
            case 6:
                stat = "AGILITY";
                break;
            case 7:
                stat = "LUCK";
                break;
            default:
                return;
        }

        typewriter.AddNewLine(stat + ":");
        GetUserInput((response) => {
            int input;
            try {
                input = int.Parse(response);
            } catch (FormatException) {
                typewriter.AddNewLine("[ERROR] Please enter an integer.");
                return;
            }

            if (input > 10 || input < 1) {
                typewriter.AddNewLine("[ERROR] Please enter an integer between 1 and 10.");
                return;
            }

            if (input + player.special.Sum() > 40) {
                typewriter.AddNewLine("[ERROR] Please respect the limit of 40 points.");
                return;
            }

            switch (statIndex) {
                case 1:
                    player.special.strength = input;
                    break;
                case 2:
                    player.special.perception = input;
                    break;
                case 3:
                    player.special.endurance = input;
                    break;
                case 4:
                    player.special.charisma = input;
                    break;
                case 5:
                    player.special.intelligence = input;
                    break;
                case 6:
                    player.special.agility = input;
                    break;
                case 7:
                    player.special.luck = input;
                    break;
                default:
                    return;
            }

            typewriter.AddNewLine("Okay. " + input + " for " + stat + ".");
            EndUserInput();
            AskUserStatValue(++statIndex);
        });
    }

    void StartAdventure() {
        health.EnableHP();
        level.EnableLevel();

        possibleMajorLocations = GetRandomMajorLocations(2);

        typewriter.SkipLine();
        typewriter.AddNewLine("- Okay. Let's do this.");
        typewriter.AddNewLine("<i>The vault door opens slowly.</i>");
        typewriter.AddNewLine("You can see the sun shining for the first time before your eyes.");
        typewriter.AddLine($"You see on the horizon: a {possibleMajorLocations[0].name} and a {possibleMajorLocations[1].name}.");
        typewriter.AddNewLine("Where do you wanna go?");

        GetUserInput((response) => {
            RespondUserCommand(response);
        });
    }

    public List<MajorLocation> GetRandomMajorLocations(int count)
    {
        List<MajorLocation> selectedLocations = new();
        
        List<MajorLocation> availableLocations = new(majorLocations);

        for (int i = 0; i < count; i++)
        {
            double totalProbability = availableLocations.Sum(location => location.probability);
            double randomValue = UnityEngine.Random.Range(0f, 1f) * totalProbability;
            double cumulativeProbability = 0.0;

            for (int j = 0; j < availableLocations.Count; j++)
            {
                cumulativeProbability += availableLocations[j].probability;
                if (randomValue <= cumulativeProbability)
                {
                    selectedLocations.Add(availableLocations[j]);
                    availableLocations.RemoveAt(j);
                    break;
                }
            }
        }

        return selectedLocations;
    }

    public List<MinorLocation> GetRandomMinorLocations(int count)
    {
        List<MinorLocation> selectedLocations = new();
        
        List<MinorLocation> availableLocations = new(currentMajorLocation.minorLocations);

        for (int i = 0; i < count; i++)
        {
            double totalProbability = availableLocations.Sum(location => location.probability);
            double randomValue = UnityEngine.Random.Range(0f, 1f) * totalProbability;
            double cumulativeProbability = 0.0;

            for (int j = 0; j < availableLocations.Count; j++)
            {
                cumulativeProbability += availableLocations[j].probability;
                if (randomValue <= cumulativeProbability)
                {
                    selectedLocations.Add(availableLocations[j]);
                    availableLocations.RemoveAt(j);
                    break;
                }
            }
        }

        return selectedLocations;
    }
}
