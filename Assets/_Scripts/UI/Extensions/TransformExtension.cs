using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Vadimskyi.Utils
{
    public static class TransformExtension
    {
        public static void SetLocalPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            Vector3 pos = transform.localPosition;
            if (x.HasValue) pos.x = x.Value;
            if (y.HasValue) pos.y = y.Value;
            if (z.HasValue) pos.z = z.Value;
            transform.localPosition = pos;
        }
    }
}