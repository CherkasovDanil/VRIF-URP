using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRIF_URP.Pipes;
using VRIF_URP.Player;
using VRIF_URP.SceneObject;
using Zenject;

namespace VRIF_URP.Game
{
    public class GameController : ITickable
    {
        private readonly PlayerInputController _playerInputController;
        private readonly PipeService _pipeService;

        private PlayerView _playerView;

        public GameController(
            TickableManager tickableManager,
            PlayerInputController playerInputController,
            PipeService pipeService,
            SceneHolder sceneHolder)
        {
            _playerInputController = playerInputController;
            _pipeService = pipeService;
            _playerView = sceneHolder.Get<PlayerView>();
            

            tickableManager.Add(this);
        }
        
        public void Tick()
        {
            if (_playerInputController.GetControllerAButton())
            {
                CheckGameWin();
            }
        }

        private void CheckGameWin()
        {
            if (_pipeService.GetEmptyPlace().Count == 0)
            {
                _playerView.ScreenFade.fadeColor = Color.black;
                _playerView.ScreenFade.FadeOut();
                DOVirtual.DelayedCall(4f, () =>
                {
                    SceneManager.LoadScene(0);
                });
               
            }
            else
            {
                _playerView.ScreenFade.fadeColor = Color.red;
                _playerView.ScreenFade.FadeOut();
                DOVirtual.DelayedCall(2f, () =>
                {
                    _playerView.ScreenFade.FadeIn();
                });
            }
        }
    }
}