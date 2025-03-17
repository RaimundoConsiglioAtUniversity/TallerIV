using UnityEngine;
using SuperTiled2Unity;

public class SetTiledProperties : MonoBehaviour
{
    public SuperCustomProperties[] customProperties;
    public InteractiveObject[] interactives = {};
    public GameObject objectPrefab;

    public static SetTiledProperties Instance => instance;
    private static SetTiledProperties instance;

    void Awake()
    {
        if (instance != null)
            Destroy(this);

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Initialize()
    {
        customProperties = FindObjectsOfType<SuperCustomProperties>();
        int objectIdx = 0;
        int buttonIdx = 0;
        int toggleIdx = 0;
        int tpIdx = 0;
        int elecIdx = 0;
        int plantIdx = 0;

        foreach (var customProperty in customProperties)
        {
            objectIdx++;
            bool startAwake    = customProperty.TryGetCustomProperty("IsActive", out CustomProperty StartAwake);
            bool canToggleWall = customProperty.TryGetCustomProperty("ToggleableWall", out CustomProperty ToggleableWall);
            bool hasWallNumber = customProperty.TryGetCustomProperty("WallNumber", out CustomProperty WallNumber);
            bool isButton      = customProperty.TryGetCustomProperty("ButtonWall", out CustomProperty ButtonWall);
            bool isElectric    = customProperty.TryGetCustomProperty("Electric", out CustomProperty Electric);
            bool canTpThrough  = customProperty.TryGetCustomProperty("CanTeleportThrough", out CustomProperty CanTpThrough);
            bool isGrowable    = customProperty.TryGetCustomProperty("GrowPlant", out CustomProperty GrowPlant);
            
            GameObject myObject = customProperty.gameObject;
            SpriteRenderer sprite = myObject.GetComponentInChildren<SpriteRenderer>();

            Rigidbody2D rb = myObject.GetComponentInChildren<Rigidbody2D>();
            Collider2D collider = myObject.GetComponentInChildren<Collider2D>();


            int wallID = hasWallNumber ? WallNumber.GetValueAsInt() : 0;

            if (isButton)
            {
                buttonIdx++;
                myObject.name = $"ButtonObject_{buttonIdx}";

                myObject.EnsureComponent(ref collider);
                myObject.EnsureComponent(ref rb);

                rb.isKinematic = true;

                PlayerButton button = myObject.GetOrAddComponent<PlayerButton>();

                button.objectID = wallID;
            }

            bool isActive = true;

            if (startAwake)
            {
                isActive = StartAwake.GetValueAsBool();
            }

            if (canToggleWall)
            {
                toggleIdx++;
                myObject.name = $"ToggleableObject_{toggleIdx}";

                myObject.EnsureComponent(ref collider);
                myObject.EnsureComponent(ref rb);
                
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;

                Toggleable wall = myObject.GetOrAddComponent<Toggleable>();

                wall.ID = wallID;
                wall.isActive = isActive;
            }

            if (canTpThrough)
            {
                tpIdx++;
                myObject.name = $"PermeableObject_{tpIdx}";

                myObject.EnsureComponent(ref collider);
                myObject.EnsureComponent(ref rb);
                
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;

                Permeable permeable = myObject.GetOrAddComponent<Permeable>();
            }

            if (isElectric)
            {
                elecIdx++;
                myObject.name = $"ElecObject_{elecIdx}";

                myObject.EnsureComponent(ref collider);
                myObject.EnsureComponent(ref rb);
                
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;

                Electric electric = myObject.GetOrAddComponent<Electric>();

                electric.objectID = wallID;
            }

            if (isGrowable)
            {
                plantIdx++;
                myObject.name = $"PlantObject_{plantIdx}";

                myObject.EnsureComponent(ref collider);
                myObject.EnsureComponent(ref rb);
                
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;

                GrowPlant plant = myObject.GetOrAddComponent<GrowPlant>();

                plant.triggerArea = myObject.GetOrAddComponent<CircleCollider2D>();
                plant.objectID = wallID;
            }
        }
        
        interactives = (InteractiveObject[])interactives.AddUniqueItems(FindObjectsOfType<InteractiveObject>());

        foreach (InteractiveObject interactive in interactives)
        {

        }
    }
}
