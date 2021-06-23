using UnityEngine;

using Circuitry;
using System.Collections;
using System.Collections.Generic;

namespace UI.CircuitConstructor
{
	public static class EventHandler
	{
		public static bool echoToConsole = false;

		public static Dictionary<Pin, PinWidget> pinBounds = new Dictionary<Pin, PinWidget>();
		public static Dictionary<Circuitry.Circuit, CircuitWidget> circuitBounds = new Dictionary<Circuitry.Circuit, CircuitWidget>();
		public static Dictionary<Assembly, AssemblyWidget> assemblyBounds = new Dictionary<Assembly, AssemblyWidget>();

		public static void Log(string text)
		{
			if (echoToConsole)
				Debug.Log($"<color=lime>[{Time.frameCount}] {text}</color>");
		}

		private static void Bind<CallerType, WidgetType>(CallerType caller, WidgetType widget, Dictionary<CallerType, WidgetType> dictionary)
		{
			if (dictionary.ContainsKey(caller))
				return;

			dictionary.Add(caller, widget);
		}

		public static void Bind(Pin pin, PinWidget widget) => Bind(pin, widget, pinBounds);
		public static void Bind(PinWidget widget) => Bind(widget.BoundPin, widget);
		public static void Bind(Circuitry.Circuit circuit, CircuitWidget widget) => Bind(circuit, widget, circuitBounds);
		public static void Bind(CircuitWidget widget) => Bind(widget.Circuit.BoundCircuit, widget);
		public static void Bind(Assembly assembly, AssemblyWidget widget) => Bind(assembly, widget, assemblyBounds);
		public static void Bind(AssemblyWidget widget) => Bind(widget.BoundAssembly, widget);

		private static bool Trigger<CallerType, WidgetType>(CallerType caller, Dictionary<CallerType, WidgetType> dictionary, string eventLabel) where WidgetType : ITriggerable<CallerType>
		{
			if (!(Game.CircuitConstructor && Game.CircuitConstructor.IsOpened))
				return false;

			if (!dictionary.ContainsKey(caller))
				return false;

			return dictionary[caller].Trigger(caller, eventLabel);
		}

		public static void Trigger(Pin pin, string eventLabel = "") => Trigger(pin, pinBounds, eventLabel);
		public static void Trigger(Circuitry.Circuit circuit, string eventLabel = "") => Trigger(circuit, circuitBounds, eventLabel);
		public static void Trigger(Assembly assembly, string eventLabel = "") => Trigger(assembly, assemblyBounds, eventLabel);
	}
}
