using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vadimskyi.Utils;
using UniRx;

namespace Vadimskyi.Game
{
    public class HealthBarFactory : IFactory<HealthBarFacade, User, Transform>
    {
        private MonoObjectPool<HealthBarView> _pool;

        public HealthBarFactory()
        {
            _pool = new MonoObjectPool<HealthBarView>(Services.Get<GameSettings>().DefaultGuiSettings.HealthBarPrefab);
        }

        public HealthBarFacade Create(User model, Transform baseTransform)
        {
            var view = _pool.Rent();
            view.transform.SetParent(baseTransform, false);
            var result = new HealthBarFacade(view, model);
            result.OnDispose?.Subscribe(OnHealthDisposed);
            return result;
        }

        private void OnHealthDisposed(HealthBarFacade bar)
        {
            _pool.Return(bar.View);
        }

        public void Dispose()
        {
            
        }
    }
}