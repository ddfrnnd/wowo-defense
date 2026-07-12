// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Player Interface.
    /// </summary>
    public class CanvasSpawner : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("Settings")]
        
        [Tooltip("Canvas prefab spawned at start. Displays the player's user interface.")]
        [SerializeField]
        private GameObject canvasPrefab;

        #endregion

        #region UNITY FUNCTIONS

        /// <summary>
        /// Awake.
        /// </summary>
        private void Awake()
        {
            //Spawn Interface.
            GameObject spawnedCanvas = Instantiate(canvasPrefab);

            // Disable only the tutorial/watermark text elements in the spawned interface
            if (spawnedCanvas != null)
            {
                TextTutorial tutorial = spawnedCanvas.GetComponentInChildren<TextTutorial>(true);
                if (tutorial != null) tutorial.gameObject.SetActive(false);

                TextTimescale timescale = spawnedCanvas.GetComponentInChildren<TextTimescale>(true);
                if (timescale != null) timescale.gameObject.SetActive(false);

                TextMouseLock mouselock = spawnedCanvas.GetComponentInChildren<TextMouseLock>(true);
                if (mouselock != null) mouselock.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}