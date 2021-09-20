using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Serialization;

public static class JsonUtils
{
	private static readonly List<Type> ignoreTypes = new List<Type>{
		typeof(AudioClip), typeof(Sprite)
	};

	public static void ErrorHandler(object sender, ErrorEventArgs e)
	{
		ErrorContext ctx = e.ErrorContext;
		ctx.Handled = true;
		Type curObjType = e.CurrentObject.GetType();

		// Silently ignore errors for selected types
		if (ignoreTypes.Contains(curObjType))
			return;

		if (e.CurrentObject is null)
			Debug.LogError($"Error serializing, Path={ctx.Path}\n{ctx.Error}");
		else
			Debug.LogError($"Error serializing {curObjType.Name}, Path={ctx.Path}\n{ctx.Error}");
	}
}
