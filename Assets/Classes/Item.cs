public enum ItemType {
    WEAPON,
    APPAREL,
    AID
}

public class Item {
    public string name;
    public string description;
    protected ItemType type;
}

public class Weapon : Item {
    public int minDamage;
    public int maxDamage;
    public int durability;

    public Weapon() {
        type = ItemType.WEAPON;
    }
}

public class Apparel : Item {
    public SPECIAL buffs;

    public Apparel() {
        type = ItemType.APPAREL;
    }
}

public class Aid : Item {
    public int healPoints;
    public string target; // HP or Radiation

    public Aid() {
        type = ItemType.AID;
    }
}