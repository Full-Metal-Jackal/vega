﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.CircuitConstructor
{
	public abstract class CircuitWidgetBase : MonoBehaviour
	{
		public bool Initialized { get; protected set; } = false;

		// <TODO> ideally, will be removed in the future; we will use Setup(GameObject circuitPrefab) instead.
		[SerializeField]
		private GameObject startCircuitPrefab;
		public GameObject CircuitPrefab { get; protected set; }

		public RectTransform RectTransform { get; private set; }

		public CircuitContainer Circuit { get; private set; }

		[field: SerializeField]
		public virtual RectTransform CircuitHolder { get; private set; }

		private void Awake()
		{
			Initialize();
		}

		protected virtual bool Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return false;
			}

			RectTransform = GetComponent<RectTransform>();

			return Initialized = true;
		}

		private void Start()
		{
			Setup();
		}

		protected virtual void Setup()
		{
			if (startCircuitPrefab)
				Setup(startCircuitPrefab);
		}

		public virtual void Setup(GameObject circuitPrefab)
		{
			CircuitPrefab = circuitPrefab;

			GameObject circuitObject = Instantiate(circuitPrefab);
			if (!circuitObject.TryGetComponent(out CircuitContainer circuit))
				throw new System.Exception($"Attempted to create invalid visualization for {this}");
			
			circuitObject.transform.SetParent(CircuitHolder, false);

			Circuit = circuit;
		}

		public override string ToString() => $"{Circuit.Circuit}'s widget";
	}
}
