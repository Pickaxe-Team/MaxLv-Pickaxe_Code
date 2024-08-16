public enum SoundType
{
    BGM,
    SFX
}

public enum BGM
{
    Title,
    Forest,
    Dungeon,
    Boss
}
public enum SFX
{
    // Player
    FootstepGrass,
    FootstepSand,
    Swing,
    Dash,
    LevelUp,
    Heal,

    // Hit
    PlayerHit,
    EnemyHit,
    RockHit,
    WoodHit,

    // UI
    Click,
    Alert,
    Upgrade,

    // Boss
    FloatingBullet,
    GolemPunch,
    GolemRoar,
    GolemStomp,
    GolemWalking,
    GolmeHeal,
    Laser,
    Lightning,
    MeteorImpact,
    MeteorShoot,
    GolemCharged,

    // Item
    Pickup
}

public enum ItemID
{
    // Equipment: 0 ~ 99

    // Consumable: 100 ~ 199
    Fruit = 100,

    // Upgrade Scroll: 200 ~ 299
    CopperScroll = 200,
    SilverScroll,
    GoldScroll,
    DiamondScroll,
    SapphireScroll,
    AmethystScroll,

    // Ore: 300 ~ 399
    Copper = 300,
    Silver,
    Gold,
    Diamond,
    Sapphire,
    Amethyst,

    // Resource: 400 ~ 499
    Wood = 400,
    Stone,
}

public enum ItemType
{
    Consumable,
    Equipment,
    Resource
}

public enum EntityID
{
    // Interactable: 0 ~ 299
    Tree,
    Rock,
    CopperOre,
    SilverOre,
    GoldOre,
    DiamondOre,
    SapphireOre,
    AmethystOre,

    // Enemy: 300 ~ 999
    BlueSlime = 300,
    Orbinaut,
    LavaOrbinaut,
    GreenSlime,
    BlueOrbinaut,
    PinkSlime,

    // Boss: 1000 ~
    Golem = 1000,
    Golem2,
    Golem3
}

public enum InteractableObjectType
{
    Tree,
    Rock,
    Ore
}

public enum Grade
{
    Normal,
    Rare,
    Epic
}

public enum HPColor
{
    Red,
    Orange
}

public enum PickaxeID
{
    CopperPickaxeLV1 = 1,
    CopperPickaxeLV2,
    CopperPickaxeLV3,
    SilverPickaxeLV1,
    SilverPickaxeLV2,
    SilverPickaxeLV3,
    GoldPickaxeLV1,
    GoldPickaxeLV2,
    GoldPickaxeLV3,
    DiamondPickaxeLV1,
    DiamondPickaxeLV2,
    DiamondPickaxeLV3,
    SapphirePickaxeLV1,
    SapphirePickaxeLV2,
    SapphirePickaxeLV3,
    AmethystPickaxeLV1,
    AmethystPickaxeLV2,
    AmethystPickaxeLV3,
    End
}

