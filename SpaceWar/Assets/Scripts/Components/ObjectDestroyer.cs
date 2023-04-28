using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class ObjectDestroyer : MonoBehaviour
    {
        public void DestroyObject() => Destroy(gameObject);
    }
}
