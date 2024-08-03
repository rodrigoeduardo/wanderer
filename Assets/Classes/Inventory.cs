using System.Collections.Generic;

public class Inventory {
    public int caps;
    public List<Weapon> weapons;
    public List<Apparel> apparels;
    public List<Aid> aidItems;

    public Weapon equippedWeapon;
    public Apparel equippedApparel;

    public Inventory() {
        caps = 0;
        weapons = new List<Weapon>();
        apparels = new List<Apparel>();
        aidItems = new List<Aid>();
    }

    public void Prompt(Typewriter tw) {
        tw.SkipLine();
        tw.AddNewLine("INVENTORY:");
        tw.AddNewLine($"{caps} CAPS | {weapons.Count} WEAPONS | {apparels.Count} APPAREL | {aidItems.Count} AID");
        if (equippedWeapon != null) {
            tw.AddNewLine(equippedWeapon.name + "is EQUIPPED.");
        }
        if (equippedApparel != null) {
            tw.AddNewLine(equippedApparel.name + "is EQUIPPED.");
        }
    }
}