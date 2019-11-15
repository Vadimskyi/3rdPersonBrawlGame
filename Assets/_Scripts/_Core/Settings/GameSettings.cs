using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public bool ReceiveSettingsFromNetwork = true;
        public ArenaSettings ArenaSettings;
        public AnimationSetting AnimationSetting;
        public NetworkSettings NetworkSettings;
        public DefaultCharacterSettings DefaultCharacterSettings;
        public DefaultGuiSettings DefaultGuiSettings;
    }

    [Serializable]
    public struct ArenaSettings
    {
        public float CharacterFallThreshold;
        public float PushForce;
        public string LocalPlayerTag;
    }

    [Serializable]
    public struct AnimationSetting
    {
        public float KickAnimationTime;
        public float StumbleAnimationTime;
        public float DashAnimationTime;
        public float DashAnimationDistance;
    }

    [Serializable]
    public struct NetworkSettings
    {
        public float UpdateSendRate;
    }

    [Serializable]
    public struct DefaultCharacterSettings
    {
        public int Health;
        public int Damage;
        public float MoveSpeed;
        public float TurnSpeed;
        public float FireRate;
        public float KickRate;
        public float DashRate;
    }

    [Serializable]
    public struct DefaultGuiSettings
    {
        public HealthBarView HealthBarPrefab;
    }
}
