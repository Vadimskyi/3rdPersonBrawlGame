using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Vadimskyi;
using Vadimskyi.Game;
using Vadimskyi.Utils;

public class UiCanvasController : MonoBehaviour
{
    [SerializeField]
    private Button _playerRespawn;

    [SerializeField]
    private HealthBarView _healthBar;

    private void Start()
    {
        _playerRespawn.OnClickAsObservable().Subscribe(_ => { PlayerController.Instance.RespawnPlayer(); }).AddTo(this);
        PlayerController.Instance.CurrentPlayer.View.Movement.OnMove += OnPlayerMove;
        PlayerController.Instance.CurrentPlayer.View.Health.OnDamageTaken += HealthOnDamageTaken;
        _healthBar.Name.text = UserData.Instance.User.Name;
    }

    private void HealthOnDamageTaken(int dmg)
    {
        _healthBar.Health.fillAmount = (float) PlayerController.Instance.CurrentPlayer.View.Health.CurrentHealth /
                                       PlayerController.Instance.CurrentPlayer.View.Health.MaxHealth;
    }

    private void OnPlayerMove(Transform target)
    {
        _healthBar.transform.localPosition = CanvasHelper.Instance.WorldToCanvasPoint(target.position);
        _healthBar.transform.SetLocalPosition(null, 250);
    }
}
