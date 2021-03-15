using UnityEngine;

namespace Difficulties
{
    [CreateAssetMenu(menuName = "Difficulty")]
    public class Difficulty: ScriptableObject
    {
        [TextArea(14, 10)] [SerializeField] private string _text;
    }
}