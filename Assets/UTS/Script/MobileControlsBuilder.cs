using UnityEngine;

/// <summary>
/// DEPRECATED: Mobile controls are now set up directly in the GameplayCanvas prefab.
/// This script is kept only for backward compatibility (existing references).
/// All mobile UI (analog stick, fire stick, buttons) should be created and positioned
/// in the prefab via Unity Editor, NOT via script.
/// </summary>
public class MobileControlsBuilder : MonoBehaviour
{
    // No longer builds UI at runtime.
    // All mobile controls are set up in the GameplayCanvas prefab.
    // This ensures WYSIWYG: what you see in Editor = what you get in Play mode.
}
