// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEngine;
using System.Collections.Generic;

namespace InfimaGames.LowPolyShooterPack
{
    public class Inventory : InventoryBehaviour
    {
        #region FIELDS
        
        /// <summary>
        /// Array of all weapons. These are gotten in the order that they are parented to this object.
        /// </summary>
        private WeaponBehaviour[] weapons;
        
        /// <summary>
        /// Currently equipped WeaponBehaviour.
        /// </summary>
        private WeaponBehaviour equipped;
        /// <summary>
        /// Currently equipped index.
        /// </summary>
        private int equippedIndex = -1;

        /// <summary>
        /// List of unlocked weapon indices. Index 0 (Assault Rifle) and 1 (Pistol) are unlocked by default.
        /// </summary>
        private List<int> unlockedIndices = new List<int>() { 0, 1 };

        #endregion
        
        #region METHODS
        
        public override void Init(int equippedAtStart = 0)
        {
            //Cache all weapons. Beware that weapons need to be parented to the object this component is on!
            weapons = GetComponentsInChildren<WeaponBehaviour>(true);
            
            //Disable all weapons. This makes it easier for us to only activate the one we need.
            foreach (WeaponBehaviour weapon in weapons)
                weapon.gameObject.SetActive(false);

            //Equip.
            Equip(equippedAtStart);
        }

        public override WeaponBehaviour Equip(int index)
        {
            //If we have no weapons, we can't really equip anything.
            if (weapons == null)
                return equipped;
            
            //The index needs to be within the array's bounds.
            if (index > weapons.Length - 1)
                return equipped;

            // Make sure the weapon is unlocked!
            if (!unlockedIndices.Contains(index))
                return equipped;

            //No point in allowing equipping the already-equipped weapon.
            if (equippedIndex == index)
                return equipped;
            
            //Disable the currently equipped weapon, if we have one.
            if (equipped != null)
                equipped.gameObject.SetActive(false);

            //Update index.
            equippedIndex = index;
            //Update equipped.
            equipped = weapons[equippedIndex];
            //Activate the newly-equipped weapon.
            equipped.gameObject.SetActive(true);

            //Return.
            return equipped;
        }

        public override void UnlockWeapon(int index)
        {
            if (!unlockedIndices.Contains(index))
            {
                unlockedIndices.Add(index);
                unlockedIndices.Sort();
                // Auto switch to the newly unlocked weapon
                Equip(index);
            }
        }
        
        #endregion

        #region Getters

        public override int GetLastIndex()
        {
            if (weapons == null || weapons.Length == 0) return -1;
            
            // Find last unlocked index with wrap around.
            int newIndex = equippedIndex;
            do
            {
                newIndex = newIndex - 1;
                if (newIndex < 0)
                    newIndex = weapons.Length - 1;
            } while (!unlockedIndices.Contains(newIndex) && newIndex != equippedIndex);

            return newIndex;
        }

        public override int GetNextIndex()
        {
            if (weapons == null || weapons.Length == 0) return -1;
            
            // Find next unlocked index with wrap around.
            int newIndex = equippedIndex;
            do
            {
                newIndex = newIndex + 1;
                if (newIndex > weapons.Length - 1)
                    newIndex = 0;
            } while (!unlockedIndices.Contains(newIndex) && newIndex != equippedIndex);

            return newIndex;
        }

        public override WeaponBehaviour GetEquipped() => equipped;
        public override int GetEquippedIndex() => equippedIndex;

        #endregion
    }
}