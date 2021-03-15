using UnityEngine;
using UnityEngine.Serialization;

namespace Difficulties
{
    [CreateAssetMenu(menuName = "Difficulty")]
    public class Difficulty: ScriptableObject
    {
        [TextArea(14, 10)] public string Text;
    }
}