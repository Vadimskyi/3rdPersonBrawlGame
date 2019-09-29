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
        _playerRespawn.OnClickAsObservable().Subscribe(_ => { PlayersController.Instance.RespawnPlayer(); }).AddTo(this);
        _healthBar.Name.text = UserData.Instance.User.Name;
    }

    public void PlayerHealthOnDamageTaken(int dmg)
    {
        _healthBar.Health.fillAmount = (float) PlayersController.Instance.CurrentPlayer.Health.CurrentHealth /
                                       PlayersController.Instance.CurrentPlayer.Health.MaxHealth;
    }

    public void OnPlayerMove(Transform target, Vector3 direction)
    {
        _healthBar.transform.localPosition = CanvasHelper.Instance.WorldToCanvasPoint(target.position);
        _healthBar.transform.SetLocalPosition(null, 250);
    }
}
