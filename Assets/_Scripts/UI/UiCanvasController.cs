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
    private HealthBarView _healthBarPrefab;

    private HealthBarFactory _factory;
    private Dictionary<int, HealthBarFacade> _userHealthbars;

    private void Start()
    {
        _factory = new HealthBarFactory();
        _userHealthbars = new Dictionary<int, HealthBarFacade>();
        _playerRespawn.OnClickAsObservable().Subscribe(_ =>
        {
            PlayersController.Instance.RespawnPlayer();
        }).AddTo(this);
        Initialize();
    }

    private void Initialize()
    {
        CombatRoomData.Instance.Users?.ForEach(CreateUserHealthbar);
        CombatRoomData.Instance.OnNewUserJoined += CreateUserHealthbar;
    }

    private void CreateUserHealthbar(User user)
    {
        var healthFacade = _factory.Create(user, transform);
        _userHealthbars.Add(user.Id, healthFacade);
        user.Character.Position.Subscribe(_ =>
        {
            UpdatePlayersPosition();
            Observable.TimerFrame(2, FrameCountType.FixedUpdate).Subscribe(_2 => { UpdatePlayersPosition(); });
        });
        UpdatePlayersPosition();
    }

    //line up healthbar with player position
    private void UpdatePlayersPosition()
    {
        foreach (var barFacade in _userHealthbars.Values)
        {
            barFacade.UpdatePosition(CanvasHelper.Instance.WorldToCanvasPoint(barFacade.User.Character.Position.Value));
        }
    }

    public void TryCharacterKick()
    {
        PlayersController.Instance.CurrentPlayer?.Kick?.TryKick();
    }

    public void TryCharacterDash()
    {
        var player = PlayersController.Instance.CurrentPlayer;
        var dash = player.Dash;
        dash.TryDash();
        Debug.Log("Dash!");
    }
}
