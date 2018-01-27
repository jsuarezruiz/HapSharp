﻿using System;
using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public abstract class MessageLightDelegate : MessageDelegate
	{
		const string TopicSetOn = "set/on";
		const string TopicGetOn = "get/on";

		public MessageLightDelegate (LightAccessory accessory) : base (accessory)
		{

		}

		protected override void OnMessageReceived (string topic, string message)
		{
			if (message.StartsWith (TopicSetOn + "/")) {
				OnChangePower (message.Substring ((TopicSetOn + "/").Length).ToBoolean ());
			} else if (message == TopicGetOn) {
				var current = OnGetPower () ? "true" : "false";
				OnSendMessage (topic, message, current);
			} else {
				throw new NotImplementedException (message);
			}
		}

		public override string OnReplaceTemplate (string prefix, string template)
		{
			return base.OnReplaceTemplate (prefix, template)
				       .Replace (TemplateHelper.GetTemplateTagId (prefix, nameof (TopicGetOn)), TopicGetOn)
				       .Replace (TemplateHelper.GetTemplateTagId (prefix, nameof (TopicSetOn)), TopicSetOn);
		}

		protected abstract bool OnGetPower ();

		protected abstract void OnChangePower (bool value);

	}
}
