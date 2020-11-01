using Unity.Entities;

//IComponent data component olarak kullanılacak structları sınırlamak için kullanılıyor
//Componentler sadece veri taşıyıcıları oldukları için struct olarak tanımlanıyorlar
//Monobeheviour aksine buradaki struct name file name ile aynı olmak zorunda değil

[GenerateAuthoringComponent]
public struct MoveSpeedData : IComponentData
{
   public float Value;
}
