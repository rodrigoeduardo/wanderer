public class Player {
    public SPECIAL special;
    public int level;
    public int xp;
    public int hp;
    public int maxHp;
    public Inventory inventory;

    public Player()
    {
        special = new SPECIAL();
        level = 1;
        xp = 0;
        inventory = new Inventory();
    }
}