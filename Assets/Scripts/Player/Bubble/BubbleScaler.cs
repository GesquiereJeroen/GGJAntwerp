using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScaler : MonoBehaviour
{
	#region Editor Fields
	[SerializeField] private float _growTime = 0.5f;
	#endregion

    #region Fields
    #endregion

    #region Properties
    #endregion

    #region Events
    #endregion

    #region Mono
    private void Awake()
    {
		// setsize to 0
		transform.DOScale(0, 0);
		// grow the bubble over the grow time
		transform.DOScale(1, _growTime);
    }
    #endregion

    #region Methods
    #endregion

    #region EventHandlers
    #endregion
}
