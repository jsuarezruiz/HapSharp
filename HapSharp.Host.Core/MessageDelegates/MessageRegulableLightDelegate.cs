﻿using HapSharp.Accessories;

namespace HapSharp.MessageDelegates
{
	public abstract class MessageRegulableLightDelegate : MessageLightDelegate
	{
		const string TopicSetBrightness = "set/brightness";
		const string TopicGetBrightness = "get/brightness";

		public MessageRegulableLightDelegate (RegulableLightAccessory accessory) : base (accessory)
		{

		}

		protected override void OnMessageReceived (string topic, string message)
		{
			if (message.StartsWith (TopicSetBrightness + "/")) {
				OnChangeBrightness (message.Substring ((TopicSetBrightness + "/").Length).ToInt ());
			} else if (message == TopicGetBrightness) {
				var current = OnGetBrightness ();
				OnSendMessage (topic, message, current);
			} else {
				base.OnMessageReceived (topic, message);
			}
		}

		public override string OnReplaceTemplate (string prefix, string template)
		{
			return base.OnReplaceTemplate (prefix, template)
				       .Replace (TemplateHelper.GetTemplateTagId (prefix, nameof (TopicGetBrightness)), TopicGetBrightness)
				       .Replace (TemplateHelper.GetTemplateTagId (prefix, nameof (TopicSetBrightness)), TopicSetBrightness);
		}

		protected abstract int OnGetBrightness ();
		protected abstract void OnChangeBrightness (int value);
	}
}
