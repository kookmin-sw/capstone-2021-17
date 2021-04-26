using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public interface IWeapon
    {
        string Id { get; }
        Transform ShellSpawnPoint { get; }
        BaseShell ShellPrefab { get; }
        bool FireIsAllowed { get; }
        WeaponType WeaponType { get; }
        void ReduceAmmo();
    }
}