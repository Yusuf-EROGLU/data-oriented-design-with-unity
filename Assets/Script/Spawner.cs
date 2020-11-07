using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;


public class Spawner : MonoBehaviour
{
    [SerializeField] private Mesh unitMesh;
    [SerializeField] private Material unitMaterial;
    [SerializeField] private float3 unitTranslation;

    [SerializeField] private GameObject gameObjectPrefab;
    [SerializeField] private GameObject gameObjectMonoPrefab;
    

    private Entity entityPrefab;
    private World defaultWorld;
    private EntityManager entityManager;

    void Start()
    {
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;
        
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, settings);

        InstantiateGrid(400,  400,1f);
    }
    private void InstantiateGrid(int xSize, int ySize, float spacing)
    {
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                InstatiateEntity(new float3(i * spacing, j * spacing, 0));
            }
        }
    }
    private void InstantiateCubeEntity(int xSize, int ySize, int zSize, float spacing)
    {
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                for (int k = 0; k < zSize; k++)
                {
                    InstatiateEntity(new float3(i * spacing, j * spacing, k * spacing));
                }
            }
        }
    }
    private void InstatiateEntity(float3 position)
    {
        Entity myEntity = entityManager.Instantiate(entityPrefab);

        entityManager.SetComponentData(myEntity, new Translation
        {
            Value = position
        });
    }
    private void InstantiateMonoCube(int xSize, int ySize, int zSize, float spacing)
    {
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                for (int k = 0; k < zSize; k++)
                {
                    Instantiate(gameObjectMonoPrefab, new Vector3(i*spacing,j*spacing,k*spacing),quaternion.identity);
                }
            }
        }
    }
    private void MakeEntity()
    {
        // World.DefaultGameObjectInjectionWorld.EntityManager idafesindeki EntityManager static olup her world için bir adet bulunmaktadır.
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype archetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            //aşağıdaki üçü hypred renderer'ın entityleri çizmesi için gerekli olan şeyler
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld),
            typeof(MoveSpeedData));

        Entity myEntity = entityManager.CreateEntity(archetype);

        // Translation => from Unity.Transforms
        // float3 => from Unity.Mathematics
        entityManager.AddComponentData(myEntity, new Translation
        {
            //public field from struct
            Value = unitTranslation,
        });


        entityManager.AddSharedComponentData(myEntity, new RenderMesh
            {
                mesh = unitMesh,
                material = unitMaterial
            }
        );
    }
}