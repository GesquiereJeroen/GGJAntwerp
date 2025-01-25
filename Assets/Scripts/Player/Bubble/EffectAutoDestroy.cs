using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoDestroy : MonoBehaviour
{
	#region Editor Fields
	[SerializeField] private float _timer = 0.5f;
    #endregion

    #region Fields
    #endregion

    #region Properties
    #endregion

    #region Events
    #endregion

    #region Mono
    void Awake()
    {
		Destroy(gameObject, _timer);
    }
    #endregion

    #region Methods
    #endregion

    #region EventHandlers
    #endregion
}
